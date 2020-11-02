﻿// Decompiled with JetBrains decompiler
// Type: CodexCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CodexCache
{
  private static string baseEntryPath;
  public static Dictionary<string, CodexEntry> entries;
  public static Dictionary<string, SubEntry> subEntries;
  private static Dictionary<string, List<string>> unlockedEntryLookup;
  private static List<Tuple<string, System.Type>> widgetTagMappings;

  public static string FormatLinkID(string linkID)
  {
    linkID = linkID.ToUpper();
    linkID = linkID.Replace("_", "");
    return linkID;
  }

  public static void CodexCacheInit()
  {
    CodexCache.entries = new Dictionary<string, CodexEntry>();
    CodexCache.subEntries = new Dictionary<string, SubEntry>();
    CodexCache.unlockedEntryLookup = new Dictionary<string, List<string>>();
    Dictionary<string, CodexEntry> entries = new Dictionary<string, CodexEntry>();
    if (CodexCache.widgetTagMappings == null)
      CodexCache.widgetTagMappings = new List<Tuple<string, System.Type>>()
      {
        new Tuple<string, System.Type>("!CodexText", typeof (CodexText)),
        new Tuple<string, System.Type>("!CodexImage", typeof (CodexImage)),
        new Tuple<string, System.Type>("!CodexDividerLine", typeof (CodexDividerLine)),
        new Tuple<string, System.Type>("!CodexSpacer", typeof (CodexSpacer)),
        new Tuple<string, System.Type>("!CodexLabelWithIcon", typeof (CodexLabelWithIcon)),
        new Tuple<string, System.Type>("!CodexLabelWithLargeIcon", typeof (CodexLabelWithLargeIcon)),
        new Tuple<string, System.Type>("!CodexContentLockedIndicator", typeof (CodexContentLockedIndicator)),
        new Tuple<string, System.Type>("!CodexLargeSpacer", typeof (CodexLargeSpacer)),
        new Tuple<string, System.Type>("!CodexVideo", typeof (CodexVideo))
      };
    string str1 = CodexCache.FormatLinkID("LESSONS");
    entries.Add(str1, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str1, (string) UI.CODEX.CATEGORYNAMES.TIPS, CodexEntryGenerator.GenerateTutorialNotificationEntries(), Assets.GetSprite((HashedString) "codexIconLessons"), sort: false, overrideHeader: ((string) UI.CODEX.CATEGORYNAMES.VIDEOS)));
    string str2 = CodexCache.FormatLinkID("creatures");
    entries.Add(str2, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str2, (string) UI.CODEX.CATEGORYNAMES.CREATURES, CodexEntryGenerator.GenerateCreatureEntries(), Assets.GetSprite((HashedString) "codexIconCritters")));
    string str3 = CodexCache.FormatLinkID("plants");
    entries.Add(str3, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str3, (string) UI.CODEX.CATEGORYNAMES.PLANTS, CodexEntryGenerator.GeneratePlantEntries(), Assets.GetSprite((HashedString) "codexIconPlants")));
    string str4 = CodexCache.FormatLinkID("food");
    entries.Add(str4, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str4, (string) UI.CODEX.CATEGORYNAMES.FOOD, CodexEntryGenerator.GenerateFoodEntries(), Assets.GetSprite((HashedString) "codexIconFood")));
    string str5 = CodexCache.FormatLinkID("buildings");
    entries.Add(str5, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str5, (string) UI.CODEX.CATEGORYNAMES.BUILDINGS, CodexEntryGenerator.GenerateBuildingEntries(), Assets.GetSprite((HashedString) "codexIconBuildings")));
    string str6 = CodexCache.FormatLinkID("tech");
    entries.Add(str6, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str6, (string) UI.CODEX.CATEGORYNAMES.TECH, CodexEntryGenerator.GenerateTechEntries(), Assets.GetSprite((HashedString) "codexIconResearch")));
    string str7 = CodexCache.FormatLinkID("roles");
    entries.Add(str7, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str7, (string) UI.CODEX.CATEGORYNAMES.ROLES, CodexEntryGenerator.GenerateRoleEntries(), Assets.GetSprite((HashedString) "codexIconSkills")));
    string str8 = CodexCache.FormatLinkID("disease");
    entries.Add(str8, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str8, (string) UI.CODEX.CATEGORYNAMES.DISEASE, CodexEntryGenerator.GenerateDiseaseEntries(), Assets.GetSprite((HashedString) "codexIconDisease"), false));
    string str9 = CodexCache.FormatLinkID("elements");
    entries.Add(str9, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str9, (string) UI.CODEX.CATEGORYNAMES.ELEMENTS, CodexEntryGenerator.GenerateElementEntries(), Assets.GetSprite((HashedString) "codexIconElements"), sort: false));
    string str10 = CodexCache.FormatLinkID("geysers");
    entries.Add(str10, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str10, (string) UI.CODEX.CATEGORYNAMES.GEYSERS, CodexEntryGenerator.GenerateGeyserEntries(), Assets.GetSprite((HashedString) "codexIconGeysers")));
    string str11 = CodexCache.FormatLinkID("equipment");
    entries.Add(str11, (CodexEntry) CodexEntryGenerator.GenerateCategoryEntry(str11, (string) UI.CODEX.CATEGORYNAMES.EQUIPMENT, CodexEntryGenerator.GenerateEquipmentEntries(), Assets.GetSprite((HashedString) "codexIconEquipment")));
    CategoryEntry categoryEntry = CodexEntryGenerator.GenerateCategoryEntry(CodexCache.FormatLinkID("HOME"), (string) UI.CODEX.CATEGORYNAMES.ROOT, entries);
    CodexEntryGenerator.GeneratePageNotFound();
    List<CategoryEntry> categoryEntryList = new List<CategoryEntry>();
    foreach (KeyValuePair<string, CodexEntry> keyValuePair in entries)
      categoryEntryList.Add(keyValuePair.Value as CategoryEntry);
    CodexCache.CollectYAMLEntries(categoryEntryList);
    CodexCache.CollectYAMLSubEntries(categoryEntryList);
    CodexCache.CheckUnlockableContent();
    categoryEntryList.Add(categoryEntry);
    foreach (KeyValuePair<string, CodexEntry> entry in CodexCache.entries)
    {
      if (entry.Value.subEntries.Count > 0)
      {
        entry.Value.subEntries.Sort((Comparison<SubEntry>) ((a, b) => a.layoutPriority.CompareTo(b.layoutPriority)));
        if ((UnityEngine.Object) entry.Value.icon == (UnityEngine.Object) null)
        {
          entry.Value.icon = entry.Value.subEntries[0].icon;
          entry.Value.iconColor = entry.Value.subEntries[0].iconColor;
        }
        int num = 0;
        foreach (SubEntry subEntry in entry.Value.subEntries)
        {
          if (subEntry.lockID != null && !Game.Instance.unlocks.IsUnlocked(subEntry.lockID))
            ++num;
        }
        if (entry.Value.subEntries.Count > 1)
        {
          List<ICodexWidget> content = new List<ICodexWidget>();
          content.Add((ICodexWidget) new CodexSpacer());
          content.Add((ICodexWidget) new CodexText(string.Format((string) CODEX.HEADERS.SUBENTRIES, (object) (entry.Value.subEntries.Count - num), (object) entry.Value.subEntries.Count), CodexTextStyle.Subtitle));
          foreach (SubEntry subEntry in entry.Value.subEntries)
          {
            if (subEntry.lockID != null && !Game.Instance.unlocks.IsUnlocked(subEntry.lockID))
            {
              content.Add((ICodexWidget) new CodexText(UI.FormatAsLink((string) CODEX.HEADERS.CONTENTLOCKED, UI.ExtractLinkID(subEntry.name))));
            }
            else
            {
              string text = UI.FormatAsLink(UI.StripLinkFormatting(subEntry.name), subEntry.id);
              content.Add((ICodexWidget) new CodexText(text));
            }
          }
          content.Add((ICodexWidget) new CodexSpacer());
          entry.Value.contentContainers.Insert(entry.Value.customContentLength, new ContentContainer(content, ContentContainer.ContentLayout.Vertical));
        }
      }
      for (int index = 0; index < entry.Value.subEntries.Count; ++index)
        entry.Value.AddContentContainerRange((IEnumerable<ContentContainer>) entry.Value.subEntries[index].contentContainers);
    }
    CodexEntryGenerator.PopulateCategoryEntries(categoryEntryList, (Comparison<CodexEntry>) ((a, b) =>
    {
      if (a.name == (string) UI.CODEX.CATEGORYNAMES.TIPS)
        return -1;
      return b.name == (string) UI.CODEX.CATEGORYNAMES.TIPS ? 1 : UI.StripLinkFormatting(a.name).CompareTo(UI.StripLinkFormatting(b.name));
    }));
  }

  public static CodexEntry FindEntry(string id)
  {
    if (CodexCache.entries == null)
    {
      Debug.LogWarning((object) "Can't search Codex cache while it's stil null");
      return (CodexEntry) null;
    }
    if (CodexCache.entries.ContainsKey(id))
      return CodexCache.entries[id];
    Debug.LogWarning((object) ("Could not find codex entry with id: " + id));
    return (CodexEntry) null;
  }

  public static SubEntry FindSubEntry(string id)
  {
    foreach (KeyValuePair<string, CodexEntry> entry in CodexCache.entries)
    {
      foreach (SubEntry subEntry in entry.Value.subEntries)
      {
        if (subEntry.id.ToUpper() == id.ToUpper())
          return subEntry;
      }
    }
    return (SubEntry) null;
  }

  private static void CheckUnlockableContent()
  {
    foreach (KeyValuePair<string, CodexEntry> entry in CodexCache.entries)
    {
      foreach (SubEntry subEntry in entry.Value.subEntries)
      {
        if (subEntry.lockedContentContainer != null)
        {
          subEntry.lockedContentContainer.content.Clear();
          subEntry.contentContainers.Remove(subEntry.lockedContentContainer);
        }
      }
    }
  }

  private static void CollectYAMLEntries(List<CategoryEntry> categories)
  {
    CodexCache.baseEntryPath = Application.streamingAssetsPath + "/codex";
    foreach (CodexEntry collectEntry in CodexCache.CollectEntries(""))
    {
      if (collectEntry != null && collectEntry.id != null && collectEntry.contentContainers != null)
      {
        if (CodexCache.entries.ContainsKey(CodexCache.FormatLinkID(collectEntry.id)))
          CodexCache.MergeEntry(collectEntry.id, collectEntry);
        else
          CodexCache.AddEntry(collectEntry.id, collectEntry, categories);
      }
    }
    foreach (string directory in Directory.GetDirectories(CodexCache.baseEntryPath))
    {
      foreach (CodexEntry collectEntry in CodexCache.CollectEntries(System.IO.Path.GetFileNameWithoutExtension(directory)))
      {
        if (collectEntry != null && collectEntry.id != null && collectEntry.contentContainers != null)
        {
          if (CodexCache.entries.ContainsKey(CodexCache.FormatLinkID(collectEntry.id)))
            CodexCache.MergeEntry(collectEntry.id, collectEntry);
          else
            CodexCache.AddEntry(collectEntry.id, collectEntry, categories);
        }
      }
    }
  }

  private static void CollectYAMLSubEntries(List<CategoryEntry> categories)
  {
    CodexCache.baseEntryPath = Application.streamingAssetsPath + "/codex";
    foreach (SubEntry collectSubEntry in CodexCache.CollectSubEntries(""))
    {
      SubEntry v = collectSubEntry;
      if (v.parentEntryID != null && v.id != null)
      {
        if (CodexCache.entries.ContainsKey(v.parentEntryID.ToUpper()))
        {
          SubEntry subEntry = CodexCache.entries[v.parentEntryID.ToUpper()].subEntries.Find((Predicate<SubEntry>) (match => match.id == v.id));
          if (!string.IsNullOrEmpty(v.lockID))
          {
            foreach (ContentContainer contentContainer in v.contentContainers)
              contentContainer.lockID = v.lockID;
          }
          if (subEntry != null)
          {
            if (!string.IsNullOrEmpty(v.lockID))
            {
              foreach (ContentContainer contentContainer in subEntry.contentContainers)
                contentContainer.lockID = v.lockID;
              subEntry.lockID = v.lockID;
            }
            for (int index = 0; index < v.contentContainers.Count; ++index)
            {
              if (!string.IsNullOrEmpty(v.contentContainers[index].lockID))
              {
                int num = subEntry.contentContainers.IndexOf(subEntry.lockedContentContainer);
                subEntry.contentContainers.Insert(num + 1, v.contentContainers[index]);
              }
              else if (v.contentContainers[index].showBeforeGeneratedContent)
                subEntry.contentContainers.Insert(0, v.contentContainers[index]);
              else
                subEntry.contentContainers.Add(v.contentContainers[index]);
            }
            subEntry.contentContainers.Add(new ContentContainer(new List<ICodexWidget>()
            {
              (ICodexWidget) new CodexLargeSpacer()
            }, ContentContainer.ContentLayout.Vertical));
            subEntry.layoutPriority = v.layoutPriority;
          }
          else
            CodexCache.entries[v.parentEntryID.ToUpper()].subEntries.Add(v);
        }
        else
          Debug.LogWarningFormat("Codex SubEntry {0} cannot find parent codex entry with id {1}", (object) v.name, (object) v.parentEntryID);
      }
    }
  }

  private static void AddLockLookup(string lockId, string articleId)
  {
    if (!CodexCache.unlockedEntryLookup.ContainsKey(lockId))
      CodexCache.unlockedEntryLookup[lockId] = new List<string>();
    CodexCache.unlockedEntryLookup[lockId].Add(articleId);
  }

  public static string GetEntryForLock(string lockId)
  {
    if (CodexCache.unlockedEntryLookup == null)
    {
      Debug.LogWarningFormat("Trying to get lock entry {0} before codex cache has been initialized.", (object) lockId);
      return (string) null;
    }
    if (string.IsNullOrEmpty(lockId))
      return (string) null;
    return CodexCache.unlockedEntryLookup.ContainsKey(lockId) && CodexCache.unlockedEntryLookup[lockId] != null && CodexCache.unlockedEntryLookup[lockId].Count > 0 ? CodexCache.unlockedEntryLookup[lockId][0] : (string) null;
  }

  public static void AddEntry(string id, CodexEntry entry, List<CategoryEntry> categoryEntries = null)
  {
    id = CodexCache.FormatLinkID(id);
    if (CodexCache.entries.ContainsKey(id))
      Debug.LogError((object) ("Tried to add " + id + " to the Codex screen multiple times"));
    CodexCache.entries.Add(id, entry);
    entry.id = id;
    if (entry.name == null)
      entry.name = (string) Strings.Get(entry.title);
    if (!string.IsNullOrEmpty(entry.iconPrefabID))
    {
      try
      {
        entry.icon = Def.GetUISpriteFromMultiObjectAnim(Assets.GetPrefab((Tag) entry.iconPrefabID).GetComponent<KBatchedAnimController>().AnimFiles[0]);
      }
      catch
      {
        Debug.LogWarningFormat("Unable to get icon for prefabID {0}", (object) entry.iconPrefabID);
      }
    }
    if (categoryEntries != null)
    {
      CodexEntry codexEntry = (CodexEntry) categoryEntries.Find((Predicate<CategoryEntry>) (group => group.id == entry.parentId));
      if (codexEntry != null)
        (codexEntry as CategoryEntry).entriesInCategory.Add(entry);
    }
    foreach (ContentContainer contentContainer in entry.contentContainers)
    {
      if (contentContainer.lockID != null)
        CodexCache.AddLockLookup(contentContainer.lockID, entry.id);
    }
  }

  public static void AddSubEntry(string id, SubEntry entry)
  {
  }

  public static void MergeSubEntry(string id, SubEntry entry)
  {
  }

  public static void MergeEntry(string id, CodexEntry entry)
  {
    id = CodexCache.FormatLinkID(entry.id);
    entry.id = id;
    CodexEntry entry1 = CodexCache.entries[id];
    int num = 0;
    while (num < entry.log.modificationRecords.Count)
      ++num;
    entry1.customContentLength = entry.contentContainers.Count;
    for (int index = entry.contentContainers.Count - 1; index >= 0; --index)
      entry1.InsertContentContainer(0, entry.contentContainers[index]);
    if (entry.disabled)
      entry1.disabled = entry.disabled;
    entry1.showBeforeGeneratedCategoryLinks = entry.showBeforeGeneratedCategoryLinks;
    foreach (ContentContainer contentContainer in entry.contentContainers)
    {
      if (contentContainer.lockID != null)
        CodexCache.AddLockLookup(contentContainer.lockID, entry.id);
    }
  }

  public static void Clear()
  {
    CodexCache.entries = (Dictionary<string, CodexEntry>) null;
    CodexCache.baseEntryPath = (string) null;
  }

  public static string GetEntryPath() => CodexCache.baseEntryPath;

  public static CodexEntry GetTemplate(string templatePath)
  {
    if (!CodexCache.entries.ContainsKey(templatePath))
      CodexCache.entries.Add(templatePath, (CodexEntry) null);
    if (CodexCache.entries[templatePath] == null)
    {
      string str = System.IO.Path.Combine(CodexCache.baseEntryPath, templatePath);
      CodexEntry codexEntry = YamlIO.LoadFile<CodexEntry>(str + ".yaml", tagMappings: CodexCache.widgetTagMappings);
      if (codexEntry == null)
        Debug.LogWarning((object) ("Missing template [" + str + ".yaml]"));
      CodexCache.entries[templatePath] = codexEntry;
    }
    return CodexCache.entries[templatePath];
  }

  private static void YamlParseErrorCB(YamlIO.Error error, bool force_log_as_warning) => throw new Exception(string.Format("{0} parse error in {1}\n{2}", (object) error.severity, (object) error.file.full_path, (object) error.message), error.inner_exception);

  public static List<CodexEntry> CollectEntries(string folder)
  {
    List<CodexEntry> codexEntryList = new List<CodexEntry>();
    string path = folder == "" ? CodexCache.baseEntryPath : System.IO.Path.Combine(CodexCache.baseEntryPath, folder);
    string[] strArray = new string[0];
    try
    {
      strArray = Directory.GetFiles(path, "*.yaml");
    }
    catch (UnauthorizedAccessException ex)
    {
      Debug.LogWarning((object) ex);
    }
    string upper = folder.ToUpper();
    foreach (string filename in strArray)
    {
      try
      {
        CodexEntry codexEntry = YamlIO.LoadFile<CodexEntry>(filename, new YamlIO.ErrorHandler(CodexCache.YamlParseErrorCB), CodexCache.widgetTagMappings);
        if (codexEntry != null)
        {
          codexEntry.category = upper;
          codexEntryList.Add(codexEntry);
        }
      }
      catch (Exception ex)
      {
        DebugUtil.DevLogErrorFormat("CodexCache.CollectEntries failed to load [{0}]: {1}", (object) filename, (object) ex.ToString());
      }
    }
    foreach (CodexEntry codexEntry in codexEntryList)
    {
      if (string.IsNullOrEmpty(codexEntry.sortString))
        codexEntry.sortString = (string) Strings.Get(codexEntry.title);
    }
    codexEntryList.Sort((Comparison<CodexEntry>) ((x, y) => x.sortString.CompareTo(y.sortString)));
    return codexEntryList;
  }

  public static List<SubEntry> CollectSubEntries(string folder)
  {
    List<SubEntry> subEntryList = new List<SubEntry>();
    string path = folder == "" ? CodexCache.baseEntryPath : System.IO.Path.Combine(CodexCache.baseEntryPath, folder);
    string[] strArray = new string[0];
    try
    {
      strArray = Directory.GetFiles(path, "*.yaml", SearchOption.AllDirectories);
    }
    catch (UnauthorizedAccessException ex)
    {
      Debug.LogWarning((object) ex);
    }
    foreach (string filename in strArray)
    {
      try
      {
        SubEntry subEntry = YamlIO.LoadFile<SubEntry>(filename, new YamlIO.ErrorHandler(CodexCache.YamlParseErrorCB), CodexCache.widgetTagMappings);
        if (subEntry != null)
          subEntryList.Add(subEntry);
      }
      catch (Exception ex)
      {
        DebugUtil.DevLogErrorFormat("CodexCache.CollectSubEntries failed to load [{0}]: {1}", (object) filename, (object) ex.ToString());
      }
    }
    subEntryList.Sort((Comparison<SubEntry>) ((x, y) => x.title.CompareTo(y.title)));
    return subEntryList;
  }
}
