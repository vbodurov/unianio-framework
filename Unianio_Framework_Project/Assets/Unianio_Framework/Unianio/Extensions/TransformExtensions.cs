using System;
using System.Collections.Generic;
using Unianio.Collections;
using Unianio.Static;
using UnityEngine;
using UnityEngine.UI;
using static Unianio.Static.fun;

namespace Unianio.Extensions
{
    public static class TransformExtensions
    {
        public static Transform AddEmptyChild(this Transform parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent);
            child.transform.localPosition = v3.zero;
            child.transform.localRotation = Quaternion.identity;
            return child.transform;
        }
        public static Transform FindInTree(this Transform transform, params string[] nodeNames)
        {
            var current = transform;
            if (current == null) return null;
            for (var i = 0; i < nodeNames.Length; ++i)
            {
                var currName = nodeNames[i];
                current = current.Find(currName);
                if (current == null)
                {
                    Debug.Log($"Cannot find transform of path element {i} with name:'{currName}' in FindInTree {nodeNames.JoinAsString("/")}");
                    break;
                }
            }

            return current;
        }
        public static void FindAndSetTextInTree(this Transform transform, string text, params string[] nodeNames)
        {
            var current = transform;
            if (current == null) return;
            for (var i = 0; i < nodeNames.Length; ++i)
            {
                var currName = nodeNames[i];
                current = current.Find(currName);
                if (current == null)
                {
                    Debug.Log($"Cannot find transform of path element {i} with name:'{currName}' in FindAndSetTextInTree {nodeNames.JoinAsString("/")}");
                    break;
                }
            }

            if (current != null)
            {
                var textUi = current.GetComponent<Text>();
                if (textUi == null)
                {
                    Debug.Log($"Cannot find Text UI element in FindAndSetTextInTree for path {nodeNames.JoinAsString("/")}");
                    return;
                }

                textUi.text = text;
            }
        }
        public static Transform SetLocal(this Transform transform, in Vector3 pos, in Vector3 fw, in Vector3 up)
        {
            transform.localPosition = pos;
            transform.localRotation = lookAt(fw, up);
            return transform;
        }
        public static Transform SetLocal(this Transform transform, in Vector3 pos, in Quaternion rot)
        {
            transform.localPosition = pos;
            transform.localRotation = rot;
            return transform;
        }
        public static Transform SetLocalPosition(this Transform transform, in Vector3 pos)
        {
            transform.localPosition = pos;
            return transform;
        }
        public static Transform SetLocalRotation(this Transform transform, in Quaternion rot)
        {
            transform.localRotation = rot;
            return transform;
        }
        public static Transform SetPosition(this Transform transform, in Vector3 pos)
        {
            transform.position = pos;
            return transform;
        }
        public static Transform AddToPosition(this Transform transform, in Vector3 toAdd)
        {
            transform.position += toAdd;
            return transform;
        }
        public static Transform SetScale(this Transform transform, double scale)
        {
            transform.localScale = new Vector3((float)scale, (float)scale, (float)scale);
            return transform;
        }
        public static Transform SetColor(this Transform transform, uint color)
        {
            transform.GetComponentInChildren<Renderer>().material.color = fun.color.Parse(color);
            return transform;
        }
        public static PrimitiveType GetPrimitiveTypeOrDefault(this Transform current, PrimitiveType defaultType)
        {
            if (current == null) return defaultType;
            if (current.GetComponent<SphereCollider>()) return PrimitiveType.Sphere;
            if (current.GetComponent<BoxCollider>()) return PrimitiveType.Cube;
            if (current.GetComponent<CapsuleCollider>()) return PrimitiveType.Capsule;
            return defaultType;
        }
        public static Transform GetTopObject(this Transform current)
        {
            if (current == null) return null;
            var level = 0;
            while (true)
            {
                var parent = current.parent;
                if (parent == null) return current;
                current = parent;
                if (++level > 512) return null;
            }
        }
        public static Transform ThrowIfNull(this Transform current)
        {
            if(current == null) throw new InvalidOperationException("Transform was not expected to be NULL");
            return current;
        }
        public static Transform[] GetDirectChildren(this Transform current)
        {
            var children = new Transform[current.childCount];
            for (var i = 0; i < children.Length; ++i)
                children[i] = current.GetChild(i);
            return children;
        }
        public static Transform Wrap(this Transform transform, Transform parent, Vector3 lookFwWorldDir, Vector3 lookUpWorldDir)
        {
            return wrapTransformInHolder(parent, transform, lookFwWorldDir, lookUpWorldDir);
        }
        public static Transform Wrap(this Transform transform, Transform parent, Func<Transform,Vector3> getLookFwWorldDir, Func<Transform, Vector3> getLookUpWorldDir)
        {
            return wrapTransformInHolder(parent, transform, getLookFwWorldDir(transform), getLookUpWorldDir(transform));
        }
        public static Transform Wrap(this Transform transform, Func<Transform, Vector3> getLookFwWorldDir, Func<Transform, Vector3> getLookUpWorldDir)
        {
            return wrapTransformInHolder(transform, getLookFwWorldDir(transform), getLookUpWorldDir(transform));
        }
        public static Quaternion RotTo(this Transform transform, Vector3 targetPoint, Vector3 upDir)
        {
            return lookAt(targetPoint, transform.position, upDir);
        }
        public static IDictionary<string, Transform> PlaceChildrenInDictionary(this Transform transform)
        {
            return transform.ForEachChild(
                new Dictionary<string, Transform>(StringComparer.InvariantCultureIgnoreCase),
                (t, dic) => dic[t.name] = t);
        }
        public static IDictionary<string, Transform> PlaceChildrenInNonStrictDictionary(this Transform transform)
        {
            return transform.ForEachChild(
                new NonStrictDictionary<string,Transform>(StringComparer.InvariantCultureIgnoreCase),
                (t, dic) => dic[t.name] = t);
        }
        public static Transform FindFirst(this Transform parent, params string[] names)
        {
            for (var i = 0; i < names.Length; i++)
            {
                var t = parent.Find(names[i]);
                if (t != null) return t;
            }
            return null;
        }
        public static Transform FindFirstRecursive(this Transform current, params string[] names)
        {
            for (var i = 0; i < names.Length; ++i)
            {
                var t = current.FindChildByRecursion(names[i]);
                if (t != null) return t;
            }
            return null;
        }
        public static Transform SetParentAndReturn(this Transform current, Transform parent)
        {
            current.SetParent(parent);
            return current;
        }
        public static Vector3 DirTo(this Transform from, Transform to)
        {
            return (to.position - from.position).normalized;
        }
        public static Vector3 DirToInSpace(this Transform from, Transform to, Transform space)
        {
            return (to.position - from.position).normalized.AsLocalDir(space);
        }
        public static Vector3 DirTo(this Transform from, in Vector3 to)
        {
            return (to - from.position).normalized;
        }
        public static Vector3 DirToInSpace(this Transform from, in Vector3 to, Transform space)
        {
            return (to - from.position).normalized.AsLocalDir(space);
        }
        public static void LerpTowards(this Transform transform, in Vector3 target, double relativeStep01)
        {
            transform.position = lerp(transform.position, in target, relativeStep01.Clamp01());
        }
        public static void MoveTowards(this Transform transform, in Vector3 position, double step = 360)
        {
            transform.position = transform.position.MoveTowards(position, step);
        }
        public static void MoveTowardsLocal(this Transform transform, in Vector3 position, double step = 360)
        {
            transform.localPosition = transform.localPosition.MoveTowards(position, step);
        }
        public static void RotateTowards(this Transform transform, in Quaternion direction, double step = 360)
        {
            transform.rotation = transform.rotation.RotateTowards(direction, (float)step);
        }
        public static void RotateTowardsLocal(this Transform transform, in Quaternion localDirection, double step = 360)
        {
            transform.localRotation = transform.localRotation.RotateTowards(localDirection, (float)step);
        }
        public static void RotateTowards(this Transform transform, in Vector3 fw, in Vector3 up, double step = 360)
        {
            transform.rotation = transform.rotation.RotateTowards(Quaternion.LookRotation(fw, fw.GetRealUp(up)), (float)step);
        }
        public static void RotateTowardsLocal(this Transform transform, in Vector3 fw, in Vector3 up, double step = 360)
        {
            transform.localRotation = transform.localRotation.RotateTowards(Quaternion.LookRotation(fw, fw.GetRealUp(up)), (float)step);
        }
        public static void Face(this Transform transform, in Vector3 target, in Vector3 up, double step = 360)
        {
            var fw = transform.DirTo(target);
            transform.rotation = transform.rotation.RotateTowards(Quaternion.LookRotation(transform.DirTo(target), fw.GetRealUp(up)), (float)step);
        }
        public static Vector3 GetMovedTowardsInSpace(this Transform transform, Transform space, in Vector3 targetInModel, double step = 360)
        {
            return transform.position.AsLocalPoint(space).MoveTowards(targetInModel, step);
        }
        /// <summary>
        /// iniPivotPoseDiff = model.position - bone.position;
        /// </summary>
        public static void SetPositionWithPivotTransform(this Transform model, in Vector3 newPos, Transform bone, in Vector3 iniPivotPoseDiff)
        {
            var currPivotBoneDiff = model.position - bone.position;
            model.position = newPos + (currPivotBoneDiff - iniPivotPoseDiff);
        }
        /// <summary>
        /// iniPivotPoseDiff = model.position - bone.position;
        /// </summary>
        public static void SetPositionWithPivotTransform(this Transform model, Vector3 newPos, Transform bone, Vector3 iniPivotPoseDiff)
        {
            var currPivotBoneDiff = model.position - bone.position;
            model.position = newPos + (currPivotBoneDiff - iniPivotPoseDiff);
        }
        public static Transform SetLookAt(this Transform model, Vector3 fw, Vector3 up)
        {
            model.rotation = Quaternion.LookRotation(fw, up);
            return model;
        }
        public static Transform SetPosX(this Transform t, float x) { t.position = new Vector3(x, t.position.y, t.position.z); return t; }
        public static Transform SetPosY(this Transform t, float y) { t.position = new Vector3(t.position.x, y, t.position.z); return t; }
        public static Transform SetPosZ(this Transform t, float z) { t.position = new Vector3(t.position.x, t.position.y, z); return t; }
        public static Transform AddToPosX(this Transform t, float x) { t.position = new Vector3(t.position.x + x, t.position.y, t.position.z); return t; }
        public static Transform AddToPosY(this Transform t, float y) { t.position = new Vector3(t.position.x, t.position.y + y, t.position.z); return t; }
        public static Transform AddToPosZ(this Transform t, float z) { t.position = new Vector3(t.position.x, t.position.y, t.position.z + z); return t; }
        public static Vector3 DirFrameChange(this Transform fromFrame, Vector3 fromDir, Transform toFrame)
        {
            fromDir.Normalize();
            var fromWorld = fromFrame.TransformDirection(fromDir);
            return toFrame.InverseTransformDirection(fromWorld);
        }

