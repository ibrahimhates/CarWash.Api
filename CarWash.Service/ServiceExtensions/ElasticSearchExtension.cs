using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace CarWash.Service.ServiceExtensions
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticConfiguration:Uri"];
            var username = configuration["ElasticConfiguration:Username"];
            var password = configuration["ElasticConfiguration:Password"];
            var index = configuration["ElasticConfiguration:DefaultIndex"];

            var settings = new ConnectionSettings(new Uri(baseUrl ?? ""));
            //Production'da Kaldırılacak @Turan @TURAN @turan
            settings.ServerCertificateValidationCallback(CertificateValidations.AllowAll);

            settings.EnableApiVersioningHeader();
            settings = settings.BasicAuthentication(username, password);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);


            CreateIndex(client, index);
        }
        private static void CreateIndex(ElasticClient client, string indexName)
        {
            if (!client.Indices.Exists(indexName).Exists)
            {
                var response = client.Indices.Create(indexName);
                client.Bulk(b => b.Index(indexName));
            }
        }
    }
}
