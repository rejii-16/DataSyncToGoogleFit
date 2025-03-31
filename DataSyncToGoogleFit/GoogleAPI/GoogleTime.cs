using System;

namespace DataSyncToGoogleFit
{
    internal class GoogleTime
    {
        private static readonly DateTime ZERO = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public long TotalMilliseconds { get; private set; }
        public long TotalNanoSeconds { get { return TotalMilliseconds * 1000000; } }

        private GoogleTime() { }

        public static GoogleTime FromDateTime(DateTime dt)
        {
            return new GoogleTime { TotalMilliseconds = (long)(dt - ZERO).TotalMilliseconds, };
        }

        public static GoogleTime FromNanoseconds(long? nanoseconds)
        {
            return new GoogleTime { TotalMilliseconds = (long)(nanoseconds.GetValueOrDefault(0) / 1000000) };
        }

        public DateTime ToDateTime()
        {
            return ZERO.AddMilliseconds(this.TotalMilliseconds);
        }
    }
}
