// Decompiled with JetBrains decompiler
// Type: KMod.Mod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace KMod
{
  [JsonObject(MemberSerialization.OptIn)]
  [DebuggerDisplay("{title}")]
  public class Mod
  {
    [JsonProperty]
    public Label label;
    [JsonProperty]
    public Mod.Status status;
    [JsonProperty]
    public bool enabled;
    [JsonProperty]
    public int crash_count;
    [JsonProperty]
    public string reinstall_path;
    public bool foundInStackTrace;
    public string relative_root = "";
    public LoadedModData loaded_mod_data;
    public IFileSource file_source;
    public IFileSource content_source;
    public bool is_subscribed;
    private const string ARCHIVED_VERSIONS_FILENAME = "archived_versions.yaml";
    public const int MAX_CRASH_COUNT = 3;

    public Content available_content { get; private set; }

    public LocString manage_tooltip { get; private set; }

    public System.Action on_managed { get; private set; }

    public bool is_managed => this.manage_tooltip != null;

    public string title => this.label.title;

    public string description { get; private set; }

    public Content loaded_content { get; private set; }

    [JsonConstructor]
    public Mod()
    {
    }

    public void CopyPersistentDataTo(Mod other_mod)
    {
      other_mod.status = this.status;
      other_mod.enabled = this.enabled;
      other_mod.crash_count = this.crash_count;
      other_mod.loaded_content = this.loaded_content;
      other_mod.loaded_mod_data = this.loaded_mod_data;
      other_mod.reinstall_path = this.reinstall_path;
    }

    public Mod(
      Label label,
      string description,
      IFileSource file_source,
      LocString manage_tooltip,
      System.Action on_managed)
    {
      this.enabled = false;
      this.label = label;
      this.status = Mod.Status.NotInstalled;
      this.description = description;
      this.file_source = file_source;
      this.manage_tooltip = manage_tooltip;
      this.on_managed = on_managed;
      this.loaded_content = (Content) 0;
      this.available_content = (Content) 0;
      this.ScanContent();
    }

    public void ScanContent()
    {
      this.available_content = (Content) 0;
      if (this.file_source == null)
        this.file_source = (IFileSource) new Directory(this.label.install_path);
      if (!this.file_source.Exists())
        return;
      this.ScanContentFromSource();
      if (this.content_source != null)
        return;
      this.content_source = (IFileSource) new Directory(this.ContentPath);
    }

    private void ScanContentFromSource(string relativeRoot = "")
    {
      this.available_content = (Content) 0;
      List<FileSystemItem> file_system_items = new List<FileSystemItem>();
      this.file_source.GetTopLevelItems(file_system_items, relativeRoot);
      bool flag = false;
      foreach (FileSystemItem fileSystemItem in file_system_items)
      {
        if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
        {
          this.AddDirectory(fileSystemItem.name.ToLower());
        }
        else
        {
          string lower = fileSystemItem.name.ToLower();
          if (lower == "archived_versions.yaml")
            flag = true;
          else
            this.AddFile(lower);
        }
      }
      if (!flag)
        return;
      if (!string.IsNullOrEmpty(this.relative_root))
      {
        Debug.LogWarning((object) ("archived version at " + this.relative_root + " also has archived_versions.yaml, ignoring."));
      }
      else
      {
        string readText = this.file_source.Read("archived_versions.yaml");
        if (string.IsNullOrEmpty(readText))
        {
          Debug.LogWarning((object) "Failed to read archived_versions.yaml, skipping");
        }
        else
        {
          Mod.ArchivedVersionArray archivedVersionArray = YamlIO.Parse<Mod.ArchivedVersionArray>(readText, new FileHandle());
          if (archivedVersionArray == null)
          {
            Debug.LogWarning((object) ("Failed to parse archived_versions.yaml, text is " + readText));
          }
          else
          {
            Mod.ArchivedVersion archivedVersion1 = (Mod.ArchivedVersion) null;
            foreach (Mod.ArchivedVersion archivedVersion2 in archivedVersionArray.archivedVersions)
            {
              if (420700 <= archivedVersion2.lastWorkingBuild && (archivedVersion1 == null || archivedVersion2.lastWorkingBuild < archivedVersion1.lastWorkingBuild))
                archivedVersion1 = archivedVersion2;
            }
            if (archivedVersion1 == null)
              return;
            this.relative_root = FileSystem.Normalize(archivedVersion1.relativePath);
            if (!this.relative_root.StartsWith("archived_version"))
            {
              Debug.LogError((object) ("Archived version with path: " + archivedVersion1.relativePath + ". For consistency among mods, please keep all old versions in a top-level directory called \"archived_versions\""));
            }
            else
            {
              Debug.Log((object) string.Format("Found archived version for mod {0} with lastWorkingBuild: {1}, redirecting content path to {2}", (object) this.title, (object) archivedVersion1.lastWorkingBuild, (object) this.relative_root));
              this.ScanContentFromSource(this.relative_root);
            }
          }
        }
      }
    }

    public string ContentPath => System.IO.Path.Combine(this.label.install_path, this.relative_root);

    public bool IsEmpty() => this.available_content == (Content) 0;

    private void AddDirectory(string directory)
    {
      string str = directory.TrimEnd('/');
      if (!(str == "strings"))
      {
        if (!(str == "codex"))
        {
          if (!(str == "elements"))
          {
            if (!(str == "templates"))
            {
              if (!(str == "worldgen"))
              {
                if (!(str == "anim"))
                  return;
                this.available_content |= Content.Animation;
              }
              else
                this.available_content |= Content.LayerableFiles;
            }
            else
              this.available_content |= Content.LayerableFiles;
          }
          else
            this.available_content |= Content.LayerableFiles;
        }
        else
          this.available_content |= Content.LayerableFiles;
      }
      else
        this.available_content |= Content.Strings;
    }

    private void AddFile(string file)
    {
      if (file.EndsWith(".dll"))
        this.available_content |= Content.DLL;
      if (!file.EndsWith(".po"))
        return;
      this.available_content |= Content.Translation;
    }

    private static void AccumulateExtensions(Content content, List<string> extensions)
    {
      if ((content & Content.DLL) != (Content) 0)
        extensions.Add(".dll");
      if ((content & (Content.Strings | Content.Translation)) == (Content) 0)
        return;
      extensions.Add(".po");
    }

    [Conditional("DEBUG")]
    private void Assert(bool condition, string failure_message)
    {
      if (string.IsNullOrEmpty(this.title))
        DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", (object) this.title, (object) this.label.ToString(), (object) failure_message));
      else
        DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", (object) this.label.ToString(), (object) failure_message));
    }

    public void Install()
    {
      if (this.IsLocal)
      {
        this.status = Mod.Status.Installed;
      }
      else
      {
        this.status = Mod.Status.ReinstallPending;
        if (this.file_source == null || !FileUtil.DeleteDirectory(this.label.install_path) || !FileUtil.CreateDirectory(this.label.install_path))
          return;
        this.file_source.CopyTo(this.label.install_path);
        this.file_source = (IFileSource) new Directory(this.label.install_path);
        this.content_source = (IFileSource) new Directory(this.ContentPath);
        this.status = Mod.Status.Installed;
      }
    }

    public bool Uninstall()
    {
      this.enabled = false;
      if (this.loaded_content != (Content) 0)
      {
        Debug.Log((object) string.Format("Can't uninstall {0}: still has loaded content: {1}", (object) this.label.ToString(), (object) this.loaded_content.ToString()));
        this.status = Mod.Status.UninstallPending;
        return false;
      }
      if (!this.IsLocal && !FileUtil.DeleteDirectory(this.label.install_path))
      {
        Debug.Log((object) string.Format("Can't uninstall {0}: directory deletion failed", (object) this.label.ToString()));
        this.status = Mod.Status.UninstallPending;
        return false;
      }
      this.status = Mod.Status.NotInstalled;
      return true;
    }

    private bool LoadStrings()
    {
      string path = FileSystem.Normalize(System.IO.Path.Combine(this.ContentPath, "strings"));
      if (!System.IO.Directory.Exists(path))
        return false;
      int num = 0;
      foreach (FileInfo file in new DirectoryInfo(path).GetFiles())
      {
        if (!(file.Extension.ToLower() != ".po"))
        {
          ++num;
          Localization.OverloadStrings(Localization.LoadStringsFile(file.FullName, false));
        }
      }
      return true;
    }

    private bool LoadTranslations() => false;

    private bool LoadAnimation()
    {
      string path = FileSystem.Normalize(System.IO.Path.Combine(this.ContentPath, "anim"));
      if (!System.IO.Directory.Exists(path))
        return false;
      int num = 0;
      foreach (DirectoryInfo directory1 in new DirectoryInfo(path).GetDirectories())
      {
        foreach (DirectoryInfo directory2 in directory1.GetDirectories())
        {
          KAnimFile.Mod anim_mod = new KAnimFile.Mod();
          foreach (FileInfo file in directory2.GetFiles())
          {
            if (file.Extension == ".png")
            {
              byte[] data = File.ReadAllBytes(file.FullName);
              Texture2D tex = new Texture2D(2, 2);
              tex.LoadImage(data);
              anim_mod.textures.Add(tex);
            }
            else if (file.Extension == ".bytes")
            {
              string withoutExtension = System.IO.Path.GetFileNameWithoutExtension(file.Name);
              byte[] numArray = File.ReadAllBytes(file.FullName);
              if (withoutExtension.EndsWith("_anim"))
                anim_mod.anim = numArray;
              else if (withoutExtension.EndsWith("_build"))
                anim_mod.build = numArray;
              else
                DebugUtil.LogWarningArgs((object) string.Format("Unhandled TextAsset ({0})...ignoring", (object) file.FullName));
            }
            else
              DebugUtil.LogWarningArgs((object) string.Format("Unhandled asset ({0})...ignoring", (object) file.FullName));
          }
          string name = directory2.Name + "_kanim";
          if (anim_mod.IsValid() && (bool) (UnityEngine.Object) ModUtil.AddKAnimMod(name, anim_mod))
            ++num;
        }
      }
      return true;
    }

    public void Load(Content content)
    {
      content &= this.available_content & ~this.loaded_content;
      if (content > (Content) 0)
        Debug.Log((object) string.Format("Loading mod content {2} [{0}:{1}] (provides {3})", (object) this.title, (object) this.label.id, (object) content.ToString(), (object) this.available_content.ToString()));
      if ((content & Content.Strings) != (Content) 0 && this.LoadStrings())
        this.loaded_content |= Content.Strings;
      if ((content & Content.Translation) != (Content) 0 && this.LoadTranslations())
        this.loaded_content |= Content.Translation;
      if ((content & Content.DLL) != (Content) 0)
      {
        this.loaded_mod_data = DLLLoader.LoadDLLs(this.label.id + "." + (object) this.label.distribution_platform, this.ContentPath);
        if (this.loaded_mod_data != null)
          this.loaded_content |= Content.DLL;
      }
      if ((content & Content.LayerableFiles) != (Content) 0)
      {
        Debug.Assert(this.content_source != null, (object) "Attempting to Load layerable files with content_source not initialized");
        FileSystem.file_sources.Insert(0, this.content_source.GetFileSystem());
        this.loaded_content |= Content.LayerableFiles;
      }
      if ((content & Content.Animation) == (Content) 0 || !this.LoadAnimation())
        return;
      this.loaded_content |= Content.Animation;
    }

    public void Unload(Content content)
    {
      content &= this.loaded_content;
      if ((content & Content.LayerableFiles) == (Content) 0)
        return;
      FileSystem.file_sources.Remove(this.content_source.GetFileSystem());
      this.loaded_content &= ~Content.LayerableFiles;
    }

    private void SetCrashCount(int new_crash_count) => this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);

    public bool IsDev => this.label.distribution_platform == Label.DistributionPlatform.Dev;

    public bool IsLocal => this.label.distribution_platform == Label.DistributionPlatform.Dev || this.label.distribution_platform == Label.DistributionPlatform.Local;

    public void SetCrashed()
    {
      this.SetCrashCount(this.crash_count + 1);
      if (this.IsDev)
        return;
      this.enabled = false;
    }

    public void Uncrash() => this.SetCrashCount(this.IsDev ? this.crash_count - 1 : 0);

    public bool IsActive() => (uint) this.loaded_content > 0U;

    public bool AllActive(Content content) => (this.loaded_content & content) == content;

    public bool AllActive() => (this.loaded_content & this.available_content) == this.available_content;

    public bool AnyActive(Content content) => (uint) (this.loaded_content & content) > 0U;

    public bool HasContent() => (uint) this.available_content > 0U;

    public bool HasAnyContent(Content content) => (uint) (this.available_content & content) > 0U;

    public bool HasOnlyTranslationContent() => this.available_content == Content.Translation;

    public enum Status
    {
      NotInstalled,
      Installed,
      UninstallPending,
      ReinstallPending,
    }

    public class ArchivedVersionArray
    {
      public Mod.ArchivedVersion[] archivedVersions { get; set; }

      public ArchivedVersionArray() => this.archivedVersions = new Mod.ArchivedVersion[0];
    }

    public class ArchivedVersion
    {
      public string relativePath { get; set; }

      public int lastWorkingBuild { get; set; }
    }
  }
}
