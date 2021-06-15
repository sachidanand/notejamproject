namespace Notejam.Api.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using EnsureThat;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Microsoft.Extensions.Logging;
    using Polly;

    public class DocumentRepository<T> : IDocumentRepository<T>
        where T : class
    {
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly ILogger<DocumentRepository<T>> _logger;
        private readonly IAsyncPolicy _retryPolicy;
        private IDocumentClient _client;

        public DocumentRepository(IDocumentClient client, IConfiguration configuration, ILogger<DocumentRepository<T>> logger)
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));
            EnsureArg.IsNotNull(logger, nameof(logger));

            _logger = logger;

            _databaseId = configuration.CosmosDatabaseId;
            _collectionId = configuration.CosmosCollectionName;
            var maxRetryAttempt = Convert.ToInt32(3, CultureInfo.InvariantCulture);
            var pauseTimeBetweenFailures = Convert.ToInt32(3, CultureInfo.InvariantCulture);

            _client = client;

            _retryPolicy = Policy
                       .Handle<System.Net.Sockets.SocketException>()
                       .Or<System.Net.Http.HttpRequestException>()
                       .Or<DocumentClientException>(_ => _.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                       .Or<DocumentClientException>(_ => _.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                       .Or<OperationCanceledException>()
                       .WaitAndRetryAsync(
                           maxRetryAttempt,
                           _ => TimeSpan.FromSeconds(pauseTimeBetweenFailures),
                           (exception, _, retryCount, context) =>
                            {
                                _logger.LogError("Retry Triggered Due To Some Exception: Retry Count Is: " + retryCount);
                                _logger.LogError(exception, exception.Message);
                            });
        }

        public async Task<T> GetItemByQueryAsync(string databaseQuery)
        {
            try
            {
                var results = new List<T>();
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var sql = new SqlQuerySpec(databaseQuery);
                    var query = _client.CreateDocumentQuery<T>(
                        UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), sql, new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                        .AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>().ConfigureAwait(false));
                    }
                }).ConfigureAwait(false);

                if (results.Count == 0)
                {
                    return null;
                }

                return results[0];
            }
            catch (DocumentClientException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAsync(string databaseQuery)
        {
            try
            {
                var results = new List<T>();
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var sql = new SqlQuerySpec(databaseQuery);

                    var query = _client.CreateDocumentQuery<T>(
                        UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                        sql,
                        new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true }).AsDocumentQuery();

                    while (query.HasMoreResults)
                    {
                        results.AddRange(await query.ExecuteNextAsync<T>().ConfigureAwait(false));
                    }
                }).ConfigureAwait(false);

                if (results.Count == 0)
                {
                    return await Task.FromResult<IEnumerable<T>>(null).ConfigureAwait(false);
                }

                return results;
            }
            catch (DocumentClientException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<string> CreateAsync(T value)
        {
            try
            {
                Document document;
                var createResult = await _retryPolicy.ExecuteAsync(async () =>
                {
                    document = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), value).ConfigureAwait(false);
                    return document;
                }).ConfigureAwait(false);

                return createResult?.Id;
            }
            catch (DocumentClientException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<string> UpdateAsync(string id, T value)
        {
            try
            {
                Document document;
                var updateResult = await _retryPolicy.ExecuteAsync(async () =>
                {
                    document = await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), value).ConfigureAwait(false);
                    return document;
                }).ConfigureAwait(false);

                return updateResult?.Id;
            }
            catch (DocumentClientException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<bool> DeleteByPartitionAsync(string id, string partitionKey)
        {
            try
            {
                Document document;
                var deleteResult = await _retryPolicy.ExecuteAsync(async () =>
                {
                    document = await _client.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(_databaseId, _collectionId, id),
                    new RequestOptions { PartitionKey = new PartitionKey(partitionKey) }).ConfigureAwait(false);
                    return true;
                }).ConfigureAwait(false);

                return deleteResult;
            }
            catch (DocumentClientException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public void Dispose()
        {
            _client = null;
        }
    }
}
