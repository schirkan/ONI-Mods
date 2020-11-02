// Decompiled with JetBrains decompiler
// Type: KMod.Local
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System.IO;
using UnityEngine;

namespace KMod
{
  public class Local : IDistributionPlatform
  {
    public string folder { get; private set; }

    public Label.DistributionPlatform distribution_platform { get; private set; }

    public string GetDirectory() => FileSystem.Normalize(System.IO.Path.Combine(Manager.GetDirectory(), this.folder));

    private void Subscribe(string id, long timestamp, IFileSource file_source)
    {
      FileHandle fileHandle = file_source.GetFileSystem().FindFileHandle(System.IO.Path.Combine(file_source.GetRoot(), "mod.yaml"));
      Local.Header header = fileHandle.full_path != null ? YamlIO.LoadFile<Local.Header>(fileHandle) : (Local.Header) null;
      if (header == null)
        header = new Local.Header()
        {
          title = id,
          description = id
        };
      Mod mod = new Mod(new Label()
      {
        id = id,
        distribution_platform = this.distribution_platform,
        version = (long) id.GetHashCode(),
        title = header.title
      }, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, (System.Action) (() => Application.OpenURL("file://" + file_source.GetRoot())));
      if (file_source.GetType() == typeof (Directory))
        mod.status = Mod.Status.Installed;
      Global.Instance.modManager.Subscribe(mod, (object) this);
    }

    public Local(string folder, Label.DistributionPlatform distribution_platform)
    {
      this.folder = folder;
      this.distribution_platform = distribution_platform;
      DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
      if (!directoryInfo.Exists)
        return;
      foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        this.Subscribe(directory.Name, directory.LastWriteTime.ToFileTime(), (IFileSource) new Directory(directory.FullName));
    }

    private class Header
    {
      public string title { get; set; }

      public string description { get; set; }
    }
  }
}
