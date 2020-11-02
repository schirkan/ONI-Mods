// Decompiled with JetBrains decompiler
// Type: KMod.IFileSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System.Collections.Generic;

namespace KMod
{
  public interface IFileSource
  {
    string GetRoot();

    bool Exists();

    void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root = "");

    IFileDirectory GetFileSystem();

    void CopyTo(string path, List<string> extensions = null);

    string Read(string relative_path);
  }
}
