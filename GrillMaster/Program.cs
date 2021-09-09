using ApplicationCore;
using AutoMapper;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrillMaster
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = AppStartup();
            try
            {
                Log.Information("Creating instance GrillMenuRequestService");
                var service = ActivatorUtilities.CreateInstance<GrillMenuRequestService>(host.Services);

                Log.Information("Creating instance Mapper");
                var mapper = ActivatorUtilities.CreateInstance<Mapper>(host.Services);

                Log.Information("Creating instance RoundGeneratorService");
                var roundGenerator = ActivatorUtilities.CreateInstance<RoundGeneratorService>(host.Services);

                Log.Information("Requesting data from API");
                IReadOnlyList<GrillMenu> grillMenus = await service.GetAllAsync();


                Log.Information("Generating Models from serialized data");
                var grillMenusModels = mapper.Map<IEnumerable<GrillMenuModel>>(grillMenus);                
                var orderedMenus = grillMenusModels.OrderBy(x => x.Menu);

                foreach (var menu in orderedMenus)
                {
                    Log.Information($"Generating round of menu {menu.Menu}");
                    menu.Rounds = await roundGenerator.GenerateRounds(menu);

                    PrintDataInConsole(menu);
                }

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// It show the data that we need from the menu
        /// </summary>
        /// <param name="menu">menu with the rounds that we need to create</param>
        private static void PrintDataInConsole(GrillMenuModel menu)
        {
            Console.WriteLine($"{menu.Menu}: {menu.Rounds.Count} rounds");

            Log.Debug($"{menu.Menu} has {menu.Items.Sum(x => x.Quantity)} items");
            for (int i = 0; i < menu.Rounds.Count; i++)
            {
                Log.Debug($"Round {i + 1} has {menu.Rounds[i].GrillRoundStrips.Count} strips");
                for (int j = 0; j < menu.Rounds[i].GrillRoundStrips.Count; j++)
                {
                    Log.Debug($"Strip {j + 1} has {menu.Rounds[i].GrillRoundStrips[j].GrillMenuItems.Count} items," +
                        $" maxHeight= {menu.Rounds[i].GrillRoundStrips[j].Height} and maxWidth {menu.Rounds[i].GrillRoundStrips[j].Width}");
                    for (int k = 0; k < menu.Rounds[i].GrillRoundStrips[j].GrillMenuItems.Count; k++)
                    {
                        Log.Debug($"Item {k + 1} height {menu.Rounds[i].GrillRoundStrips[j].GrillMenuItems[k].Height} " +
                            $"and width {menu.Rounds[i].GrillRoundStrips[j].GrillMenuItems[k].Width}");
                    }
                }
            }
            Log.Debug("--------------------------------------------------------------------------");
            Log.Debug("--------------------------------------------------------------------------");
        }

        private static void ConfigSetup(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>return <see cref="IHost"/> with all the services loaded</returns>
        private static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();
            ConfigSetup(builder);
            var configuration = builder.Build();

            //Load settings from appsettings.json
            GrillSettings settings = new GrillSettings();
            configuration.GetSection(GrillSettings.SECTION).Bind(settings);


            //Config Serilog to sink data into Console
            var loggerConf = new LoggerConfiguration()
             .ReadFrom.Configuration(configuration)
             .Enrich.FromLogContext()
             .WriteTo.Console();

            if (settings.IsDev)
                loggerConf.MinimumLevel.Debug();
            else
                loggerConf.MinimumLevel.Error();


            Log.Logger = loggerConf.CreateLogger();


            // Add services to current host
            var host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) =>
                        {
                            services.AddHttpClient(Client.DEFAULT, client =>
                            {
                                HttpClientHelper.AddHttpBaseClientConfig(client, settings.BaseURL);
                            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));

                            services.AddAutoMapper(x => x.AddProfile(typeof(GrillProfile)));

                            services.AddSingleton(typeof(IGrillMenuRequestService), typeof(GrillMenuRequestService));
                            services.AddSingleton(typeof(IAsyncRoundGenerator<GrillMenuModel, GrillRoundModel>), typeof(RoundGeneratorService));
                        })
                        .UseSerilog()
                        .Build();



            return host;
        }

       
    }
}
