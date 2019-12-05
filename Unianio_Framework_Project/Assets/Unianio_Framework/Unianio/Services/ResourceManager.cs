using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Unianio.Services
{
    [AsSingleton]
    public interface IResourceManager
    {
        Sprite LoadSprite(string path);
        Material LoadMaterial(string path);
        Texture LoadTexture(string path);
        Font LoadFont(string path);
        GameObject LoadAndInstantiate(string path);
        AudioClip LoadAudioClip(string path);
        byte[] LoadBytes(string mapWalkBin);
        void UnloadAsset(Object assetToUnload);

        Sprite GetSpriteInAtlas(string atlasAndSpriteNames);
        Sprite GetSpriteInAtlas(string spriteName, string atlasName);
        void ClearAtlas(string atlasName);
        Shader LoadShader(string path);
        ResourceRequest LoadAudioClipAsync(string path);
        AudioMixer LoadAudioMixer(string path);
        string LoadText(string path);
    }
    internal sealed class ResourceManager : IResourceManager
    {
        private readonly Dictionary<string, Dictionary<string, Sprite>> _atlases =
            new Dictionary<string, Dictionary<string, Sprite>>();

        Sprite IResourceManager.GetSpriteInAtlas(string atlasAndSpriteNames)
        {
            var arr = atlasAndSpriteNames.Split('_');
            if (arr.Length == 1) arr = new[] {arr[0], arr[0]};
            return ((IResourceManager) this).GetSpriteInAtlas(arr[1], arr[0]);
        }
        Sprite IResourceManager.GetSpriteInAtlas(string spriteName, string atlasName)
        {
            Dictionary<string, Sprite> atlasSprites;
            if (!_atlases.TryGetValue(atlasName, out atlasSprites))
            {
                var sprites = Resources.LoadAll<Sprite>("Sprites/" + atlasName);
                atlasSprites = sprites.ToDictionary(s => s.name, StringComparer.OrdinalIgnoreCase);
                _atlases[atlasName] = atlasSprites;
            }
            Sprite sprite;
            if (atlasSprites.TryGetValue(spriteName, out sprite))
            {
                return sprite;
            }
            throw new ArgumentException("Sprite " + spriteName + " not found in atlas " + atlasName);
        }

        void IResourceManager.ClearAtlas(string atlasName)
        {
            _atlases.Remove(atlasName);
        }

        Sprite IResourceManager.LoadSprite(string path)
        {
            return Resources.Load<Sprite>(path);
        }
        Material IResourceManager.LoadMaterial(string path)
        {
            return Resources.Load<Material>(path);
        }
        AudioMixer IResourceManager.LoadAudioMixer(string path)
        {
            return Resources.Load<AudioMixer>(path);
        }
        Shader IResourceManager.LoadShader(string path)
        {
            return Resources.Load<Shader>(path);
        }
        Texture IResourceManager.LoadTexture(string path)
        {
            return Resources.Load<Texture>(path);
        }
        Font IResourceManager.LoadFont(string path)
        {
            return Resources.Load<Font>(path);
        }
        GameObject IResourceManager.LoadAndInstantiate(string path)
        {
            var obj = Resources.Load(path);
            if(obj == null) throw new ArgumentException("No object found at path "+path);
            return (GameObject)Object.Instantiate(obj);
        }
        ResourceRequest IResourceManager.LoadAudioClipAsync(string path)
        {
            return Resources.LoadAsync<AudioClip>(path);
        }
        AudioClip IResourceManager.LoadAudioClip(string path)
        {
            return Resources.Load<AudioClip>(path);
        }
        byte[] IResourceManager.LoadBytes(string path)
        {
            return ((TextAsset)Resources.Load(path)).bytes;
        }
        string IResourceManager.LoadText(string path)
        {
            return ((TextAsset)Resources.Load(path))?.text;
        }
        void IResourceManager.UnloadAsset(Object assetToUnload)
        {
            Resources.UnloadAsset(assetToUnload);
        }
    }
}