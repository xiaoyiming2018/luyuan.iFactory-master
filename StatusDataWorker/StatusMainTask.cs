using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Advantech.IFactory.CommonHelper.ScadaAPI;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class StatusMainTask
    {
        private static MachineInfoManager machineInfoManager = new MachineInfoManager();
        private static StationManager stationManager = new StationManager();
        private static LineInfoManager lineInfoManager = new LineInfoManager();
        private static TagService tagService = new TagService();
        private static DateTime lastDatetime = DateTime.Now;
        private static int updateCycleSec = 3000;//默认更新时间
        private static TricolorTagDurationManager tricolorTagDurationManager = new TricolorTagDurationManager();
        private static MongoDBHelper mongoDBHelper = new MongoDBHelper();
        private static TagValueLogManager tagValueLogManager = new TagValueLogManager();
        private static SRPInnerLogManager sRPInnerLogManager = new SRPInnerLogManager();
        private static UtilizationRateHelper utilizationRateHelper = new UtilizationRateHelper();
        private static TricolorTagLogManager triColorTagLogManager = new TricolorTagLogManager();
        private static NetworkManager networkManager = new NetworkManager(mongoDBHelper);

        private static readonly string SrpCode = "MachineStatus";
        private static Thread thread;
        private static TagDurationManager tagDurationManager = new TagDurationManager();
        private static SRPLogManager srpLogManager = new SRPLogManager();
        private static TagAreaAttributeEnum tagAreaAttributeMode = TagAreaAttributeEnum.station_info;//最小站位模式
        
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="UpdateCycleSec">扫描周期</param>
        /// <returns></returns>
        public static bool Start_Task(int UpdateCycleSec = 10000)
        {
            try
            {
                updateCycleSec = UpdateCycleSec;
                string times = sRPInnerLogManager.GetLastTimeByCode(SrpCode);
                lastDatetime = Convert.ToDateTime(times);     //获取上一次的时间记录
                Console.WriteLine("StatusMainTask读取到上一次的时间值为：" + times);
                networkManager.DeviceNetWorkBreakEvent += StatusHandle;

                Thread thread1 = new Thread(() => StatusCycle());
                thread1.Start();
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
                    networkManager.CheckAllDevice();//检查节点网络连接状态
                    TagValueWatch(tagAreaAttributeMode);//标签值变化监测
                    ++count;
                    if (count >= 6)//标签值处理完成再处理稼动率计算    
                    {
                        utilizationRateHelper.CalUtilizationRate();//
                        if (ScadaAPIConfig.EnableScadaApi)
                        {
                            networkManager.CheckAllDevice();//检查节点网络连接状态
                        }
                            
                        count = 0;
                    }
                    ++deleteCount;
                    if (deleteCount>=100)//定期删除记录
                    {
                        datetimeNow = DateTime.Now.AddDays(-1 * GlobalDefine.LogTableKeepDays);//删除中间记录
                        triColorTagLogManager.DeleteByTime(datetimeNow);
                        tagValueLogManager.DeleteByTime(datetimeNow);

                        datetimeNow = DateTime.Now.AddDays(-1 * GlobalDefine.MiddleTableKeepDays);//删除中间记录
                        tricolorTagDurationManager.DeleteByTime(datetimeNow);
                        deleteCount = 0;

                        datetimeNow = DateTime.Now.AddDays(-1 * GlobalDefine.MongoDBLogKeepDays);//mongodb数据删除
                        var filterBuilder = Builders<MongoDbTag>.Filter;
                        var filter = filterBuilder.Lte(x => x.ts, datetimeNow);
                        count = mongoDBHelper.DeleteMany("scada_HistRawData", filter);
                    }
                }
                catch(Exception ex)
                {
                    srpLogManager.Insert("StatusMainTask error=" + ex.Message);
                }
                Thread.Sleep(updateCycleSec);
            }
        }
       
        /// <summary>
        /// Tag标签值变化监测
        /// </summary>
        private static void TagValueWatch(TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            try
            {
                DateTime dateTimeNow = DateTime.Now;
               
                List<DeviceTagValueInfo> vList = new List<DeviceTagValueInfo>();
                string subject = string.Empty, content = string.Empty;
                int value = 0;
                List<MongoDbTag> Tags = new List<MongoDbTag>();

                if (ScadaAPIConfig.EnableScadaApi==false)//本地版本
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
                vList = vList.Where(x => x.system_type_code == TagTypeEnum.UtilizationRate.ToString()).OrderBy(x=>x.insert_time).ToList();//筛选稼动率类别
                foreach (var valueItem in vList)
                {
                    int.TryParse(valueItem.tag_value, out value);
                    if (value == 1)
                    {
                        StatusHandle(valueItem);

                        //日志记录
                        tag_value_log tagLog = new tag_value_log();
                        tagLog.insert_time = valueItem.insert_time.AddHours(GlobalDefine.SysTimeZone); ;
                        tagLog.srp_code = SrpCode;
                        tagLog.system_tag_code = valueItem.system_tag_code;
                        tagLog.tag_value = valueItem.tag_value;
                        tagLog.tag_code = valueItem.tag_code;
                        tagValueLogManager.Insert(tagLog);
                    }
                }

                sRPInnerLogManager.UpdateSrpTimeByCode(SrpCode, lastDatetime);//更新时间
            }
            catch (Exception ex)
            {
                srpLogManager.Insert("TagValueWatch error=" + ex.Message);
            }
        }

        /// <summary>
        /// 状态判断与处理
        /// </summary>
        /// <param name="machineTagValueItem">设备Tag值</param>
        private static void StatusHandle(DeviceTagValueInfo deviceTagValueItem)
        {
            string tag_code;
            int retCount = 0;
            DateTime insert_time = deviceTagValueItem.insert_time.AddHours(GlobalDefine.SysTimeZone);
            bool status = true;
            bool whether = false;
            MachineInfo machine=null;

            try
            {
                if (tagAreaAttributeMode == TagAreaAttributeEnum.machine_info)
                {
                    //判断当前机台状态
                    machine = machineInfoManager.SelectSingleById(deviceTagValueItem.device_id);
                }
                else if (tagAreaAttributeMode == TagAreaAttributeEnum.station_info)
                {
                    List<MachineInfo> machines = machineInfoManager.SelectMachineNameByStaion(deviceTagValueItem.device_id);
                    if(machines.Count>0)
                    {
                        machine = machines[0];//取站位下第一台设备
                    }
                }
                
                if (machine != null)
                {
                    //如果机台属于停止，那将机台设置为开启(10-15修改为不开启状态不做处理)
                    if (machine.status_no.Trim() != "S")
                    {
                        //插入triclor_tag_log原始记录表，状态未转换
                        retCount=tagDurationManager.InsertTriColorTagLog(deviceTagValueItem.device_code, deviceTagValueItem.device_id,
                                                                         deviceTagValueItem.system_tag_code, insert_time,
                                                                         tagAreaAttributeMode);
                        if(retCount==0)
                        {
                            srpLogManager.Insert("tagDurationManager.InsertTriColorTagLog执行失败"+ deviceTagValueItem.device_code+" "+ deviceTagValueItem.system_tag_code+" "+ insert_time);
                        }
                        //获取线别信息
                        line_info objLine = lineInfoManager.SelectSingle(machine.line_id);
                        if (objLine != null && objLine.status_no == "S")
                        {
                            //更新线别为开启状态(10-15修改为不开启状态不做处理)
                            //LIM.UpdateStatus(objMachine.line_id, "A");
                            srpLogManager.Insert("错误，线别为空，线别状态为S" + deviceTagValueItem.device_code + " " + deviceTagValueItem.system_tag_code);
                        }
                        else
                        {
                            //根据休息进行系统标签转换
                            tag_code = MachineBreakManager.MachineBreakCheck(machine, deviceTagValueItem.device_code, insert_time, deviceTagValueItem.system_tag_code.Trim());
                            if (string.IsNullOrEmpty(tag_code)==false && tag_code != deviceTagValueItem.system_tag_code.Trim())
                            {
                                whether = true;
                            }

                            //插入转换过后的及时状态持续表
                            int count = tagDurationManager.InsertTriColorDurationTable(deviceTagValueItem.device_code, deviceTagValueItem.device_id,
                                                                                       tag_code, whether, status, insert_time, false,
                                                                                       tagAreaAttributeMode);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("StatusHandle error=" + ex.Message);
            }
        }

    }
}
