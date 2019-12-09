using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorQualityLogManager
    {
        ErrorQualityLogService EQLS = new ErrorQualityLogService();
        public int Insert(error_quality_log obj)
        {
            int count = EQLS.Insert(obj);
            return count;
        }

        public int Update(error_quality_log obj)
        {
            int count = EQLS.Update(obj);
            return count;
        }

        public error_quality_log GetQualityLog(int LogId)
        {
            error_quality_log obj = EQLS.GetQualityLog(LogId);
            return obj;
        }
    }
}
