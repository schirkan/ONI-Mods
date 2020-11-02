// Decompiled with JetBrains decompiler
// Type: SubEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class SubEntry
{
  public ContentContainer lockedContentContainer;
  public Color iconColor = Color.white;

  public SubEntry()
  {
  }

  public SubEntry(
    string id,
    string parentEntryID,
    List<ContentContainer> contentContainers,
    string name)
  {
    this.id = id;
    this.parentEntryID = parentEntryID;
    this.name = name;
    this.contentContainers = contentContainers;
    if (!string.IsNullOrEmpty(this.lockID))
    {
      foreach (ContentContainer contentContainer in contentContainers)
        contentContainer.lockID = this.lockID;
    }
    if (!string.IsNullOrEmpty(this.sortString))
      return;
    if (!string.IsNullOrEmpty(this.title))
      this.sortString = UI.StripLinkFormatting(this.title);
    else
      this.sortString = UI.StripLinkFormatting(name);
  }

  public List<ContentContainer> contentContainers { get; set; }

  public string parentEntryID { get; set; }

  public string id { get; set; }

  public string name { get; set; }

  public string title { get; set; }

  public string subtitle { get; set; }

  public Sprite icon { get; set; }

  public int layoutPriority { get; set; }

  public bool disabled { get; set; }

  public string lockID { get; set; }

  public string sortString { get; set; }

  public bool showBeforeGeneratedCategoryLinks { get; set; }
}
