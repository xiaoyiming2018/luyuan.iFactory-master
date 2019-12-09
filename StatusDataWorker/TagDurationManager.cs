using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class TagDurationManager
    {
        private TricolorTagManager triColorTagManager = new TricolorTagManager();
        private TricolorTagLogManager triColorTagLogManager = new TricolorTagLogManager();
        private TricolorTagDurationManager tricolorTagDurationManager = new TricolorTagDurationManager();
        private StationManager stationManager = new StationManager();
        private MachineInfoManager machineInfoManager = new MachineInfoManager();
        private SRPLogManager srpLogManager = new SRPLogManager();

        /// <summary>
        /// 状态持续时间表。状态变化会计算上一个状态时间
        /// </summary>
        /// <param name="device_code">可能是设备的code，也可能是站位的code</param>
        /// <param name="device_id">当前设备所对应的的id，machine_id或者station_id</param>
        /// <param name="system_tag_code">系统编码</param>
        /// <param name="whether">是否发生变化</param>
        /// <param name="status">状态</param>
        /// <param name="dateTimeNow">当前经过转换后的时间</param>
        /// <param name="IsDataOrClassChange">跨班次或者跨日期</param>
        /// <param name="AreaAttributeMode">当前的模式</param>
        /// <returns></returns>
        public int InsertTriColorDurationTable(string device_code, int device_id,
                                               string system_tag_code,
                                               bool whether, bool status,
                                               DateTime dateTimeNow,
                                               bool IsDateOrClassChange=false,
                                               TagAreaAttributeEnum AreaAttributeMode= TagAreaAttributeEnum.station_info)
        {

            List<Tricolor_tag_duration> objOldList = tricolorTagDurationManager.SelectLatestOnlineStatus(device_code);
            Tricolor_tag_duration newTagDuration = new Tricolor_tag_duration();
            newTagDuration.machine_code = device_code;
            newTagDuration.tag_code = system_tag_code;
            newTagDuration.insert_time = dateTimeNow;
            newTagDuration.whether = whether;
            newTagDuration.tag_status = status;
            newTagDuration.duration_second = 0;
            int count = 0;

            if(AreaAttributeMode== TagAreaAttributeEnum.machine_info)
            {
                MachineInfo machine = machineInfoManager.SelectSingleById(device_id);
                if(machine !=null)
                {
                    newTagDuration.station_id = machine.station_id;
                }
            }
            else if (AreaAttributeMode == TagAreaAttributeEnum.station_info)
            {
                newTagDuration.station_id = device_id;
            }
            try
            {
                if (objOldList != null && objOldList.Count > 0)
                {
                    if (objOldList.Any(x => x.tag_code == newTagDuration.tag_code) == false || IsDateOrClassChange)//状态发生变化，或者班次发生变化
                    {
                        foreach (var item in objOldList)//关闭之前的状态
                        {
                            item.tag_status = false;
                            item.away_time = dateTimeNow;
                            TimeSpan tSpan = (TimeSpan)(dateTimeNow - item.insert_time);
                            if ((int)tSpan.TotalSeconds >= 0)
                            {
                                item.duration_second = (int)tSpan.TotalSeconds;
                            }
                            else
                            {
                                item.duration_second = 0;
                            }
                            count += tricolorTagDurationManager.Update(item); //关闭上一个状态
                        }
                        if (count > 0)
                        {
                            count = tricolorTagDurationManager.Insert(newTagDuration);//插入新的状态
                            DataWorkCfg.statusCollection.AddDuration(newTagDuration);
                        }
                    }
                }
                else
                {
                    if (status)//不存在上一个状态，当前状态为true
                    {
                        count = tricolorTagDurationManager.Insert(newTagDuration);//插入新的状态
                        DataWorkCfg.statusCollection.AddDuration(newTagDuration);
                    }
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("InsertTriColorDurationTable error=" + ex.Message);
            }
            return count;
        }

        /// <summary>
        /// 状态持续时间表。状态变化会计算上一个状态时间
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="system_tag_code"></param>
        /// <param name="dateTimeNow">经过转换后的当前时间</param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public void CalculateTriColorDurationValue(MachineInfo machine,station_info station,
                                                   DateTime dateTimeNow,
                                                   TagAreaAttributeEnum AreaAttributeMode)
        {
            string tag_code=string.Empty;
            TimeSpan tSpan;
            List<Tricolor_tag_duration> tagDurationDataList=new List<Tricolor_tag_duration>();
            int index = 0;

            if (AreaAttributeMode == TagAreaAttributeEnum.machine_info)
            {
                tag_code = MachineBreakManager.MachineBreakCheck(machine, machine.machine_code,dateTimeNow);//取出休息状态转变后的状态
                if (string.IsNullOrEmpty(tag_code) == false)
                {
                    //插入转换过后的及时状态持续表
                    int count = InsertTriColorDurationTable(machine.machine_code, machine.station_id, tag_code, false, true, dateTimeNow,false, AreaAttributeMode);
                }
                //获取当前时间范围内的machine_code对应的数据
                tagDurationDataList = tricolorTagDurationManager.SelectTagDurationList(machine.machine_code, null, null);

            }
            else if(AreaAttributeMode == TagAreaAttributeEnum.station_info)
            {
                List<MachineInfo> machines = machineInfoManager.SelectMachineNameByStaion(station.station_id);
                foreach(MachineInfo machineInfo in machines)
                {
                    tag_code = MachineBreakManager.MachineBreakCheck(machineInfo, station.station_name_en, dateTimeNow);//取出休息状态转变后的状态
                    break;//取出第一台的状态
                }
                if (string.IsNullOrEmpty(tag_code) == false)
                {
                    //插入转换过后的及时状态持续表
                    int count = InsertTriColorDurationTable(station.station_name_en, station.station_id, tag_code, false, true, dateTimeNow, false, AreaAttributeMode);
                }
                //获取当前时间范围内的machine_code对应的数据
                tagDurationDataList = tricolorTagDurationManager.SelectLatestOnlineStatus(station.station_name_en);
            }
            try
            {
                ///计算时间差值
                if (tagDurationDataList != null && tagDurationDataList.Count > 0)//每个设备或者站位当前只允许存在一个状态
                {
                    if (tagDurationDataList.Count > 1)
                    {
                        for (int i = 0; i < tagDurationDataList.Count - 2; i++)//关闭前面所有，保留最后一个
                        {
                            tagDurationDataList[i].tag_status = false;
                            tagDurationDataList[i].duration_second = 0;
                            tagDurationDataList[i].away_time = dateTimeNow;
                            tricolorTagDurationManager.Update(tagDurationDataList[i]);
                        }
                    }
                    index = tagDurationDataList.Count - 1;
                    tSpan = (TimeSpan)(dateTimeNow - tagDurationDataList[index].insert_time);
                    if ((int)tSpan.TotalSeconds >= 0)
                    {
                        tagDurationDataList[index].duration_second = (int)tSpan.TotalSeconds;
                    }
                    else
                    {
                        tagDurationDataList[index].insert_time = dateTimeNow;//时间错误，重新修正
                        tagDurationDataList[index].duration_second = 0;
                    }
                    tricolorTagDurationManager.Update(tagDurationDataList[index]);
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("CalculateTriColorDurationValue error=" + ex.Message);
            }
        }
        /// <summary>
        /// 计算队列里面的时间
        /// </summary>
        /// <param name="DurationList"></param>
        /// <param name="MinuteMode">分钟模式</param>
        /// <returns></returns>
        public double GetDurationTimeValue(List<Tricolor_tag_duration> DurationList, bool MinuteMode = false,
                                           DateTime MinusTime=default)
        {
            double minValue = 0;
            double minusValue = 0;
            if (DurationList != null && DurationList.Count > 0)
            {
                double value = DurationList.Sum(x => x.duration_second);
                if (value > 0 && MinuteMode)
                {
                    minValue = Math.Round((value / 60), 2);//转为分钟
                }
                else
                {
                    minValue = value;//返回秒
                }

                if (MinusTime != default)//需要减去某一时间点时间.如状态从08:00一直持续不变，工单从09:00才开始，需减去08:00-09:00之间时间差值
                {
                    foreach (var item in DurationList)
                    {
                        TimeSpan span = MinusTime - item.insert_time;
                        if (span.TotalSeconds > 0)
                        {
                            minusValue += span.TotalSeconds;
                        }
                    }
                    if(MinuteMode)
                    {
                        minusValue = Math.Round((minusValue / 60), 2);//转为分钟
                    }
                    minValue = Math.Round(minValue - minusValue,2);
                }
            }
            return minValue;
        }

        /// <summary>
        /// 插入及时状态log表
        /// </summary>
        /// <param name="device_code">装置编码，可以为设备编码，也可以为站位编码</param>
        /// <param name="system_tag_code"></param>
        /// <param name="insert_time"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public int InsertTriColorTagLog(string device_code, int device_id, 
                                        string system_tag_code, DateTime insert_time,
                                        TagAreaAttributeEnum AreaAttributeMode)
        {
            Tricolor_tag_log obj = new Tricolor_tag_log();
            obj.machine_code = device_code;
            obj.station_id = device_id;
            obj.tag_code = system_tag_code;
            obj.insert_time = insert_time;
            if (AreaAttributeMode == TagAreaAttributeEnum.machine_info)
            {
                MachineInfo machine = machineInfoManager.SelectSingleById(device_id);
                if (machine != null)
                {
                    obj.station_id = machine.station_id;
                }
            }
            else if (AreaAttributeMode == TagAreaAttributeEnum.machine_info)
            {
                obj.station_id = device_id;
            }
            //插入日志表
            int flag = triColorTagLogManager.Insert(obj);
            
            return flag;
        }

    }
}
