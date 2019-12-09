using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.Andon
{
    public class MachineErrorCodeHelper
    {
        private static MachineErrorCodeManager machineErrorCodeManager = new MachineErrorCodeManager();
        private static MachineErrorCodeLogManager machineErrorCodeLogManager = new MachineErrorCodeLogManager();
        private static SRPLogManager srpLogManager = new SRPLogManager();

        /// <summary>
        /// 设备异常编码处理
        /// </summary>
        /// <param name="TagValueItem"></param>
        /// <returns>返回字符信息，则需要触发安灯</returns>
        public static string MachineErrorCodeHandle(DeviceTagValueInfo TagValueItem)
        {
            int value = 0, errorCodeId = 0;
            string mErrCode = string.Empty;
            MachineErrorCodeLog objLog;
            DateTime dateTimeNow = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            int.TryParse(TagValueItem.tag_value, out value);//值比较

            if (value > 0)
            {
                MachineErrorCode machineErrorCode = machineErrorCodeManager.GetErrorCodeByValue(TagValueItem.tag_value);
                if (machineErrorCode != null)//查找到该异常代码的值
                {
                    errorCodeId = machineErrorCode.id;
                    if (machineErrorCode.require_andon > 0)
                    {
                        mErrCode= machineErrorCode.code_name_cn;
                    }
                }

                objLog = new MachineErrorCodeLog();//记录日志
                objLog.machine_code = TagValueItem.device_code;
                objLog.error_code_id = errorCodeId;
                objLog.value = value;
                objLog.insert_time = dateTimeNow;
                if (machineErrorCodeLogManager.Insert(objLog) < 0)
                {
                    srpLogManager.Insert("error_code插入记录失败,TagValueItem.device_code=" + TagValueItem.device_code + "value=" + value);
                }
            }
            else
            {
                List<MachineErrorCodeLog> list = machineErrorCodeLogManager.SelectUnclosed(TagValueItem.device_code);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        item.away_time = dateTimeNow;
                        Dictionary<string, object> Dic = new Dictionary<string, object>();
                        Dic.Add("away_time", item.away_time);
                        machineErrorCodeLogManager.Update(item, Dic);
                    }
                }
                else
                {
                    srpLogManager.Insert("未查找到error_code记录，更新结束时间失败,TagValueItem.device_code=" + TagValueItem.device_code);
                }
            }
            return mErrCode;
        }
    }
}
