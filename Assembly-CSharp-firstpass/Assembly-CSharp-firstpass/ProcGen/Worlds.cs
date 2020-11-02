// Decompiled with JetBrains decompiler
// Type: ProcGen.Worlds
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
  public class Worlds
  {
    public Dictionary<string, World> worldCache = new Dictionary<string, World>();

    public bool HasWorld(string name) => name != null && this.worldCache.ContainsKey(name);

    public World GetWorldData(string name)
    {
      World world;
      return this.worldCache.TryGetValue(name, out world) ? world : this.worldCache["worlds/SandstoneDefault"];
    }

    public List<string> GetNames() => new List<string>((IEnumerable<string>) this.worldCache.Keys);

    public static string GetWorldName(string path) => "worlds/" + System.IO.Path.GetFileNameWithoutExtension(path);

    public void LoadFiles(string path, List<YamlIO.Error> errors)
    {
      this.worldCache.Clear();
      this.UpdateWorldCache(path, errors);
    }

    private void UpdateWorldCache(string path, List<YamlIO.Error> errors)
    {
      ListPool<FileHandle, Worlds>.PooledList pooledList = ListPool<FileHandle, Worlds>.Allocate();
      FileSystem.GetFiles(FileSystem.Normalize(System.IO.Path.Combine(path, "worlds")), "*.yaml", (ICollection<FileHandle>) pooledList);
      foreach (FileHandle fileHandle in (List<FileHandle>) pooledList)
      {
        World world = YamlIO.LoadFile<World>(fileHandle.full_path, (YamlIO.ErrorHandler) ((error, force_log_as_warning) => errors.Add(error)));
        if (world == null)
          DebugUtil.LogWarningArgs((object) "Failed to load world: ", (object) fileHandle.full_path);
        else if (world.skip != World.Skip.Always && (world.skip != World.Skip.EditorOnly || Application.isEditor))
        {
          world.filePath = Worlds.GetWorldName(fileHandle.full_path);
          this.worldCache[world.filePath] = world;
        }
      }
      pooledList.Recycle();
    }
  }
}
