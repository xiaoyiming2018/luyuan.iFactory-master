using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
     public class SystemTagTypeManager
    {
        SystemTagTypeService STTS = new SystemTagTypeService();
        public List<System_tag_type> SeclectAll()
        {
            List<System_tag_type> list = new List<System_tag_type>();
            list = STTS.SeclectAll();
            return list;
        }

        public System_tag_type SeclectOne(string type_name_en)
        {
            System_tag_type re = new System_tag_type();
            re = STTS.SeclectOne(type_name_en);
            return re;
        }

        public System_tag_type SeclectOneById(int id)
        {
            System_tag_type re = new System_tag_type();
            re = STTS.SeclectOneById(id);
            return re;
        }
    }
}
