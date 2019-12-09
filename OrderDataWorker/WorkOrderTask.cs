using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using MongoDB.Driver;

namespace Advantech.IFactory.WorkOrderManage
{
    public class WorkOrderTask
    {
        private static ProScheduleManager proScheduleManager = new ProScheduleManager();
        private static ProSchedulemachineLogManager proSchedulemachineLogManager = new ProSchedulemachineLogManager();
        private static MachineInfoManager machineInfoManager = new MachineInfoManager();
        private static TagService tagService = new TagService();
        private static DateTime lastDatetime = DateTime.Now;
        private static MongoDBHelper mongoDBHelper = new MongoDBHelper();
        private static CTLogManager ctLogManager = new CTLogManager();
        private static DeviceProScheduleHelper deviceProScheduleHelper = new DeviceProScheduleHelper();
        private static CTManager ctManager = new CTManager();

        private static int updateOrderSec = 1000;
        private static LineBalanceRateHelper lineBalanceRateHelper = new LineBalanceRateHelper();
        private static TagAreaAttributeEnum tagAreaAttributeMode = TagAreaAttributeEnum.station_info;//最小站位模式
        private static SRPInnerLogManager sRPInnerLogManager = new SRPInnerLogManager();
        private static SRPLogManager srpLogManager = new SRPLogManager();

