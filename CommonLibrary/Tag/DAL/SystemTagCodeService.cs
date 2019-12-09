using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SystemTagCodeService
    {
        public List<system_tag_code> SeclectAll()
        {
            List<system_tag_code> list = new List<system_tag_code>();
            string command = "SELECT id, type_id, code_name_en, code_name_cn, code_name_tw, code_description FROM fimp.system_tag_code order by id ASC";
            list = PostgreHelper.GetEntityList<system_tag_code>(command);
            return list;
        }

        public List<system_tag_code_web> SeclectAllForWeb()
        {
            List<system_tag_code_web> list = new List<system_tag_code_web>();
            string command = "SELECT a.id,a.type_id, b.type_name_en, a.code_name_en, a.code_name_cn, a.code_name_tw, a.code_description "+
                " FROM fimp.system_tag_code a,fimp.system_tag_type b " +
                " where a.type_id = b.id order by a.id ASC; ";
            list = PostgreHelper.GetEntityList<system_tag_code_web>(command);
            return list;
        }

        public system_tag_code_web SelectOne(string type_name_en)
        {
            system_tag_code_web re = new system_tag_code_web();
            string command = "SELECT a.id,a.type_id, b.type_name_en, a.code_name_en, a.code_name_cn, a.code_name_tw, a.code_description " +
                             " FROM fimp.system_tag_code a,fimp.system_tag_type b " +
                             " where a.type_id = b.id "+
                             " and code_name_en =" + "'" + type_name_en + "'" +
                             " order by a.id ASC; ";
            re = PostgreHelper.GetSingleEntity<system_tag_code_web>(command);
            return re;
        }

        public int InsertOne(int type_id,string code_name_en,string code_name_cn,string code_name_tw,string code_description)
        {
            int re = -1;
            string command = "INSERT INTO fimp.system_tag_code" +
                             "(type_id, code_name_en, code_name_cn, code_name_tw, code_description) " +
                             "VALUES(" + "'" + type_id + "'" + 
                             ", " + "'" + code_name_en + "'" + 
                             ", " + "'" + code_name_cn + "'" + 
                             ", " + "'" + code_name_tw + "'" + 
                             ", " + "'" + code_description + "'" + ")";
            re = PostgreHelper.ExecuteNonQuery(command);
            return re;
        }

        public int Update(int type_id, string code_name_en, string code_name_cn, string code_name_tw, string code_description)
        {
            int re = -1;
            string command = "UPDATE fimp.system_tag_code set " +
                             " type_id = "+ "'" + type_id + "'" + 
                             ", code_name_cn = "+ "'" + code_name_cn + "'" + 
                             ", code_name_tw = "+ "'" + code_name_tw + "'" +
                             ", code_description = " + "'" + code_description + "'" +
                             " WHERE code_name_en = " + "'" + code_name_en + "'";
            re = PostgreHelper.ExecuteNonQuery(command);
            return re;
        }

        public bool DeleteOne(string code_name_en)
        {
            bool re = false;
            string command = "DELETE FROM fimp.system_tag_code WHERE code_name_en = " + "'" + code_name_en + "'" + "";
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
        public List<system_tag_code> SeclectByTagType(int type_id)
        {
            List<system_tag_code> list = new List<system_tag_code>();
            string command = "SELECT id, type_id, code_name_en, code_name_cn, code_name_tw, code_description FROM fimp.system_tag_code where type_id = " + "" + type_id + "" +"order by id ASC";
            list = PostgreHelper.GetEntityList<system_tag_code>(command);
            return list;
        }


    }
}
