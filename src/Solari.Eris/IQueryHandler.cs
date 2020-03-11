using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface IQueryHandler<in T, TResult> where T : IQuery
    {
        Task<TResult> HandleQueryAsync(T query);
    }
}