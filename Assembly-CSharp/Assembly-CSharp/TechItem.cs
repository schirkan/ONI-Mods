// Decompiled with JetBrains decompiler
// Type: TechItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class TechItem : Resource
{
  public string description;
  public Func<string, bool, Sprite> getUISprite;
  public Tech parentTech;

  public TechItem(
    string id,
    ResourceSet parent,
    string name,
    string description,
    Func<string, bool, Sprite> getUISprite,
    Tech parentTech)
    : base(id, parent, name)
  {
    this.description = description;
    this.getUISprite = getUISprite;
    this.parentTech = parentTech;
  }

  public Sprite UISprite() => this.getUISprite("ui", false);

  public bool IsComplete() => this.parentTech.IsComplete();
}
