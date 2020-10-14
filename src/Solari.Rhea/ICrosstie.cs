using System.Threading.Tasks;

namespace Solari.Rhea
{
    public interface ICrosstie
    {
        Task Execute<TContext>(TContext context);
    }
}
