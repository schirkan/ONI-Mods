// Decompiled with JetBrains decompiler
// Type: VoronoiTree.Leaf
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using System.Collections.Generic;
using UnityEngine;

namespace VoronoiTree
{
  public class Leaf : Node
  {
    public Leaf()
      : base(Node.NodeType.Leaf)
    {
    }

    public Leaf(Diagram.Site site, Tree parent)
      : base(site, Node.NodeType.Leaf, parent)
    {
    }

    public override Tree Split(Node.SplitCommand cmd)
    {
      Tree tree = this.parent.ReplaceLeafWithTree(this);
      tree.Split(cmd);
      return tree;
    }

    public void GetIntersectingSites(LineSegment edge, List<Diagram.Site> intersectingSites)
    {
      if (this.site == null || this.site.poly == null)
        return;
      LineSegment intersectingSegment = new LineSegment(new Vector2?(), new Vector2?());
      if (!this.site.poly.ClipSegment(edge, ref intersectingSegment))
        return;
      intersectingSites.Add(this.site);
    }
  }
}
