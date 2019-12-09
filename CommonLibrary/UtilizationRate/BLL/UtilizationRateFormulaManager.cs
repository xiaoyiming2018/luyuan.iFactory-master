using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateFormulaManager
    {
        UtilizationRateFormulaService URFS = new UtilizationRateFormulaService();
        public UtilizationRateFormula SelectSingle()
        {
            UtilizationRateFormula obj = URFS.SelectSingle();
            return obj;
        }
        public int Insert(UtilizationRateFormula obj)
        {
            int count = URFS.Insert(obj);
            return count;
        }

        public int Update(UtilizationRateFormula obj)
        {
            int count = URFS.Update(obj);
            return count;
        }
        public string ReturnState(string value)
        {
            string result = URFS.ReturnState(value);
            return result;
        }
    }
}
