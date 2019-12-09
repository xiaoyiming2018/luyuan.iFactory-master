using System;
using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Advantech.IFactory.CommonHelper.ScadaAPI;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            string connString = "";
            try
            {        
                connString = Configuration.GetValue<string>("connString");
                PostgreBase.connString = connString;

                //mongoDB数据库配置
                string MongoDBConnString = Configuration.GetValue<string>("MongoDBConnString");
                MongoDBHelper.connectionstring = MongoDBConnString;
                string MongoDBDataBase = Configuration.GetValue<string>("MongoDBDataBase");
                MongoDBHelper.databaseName = MongoDBDataBase;

                //异常呼叫中按钮是否显示
                string ExceptionShow = Configuration.GetValue<string>("ExceptionShow");

                //Scada API配置信息
                ScadaAPIConfig.ScadaID = Configuration.GetValue<string>("ScadaID");
                ScadaAPIConfig.EnableScadaApi = Configuration.GetValue<bool>("EnableScadaApi");

                GlobalDefine.IsLocalMode= Configuration.GetValue<bool>("IsLocalMode");
                GlobalDefine.MongoDBLogKeepDays= Configuration.GetValue<int>("MongoDBLogKeepDays");
                GlobalDefine.LogTableKeepDays = Configuration.GetValue<int>("PostgelLogKeepDays");
                GlobalDefine.LineBalanceRateKeepDays = Configuration.GetValue<int>("LineBalanceRateKeepDays");

                //拉取系统所在的时区
                AreaInfoManager areaInfoManager = new AreaInfoManager();
                var list = areaInfoManager.SelectAll();
                if (list != null && list.Count > 0)
                {
                    GlobalDefine.SysTimeZone = list[0].time_zone;//获取系统设定的时区
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ex.message="+ex.Message);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加session

            services.AddDistributedMemoryCache();

            services.AddSession();

            services.AddMvc();
            
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //注入Session工厂类接口

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSession(); //必须加上这句才能用session
            ServiceLocator.Instance = app.ApplicationServices;
            if (env.IsDevelopment() || env.IsStaging())//IsStaging为中间环境，IsDevelopment为开发环境
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseDeveloperExceptionPage();//始终以开发模式

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Login}/{id?}");
            });
        }
    }
}
