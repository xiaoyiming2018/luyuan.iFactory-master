using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class TagInfoController : BaseController
    {
        private const int _pagesize = 15;  //显示的行数
        TagInfoManager TTM = new TagInfoManager();
        AreaInfoManager TIM = new AreaInfoManager();
        SystemTagTypeManager STTM = new SystemTagTypeManager();
        SystemTagCodeManager STCM = new SystemTagCodeManager();
        // GET: TagInfo
        public ActionResult Index(int pageindex = 1, int pagesize = _pagesize)
        {
            List<webaccess_tag_info_web> list = new List<webaccess_tag_info_web>();
            list = TTM.GetAllTagInfo();
            if (list != null)
            {
                var pagedList = PagedList<webaccess_tag_info_web>.PageList(pageindex, pagesize, list);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                return View();
            }
        }

        // GET: TagInfo/Edit/5
        public ActionResult Edit(int id)
        {
            List<string> li = new List<string>();
            foreach (int i in Enum.GetValues(typeof(TagAreaAttributeEnum)))
            {
                string name = Enum.GetName(typeof(TagAreaAttributeEnum), i);
                li.Add(name);
            }

            ViewBag.TagAreaAttributeList = li;
            string system_type_code = Request.Query["1"];
            string tag_code = Request.Query["2"];
            string system_tag_code = Request.Query["3"];
            string tag_description = Request.Query["4"];
            string area_attribute_name = Request.Query["5"];
            string area_attribute_sub_name = Request.Query["6"];

            string AreaAttribute = HttpContext.Request.Query["TagAreaAttribute"];

            List<area_info> list = new List<area_info>();
            list = TIM.SelectAll();
            ViewBag.AreaAttributeList = list;

            List<System_tag_type> sttList = new List<System_tag_type>();
            sttList = STTM.SeclectAll();
            ViewBag.TypeList = sttList;

            List<system_tag_code_web> stcList = new List<system_tag_code_web>();
            stcList = STCM.SeclectAllForWeb();
            ViewBag.SystemTagCodeList = stcList;

            if (system_type_code != null)
            {
                webaccess_tag_info_web wtiw = new webaccess_tag_info_web();
                wtiw.system_type_code = system_type_code;
                wtiw.tag_code = tag_code;
                wtiw.system_tag_code = system_tag_code;
                wtiw.tag_description = tag_description;
                wtiw.area_attribute_name = area_attribute_name;
                wtiw.area_attribute_sub_name = area_attribute_sub_name;
                return View(wtiw);
            }
            else
            {
                return View();
            }          
        }

        // GET: TagInfo/Delete/5
        public ActionResult Delete(int pagesize = _pagesize)
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            int pageIndex = Convert.ToInt32(Request.Query["pageIndex"]);
            int t = TTM.Delete(id);
            if(t>= 1) //删除成功
            {
                List<webaccess_tag_info_web> list = new List<webaccess_tag_info_web>();
                list = TTM.GetAllTagInfo();
                var pagedList = PagedList<webaccess_tag_info_web>.PageList(pageIndex, pagesize, list);
                ViewBag.model = pagedList.Item2;
                return View("Views/TagInfo/Index.cshtml",pagedList.Item1);
            }
            else
            {
                return View();
            }
        }

        public ActionResult AscriptionConfig()
        {
            string AreaAttribute = HttpContext.Request.Query["TagAreaAttribute"];
            foreach (int i in Enum.GetValues(typeof(TagAreaAttributeEnum)))  //根据枚举名称获取值
            {
                string name = Enum.GetName(typeof(TagAreaAttributeEnum), i);
                if(AreaAttribute == name)
                {
                    ViewBag.ascriptionIndex = 8-i;
                    break;
                }
            }
            List<area_info> list = new List<area_info>();
            list = TIM.SelectAll();
            return View(list);
        }

        /// <summary>
        /// 新增和编辑Tag info
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult EditHandle(int pagesize = _pagesize)
        {
            try
            {
                string Tag_Type = HttpContext.Request.Form["Tag_Type_Text"];
                string Tag_code = HttpContext.Request.Form["Tag_code"];
                string System_Tag_code = HttpContext.Request.Form["System_Tag_code_Text"];
                string Tag_description = HttpContext.Request.Form["Tag_description"];
                string Tag_area_attribute = HttpContext.Request.Form["Tag_area_attribute"];
                string Tag_area_attribute_sub = HttpContext.Request.Form["Tag_area_attribute_sub"];
                if (Tag_Type != null && Tag_code != null && System_Tag_code != null && Tag_description != null && Tag_area_attribute != null && Tag_area_attribute_sub != null)
                {
                    webaccess_tag_info obj = new webaccess_tag_info();
                    obj = TTM.SelectOne(Tag_code);                    
                    if (obj == null)
                    {
                        webaccess_tag_info_web wtiw = new webaccess_tag_info_web();
                        wtiw.system_type_code = Tag_Type;
                        wtiw.tag_code = Tag_code;
                        wtiw.system_tag_code = System_Tag_code;
                        wtiw.tag_description = Tag_description;
                        wtiw.area_attribute_name = Tag_area_attribute;
                        wtiw.area_attribute_sub_name = Tag_area_attribute_sub;
                        TTM.Insert(wtiw);//如果没有查询到则插入一笔数据
                    }                       
                    else
                    {
                        webaccess_tag_info_web wtiw = new webaccess_tag_info_web();
                        wtiw.system_type_code = Tag_Type;
                        wtiw.tag_code = Tag_code;
                        wtiw.system_tag_code = System_Tag_code;
                        wtiw.tag_description = Tag_description;
                        wtiw.area_attribute_name = Tag_area_attribute;
                        wtiw.area_attribute_sub_name = Tag_area_attribute_sub;
                        TTM.Update(wtiw);//如果没有查询到则插入一笔数据
                    }                 
                    List<webaccess_tag_info_web> list = new List<webaccess_tag_info_web>();
                    list = TTM.GetAllTagInfo();
                    var pagedList = PagedList<webaccess_tag_info_web>.PageList(1, pagesize, list);
                    ViewBag.model = pagedList.Item2;
                    return View("Views/TagInfo/Index.cshtml", pagedList.Item1);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        CityInfoManager CIT = new CityInfoManager();
        public ActionResult GetCityList()
        {
            int area_id = Convert.ToInt32(HttpContext.Request.Form["area_id"]);
            List<city_info> list = new List<city_info>();
            list = CIT.SelectAll(area_id);
            return View(list);
        }
        PlantInfoManager PIM = new PlantInfoManager();
        public ActionResult GetPlantList()
        {
            int city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            List<plant_info> list = new List<plant_info>();
            list = PIM.SelectAll(city_id);
            return View(list);
        }

        UnitInfoManager UIM = new UnitInfoManager();
        public ActionResult GetUnitList()
        {
            int plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            List<unit_info> list = new List<unit_info>();
            //list = UIM.SelectAll(plant_id);
            list = UIM.SelectAll();
            return View(list);
        }

        DeptInfoManager DIM = new DeptInfoManager();
        public ActionResult GetDeptList()
        {
            int unit_id = Convert.ToInt32(HttpContext.Request.Form["unit_id"]);
            List<dept_info> list = new List<dept_info>();
            list = DIM.SelectAll();
            return View(list);
        }

        LineInfoManager LIM = new LineInfoManager();
        public ActionResult GetLineList()
        {
            int dept_id = Convert.ToInt32(HttpContext.Request.Form["dept_id"]);
            List<line_info> list = new List<line_info>();
            list = LIM.SelectAll();
            return View(list);
        }

        StationManager SM = new StationManager();
        public ActionResult GetStationList()
        {
            int line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            List<station_info> list = new List<station_info>();
            list = SM.SelectAll(line_id);
            return View(list);
        }

        MachineInfoManager MIM = new MachineInfoManager();
        public ActionResult GetMachineList()
        {
            int station_id = Convert.ToInt32(HttpContext.Request.Form["station_id"]);
            List<MachineInfo> list = new List<MachineInfo>();
            list = MIM.SelectAll(new MachineInfo());
            return View(list);
        }

        public ActionResult GetSystemTagCodeList()
        {
            int Tag_Type = Convert.ToInt32(HttpContext.Request.Form["Tag_Type"]);
            List<system_tag_code> list = new List<system_tag_code>();
            list = STCM.SeclectByTagType(Tag_Type);
            return View(list);
        }

    }
}