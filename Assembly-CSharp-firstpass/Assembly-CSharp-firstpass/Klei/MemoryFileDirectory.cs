// Decompiled with JetBrains decompiler
// Type: Klei.MemoryFileDirectory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Klei
{
  public class MemoryFileDirectory : IFileDirectory
  {
    private string id;
    private string mountPoint;
    private Dictionary<string, byte[]> dataMap = new Dictionary<string, byte[]>();

    public string GetID() => this.id;

    public MemoryFileDirectory(string id, string mount_point = "")
    {
      this.id = id;
      this.mountPoint = FileSystem.Normalize(mount_point);
    }

    public string GetRoot() => this.mountPoint;

    public byte[] ReadBytes(string filename)
    {
      byte[] numArray = (byte[]) null;
      this.dataMap.TryGetValue(filename, out numArray);
      return numArray;
    }

    private string GetFullFilename(string filename) => Path.Combine(this.mountPoint, FileSystem.Normalize(filename));

    public void Map(string filename, byte[] data)
    {
      string fullFilename = this.GetFullFilename(filename);
      if (this.dataMap.ContainsKey(fullFilename))
        throw new ArgumentException(string.Format("MemoryFileSystem: '{0}' is already mapped.", (object[]) Array.Empty<object>()));
      this.dataMap[fullFilename] = data;
    }

    public void Unmap(string filename) => this.dataMap.Remove(this.GetFullFilename(filename));

    public void Clear() => this.dataMap.Clear();

    public void GetFiles(Regex re, string path, ICollection<string> result)
    {
      foreach (string key in this.dataMap.Keys)
      {
        if (re.IsMatch(key))
          result.Add(key);
      }
    }

    public bool FileExists(string path) => this.dataMap.ContainsKey(path);

    public FileHandle FindFileHandle(string path)
    {
      if (!this.FileExists(path))
        return new FileHandle();
      return new FileHandle()
      {
        full_path = path,
        source = (IFileDirectory) this
      };
    }
  }
}