        public static Vector3 DirWorldToLocal(this Transform t, Vector3 world)
        {
            return t.InverseTransformDirection(world);
        }
        public static Vector3 DirWorldToLocal(this Transform t, in Vector3 world)
        {
            return t.InverseTransformDirection(world);
        }
        public static Vector3 PointWorldToLocal(this Transform t, Vector3 world)
        {
            return t.InverseTransformPoint(world);
        }
        public static Vector3 PointWorldToLocal(this Transform t, in Vector3 world)
        {
            return t.InverseTransformPoint(world);
        }
        public static Vector3 VecWorldToLocal(this Transform t, Vector3 world)
        {
            return t.InverseTransformVector(world);
        }
        public static Vector3 VecWorldToLocal(this Transform t, in Vector3 world)
        {
            return t.InverseTransformVector(world);
        }

        public static Vector3 DirLocalToWorld(this Transform t, Vector3 local)
        {
            return t.TransformDirection(local);
        }
        public static Vector3 DirLocalToWorld(this Transform t, in Vector3 local)
        {
            return t.TransformDirection(local);
        }
        public static Vector3 PointLocalToWorld(this Transform t, Vector3 local)
        {
            return t.TransformPoint(local);
        }
        public static Vector3 PointLocalToWorld(this Transform t, in Vector3 local)
        {
            return t.TransformPoint(local);
        }
        public static Vector3 VecLocalToWorld(this Transform t, Vector3 local)
        {
            return t.TransformVector(local);
        }
        public static Vector3 VecLocalToWorld(this Transform t, in Vector3 local)
        {
            return t.TransformVector(local);
        }
        public static T ForEachDirectChild<T>(this Transform transform, T state, Action<Transform, T> process)
        {
            for (var i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                process(child, state);
            }
            return state;
        }
        public static T ForEachChild<T>(this Transform transform, T state, int index, Action<int, Transform, T> process)
        {
            process(index, transform, state);
            for (var i = 0; i < transform.childCount; ++i)
            {
                state = transform.GetChild(i).ForEachChild(state, index + 1, process);
            }
            return state;
        }
        public static T ForEachChild<T>(this Transform transform, T state, Action<Transform, T> process)
        {
            return transform.ForEachChild(state, 0, (i,t,s) => process(t,s));
        }
        public static void ForEachChild(this Transform transform, Action<Transform> process)
        {
            transform.ForEachChild(0, 0, (i, t, s) => process(t));
        }

