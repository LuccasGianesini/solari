using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface ICommandHandler<in T> where T : class, ICommand
    {
        Task HandleCommandAsync(T command);
    }

    public interface ICommandHandler<in T, TResult> where T : class, ICommand
    {
        Task<TResult> HandleCommandAsync(T command);
    }
}