using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class ClassInfoManager
    {
        ClassInfoService UIS = new ClassInfoService();
        public List<class_info> SelectAll(string class_no = null)
        {
            List<class_info> objList = UIS.SelectAll(class_no);
            return objList;
        }

        public class_info SelectSingle(string class_no = null,int id=0)
        {
            class_info obj = UIS.SelectSingle(class_no: class_no, id: id);
            return obj;
        }
        public int Insert(class_info obj)
        {
            int count = UIS.Insert(obj);
            return count;
        }
        public int Update(class_info obj)
        {
            int count = UIS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = UIS.Del(id);
            return count;
        }
        /// <summary>
        /// 获取当前的班次代码
        /// </summary>
        /// <returns></returns>
        public string GetCurrentClassNo()
        {
            List<class_info> class_Infos = SelectAll();
            string times = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("yyyy-MM-dd");
            DateTime st, ed;
            foreach(var item in class_Infos)
            {
                DateTime.TryParse(times + item.start_time,out st);
                DateTime.TryParse(times + item.end_time, out ed);
                if(DateTime.Now.AddHours(GlobalDefine.SysTimeZone) >= st && DateTime.Now.AddHours(GlobalDefine.SysTimeZone) <= ed)
                {
                    return item.class_no;
                }
            }
            if (class_Infos.Count > 0)
            {
                return class_Infos[0].class_no;
            }
            return string.Empty;
        }

        public class_info SelectClass(string date_time, string date)
        {
            class_info obj = UIS.SelectClass(date_time, date);
            return obj;
        }

    }
}
