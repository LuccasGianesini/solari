using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface ICommandHandler<in T> : Convey.CQRS.Commands.ICommandHandler<T> where T : class, ICommand
    {
        Task HandleCommandAsync(T command);
    }
}