namespace Notejam.Api.Persistance
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDocumentRepository<T> : IDisposable
        where T : class
    {
        Task<T> GetItemByQueryAsync(string databaseQuery);

        Task<IEnumerable<T>> GetAsync(string databaseQuery);

        Task<string> CreateAsync(T value);

        Task<string> UpdateAsync(string id, T value);

        Task<bool> DeleteByPartitionAsync(string id, string partitionKey);
    }
}
