using System;
using System.Linq;
using Unianio.Services;
using UnityEngine;

namespace Unianio.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Checks if a GameObject has been destroyed.
        /// </summary>
        /// <param name="gameObject">GameObject reference to check for destructedness</param>
        /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
            return gameObject == null && !ReferenceEquals(gameObject, null);
        }
        public static GameObject AddEmptyChild(this GameObject parent, string name)
        {
            var child = new GameObject(name);
            child.transform.SetParent(parent.transform);
            child.transform.localPosition = v3.zero;
            child.transform.localRotation = Quaternion.identity;
            return child;
        }
        public static Vector3 DirWorldToLocal(this GameObject go, Vector3 world)
        {
            return go.transform.InverseTransformDirection(world);
        }
        public static Vector3 DirWorldToLocal(this GameObject go, in Vector3 world)
        {
            return go.transform.InverseTransformDirection(world);
        }
        public static Vector3 PointWorldToLocal(this GameObject go, Vector3 world)
        {
            return go.transform.InverseTransformPoint(world);
        }
        public static Vector3 PointWorldToLocal(this GameObject go, in Vector3 world)
        {
            return go.transform.InverseTransformPoint(world);
        }

        public static Vector3 VecWorldToLocal(this GameObject go, Vector3 world)
        {
            return go.transform.InverseTransformVector(world);
        }

        public static Vector3 VecWorldToLocal(this GameObject go, in Vector3 world)
        {
            return go.transform.InverseTransformVector(world);
        }

        public static Vector3 DirLocalToWorld(this GameObject go, Vector3 local)
        {
            return go.transform.TransformDirection(local);
        }

        public static Vector3 DirLocalToWorld(this GameObject go, in Vector3 local)
        {
            return go.transform.TransformDirection(local);
        }

        public static Vector3 PointLocalToWorld(this GameObject go, Vector3 local)
        {
            return go.transform.TransformPoint(local);
        }

        public static Vector3 PointLocalToWorld(this GameObject go, in Vector3 local)
        {
            return go.transform.TransformPoint(local);
        }

        public static Vector3 VecLocalToWorld(this GameObject go, Vector3 local)
        {
            return go.transform.TransformVector(local);
        }

        public static Vector3 VecLocalToWorld(this GameObject go, in Vector3 local)
        {
            return go.transform.TransformVector(local);
        }

        public static GameObject SetMaterial(this GameObject go, Action<Material> setter)
        {
            var renderer = go.GetComponent<Renderer>();
            setter(renderer.material);
            return go;
        }
        public static GameObject SetName(this GameObject go, string name)
        {
            go.name = name;
            return go;
        }
        public static GameObject ChangeMaterial(this GameObject go, Material material)
        {
            var renderer = go.GetComponent<Renderer>();
            renderer.material = material;
            return go;
        }
        public static GameObject ChangeAllMaterialsInChildren(this GameObject go, Func<string, Material> getMaterialByName)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                var mats = renderer.materials;
                var hasChange = false;
                for (var i = 0; i < mats.Length; ++i)
                {
                    var newMat = getMaterialByName(mats[i].name);
                    if (newMat != null)
                    {
                        hasChange = true;
                        mats[i] = newMat;
                    }
                }
                if (hasChange)
                {
                    renderer.materials = mats;
                }
            }
            return go;
        }

        public static GameObject SetTransform(this GameObject go, Action<Transform> setter)
        {
            setter(go.transform);
            return go;
        }

        public static GameObject WrapTransformed(this GameObject go, in Vector3 pos, in Quaternion rot)
        {
            var parentObj = new GameObject(go.name + "_wrapped");
            parentObj.transform.position = go.transform.position;
            parentObj.transform.LookAt(go.transform.position + go.transform.forward, go.transform.up);

            go.transform.SetParent(parentObj.transform);
            go.transform.localPosition = pos;
            go.transform.localRotation = rot;

            go.hideFlags = HideFlags.HideInHierarchy;
            return parentObj;
        }
        public static GameObject SetStandardShaderTransparentColor(this GameObject go, double r, double g, double b,
            double a)
        {
            var renderer = go.GetComponent<Renderer>();
            renderer.material.SetStandardShaderRenderingModeTransparent();
            renderer.material.color = new Color((float) r, (float) g, (float) b, (float) a);
            return go;
        }
        public static GameObject SetStandardShaderTransparentColor(this GameObject go, in Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            renderer.material.SetStandardShaderRenderingModeTransparent();
            renderer.material.color = color;
            return go;
        }
        public static GameObject SetStandardShaderTransparentColor(this GameObject go, in Color color,
            out Renderer renderer)
        {
            renderer = go.GetComponent<Renderer>();
            renderer.material.SetStandardShaderRenderingModeTransparent();
            renderer.material.color = color;
            return go;
        }
        public static GameObject SetPosition(this GameObject go, in Vector3 position)
        {
            go.transform.position = position;
            return go;
        }
        public static GameObject SetLocalPosition(this GameObject go, in Vector3 localPosition)
        {
            go.transform.localPosition = localPosition;
            return go;
        }
        public static GameObject SetLocalRotation(this GameObject go, in Quaternion localRotation)
        {
            go.transform.localRotation = localRotation;
            return go;
        }
        public static GameObject SetParent(this GameObject go, Transform parent)
        {
            go.transform.SetParent(parent);
            return go;
        }

    }
}