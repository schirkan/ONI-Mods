// Decompiled with JetBrains decompiler
// Type: KAnimGroupFile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KAnimGroupFile : ScriptableObject
{
  private const string MASTER_GROUP_FILE = "animgrouptags";
  public const int MAX_ANIMS_PER_GROUP = 10;
  private static KAnimGroupFile groupfile;
  private Dictionary<int, KAnimFileData> fileData = new Dictionary<int, KAnimFileData>();
  [SerializeField]
  private List<KAnimGroupFile.Group> groups = new List<KAnimGroupFile.Group>();
  [SerializeField]
  private List<Pair<HashedString, HashedString>> currentGroup = new List<Pair<HashedString, HashedString>>();
  private static bool hasCompletedLoadAll;

  public static void DestroyInstance() => KAnimGroupFile.groupfile = (KAnimGroupFile) null;

  public static string GetFilePath() => "Assets/anim/resources/animgrouptags.asset";

  public static KAnimGroupFile GetGroupFile()
  {
    if ((UnityEngine.Object) KAnimGroupFile.groupfile == (UnityEngine.Object) null)
      KAnimGroupFile.groupfile = (KAnimGroupFile) Resources.Load("animgrouptags", typeof (KAnimGroupFile));
    return KAnimGroupFile.groupfile;
  }

  public static void SetGroupFile(KAnimGroupFile file)
  {
    KAnimGroupFile.groupfile = file;
    KAnimGroupFile.groupfile.Sort();
  }

  public static KAnimGroupFile.Group GetGroup(HashedString tag)
  {
    KAnimGroupFile.Group group1 = (KAnimGroupFile.Group) null;
    KAnimGroupFile.GetGroupFile();
    List<KAnimGroupFile.Group> groups = KAnimGroupFile.groupfile.groups;
    Debug.Assert(groups != null, (object) (groups.Count > 0));
    for (int index = 0; index < groups.Count; ++index)
    {
      KAnimGroupFile.Group group2 = groups[index];
      if (group2.id == tag || group2.target == tag)
      {
        group1 = group2;
        break;
      }
    }
    return group1;
  }

  public HashedString GetGroupForHomeDirectory(HashedString homedirectory)
  {
    for (int index = 0; index < this.currentGroup.Count; ++index)
    {
      if (this.currentGroup[index].first == homedirectory)
        return this.currentGroup[index].second;
    }
    return new HashedString();
  }

  public List<KAnimGroupFile.Group> GetData() => this.groups;

  public void Reset()
  {
    this.groups = new List<KAnimGroupFile.Group>();
    this.currentGroup = new List<Pair<HashedString, HashedString>>();
  }

  private int AddGroup(AnimCommandFile akf, KAnimGroupFile.GroupFile gf, KAnimFile file)
  {
    bool flag = akf.IsSwap(file);
    HashedString groupId = new HashedString(gf.groupID);
    int num = this.groups.FindIndex((Predicate<KAnimGroupFile.Group>) (t => t.id == groupId));
    if (num == -1)
    {
      num = this.groups.Count;
      KAnimGroupFile.Group group = new KAnimGroupFile.Group(groupId);
      group.commandDirectory = akf.directory;
      group.maxGroupSize = akf.MaxGroupSize;
      group.renderType = akf.RendererType;
      if (this.groups.FindIndex((Predicate<KAnimGroupFile.Group>) (t => t.commandDirectory == group.commandDirectory)) == -1)
      {
        if (flag)
        {
          if (!string.IsNullOrEmpty(akf.TargetBuild))
            group.target = new HashedString(akf.TargetBuild);
          if (group.renderType != KAnimBatchGroup.RendererType.DontRender)
          {
            group.renderType = KAnimBatchGroup.RendererType.DontRender;
            group.swapTarget = new HashedString(akf.SwapTargetBuild);
          }
        }
        if (akf.Type == AnimCommandFile.ConfigType.AnimOnly)
        {
          group.target = new HashedString(akf.TargetBuild);
          group.renderType = KAnimBatchGroup.RendererType.AnimOnly;
          group.animTarget = new HashedString(akf.AnimTargetBuild);
          group.swapTarget = new HashedString(akf.SwapTargetBuild);
        }
      }
      this.groups.Add(group);
    }
    return num;
  }

  public bool AddAnimFile(KAnimGroupFile.GroupFile gf, AnimCommandFile akf, KAnimFile file)
  {
    Debug.Assert(gf != null);
    Debug.Assert((UnityEngine.Object) file != (UnityEngine.Object) null, (object) gf.groupID);
    Debug.Assert(akf != null, (object) gf.groupID);
    return this.AddFile(this.AddGroup(akf, gf, file), file);
  }

  private bool AddFile(int groupIndex, KAnimFile file)
  {
    if (this.groups[groupIndex].files.Contains(file))
      return false;
    Pair<HashedString, HashedString> pair = new Pair<HashedString, HashedString>((HashedString) file.homedirectory, this.groups[groupIndex].id);
    bool flag = false;
    for (int index = 0; index < this.currentGroup.Count; ++index)
    {
      if (this.currentGroup[index].first == (HashedString) file.homedirectory)
      {
        this.currentGroup[index] = pair;
        flag = true;
        break;
      }
    }
    if (!flag)
      this.currentGroup.Add(pair);
    this.groups[groupIndex].files.Add(file);
    return true;
  }

  public KAnimGroupFile.AddModResult AddAnimMod(
    KAnimGroupFile.GroupFile gf,
    AnimCommandFile akf,
    KAnimFile file)
  {
    Debug.Assert(gf != null);
    Debug.Assert((UnityEngine.Object) file != (UnityEngine.Object) null, (object) gf.groupID);
    Debug.Assert(akf != null, (object) gf.groupID);
    int index1 = this.AddGroup(akf, gf, file);
    string name = file.GetData().name;
    int index2 = this.groups[index1].files.FindIndex((Predicate<KAnimFile>) (candidate => (UnityEngine.Object) candidate != (UnityEngine.Object) null && candidate.GetData().name == name));
    if (index2 == -1)
    {
      this.groups[index1].files.Add(file);
      return KAnimGroupFile.AddModResult.Added;
    }
    this.groups[index1].files[index2].mod = file.mod;
    return KAnimGroupFile.AddModResult.Replaced;
  }

  public void LoadAll()
  {
    Debug.Assert(!KAnimGroupFile.hasCompletedLoadAll, (object) "You cannot load all the anim data twice!");
    this.fileData.Clear();
    for (int index1 = 0; index1 < this.groups.Count; ++index1)
    {
      if (!this.groups[index1].id.IsValid)
        Debug.LogErrorFormat("Group invalid groupIndex [{0}]", (object) index1);
      KBatchGroupData data = !this.groups[index1].target.IsValid ? KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].id) : KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].target);
      HashedString hashedString1 = this.groups[index1].id;
      if (this.groups[index1].renderType == KAnimBatchGroup.RendererType.AnimOnly)
      {
        if (this.groups[index1].swapTarget.IsValid)
        {
          data = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].swapTarget);
          hashedString1 = this.groups[index1].swapTarget;
        }
        else
          continue;
      }
      for (int index2 = 0; index2 < this.groups[index1].files.Count; ++index2)
      {
        KAnimFile file1 = this.groups[index1].files[index2];
        if ((UnityEngine.Object) file1 != (UnityEngine.Object) null && file1.buildBytes != null && !this.fileData.ContainsKey(file1.GetInstanceID()))
        {
          if (file1.buildBytes.Length == 0)
          {
            Debug.LogWarning((object) ("Build File [" + file1.GetData().name + "] has 0 bytes"));
          }
          else
          {
            HashedString hashedString2 = new HashedString(file1.name);
            HashCache.Get().Add(hashedString2.HashValue, file1.name);
            KAnimFileData file2 = KGlobalAnimParser.Get().GetFile(file1);
            file2.maxVisSymbolFrames = 0;
            file2.batchTag = hashedString1;
            file2.buildIndex = KGlobalAnimParser.ParseBuildData(data, (KAnimHashedString) hashedString2, new FastReader(file1.buildBytes), file1.textureList);
            this.fileData.Add(file1.GetInstanceID(), file2);
          }
        }
      }
    }
    for (int index1 = 0; index1 < this.groups.Count; ++index1)
    {
      if (this.groups[index1].renderType == KAnimBatchGroup.RendererType.AnimOnly)
      {
        KBatchGroupData batchGroupData1 = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].swapTarget);
        KBatchGroupData batchGroupData2 = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].animTarget);
        for (int index2 = 0; index2 < batchGroupData1.builds.Count; ++index2)
        {
          KAnim.Build build = batchGroupData1.builds[index2];
          if (build != null && build.symbols != null)
          {
            for (int index3 = 0; index3 < build.symbols.Length; ++index3)
            {
              KAnim.Build.Symbol symbol1 = build.symbols[index3];
              if (symbol1 != null && symbol1.hash.IsValid() && batchGroupData2.GetFirstIndex(symbol1.hash) == -1)
              {
                KAnim.Build.Symbol symbol2 = new KAnim.Build.Symbol();
                symbol2.build = build;
                symbol2.hash = symbol1.hash;
                symbol2.path = symbol1.path;
                symbol2.colourChannel = symbol1.colourChannel;
                symbol2.flags = symbol1.flags;
                symbol2.firstFrameIdx = batchGroupData2.symbolFrameInstances.Count;
                symbol2.numFrames = symbol1.numFrames;
                symbol2.symbolIndexInSourceBuild = batchGroupData2.frameElementSymbols.Count;
                for (int index4 = 0; index4 < symbol2.numFrames; ++index4)
                {
                  KAnim.Build.SymbolFrameInstance symbolFrameInstance = batchGroupData1.GetSymbolFrameInstance(index4 + symbol1.firstFrameIdx);
                  batchGroupData2.symbolFrameInstances.Add(new KAnim.Build.SymbolFrameInstance()
                  {
                    symbolFrame = symbolFrameInstance.symbolFrame,
                    buildImageIdx = -1,
                    symbolIdx = batchGroupData2.GetSymbolCount()
                  });
                }
                batchGroupData2.AddBuildSymbol(symbol2);
              }
            }
          }
        }
      }
    }
    for (int index1 = 0; index1 < this.groups.Count; ++index1)
    {
      if (!this.groups[index1].id.IsValid)
        Debug.LogErrorFormat("Group invalid groupIndex [{0}]", (object) index1);
      if (this.groups[index1].renderType != KAnimBatchGroup.RendererType.DontRender)
      {
        KBatchGroupData batchGroupData;
        if (this.groups[index1].animTarget.IsValid)
        {
          batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].animTarget);
          if (batchGroupData == null)
            Debug.LogErrorFormat("Anim group is null for [{0}] -> [{1}]", (object) this.groups[index1].id, (object) this.groups[index1].animTarget);
        }
        else
        {
          batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index1].id);
          if (batchGroupData == null)
            Debug.LogErrorFormat("Anim group is null for [{0}]", (object) this.groups[index1].id);
        }
        for (int index2 = 0; index2 < this.groups[index1].files.Count; ++index2)
        {
          KAnimFile file1 = this.groups[index1].files[index2];
          if ((UnityEngine.Object) file1 != (UnityEngine.Object) null && file1.animBytes != null)
          {
            if (file1.animBytes.Length == 0)
            {
              Debug.LogWarning((object) ("Anim File [" + file1.GetData().name + "] has 0 bytes"));
            }
            else
            {
              if (!this.fileData.ContainsKey(file1.GetInstanceID()))
              {
                KAnimFileData file2 = KGlobalAnimParser.Get().GetFile(file1);
                file2.maxVisSymbolFrames = 0;
                file2.batchTag = this.groups[index1].id;
                this.fileData.Add(file1.GetInstanceID(), file2);
              }
              HashedString fileNameHash = new HashedString(file1.name);
              FastReader reader = new FastReader(file1.animBytes);
              KAnimFileData animFile = this.fileData[file1.GetInstanceID()];
              KGlobalAnimParser.ParseAnimData(batchGroupData, fileNameHash, reader, animFile);
            }
          }
        }
      }
    }
    for (int index = 0; index < this.groups.Count; ++index)
    {
      if (!this.groups[index].id.IsValid)
        Debug.LogErrorFormat("Group invalid groupIndex [{0}]", (object) index);
      KBatchGroupData batchGroupData;
      if (this.groups[index].target.IsValid)
      {
        batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index].target);
        if (batchGroupData == null)
          Debug.LogErrorFormat("Group is null for  [{0}] target [{1}]", (object) this.groups[index].id, (object) this.groups[index].target);
      }
      else
      {
        batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.groups[index].id);
        if (batchGroupData == null)
          Debug.LogErrorFormat("Group is null for [{0}]", (object) this.groups[index].id);
      }
      KGlobalAnimParser.PostParse(batchGroupData);
    }
    KAnimGroupFile.hasCompletedLoadAll = true;
  }

  private void Sort()
  {
    for (int index = 0; index < this.groups.Count; ++index)
      this.groups[index].files.RemoveAll((Predicate<KAnimFile>) (f => (UnityEngine.Object) f == (UnityEngine.Object) null || f.name == null));
    this.groups.RemoveAll((Predicate<KAnimGroupFile.Group>) (f => f == null || f.files.Count == 0));
    this.groups.Sort((Comparison<KAnimGroupFile.Group>) ((file0, file1) => file0.id.HashValue.CompareTo(file1.id.HashValue)));
    for (int index = 0; index < this.groups.Count; ++index)
    {
      if (this.groups[index].files.Count != 1)
      {
        List<KAnimFile> all = this.groups[index].files.FindAll((Predicate<KAnimFile>) (f => f.buildBytes != null));
        this.groups[index].files.RemoveAll((Predicate<KAnimFile>) (f => f.buildBytes != null));
        all.Sort((Comparison<KAnimFile>) ((file0, file1) => (file0.homedirectory + file0.name).CompareTo(file1.homedirectory + file1.name)));
        this.groups[index].files.Sort((Comparison<KAnimFile>) ((file0, file1) => (file0.homedirectory + file0.name).CompareTo(file1.homedirectory + file1.name)));
        this.groups[index].files.InsertRange(0, (IEnumerable<KAnimFile>) all);
      }
    }
  }

  [Serializable]
  public class Group
  {
    [SerializeField]
    public HashedString id;
    [SerializeField]
    public string commandDirectory = "";
    [SerializeField]
    public List<KAnimFile> files = new List<KAnimFile>();
    [SerializeField]
    public KAnimBatchGroup.RendererType renderType;
    [SerializeField]
    public int maxVisibleSymbols;
    [SerializeField]
    public int maxGroupSize;
    [SerializeField]
    public HashedString target;
    [SerializeField]
    public HashedString swapTarget;
    [SerializeField]
    public HashedString animTarget;

    public Group(HashedString tag) => this.id = tag;
  }

  public class GroupFile
  {
    public string groupID { get; set; }

    public string commandDirectory { get; set; }
  }

  public enum AddModResult
  {
    Added,
    Replaced,
  }
}
