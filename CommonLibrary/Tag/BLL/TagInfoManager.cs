using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace Advantech.IFactory.CommonLibrary
{
    public class TagInfoManager
    {
        TagInfoService TIS = new TagInfoService();
        SystemTagTypeManager STTM = new SystemTagTypeManager();
        TagService TS = new TagService();

        public List<webaccess_tag_info_web> GetAllTagInfo()
        {
            List<webaccess_tag_info_web> list = new List<webaccess_tag_info_web>();
            list = TIS.GetAllTagWebInfos();
            return list;
        }

        public List<area_attribute_info> GetAllAreaAttributeinfo()
        {
            List<area_attribute_info> list = new List<area_attribute_info>();
            list = TIS.GetAllAreaAttributeinfo();
            return list;
        }

        public List<area_attribute_info> GetAreaAttributeinfoSubNameList(string tablename)
        {
            List<area_attribute_info> list = new List<area_attribute_info>();
            list = TIS.GetAreaAttributeinfoSubNameList(tablename);
            return list;
        }

        public int Insert(webaccess_tag_info_web ob)
        {
            int id = -1;
            webaccess_tag_info wti = new webaccess_tag_info();
            webaccess_tag_info areaAttribute = new webaccess_tag_info();
            wti.tag_code = ob.tag_code;
            wti.system_type_code = ob.system_type_code;
            wti.tag_description = ob.tag_description;
            wti.system_tag_code = ob.system_tag_code;
            wti.area_attribute = GetEnumValue(typeof(TagAreaAttributeEnum), ob.area_attribute_name); //获取枚举值
            areaAttribute = TIS.GetAreaAttributeInfo(ob.area_attribute_name, ob.area_attribute_sub_name);
            wti.area_attribute_value = areaAttribute.id;
            id = TIS.Insert(wti);
            return id;
        }


        public int Update(webaccess_tag_info_web ob)
        {
            int id = -1;
            webaccess_tag_info wti = new webaccess_tag_info();
            webaccess_tag_info areaAttribute = new webaccess_tag_info();
            wti.tag_code = ob.tag_code;
            wti.system_type_code = ob.system_type_code;
            wti.tag_description = ob.tag_description;
            wti.system_tag_code = ob.system_tag_code;
            wti.area_attribute = GetEnumValue(typeof(TagAreaAttributeEnum), ob.area_attribute_name); //获取枚举值
            areaAttribute = TIS.GetAreaAttributeInfo(ob.area_attribute_name, ob.area_attribute_sub_name);
            wti.area_attribute_value = areaAttribute.id;
            id = TIS.Update(wti);
            return id;
        }
        /// <summary>
        /// 根据tag名称获取
        /// </summary>
        /// <param name="tag_code"></param>
        /// <returns></returns>
        public webaccess_tag_info SelectOne(string tag_code)
        {
            webaccess_tag_info re = new webaccess_tag_info();
            re = TIS.SelectOne(tag_code);
            return re;
        }

        public int Delete(int id)
        {
            int re = -1;
            re = TIS.Delete(id);
            return re;
        }

        public static int GetEnumValue(Type enumType, string enumName)
        {
            try
            {
                if (!enumType.IsEnum)
                    throw new ArgumentException("enumType必须是枚举类型");
                var values = Enum.GetValues(enumType);
                var ht = new Hashtable();
                foreach (var val in values)
                {
                    ht.Add(Enum.GetName(enumType, val), val);
                }
                return (int)ht[enumName];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<webaccess_tag_info> GetWaTagAndTypeInfo(TagAreaAttributeEnum TagAttribute, int TagAreaAttributeValue = -1, string TagSysCode = "")
        {
            List<webaccess_tag_info> list = new List<webaccess_tag_info>();
            list = TS.GetWaTagAndTypeInfo<webaccess_tag_info>(TagAttribute, TagAreaAttributeValue, TagSysCode);
            return (list);
        }
        /// <summary>
        /// 获取站位的标签列表
        /// </summary>
        /// <param name="type_code">类型名称</param>
        /// <returns></returns>
        public List<StationWaTag> GetStationTagInfos(string type_code)
        {
            List<StationWaTag> list = new List<StationWaTag>();
            list = TS.GetStationTagInfos(type_code);
            return (list);
        }
    }
}
