// Decompiled with JetBrains decompiler
// Type: CategoryEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class CategoryEntry : CodexEntry
{
  public List<CodexEntry> entriesInCategory = new List<CodexEntry>();

  public bool largeFormat { get; set; }

  public bool sort { get; set; }

  public CategoryEntry(
    string category,
    List<ContentContainer> contentContainers,
    string name,
    List<CodexEntry> entriesInCategory,
    bool largeFormat,
    bool sort)
    : base(category, contentContainers, name)
  {
    this.entriesInCategory = entriesInCategory;
    this.largeFormat = largeFormat;
    this.sort = sort;
  }
}
