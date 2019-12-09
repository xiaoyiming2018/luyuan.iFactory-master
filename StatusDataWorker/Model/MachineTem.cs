using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class MachineTem
    {
        public int machine_id;
        public string machine_code;
        public string workorder;
        public string partnum;
   
    }
    public class Allmachtion_teminfo
    {
        public static List<MachineTem> MachineList = new List<MachineTem>();
    }
}