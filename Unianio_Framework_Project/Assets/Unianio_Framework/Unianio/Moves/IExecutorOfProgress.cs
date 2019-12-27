using System;

namespace Unianio.Moves
{
    public interface IExecutorOfProgress
    {
        int Tag { get; set; }
        void Apply(double progress, Func<double, double> function = null);
    }
}