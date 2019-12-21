using System;
using System.Collections.Generic;
using Unianio.Extensions;
using Unianio.Static;
using UnityEngine;

namespace Unianio.Genesis.GenDbg
{
    public class BoneDebugVisualizer
    {
        public BoneDebugVisualizer Set(Transform model, string bodyName)
        {
            var body = model.FindFirst(bodyName);
            SkinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
            return this;
        }
        SkinnedMeshRenderer SkinnedMeshRenderer { get; set; }
        public BoneDebugRef GetBone(string name)
        {
            var boneIndex = SkinnedMeshRenderer.bones.FirstIndexOf(b => b.name == name);
            if(boneIndex < 0) throw new ArgumentException($"Cannot find bone {name}");
            return new BoneDebugRef(boneIndex, SkinnedMeshRenderer);
        }
        public void Compute(params BoneDebugRef[] bones)
        {
            var mesh = SkinnedMeshRenderer.sharedMesh;
            for (var i = 0; i < mesh.boneWeights.Length; ++i)
            {
                var bw = mesh.boneWeights[i];
                for(var j = 0; j < bones.Length; ++j)
                {
                    var curr = bones[j];
                    if (curr.BoneIndex.IsOneOfBones(bw, out var weight))
                    {
                        curr.Add(i, bw, weight);
                    }
                }
            }
        }
    }
    public class BoneDebugRef
    {
        static readonly Range<float> LenLimits = new Range<float>(0.002f, 0.08f);
        readonly int _id;
        readonly SkinnedMeshRenderer _smr;
        readonly Mesh _mesh;
        readonly List<(int vertIndex, BoneWeight boneWeight, float weight)> _wights = 
            new List<(int vertIndex, BoneWeight boneWeight, float weight)>();
        public BoneDebugRef(int boneIndex, SkinnedMeshRenderer smr)
        {
            BoneIndex = boneIndex;
            _smr = smr;
            _mesh = smr.sharedMesh;
            _id = boneIndex * 100000;
        }
        public int BoneIndex { get; }
        public void Add(int vertexIndex, BoneWeight bw, float weight)
        {
            _wights.Add((vertexIndex, bw, weight));
        }
        public void Draw(in Color color)
        {
            for (var i = 0; i < _wights.Count; ++i)
            {
                var t = _wights[i];
                var bm0 = _smr.bones[t.boneWeight.boneIndex0].localToWorldMatrix * _mesh.bindposes[t.boneWeight.boneIndex0];
                var bm1 = _smr.bones[t.boneWeight.boneIndex1].localToWorldMatrix * _mesh.bindposes[t.boneWeight.boneIndex1];
                var bm2 = _smr.bones[t.boneWeight.boneIndex2].localToWorldMatrix * _mesh.bindposes[t.boneWeight.boneIndex2];
                var bm3 = _smr.bones[t.boneWeight.boneIndex3].localToWorldMatrix * _mesh.bindposes[t.boneWeight.boneIndex3];

                var vertexMatrix = new Matrix4x4();
                
                for (var n = 0; n < 16; n++){
                    vertexMatrix[n] =
                        bm0[n] * t.boneWeight.weight0 +
                        bm1[n] * t.boneWeight.weight1 +
                        bm2[n] * t.boneWeight.weight2 +
                        bm3[n] * t.boneWeight.weight3;
                }

                var vx = vertexMatrix.MultiplyPoint3x4(_mesh.vertices[t.vertIndex]);
                var nm = vertexMatrix.MultiplyVector(_mesh.normals[t.vertIndex]);
                var x = t.weight.Clamp01();
#if UNIANIO_DEBUG
                dbg.DrawLine(_id+i, vx, vx + nm * LenLimits.ValueByProgress(x), GetColor(in color, x));
#endif

            }
        }
        static Color GetColor(in Color color, float x)
            => color.a < 0.5 ? fun.color.Rainbow(x) : color;
        public void Clear()
        {
            for (var i = 0; i < _wights.Count; ++i)
            {
#if UNIANIO_DEBUG
                dbg.ClearLine(_id + i);
#endif
            }
        }
    }
}