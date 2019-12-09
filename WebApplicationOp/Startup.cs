using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonLibrary;
using iFactory.Op.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplicationOp
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

                //计算的间隔时间
                //Advantech.IFactory.MachineStatusCollect.DataTask.CalSetTime = Configuration.GetValue<string>("setTime");
                //拉取系统所在的时区
                AreaInfoManager areaInfoManager = new AreaInfoManager();
                var list = areaInfoManager.SelectAll();
                if (list != null && list.Count > 0)
                {
                    GlobalDefine.SysTimeZone = list[0].time_zone;//获取系统设定的时区
                }

                //此部分需要优化！！！！！！！！！
                //GlobalCfgData.SysTimeZone = GlobalDefine.SysTimeZone;

            }
            catch (Exception ex)
            {
                Console.WriteLine("ex.message=" + ex.Message);
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            //添加session

            services.AddDistributedMemoryCache();
            services.AddSession();
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromDays(30);// 设置 Session 过期时间
            //    options.Cookie.Domain = "";
            //});

            services.AddMvc();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //注入Session工厂类接口

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddTransient<IDataService<pro_schedule>, ProScheduleService>();//注入工单服务
            services.AddTransient<IDataService<Pro_schedule_machine>, ProScheduleMachineService>();//注入工位工单服务
            services.AddTransient<IDataService<CtAveraged>, CtAveragedService>(); // 注入节拍时间服务
            services.AddHostedService<BackgroundTask>();//注册后台周期服务
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

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
