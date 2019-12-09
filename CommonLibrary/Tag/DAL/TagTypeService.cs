using System;
using System.Collections.Generic;
using Advantech.IFactory.CommonHelper;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagTypeService
    {
        public List<System_tag_type> GetAllTagTypeInfo()
        {
            List<System_tag_type> re = new List<System_tag_type>();
            string command = "SELECT id, type_name_en, type_name_cn, type_name_tw, type_description FROM fimp.system_tag_type order by id ASC";
            re = PostgreHelper.GetEntityList<System_tag_type>(command);
            return re;
        }

        public System_tag_type SelectTagTypeInfo(string type_name_en)
        {
            System_tag_type re = new System_tag_type();
            string command = "SELECT id ,type_name_en, type_name_cn, type_name_tw, type_description FROM fimp.system_tag_type where type_name_en = " + "'" + type_name_en + "'";
            re = PostgreHelper.GetSingleEntity<System_tag_type>(command);
            return re;
        }

        public int Insert(System_tag_type obj)
        {
            int id = -1;
            
            try
            {
                id = PostgreHelper.InsertSingleEntity<System_tag_type>(obj);
            }
            catch (Exception e)
            {

            }
            return id;
        }
        public bool DeleteTagTypeInfo(string type_name_en)
        {
            bool re = false;
            string command = "DELETE FROM fimp.system_tag_type WHERE type_name_en = " + "'" + type_name_en + "'" + "";
            try
            {
                if (PostgreHelper.ExecuteNonQuery(command) == -1)
                    re = false;
                else
                    re = true;
                return re;
            }
            catch (Exception e)
            {
                return re;
            }
        }
        public bool Update(System_tag_type obj)
        {
            bool re = false;
            try
            {
                PostgreHelper.UpdateSingleEntity<System_tag_type>(obj);
                re = true;
            }
            catch (Exception e)
            {
                re = false;
                return re;
            }
            return re;
        }
    }
}
