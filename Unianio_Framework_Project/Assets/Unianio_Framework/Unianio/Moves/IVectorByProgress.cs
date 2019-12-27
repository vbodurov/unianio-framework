using System;
using Unianio.Enums;
using UnityEngine;

namespace Unianio.Moves
{
    public interface IVectorByProgress
    {
        MoveType Type { get; }
        // progress is from 0 to 1
        Vector3 GetValueByProgress(double progress);
    }
    public interface IVectorByProgressInRange : IVectorByProgress, IRangeOfNumbers { }
    public interface IVector2DByProgress
    {
        MoveType Type { get; }
        // progress is from 0 to 1
        Vector2 GetValueByProgress(double progress);
        Vector2 GetDirectionByProgress(double progress);
    }
}