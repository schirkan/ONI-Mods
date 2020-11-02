// Decompiled with JetBrains decompiler
// Type: ProcGen.Map.Edge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using UnityEngine;

namespace ProcGen.Map
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Edge : ProcGen.Arc
  {
    public Corner corner0;
    public Corner corner1;
    public Cell site0;
    public Cell site1;

    public Edge()
      : base(WorldGenTags.Edge.Name)
      => this.tags = new TagSet();

    public Edge(Corner c0, Corner c1)
      : base(WorldGenTags.Edge.Name)
    {
      this.corner0 = c0;
      this.corner1 = c1;
      c0.Add(this);
      c1.Add(this);
      this.tags = new TagSet();
    }

    public Edge(Satsuma.Arc arc, Corner c0, Corner c1)
      : base(arc, WorldGenTags.Edge.Name)
    {
      this.corner0 = c0;
      this.corner1 = c1;
      c0.Add(this);
      c1.Add(this);
      this.tags = new TagSet();
    }

    public Edge(Satsuma.Arc arc, Corner c0, Corner c1, Cell s0, Cell s1)
      : base(arc, WorldGenTags.Edge.Name)
    {
      this.corner0 = c0;
      this.corner1 = c1;
      this.site0 = s0;
      this.site1 = s1;
      c0.Add(this);
      c1.Add(this);
      s0.Add(this);
      s1.Add(this);
      this.tags = new TagSet();
    }

    public void SetSite0(Cell s0)
    {
      this.site0 = s0;
      s0.Add(this);
    }

    public void SetSite1(Cell s1)
    {
      this.site1 = s1;
      s1.Add(this);
    }

    public Vector2 MidPoint() => (this.corner1.position - this.corner0.position) * 0.5f + this.corner0.position;
  }
}