        public static void LookAtWithMaxAngleChange(this Transform t, Vector3 worldPosition, Vector3 worldUp, float maxDegreesChange)
        {
            t.LookAtWithMaxAngleChange(in worldPosition, in worldUp, maxDegreesChange);
        }
        public static void LookAtWithMaxAngleChange(this Transform t, in Vector3 worldPosition, float maxAngleChange)
        {
            var up = t.up;
            t.LookAtWithMaxAngleChange(in worldPosition, in up, maxAngleChange);
        }
        public static void LookAtWithMaxAngleChange(this Transform t, in Vector3 worldPosition, in Vector3 worldUp, float maxDegreesChange)
        {
            var currentForward = t.forward;
            var desiredForward = (worldPosition - t.position).normalized;
            var nextFraForward = currentForward.RotateTowards(in desiredForward, maxDegreesChange);
            t.LookAt(t.position+nextFraForward, worldUp);
        }
        public static float DotProduct(in Vector3 lhs, in Vector3 rhs)
        {
            return (lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z);
        }
        public static readonly float RTD = (float)(180 / Math.PI);
        public static Transform FindChildByRecursion(this Transform aParent, string aName)
        {
            if (aParent == null) return null;
            var result = aParent.Find(aName);
            if (result != null)
                return result;
            foreach (Transform child in aParent)
            {
                result = child.FindChildByRecursion(aName);
                if (result != null)
                    return result;
            }
            return null;
        }
        public static TState FindChildByRecursion<TState>(this Transform aParent, TState state, Action<Transform,TState> action)
        {
            foreach (Transform child in aParent)
            {
                action(child,state);
                child.FindChildByRecursion(state, action);
            }
            return state;
        }
        public static Transform FindChildByRecursion(this Transform aParent, Func<Transform, bool> tryFind)
        {
            Transform found = null;
            foreach (Transform child in aParent)
            {
                if (tryFind(child)) found = child;
                else found = child.FindChildByRecursion(tryFind);
                if (found != null) return found;
            }
            return null;
        }
        public static void TraverseTree(this Transform aParent, Func<Transform, bool> action)
        {
            foreach (Transform child in aParent)
            {
                if (action(child)) child.TraverseTree(action);
            }
        }
        public static TState TraverseTree<TState>(this Transform aParent, TState state, Func<Transform, TState, bool> action)
        {
            foreach (Transform child in aParent)
            {
                if(action(child, state)) child.TraverseTree(state, action);
            }
            return state;
        }
    }
}