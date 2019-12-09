using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonData;
namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tag_logManager
    {
        Tricolor_tag_logService tricolor_log = new Tricolor_tag_logService();

        public int Insert(MachineTagValueInfo TricolorTagLogObject)
        {
            

            Tricolor_tag_log tricolortaglog = new Tricolor_tag_log();
            tricolortaglog.machine_code = TricolorTagLogObject.machine_code;
            tricolortaglog.system_tag_code = TricolorTagLogObject.system_tag_code.ToString();
            tricolortaglog.insert_time = TricolorTagLogObject.insert_time.ToString();

            int flag = tricolor_log.Insert(tricolortaglog);
            return flag;
        }

        public Tricolor_tag_log SelectNewsLog(string machine_code, string ts)
        {
            Tricolor_tag_log tricolorlog = tricolor_log.SelectNewsLog(machine_code, ts);
            return tricolorlog;
        }

        public List<Tricolor_tag_log> SelectDayLog(string datetime)
        {
            List<Tricolor_tag_log> tricolorlogs = tricolor_log.SelectDayLog(datetime);
            return tricolorlogs;
        }
    }
}
