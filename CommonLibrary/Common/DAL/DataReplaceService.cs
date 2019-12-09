using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class DataReplaceService
    {
        /// <summary>
        /// 将tag标签替换为数值
        /// </summary>
        /// <param name="replaceString">需要替换的字符串</param>
        /// <param name="value">值</param>
        /// <param name="characterString">字符串公式</param>
        /// <returns></returns>
        public string ReplaceFormula(string replaceString, double value, string characterString)
        {
            string result = null;
            try
            {
                //result = Regex.Replace(characterString, replaceString,value.ToString(), RegexOptions.IgnoreCase); 
                result = characterString.ToUpper().Replace(replaceString.ToUpper(), value.ToString());
            }
            catch
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// 判断计算公式是否存在错误（+,-,*,/,%,^,!,(,),.）
        /// </summary>
        /// <param name="characterString"></param>
        /// <returns></returns>
        public bool GetFormula(string characterString)
        {
            bool result = true;
            try
            {
                //Char[] stringArr = characterString.ToArray();
                foreach (char c in characterString)
                {
                    switch (c)
                    {
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                        case '%':
                        case '^':
                        case '!':
                        case '(':
                        case ')':
                        case '.':
                            break;
                        default:
                            string character = c.ToString();
                            result = WhetherInt(character);
                            break;
                    }
                    if (!result)
                    {
                        break;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 判断字符是否为整型
        /// </summary>
        /// <param name="character">字符</param>
        /// <returns>是或否</returns>
        public bool WhetherInt(string character)
        {
            bool result = true;
            try
            {
                int num = 0;
                //result= Regex.IsMatch(character, @"^[+-]?/d*$");
                if (int.TryParse(character, out num) == true)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 获取字符串中参数列表
        /// </summary>
        /// <param name="character">字符串</param>
        /// <param name="label">截断字符串</param>
        /// <returns></returns>
        public List<string> GetParameterList(string character, char label)
        {
            List<string> objList = new List<string>();
            List<string> objStringList = new List<string>();
            try
            {
                objList = character.Split(label).ToList();
                if (objList.Count > 0)
                {
                    for (int i = 1; i < objList.Count; i++)
                    {
                        if (objList[i].Length > 0)
                        {
                            int position = GetPosition(objList[i]);
                            string parameter = null;
                            if (position > 0)
                            {
                                parameter = objList[i].Substring(0, position);
                            }
                            else
                            {
                                parameter = objList[i];
                            }
                            if (!string.IsNullOrEmpty(parameter))
                            {
                                objStringList.Add(parameter);
                            }
                        }
                    }
                }
            }
            catch
            {
                objStringList = new List<string>();
            }
            return objStringList;
        }

        /// <summary>
        /// 获取字符串截取位置
        /// </summary>
        /// <param name="characterString"></param>
        /// <returns></returns>
        public int GetPosition(string characterString)
        {
            int position = 0;
            try
            {
                Char[] stringArr = characterString.ToArray();
                for (int i = 0; i < stringArr.Length; i++)
                {
                    switch (stringArr[i])
                    {
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                        case '%':
                        case '^':
                        case '!':
                        case '(':
                        case ')':
                        case '.':
                            position = i;
                            break;
                    }
                    if (position > 0)
                    {
                        break;
                    }
                }
            }
            catch
            {
                position = 0;
            }
            return position;
        }
    }
}
