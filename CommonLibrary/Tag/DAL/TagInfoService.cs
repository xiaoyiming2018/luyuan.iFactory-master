using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagInfoService
    {
        /// <summary>
        /// 获取system_tag_info表中的所有信息
        /// </summary>
        /// <returns></returns>
        public List<webaccess_tag_info> GetAllTagInfos()
        {
            List<webaccess_tag_info> list = new List<webaccess_tag_info>();
            string command = "SELECT * FROM fimp.webaccess_tag_info order by id ASC";
            list = PostgreHelper.GetEntityList<webaccess_tag_info>(command);
            return list;
        }
        /// <summary>
        /// 获取tag_info页面所需要的所有信息
        /// </summary>
        /// <returns></returns>
        public List<webaccess_tag_info_web> GetAllTagWebInfos()
        {
            List<webaccess_tag_info_web> list = new List<webaccess_tag_info_web>();
            string command = "SELECT a.id, a.tag_code,a.system_type_code,a.system_tag_code,a.tag_description,b.tablename as area_attribute_name,b.name as area_attribute_sub_name "
                            + "FROM fimp.area_attribute_view b, fimp.webaccess_tag_info a  "
                            + "where b.tablename = case ";
            foreach (int i in Enum.GetValues(typeof(TagAreaAttributeEnum)))
            {
                string name = Enum.GetName(typeof(TagAreaAttributeEnum), i);
                command += "when a.area_attribute = " + "'" + i + "'" + " then " + "'" + name + "' ";
            }
            command +=   "end "
                       + "and a.area_attribute_value = b.id order by a.id ASC";

            list = PostgreHelper.GetEntityList<webaccess_tag_info_web>(command);
            
            return list;
        }

        /// <summary>
        /// 获取area_attribute_view中的所有信息  用于area_attribute下拉框
        /// </summary>
        /// <returns></returns>
        public List<area_attribute_info> GetAllAreaAttributeinfo()
        {
            List<area_attribute_info> list = new List<area_attribute_info>();
            string command = "SELECT * FROM fimp.area_attribute_view";
            list = PostgreHelper.GetEntityList<area_attribute_info>(command);
            return list;
        }
        /// <summary>
        /// 获取area_attribute_view 中的某个Table下的List  用于 area_attribute_sub下拉框
        /// </summary>
        /// <returns></returns>
        public List<area_attribute_info> GetAreaAttributeinfoSubNameList(string tablename)
        {
            List<area_attribute_info> list = new List<area_attribute_info>();
            string command = "SELECT * FROM  fimp.area_attribute_view where tablename = " + "'" + tablename + "' "+" order by id";
            list = PostgreHelper.GetEntityList<area_attribute_info>(command);
            return list;
        }

        public int Insert(webaccess_tag_info ob)
        {
            int id = -1;
            string command = "INSERT INTO fimp.webaccess_tag_info" +
                             "(tag_code, system_type_code, min_threshold, max_threshold, tag_description, system_tag_code, area_attribute, area_attribute_value) " +
                             "VALUES(" + "'" + ob.tag_code + "'" +
                             ", " + "'" + ob.system_type_code + "'" +
                             ", " + "'" + ob.min_threshold + "'" +
                             ", " + "'" + ob.max_threshold + "'" +
                             ", " + "'" + ob.tag_description + "'" +
                             ", " + "'" + ob.system_tag_code + "'" +
                             ", " + "'" + ob.area_attribute + "'" +
                             ", " + "'" + ob.area_attribute_value + "'" + ")";
            id = PostgreHelper.ExecuteNonQuery(command);
            return id;
        }

        public int Update(webaccess_tag_info ob)
        {
            int re = -1;
            string command = "UPDATE fimp.webaccess_tag_info set " +
                             " system_type_code = " + "'" + ob.system_type_code + "'" +
                             ", tag_description = " + "'" + ob.tag_description + "'" +
                             ", system_tag_code = " + "'" + ob.system_tag_code + "'" +
                             ", area_attribute = " + "'" + ob.area_attribute + "'" +
                             ", area_attribute_value = " + "'" + ob.area_attribute_value + "'" +
                             " WHERE tag_code = " + "'" + ob.tag_code + "'";
            re = PostgreHelper.ExecuteNonQuery(command);
            return re;
        }

        public webaccess_tag_info SelectOne(string tag_code)
        {
            webaccess_tag_info re = new webaccess_tag_info();
            string command = "SELECT *  FROM fimp.webaccess_tag_info " +
                             " where tag_code =" + "'" + tag_code + "'" +
                             " order by id ASC; ";
            re = PostgreHelper.GetSingleEntity<webaccess_tag_info>(command);
            return re;
        }

        public webaccess_tag_info GetAreaAttributeInfo(string tableName,string areaAttributeSubName)
        {
            webaccess_tag_info wtiw = new webaccess_tag_info();
            string command = "SELECT id FROM fimp.area_attribute_view " +
                             " where tablename = " + "'" + tableName + "'" +
                             " and name = " + "'" + areaAttributeSubName + "'";
            wtiw = PostgreHelper.GetSingleEntity<webaccess_tag_info>(command);
            return wtiw;
        }

        public int Delete(int id)
        {
            int re = -1;
            string command = "DELETE FROM fimp.webaccess_tag_info WHERE id = " + 
                             "'" + id + "'" + "";
            re = PostgreHelper.ExecuteNonQuery(command);
            return re;
        }
    }
}
