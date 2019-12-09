using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;
using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonHelper.ScadaAPI;

namespace WebApplication.Controllers
{
    public class MaterialRequestController : BaseController
    {
        int mode = 0; //0：非班长模式 1：班长模式
        MaterialInfoManager MIM = new MaterialInfoManager();
        ClientConfigInfoManager CCIM = new ClientConfigInfoManager();
        StationManager SM = new StationManager();
        MaterialRequestInfoManager MRIM = new MaterialRequestInfoManager();
        PersonManager PM = new PersonManager();        
        LineInfoManager LIM = new LineInfoManager();
        //MqttClientHelper MqttClient = new MqttClientHelper();
        TagInfoManager TIM = new TagInfoManager();
        LineInfoManager LM = new LineInfoManager();

        /// <summary>
        /// 物料呼叫首页
        /// </summary>
        /// <param name="material_name">用于物料的模糊查询（代码/名称）</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public IActionResult Index(string material_name,string cookies, int pageindex = 1, int pagesize = 15)
        {
             string station_id_str = Request.Cookies["Andon_station_id"];
            if (station_id_str != null)
            {
                client_config_info ci = GetLineAndStation();
                string line_name = LM.SelectSingle(ci.line_id).line_name_en;
                ViewBag.Line = line_name;

                List<material_info> result = new List<material_info>();
                if (mode == 0)
                {
                    //非班长模式
                    //获取当前站位,将单一结果放入list中便于与前端班长模式一致
                    station_info station_Infos = SM.SelectSingle(ci.station_id);
                    List<station_info> stations = new List<station_info>();
                    stations.Add(station_Infos);

                    //下拉框站位
                    ViewBag.Station = stations;
                    //显示本站所有物料呼叫
                    List<RequestAndInfo> results1 = MRIM.SelectRequestAndInfoSatation(ci.station_id);
                    ViewBag.Material_Request_Info = results1;
                }
                else
                {
                    //班长模式
                    //获取该线所有站位
                    List<station_info> station_Infos = SM.SelectByLine(ci.line_id);
                    //下拉框站位
                    ViewBag.Station = station_Infos;
                    //显示所有物料呼叫
                    List<RequestAndInfo> results1 = MRIM.SelectRequestAndInfoAll();
                    ViewBag.Material_Request_Info = results1;
                }


                //当提交查询表单时，将start_time end_time material_name这三个参数保存到cookies中
                if (!string.IsNullOrEmpty(cookies))
                {
                    //将信息存入到cookies中
                    //HttpCookie cok = new HttpCookie();
                    DateTimeOffset dto = new DateTimeOffset(DateTime.Now.AddHours(GlobalDefine.SysTimeZone));
                    dto = dto.AddDays(1);
                    CookieOptions co = new CookieOptions();
                    co.Expires = dto;    //设置cookies保存的时间 这边设定为30天
                    if (string.IsNullOrEmpty(material_name)) { material_name = ""; }
                    Response.Cookies.Append("Andon_material_name0", material_name, co);
                }
                PageInfo pageInfo = GetSearchInfo();

                //记录用户输入的查询信息,response和request存在时间差，即当前response添加的值，request获取不到，只能在下一次才能获取
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
                //显示所有库存物料
                List<material_info> results = MIM.SelectAll(ViewBag.material_name);
                var pagedList = PagedList<material_info>.PageList(pageindex, pagesize, results);
                ViewBag.model = pagedList.Item2;
                result = pagedList.Item1;
                return View(result);
            }
            else
            {
                Response.ContentType = "charset=utf-8";//不写的话会导致中文乱码
                Response.WriteAsync("<script type='text/javascript'>alert('请先配置站位信息');location.href='/Configuration/Index';</script>"); //如果没设定站位 则需要先设定站位
                return View();
            }
        }

