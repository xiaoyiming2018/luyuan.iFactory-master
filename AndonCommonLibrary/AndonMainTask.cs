using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using MongoDB.Driver;
using System.Threading.Tasks;
using Advantech.IFactory.CommonHelper.ScadaAPI;

namespace Advantech.IFactory.Andon
{
    /// <summary>
    /// 异常事件程序主要处理
    /// </summary>
    public class AndonMainTask
    {
        private static AndonErrorManager andonErrorManager = new AndonErrorManager();
        private static AndonMaterialRequestManager andonMaterialRequestManager = new AndonMaterialRequestManager();
        private static ErrorLogManager errorLogManager = new ErrorLogManager();
        private static LightTowerTagManager lightTowerTagManager=new LightTowerTagManager();
        private static ErrorMessageManager errorMessageManager = new ErrorMessageManager();
        private static MachineInfoManager machineInfoManager = new MachineInfoManager();
        private static StationManager stationManager = new StationManager();
        private static TagService tagService = new TagService();
        private static SRPLogManager srpLogManager = new SRPLogManager();
        private static MongoDBHelper mongoDBHelper = new MongoDBHelper();
        private static ErrorConfigManager errorConfigManager = new ErrorConfigManager();
        private static MachineworkingtimeManager machineworkingtimeManager = new MachineworkingtimeManager();
        private static SRPInnerLogManager srpInnerLogManager = new SRPInnerLogManager();
        private static List<error_config> deviceTimeErrCfgList = new List<error_config>();

        private static Dictionary<int, int> deviceTimeErrDic = new Dictionary<int, int>();//记录设备工时超时队列

        private static DateTime lastDatetime = DateTime.Now.ToUniversalTime();//最后一次更新时间
        private static int updateCycleSec = 2000;//默认更新时间

