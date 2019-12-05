using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Graphs
{
    public interface IVectorByProgress
    {
        PathType Type { get; }
        // progress is from 0 to 1
        Func<double, double> Func { get; set; }
        Vector3 GetValueByProgress(double progress);
        Vector3 GetDirectionByProgress(double progress);
    }
    public interface IStartEndVectorByProgress : IVectorByProgress
    {
        Vector3 Start { get; set; }
        Vector3 End { get; set; }
    }
    public interface IVectorByProgressInRange : IVectorByProgress, IProviderInRange { }
    public interface IVector2DByProgress
    {
        PathType Type { get; }
        // progress is from 0 to 1
        Func<double, double> Func { get; set; }
        Vector2 GetValueByProgress(double progress);
        Vector2 GetDirectionByProgress(double progress);
    }
}