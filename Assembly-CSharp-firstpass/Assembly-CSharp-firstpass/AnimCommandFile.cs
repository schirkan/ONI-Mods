// Decompiled with JetBrains decompiler
// Type: AnimCommandFile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;
using System;
using System.Collections.Generic;
using System.IO;

public class AnimCommandFile
{
  [NonSerialized]
  public string directory = "";
  [NonSerialized]
  private List<KAnimGroupFile.GroupFile> groupFiles = new List<KAnimGroupFile.GroupFile>();

  [StringEnumConverter]
  public AnimCommandFile.ConfigType Type { get; private set; }

  [StringEnumConverter]
  public AnimCommandFile.GroupBy TagGroup { get; private set; }

  [StringEnumConverter]
  public KAnimBatchGroup.RendererType RendererType { get; private set; }

  public string TargetBuild { get; private set; }

  public string AnimTargetBuild { get; private set; }

  public string SwapTargetBuild { get; private set; }

  public Dictionary<string, List<string>> DefaultBuilds { get; private set; }

  public int MaxGroupSize { get; private set; }

  public AnimCommandFile()
  {
    this.MaxGroupSize = 30;
    this.DefaultBuilds = new Dictionary<string, List<string>>();
    this.TagGroup = AnimCommandFile.GroupBy.DontGroup;
  }

  public bool IsSwap(KAnimFile file)
  {
    if (this.TagGroup != AnimCommandFile.GroupBy.NamedGroup)
      return false;
    string fileName = Path.GetFileName(file.homedirectory);
    foreach (KeyValuePair<string, List<string>> defaultBuild in this.DefaultBuilds)
    {
      if (defaultBuild.Value.Contains(fileName))
        return false;
    }
    return true;
  }

  public void AddGroupFile(KAnimGroupFile.GroupFile gf)
  {
    if (this.groupFiles.Contains(gf))
      return;
    this.groupFiles.Add(gf);
  }

  public string GetGroupName(KAnimFile kaf)
  {
    switch (this.TagGroup)
    {
      case AnimCommandFile.GroupBy.__IGNORE__:
        return (string) null;
      case AnimCommandFile.GroupBy.DontGroup:
        return kaf.name;
      case AnimCommandFile.GroupBy.Folder:
        return Path.GetFileName(this.directory) + (this.groupFiles.Count / 10).ToString();
      case AnimCommandFile.GroupBy.NamedGroup:
        string fileName = Path.GetFileName(kaf.homedirectory);
        foreach (KeyValuePair<string, List<string>> defaultBuild in this.DefaultBuilds)
        {
          if (defaultBuild.Value.Contains(fileName))
            return defaultBuild.Key;
        }
        return this.TargetBuild;
      case AnimCommandFile.GroupBy.NamedGroupNoSplit:
        return this.TargetBuild;
      default:
        return (string) null;
    }
  }

  public enum ConfigType
  {
    Default,
    AnimOnly,
  }

  public enum GroupBy
  {
    __IGNORE__,
    DontGroup,
    Folder,
    NamedGroup,
    NamedGroupNoSplit,
  }
}
