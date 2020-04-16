using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace testapi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication("DefaultAuth");
                

            services.AddAuthorization(config =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //  .Build();

                //config.DefaultPolicy = defaultAuthPolicy;

            });

            //services.AddHttpClient()
            //    .AddHttpContextAccessor();

            //services.AddControllers();



            string structTest = string.Format("https://jsonplaceholder.typicode.com/users");
            WebRequest requestobject = WebRequest.Create(structTest);
            requestobject.Method = "GET";
            HttpWebResponse responseObject = null;
            responseObject = (HttpWebResponse)requestobject.GetResponse();

            string testResult = null;
            using (Stream stream = responseObject.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                testResult = sr.ReadToEnd();
                sr.Close();
            }

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

            });
        }
    }
}
