using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advantech.IFactory.Andon
{
    public class AndonErrorManager
    {
        private ErrorLogManager errorLogManager =new ErrorLogManager();
        private PersonManager personManager = new PersonManager();
        private ErrorConfigPnManager errorConfigPnManager = new ErrorConfigPnManager();
        private ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        private ErrorConfigManager errorConfigManager = new ErrorConfigManager();
        private ErrorPersonManager errorPersonManager = new ErrorPersonManager();
        private TagService tagService = new TagService();
        private MaterialRequestInfoManager materialRequestInfoManager = new MaterialRequestInfoManager();
        private SRPLogManager srpLogManager = new SRPLogManager();

        public AndonErrorManager()
        {
        }
        /// <summary>
        /// 根据提供的设备id和tag_code记录加载所有信息
        /// </summary>
        /// <param name="device_id">装置id</param>
        /// <param name="system_tag_code">系统编码</param>
        /// <param name="unit_no">制程</param>
        /// <param name="line_id">线别</param>
        /// <returns></returns>
        public ErrorObject LoadErrorLogObject(int device_id,string system_tag_code,string unit_no, int line_id)
        {
            ErrorObject errorObject = new ErrorObject();
            error_log eLogObject;
            
            eLogObject = errorLogManager.GetUnAckLogByDeviceId(device_id, system_tag_code);//查找error_log记录
            if (eLogObject != null)
            {
                errorObject.ELog = eLogObject;
            }
            //先按照最小单位查询
            errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(system_tag_code, unit_no, line_id);
            if (errorObject.eConfig == null) //未查询到，则以制程查询尝试
            {
                errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(system_tag_code, unit_no);
            }
            if (errorObject.eConfig != null && errorObject.ELog !=null)
            {
                errorObject.eCfgPersonList = errorPersonManager.GetCfgPersonList(errorObject.eConfig.id);  //需通知的配置人员加载

                List<error_config_pn> eCfgPns = errorConfigPnManager.GetCfgPns(errorObject.eConfig.id);
                errorObject.eCfgPnList = eCfgPns;          //关联的机种信息加载
                                                           //已通知人员加载
                errorObject.eMsgedPersonList = errorPersonManager.GetErrorLogPersonList(errorObject.ELog.id);
            }

            return errorObject;
        }
        /// <summary>
        /// 根据异常log记录加载所有信息.初始化加载和web页面确认使用
        /// </summary>
        /// <param name="eLogObject">error_log对象</param>
        /// <param name="unit_no">制程工序</param>
        /// <param name="line_id">线别id</param>
        /// <returns></returns>
        public ErrorObject LoadErrorLogObject(error_log eLogObject,error_config eCfgObject=null)
        {
            ErrorObject errorObject = new ErrorObject();
            errorObject.ELog = eLogObject;
            if (eCfgObject == null)
            {
                errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(eLogObject.system_tag_code, eLogObject.unit_no, eLogObject.line_id);
                if (errorObject.eConfig == null) //未查询到，同时line非0，则以制程查询尝试
                {
                    errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(eLogObject.system_tag_code, eLogObject.unit_no);
                }
            }
            else
            {
                errorObject.eConfig = eCfgObject;
            }
            
            if (errorObject.eConfig !=null)
            {
                errorObject.eCfgPersonList = errorPersonManager.GetCfgPersonList(errorObject.eConfig.id);  //需通知的配置人员加载

                List<error_config_pn> eCfgPns = errorConfigPnManager.GetCfgPns(errorObject.eConfig.id);
                errorObject.eCfgPnList = eCfgPns;          //关联的机种信息加载
                //已通知人员加载
                errorObject.eMsgedPersonList = errorPersonManager.GetErrorLogPersonList(errorObject.ELog.id);
            }
            
            return errorObject;
        }
        /// <summary>
        /// 根据Tag值信息加载所有信息.外部处理进入,需要确认和优化？？？？？？？？？？
        /// </summary>
        /// <param name="TagValueObject"></param>
        /// <param name="tagAreaAttributeEnum"></param>
        /// <returns></returns>
        public ErrorObject LoadErrorObject(DeviceTagValueInfo TagValueObject,TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            ErrorObject errorObject = new ErrorObject();
            List<error_config_pn> eCfgPns;
            string partNo=string.Empty;//当前机种
            Pro_schedule_machine deviceSchedule=null;

            if (tagAreaAttributeEnum==TagAreaAttributeEnum.machine_info)//设备形式
            {
                MachineInfo machine = AndonGlobalCfg.MachinesList.FirstOrDefault(x=>x.machine_id== TagValueObject.device_id);
                if(machine !=null)
                {
                    errorObject.eConfig= errorConfigManager.GetFirstErrorConfig(TagValueObject.system_tag_code, machine.unit_no, machine.line_id);
                    if(errorObject.eConfig==null)
                    {
                        errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(TagValueObject.system_tag_code, machine.unit_no);
                    }
                }
            }
            else if(tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)//站位形式
            {
                station_info station = AndonGlobalCfg.StationsList.FirstOrDefault(x => x.station_id == TagValueObject.device_id);
                if (station != null)
                {
                    errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(TagValueObject.system_tag_code, station.unit_no, station.line_id);
                    if (errorObject.eConfig == null)
                    {
                        errorObject.eConfig = errorConfigManager.GetFirstErrorConfig(TagValueObject.system_tag_code, station.unit_no);
                    }
                }
            }
            
            if (errorObject.eConfig == null) return errorObject;
            errorObject.DeviceCode = TagValueObject.device_code;

            if(TagValueObject.system_type_code==SystemTagCodeEnum.quality_error.ToString())
            {
                eCfgPns = errorConfigPnManager.GetCfgPns(errorObject.eConfig.id);//获取该异常配置的机种信息
                deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(TagValueObject.device_code);
                if (deviceSchedule != null)
                {
                    partNo = deviceSchedule.part_num;//获取当前机种
                }
                if (eCfgPns != null && eCfgPns.Count > 0)
                {
                    if (eCfgPns.Any(x => x.part_num.Equals(partNo))) //当前机种在此配置里面
                    {
                        errorObject.eCfgPnList = eCfgPns;
                    }
                }
            }
            if (errorObject.eConfig != null)
            {
                errorObject.eCfgPersonList = errorPersonManager.GetCfgPersonList(errorObject.eConfig.id);  //需通知的配置人员加载

                eCfgPns = errorConfigPnManager.GetCfgPns(errorObject.eConfig.id);
                errorObject.eCfgPnList = eCfgPns;          //关联的机种信息加载
            }

            return errorObject;
        }

        /// <summary>
        /// 增加设备新的异常记录。会将该异常的所有配置，记录，人员，类型对象一次性加载并返回
        /// </summary>
        /// <param name="TagValueObject"></param>
        /// <param name="eCodeDeatials"></param>
        /// <returns>ErrorObject:包含该异常的配置，记录，需要通知的人员，以及类型</returns>
        public ErrorObject AddNewErrorLog(DeviceTagValueInfo TagValueObject, ErrorObject errorObject, 
                                          TagAreaAttributeEnum tagAreaAttributeEnum,string AppendDetailsInfo = "")
        {
            int id = -1;
            error_log eLog = new error_log();
            MachineInfo machine=null;
            station_info station=null;
            Pro_schedule_machine deviceSchedule = null;

            if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)//设备形式
            {
                machine = AndonGlobalCfg.MachinesList.FirstOrDefault(x => x.machine_code == TagValueObject.device_code);
                deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(TagValueObject.device_code);
            }
            else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)//站位形式
            {
                station = AndonGlobalCfg.StationsList.FirstOrDefault(x => x.station_id == TagValueObject.device_id);
                deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(TagValueObject.device_code);
            }

            if (machine != null || station != null)
            {
                if(deviceSchedule != null)//从在制进度中获取工单机种
                {
                    eLog.pn = deviceSchedule.part_num;
                    eLog.work_order = deviceSchedule.work_order;
                }
                if (TagValueObject.system_tag_code == SystemTagCodeEnum.machine_time_error.ToString())//工时异常类型
                {
                    eLog.start_time = TagValueObject.insert_time;//CT从CT模块过来，无时差
                }
                else if (TagValueObject.system_tag_code == SystemTagCodeEnum.material_require.ToString())//物料呼叫类型
                {
                    eLog.start_time = TagValueObject.insert_time.AddHours(GlobalDefine.SysTimeZone);//加上时区
                    int requireCount = 0, materialId = 0;
                    //支持两种格式：数量，数量&物料id
                    TagIdAndValue valueObject = tagService.GetTagValue(TagValueObject.tag_value);
                    if (valueObject.id > 0 && valueObject.value.Length > 0)
                    {
                        requireCount = valueObject.id;
                        int.TryParse(valueObject.value, out materialId);//id为物料呼叫的id
                        eLog.error_type_id = materialId;//物料呼叫的id直接给到error_type_id字段
                    }
                }
                else
                {
                    eLog.start_time = TagValueObject.insert_time.AddHours(GlobalDefine.SysTimeZone);//加上时区
                }
                eLog.arrival_time= default(DateTime);     //分配默认时间
                eLog.release_time = default(DateTime);    //分配默认时间
                eLog.maintenance_time = default(DateTime);//分配默认时间
                if (machine != null)
                {
                    eLog.machine_code = TagValueObject.device_code;
                    eLog.station_id = machine.station_id;
                    eLog.line_id = machine.line_id;
                    eLog.unit_no = machine.unit_no;
                }
                else
                {
                    eLog.station_id = station.station_id;
                    eLog.machine_code = station.station_name_en;
                    eLog.line_id = station.line_id;
                    eLog.unit_no = station.unit_no;
                }

                eLog.system_tag_code = TagValueObject.system_tag_code;
                eLog.error_name = errorObject.eConfig.error_name + AppendDetailsInfo;

                if (errorObject.eConfig.check_ack == (int)AckModeEnum.NoAck)//只是记录，不需要确认
                {
                    eLog.release_time = eLog.start_time;
                }
                eLog.ack_person_id = -1;//确认人员为空
                if(eLog.error_type_id==0)
                {
                    eLog.error_type_id = -1;//确认编码为空
                }
               
                id = errorLogManager.Insert(eLog);     //插入数据库
                if (id > 0)
                {
                    eLog.id = id;          //id记录
                    errorObject.ELog = eLog;
                }
            }

            return errorObject;
        }

        /// <summary>
        /// 确认并解除异常记录,用于现场提交解除.
        /// 支持三种格式：sn或者id&sn,id&M&M
        /// 格式1：只有card内容时，为HMI触控屏发送的格式，此方式只能确认第一条记录,HMI触控屏无物料呼叫列表；
        /// </summary>
        /// <param name="ErrorWatchList">当前监控队列</param>
        /// <param name="MachineCode">设备编码</param>
        /// <param name="StationId">站位id</param>
        /// <param name="TagValueInItem">标签值对象</param>
        /// <returns>正确执行返回ErrorObject，错误执行返回null</returns>
        public ErrorObject AckErrorLogDetails(string MachineCode, int StationId,
                                              DeviceTagValueInfo TagValueInItem,
                                              TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            ErrorTypeService errorTypeService = new ErrorTypeService();
            error_type_details eDetails = null;
            ErrorObject errorObject=null;
            string TypeSn=string.Empty;
            int errorLogId = -1;
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息

            //支持两种格式，sn或者id&sn，id为异常记录的唯一标识号
            TagIdAndValue valueObject = tagService.GetTagValue(TagValueInItem.tag_value);
            if(tagAreaAttributeEnum==TagAreaAttributeEnum.machine_info)
            {
                MachineInfo machine = AndonGlobalCfg.MachinesList.FirstOrDefault(x=>x.machine_code== MachineCode);
                errorObject = LoadErrorLogObject(TagValueInItem.device_id, TagValueInItem.system_tag_code, machine.unit_no, machine.line_id);
            }
            else if(tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                station_info station = AndonGlobalCfg.StationsList.FirstOrDefault(x => x.station_id == StationId);
                errorObject = LoadErrorLogObject(TagValueInItem.device_id, TagValueInItem.system_tag_code, station.unit_no, station.line_id);
            }
            
            if (errorObject !=null && errorObject.eConfig!=null && 
                errorObject.eConfig.check_ack == (int)AckModeEnum.CodeAck)
            {
                if (valueObject.value != null && valueObject.value.Trim().Length >= 0)
                {
                    if (valueObject.id >= 0 && valueObject.value.Length > 0 && valueObject.value2.Length == 0)
                    {
                        errorLogId = valueObject.id;
                        TypeSn = valueObject.value;
                        Console.WriteLine("AckErrorLogDetails,errorLogId=" + errorLogId + ",TypeSn=" + TypeSn);
                    }

                    else if (valueObject.id < 0 && valueObject.value.Length > 0)
                    {
                        TypeSn = valueObject.value;
                        Console.WriteLine("AckErrorLogDetails,TypeSn=" + TypeSn);
                    }
                }

                if (TypeSn.Length > 0)//获取具体的描述对象
                {
                    eDetails = errorTypeService.GetTypeDetails(-1, TypeSn);
                    if (eDetails == null)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else if (errorObject != null && errorObject.eConfig != null && 
                     errorObject.eConfig.check_ack == (int)AckModeEnum.WithoutCodeAck)//不需要代码解除的
            {
                int.TryParse(valueObject.value,out errorLogId);
                if(errorLogId!= errorObject.ELog.id)//传递的id不匹配
                {
                    Console.WriteLine("安灯解除出错，id不匹配退出：errorLogId="+ errorLogId+ "errorObject.ELog.id="+ errorObject.ELog.id);
                    return null;
                }
            }

            if (errorObject != null && errorObject.ELog !=null)
            {
                if (errorObject.eConfig.check_ack == (int)AckModeEnum.CodeAck)
                {
                    errorObject.ELog.error_type_id = eDetails.id;    //更新具体的id信息
                    errorObject.eTypeDetails = eDetails;             //记录详细原因
                    errorObject.ELog.maintenance_time= TagValueInItem.insert_time.AddHours(GlobalDefine.SysTimeZone);
                    dic.Add("maintenance_time", errorObject.ELog.maintenance_time);
                    dic.Add("error_type_id", errorObject.ELog.error_type_id);//未确认原因的再给值。物料呼叫监测typeid会提前赋值
                }

                errorObject.ELog.release_time = TagValueInItem.insert_time.AddHours(GlobalDefine.SysTimeZone);  //以变量值的时间更新结束时间
                dic.Add("release_time", errorObject.ELog.release_time);

                int count=errorLogManager.Update(errorObject.ELog, dic);        //提交数据库
                if(count>0)
                {
                    Console.WriteLine("AckErrorLogDetails,确认成功,id=" + errorObject.ELog.id);
                }
                else
                {
                    Console.WriteLine("安灯解除更新数据失败！");
                }
                return errorObject;
            }
            else
            {
                Console.WriteLine("安灯解除出错，errorObject != null && errorObject.ELog !=null 失败！");
            }

            return null;
        }
        /// <summary>
        /// 用于网页模式下的解除.时间均在外部维护
        /// </summary>
        /// <param name="logItem">记录对象</param>
        /// <param name="cfgItem">配置对象</param>
        /// <param name="error_code">解除编码</param>
        /// <param name="defectives_count">不良品数量</param>
        /// <returns>正确执行返回ErrorObject，错误执行返回null</returns>
        public ErrorObject AckErrorLogDetails(error_log logItem,error_config cfgItem)
        {
            ErrorObject errorObject = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息

            errorObject = LoadErrorLogObject(logItem, cfgItem);

            if (errorObject != null && errorObject.ELog != null)
            {
                if (errorObject.eConfig.check_ack == (int)AckModeEnum.CodeAck)
                {
                    dic.Add("maintenance_time", errorObject.ELog.maintenance_time);
                    dic.Add("error_type_id", errorObject.ELog.error_type_id);//未确认原因的再给值。物料呼叫监测typeid会提前赋值
                }
                dic.Add("defectives_count", errorObject.ELog.defectives_count);
                dic.Add("release_time", errorObject.ELog.release_time);
                int count = errorLogManager.Update(errorObject.ELog, dic);        //提交数据库
                if (count > 0)
                {
                    Console.WriteLine("AckErrorLogDetails,确认成功,id=" + errorObject.ELog.id);
                }
                else
                {
                    Console.WriteLine("安灯解除更新数据失败！");
                }
                return errorObject;
            }
            else
            {
                Console.WriteLine("安灯解除出错，errorObject != null && errorObject.ELog !=null 失败！");
            }

            return null;
        }
        /// <summary>
        /// 确认并解除异常记录,用于现场提交解除.
        /// 格式1：id&M&M:为web网页物料呼叫传递内容，id为material_require_info表id，M为特殊标识符
        /// 格式2：id:为web网页物料呼叫传递内容，id为error_log表id
        /// </summary>
        /// <param name="ErrorWatchList">当前监控队列</param>
        /// <param name="MachineCode">设备编码</param>
        /// <param name="StationId">站位id</param>
        /// <param name="TagValueInItem">标签值对象</param>
        /// <returns>正确执行返回ErrorObject，错误执行返回null</returns>
        public ErrorObject AckErrorLogDetailsForMat(string MachineCode, int StationId,
                                                    DeviceTagValueInfo TagValueInItem, 
                                                    TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            ErrorTypeService errorTypeService = new ErrorTypeService();
            ErrorObject errorObject = null;
            int errorLogId = -1;
            material_request_info reInfo = null;

            //支持两种格式，sn或者id&sn，id为异常记录的唯一标识号
            TagIdAndValue valueObject = tagService.GetTagValue(TagValueInItem.tag_value);
            if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)
            {
                MachineInfo machine = AndonGlobalCfg.MachinesList.FirstOrDefault(x => x.machine_code == MachineCode);
                errorObject = LoadErrorLogObject(TagValueInItem.device_id, TagValueInItem.system_tag_code, machine.unit_no, machine.line_id);
            }
            else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                station_info station = AndonGlobalCfg.StationsList.FirstOrDefault(x => x.station_id == StationId);
                errorObject = LoadErrorLogObject(TagValueInItem.device_id, TagValueInItem.system_tag_code, station.unit_no, station.line_id);
            }

            if (valueObject.value != null && valueObject.value.Trim().Length >= 0)
            {
                if(AndonGlobalCfg.MaterialType==0)
                {
                    errorLogId = valueObject.id;
                }
                else
                {
                    if (valueObject.id >= 0 && valueObject.value.Length > 0 && valueObject.value2.Length > 0)//物料呼叫确认
                    {
                        reInfo = materialRequestInfoManager.SelectById(valueObject.id);
                        Console.WriteLine("AckErrorLogDetails,errorLogId=" + errorLogId);
                        Console.WriteLine("AckErrorLogDetails,物料呼叫=" + valueObject.id);
                    }
                } 
            }

            if (errorObject != null && errorObject.ELog !=null)
            {
                if (reInfo != null)//物料呼叫确认的对象
                {
                    errorObject.ELog.error_type_id = reInfo.material_id;//更新为物料id信息
                    errorObject.ELog.maintenance_time= TagValueInItem.insert_time.AddHours(GlobalDefine.SysTimeZone);
                    errorObject.RequireMat = reInfo;
                }
                else
                {
                    errorObject.ELog.error_type_id = -1;    //只有呼叫，无具体信息
                    errorObject.eTypeDetails = null;        
                }

                errorObject.ELog.release_time = TagValueInItem.insert_time.AddHours(GlobalDefine.SysTimeZone);  //以变量值的时间更新结束时间

                Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
                if (errorObject.ELog.error_type_id > 0)
                {
                    dic.Add("error_type_id", errorObject.ELog.error_type_id);//未确认原因的再给值。物料呼叫监测typeid会提前赋值
                    dic.Add("maintenance_time", errorObject.ELog.maintenance_time);
                }

                dic.Add("release_time", errorObject.ELog.release_time);
                errorLogManager.Update(errorObject.ELog, dic);        //提交数据库

                Console.WriteLine("AckErrorLogDetails,确认成功,id=" + errorObject.ELog.id);
                return errorObject;
            }

            return null;
        }
        /// <summary>
        /// 责任人员现场签到
        /// 现场提交有3格式：card或者id&card,id&card&M
        /// 格式1：只有card内容时，为HMI触控屏发送的格式，此方式只能确认第一条记录,HMI触控屏无物料呼叫列表；
        /// 格式2：id&card格式：为Web网页异常呼叫时传递内容。Id为error_log表id，card为人员卡号；
        /// 格式3：id&card&M:为web网页物料呼叫传递内容，id为material_require_info表id，card为人员卡号，M为特殊标识符
        /// </summary>
        /// <param name="ErrorWatchList">异常监控队列</param>
        /// <param name="MachineCode">设备编码</param>
        /// <param name="StationId">站位id</param>
        /// <param name="TagValueInItem">标签值对象</param>
        /// <returns></returns>
        public ErrorObject AckErrorLogPerson(string MachineCode, int StationId, 
                                             DeviceTagValueInfo TagValueInItem, 
                                             TagAreaAttributeEnum tagAreaAttributeEnum= TagAreaAttributeEnum.station_info)
        {
            ErrorTypeService errorTypeService = new ErrorTypeService();
            person person = null;
            ErrorObject errorObject = null;
            string PersonCardId=String.Empty;
            int errorLogId=-1;
            material_request_info reInfo=null;
           
            TagIdAndValue valueObject = tagService.GetTagValue(TagValueInItem.tag_value);

            if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)
            {
                MachineInfo machine = AndonGlobalCfg.MachinesList.FirstOrDefault(x => x.machine_code == MachineCode);
                errorObject = LoadErrorLogObject(TagValueInItem.device_id, TagValueInItem.system_tag_code, machine.unit_no, machine.line_id);
            }
            else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                station_info station = AndonGlobalCfg.StationsList.FirstOrDefault(x => x.station_id == StationId);
                errorObject = LoadErrorLogObject(TagValueInItem.device_id, TagValueInItem.system_tag_code, station.unit_no, station.line_id);
            }
            if (valueObject.value == null || valueObject.value.Trim().Length == 0)
            {
                return null; //值无效
            }
            else
            {
                if (valueObject.id >= 0 && valueObject.value.Length > 0 && valueObject.value2.Length == 0)
                {
                    errorLogId = valueObject.id;
                    PersonCardId = valueObject.value;
                    Console.WriteLine("AckErrorLogPerson,errorLogId=" + errorLogId + ",PersonCardId=" + PersonCardId);
                }
                else if (valueObject.id >= 0 && valueObject.value.Length > 0 && valueObject.value2.Length > 0)//物料呼叫传递内容
                {
                    reInfo = materialRequestInfoManager.SelectById(valueObject.id);

                    PersonCardId = valueObject.value;
                    Console.WriteLine("AckErrorLogPerson,物料呼叫传递,PersonCardId=" + PersonCardId);
                }
                else if (valueObject.id < 0 && valueObject.value.Length > 0)
                {
                    PersonCardId = valueObject.value;
                    Console.WriteLine("AckErrorLogPerson,PersonCardId=" + PersonCardId);
                }
            }

            if (errorObject.eConfig.check_arrival == (int)ArrivalModeEnum.CardArrival)//以卡信息签到
            {
                if (PersonCardId.Length > 0)//获取具体的描述对象
                {
                    person = personManager.GetPersonByCardId(PersonCardId);
                    if (person == null)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            if (errorObject != null && errorObject.ELog !=null)
            {
                errorObject.ELog.arrival_time = TagValueInItem.insert_time.AddHours(GlobalDefine.SysTimeZone);  //时间加8小时,更新结束时间
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if(errorObject.eConfig.check_arrival == (int)ArrivalModeEnum.CardArrival)
                {
                    errorObject.eSignedPersons.Add(person);             //记录签到的人员
                    errorObject.ELog.arrival_person_id = person.id;     //更新具体的id信息
                    dic.Add("arrival_person_id", errorObject.ELog.arrival_person_id);
                    errorObject.eSignedPersons.Add(person);              //签到人员记录
                }
                
                dic.Add("arrival_time", errorObject.ELog.arrival_time);
                
                errorLogManager.Update(errorObject.ELog, dic);      //提交数据库
                Console.WriteLine("AckErrorLogPerson,执行结束,id=" + errorObject.ELog.id);
                return errorObject;
            }

            return null;
        }
        /// <summary>
        /// 责任人员现场签到，用于网页端
        /// </summary>
        /// <param name="logItem">异常监控队列</param>
        /// <param name="cfgItem">设备编码</param>
        /// <returns></returns>
        public ErrorObject AckErrorLogPersonForWeb(error_log logItem, error_config cfgItem)
        {
            ErrorTypeService errorTypeService = new ErrorTypeService();
            person person = null;
            material_request_info reInfo = null;
            ErrorObject errorObject = null;

            errorObject = LoadErrorLogObject(logItem, cfgItem);
            if (errorObject.ELog != null && errorObject.eConfig != null)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                if (errorObject.eConfig.check_arrival == (int)ArrivalModeEnum.CardArrival)
                {
                    errorObject.eSignedPersons.Add(person);             //记录签到的人员
                    errorObject.ELog.arrival_person_id = person.id;     //更新具体的id信息
                    dic.Add("arrival_person_id", logItem.arrival_person_id);
                    errorObject.eSignedPersons.Add(person);              //签到人员记录
                }

                dic.Add("arrival_time", errorObject.ELog.arrival_time);

                errorLogManager.Update(errorObject.ELog, dic);      //提交数据库
                Console.WriteLine("AckErrorLogPerson,执行结束,id=" + errorObject.ELog.id);
                return errorObject;
            }

            return null;
        }

    }
}
