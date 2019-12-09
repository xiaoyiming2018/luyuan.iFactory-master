using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class DataReplaceManager
    {
        DataReplaceService DS = new DataReplaceService();
        /// <summary>
        /// 将tag标签替换为数值
        /// </summary>
        /// <param name="replaceString">需要替换的字符串</param>
        /// <param name="value">值</param>
        /// <param name="characterString">字符串公式</param>
        /// <returns></returns>
        public string ReplaceFormula(string replaceString, double value, string characterString)
        {
            string result = DS.ReplaceFormula(replaceString, value, characterString);
            return result;
        }

        /// <summary>
        /// 判断计算公式是否存在错误（+,-,*,/,%,^,!,(,),.）
        /// </summary>
        /// <param name="characterString"></param>
        /// <returns></returns>
        public bool GetFormula(string characterString)
        {
            bool result = DS.GetFormula(characterString);
            return result;
        }

        /// <summary>
        /// 判断字符是否为整型
        /// </summary>
        /// <param name="character">字符</param>
        /// <returns>是或否</returns>
        public bool WhetherInt(string character)
        {
            bool result = DS.WhetherInt(character);
            return result;
        }

        /// <summary>
        /// 获取字符串中参数列表
        /// </summary>
        /// <param name="character">字符串</param>
        /// <param name="label">截断字符串</param>
        /// <returns></returns>
        public List<string> GetParameterList(string character, char label = '$')
        {
            List<string> objList = DS.GetParameterList(character, label);
            return objList;
        }

        /// <summary>
        /// 获取字符串截取位置
        /// </summary>
        /// <param name="characterString"></param>
        /// <returns></returns>
        public int GetPosition(string characterString)
        {
            int position = DS.GetPosition(characterString);
            return position;
        }
    }
}
