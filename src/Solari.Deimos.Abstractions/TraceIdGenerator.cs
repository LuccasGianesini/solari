using System;
using System.Threading;

namespace Solari.Deimos.Abstractions
{
    public class TraceIdGenerator
    {
        private const string Encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
        private static long _requestId = DateTime.UtcNow.Ticks;
        private string _id;

        public TraceIdGenerator()
        {
            
        }

        public TraceIdGenerator(string id)
        {
            _id = id;
        }
        public string TraceIdentifier 
            => _id ??= GenerateRequestId(Interlocked.Increment(ref _requestId));
        

        private static unsafe string GenerateRequestId(long id)
        {
            short* numPtr = stackalloc short[13];
            numPtr[0] = (short) Encode32Chars[(int) (id >> 60) & 31];
            numPtr[1] = (short) Encode32Chars[(int) (id >> 55) & 31];
            numPtr[2] = (short) Encode32Chars[(int) (id >> 50) & 31];
            numPtr[3] = (short) Encode32Chars[(int) (id >> 45) & 31];
            numPtr[4] = (short) Encode32Chars[(int) (id >> 40) & 31];
            numPtr[5] = (short) Encode32Chars[(int) (id >> 35) & 31];
            numPtr[6] = (short) Encode32Chars[(int) (id >> 30) & 31];
            numPtr[7] = (short) Encode32Chars[(int) (id >> 25) & 31];
            numPtr[8] = (short) Encode32Chars[(int) (id >> 20) & 31];
            numPtr[9] = (short) Encode32Chars[(int) (id >> 15) & 31];
            numPtr[10] = (short) Encode32Chars[(int) (id >> 10) & 31];
            numPtr[11] = (short) Encode32Chars[(int) (id >> 5) & 31];
            numPtr[12] = (short) Encode32Chars[(int) id & 31];

            return new string((char*) numPtr, 0, 13);
        }
        public static string Create() => new TraceIdGenerator().TraceIdentifier;
        public static string Create(string id) => new TraceIdGenerator(id).TraceIdentifier;
    }
}