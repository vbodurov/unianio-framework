﻿using System;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector2 MoveTowards(in this Vector2 current, in Vector2 target, double maxDistanceDelta)
        {
          Vector2 vector2 = target - current;
          float magnitude = vector2.magnitude;
          if ((double) magnitude <= (double) maxDistanceDelta || (double) magnitude == 0.0)
            return target;
          return current + vector2 / magnitude * (float)maxDistanceDelta;
        }
        public static Vector3 As3d(in this Vector2 v2, Vector3 origin, in Vector3 normalizedX, Vector3 normalizedY)
        {
            return origin + normalizedX*v2.x + normalizedY*v2.y;
        }
        public static Vector3 As3d(in this Vector2 v2, in Vector3 origin, in Vector3 normalizedX, in Vector3 normalizedY)
        {
            return origin + normalizedX*v2.x + normalizedY*v2.y;
        }
        public static void As3d(in this Vector2 v2, in Vector3 origin, in Vector3 normalizedX, in Vector3 normalizedY, out Vector3 result)
        {
            result = origin + normalizedX*v2.x + normalizedY*v2.y;
        }
        public static Vector3 ToV3(in this Vector2 v)
        {
            return new Vector3(v.x, 0, v.y);
        }
        public static Vector3 ToV3(in this Vector2 v, double y)
        {
            return new Vector3(v.x, (float)y, v.y);
        }
        public static bool IsEqual(in this Vector2 a, in Vector2 b)
        {
            return 
                a.x.IsEqual(b.x) && 
                a.y.IsEqual(b.y);
        }
        public static bool IsEqual(in this Vector2 a, in Vector2 b, double delta)
        {
            return 
                a.x.IsEqual(b.x, delta) && 
                a.y.IsEqual(b.y, delta);
        }
        public static bool Equals(in this Vector2 a, in Vector2 b)
        {
            return a.x.Equals(b.x) && a.y.Equals(b.y);
        }
        public static Vector2 GoTowards(in this Vector2 from, in Vector2 to, double stepDistance)
        {
            if (stepDistance < 0) stepDistance *= -1;
            var diff = to - from;
            if (diff.magnitude <= stepDistance) return to;
            return from + ((float)stepDistance)*diff.normalized;
        }

        public static string s(in this Vector2 v)
        {
            return v.x.s() + "," + v.y.s();
        }
        public static string s(in this Vector2 v, int digits)
        {
            return Math.Round(v.x, digits).s() + "," + Math.Round(v.y, digits).s();
        }
    }
}