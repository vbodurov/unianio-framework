using UnityEngine;

namespace Unianio.Extensions
{
    public static class RendererExtensions
    {
        public static  void ChangeAllMaterials(this Renderer renderer, Material material)
        {
            var mats = renderer.materials;
            for (var i = 0; i < mats.Length; ++i)
            {
                mats[i] = material;
            }
            renderer.materials = mats;
        }
    }
}
