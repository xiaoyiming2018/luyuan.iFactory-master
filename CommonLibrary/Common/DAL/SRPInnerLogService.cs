using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SRPInnerLogService
    {
        public int Insert(srp_inner_log Obj)
        {
            int count = PostgreHelper.InsertSingleEntity<srp_inner_log>(Obj);
            return count;
        }
        public int Update(srp_inner_log Obj)
        {
            int count = PostgreHelper.UpdateSingleEntity<srp_inner_log>(Obj);
            return count;
        }
        /// <summary>
        /// 根据code获取单个对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public srp_inner_log GetInnerLogByCode(string code)
        {
            string command = string.Empty;

            command = string.Format("Select * from fimp.srp_inner_log where srp_code='{0}'", code);

            return PostgreHelper.GetSingleEntity<srp_inner_log>(command);
        }
    }
}
