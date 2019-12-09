using System;
using System.Linq;
using System.Collections.Generic;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ErrorConfigController : BaseController
    {
        ErrorConfigManager errorConfigManager = new ErrorConfigManager();
        ErrorConfigPnManager errorConfigPnManager = new ErrorConfigPnManager();
        ErrorConfigPersonManager errorConfigPersonManager = new ErrorConfigPersonManager();
        PersonManager personManager = new PersonManager();
        ClassInfoManager ClassInfoManager = new ClassInfoManager();

        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            int line_id = 0;
            string unit_no = null;
            string part_num = null;
            var objList = errorConfigManager.SelectAll(line_id, unit_no, part_num);
            var pagedList = PagedList<error_config>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Del()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                int count = errorConfigManager.Del(id);
                int PN= errorConfigPnManager.DelErrorConfigId(id);
                int p= errorConfigPersonManager.DelErrorConfigid(id);
                if (count > 0)
                {
                    return View("Views/ErrorConfig/alert.cshtml");
                }
                else
                {
                    return View("alert-fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    var obj = errorConfigManager.GetErrorConfigById(id);
                    return View(obj);
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

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditHandle()
        {
            int id = 0;
            int count = 0;

            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }
            error_config obj = new error_config();
            obj.unit_no = HttpContext.Request.Form["unit_no"];
            if (HttpContext.Request.Form["line_id"].ToString() == "")
            {
                obj.line_id = -1;
            }
            else
            {
                obj.line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            }
            obj.error_name = HttpContext.Request.Form["error_name"];
            obj.system_tag_code = HttpContext.Request.Form["system_tag_code"].ToString().Split("+")[0];
            obj.error_active =Convert.ToInt32( HttpContext.Request.Form["error_active"]);
            if (HttpContext.Request.Form["trigger_count"].ToString()=="")
            {
                obj.trigger_count = 1;
            }
            else
            {
                obj.trigger_count = Convert.ToInt32(HttpContext.Request.Form["trigger_count"]);
            }
            obj.trigger_out_color =Convert.ToInt32( HttpContext.Request.Form["trigger_out_color"]);
            obj.trigger_message_type =Convert.ToInt32( HttpContext.Request.Form["trigger_message_type"]);
            obj.message_multilevel =Convert.ToInt32( HttpContext.Request.Form["message_multilevel"]);
            obj.check_ack =Convert.ToInt32( HttpContext.Request.Form["check_ack"]);
            if (HttpContext.Request.Form["timeout_setting"].ToString()=="")
            {
                obj.timeout_setting = 20;
            }
            else
            {
                obj.timeout_setting = Convert.ToInt32(HttpContext.Request.Form["timeout_setting"]);
            }
            obj.wechat_type =Convert.ToInt32( HttpContext.Request.Form["wechat_type"]);
            obj.check_arrival =Convert.ToInt32( HttpContext.Request.Form["check_arrival"]);
            obj.arrival_message_type =Convert.ToInt32( HttpContext.Request.Form["arrival_message_type"]);
            obj.arrival_out_color =Convert.ToInt32( HttpContext.Request.Form["arrival_out_color"]);
            obj.part_num = HttpContext.Request.Form["part_num"];
            obj.description = HttpContext.Request.Form["description"];

            var list = errorConfigManager.GetErrorConfig(obj.system_tag_code, obj.unit_no, obj.line_id);
           
            if (id > 0)
            {
                obj.id = id;
                count = errorConfigManager.Update(obj);
            }
            else
            {
                if (list != null && list.Count > 0)
                {
                    return View("Views/ErrorConfig/alertInfo.cshtml");
                }
                else
                {
                    count = errorConfigManager.Insert(obj);
                }
            }

            if (count > 0)
            {
                return View("Views/ErrorConfig/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
            //}
        }
        
        //用于缓存机种列表信息
        private static List<error_config_pn> pnList = new List<error_config_pn>();
        /// <summary>
        /// 获取机种列表
        /// </summary>
        /// <param name="config_id"></param>
        /// <returns></returns>
        public ActionResult GetErrCfgPns(int config_id)
        {
            pnList = new List<error_config_pn>();
            pnList = errorConfigPnManager.GetCfgPns(config_id);
            return View("CfgPn", pnList);
        }
        /// <summary>
        /// 机种提交
        /// </summary>
        /// <returns></returns>
        public ActionResult AddErrCfgPN(int config_id, string part_num, string description)
        {
            int id = 0;
            try
            {
                error_config_pn obj = new error_config_pn();
                obj.error_config_id = config_id;
                obj.class_no = "";//机种跟班次无关
                obj.description = description;
                obj.part_num = part_num;
                var objLists = errorConfigPnManager.GetCfgPns(obj.error_config_id, obj.class_no, obj.part_num);
                if (objLists.Count > 0)
                {
                    return Json("pn_error");
                }
                else
                {
                    id = errorConfigPnManager.Insert(obj,true);//返回id
                }
                if (id > 0)
                {
                    obj.id = id;
                    pnList.Add(obj);
                    return View("CfgPn", pnList);
                }
            }
            catch (Exception ex)
            {
               
            }
            return Json("fail");
        }
        /// <summary>
        /// 删除机种
        /// </summary>
        /// <returns></returns>
        public ActionResult DelErrCfgPN(int id, int config_id)
        {
            int count = errorConfigPnManager.Del(id);
            if (count > 0)//删除成功
            {
                error_config_pn item = pnList.FirstOrDefault(x => x.id == id);
                if (item != null)
                {
                    pnList.Remove(item);
                }
                return View("CfgPn", pnList);
            }

            return Json("fail");
        }
        //用于缓存人员列表信息
        private static List<ErrorCfgPersonView> personList=new List<ErrorCfgPersonView>();
        /// <summary>
        /// 获取配置人员
        /// </summary>
        /// <param name="config_id"></param>
        /// <returns></returns>
        public ActionResult GetErrCfgPersons(int config_id)
        {
            personList = new List<ErrorCfgPersonView>();
            personList = errorConfigPersonManager.GetCfgPersonsView(config_id);
            return View("CfgPerson", personList);
        }
        /// <summary>
        /// 增加配置人员
        /// </summary>
        /// <param name="config_id"></param>
        /// <returns></returns>
        public ActionResult AddErrCfgPersons(int config_id,int person_id,string class_no, int person_level)
        {
            int id=0;
            bool checkBit = false;
            var objLists = errorConfigPersonManager.GetCfgPersons(config_id);
            if(objLists !=null && objLists.Count > 0)
            {
                checkBit= objLists.Any(x => x.person_level == person_level && x.person_id == person_id && x.class_no == class_no);
            }
            if (checkBit)
            {
                return Json("person_error");
            }
            else
            {
                error_config_person obj = new error_config_person();
                obj.error_config_id = config_id;
                obj.class_no = class_no;
                obj.person_id = person_id;
                obj.person_level = person_level;
                id = errorConfigPersonManager.Insert(obj,true);//返回id
                if(id > 0)
                {
                    PersonDept personDept = personManager.SelectPersonByID(person_id);
                    class_info classObj= ClassInfoManager.SelectSingle(class_no);
                    if (personDept !=null)
                    {
                        ErrorCfgPersonView errorCfgPersonView = new ErrorCfgPersonView();
                        errorCfgPersonView.error_config_id = config_id;
                        if(classObj !=null)
                        {
                            errorCfgPersonView.class_no = classObj.class_no;
                            errorCfgPersonView.class_name_cn = classObj.class_name_cn;
                        }
                        errorCfgPersonView.dept_name_cn = personDept.dept_name_cn;
                        errorCfgPersonView.id = id;
                        errorCfgPersonView.person_id = person_id;
                        errorCfgPersonView.person_level = person_level;
                        errorCfgPersonView.user_name = personDept.user_name;
                        personList.Add(errorCfgPersonView);
                    }
                    return View("CfgPerson", personList);
                }
            }
            return Json("fail");
        }
        /// <summary>
        /// 删除配置人员
        /// </summary>
        /// <param name="config_id"></param>
        /// <returns></returns>
        public ActionResult DelErrCfgPersons(int id, int config_id)
        {
            int count = errorConfigPersonManager.Del(id);
            if(count>0)//删除成功
            {
                ErrorCfgPersonView item = personList.FirstOrDefault(x=>x.id== id);
                if(item !=null)
                {
                    personList.Remove(item);
                }
                return View("CfgPerson", personList);
            }
            
            return Json("fail");
        }
    }
}