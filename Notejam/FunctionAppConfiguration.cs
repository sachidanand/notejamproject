namespace Notejam.Api
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    internal class FunctionAppConfiguration : IConfiguration
    {
        public FunctionAppConfiguration()
          : this("local.settings.json")
        {
        }

        internal FunctionAppConfiguration(string settingsFile)
        {
            var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(settingsFile, true, false)
            .AddEnvironmentVariables();

            var configurationRoot = configurationBuilder.Build();

            ConfigurationRoot = configurationBuilder.Build();
            ConfigurationRoot.Bind(new AzureKeyVaultConfigurationMapping(this));
        }
        public IConfigurationRoot ConfigurationRoot { get; }

        public Uri CosmosDatabaseEndpointUrl { get; set; }

        public string CosmosDatabaseAuthorizationKey { get; set; }

        public string CosmosDatabaseId { get; set; }

        public string ApplicationInsightsInstrumentationKey { get; set; }
        public string CosmosCollectionName { get; set; }
    }

    internal sealed class AzureKeyVaultConfigurationMapping
    {
        private const string NotAvailable = "N/A";

        private readonly FunctionAppConfiguration _parent;

        public AzureKeyVaultConfigurationMapping(FunctionAppConfiguration parent)
        {
            _parent = parent;
        }

        public Uri CosmosDatabaseAccount_Uri
        {
            get => null;
            set => _parent.CosmosDatabaseEndpointUrl = value;
        }
        public string ApplicationInsights_InstrumentationKey
        {
            get => NotAvailable;
            set => _parent.ApplicationInsightsInstrumentationKey = value;
        }
        public string CosmosDB_DatabaseId
        {
            get => NotAvailable;
            set => _parent.CosmosDatabaseId = value;
        }

        public string CosmosDB_Key
        {
            get => NotAvailable;
            set => _parent.CosmosDatabaseAuthorizationKey = value;
        }
        public string CosmosDB_Collection
        {
            get => NotAvailable;
            set => _parent.CosmosCollectionName = value;
        }
    }
}
