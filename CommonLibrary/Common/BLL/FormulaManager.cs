using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class FormulaManager
    {
        FormulaService FS = new FormulaService();
        /// <summary>
        /// 执行string公式
        /// </summary>
        /// <param name="statement">string类型公式</param>
        /// <returns>值</returns>
        public object Eval(string statement)
        {
            object result = FS.Eval(statement);
            return result;
        }
    }
}
