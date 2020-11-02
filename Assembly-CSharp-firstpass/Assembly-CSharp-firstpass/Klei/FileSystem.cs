// Decompiled with JetBrains decompiler
// Type: Klei.FileSystem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Klei
{
  public static class FileSystem
  {
    public static List<IFileDirectory> file_sources = new List<IFileDirectory>();

    public static void Initialize()
    {
      if (FileSystem.file_sources.Count != 0)
        return;
      FileSystem.file_sources.Add((IFileDirectory) new RootDirectory());
    }

    public static byte[] ReadBytes(string filename)
    {
      FileSystem.Initialize();
      foreach (IFileDirectory fileSource in FileSystem.file_sources)
      {
        byte[] numArray = fileSource.ReadBytes(filename);
        if (numArray != null)
          return numArray;
      }
      return (byte[]) null;
    }

    public static FileHandle FindFileHandle(string filename)
    {
      FileSystem.Initialize();
      foreach (IFileDirectory fileSource in FileSystem.file_sources)
      {
        if (fileSource.FileExists(filename))
          return fileSource.FindFileHandle(filename);
      }
      return new FileHandle();
    }

    public static void GetFiles(Regex re, string path, ICollection<FileHandle> result)
    {
      FileSystem.Initialize();
      ListPool<string, IFileDirectory>.PooledList pooledList = ListPool<string, IFileDirectory>.Allocate();
      foreach (IFileDirectory fileSource in FileSystem.file_sources)
      {
        pooledList.Clear();
        fileSource.GetFiles(re, path, (ICollection<string>) pooledList);
        foreach (string str in (List<string>) pooledList)
          result.Add(new FileHandle()
          {
            full_path = str,
            source = fileSource
          });
      }
      pooledList.Recycle();
    }

    public static void GetFiles(Regex re, string path, ICollection<string> result)
    {
      FileSystem.Initialize();
      foreach (IFileDirectory fileSource in FileSystem.file_sources)
        fileSource.GetFiles(re, path, result);
    }

    public static void GetFiles(
      string path,
      string filename_glob_pattern,
      ICollection<string> result)
    {
      string normalized_path;
      Regex filename_regex;
      FileSystem.GetFilesSearchParams(path, filename_glob_pattern, out normalized_path, out filename_regex);
      FileSystem.GetFiles(filename_regex, normalized_path, result);
    }

    public static void GetFiles(
      string path,
      string filename_glob_pattern,
      ICollection<FileHandle> result)
    {
      string normalized_path;
      Regex filename_regex;
      FileSystem.GetFilesSearchParams(path, filename_glob_pattern, out normalized_path, out filename_regex);
      FileSystem.GetFiles(filename_regex, normalized_path, result);
    }

    public static void GetFiles(string filename, ICollection<FileHandle> result)
    {
      string normalized_path;
      Regex filename_regex;
      FileSystem.GetFilesSearchParams(Path.GetDirectoryName(filename), Path.GetFileName(filename), out normalized_path, out filename_regex);
      FileSystem.GetFiles(filename_regex, normalized_path, result);
    }

    public static bool FileExists(string path)
    {
      FileSystem.Initialize();
      foreach (IFileDirectory fileSource in FileSystem.file_sources)
      {
        if (fileSource.FileExists(path))
          return true;
      }
      return false;
    }

    public static void ReadFiles(string filename, ICollection<byte[]> result)
    {
      FileSystem.Initialize();
      foreach (IFileDirectory fileSource in FileSystem.file_sources)
      {
        byte[] numArray = fileSource.ReadBytes(filename);
        if (numArray != null)
          result.Add(numArray);
      }
    }

    public static string ConvertToText(byte[] bytes) => Encoding.UTF8.GetString(bytes);

    public static string Normalize(string filename) => filename.Replace("\\", "/");

    private static void GetFilesSearchParams(
      string path,
      string filename_glob_pattern,
      out string normalized_path,
      out Regex filename_regex)
    {
      normalized_path = (string) null;
      filename_regex = (Regex) null;
      int index = path.Length - 1;
      while (index >= 0 && path[index] == '\\' || path[index] == '/')
        --index;
      if (index < 0)
        return;
      if (index < path.Length - 1)
        path = path.Substring(0, index + 1);
      normalized_path = path = FileSystem.Normalize(path);
      string str = filename_glob_pattern.Replace(".", "\\.").Replace("*", ".*");
      string pattern = path.Replace("\\", "\\\\").Replace("/", "\\/").Replace("(", "\\(").Replace(")", "\\)").Replace("[", "\\[").Replace("]", "\\]").Replace(".", "\\.").Replace("+", "\\+") + "/" + str + "$";
      filename_regex = new Regex(pattern);
    }
  }
}
