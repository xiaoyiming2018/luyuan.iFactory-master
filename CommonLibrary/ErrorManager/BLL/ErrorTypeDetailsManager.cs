using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorTypeDetailsManager
    {
        ErrorTypeService ECDS = new ErrorTypeService();
        public List<error_type_details> SelectAll( string code_no = null)
        {
            List<error_type_details> objList = ECDS.SelectAllDetails();
            return objList;
        }
        public List<error_type_details> SelectbyErrorid(int type_id = 0)
        {
            List<error_type_details> objList = ECDS.SelectbyErrorid(type_id);
            return objList;
        }
        public error_type_details SelectSingle(string code_no = null,int id=-1)
        {
            error_type_details obj = ECDS.GetTypeDetails(id, code_no);
            return obj;
        }
        public int Insert(error_type_details obj)
        {
            int count = ECDS.InsertTypeDetails(obj);
            return count;
        }
        public int Update(error_type_details obj)
        {
            int count = ECDS.UpdateTypeDetails(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = ECDS.DelTypeDetails(id);
            return count;
        }
    }
}
