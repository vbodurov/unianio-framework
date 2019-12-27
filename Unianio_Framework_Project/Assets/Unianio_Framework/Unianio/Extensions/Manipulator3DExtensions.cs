using System;
using Unianio.Enums;
using Unianio.Rigged.IK;
using UnityEngine;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class Manipulator3DExtensions
    {
        public static Vector3 GetPosBySpace(this IManipulator3D obj, ObjectSpace space)
        {
            switch (space)
            {
                case ObjectSpace.Model: return obj.ModelPos;
                case ObjectSpace.Local: return obj.LocalPos;
            }
            return obj.Control.position;
        }
        public static Quaternion GetRotBySpace(this IManipulator3D obj, ObjectSpace space)
        {
            switch (space)
            {
                case ObjectSpace.Model: return lookAt(obj.ModelFw, obj.ModelUp);
                case ObjectSpace.Local: return lookAt(obj.LocalFw, obj.LocalUp);
            }
            return obj.Control.rotation;
        }
        public static Vector3 GetFwBySpace(this IManipulator3D obj, ObjectSpace space)
        {
            switch (space)
            {
                case ObjectSpace.Model: return obj.ModelFw;
                case ObjectSpace.Local: return obj.LocalFw;
            }
            return obj.Control.forward;
        }
        public static Vector3 GetUpBySpace(this IManipulator3D obj, ObjectSpace space)
        {
            switch (space)
            {
                case ObjectSpace.Model: return obj.ModelUp;
                case ObjectSpace.Local: return obj.LocalUp;
            }
            return obj.Control.up;
        }
    }
}