        /// <summary>
        /// AJAX操作，对用户呼叫的数量进行管控
        /// </summary>
        /// <param name="number">呼叫量</param>
        /// <param name="material_inventory">库存量</param>
        /// <param name="id">呼叫的物料对应的id</param>
        /// <param name="station_id">呼叫站位id</param>
        /// <returns></returns>
        public IActionResult RequestInfo(string number,string material_inventory, string id, string station_id)
        {
            //用于判断输入数量是否为整数
            int tmp;           
          
            if (int.TryParse(number, out tmp))
            {
                //呼叫数量
               int Number = Convert.ToInt32(number);
               int real_num = Convert.ToInt32(material_inventory);
               int request_id = Convert.ToInt32(id);
                //判断请求数量和仓库实际数量关系
                if (Number > real_num || Number<0)
                {
                    return Json("Fail");
                }
                else
                {
                    material_info mi = MIM.SelectSingle(request_id);

                    //将提交的数量传到webaccess中
                    List<webaccess_tag_info> tagList = new List<webaccess_tag_info>();
                    tagList = TIM.GetWaTagAndTypeInfo(TagAreaAttributeEnum.station_info, Convert.ToInt32(station_id), "material_require");
                    if (tagList.Count == 0)
                    {
                        Response.WriteAsync("<script type='text/javascript'>alert('There is no Tag');history.back();</script>");
                        return Json("Fail");
                    }
                    else
                    {
                        if(MRIM.AddMaterialRequest(request_id, Number, Convert.ToInt32(station_id)))//数据库插入数量
                        {
                            string TagValue = Number.ToString() + "&" + id.ToString();
                            //将呼叫的数量发送至webaccess
                            if (ScadaAPIConfig.EnableScadaApi)
                            {
                                ScadaAPIHelper.WriteValueAsync(tagList[0].tag_code, TagValue);
                            }
                            else
                            {
                                //MqttManager.MqttHelper.WriteTagValueMsgToMqtt(tagList[0].tag_code, TagValue, true);//写值
                            }
                        }
                        //更新material_info信息
                        int count = 0;
                        //当呼叫的数量小于库存数量则更新
                        if (real_num - Number > 0)
                        {
                            mi.id = request_id;
                            mi.material_inventory = real_num - Number;
                            count = MIM.Update(mi);
                        }
                        //当呼叫的数量等于库存数量则删除
                        else
                        {
                            count = MIM.Del(request_id);
                        }
                        //数据库操作成功与否
                        if (count > 0)
                        {
                            return Json("Success");
                        }
                        else
                        {
                            return Json("Fail");
                        }
                    }                                       
                }
            }
            else
            {
                return Json("Fail");
            }
        }
        /// <summary>
        /// 已呼叫列表局部刷新
        /// </summary>
        /// <returns></returns>
        public IActionResult Refresh()
        {
            client_config_info ci = GetLineAndStation();
            List<RequestAndInfo> results = new List<RequestAndInfo>();
            if (mode == 0)
            {
                //非班长模式
                //显示本站所有物料呼叫
                results = MRIM.SelectRequestAndInfoSatation(ci.station_id);
            }
            else
            {
                //班长模式
                //显示所有物料呼叫
                results = MRIM.SelectRequestAndInfoAll();
            }
            return View(results);
        }
            
        /// <summary>
        /// 从cookie中获取配置信息
        /// </summary>
        /// <returns></returns>
        private client_config_info GetLineAndStation()
        {
            client_config_info re = new client_config_info();
            re.plant_id = Convert.ToInt32(Request.Cookies["Andon_plant_id"]);
            re.unit_no = Request.Cookies["Andon_unit_id"];
            re.line_id = Convert.ToInt32(Request.Cookies["Andon_line_id"]);
            re.station_id = Convert.ToInt32(Request.Cookies["Andon_station_id"]);
            re.machine_code = Request.Cookies["Andon_machine_id"];
            return re;
        }

        private PageInfo GetSearchInfo()
        {
            PageInfo pageInfo = new PageInfo();
            pageInfo.material_name = Request.Cookies["Andon_material_name0"];
            return pageInfo;
        }


    }
}