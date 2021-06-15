using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Notejam.Api;
using Notejam.Api.Persistance;
using System;
using Notejam.Api.Business;

[assembly: WebJobsStartup(typeof(Notejam.Api.Startup))]

namespace Notejam.Api
{

    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            RegisterServices(builder.Services);
        }
        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration, FunctionAppConfiguration>();
            services.AddScoped<INotesManagement, NotesManagement>();
            services.AddSingleton(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
            services.AddSingleton<INotesRepository, NotesRepository>();
            RegisterDepedentClientOfService(services);
        }

        private void RegisterDepedentClientOfService(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
 

            services.AddSingleton<IDocumentClient>(_ => new DocumentClient(
                                                                            config.CosmosDatabaseEndpointUrl,
                                                                            config.CosmosDatabaseAuthorizationKey,
                                                                            new ConnectionPolicy
                                                                            {
                                                                                ConnectionMode = ConnectionMode.Direct,
                                                                                ConnectionProtocol = Protocol.Tcp,
                                                                            }));
        }

    }
}
