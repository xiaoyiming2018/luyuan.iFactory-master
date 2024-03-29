﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;
using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonHelper.ScadaAPI;

namespace WebApplication.Controllers
{
    public class MaterialConfirmController : BaseController
    {
        MaterialRequestInfoManager MRIM = new MaterialRequestInfoManager();
        TagInfoManager TIM = new TagInfoManager();
        PersonManager PM = new PersonManager();
        /// <summary>
        /// 物料确认首页
        /// </summary>
        /// <param name="start_time">查询（开始时间）</param>
        /// <param name="end_time">查询（结束时间）</param>
        /// <param name="material_name">查询（物料名称——模糊查询）</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IActionResult Index(string start_time, string end_time, string material_name, string cookies1, int pageindex = 1, int pagesize = 15)
        {
            //当提交查询表单时，将start_time end_time material_name这三个参数保存到cookies中
            if (!string.IsNullOrEmpty(cookies1))
            {
                //将信息存入到cookies中
                //HttpCookie cok = new HttpCookie();
                DateTimeOffset dto = new DateTimeOffset(DateTime.Now.AddHours(GlobalDefine.SysTimeZone));
                dto = dto.AddDays(1);
                CookieOptions co = new CookieOptions();
                co.Expires = dto;    //设置cookies保存的时间 这边设定为30天
                if (string.IsNullOrEmpty(start_time)) { start_time = ""; }
                if (string.IsNullOrEmpty(end_time)) { end_time = ""; }
                if (string.IsNullOrEmpty(material_name)) { material_name = ""; }
                Response.Cookies.Append("Andon_start_time1", start_time, co);
                Response.Cookies.Append("Andon_end_time1", end_time, co);
                Response.Cookies.Append("Andon_material_name1", material_name, co);
            }
            PageInfo pageInfo = GetSearchInfo1();

            //记录用户输入的查询信息,response和request存在时间差，即当前response添加的值，request获取不到，只能在下一次才能获取
            if (!string.IsNullOrEmpty(start_time))
            {
                ViewBag.start_time = start_time;
            }
            else
            {
                if (string.IsNullOrEmpty(pageInfo.start_time))
                {
                    ViewBag.start_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).AddDays(-1).ToString();
                }
                else
                {
                    ViewBag.start_time = pageInfo.start_time;
                }
            }

            if (!string.IsNullOrEmpty(end_time))
            {
                ViewBag.end_time = end_time;
            }
            else
            {
                if (string.IsNullOrEmpty(pageInfo.end_time))
                {
                    ViewBag.end_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString();
                }
                else
                {
                    ViewBag.end_time = pageInfo.end_time;
                }
            }
            if (material_name != null)
            {
                ViewBag.material_name = material_name;
            }
            else
            {
                if (string.IsNullOrEmpty(pageInfo.material_name))
                {
                    ViewBag.material_name = "";
                }
                else
                {
                    ViewBag.material_name = pageInfo.material_name;
                }
            }

            //显示所有物料呼叫
            List<RequestAndInfo> results = MRIM.SelectRequestAndInfoAll(ViewBag.start_time, ViewBag.end_time, ViewBag.material_name);

