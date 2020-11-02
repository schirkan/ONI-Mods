// Decompiled with JetBrains decompiler
// Type: TechTreeTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TechTreeTitle : Resource
{
  public string desc;
  private ResourceTreeNode node;

  public Vector2 center => this.node.center;

  public float width => this.node.width;

  public float height => this.node.height;

  public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node)
    : base(id, parent, name)
    => this.node = node;
}
