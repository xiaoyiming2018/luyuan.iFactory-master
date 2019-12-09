using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class UtilizationRateHelper
    {
        private  TagDurationManager tagDurationManager = new TagDurationManager();
        private  UtilizationRateDayManager utilizationRateDayManager = new UtilizationRateDayManager();
        private  UtilizationRateClassManager utilizationRateClassManager = new UtilizationRateClassManager();
        private  DataReplaceManager dataReplaceManager = new DataReplaceManager();
        private  FormulaManager formulaManager = new FormulaManager();
        private  TagTimeClassManager tagTimeClassManager = new TagTimeClassManager();
        private  TagTimeDayManager tagTimeDayManager = new TagTimeDayManager();
        private  SRPLogManager srpLogManager = new SRPLogManager();
        private  TricolorTagDurationManager tricolorTagDurationManager = new TricolorTagDurationManager();
        private  List<system_tag_code> TagCodeList = new List<system_tag_code>();
        private  SystemTagCodeManager systemTagCodeManager = new SystemTagCodeManager();
        private  TagTimeManager tagTimeManager = new TagTimeManager();
        private  TagAreaAttributeEnum tagAreaAttributeMode = TagAreaAttributeEnum.station_info;//最小站位模式
        private  ProScheduleManager proScheduleManager = new ProScheduleManager();
        private UtilizationRateOrderManager utilizationRateOrderManager = new UtilizationRateOrderManager();
        private  ClassInfoManager classInfoManager = new ClassInfoManager();
        private  UtilizationRateFormulaManager utilizationRateFormulaManager = new UtilizationRateFormulaManager();
        private static MachineInfoManager machineInfoManager = new MachineInfoManager();


        public UtilizationRateHelper(TagAreaAttributeEnum AreaAttributeMode= TagAreaAttributeEnum.station_info)
        {
            TagCodeList = systemTagCodeManager.SeclectByTagType((int)TagTypeEnum.UtilizationRate);//加载稼动率类别标签
            tagAreaAttributeMode = AreaAttributeMode;
        }
        /// <summary>
        /// 稼动率计算
        /// </summary>
        public  void CalUtilizationRate()
        {
            DateTime dateTimeNow;
            List<MachineInfo> machines;
            List<station_info> stations;
            List<Tricolor_tag_duration> tagDurationDataList;
            class_info currentClassObj, classObj;

            dateTimeNow = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);//加上时区
            UtilizationRateFormula formulaObj = utilizationRateFormulaManager.SelectSingle();//获取稼动率公式
            DaysData daysData = DateTimeHelper.GetDateTimeData(dateTimeNow, DataWorkCfg.DailyStartTime);                                                                                 //查找班次，计算班次稼动率
            currentClassObj = classInfoManager.SelectClass(dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss"), daysData.TodayDate);
            List<class_info> objClassInfoList = new List<class_info>();

            if (currentClassObj != null)
            {
                objClassInfoList = classInfoManager.SelectAll();
            }

            if (tagAreaAttributeMode == TagAreaAttributeEnum.machine_info)
            {
                machines = machineInfoManager.SelectAll(null);//抓取设备列表
                if (formulaObj != null)//稼动率公式对象不为空
                {
                    foreach (MachineInfo machine in machines)//计算设备的时间数据
                    {
                        tagDurationManager.CalculateTriColorDurationValue(machine, null, dateTimeNow, tagAreaAttributeMode);//计算状态持续时间
                        CalculateDeviceUtilizationRate(machine.machine_code, machine.machine_id, dateTimeNow, formulaObj, currentClassObj, objClassInfoList);//计算稼动率
                    }
                }
            }
            else if (tagAreaAttributeMode == TagAreaAttributeEnum.station_info)
            {
                try
                {
                    stations = DataWorkCfg.StationsList;// stationManager.SelectAll();//从内存中抓取所有站别
                    if (formulaObj != null)//稼动率公式对象不为空
                    {
                        tagDurationDataList = tricolorTagDurationManager.SelectLatestOnlineStatus();//抓取在线的
                        foreach (var item in tagDurationDataList)//比较班次或者日期是否发生变化
                        {
                            classObj = classInfoManager.SelectClass(item.insert_time.ToString("yyyy-MM-dd HH:mm:ss"), daysData.TodayDate);
                            if (classObj.class_no != currentClassObj.class_no || item.insert_time.Day != dateTimeNow.Day)//班次或者日期发生变化，数据结转
                            {
                                tagDurationManager.InsertTriColorDurationTable(item.machine_code, item.station_id, item.tag_code, item.whether, true, dateTimeNow, true, tagAreaAttributeMode);
                            }
                        }
                        tagDurationDataList.Clear();

                        foreach (station_info station in stations)//计算时间数据
                        {
                            tagDurationManager.CalculateTriColorDurationValue(null, station, dateTimeNow, tagAreaAttributeMode);//计算状态持续时间
                            CalculateDeviceUtilizationRate(station.station_name_en, station.station_id, dateTimeNow, formulaObj, currentClassObj, objClassInfoList);//计算稼动率
                        }
                        //计算工单的稼动率数据
                        CalculateOrderUtilizationRate(dateTimeNow, formulaObj);
                    }
                }
                catch (Exception ex)
                {
                    srpLogManager.Insert("UtilizationRateTask error=" + ex.Message);
                }
            }
        }
        /// <summary>
        /// 计算稼动率
        /// </summary>
        /// <param name="device_code">装置的编号</param>
        /// <param name="device_id">装置的id</param>
        /// <param name="insert_time">插入时间</param>
        /// <param name="formulaObj">稼动率公式</param>
        /// <param name="classObj">班次对象</param>
        /// <param name="ClassInfoList">班次列表</param>
        public  void CalculateDeviceUtilizationRate(string device_code, int device_id,
                                                    DateTime insert_time, UtilizationRateFormula formulaObj,
                                                    class_info classObj, List<class_info> ClassInfoList)
        {
            DaysData daysData = DateTimeHelper.GetDateTimeData(insert_time, DataWorkCfg.DailyStartTime);
            List<Tricolor_tag_duration> tagDurationDataList;
            List<Tag_time_class> tagTimeClassList;
            List<Tag_time_day> tagTimeDayList;
            UtilizationRateClass utilizationRateObj;
            DateTime startTime;
            double duration = 0;

            try
            {
                //查找班次，计算班次稼动率
                if (classObj != null)//稼动率公式对象不为空
                {
                    foreach (var sysTagCodeItem in TagCodeList)//遍历所有的状态进行时间计算，时间从TagDuration->tag_time_class
                    {
                        //获取当前时间范围内的machine_code对应的数据
                        tagDurationDataList = tricolorTagDurationManager.SelectTagDurationList(device_code, classObj.start_time, classObj.end_time, sysTagCodeItem.code_name_en);
                        duration = 0;
                        ///处理每一个code的数据
                        if (tagDurationDataList.Count > 0)
                        {
                            duration = tagDurationManager.GetDurationTimeValue(tagDurationDataList, true);
                        }
                        tagTimeManager.InserTagTimeClassTable(device_code, device_id, sysTagCodeItem.code_name_en, classObj.class_no, daysData, duration, tagAreaAttributeMode);
                    }

                    //获取当前时间范围内的machine_code对应的数据
                    tagTimeClassList = tagTimeClassManager.SelectByDateAndClass(device_code, classObj.class_no, daysData.TodayDate);
                    startTime = Convert.ToDateTime(classObj.start_time);
                    tagTimeDayList = tagTimeManager.GetListFromClass(tagTimeClassList);//转换
                    utilizationRateObj = CalcuteUtilizationRateTime(formulaObj, tagTimeDayList, insert_time, startTime, ClassInfoList.Count);

                    utilizationRateClassManager.InsertUtilizationRateClass(device_code, daysData.RawDate.Year, daysData.RawDate.Month, daysData.RawDate.Day, daysData.TodayDate,
                                                                           classObj.class_no,
                                                                           utilizationRateObj.run_time, utilizationRateObj.error_time,
                                                                           utilizationRateObj.others_time, utilizationRateObj.rest_time,
                                                                           utilizationRateObj.boot_time,
                                                                           utilizationRateObj.utilization_rate);
                }

                //计算当日的稼动率数据
                //tagDurationDataList = tricolorTagDurationManager.SelectTagDurationList(device_code, daysData.TodayDateStartTime, daysData.TodayDateEndTime);
                foreach (var sysTagCodeItem in TagCodeList)//遍历所有的状态进行查询,时间从TagDuration->tag_time_day
                {
                    //获取当前时间范围内的machine_code对应的数据
                    tagDurationDataList = tricolorTagDurationManager.SelectTagDurationList(device_code, daysData.TodayDateStartTime, daysData.TodayDateEndTime, sysTagCodeItem.code_name_en);
                    duration = 0;
                    ///处理每一个code的数据
                    if (tagDurationDataList.Count > 0)
                    {
                        duration = tagDurationManager.GetDurationTimeValue(tagDurationDataList, true);
                    }
                    tagTimeManager.InsertTagTimeDayTable(device_code, device_id, sysTagCodeItem.code_name_en, daysData, duration, tagAreaAttributeMode);
                }
                tagTimeDayList = tagTimeDayManager.SelectByDeviceAndDate(device_code, daysData.TodayDate);
                startTime = Convert.ToDateTime(daysData.TodayDateStartTime);
                utilizationRateObj = CalcuteUtilizationRateTime(formulaObj, tagTimeDayList, insert_time, startTime);

                utilizationRateDayManager.InsertUtilizationRate(device_code, daysData.RawDate.Year, daysData.RawDate.Month, daysData.RawDate.Day, daysData.TodayDate,
                                                                utilizationRateObj.run_time, utilizationRateObj.error_time,
                                                                utilizationRateObj.others_time, utilizationRateObj.rest_time,
                                                                utilizationRateObj.boot_time, utilizationRateObj.utilization_rate);


            }
            catch (Exception ex)
            {
                srpLogManager.Insert("CalculateUtilizationRate error=" + ex.Message + "device_code=" + device_code);
            }

        }
        /// <summary>
        /// 计算工单与机种的稼动率
        /// </summary>
        /// <param name="insert_time">插入时间</param>
        /// <param name="formulaObj">稼动率公式</param>
        public void CalculateOrderUtilizationRate(DateTime insert_time, UtilizationRateFormula formulaObj)
        {
            List<Tricolor_tag_duration> tagDurationDataList;
            List<Tag_time_day> tagTimeDayList=new List<Tag_time_day>();
            UtilizationRateClass utilizationRateObj;
            UtilizationRateOrder utilizationRateOrder;
            double duration = 0;

            try
            {
                foreach (var line in DataWorkCfg.LinesList)//查找所有线别
                {
                    List<pro_schedule> list = proScheduleManager.SelectSchedules(line.unit_no,line.line_id,(int)OrderStatusEnum.Excuting);
                    foreach (var schedule in list)
                    {
                        if(schedule.work_order==null || schedule.work_order.Length==0)
                        {
                            continue;
                        }
                        //计算当日的稼动率数据
                        foreach (var sysTagCodeItem in TagCodeList)//遍历所有的状态进行查询,时间从TagDuration->tag_time_day
                        {
                            //获取当前时间范围内的machine_code对应的数据
                            tagDurationDataList = tricolorTagDurationManager.SelectTagDurationListByTime(schedule.start_time, schedule.end_time, sysTagCodeItem.code_name_en);
                            duration = 0;
                            ///处理每一个code的数据
                            if (tagDurationDataList != null && tagDurationDataList.Count > 0)
                            {
                                duration = tagDurationManager.GetDurationTimeValue(tagDurationDataList,true);
                            }
                            //运行时间从08:00开始，一直持续，工单从09:00开始，则需要减去这段时间
                            tagDurationDataList = tricolorTagDurationManager.SelectInterStatus(schedule.start_time, sysTagCodeItem.code_name_en);
                            if(tagDurationDataList!=null && tagDurationDataList.Count>0)//存在状态交替的时间段，加上
                            {
                                duration += tagDurationManager.GetDurationTimeValue(tagDurationDataList, true, schedule.start_time);//计算工单开始时间点的时间
                            }
                            Tag_time_day dayItem = new Tag_time_day();
                            dayItem.duration = duration;
                            dayItem.tag_code = sysTagCodeItem.code_name_en;
                            tagTimeDayList.Add(dayItem);
                        }
                        
                        utilizationRateObj = CalcuteUtilizationRateTime(formulaObj, tagTimeDayList, insert_time, schedule.start_time,1,false);
                        
                        utilizationRateOrder= utilizationRateOrderManager.SelectByWorkOrder(schedule.work_order, line.line_id);
                        if(utilizationRateOrder !=null)
                        {
                            utilizationRateOrder.run_time = utilizationRateObj.run_time;
                            utilizationRateOrder.boot_time = utilizationRateObj.boot_time;
                            utilizationRateOrder.error_time = utilizationRateObj.error_time;
                            utilizationRateOrder.others_time = utilizationRateObj.others_time;
                            utilizationRateOrder.rest_time = utilizationRateObj.rest_time;
                            utilizationRateOrder.utilization_rate = utilizationRateObj.utilization_rate;
                            utilizationRateOrderManager.Update(utilizationRateOrder);
                        }
                        else
                        {
                            utilizationRateOrder = new UtilizationRateOrder();
                            utilizationRateOrder.run_time = utilizationRateObj.run_time;
                            utilizationRateOrder.boot_time = utilizationRateObj.boot_time;
                            utilizationRateOrder.error_time = utilizationRateObj.error_time;
                            utilizationRateOrder.others_time = utilizationRateObj.others_time;
                            utilizationRateOrder.rest_time = utilizationRateObj.rest_time;
                            utilizationRateOrder.utilization_rate = utilizationRateObj.utilization_rate;
                            utilizationRateOrder.part_number = schedule.part_num;
                            utilizationRateOrder.work_order = schedule.work_order;
                            utilizationRateOrder.line_id = line.line_id;
                            utilizationRateOrder.insert_time = insert_time;
                            
                            utilizationRateOrderManager.Insert(utilizationRateOrder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                srpLogManager.Insert("CalculateOrderUtilizationRate error=" + ex.Message);
            }

        }
        /// <summary>
        /// 稼动率分项时间计算
        /// </summary>
        /// <param name="FormulaObj">稼动率公式对象</param>
        /// <param name="TagDayList">时间集合</param>
        /// <param name="InsertTime">当前时间</param>
        /// <param name="ClassCount">班次数量</param>
        /// <param name="TimeLimitCheck">时间显示判断</param>
        /// <returns></returns>
        public UtilizationRateClass CalcuteUtilizationRateTime(UtilizationRateFormula FormulaObj,
                                                               List<Tag_time_day> TagDayList,
                                                               DateTime InsertTime,
                                                               DateTime StartTime,
                                                               int ClassCount = 1,bool TimeLimitCheck=true)
        {
            //休息时间
            double value;
            UtilizationRateClass utilizationRateObj = new UtilizationRateClass();

            try
            {
                utilizationRateObj.rest_time = TagDayList.Where(x => x.tag_code == SystemTagCodeEnum.machine_break.ToString()).Sum(y => y.duration);

                //运行时间
                double run_time = 0;
                string run_time_formula = FormulaObj.run_time_formula;
                List<string> objRunList = dataReplaceManager.GetParameterList(FormulaObj.run_time_formula);
                for (int i = 0; i < objRunList.Count; i++)
                {
                    value = TagDayList.Where(x => x.tag_code == objRunList[i]).Sum(y => y.duration);
                    run_time_formula = dataReplaceManager.ReplaceFormula("$" + objRunList[i], value, run_time_formula);
                }
                //执行公式，并将秒转为分钟
                run_time = Convert.ToDouble(formulaManager.Eval(run_time_formula));
                if (run_time > 0)
                {
                    utilizationRateObj.run_time = Math.Round(run_time, 2);
                }

                //异常时间
                double error_time = 0;
                string error_time_formula = FormulaObj.error_time_formula;
                List<string> objErrorList = dataReplaceManager.GetParameterList(FormulaObj.error_time_formula);
                for (int i = 0; i < objErrorList.Count; i++)
                {
                    value = TagDayList.Where(x => x.tag_code == objErrorList[i]).Sum(y => y.duration);
                    error_time_formula = dataReplaceManager.ReplaceFormula("$" + objErrorList[i], value, error_time_formula);
                }
                //执行公式，并将秒转为分钟
                error_time = Convert.ToDouble(formulaManager.Eval(error_time_formula));
                if (error_time > 0)
                {
                    utilizationRateObj.error_time = Math.Round(error_time, 2);
                }

                //其余时间
                double others_time = 0;
                string others_time_formula = FormulaObj.others_time_formula;
                List<string> objOthersList = dataReplaceManager.GetParameterList(FormulaObj.others_time_formula);
                for (int i = 0; i < objOthersList.Count; i++)
                {
                    value = TagDayList.Where(x => x.tag_code == objOthersList[i]).Sum(y => y.duration);
                    others_time_formula = dataReplaceManager.ReplaceFormula("$" + objOthersList[i], value, others_time_formula);
                }
                //执行公式，并将秒转为分钟
                others_time = Convert.ToDouble(formulaManager.Eval(others_time_formula));
                if (others_time > 0)
                {
                    utilizationRateObj.others_time = Math.Round(others_time, 2);
                }
                //（日期转结）超过时间进行处理使其不超过实际时间
                if (utilizationRateObj.others_time > (1440 / ClassCount))
                {
                    utilizationRateObj.others_time = Math.Round((1440 / (double)ClassCount), 2);
                }

                //开机时间
                double boot_time = 0;
                string boot_time_formula = FormulaObj.boot_time_formula;
                if (!string.IsNullOrEmpty(boot_time_formula))
                {
                    List<string> objBootList = dataReplaceManager.GetParameterList(FormulaObj.boot_time_formula);
                    for (int i = 0; i < objBootList.Count; i++)
                    {
                        value = TagDayList.Where(x => x.tag_code == objBootList[i]).Sum(y => y.duration);
                        boot_time_formula = dataReplaceManager.ReplaceFormula("$" + objBootList[i], value, boot_time_formula);
                    }
                    //执行公式
                    boot_time = Convert.ToDouble(formulaManager.Eval(boot_time_formula));
                    if (boot_time > 0)
                    {
                        utilizationRateObj.boot_time = Math.Round(boot_time, 2);
                    }
                }
                else
                {
                    //计算时间差
                    TimeSpan tSpan = (TimeSpan)(InsertTime - StartTime);
                    utilizationRateObj.boot_time = Math.Round(tSpan.TotalMinutes, 2);
                }
                //（日期转结）超过时间进行处理使其不超过实际时间
                if (TimeLimitCheck && utilizationRateObj.boot_time > (1440 / ClassCount))
                {
                    utilizationRateObj.boot_time = Math.Round((1440 / (double)ClassCount), 2);
                }

                //稼动率
                if (utilizationRateObj.run_time > 0 && utilizationRateObj.boot_time > 0)
                {
                    utilizationRateObj.utilization_rate = Math.Round((utilizationRateObj.run_time / utilizationRateObj.boot_time) * 100, 2);
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("CalcuteUtilizationRateTime error="+ex.Message);
            }
            return utilizationRateObj;
        }
    }
}
