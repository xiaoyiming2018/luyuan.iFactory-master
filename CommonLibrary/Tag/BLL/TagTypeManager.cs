using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagTypeManager
    {
        TagTypeService TTS = new TagTypeService();
        public List<System_tag_type> GetALL()
        {
            List<System_tag_type> re = new List<System_tag_type>();
            re = TTS.GetAllTagTypeInfo();
            return re;
        }

        public int Insert(System_tag_type obj)
        {
            int id=TTS.Insert(obj);
            return id;
        }

        public bool Del(string type_name_en)
        {
            bool re = false;
            System_tag_type obj = new System_tag_type();
            obj = TTS.SelectTagTypeInfo(type_name_en);
            if (obj != null && obj.id > 7)      //前几个是固有类型 不能删除
                re = TTS.DeleteTagTypeInfo(type_name_en);
            return re;
        }
        public bool Update(System_tag_type obj)
        {
           return TTS.Update(obj);
        }
        public System_tag_type SelectOne(string type_name_en)
        {
            return TTS.SelectTagTypeInfo(type_name_en);
        }
    }
}
