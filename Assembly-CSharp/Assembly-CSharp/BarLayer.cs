﻿// Decompiled with JetBrains decompiler
// Type: BarLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class BarLayer : GraphLayer
{
  public GameObject bar_container;
  public GameObject prefab_bar;
  public GraphedBarFormatting[] bar_formats;
  private List<GraphedBar> bars = new List<GraphedBar>();

  public int bar_count => this.bars.Count;

  public void NewBar(int[] values, float x_position, string ID = "")
  {
    GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
    if (ID == "")
      ID = this.bars.Count.ToString();
    gameObject.name = string.Format("bar_{0}", (object) ID);
    GraphedBar component = gameObject.GetComponent<GraphedBar>();
    component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
    int[] values1 = new int[values.Length];
    for (int index = 0; index < values.Length; ++index)
      values1[index] = (int) ((double) this.graph.rectTransform().rect.height * (double) this.graph.GetRelativeSize(new Vector2(0.0f, (float) values[index])).y);
    component.SetValues(values1, this.graph.GetRelativePosition(new Vector2(x_position, 0.0f)).x);
    this.bars.Add(component);
  }

  public void ClearBars()
  {
    foreach (GraphedBar bar in this.bars)
    {
      if ((Object) bar != (Object) null && (Object) bar.gameObject != (Object) null)
        Object.DestroyImmediate((Object) bar.gameObject);
    }
    this.bars.Clear();
  }
}
