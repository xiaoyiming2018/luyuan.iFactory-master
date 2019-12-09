using System;

using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class RestTimeRowController : BaseController
    {
        RestTimeRowManager RTRM = new RestTimeRowManager();
        SharedManager SM = new SharedManager();
        MachineInfoManager MIM = new MachineInfoManager();
      //  TagInfoManager TIM = new TagInfoManager();
        public ActionResult Index(string start_time,string end_time, int pageindex = 1, int pagesize = 15)
        {
            //string start_time = Request.Query["start_time"];
            //string end_time = Request.Query["end_time"];
            if (!string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
            {
                var objList = RTRM.SelectAll(start_time: start_time, end_time: end_time);
                var pagedList = PagedList<rest_time_row>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = RTRM.SelectAll();
                var pagedList = PagedList<rest_time_row>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
        }

        public ActionResult Edit()
        {
             return View();

        }

        //public ActionResult EditHandle()
        //{
        //    HttpPostedFileBase file = Request.Files["file_address"];
        //    if (file == null || file.ContentLength <= 0)
        //    {
        //        return Content("<script>alert('Unselected files or files are empty');history.go(-1)</script>");
        //    }
        //    else
        //    {
        //        string filename = Path.GetFileName(file.FileName);
        //        string savePath = Server.MapPath("~/upload/");
        //        //检查服务器上是否存在这个物理路径，如果不存在则创建 
        //        if (!System.IO.Directory.Exists(savePath))
        //        {
        //            //需要注意的是，需要对这个物理路径有足够的权限，否则会报错 
        //            //另外，这个路径应该是在网站之下，而将网站部署在C盘却把上传文件保存在D盘 
        //            System.IO.Directory.CreateDirectory(savePath);
        //        }
        //        savePath = savePath + "\\" + filename;
        //        file.SaveAs(savePath);//保存文件 
        //        DataTable dt = new DataTable();
        //        bool result=true;
        //        dt = SM.OpenCSV(savePath, ref result);
        //        if (dt.Rows.Count > 0 && result)
        //        {
        //            Thread ThreadTHandle = new Thread(Handle);
        //            ThreadTHandle.Start(dt);

        //            return Content("<script>alert('Background processing');window.location = '/RestTimeRow/Index';</script>");
        //        }
        //        else
        //        {
        //            return Content("<script>alert('Unselected files or files are empty');history.go(-1)</script>");
        //        }
        //    }
        //}

        public ActionResult Del()
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            int count = RTRM.Del(id);
            if (count > 0)
            {
                return View("Views/RestTimeRow/alert.cshtml");
            }
            else
            {
                return Content("<script>alert('Failure to execute');history.go(-1)</script>");
            }
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult AddHandle()
        {
            try
            {
                rest_time_row obj = new rest_time_row();
                obj.machine_code = HttpContext.Request.Form["machine_code"];
                obj.start_time = Convert.ToDateTime(HttpContext.Request.Form["start_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj.end_time = Convert.ToDateTime(HttpContext.Request.Form["end_time"]).ToString("yyyy-MM-dd HH:mm:ss");
                obj.tag_code = HttpContext.Request.Form["tag_code"];
                obj.remarks = HttpContext.Request.Form["remarks"];
                int count = 0;
                rest_time_row objRestTimeRow = RTRM.IsUpdate(obj.machine_code, obj.start_time, obj.end_time);
                if (objRestTimeRow == null)
                {

                   count= RTRM.Insert(obj);
                }
                else
                {
                    obj.id = objRestTimeRow.id;
                   count= RTRM.Update(obj);
                }
                if(count>0)
                {
                    return View("Views/RestTimeRow/alert.cshtml");
                }
                else
                {
                    return Content("<script>alert('Failure to execute');history.go(-1)</script>");
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //public void Handle(object objDt)
        //{
        //    DataTable dt = (DataTable)objDt;
        //    for(int i=0;i<dt.Rows.Count;i++)
        //    {
        //        try
        //        {
        //            rest_time_row obj = new rest_time_row();
        //            obj.machine_code = dt.Rows[i][0].ToString();
        //            obj.start_time = Convert.ToDateTime(dt.Rows[i][1]).ToString("yyyy-MM-dd HH:mm:ss");
        //            obj.end_time = Convert.ToDateTime(dt.Rows[i][2]).ToString("yyyy-MM-dd HH:mm:ss");
        //            obj.tag_code = dt.Rows[i][3].ToString();
        //            obj.remarks = dt.Rows[i][4].ToString();
        //            if (!string.IsNullOrEmpty(obj.machine_code) && !string.IsNullOrEmpty(obj.start_time) && !string.IsNullOrEmpty(obj.end_time))
        //            {
        //                bool result = false;
        //                if(string.IsNullOrEmpty(obj.tag_code))
        //                {
        //                    result = true;
        //                }
        //                else
        //                {
        //                    TagInfo objTagInfo = TIM.SelectSingle(tag_code: obj.tag_code);
        //                    if (objTagInfo != null)
        //                    {
        //                        //tag属于稼动率运算的返回true
        //                        if (objTagInfo.type_id == 4 || objTagInfo.type_id == 1)
        //                        {
        //                            result = true;
        //                        }
        //                        else
        //                        {
        //                            result = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        result = false;
        //                    }
        //                }
        //                MachineInfo objMachineInfo = MIM.SelectSingle(machine_code: obj.machine_code);
        //                if (objMachineInfo != null && result)
        //                {
        //                    rest_time_row objRestTimeRow = RTRM.IsUpdate(obj.machine_code, obj.start_time, obj.end_time);
        //                    if (objRestTimeRow == null)
        //                    {
        //                        RTRM.Insert(obj);
        //                    }
        //                    else
        //                    {
        //                        obj.id=objRestTimeRow.id;
        //                        RTRM.Update(obj);
        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}
	}
}