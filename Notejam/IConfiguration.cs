using System;
using System.Collections.Generic;
using System.Text;

namespace Notejam.Api
{
    public interface IConfiguration
    {
        Uri CosmosDatabaseEndpointUrl { get; }

        string CosmosDatabaseAuthorizationKey { get; }

        string CosmosDatabaseId { get; }

        string ApplicationInsightsInstrumentationKey { get; }
        string CosmosCollectionName { get; }
    }
}
