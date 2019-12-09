using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SystemTagCodeManager
    {
        SystemTagCodeService STCS = new SystemTagCodeService();
        public List<system_tag_code_web> SeclectAllForWeb()
        {
            List<system_tag_code_web> list = new List<system_tag_code_web>();
            list = STCS.SeclectAllForWeb();
            return list;
        }

        public List<system_tag_code> SeclectAll()
        {
            List<system_tag_code> list = new List<system_tag_code>();
            list = STCS.SeclectAll();
            return list;
        }
        public system_tag_code_web SelectOne(string type_name_en)
        {
            system_tag_code_web re = new system_tag_code_web();
            re = STCS.SelectOne(type_name_en);
            return re;
        }

        public int InsertOne(int type_id, string code_name_en, string code_name_cn, string code_name_tw, string code_description)
        {
            int re = -1;
            re = STCS.InsertOne(type_id, code_name_en, code_name_cn, code_name_tw, code_description);
            return re;
        }

        public int Update(int type_id, string code_name_en, string code_name_cn, string code_name_tw, string code_description)
        {
            int re = -1;
            re = STCS.Update(type_id, code_name_en, code_name_cn, code_name_tw, code_description);
            return re;
        }

        public bool DeleteOne(string code_name_en)
        {
            bool re = false;
            system_tag_code_web obj = new system_tag_code_web();
            obj = STCS.SelectOne(code_name_en);
            if (obj != null && obj.id >= 25)      //前几个是固有类型 不能删除
                re = STCS.DeleteOne(code_name_en);
            return re;
        }

        public List<system_tag_code> SeclectByTagType(int type_id)
        {
            List<system_tag_code> list = new List<system_tag_code>();
            list = STCS.SeclectByTagType(type_id);
            return list;
        }
    }
}
