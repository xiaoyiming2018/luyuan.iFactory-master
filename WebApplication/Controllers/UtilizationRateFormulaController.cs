using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Advantech.IFactory.CommonLibrary;

namespace WebApplication.Controllers
{
    public class UtilizationRateFormulaController : BaseController
    {
        UtilizationRateFormulaManager URFM = new UtilizationRateFormulaManager();
        SystemTagCodeManager STCM = new SystemTagCodeManager();
        SystemTagTypeManager STTM = new SystemTagTypeManager();
        DataReplaceManager DM = new DataReplaceManager();
        SharedManager SM = new SharedManager();
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            UtilizationRateFormula obj = URFM.SelectSingle();
            if (obj != null)
            {
                ViewBag.id = obj.id;
                return View(obj);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditHandle()
        {
            try
            {
                int id = 0;
                int count = 0;
                if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
                {
                    id = Convert.ToInt32(HttpContext.Request.Form["id"]);
                }
                UtilizationRateFormula obj = new UtilizationRateFormula();

                obj.run_time_formula = HttpContext.Request.Form["run_time_formula"];
                obj.error_time_formula = HttpContext.Request.Form["error_time_formula"];
                obj.others_time_formula = HttpContext.Request.Form["others_time_formula"];
                obj.boot_time_formula = HttpContext.Request.Form["boot_time_formula"];

                if (id > 0)
                {
                    obj.id = id;
                    count = URFM.Update(obj);
                }
                else
                {
                    count = URFM.Insert(obj);
                }
                if (count > 0)
                {                    
                    return View("Views/UtilizationRateFormula/alert.cshtml");
                }
                else
                {
                    return Content("<script>alert('Failure to execute');history.go(-1)</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult Verification()
        {
            int id = 0;
            string run_time_formula = HttpContext.Request.Form["run_time_formula"];
            string error_time_formula = HttpContext.Request.Form["error_time_formula"];
            string others_time_formula = HttpContext.Request.Form["others_time_formula"].ToString();
            string boot_time_formula = HttpContext.Request.Form["boot_time_formula"];
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }

            List<string> formulaList = new List<string>();
            formulaList.Add(run_time_formula);
            formulaList.Add(error_time_formula);
            formulaList.Add(others_time_formula);
            formulaList.Add(boot_time_formula);

            //返回结构
            string stringResult = null;
            //公式名称
            string formulaName = null;
            for (int i = 0; i < formulaList.Count; i++)
            {
                //判断是那个公式有问题
                switch (i)
                {
                    case 0:
                        formulaName = "运行时间公式";
                        break;
                    case 1:
                        formulaName = "异常时间公式";
                        break;
                    case 2:
                        formulaName = "其他时间公式";
                        break;
                    case 3:
                        formulaName = "开机时间公式";
                        break;
                }

                //参数列表
                List<string> parameterList = new List<string>();
                //公式
                string formula = formulaList[i];
                //判断公式有否存在问题，并返回参数列表
                bool result = TrueOrFalse(formula, formulaName, ref parameterList);
                if (result)
                {
                    if (parameterList.Count > 0)
                    {
                        for (int j = 0; j < parameterList.Count; j++)
                        {
                            int randomNum = SM.GetRandom();
                            formula = DM.ReplaceFormula("$" + parameterList[j], randomNum, formula);
                        }

                        bool getResult = DM.GetFormula(formula);
                        if (getResult)
                        {
                            stringResult = "Success";
                        }
                        else
                        {
                            stringResult = formulaName;
                            break;
                        }
                    }
                    else
                    {
                        //开机时间可以不设定
                        if (formulaName == "BootTimeFormula")
                        {
                            stringResult = "Success";
                        }
                        else
                        {
                            stringResult = formulaName;
                            break;
                        }
                    }
                }
                else
                {
                    stringResult = formulaName;
                    break;
                }

                //如果异常则跳出循环
                if (!string.IsNullOrEmpty(stringResult))
                {
                    if (stringResult != "Success")
                    {
                        break;
                    }
                }
            }
            return Json(stringResult);
        }


        /// <summary>
        /// 判断公式是否正确（是否有$符号，是否在Lamp List中有存在）
        /// </summary>
        /// <param name="formula">公式</param>
        /// <param name="formula">时间类型</param>
        /// <param name="parameterList"></param>
        /// <returns></returns>
        public bool TrueOrFalse(string formula, string formulaName, ref List<string> parameterList)
        {
            bool result = false;
            try
            {
                parameterList = DM.GetParameterList(formula);

                if (parameterList.Count > 0)
                {
                    for (int i = 0; i < parameterList.Count; i++)
                    {

                        system_tag_code_web res = STCM.SelectOne(parameterList[i]);
                        System_tag_type obj = STTM.SeclectOneById(res.type_id);
                        if (obj != null)
                        {
                            //tag属于稼动率运算的返回true
                            if (obj.type_name_en == "UtilizationRate" || obj.type_name_en == "Error")
                            {
                                result = true;
                            }
                            else
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                            break;
                        }

                    }
                }
                else
                {
                    //如果数据为空
                    if (string.IsNullOrEmpty(formula))
                    {
                        //并且开机时间未设定，则可以通过验证
                        if (formulaName == "BootTimeFormula")
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }

                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}