            var pagedList = PagedList<RequestAndInfo>.PageList(pageindex, pagesize, results);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);

        }

        /// <summary>
        /// 已完成配送页面
        /// </summary>
        /// <param name="start_time">查询（开始时间）</param>
        /// <param name="end_time">查询（结束时间）</param>
        /// <param name="material_name">查询（物料名称——模糊查询）</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IActionResult Finish(string start_time, string end_time, string material_name, string cookies2, int pageindex = 1, int pagesize = 15)
        {
            //当提交查询表单时，将start_time end_time material_name这三个参数保存到cookies中
            if (!string.IsNullOrEmpty(cookies2))
            {
                //将信息存入到cookies中
                //HttpCookie cok = new HttpCookie();
                DateTimeOffset dto = new DateTimeOffset(DateTime.Now.AddHours(GlobalDefine.SysTimeZone));
                dto = dto.AddDays(1);
                CookieOptions co = new CookieOptions();
                co.Expires = dto;    //设置cookies保存的时间 这边设定为30天
                if (string.IsNullOrEmpty(start_time)) { start_time = ""; }
                if (string.IsNullOrEmpty(end_time)) { end_time = ""; }
                if (string.IsNullOrEmpty(material_name)) { material_name = ""; }
                Response.Cookies.Append("Andon_start_time2", start_time, co);
                Response.Cookies.Append("Andon_end_time2", end_time, co);
                Response.Cookies.Append("Andon_material_name2", material_name, co);
            }
            PageInfo pageInfo = GetSearchInfo2();

            //记录用户输入的查询信息,response和request存在时间差，即当前response添加的值，request获取不到，只能在下一次才能获取
            if (!string.IsNullOrEmpty(start_time))
            {
                ViewBag.start_time = start_time;
            }
            else
            {
                if (string.IsNullOrEmpty(pageInfo.start_time))
                {
                    ViewBag.start_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).AddDays(-1).ToString();
                }
                else
                {
                    ViewBag.start_time = pageInfo.start_time;
                }
            }

            if (!string.IsNullOrEmpty(end_time))
            {
                ViewBag.end_time = end_time;
            }
            else
            {
                if (string.IsNullOrEmpty(pageInfo.end_time))
                {
                    ViewBag.end_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString();
                }
                else
                {
                    ViewBag.end_time = pageInfo.end_time;
                }
            }
            if (material_name != null)
            {
                ViewBag.material_name = material_name;
            }
            else
            {
                if (string.IsNullOrEmpty(pageInfo.material_name))
                {
                    ViewBag.material_name = "";
                }
                else
                {
                    ViewBag.material_name = pageInfo.material_name;
                }
            }
            //显示所有已配送完成的呼叫物料
            List<RequestAndInfo> results = MRIM.SelectFinishAll(ViewBag.start_time, ViewBag.end_time, ViewBag.material_name);

            var pagedList = PagedList<RequestAndInfo>.PageList(pageindex, pagesize, results);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);

        }
        /// <summary>
        /// 人员确认
        /// </summary>
        /// <param name="ConfirmCard"></param>
        /// <param name="MaterialId"></param>
        /// <returns></returns>
        public IActionResult MaConfirm(string card_number, string select_id)
        {
            //根据编号查找对应人员姓名并传到前台           
            person res = PM.Selectnum(card_number);
            if (res == null)
            {
                ViewBag.ConfirmName = null;
                return View("~/Views/MaterialConfirm/Alert.cshtml");
            }
            else
            {
                //根据物料的id获取站位id
                int station_id = MRIM.SelectById(Convert.ToInt32(select_id)).station_id;

                List<webaccess_tag_info> tagList = new List<webaccess_tag_info>();
                tagList = TIM.GetWaTagAndTypeInfo(TagAreaAttributeEnum.station_info, station_id, "andon_ack_person");
                string TagValue = select_id + "&" + card_number + "&M";
                if(tagList.Count>0)
                {
                    if (ScadaAPIConfig.EnableScadaApi)
                    {
                        ScadaAPIHelper.WriteValueAsync(tagList[0].tag_code, TagValue);
                    }
                    else
                    {
                        // MqttManager.MqttHelper.WriteTagValueMsgToMqtt(tagList[0].tag_code, TagValue, true);
                    }
                }
                
                //将获取到的person id更新到material_request_info中
                material_request_info UpdatePersonId = new material_request_info();
                
                UpdatePersonId.id = Convert.ToInt32(select_id);
                UpdatePersonId.take_person_id = res.id;
                UpdatePersonId.take_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                MRIM.Update(UpdatePersonId);
                return RedirectToAction(nameof(Index));
                //return Redirect("/MaterialConfirm/Index");
            }
        }
        /// <summary>
        /// 配获完成
        /// </summary>
        /// <param name="request_id"></param>
        /// <returns></returns>
        public IActionResult MaFinish(string request_id)
        {
            //首先根据id判断对应的人员是否为空
            material_request_info results = MRIM.SelectById(Convert.ToInt32(request_id));
            if (results.take_person_id > 0)
            {
                results.depot_ack_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                //若不为空则更新数据
                int tmp = MRIM.Update(results);
                
                List<webaccess_tag_info> tagList = new List<webaccess_tag_info>();
                tagList = TIM.GetWaTagAndTypeInfo(TagAreaAttributeEnum.station_info, results.station_id, "andon_ack_code");
                string TagValue = request_id + "&M&M";//物料呼叫解除的格式
                
                if (ScadaAPIConfig.EnableScadaApi)
                {
                    //ScadaAPIHelper.WriteValue();
                }
                else
                {
                    // MqttManager.MqttHelper.WriteTagValueMsgToMqtt(tagList[0].tag_code, TagValue, true);//向webaccess写入消息，用于异常监控的解除
                }
                if (tmp > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            else
            {
                return Json("Fail");
            }
        }

        /// <summary>
        /// 从cookie中获取配置信息
        /// </summary>
        /// <returns></returns>
        private PageInfo GetSearchInfo1()
        {
            PageInfo pageInfo = new PageInfo();
            pageInfo.start_time = Request.Cookies["Andon_start_time1"];
            pageInfo.end_time = Request.Cookies["Andon_end_time1"];
            pageInfo.material_name = Request.Cookies["Andon_material_name1"];
            return pageInfo;
        }

        private PageInfo GetSearchInfo2()
        {
            PageInfo pageInfo = new PageInfo();
            pageInfo.start_time = Request.Cookies["Andon_start_time2"];
            pageInfo.end_time = Request.Cookies["Andon_end_time2"];
            pageInfo.material_name = Request.Cookies["Andon_material_name2"];
            return pageInfo;
        }

    }
}