using System.Threading;
using MongoDB.Driver;
using Solari.Callisto.Abstractions.Contracts.CQR;
using Solari.Callisto.Abstractions.Validators;

namespace Solari.Callisto.Abstractions.CQR
{
    public class DefaultCallistoQuery<T> : ICallistoQuery<T> where T : class, IDocumentRoot
    {
        public DefaultCallistoQuery(string operationName, FilterDefinition<T> filterDefinition ,FindOptions<T> findOptions)
        {
            OperationName = operationName;
            FindOptions = findOptions;
            FilterDefinition = filterDefinition;
        }

        public string OperationName { get; }
        public CancellationToken CancellationToken { get; set; }
        public IClientSessionHandle ClientSessionHandle { get; set; }
        public void Validate()
        {
            new QueryValidator<T>().Validate(this);
        }

        public FindOptions<T> FindOptions { get; }
        public FilterDefinition<T> FilterDefinition { get; }
    }
}
