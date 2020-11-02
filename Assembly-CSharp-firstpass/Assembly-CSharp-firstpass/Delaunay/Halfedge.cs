// Decompiled with JetBrains decompiler
// Type: Delaunay.Halfedge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.LR;
using Delaunay.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
  public sealed class Halfedge : IDisposable
  {
    private static Stack<Halfedge> _pool = new Stack<Halfedge>();
    public Halfedge edgeListLeftNeighbor;
    public Halfedge edgeListRightNeighbor;
    public Halfedge nextInPriorityQueue;
    public Edge edge;
    public Side? leftRight;
    public Vertex vertex;
    public float ystar;

    public static Halfedge Create(Edge edge, Side? lr) => Halfedge._pool.Count > 0 ? Halfedge._pool.Pop().Init(edge, lr) : new Halfedge(edge, lr);

    public static Halfedge CreateDummy() => Halfedge.Create((Edge) null, new Side?());

    public Halfedge(Edge edge = null, Side? lr = null) => this.Init(edge, lr);

    private Halfedge Init(Edge edge, Side? lr)
    {
      this.edge = edge;
      this.leftRight = lr;
      this.nextInPriorityQueue = (Halfedge) null;
      this.vertex = (Vertex) null;
      return this;
    }

    public override string ToString() => "Halfedge (leftRight: " + this.leftRight.ToString() + "; vertex: " + this.vertex.ToString() + ")";

    public void Dispose()
    {
      if (this.edgeListLeftNeighbor != null || this.edgeListRightNeighbor != null || this.nextInPriorityQueue != null)
        return;
      this.edge = (Edge) null;
      this.leftRight = new Side?();
      this.vertex = (Vertex) null;
      Halfedge._pool.Push(this);
    }

    public void ReallyDispose()
    {
      this.edgeListLeftNeighbor = (Halfedge) null;
      this.edgeListRightNeighbor = (Halfedge) null;
      this.nextInPriorityQueue = (Halfedge) null;
      this.edge = (Edge) null;
      this.leftRight = new Side?();
      this.vertex = (Vertex) null;
      Halfedge._pool.Push(this);
    }

    internal bool IsLeftOf(Vector2 p)
    {
      Vector2 coord = this.edge.rightSite.Coord;
      bool flag1 = (double) p.x > (double) coord.x;
      if (flag1)
      {
        Side? leftRight = this.leftRight;
        Side side = Side.LEFT;
        if (leftRight.GetValueOrDefault() == side & leftRight.HasValue)
          return true;
      }
      if (!flag1)
      {
        Side? leftRight = this.leftRight;
        Side side = Side.RIGHT;
        if (leftRight.GetValueOrDefault() == side & leftRight.HasValue)
          return false;
      }
      bool flag2;
      if ((double) this.edge.a == 1.0)
      {
        float num1 = p.y - coord.y;
        float num2 = p.x - coord.x;
        bool flag3 = false;
        if (!flag1 && (double) this.edge.b < 0.0 || flag1 && (double) this.edge.b >= 0.0)
        {
          flag2 = (double) num1 >= (double) this.edge.b * (double) num2;
          flag3 = flag2;
        }
        else
        {
          flag2 = (double) p.x + (double) p.y * (double) this.edge.b > (double) this.edge.c;
          if ((double) this.edge.b < 0.0)
            flag2 = !flag2;
          if (!flag2)
            flag3 = true;
        }
        if (!flag3)
        {
          float num3 = coord.x - this.edge.leftSite.x;
          flag2 = (double) this.edge.b * ((double) num2 * (double) num2 - (double) num1 * (double) num1) < (double) num3 * (double) num1 * (1.0 + 2.0 * (double) num2 / (double) num3 + (double) this.edge.b * (double) this.edge.b);
          if ((double) this.edge.b < 0.0)
            flag2 = !flag2;
        }
      }
      else
      {
        float num1 = this.edge.c - this.edge.a * p.x;
        double num2 = (double) p.y - (double) num1;
        float num3 = p.x - coord.x;
        float num4 = num1 - coord.y;
        flag2 = num2 * num2 > (double) num3 * (double) num3 + (double) num4 * (double) num4;
      }
      Side? leftRight1 = this.leftRight;
      Side side1 = Side.LEFT;
      return !(leftRight1.GetValueOrDefault() == side1 & leftRight1.HasValue) ? !flag2 : flag2;
    }
  }
}