        private static readonly string SrpCode = "WorkOrder";
       
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="UpdateOrderSec">扫描周期</param>
        /// <returns></returns>
        public static bool Start_Task(int UpdateOrderSec = 10000)
        {
            try
            {
                updateOrderSec = UpdateOrderSec;
                string times = sRPInnerLogManager.GetLastTimeByCode(SrpCode);
                lastDatetime = Convert.ToDateTime(times);     //获取上一次的时间记录
                Console.WriteLine("WorkOrderTask读取到上一次的时间值为：" + times);

                CTManager.CtValueChangedEvent += WorkPieceCount.CtNumChangedEvent;
                Thread thread = new Thread(() => StatusCycle());
                thread.Start();

                //两个不在一块，只能通过循环去判断
                Thread thread2 = new Thread(() => CheckExcutingOrders());
                thread2.Start();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        
        private static void StatusCycle()
        {
            int count = 0;
            int deleteCount = 0;

            DateTime datetimeNow;

            while (true)
            {
                try
                {
                    datetimeNow = DateTime.Now.AddHours(DataWorkerCfg.time_zone);
                    //当监测数据库中标签值变化，记录至SQL，并缓存在本地队列
                    TagValueWatch(TagAreaAttributeEnum.station_info);//标签值变化监测
                    ++count;
                    ++deleteCount;
                    if (count >= 50)//5分钟计算一次线平衡率
                    {
                        foreach (var line in DataWorkerCfg.LinesList)
                        {
                            List<pro_schedule> list = proScheduleManager.SelectSchedules(line.unit_no, line.line_id, (int)OrderStatusEnum.Excuting);
                            if (list != null && list.Count>0)
                            {
                                foreach (var schdule in list)
                                {
                                    //计算线平衡率
                                    MachineInfo machine = DataWorkerCfg.MachinesList.FirstOrDefault(x => x.line_id == line.line_id);
                                    if (machine != null)
                                    {
                                        lineBalanceRateHelper.InsertLineBalanceRate(machine.area_id, machine.city_id,
                                                                                    line.plant_id, line.unit_no,
                                                                                    line.line_id, schdule.part_num, schdule.work_order,
                                                                                    datetimeNow, tagAreaAttributeMode);
                                    }
                                }
                            }
                        }
                        
                        count = 0;
                    }

                    if(deleteCount>=100)//定期删除记录
                    {
                        datetimeNow = DateTime.Now.AddDays(-1 * GlobalDefine.LogTableKeepDays);//删除中间记录
                        ctLogManager.DeleteByTime(datetimeNow);
                        proSchedulemachineLogManager.DelByTime(datetimeNow);
                        
                        datetimeNow = DateTime.Now.AddDays(-1 * GlobalDefine.MiddleTableKeepDays);//删除中间记录
                        ctManager.DelByTime(datetimeNow);            //ct
                        lineBalanceRateHelper.DelByTime(datetimeNow);//线平衡率
                        deleteCount = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("WorkOrderTask error="+ex.Message);
                    srpLogManager.Insert("WorkOrderTask error=" + ex.Message);
                }
                Thread.Sleep(updateOrderSec);
            }
        }
        /// <summary>
        /// 工单检查
        /// </summary>
        private static void CheckExcutingOrders()
        {
            while (true)
            {
                DeviceOrdersCheck.CheckOrders();
                Thread.Sleep(15000);//15秒检查一次
            }
        }

        /// <summary>
        /// Tag标签值变化监测
        /// </summary>
        private static void TagValueWatch(TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            DateTime dateTimeNow = DateTime.Now;
            List<DeviceTagValueInfo> vList = new List<DeviceTagValueInfo>();
            //获取站位是否开启
            MachineInfo machine = null;
            string subject = string.Empty, content = string.Empty;
            string machineCode;
            int stationId;
            int value = 0;
            List<MongoDbTag> Tags = new List<MongoDbTag>();

            try
            {
                if (GlobalDefine.IsLocalMode)//本地版本
                {
                    var filterBuilder1 = Builders<LocalMongoDbTag>.Filter;
                    var filter1 = filterBuilder1.And(filterBuilder1.Gt(x => x.ts, lastDatetime),
                                                     filterBuilder1.Lte(x => x.ts, dateTimeNow),
                                                     filterBuilder1.Ne(x => x.v, "*"),
                                                     filterBuilder1.Ne(x => x.v, ""));
                    var localTags = mongoDBHelper.GetList<LocalMongoDbTag>("HistoryRawDatas", filter1).OrderBy(x => x.ts).ToList();

                    if (localTags == null || localTags.Count == 0)
                    {
                        return;
                    }
                    //Console.WriteLine("success find MongoDb list=" + localTags.Count.ToString() +",time="+ DateTime.Now.ToLongTimeString());
                    foreach (var item in localTags)//分项合并
                    {
                        MongoDbTag tag = new MongoDbTag();
                        tag.ID = item.ID;
                        tag.t = string.Format("{0}:{1}:{2}", item.s, item.d, item.t);
                        tag.v = item.v;
                        tag.ts = item.ts;
                        Tags.Add(tag);
                    }
                    if (localTags.Count > 0)
                    {
                        lastDatetime = localTags[localTags.Count - 1].ts; //缓存上一次的时间，以最后一笔记录的时间为准
                    }
                }
                else
                {
                    var filterBuilder = Builders<MongoDbTag>.Filter;
                    var filter = filterBuilder.And(filterBuilder.Gt(x => x.ts, lastDatetime),
                                                   filterBuilder.Lte(x => x.ts, dateTimeNow),
                                                   filterBuilder.Ne(x => x.v, "*"),
                                                   filterBuilder.Ne(x => x.v, ""));
                    Tags = mongoDBHelper.GetList<MongoDbTag>("scada_HistRawData", filter).OrderBy(x => x.ts).ToList();
                    //Console.WriteLine("---------------start to scan MongoDb,time span ="+ lastDatetime.ToString()+" --"+dateTimeNow.ToString());

                    if (Tags == null || Tags.Count == 0)
                    {
                        return;
                    }
                    Console.WriteLine("success find MongoDb list=" + Tags.Count.ToString() + ",time=" + DateTime.Now.ToLongTimeString());
                    if (Tags.Count > 0)
                    {
                        lastDatetime = Tags[Tags.Count - 1].ts; //缓存上一次的时间，以最后一笔记录的时间为准
                    }
                }

                vList = tagService.GetDeviceTagValueInfo(Tags, tagAreaAttributeEnum);
                vList = vList.Where(x => x.system_type_code == TagTypeEnum.ProductionSchedule.ToString() || 
                                         x.system_type_code == TagTypeEnum.CircleTime.ToString()).OrderBy(x => x.insert_time).ToList();//在制进度与节拍时间类型
                foreach (var valueItem in vList)
                {
                    if (tagAreaAttributeMode == TagAreaAttributeEnum.station_info)
                    {
                        machineCode = valueItem.device_code;
                        stationId = valueItem.device_id;
                    }
                    else
                    {
                        machineCode = "";
                        stationId = valueItem.device_id;
                    }

                    if (valueItem.system_tag_code == SystemTagCodeEnum.work_order.ToString() ||
                        valueItem.system_tag_code == SystemTagCodeEnum.part_number.ToString())     //工单
                    {
                        //获取站位是否开启
                        machine = machineInfoManager.SelectSingle(valueItem.device_id, null);
                        if (machine != null && machine.status_no == "A")
                        {
                            //插入在线工单
                            deviceProScheduleHelper.ParsTagValueToSchedule(valueItem.device_code, stationId, valueItem.tag_value, valueItem.insert_time.AddHours(DataWorkerCfg.time_zone));
                        }
                    }

                    else if (valueItem.system_tag_code == SystemTagCodeEnum.cycle_time.ToString() ||
                             valueItem.system_tag_code == SystemTagCodeEnum.staff_time.ToString() ||
                             valueItem.system_tag_code == SystemTagCodeEnum.machine_time.ToString())           //ct
                    {
                        if (tagAreaAttributeMode == TagAreaAttributeEnum.machine_info)
                        {
                            machine = DataWorkerCfg.MachinesList.FirstOrDefault(x => x.machine_id == valueItem.device_id);
                        }
                        else if (tagAreaAttributeMode == TagAreaAttributeEnum.station_info)
                        {
                            List<MachineInfo> machines = DataWorkerCfg.MachinesList.Where(x => x.station_id == valueItem.device_id).ToList();
                            if (machines.Count > 0)
                            {
                                machine = machines[0];//取站位下第一台设备
                            }
                        }

                        if (machine != null)
                        {
                            if (machine.status_no.Trim() == "A")//获取站位是否开启
                            {
                                value = Convert.ToInt32(valueItem.tag_value);

                                //加上时区
                                DateTime insert_time = valueItem.insert_time.AddHours(DataWorkerCfg.time_zone);
                                //CT时间
                                float ctvalue =CTHelper.CalCTValue(valueItem, Convert.ToInt32(value), insert_time, tagAreaAttributeEnum);
                            }
                        }
                    }
                }
                sRPInnerLogManager.UpdateSrpTimeByCode(SrpCode, lastDatetime);//更新时间
            }
            catch (Exception ex)
            {
                Console.WriteLine("WorkOrderTask error=" + ex.Message);
                srpLogManager.Insert("WorkOrderTask TagValueWatch error=" + ex.Message);
            }
        }
       
    }
}



