using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorTypeManager
    {
        ErrorTypeService ECDS = new ErrorTypeService();
        public List<error_type> SelectAll( string code_no = null)
        {
            List<error_type> objList = ECDS.SelectAll();
            return objList;
        }
        public error_type SelectSingle(string code_no=null,int id=0)
        {
            error_type obj = ECDS.GetType(id, code_no);
            return obj;
        }
        public int Insert(error_type obj)
        {
            int count = ECDS.InsertType(obj);
            return count;
        }
        public int Update(error_type obj)
        {
            int count = ECDS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = ECDS.Del(id);
            return count;
        }
    }
}