        private static TagAreaAttributeEnum tagAreaAttributeMode= TagAreaAttributeEnum.station_info;//最小单位模式
        private static string MessageAdditional = Environment.NewLine+" 来自于安灯系统";
        private static readonly string SrpCode = "Andon";
        private static bool TaskRunBit = false;
        private static Thread thread;
        
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="UpdateCycleSec">扫描周期</param>
        /// <returns></returns>
        public static bool Start_Task(int UpdateCycleSec = 2000)
        {
            try
            {
                TaskInitial(tagAreaAttributeMode);//初始化
                updateCycleSec = UpdateCycleSec;
                TaskRunBit = true;
                thread = new Thread(() => ErrorCycle());
                thread.Start();
                Thread thread2 = new Thread(() => TimeoutCheck());
                thread2.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        
        private static void ErrorCycle()
        {
            while (TaskRunBit)
            {
                //当监测数据库中标签值变化，记录至SQL，并缓存在本地队列
                TagValueWatch(tagAreaAttributeMode);//标签值变化监测
                Thread.Sleep(updateCycleSec);
            }
        }
        /// <summary>
        /// 队列超时监测
        /// </summary>
        private static void TimeoutCheck()
        {
            while(true)
            {
                TimeoutWatch(tagAreaAttributeMode);//异常队列超时监测                   
                //lightTowerTagManager.GreenStatusLightOutAsync(ErrorWatchList, tagAreaAttributeMode);// 现场状态灯输出
                Thread.Sleep(60000);
            }
        }
        /// <summary>
        /// 初始化：灯状态输出
        /// </summary>
        public static void TaskInitial(TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            string times = srpInnerLogManager.GetLastTimeByCode(SrpCode);
            lastDatetime = Convert.ToDateTime(times);     //获取上一次的时间记录

            if (tagAreaAttributeEnum==TagAreaAttributeEnum.machine_info)
            {
                List<MachineInfo> machines = machineInfoManager.SelectAll(null);
                foreach (MachineInfo machine in machines)
                {
                    lightTowerTagManager.GreenStatusLightOutAsync(machine.machine_id, tagAreaAttributeEnum);
                }
            }
            else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                List<station_info> stations = stationManager.SelectAll(-1);
                foreach (station_info station in stations)
                {
                    lightTowerTagManager.GreenStatusLightOutAsync(station.station_id, tagAreaAttributeEnum);
                }
            }
            //公式需要优化
            //
            ////读取所有的设备工时异常配置
            //deviceTimeErrCfgList = errorConfigManager.GetErrorConfig(SystemTagCodeEnum.machine_time_error.ToString());
            //if(deviceTimeErrCfgList != null && deviceTimeErrCfgList.Count>0)//存在工时的异常配置
            //{
            //    CTManager.CtValueChangedEvent += CTManager_CtValueChangedEvent;
            //}
        }
        /// <summary>
        /// 工时更新变化事件接收。需要优化！！！！！！！！！！！！！1
        /// </summary>
        /// <param name="e"></param>
        private static void CTManager_CtValueChangedEvent(CT ct)
        {
            machine_working_time mWorking_Time;
            MachineInfo machine=null;
            station_info station = null;
            int deviceId = -1;
            error_config ecfg=null;
            DeviceTagValueInfo deviceTagValueInfo = new DeviceTagValueInfo();
            List<error_config> eCfgList;

            if (tagAreaAttributeMode==TagAreaAttributeEnum.machine_info)
            {
                machine = machineInfoManager.SelectSingle(-1, ct.machine_code);
            }
            else
            {
                station = stationManager.SelectSingle(ct.station_id);
            }
            if(machine !=null || station !=null)
            {
                if (machine != null)
                {
                    deviceId = machine.machine_id;
                    mWorking_Time = machineworkingtimeManager.SelectSingle(deviceId);
                    eCfgList = errorConfigManager.GetErrorConfig(SystemTagCodeEnum.machine_time_error.ToString(), machine.unit_no, machine.line_id);//先按照最小单位查询
                    ////ecfg = deviceTimeErrCfgList.FirstOrDefault(x => x.unit_no == machine.unit_no && x.line_id == machine.line_id);
                    //if (ecfg == null)
                    //{
                    //    ecfg = deviceTimeErrCfgList.FirstOrDefault(x => x.unit_no == station.unit_no);//按照制程查找
                    //}
                    if (eCfgList == null || eCfgList.Count == 0)
                    {
                        eCfgList = errorConfigManager.GetErrorConfig(SystemTagCodeEnum.machine_time_error.ToString(), machine.unit_no);//再按照大一级别查询
                    }
                    if (eCfgList !=null && eCfgList.Count>0)
                    {
                        ecfg = eCfgList[0];
                    }
                    deviceTagValueInfo.device_code = ct.machine_code; 
                }
                else
                {
                    deviceId = station.station_id;
                    mWorking_Time = machineworkingtimeManager.SelectSingle(-1, deviceId);
                    //ecfg = deviceTimeErrCfgList.FirstOrDefault(x => x.unit_no == station.unit_no && x.line_id == station.line_id);//先按照制程+线别查找
                    //if(ecfg==null)
                    //{
                    //    ecfg = deviceTimeErrCfgList.FirstOrDefault(x => x.unit_no == station.unit_no);//按照制程查找
                    //}
                    eCfgList = errorConfigManager.GetErrorConfig(SystemTagCodeEnum.machine_time_error.ToString(), station.unit_no, station.line_id);//先按照最小单位查询
                    if(eCfgList==null || eCfgList.Count == 0)
                    {
                        eCfgList = errorConfigManager.GetErrorConfig(SystemTagCodeEnum.machine_time_error.ToString(), station.unit_no);//再按照大一级别查询
                    }
                    if (eCfgList != null && eCfgList.Count > 0)
                    {
                        ecfg = eCfgList[0];
                    }
                    deviceTagValueInfo.device_code = station.station_name_en;
                }

                if (ct.value > mWorking_Time.standard_time)//大于标准设定时间
                {
                    try
                    {
                        if (ct.pn != string.Empty &&
                            mWorking_Time.part_num != string.Empty &&
                            mWorking_Time.part_num.Contains(ct.pn))//机种不为空，在设定的队列里面
                        {
                            if (deviceTimeErrDic.ContainsKey(deviceId))//已经发生过
                            {
                                deviceTimeErrDic[deviceId] += 1;
                            }
                            else
                            {
                                deviceTimeErrDic.Add(deviceId, 1);   //新增到临时
                            }

                            if (ecfg != null &&
                                ecfg.error_active == 1 &&
                                ecfg.trigger_count <= deviceTimeErrDic[deviceId])//激活，且超过了设定的次数
                            {
                                deviceTagValueInfo.device_id = deviceId;
                                deviceTagValueInfo.system_tag_code = SystemTagCodeEnum.machine_time_error.ToString();
                                deviceTagValueInfo.system_type_code = TagTypeEnum.Error.ToString();
                                deviceTagValueInfo.tag_value = ct.value.ToString();
                                deviceTagValueInfo.insert_time = ct.end_time;
                                ErrorHandle(deviceTagValueInfo, tagAreaAttributeMode);//工时异常处理
                                deviceTimeErrDic.Remove(deviceId);//移除
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("CTManager_CtValueChangedEvent error:" + ex.Message);
                        srpLogManager.Insert("CTManager_CtValueChangedEvent error:" + ex.Message);
                    }
                }
            }
        }
        /// <summary>
        /// Tag标签值变化监测
        /// </summary>
        private static void TagValueWatch(TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            DateTime dateTimeNow = DateTime.Now;
            //Console.WriteLine("---------------start to scan MongoDb,time span =" + lastDatetime.ToString() + " --" + dateTimeNow.ToString()+",Dt now="+ DateTime.Now.ToLongTimeString());
            List<DeviceTagValueInfo> vList = new List<DeviceTagValueInfo>();
            List<error_config_person> eCfgPersons = new List<error_config_person>();
            string subject = string.Empty, content = string.Empty;
            int value = 0;
            string machineCode;
            int stationId;
            string mErrCode = string.Empty;
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
                //Console.WriteLine("AndonMainTask success find MongoDb list=" + Tags.Count.ToString() + ",time=" + DateTime.Now.ToLongTimeString());
                if (Tags.Count > 0)
                {
                    lastDatetime = Tags[Tags.Count - 1].ts; //缓存上一次的时间，以最后一笔记录的时间为准
                }
            }

            Console.WriteLine("Mongodb combine finished,begin to find with postgre,time is "+ DateTime.Now.ToLongTimeString()+ " Tags count=" + Tags.Count.ToString());
            try
            {
                vList = tagService.GetDeviceTagValueInfo(Tags, tagAreaAttributeEnum);
                vList = vList.OrderBy(x => x.insert_time).ToList();
                foreach (var valueItem in vList)
                {
                    if (valueItem.tag_value == "" || valueItem.tag_value == "*") continue;
                    if (tagAreaAttributeMode == TagAreaAttributeEnum.machine_info)
                    {
                        machineCode = valueItem.device_code;
                        stationId = -1;
                    }
                    else
                    {
                        machineCode = "";
                        stationId = valueItem.device_id;
                    }
                    if (valueItem.system_type_code == TagTypeEnum.Error.ToString())//属于异常类型
                    {
                        if (valueItem.system_tag_code == SystemTagCodeEnum.material_require.ToString())//物料呼叫信息标签
                        {
                            int requireCount = 0, materialId = 0;
                            //支持两种格式：数量，数量&物料id
                            TagIdAndValue valueObject = tagService.GetTagValue(valueItem.tag_value);
                            if (valueObject.id > 0 && valueObject.value.Length > 0)
                            {
                                requireCount = valueObject.id;
                                int.TryParse(valueObject.value, out materialId);//id为物料的id
                                                                                //andonMaterialRequestManager.AddMaterialRequest(materialId, requireCount, valueItem, tagAreaAttributeMode);//增加
                                                                                //传递的有物料数量和id，在异常里记录，监测时间                                                                                                         //
                                ErrorHandle(valueItem, tagAreaAttributeEnum, "", materialId);
                            }
                            else if (valueObject.id < 0 && valueObject.value.Length > 0)
                            {
                                int.TryParse(valueObject.value, out requireCount);
                                if (requireCount > 0)
                                {
                                    ErrorHandle(valueItem, tagAreaAttributeEnum);//只传递的有数量，则记录，此方式为触摸屏HMI方式
                                }
                            }
                        }
                        else if (valueItem.system_tag_code == SystemTagCodeEnum.machine_error_code.ToString())//采集到的异常编码
                        {
                            mErrCode= MachineErrorCodeHelper.MachineErrorCodeHandle(valueItem);//异常编码处理
                            if(mErrCode !=string.Empty)
                            {
                                ErrorHandle(valueItem, tagAreaAttributeMode, mErrCode);
                            }
                        }
                        else//其他品质、设备、环膜等扩展异常
                        {
                            int.TryParse(valueItem.tag_value, out value);//值比较
                            if (value == 1)
                            {
                                ErrorHandle(valueItem, tagAreaAttributeEnum);
                                string msg = "error handle:" + DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToLongTimeString() + valueItem.device_code + " " + valueItem.device_id + " " + valueItem.tag_code + valueItem.system_type_code + " " + valueItem.system_tag_code + " " + valueItem.tag_value;
                                //srpLogManager.Insert(new srp_log(msg, DateTime.Now.AddHours(GlobalDefine.SysTimeZone)));
                                Console.WriteLine(msg);
                            }
                        }
                    }
                    else if (valueItem.system_type_code == TagTypeEnum.Other.ToString())//其他类别
                    {
                        if (valueItem.system_tag_code == SystemTagCodeEnum.andon_ack_code.ToString())//现场确认并提交错误编码sn
                        {
                            //支持两种格式，sn或者id&sn，id为异常记录的唯一标识号
                            HandleErrorAckForTag(machineCode, stationId, valueItem, tagAreaAttributeEnum);
                        }
                        else if (valueItem.system_tag_code == SystemTagCodeEnum.andon_ack_person.ToString())//签到人员的卡号
                        {
                            HandlePersonArrivalForTag(machineCode, stationId, valueItem, tagAreaAttributeEnum);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("Andon tagvalue错误:" + ex.Message);
            }

            try
            {
                //------------------------------------------------------------------------------------------------
                //对设备层的编码特殊处理
                //Console.WriteLine("start to find machine error code!");
                if (tagAreaAttributeEnum != TagAreaAttributeEnum.machine_info)
                {
                    //异常代码特殊处理，因为绑定在设备上面
                    vList = tagService.GetDeviceTagValueInfo(Tags, TagAreaAttributeEnum.machine_info);//异常代码特殊处理
                    vList = vList.Where(x => x.system_type_code == TagTypeEnum.Error.ToString() &&
                                        x.system_tag_code == SystemTagCodeEnum.machine_error_code.ToString()).ToList();
                    foreach (var valueItem in vList)
                    {
                        mErrCode = MachineErrorCodeHelper.MachineErrorCodeHandle(valueItem);//异常编码处理
                        if (mErrCode != string.Empty)
                        {
                            ErrorHandle(valueItem, tagAreaAttributeMode, mErrCode);
                        }
                    }
                }
                //Console.WriteLine("machine error code handle finished!");
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("抓取设备异常信息失败:"+ex.Message);
            }

            srpInnerLogManager.UpdateSrpTimeByCode(SrpCode,lastDatetime);//更新时间
            Tags.Clear();
            vList.Clear();
        }
        
        /// <summary>
        /// 人员签到处理.以Tag点形式上抛的。方案待优化！！！！！！！！！！！！！！！！！！！
        /// </summary>
        /// <param name="machineCode"></param>
        /// <param name="stationId"></param>
        /// <param name="deviceTagValueInfo"></param>
        /// <param name="tagAreaAttributeEnum"></param>
        public static void HandlePersonArrivalForTag(string machineCode,int stationId,
                                                     DeviceTagValueInfo deviceTagValueInfo,
                                                     TagAreaAttributeEnum tagAreaAttributeEnum= TagAreaAttributeEnum.station_info)
        {
            string subject = string.Empty, content = string.Empty;
            ErrorObject errorObject = andonErrorManager.AckErrorLogPerson(machineCode, stationId, deviceTagValueInfo, tagAreaAttributeEnum);
            if(errorObject !=null)
            {
                HandlePersonArrival(null, null, errorObject);
            }
        }
        /// <summary>
        /// 人员签到处理
        /// </summary>
        /// <param name="logItem"></param>
        /// <param name="cfgItem"></param>
        /// <param name="errorObj"></param>
        public static void HandlePersonArrival(error_log logItem, error_config cfgItem, ErrorObject errorObj = null)
        {
            string subject = string.Empty, content = string.Empty;
            ErrorObject errorObject;
            if(errorObj !=null)
            {
                errorObject = errorObj;
            }
            else
            {
                errorObject = andonErrorManager.AckErrorLogPersonForWeb(logItem, cfgItem);
            }
            if (errorObject != null && errorObject.eConfig != null)
            {
                if (errorObject.eConfig.arrival_message_type > (int)ErrorMsgType.None)//需要消息通知
                {
                    if (errorObject.eConfig.check_arrival == (int)ArrivalModeEnum.CardArrival)
                    {
                        subject = "安灯系统" + errorObject.ELog.error_name;
                        if (errorObject.eSignedPersons.Count > 0)
                        {
                            content = String.Format("{0}发生{1}，签到人员为{2}" + MessageAdditional, errorObject.ELog.machine_code, subject, errorObject.eSignedPersons[0].user_name);
                        }
                        else
                        {
                            content = String.Format("{0}发生{1}，人员已签到" + MessageAdditional, errorObject.ELog.machine_code, subject);
                        }
                    }
                    else
                    {
                        subject = "安灯系统" + errorObject.ELog.error_name;
                        content = String.Format("{0}发生{1}，人员已签到" + MessageAdditional, errorObject.ELog.machine_code, subject);
                    }
                    errorMessageManager.SendErrorMessageAsync(errorObject, (int)MessageLevel.Level1, EventHandleFlowEnum.Event_SignIn, subject, content);//发送消息通知,人员签到在第一层级？？
                }
                if (errorObject.eConfig.arrival_out_color > (int)LightTowerEnum.None)//灯颜色输出
                {
                    //写值
                    lightTowerTagManager.WriteLightColorAsync(errorObject.ELog.station_id, errorObject.eConfig.arrival_out_color, tagAreaAttributeMode, 1);
                    lightTowerTagManager.GreenStatusLightOutAsync(errorObject.ELog.station_id, tagAreaAttributeMode);//绿灯状态切换
                }
            }
        }
        /// <summary>
        /// 处理异常解除,以tag点方式，待实际场合进一步优化代码！！！！！！！！！！！！！！！！！！！！！！！！！！1
        /// </summary>
        /// <param name="machineCode"></param>
        /// <param name="stationId"></param>
        /// <param name="deviceTagValueInfo"></param>
        /// <param name="tagAreaAttributeEnum"></param>
        public static void HandleErrorAckForTag(string machineCode, int stationId,
                                                DeviceTagValueInfo deviceTagValueInfo,
                                                TagAreaAttributeEnum tagAreaAttributeEnum = TagAreaAttributeEnum.station_info)
        {
            string subject = string.Empty, content = string.Empty;
            ErrorObject errorObject=null;
            if (deviceTagValueInfo.system_tag_code==SystemTagCodeEnum.material_require.ToString())
            {
                errorObject = andonErrorManager.AckErrorLogDetailsForMat(machineCode, stationId, deviceTagValueInfo, tagAreaAttributeEnum);
            }
            else
            {
                errorObject = andonErrorManager.AckErrorLogDetails(machineCode, stationId, deviceTagValueInfo, tagAreaAttributeEnum);
            }
            HandleErrorAck(null,null, errorObject);
            
        }
        /// <summary>
        /// Web页面处理异常解除
        /// </summary>
        /// <param name="logItem">记录对象</param>
        /// <param name="cfgItem">配置对象</param>
        /// <param name="error_code">确认代码</param>
        /// <param name="defectives_count">不良品数量</param>
        public static void HandleErrorAck(error_log logItem,error_config cfgItem, ErrorObject errorObj=null)
        {
            string subject = string.Empty, content = string.Empty;
            ErrorObject errorObject = null;

            if(errorObj !=null)
            {
                errorObject = errorObj;
            }
            else
            {
                errorObject = andonErrorManager.AckErrorLogDetails(logItem, cfgItem);
            }

            if (errorObject != null && errorObject.eConfig != null)
            {
                if (errorObject.eConfig.trigger_message_type > (int)ErrorMsgType.None)//需要消息通知
                {
                    subject = "安灯系统" + errorObject.ELog.error_name;
                    if (errorObject.eTypeDetails != null)
                    {
                        content = String.Format("{0}已解除{1}，原因为{2}" + MessageAdditional, logItem.machine_code, subject, errorObject.eTypeDetails.code_name_cn);
                    }
                    else if (errorObject.RequireMat != null)
                    {
                        content = String.Format("{0}已解除{1}，呼叫的物料id为{2}" + MessageAdditional, logItem.machine_code, subject, errorObject.RequireMat.material_id);
                    }
                    else
                    {
                        content = String.Format("{0}已解除{1}" + MessageAdditional, logItem.machine_code, subject);
                    }
                    errorMessageManager.SendErrorMessageAsync(errorObject, (int)MessageLevel.Level1, EventHandleFlowEnum.Event_Ack, subject, content, true);//发送消息通知
                }
                if (errorObject.eConfig.arrival_out_color > (int)LightTowerEnum.None &&
                    errorObject.eConfig.check_arrival > (int)ArrivalModeEnum.NoArrival)     //签到灯颜色复位
                {
                    //写值
                    lightTowerTagManager.WriteLightColorAsync(logItem.station_id, errorObject.eConfig.arrival_out_color, tagAreaAttributeMode, 0);
                    lightTowerTagManager.GreenStatusLightOutAsync(logItem.station_id, tagAreaAttributeMode);//绿灯状态切换
                }
                if (errorObject.eConfig.trigger_out_color > (int)LightTowerEnum.None)//警示灯颜色复位
                {
                    //写值
                    lightTowerTagManager.WriteLightColorAsync(logItem.station_id, errorObject.eConfig.trigger_out_color, tagAreaAttributeMode, 0);
                    lightTowerTagManager.GreenStatusLightOutAsync(logItem.station_id, tagAreaAttributeMode);//绿灯状态切换
                }
            }
            else
            {
                Console.WriteLine("安灯解除记录失败，errorObject != null && errorObject.eConfig != null 失败！");
                if (errorObject.eConfig == null)
                {
                    Console.WriteLine("安灯解除记录失败，errorObject.eConfig == null" + logItem.system_tag_code);
                }
            }
        }
       
        /// <summary>
        /// 异常判断与处理
        /// </summary>
        /// <param name="deviceTagValueItem">设备Tag值</param>
        /// <param name="stationTagValueItem">站位Tag值</param>
        private static void ErrorHandle(DeviceTagValueInfo deviceTagValueItem, TagAreaAttributeEnum tagAreaAttributeEnum,string DetailsInfo="",int MaterialId=-1)
        {
            bool dbExist = false;
            //根据类型检索是否已经存在
            if (deviceTagValueItem.system_tag_code != SystemTagCodeEnum.machine_time_error.ToString())//工时异常不查询数据库
            {
                if (MaterialId>0)//物料呼叫有传入id则特殊处理
                {
                    if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)
                    {
                        dbExist = errorLogManager.CheckMaterialRequireExist(deviceTagValueItem.system_tag_code, MaterialId, deviceTagValueItem.device_code);
                    }
                    else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
                    {
                        dbExist = errorLogManager.CheckMaterialRequireExist(deviceTagValueItem.system_tag_code, MaterialId, "", deviceTagValueItem.device_id);
                    }
                }
                else
                {
                    if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)
                    {
                        dbExist = errorLogManager.CheckExist(deviceTagValueItem.system_tag_code, deviceTagValueItem.device_code);
                    }
                    else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
                    {
                        dbExist = errorLogManager.CheckExist(deviceTagValueItem.system_tag_code, "", deviceTagValueItem.device_id);
                        //list = ErrorWatchList.Where(x => x.ELog.station_id == deviceTagValueItem.device_id &&
                        //                                x.ELog.system_tag_code == deviceTagValueItem.system_tag_code).ToList();
                    }
                }
            }

            //查找当前是否配置有关联的机种信息
            ErrorObject errorObject = andonErrorManager.LoadErrorObject(deviceTagValueItem, tagAreaAttributeEnum);
            //获取到配置，并不存在
            if (errorObject != null && errorObject.eConfig !=null &&
                errorObject.eConfig.error_active==1 && dbExist==false)           //不存在,且激活，则新增
            {
                errorObject = andonErrorManager.AddNewErrorLog(deviceTagValueItem, errorObject, tagAreaAttributeEnum);//记录增加至数据库
                if(errorObject !=null && errorObject.eConfig != null)
                {
                    if (errorObject.eConfig.check_ack > (int)AckModeEnum.NoAck)//只是记录，不需要确认，返回空对象
                    {
                        if (errorObject.eConfig.trigger_message_type > (int)ErrorMsgType.None)//异常发生时需要消息通知
                        {
                            string subject = "安灯系统" + deviceTagValueItem.system_code_name_cn + DetailsInfo;
                            string content = String.Format("{0}发生{1}，请相关人员尽快前往解决！" + MessageAdditional, deviceTagValueItem.device_code, subject);
                            errorMessageManager.SendErrorMessageAsync(errorObject, (int)MessageLevel.Level1, EventHandleFlowEnum.Event_Trigger, subject, content);//发送消息通知
                        }
                        if (errorObject.eConfig.trigger_out_color > (int)LightTowerEnum.None)//灯颜色输出.需要确认解除的才亮灯
                        {
                            //写值
                            lightTowerTagManager.WriteLightColorAsync(deviceTagValueItem.device_id, errorObject.eConfig.trigger_out_color, tagAreaAttributeEnum, 1);
                            lightTowerTagManager.GreenStatusLightOutAsync(deviceTagValueItem.device_id, tagAreaAttributeEnum);//绿灯状态切换
                            Console.WriteLine("error handle finish,time= " + DateTime.Now.ToLongTimeString() + " " + errorObject.DeviceCode + " " + errorObject.DeviceId + " " + errorObject.eConfig.system_tag_code);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 队列超时监测
        /// </summary>
        private static void TimeoutWatch(TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            DateTime dateTime = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);//此处需要加上时区
            int timeDiffMin = 0, ratio = 0;
            string subject = string.Empty, content = string.Empty;

            AndonGlobalCfg.ErrorWatchList.Clear();
           
            var list = errorLogManager.GetAllUnfinishedByDeviceCode(null) ;
            if(list !=null && list.Count>0)
            {
                foreach(var item in list)
                {
                    var errObject = andonErrorManager.LoadErrorLogObject(item);
                    AndonGlobalCfg.ErrorWatchList.Add(errObject);
                }
            }
            try
            {
                for (int i = 0; i < AndonGlobalCfg.ErrorWatchList.Count; i++)
                {
                    ErrorObject errItem = AndonGlobalCfg.ErrorWatchList[i];
                    if (errItem.eConfig == null) continue;

                    TimeSpan interval = dateTime - errItem.ELog.start_time;
                    timeDiffMin = interval.Days * 24 + interval.Hours * 60 + interval.Minutes;
                    subject = "安灯系统" + errItem.ELog.error_name;
                    content = String.Format("{0}发生{1},已持续{2}分钟,已经超时！" + MessageAdditional, errItem.DeviceCode, subject, timeDiffMin);

                    if (errItem.eMsgedPersonList.Any(x => x.eLogPerson.message_level == (int)MessageLevel.Level1 &&
                        x.eLogPerson.message_flow == EventHandleFlowEnum.Event_Trigger.ToString()) == false)
                    {
                        errorMessageManager.SendErrorMessageAsync(errItem, (int)MessageLevel.Level1, EventHandleFlowEnum.Event_Trigger, subject, content);//一级人员通知，此处是补充通知，如果系统down机情况
                    }

                    if (timeDiffMin >= errItem.eConfig.timeout_setting)//大于超时时间
                    {
                        ratio = (timeDiffMin / errItem.eConfig.timeout_setting) + 1;
                        if (ratio > (int)MessageLevel.Level3)//最大级别限制
                        {
                            ratio = (int)MessageLevel.Level3;
                        }
                        if (ratio > (int)MessageLevel.Level1 && ratio <= (int)MessageLevel.Level3)//限定在2-3级
                        {
                            if (errItem.eCfgPersonList.Any(x => x.eCfgPerson.person_level == ratio))//存在该级别的人员
                            {
                                if (errItem.eMsgedPersonList.Any(x => x.eLogPerson.message_level == ratio &&
                                                                 x.eLogPerson.message_flow == EventHandleFlowEnum.Event_Timeout.ToString()) == false)
                                {
                                    error_log logItem = errorLogManager.GetErrorLogById(errItem.ELog.id);
                                    if (logItem != null)//检查数据库是否还存在
                                    {
                                        Console.WriteLine("error timeout level2,time= " + DateTime.Now.ToLongTimeString() + " " + errItem.DeviceCode + " " + errItem.eConfig.error_name);
                                        errorMessageManager.SendErrorMessageAsync(errItem, ratio, EventHandleFlowEnum.Event_Timeout, subject, content);
                                    }
                                    else
                                    {
                                        AndonGlobalCfg.ErrorWatchList.RemoveAll(x => x.ELog.id == errItem.ELog.id);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("TimeoutWatch error:" + ex.Message);
            }
        }
        
    }
}
