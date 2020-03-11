using System.Threading.Tasks;

namespace Solari.Eris
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        Task HandleCommandAsync(T command);
    }
}