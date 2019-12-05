using System;

namespace Unianio
{
    public static class ShortGuid
    {
        static readonly long BaseDate = new DateTime(2005,7,3,2,15,11).Ticks;// just randomly picked number
        const double TicksPerSecond = 1.0/TimeSpan.TicksPerSecond;
        const int BitsAll = 64;
        const int BitsQuater = BitsAll/4;
        const int BitsQuaterX2 = BitsQuater*2;
        const int BitsQuaterX3 = BitsQuater*3;
        const int BitsTime = 30;
        const int BitsCount = BitsAll - BitsTime;
        static readonly ulong MaxTime = (ulong) Math.Pow(2, BitsTime);// 30 bits = 1,073,741,824 seconds ~= 34.9 years
        static readonly ulong MaxCount = (ulong) Math.Pow(2, BitsCount);// 34 bits = 17,179,869,184
        static ulong _count = (ulong)(DateTime.UtcNow.Ticks % 17000000000);

        public static string NewToString()
        {
            return "0x" + New().ToString("X");
        }
        public static ulong New()
        {
            var now = DateTime.UtcNow;
            var time = (ulong)((now.Ticks - BaseDate) * TicksPerSecond) % MaxTime;
            _count = (_count + 1) % MaxCount;
            ulong raw = (time << BitsCount) | _count;
            ushort a = (ushort) raw;
            ushort b = (ushort) (raw>>BitsQuater);
            ushort c = (ushort) (raw>>BitsQuaterX2);
            ushort d = (ushort) (raw>>BitsQuaterX3);
            ulong val = 0;
            var i = 0;
            var ai = 0;
            var bi = 0;
            var ci = 0;
            var di = 0;
            for (; i < BitsAll; ++i)
            {
                ulong curr = (ulong)1 << i;
                var n = i%4;

                if (n == 0)
                {
                    if ((a & (1 << ai)) > 0)
                    {
                        val |= curr;
                    }
                    ++ai;
                }
                else if (n == 2)
                {
                    if ((b & (1 << bi)) > 0)
                    {
                        val |= curr;
                    }
                    ++bi;
                }
                else if (n == 1)
                {
                    if ((c & (1 << ci)) > 0)
                    {
                        val |= curr;
                    }
                    ++ci;
                }
                else if (n == 3)
                {
                    if ((d & (1 << di)) > 0)
                    {
                        val |= curr;
                    }
                    ++di;
                }
            }
            return val;
        }
    }
}