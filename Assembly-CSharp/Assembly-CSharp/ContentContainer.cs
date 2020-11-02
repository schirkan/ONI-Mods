﻿// Decompiled with JetBrains decompiler
// Type: ContentContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization.Converters;
using System.Collections.Generic;
using UnityEngine;

public class ContentContainer
{
  public GameObject go;

  public ContentContainer() => this.content = new List<ICodexWidget>();

  public ContentContainer(List<ICodexWidget> content, ContentContainer.ContentLayout contentLayout)
  {
    this.content = content;
    this.contentLayout = contentLayout;
  }

  public List<ICodexWidget> content { get; set; }

  public string lockID { get; set; }

  [StringEnumConverter]
  public ContentContainer.ContentLayout contentLayout { get; set; }

  public bool showBeforeGeneratedContent { get; set; }

  public enum ContentLayout
  {
    Vertical,
    Horizontal,
    Grid,
    GridTwoColumn,
    GridTwoColumnTall,
  }
}
