// Decompiled with JetBrains decompiler
// Type: Klei.AliasDirectory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Klei
{
  public class AliasDirectory : IFileDirectory
  {
    private string id;
    private string root;
    private string prefix;

    public string GetID() => this.id;

    public AliasDirectory(string id, string actual_location, string path_prefix)
    {
      this.id = id;
      actual_location = FileSystem.Normalize(actual_location);
      path_prefix = FileSystem.Normalize(path_prefix);
      this.root = actual_location;
      this.prefix = path_prefix;
    }

    private string GetActualPath(string filename) => filename.StartsWith(this.prefix) ? FileSystem.Normalize(this.root + filename.Substring(this.prefix.Length)) : filename;

    private string GetVirtualPath(string filename) => filename.StartsWith(this.root) ? FileSystem.Normalize(this.prefix + filename.Substring(this.root.Length)) : filename;

    public string GetRoot() => this.root;

    public byte[] ReadBytes(string src_filename)
    {
      string actualPath = this.GetActualPath(src_filename);
      return !File.Exists(actualPath) ? (byte[]) null : File.ReadAllBytes(actualPath);
    }

    public void GetFiles(Regex re, string src_path, ICollection<string> result)
    {
      string actualPath = this.GetActualPath(src_path);
      if (!Directory.Exists(actualPath))
        return;
      foreach (string file in Directory.GetFiles(actualPath))
      {
        string virtualPath = this.GetVirtualPath(FileSystem.Normalize(file));
        if (re.IsMatch(virtualPath))
          result.Add(virtualPath);
      }
    }

    public bool FileExists(string path) => File.Exists(this.GetActualPath(path));

    public FileHandle FindFileHandle(string path)
    {
      if (!this.FileExists(path))
        return new FileHandle();
      path = this.GetVirtualPath(FileSystem.Normalize(path));
      return new FileHandle()
      {
        full_path = path,
        source = (IFileDirectory) this
      };
    }
  }
}
