// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Utilities.ResourceManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework.Utilities
{
  public static class ResourceManager
  {
    private static List<ResourceManager.MemoryTexture> loadedTextures = new List<ResourceManager.MemoryTexture>();

    public static void SetDefaultResourcePath(string defaultResourcePath)
    {
    }

    public static string PreparePath(string path)
    {
      path = path.Replace(Application.dataPath, "Assets");
      if (path.Contains("Resources"))
        path = path.Substring(path.LastIndexOf("Resources") + 10);
      return path.Substring(0, path.LastIndexOf('.'));
    }

    public static T[] LoadResources<T>(string path) where T : UnityEngine.Object
    {
      path = ResourceManager.PreparePath(path);
      throw new NotImplementedException("Currently it is not possible to load subAssets at runtime!");
    }

    public static T LoadResource<T>(string path) where T : UnityEngine.Object
    {
      path = ResourceManager.PreparePath(path);
      return Resources.Load<T>(path);
    }

    public static Texture2D LoadTexture(string texPath)
    {
      if (string.IsNullOrEmpty(texPath))
        return (Texture2D) null;
      int index = ResourceManager.loadedTextures.FindIndex((Predicate<ResourceManager.MemoryTexture>) (memTex => memTex.path == texPath));
      if (index != -1)
      {
        if (!((UnityEngine.Object) ResourceManager.loadedTextures[index].texture == (UnityEngine.Object) null))
          return ResourceManager.loadedTextures[index].texture;
        ResourceManager.loadedTextures.RemoveAt(index);
      }
      Texture2D texture = ResourceManager.LoadResource<Texture2D>(texPath);
      ResourceManager.AddTextureToMemory(texPath, texture, (string[]) Array.Empty<string>());
      return texture;
    }

    public static Texture2D GetTintedTexture(string texPath, Color col)
    {
      string str = "Tint:" + col.ToString();
      Texture2D texture = ResourceManager.GetTexture(texPath, str);
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
      {
        Texture2D texture2D = ResourceManager.LoadTexture(texPath);
        ResourceManager.AddTextureToMemory(texPath, texture2D, (string[]) Array.Empty<string>());
        texture = RTEditorGUI.Tint(texture2D, col);
        ResourceManager.AddTextureToMemory(texPath, texture, str);
      }
      return texture;
    }

    public static void AddTextureToMemory(
      string texturePath,
      Texture2D texture,
      params string[] modifications)
    {
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
        return;
      ResourceManager.loadedTextures.Add(new ResourceManager.MemoryTexture(texturePath, texture, modifications));
    }

    public static ResourceManager.MemoryTexture FindInMemory(Texture2D tex)
    {
      int index = ResourceManager.loadedTextures.FindIndex((Predicate<ResourceManager.MemoryTexture>) (memTex => (UnityEngine.Object) memTex.texture == (UnityEngine.Object) tex));
      return index == -1 ? (ResourceManager.MemoryTexture) null : ResourceManager.loadedTextures[index];
    }

    public static bool HasInMemory(string texturePath, params string[] modifications)
    {
      int index = ResourceManager.loadedTextures.FindIndex((Predicate<ResourceManager.MemoryTexture>) (memTex => memTex.path == texturePath));
      return index != -1 && ResourceManager.EqualModifications(ResourceManager.loadedTextures[index].modifications, modifications);
    }

    public static ResourceManager.MemoryTexture GetMemoryTexture(
      string texturePath,
      params string[] modifications)
    {
      List<ResourceManager.MemoryTexture> all = ResourceManager.loadedTextures.FindAll((Predicate<ResourceManager.MemoryTexture>) (memTex => memTex.path == texturePath));
      if (all == null || all.Count == 0)
        return (ResourceManager.MemoryTexture) null;
      foreach (ResourceManager.MemoryTexture memoryTexture in all)
      {
        if (ResourceManager.EqualModifications(memoryTexture.modifications, modifications))
          return memoryTexture;
      }
      return (ResourceManager.MemoryTexture) null;
    }

    public static Texture2D GetTexture(string texturePath, params string[] modifications) => ResourceManager.GetMemoryTexture(texturePath, modifications)?.texture;

    private static bool EqualModifications(string[] modsA, string[] modsB) => modsA.Length == modsB.Length && Array.TrueForAll<string>(modsA, (Predicate<string>) (mod => ((IEnumerable<string>) modsB).Count<string>((Func<string, bool>) (oMod => mod == oMod)) == ((IEnumerable<string>) modsA).Count<string>((Func<string, bool>) (oMod => mod == oMod))));

    public class MemoryTexture
    {
      public string path;
      public Texture2D texture;
      public string[] modifications;

      public MemoryTexture(string texPath, Texture2D tex, params string[] mods)
      {
        this.path = texPath;
        this.texture = tex;
        this.modifications = mods;
      }
    }
  }
}
