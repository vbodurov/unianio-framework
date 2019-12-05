using System;

namespace Unianio.Graphs
{
    public interface IExecutorOfProgress
    {
        int ID { get; set; }
        void Apply(double progress);
        void Apply(double progress, Func<double, double> function);
    }
}