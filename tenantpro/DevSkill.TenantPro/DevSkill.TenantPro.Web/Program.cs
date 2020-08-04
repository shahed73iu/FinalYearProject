using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;

namespace DevSkill.TenantPro.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
                                  .Build()
                                  .GetConnectionString("DefaultConnection");
            const string logFilePath = "Logs.txt";
            const string tableName = "Logs";
            var columnOption = new ColumnOptions();

            var emailInfo = new EmailConnectionInfo
            {
                FromEmail = "hashtag626768@gmail.com",
                ToEmail = "hashtag626768@gmail.com",
                MailServer = "smtp.gmail.com",
                EmailSubject = "An Log Error Occured in Tenant Pro Project, Please check it",
                Port = 465,
                EnableSsl = true,
                NetworkCredentials = new NetworkCredential("Enter your email here", "enter mail password")
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(connectionString,
                                    tableName,
                                    columnOptions: columnOption,
                                    autoCreateSqlTable: true)
                .WriteTo.Email(emailInfo,
                            outputTemplate: "Time : {Timestamp:HH:mm:ss}\t\n{Level:u3}\t\n" +
                            "{SourceContext}\t\n{Message}{NewLine}{Exception}",
                            LogEventLevel.Fatal,
                            1)
                .WriteTo.File(logFilePath,LogEventLevel.Error)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
