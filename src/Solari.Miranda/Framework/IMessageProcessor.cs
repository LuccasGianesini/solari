using System.Threading.Tasks;

namespace Solari.Miranda.Framework
{
    /// <summary>
    /// FROM CONVEY
    /// </summary>
    public interface IMessageProcessor
    {
        Task<bool> TryProcessAsync(string id);
        Task RemoveAsync(string id);
    }
}