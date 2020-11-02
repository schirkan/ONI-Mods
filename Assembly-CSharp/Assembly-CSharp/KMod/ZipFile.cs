// Decompiled with JetBrains decompiler
// Type: KMod.ZipFile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Ionic.Zip;
using Klei;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KMod
{
  internal struct ZipFile : IFileSource
  {
    private string filename;
    private Ionic.Zip.ZipFile zipfile;
    private ZipFileDirectory file_system;

    public ZipFile(string filename)
    {
      this.filename = filename;
      this.zipfile = Ionic.Zip.ZipFile.Read(filename);
      this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath);
    }

    public string GetRoot() => this.filename;

    public bool Exists() => File.Exists(this.GetRoot());

    public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
    {
      HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
      relative_root = relative_root ?? "";
      relative_root = FileSystem.Normalize(relative_root);
      foreach (ZipEntry zipEntry in this.zipfile)
      {
        string str1 = FileSystem.Normalize(zipEntry.FileName);
        if (str1.StartsWith(relative_root))
        {
          string[] strArray = str1.Remove(0, relative_root.Length).Split('/');
          string str2 = strArray[0];
          if (pooledHashSet.Add(str2))
            file_system_items.Add(new FileSystemItem()
            {
              name = str2,
              type = 1 < strArray.Length ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File
            });
        }
      }
      pooledHashSet.Recycle();
    }

    public IFileDirectory GetFileSystem() => (IFileDirectory) this.file_system;

    public void CopyTo(string path, List<string> extensions = null)
    {
      foreach (ZipEntry entry in (IEnumerable<ZipEntry>) this.zipfile.Entries)
      {
        bool flag = extensions == null || extensions.Count == 0;
        if (extensions != null)
        {
          foreach (string extension in extensions)
          {
            if (entry.FileName.ToLower().EndsWith(extension))
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
        {
          string str = FileSystem.Normalize(System.IO.Path.Combine(path, entry.FileName));
          string directoryName = System.IO.Path.GetDirectoryName(str);
          if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName))
          {
            using (MemoryStream memoryStream = new MemoryStream((int) entry.UncompressedSize))
            {
              entry.Extract((Stream) memoryStream);
              using (FileStream fileStream = FileUtil.Create(str))
                fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
            }
          }
        }
      }
    }

    public string Read(string relative_path)
    {
      ICollection<ZipEntry> zipEntries = this.zipfile.SelectEntries(relative_path);
      if (zipEntries.Count == 0)
        return string.Empty;
      using (IEnumerator<ZipEntry> enumerator = zipEntries.GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          ZipEntry current = enumerator.Current;
          using (MemoryStream memoryStream = new MemoryStream((int) current.UncompressedSize))
          {
            current.Extract((Stream) memoryStream);
            return Encoding.UTF8.GetString(memoryStream.GetBuffer());
          }
        }
      }
      return string.Empty;
    }
  }
}
