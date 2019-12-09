using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advantech.IFactory.WorkOrderManage
{
    public class LineBalanceRateHelper
    {
        LineBalanceRateManager lineBalanceRateManager = new LineBalanceRateManager();
        MachineInfoManager machineInfoManager = new MachineInfoManager();
        CTManager ctManager = new CTManager();
        CtAveragedManager ctAveragedManager = new CtAveragedManager();

        /// <summary>
        /// 计算线平衡率
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="part_num"></param>
        /// <param name="work_order"></param>
        /// <param name="insert_time"></param>
        public void InsertLineBalanceRate(int area_id, int city_id, int plant_id,
                                          string unit_no, int line_id, string part_num, 
                                          string work_order, DateTime insert_time,
                                          TagAreaAttributeEnum AreaAttributeEnum)
        {
            double TotalValue = 0;
            double averageValue = 0;
            List<CtAveraged> CtValueList = new List<CtAveraged>();
            Dictionary<string, object> Dic = new Dictionary<string, object>();
            try
            {
                MachineInfo obj = new MachineInfo();
                obj.area_id = area_id;
                obj.city_id = city_id;
                obj.plant_id = plant_id;
                obj.unit_no = unit_no;
                obj.line_id = line_id;

                if (AreaAttributeEnum == TagAreaAttributeEnum.machine_info)//设备暂未编写，需要补充代码
                {
                    
                }
                else if (AreaAttributeEnum == TagAreaAttributeEnum.station_info)//站位形式
                {
                    List<station_info> stationList = DataWorkerCfg.StationsList.Where(x=>x.line_id== line_id).ToList();
                    if (stationList!=null && stationList.Count > 0)//循环并获取最新C/T
                    {
                        for (int i = 0; i < stationList.Count; i++)
                        {
                            foreach(var tagCodeItem in DataWorkerCfg.CircleTimeTagCodeList)
                            {
                                Dic = new Dictionary<string, object>();
                                List<CT> objCTs = ctManager.SelectAllByWorkOrder(stationList[i].station_name_en, work_order, part_num, tagCodeItem.code_name_en);
                                if(objCTs!=null && objCTs.Count>0)
                                {
                                    TotalValue = objCTs.Sum(x => x.value);
                                    averageValue =( TotalValue / objCTs.Count());//平均时间(秒)
                                    averageValue = Math.Round(averageValue / 60, 2);//分钟
                                    averageValue = averageValue/stationList[i].convert_multiplier;//除以每个工位的转换系数

                                    CtAveraged ctAveraged=ctAveragedManager.SelectLastes(stationList[i].station_name_en, tagCodeItem.code_name_en, work_order);
                                    if (ctAveraged !=null)
                                    {
                                        ctAveraged.averaged_min = averageValue;
                                        Dic.Add("averaged_min", ctAveraged.averaged_min);
                                        ctAveraged.last_time = insert_time;
                                        Dic.Add("last_time", ctAveraged.last_time);
                                        ctAveragedManager.Update(ctAveraged,Dic);
                                    }
                                    else
                                    {
                                        ctAveraged = new CtAveraged();
                                        ctAveraged.work_order = work_order;
                                        ctAveraged.part_number = part_num;
                                        ctAveraged.tag_code = tagCodeItem.code_name_en;
                                        ctAveraged.averaged_min = averageValue;
                                        ctAveraged.device_code = stationList[i].station_name_en;
                                        ctAveraged.device_id = stationList[i].station_id;
                                        ctAveraged.insert_time = insert_time;
                                        ctAveraged.last_time = insert_time;
                                        ctAveragedManager.Insert(ctAveraged);
                                    }
                                    
                                    //节拍时间加入进行计算
                                    if(tagCodeItem.code_name_en.Trim()==SystemTagCodeEnum.cycle_time.ToString())
                                    {
                                        CtValueList.Add(ctAveraged);
                                    }
                                }
                            }
                        }
                    }
                }
                
                if (CtValueList.Count > 0)//根据平均数计算出当前的线平衡率
                {
                    double maxValue = CtValueList.Max(x=>x.averaged_min);
                    double totalValue = CtValueList.Sum(x=>x.averaged_min);
                    double balance_rate = Math.Round(((double)totalValue / (maxValue * CtValueList.Count)) * 100, 2);
                    LineBalanceRate balanceRate = new LineBalanceRate();
                    balanceRate.area_id = area_id;
                    balanceRate.city_id = city_id;
                    balanceRate.plant_id = plant_id;
                    balanceRate.unit_no = unit_no;
                    balanceRate.line_id = line_id;
                    balanceRate.wo = work_order;
                    balanceRate.pn = part_num;
                    balanceRate.insert_time = insert_time;
                    balanceRate.balance_rate = balance_rate;
                    lineBalanceRateManager.Insert(balanceRate);
                }
            }
            catch (Exception ex)
            {
                string date = System.DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("yyyy-MM-dd");
                Console.WriteLine("InsertLineBalanceRate error=" + ex.Message);
            }
        }
        /// <summary>
        /// 按照日期删除
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int DelByTime(DateTime datetime)
        {
            return lineBalanceRateManager.DelByTime(datetime);
        }
    }
}
