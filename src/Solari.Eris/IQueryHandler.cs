using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface IQueryHandler<in T, TResult> where T : IQuery<TResult>
    {
        Task<TResult> HandleQueryAsync(T query);
    }
}