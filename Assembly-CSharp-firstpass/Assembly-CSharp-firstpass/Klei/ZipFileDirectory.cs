// Decompiled with JetBrains decompiler
// Type: Klei.ZipFileDirectory
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Ionic.Zip;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Klei
{
  public class ZipFileDirectory : IFileDirectory
  {
    private string id;
    private string mountPoint;
    private ZipFile zipfile;

    public string GetID() => this.id;

    public ZipFileDirectory(string id, ZipFile zipfile, string mount_point = "")
    {
      this.id = id;
      this.mountPoint = FileSystem.Normalize(mount_point);
      this.zipfile = zipfile;
    }

    public ZipFileDirectory(string id, Stream zip_data_stream, string mount_point = "")
      : this(id, ZipFile.Read(zip_data_stream), mount_point)
    {
    }

    public string MountPoint => this.mountPoint;

    public string GetRoot() => this.MountPoint;

    public byte[] ReadBytes(string filename)
    {
      if (this.mountPoint.Length > 0)
        filename = filename.Substring(this.mountPoint.Length);
      ZipEntry zipEntry = this.zipfile[filename];
      if (zipEntry == null)
        return (byte[]) null;
      MemoryStream memoryStream = new MemoryStream();
      zipEntry.Extract((Stream) memoryStream);
      return memoryStream.ToArray();
    }

    public void GetFiles(Regex re, string path, ICollection<string> result)
    {
      if (this.zipfile.Count <= 0)
        return;
      foreach (ZipEntry entry in (IEnumerable<ZipEntry>) this.zipfile.Entries)
      {
        if (!entry.IsDirectory)
        {
          string input = FileSystem.Normalize(Path.Combine(this.mountPoint, entry.FileName));
          if (re.IsMatch(input))
            result.Add(input);
        }
      }
    }

    public bool FileExists(string path)
    {
      if (this.mountPoint.Length > 0)
        path = path.Substring(this.mountPoint.Length);
      return this.zipfile.ContainsEntry(path);
    }

    public FileHandle FindFileHandle(string path)
    {
      if (!this.FileExists(path))
        return new FileHandle();
      if (this.mountPoint.Length > 0)
        path = path.Substring(this.mountPoint.Length);
      return new FileHandle()
      {
        full_path = FileSystem.Normalize(Path.Combine(this.mountPoint, path)),
        source = (IFileDirectory) this
      };
    }
  }
}
