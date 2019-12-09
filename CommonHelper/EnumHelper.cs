using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Reflection;
using System.ComponentModel;

namespace Advantech.IFactory.CommonHelper
{
    /// <summary>
    /// 将枚举类型转换为结构
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 从枚举类型和它的特性读出并返回队列
        /// </summary>
        /// <param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        /// <returns>键值对</returns>
        public static List<EnumStruct> ConvertEnumToList(Type enumType)
        {
            List<EnumStruct> list = new List<EnumStruct>();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            int Value = 0;

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    Value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute Description = (DescriptionAttribute)arr[0];
                        strText = Description.Description;
                    }
                    else
                    {
                        strText = field.Name;
                    }
                    EnumStruct eStruct = new EnumStruct();
                    eStruct.Discription = strText;
                    eStruct.Key = field.Name;
                    eStruct.Value = Value;
                    list.Add(eStruct);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据描述名称获取该描述所对应的键值
        /// </summary>
        /// <param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        /// <param name="Description">描述名称</param>
        /// <returns>键值</returns>
        public static int GetEnumValueFromDescript(Type enumType, string Description)
        {
            NameValueCollection nvc = new NameValueCollection();
            Type typeDescription = typeof(DescriptionAttribute);
            System.Reflection.FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            int KeyValue;

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    KeyValue = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute DescriptionStr = (DescriptionAttribute)arr[0];
                        if (DescriptionStr.Description.Equals(Description))
                        {
                            return KeyValue;
                        }
                    }
                }
            }
            return 0;
        }
        /// <summary>
        /// 根据枚举值获取属性名称
        /// </summary>
        /// <param name="EnumType">枚举类型</param>
        /// <param name="KeyValue">枚举值</param>
        /// <returns></returns>
        public static string GetEnumNameByKey(Type EnumType, int KeyValue)
        {
            return Enum.GetName(EnumType, KeyValue);
        }

    }
    /// <summary>
    /// 枚举类型结构化
    /// </summary>
    public struct EnumStruct
    {
        public string Key;
        public string Discription;
        public int Value;
    }
}