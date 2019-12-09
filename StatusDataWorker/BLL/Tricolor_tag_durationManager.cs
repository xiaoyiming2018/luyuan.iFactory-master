using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tag_durationManager
    {
        Tricolor_tag_durationService duration = new Tricolor_tag_durationService();

        public int Insert(Tricolor_tag_duration obj)
        {
            int flag = duration.Insert(obj);
            return flag;
        }

        public int Update(Tricolor_tag_duration obj)
        {
            int flag = duration.Update(obj);
            return flag;
        }

        public int SelectDuration(string machinecode)
        {
            int flag = duration.SelectDuration(machinecode);
            return flag;
        }
    }
}
