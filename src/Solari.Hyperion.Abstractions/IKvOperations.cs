using System.Threading.Tasks;
using Consul;

namespace Solari.Hyperion.Abstractions
{
    public interface IKvOperations
    {
        Task<WriteResult<bool>> SaveToKv<T>(string key, T value) where T : class;
        Task<WriteResult<bool>> SaveToKv(string key, bool value);
        Task<WriteResult<bool>> SaveToKv(string key, string value);
        Task<WriteResult<bool>> SaveToKv(string key, char value);
        Task<WriteResult<bool>> SaveToKv(string key, int value);
        Task<WriteResult<bool>> SaveToKv(string key, float value);
        Task<WriteResult<bool>> SaveToKv(string key, double value);
        Task<WriteResult<bool>> SaveToKv(string key, short value);
        Task<WriteResult<bool>> SaveToKv(string key, decimal value);
        Task<WriteResult<bool>> SaveToKv(string key, uint value);
        Task<WriteResult<bool>> SaveToKv(string key, ulong value);
        Task<WriteResult<bool>> SaveToKv(string key, ushort value);
        Task<short> GetShortFromKv(string key);
        Task<WriteResult<bool>> DeleteFromKv(string key);
        Task<T> GetObjectFromKv<T>(string key);
        Task<bool> GetBoolFromKv(string key);
        Task<string> GetStringFromKv(string key);
        Task<char> GetCharFromKv(string key);
        Task<int> GetIntFromKv(string key);
        Task<float> GetFloatFromKv(string key);
        Task<double> GetDoubleFromKv(string key);
        Task<decimal> GetDecimalFromKv(string key);
        Task<uint> GetUIntFromKv(string key);
        Task<ulong> GetULongFromKv(string key);
        Task<ushort> GetUShortFromKv(string key);
    }
}