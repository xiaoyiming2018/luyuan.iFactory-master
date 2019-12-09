using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorConfigPersonManager
    {
        ErrorConfigPersonService ECPS = new ErrorConfigPersonService();
        public List<error_config_person> SelectAll(string code_no = null)
        {
            List<error_config_person> objList = ECPS.SelectAll(code_no);
            return objList;
        }
        public error_config_person SelectSingle(string class_no = null,int id=0)
        {
            error_config_person obj = ECPS.SelectSingle(class_no, id);
            return obj;
        }
        public int Insert(error_config_person obj, bool RetId = false)
        {
            int count = ECPS.Insert(obj, RetId);
            return count;
        }
        public int Update(error_config_person obj)
        {
            int count = ECPS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = ECPS.Del(id);
            return count;
        }
        public int DelErrorConfigid(int id)
        {
            int count = ECPS.DelErrorConfigid(id);
            return count;
        }
        /// <summary>
        /// ��ѯ�쳣��������������Ա
        /// </summary>
        /// <param name="config_id">����id</param>
        /// <param name="class_no">���</param>
        /// <param name="person_level">������Ա�ȼ�</param>
        /// <returns></returns>
        public List<error_config_person> GetCfgPersons(int config_id)
        {
            return ECPS.GetCfgPersons(config_id);
        }
        /// <summary>
        /// ��ѯ�쳣��������������Ա
        /// </summary>
        /// <param name="config_id">����id</param>
        /// <param name="class_no">���</param>
        /// <param name="person_level">������Ա�ȼ�</param>
        /// <returns></returns>
        public List<ErrorCfgPersonView> GetCfgPersonsView(int config_id)
        {
            return ECPS.GetCfgPersonsView(config_id);
        }
    }
}
