﻿// Decompiled with JetBrains decompiler
// Type: DetailsPanelDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DetailsPanelDrawer
{
  private List<DetailsPanelDrawer.Label> labels = new List<DetailsPanelDrawer.Label>();
  private int activeLabelCount;
  private UIStringFormatter stringformatter;
  private UIFloatFormatter floatFormatter;
  private GameObject parent;
  private GameObject labelPrefab;

  public DetailsPanelDrawer(GameObject label_prefab, GameObject parent)
  {
    this.parent = parent;
    this.labelPrefab = label_prefab;
    this.stringformatter = new UIStringFormatter();
    this.floatFormatter = new UIFloatFormatter();
  }

  public DetailsPanelDrawer NewLabel(string text)
  {
    DetailsPanelDrawer.Label label = new DetailsPanelDrawer.Label();
    if (this.activeLabelCount >= this.labels.Count)
    {
      label.text = Util.KInstantiate(this.labelPrefab, this.parent).GetComponent<LocText>();
      label.tooltip = label.text.GetComponent<ToolTip>();
      label.text.transform.localScale = new Vector3(1f, 1f, 1f);
      this.labels.Add(label);
    }
    else
      label = this.labels[this.activeLabelCount];
    ++this.activeLabelCount;
    label.text.text = text;
    label.tooltip.toolTip = "";
    label.tooltip.OnToolTip = (Func<string>) null;
    label.text.gameObject.SetActive(true);
    return this;
  }

  public DetailsPanelDrawer Tooltip(string tooltip_text)
  {
    this.labels[this.activeLabelCount - 1].tooltip.toolTip = tooltip_text;
    return this;
  }

  public DetailsPanelDrawer Tooltip(Func<string> tooltip_cb)
  {
    this.labels[this.activeLabelCount - 1].tooltip.OnToolTip = tooltip_cb;
    return this;
  }

  public string Format(string format, float value) => this.floatFormatter.Format(format, value);

  public string Format(string format, string s0) => this.stringformatter.Format(format, s0);

  public string Format(string format, string s0, string s1) => this.stringformatter.Format(format, s0, s1);

  public DetailsPanelDrawer BeginDrawing()
  {
    this.activeLabelCount = 0;
    this.stringformatter.BeginDrawing();
    this.floatFormatter.BeginDrawing();
    return this;
  }

  public DetailsPanelDrawer EndDrawing()
  {
    this.floatFormatter.EndDrawing();
    this.stringformatter.EndDrawing();
    for (int activeLabelCount = this.activeLabelCount; activeLabelCount < this.labels.Count; ++activeLabelCount)
    {
      if (this.labels[activeLabelCount].text.gameObject.activeSelf)
        this.labels[activeLabelCount].text.gameObject.SetActive(false);
    }
    return this;
  }

  private struct Label
  {
    public LocText text;
    public ToolTip tooltip;
  }
}