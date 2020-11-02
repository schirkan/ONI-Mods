// Decompiled with JetBrains decompiler
// Type: Klei.RootDirectory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Klei
{
  public class RootDirectory : IFileDirectory
  {
    private string id = "StandardFS";

    public string GetID() => this.id;

    public string GetRoot() => "";

    public byte[] ReadBytes(string filename) => File.ReadAllBytes(filename);

    public string ReadText(string filename) => Encoding.UTF8.GetString(this.ReadBytes(filename));

    public void GetFiles(Regex re, string path, ICollection<string> result)
    {
      if (!Directory.Exists(path))
        return;
      foreach (string file in Directory.GetFiles(path))
      {
        string input = FileSystem.Normalize(file);
        if (re.IsMatch(input))
          result.Add(input);
      }
    }

    public bool FileExists(string path) => File.Exists(path);

    public FileHandle FindFileHandle(string path)
    {
      if (!this.FileExists(path))
        return new FileHandle();
      return new FileHandle()
      {
        full_path = FileSystem.Normalize(path),
        source = (IFileDirectory) this
      };
    }
  }
}
