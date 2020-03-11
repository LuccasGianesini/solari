using System;
using System.Threading;

namespace Solari.Rhea
{
    // Copied from Microsoft code.
    public class RequestTraceIdGenerator
    {
        private const string Encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";
        private static long _requestId = DateTime.UtcNow.Ticks;
        private string _id;

        /// <summary>
        /// Generates a TraceId.
        /// </summary>
        /// <returns></returns>
        public string TraceIdentifier()
        {
            return _id ??= GenerateRequestId(Interlocked.Increment(ref _requestId));
        }

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
    }
}