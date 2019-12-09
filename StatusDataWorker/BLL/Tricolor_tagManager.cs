using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonData;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tagManager
    {
        Tricolor_tagService tricolorservice = new Tricolor_tagService();
        public int Update(string dbname, int id, Tricolor_tag tritag)
        {
            int flag = tricolorservice.Update(dbname, id, tritag);
            return flag;
        }

        public Tricolor_tag GetStatus(string machine_code)
        {
            Tricolor_tag tricolor = tricolorservice.GetStatus(machine_code);
            return tricolor;
        }

        public int Insert(MachineTagValueInfo TricolorTagObject)
        {
            Tricolor_tag tricolor = new Tricolor_tag();
            tricolor.machine_code = TricolorTagObject.machine_code;
            tricolor.system_tag_code = TricolorTagObject.system_tag_code.ToString();
            tricolor.insert_time = TricolorTagObject.insert_time.ToString();
            
            int flag = tricolorservice.Insert(tricolor);
            return flag;
        }

    }
}
