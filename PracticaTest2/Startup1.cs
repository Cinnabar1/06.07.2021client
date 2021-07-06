using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Hangfire;
using System.Web;
using Hangfire.SqlServer;



[assembly: OwinStartup(typeof(PracticaTest2.Startup1))]

namespace PracticaTest2
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(@"Server=.\\SQLEXPRESS;Database=Test;Trusted_Connection=True;");
            //basic process to check
            BackgroundJob.Enqueue(() => Console.WriteLine("Getting Started with HangFire!"));

            //will create hangfire dashboard
            app.UseHangfireDashboard();
            app.UseHangfireServer();

            // Дополнительные сведения о настройке приложения см. на странице https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}

