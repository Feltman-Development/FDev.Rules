using System;
using System.Threading;

namespace FDEV.Rules.Demo.Core.Utility
{
    public static class GuidUtility
    {
        private static long _counter = DateTime.UtcNow.Ticks;

        /// <summary>
        /// Generates sequential <see cref="Guid" /> values using the same algorithm as NEWSEQUENTIALID() used in Microsoft SQL Server. 
        /// This is useful when entities are being saved to a database with Guid as the main id, and sequential assigned Guids will
        /// provide a performance benefit in queries.
        /// </summary>
        public static Guid NewSequentialGuid()
        {
            var guidBytes = Guid.NewGuid().ToByteArray();
            var counterBytes = BitConverter.GetBytes(Interlocked.Increment(ref _counter));

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            guidBytes[08] = counterBytes[1];
            guidBytes[09] = counterBytes[0];
            guidBytes[10] = counterBytes[7];
            guidBytes[11] = counterBytes[6];
            guidBytes[12] = counterBytes[5];
            guidBytes[13] = counterBytes[4];
            guidBytes[14] = counterBytes[3];
            guidBytes[15] = counterBytes[2];

            return new Guid(guidBytes);
        }

        /// <summary>
        /// Generate a new <see cref="Guid"/> using the comb algorithm.
        /// </summary>
        public static Guid NewCombGuid()
        {
            var guidArray = Guid.NewGuid().ToByteArray();
            var baseDate = new DateTime(1900, 1, 1);
            var now = DateTime.Now;
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = now.TimeOfDay;

            // Convert to a byte array. Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Copy to array, after reversing the bytes to match SQL Servers ordering 
            Array.Reverse(daysArray);
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Reverse(msecsArray);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }
    }
}
