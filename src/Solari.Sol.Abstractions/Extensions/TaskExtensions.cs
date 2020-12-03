using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Solari.Sol.Abstractions.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> AsCompletedTask<T>(this T obj) => Task.FromResult(obj);

        public static ConfiguredTaskAwaitable DefaultAwait(this System.Threading.Tasks.Task task, bool await = false) => task.ConfigureAwait(await);

        public static ConfiguredTaskAwaitable<T> DefaultAwait<T>(this Task<T> task, bool await = false) => task.ConfigureAwait(await);
    }
}
