using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
     public class SystemTagTypeService
    {
        public List<System_tag_type> SeclectAll()
        {
            List<System_tag_type> list = new List<System_tag_type>();
            string command = "SELECT id, type_name_en, type_name_cn, type_name_tw, type_description FROM fimp.system_tag_type order by id ASC";
            list = PostgreHelper.GetEntityList<System_tag_type>(command);
            return list;
        }

        public System_tag_type SeclectOne(string type_name_en)
        {
            System_tag_type re = new System_tag_type();
            string command = "SELECT id, type_name_en, type_name_cn, type_name_tw, type_description FROM fimp.system_tag_type  " +
                             "where type_name_en = "+ "'" + type_name_en + "'" +
                             " order by id ASC";
            re = PostgreHelper.GetSingleEntity<System_tag_type>(command);
            return re;
        }

        public System_tag_type SeclectOneById(int id)
        {
            System_tag_type re = new System_tag_type();
            string command = "SELECT id, type_name_en, type_name_cn, type_name_tw, type_description FROM fimp.system_tag_type  " +
                             "where id = " + id + " order by id ASC";
            re = PostgreHelper.GetSingleEntity<System_tag_type>(command);
            return re;
        }

    }
}
