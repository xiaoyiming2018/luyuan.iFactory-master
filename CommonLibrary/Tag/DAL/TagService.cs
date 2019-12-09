using System;
using System.Collections.Generic;
using Advantech.IFactory.CommonHelper;
using System.Linq;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagService
    {
        /// <summary>
        /// 获取所有Tag类型
        /// </summary>
        /// <returns></returns>
        public List<SysTagCodeAndType> GetAllSysTagCode()
        {
            List<SysTagCodeAndType> list = new List<SysTagCodeAndType>();
            string command = " SELECT S.id,S.code_name_en,S.code_name_cn,S.code_name_tw," +
                             " S.code_description,S.type_id,T.type_name_en,T.type_name_cn,T.type_name_tw " +
                             " FROM fimp.system_tag_code AS S " +
                             " LEFT JOIN fimp.system_tag_type AS T " +
                             " ON S.type_id = T.id";
            list = PostgreHelper.GetEntityList<SysTagCodeAndType>(command);
            return list;
        }
        /// <summary>
        /// 获取指定时间段内的标签值详细信息列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="Start">开始时间</param>
        /// <param name="End">结束时间</param>
        /// <param name="AttributeValue">类型值</param>
        /// <returns></returns>
        public List<T> GetTagValueInfo<T>(DateTime Start, DateTime End,
                                          TagAreaAttributeEnum AttributeValue = TagAreaAttributeEnum.machine_info) where T : new()
        {
            List<T> list = new List<T>();
            string command = string.Empty;
            if (AttributeValue == TagAreaAttributeEnum.machine_info)//设备所关联的Tag信息
            {
                command = string.Format(" SELECT V.tag_code, V.tag_value, V.insert_time, " +
                                        " I.id,I.area_attribute_value as machine_id,I.system_code, I.description, " +
                                        " T.type_name_en, T.type_name_cn,M.machine_code " +
                                        " FROM fimp.machine_info AS M RIGHT OUTER JOIN" +
                                        " fimp.webaccess_tag_info AS I ON M.machine_id = I.area_attribute_value LEFT OUTER JOIN" +
                                        " fimp.system_tag_type AS T ON I.type_id = T.id RIGHT OUTER JOIN " +
                                        " fimp.tag_value_trend AS V ON I.tag_code = V.tag_code " +
                                        " where I.area_attribute='{0}' V.insert_time>='{1}' and V.insert_time<'{2}'", (int)AttributeValue, Start, End);
            }
            else if (AttributeValue == TagAreaAttributeEnum.station_info)//站位所关联的Tag信息
            {
                command = string.Format(" SELECT V.tag_code, V.tag_value, V.insert_time, " +
                                        " I.id,I.area_attribute_value as station_id,I.system_code, I.description, " +
                                        " T.type_name_en, T.type_name_cn,S.station_name_en " +
                                        " FROM fimp.station_info AS S RIGHT OUTER JOIN" +
                                        " fimp.webaccess_tag_info AS I ON S.station_id = I.area_attribute_value LEFT OUTER JOIN" +
                                        " fimp.system_tag_type AS T ON I.type_id = T.id RIGHT OUTER JOIN " +
                                        " fimp.tag_value_trend AS V ON I.tag_code = V.tag_code " +
                                        " where I.area_attribute='{0}' V.insert_time>='{1}' and V.insert_time<'{2}'", (int)AttributeValue, Start, End);
            }
            else if (AttributeValue == TagAreaAttributeEnum.line_info)//线别所关联的Tag信息
            {
            }
            else if (AttributeValue == TagAreaAttributeEnum.unit_info)//制程所关联的Tag信息
            {
            }
            else if (AttributeValue == TagAreaAttributeEnum.plant_info)//厂区所关联的Tag信息
            {
            }
            else if (AttributeValue == TagAreaAttributeEnum.dept_info)//部门所关联的Tag信息
            {
            }
            else if (AttributeValue == TagAreaAttributeEnum.area_info)//区域所关联的Tag信息
            {
            }
            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<T>(command);
            }
            return list;
        }
        /// <summary>
        /// 获取tag信息
        /// </summary>
        /// <param name="Tags">云端的tag形式记录</param>
        /// <param name="tagAreaAttributeEnum">类型</param>
        /// <returns></returns>
        public List<DeviceTagValueInfo> GetDeviceTagValueInfo(List<MongoDbTag> Tags, TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            if (tagAreaAttributeEnum == TagAreaAttributeEnum.machine_info)
            {
                List<MachineTagInfo> MachineTagList = GetWaTagAndTypeInfo<MachineTagInfo>(tagAreaAttributeEnum);
                
                if (MachineTagList != null & Tags !=null)
                {
                    var query1 = from V in Tags
                                 join I in MachineTagList on V.t.Trim().ToLower() equals I.tag_code.Trim().ToLower()
                                 select new DeviceTagValueInfo
                                 {
                                     device_id = I.machine_id,
                                     device_code = I.machine_code,
                                     id = I.id,
                                     tag_value = V.v.ToString(),
                                     insert_time = V.ts,
                                     tag_code = I.tag_code,
                                     system_type_code = I.system_type_code,
                                     system_type_code_cn = I.system_type_code_cn,
                                     system_tag_code = I.system_tag_code,
                                     system_code_name_cn = I.system_code_name_cn,
                                     tag_description = I.tag_description
                                 };

                    return query1.ToList();
                }
            }
            else if (tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                List<StationTagInfo> StationTagList = GetWaTagAndTypeInfo<StationTagInfo>(tagAreaAttributeEnum);
               
                var query1 = from V in Tags
                             join I in StationTagList on V.t.Trim().ToLower() equals I.tag_code.Trim().ToLower()
                             select new DeviceTagValueInfo
                             {
                                 device_id = I.station_id,
                                 device_code = I.station_name_en,
                                 id = I.id,
                                 tag_value = V.v.ToString(),
                                 insert_time = V.ts,
                                 tag_code = I.tag_code,
                                 system_type_code = I.system_type_code,
                                 system_type_code_cn = I.system_type_code_cn,
                                 system_tag_code = I.system_tag_code,
                                 system_code_name_cn = I.system_code_name_cn,
                                 tag_description = I.tag_description
                             };
                return query1.ToList();
            }
            else if(tagAreaAttributeEnum == TagAreaAttributeEnum.line_info)//线别
            {

            }
            return null;
        }
       
        /// <summary>
        /// 获取webaccess_tag_info内的标签值详细信息列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="TagAttribute">属性类型</param>
        /// <param name="TagAreaAttributeValue">属性值</param>
        /// <param name="TagSysCode">系统编码</param>
        /// <returns></returns>
        public List<T> GetWaTagAndTypeInfo<T>(TagAreaAttributeEnum TagAttribute, int TagAreaAttributeValue = -1, string TagSysCode = "") where T : new()
        {
            List<T> list = new List<T>();
            string command = string.Empty;
            string appendstr = string.Empty;

            if (TagAreaAttributeValue > 0)
            {
                appendstr = string.Format(" and I.area_attribute_value = '{0}'", TagAreaAttributeValue);
            }
            if (TagSysCode.Length > 0)
            {
                appendstr += string.Format(" and I.system_tag_code = '{0}'", TagSysCode);
            }

            if (TagAttribute == TagAreaAttributeEnum.machine_info)//设备所关联的Tag信息
            {

                command = string.Format(" SELECT  I.id, I.tag_code, I.system_tag_code,I.system_type_code, I.tag_description,  " +
                                        " T.type_name_cn as system_type_code_cn,  " +
                                        " C.code_name_cn as system_code_name_cn,  " +
                                        " M.machine_id,M.machine_code  " +
                                        " FROM fimp.webaccess_tag_info as I INNER JOIN fimp.machine_info M ON I.area_attribute_value = M.machine_id  " +
                                        " LEFT OUTER JOIN fimp.system_tag_type as T ON I.system_type_code = T.type_name_en " +
                                        " LEFT OUTER JOIN fimp.system_tag_code as C ON I.system_tag_code = C.code_name_en " +
                                        " where I.area_attribute='{0}'" + appendstr, (int)TagAttribute);
                
            }
            else if (TagAttribute == TagAreaAttributeEnum.station_info)//站位所关联的Tag信息
            {
                command = string.Format(" SELECT  I.id, I.tag_code, I.system_tag_code,I.system_type_code, I.tag_description," +
                                        " T.type_name_cn as system_type_code_cn,   " +
                                        " C.code_name_cn as system_code_name_cn, " +
                                        " S.station_id,S.station_name_en  " +
                                        " FROM    fimp.webaccess_tag_info as I INNER JOIN fimp.station_info S ON I.area_attribute_value = S.station_id  " +
                                        " LEFT OUTER JOIN fimp.system_tag_type as T ON I.system_type_code = T.type_name_en " +
                                        " LEFT OUTER JOIN fimp.system_tag_code as C ON I.system_tag_code = C.code_name_en " +
                                        " where I.area_attribute='{0}'" + appendstr, (int)TagAttribute);
                
            }
            else if (TagAttribute == TagAreaAttributeEnum.line_info)//线别所关联的Tag信息
            {
            }
            else if (TagAttribute == TagAreaAttributeEnum.unit_info)//制程所关联的Tag信息
            {
            }
            else if (TagAttribute == TagAreaAttributeEnum.plant_info)//厂区所关联的Tag信息
            {
            }
            else if (TagAttribute == TagAreaAttributeEnum.dept_info)//部门所关联的Tag信息
            {
            }
            else if (TagAttribute == TagAreaAttributeEnum.area_info)//区域所关联的Tag信息
            {
            }
            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<T>(command);
            }
            return list;
        }
        /// <summary>
        /// 从字符串中解析出值里面包含的id和值
        /// </summary>
        /// <param name="TagValueStr"></param>
        /// <returns></returns>
        public TagIdAndValue GetTagValue(string TagValueStr)
        {
            TagIdAndValue tagIdAndValue = new TagIdAndValue();
            string TagValueSplit = "&";
            int value = -1;
            if (TagValueStr.Contains(TagValueSplit))
            {
                string[] keysArray = TagValueStr.Split(TagValueSplit);
                int.TryParse(keysArray[0], out value);//id
                tagIdAndValue.id = value;
                if(keysArray.Length>1)
                {
                    tagIdAndValue.value = keysArray[1];//value
                }
                if (keysArray.Length > 2)
                {
                    tagIdAndValue.value2 = keysArray[2];//value2
                }
            }
            else
            {
                tagIdAndValue.id = -1;
                tagIdAndValue.value = TagValueStr;
            }
            return tagIdAndValue;
        }

        /// <summary>
        /// 获取站位的标签列表
        /// </summary>
        /// <param name="type_code">类型名称</param>
        /// <returns></returns>
        public List<StationWaTag> GetStationTagInfos(string type_code)
        {
            string command;
            if(string.IsNullOrEmpty(type_code)==false)
            {
                command = " select * from fimp.station_tag where system_type_code='{0}' order BY area_attribute_value";
                command = string.Format(command, type_code);
            }
            else
            {
                command = " select * from fimp.station_tag order BY area_attribute_value";
            }
            var list = PostgreHelper.GetEntityList<StationWaTag>(command);
            return list;
        }
    }
}
