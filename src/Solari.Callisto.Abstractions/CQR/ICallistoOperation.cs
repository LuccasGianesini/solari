using System.Threading;
using FluentValidation.Results;
using MongoDB.Driver;

namespace Solari.Callisto.Abstractions.CQR
{
    public interface ICallistoOperation<T> where T : class, IDocumentRoot
    {
        string OperationName { get; }
        CancellationToken CancellationToken { get; set;  }
        IClientSessionHandle ClientSessionHandle { get; set; }
        void Validate();
    }
}