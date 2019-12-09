using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorConfigPnManager
    {
        ErrorConfigPnService errorConfigPnService = new ErrorConfigPnService();
        public int Insert(error_config_pn Obj, bool RetId = false)
        {
            return errorConfigPnService.Insert(Obj, RetId);
        }
        /// <summary>
        /// 查询异常配置所关联的机种信息
        /// </summary>
        /// <param name="config_id">配置id</param>
        /// <param name="class_no">班别</param>
        /// <param name="part_num">机种编号</param>
        /// <returns></returns>
        public List<error_config_pn> GetCfgPns(int config_id, string class_no = null, string part_num = null)
        {
            List<error_config_pn> list = new List<error_config_pn>();

            list = errorConfigPnService.GetCfgPartNum(config_id, class_no, part_num);
            return list;
        }

        //public error_config_pn SelectSingle(string class_no = null, int id = 0)
        //{
        //    error_config_pn obj = errorConfigPnService.SelectSingle(class_no, id);
        //    return obj;
        //}

        public int Del(int id)
        {
            int count = errorConfigPnService.Del(id);
            return count;
        }
        public int DelErrorConfigId(int id)
        {
            int count = errorConfigPnService.DelErrorConfigId(id);
            return count;
        }
    }
}
