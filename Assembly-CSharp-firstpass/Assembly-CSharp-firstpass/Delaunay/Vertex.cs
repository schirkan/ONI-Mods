// Decompiled with JetBrains decompiler
// Type: Delaunay.Vertex
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.LR;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
  public sealed class Vertex : ICoord
  {
    public static readonly Vertex VERTEX_AT_INFINITY = new Vertex(float.NaN, float.NaN);
    private static Stack<Vertex> _pool = new Stack<Vertex>();
    private static int _nvertices = 0;
    private Vector2 _coord;
    private int _vertexIndex;

    private static Vertex Create(float x, float y)
    {
      if (float.IsNaN(x) || float.IsNaN(y))
        return Vertex.VERTEX_AT_INFINITY;
      return Vertex._pool.Count > 0 ? Vertex._pool.Pop().Init(x, y) : new Vertex(x, y);
    }

    public Vector2 Coord => this._coord;

    public int vertexIndex => this._vertexIndex;

    public Vertex(float x, float y) => this.Init(x, y);

    private Vertex Init(float x, float y)
    {
      this._coord = new Vector2(x, y);
      return this;
    }

    public void Dispose() => Vertex._pool.Push(this);

    public void SetIndex() => this._vertexIndex = Vertex._nvertices++;

    public override string ToString() => "Vertex (" + (object) this._vertexIndex + ")";

    public static Vertex Intersect(Halfedge halfedge0, Halfedge halfedge1)
    {
      Edge edge1 = halfedge0.edge;
      Edge edge2 = halfedge1.edge;
      if (edge1 == null || edge2 == null)
        return (Vertex) null;
      if (edge1.rightSite == edge2.rightSite)
        return (Vertex) null;
      float num = (float) ((double) edge1.a * (double) edge2.b - (double) edge1.b * (double) edge2.a);
      if (-1E-10 < (double) num && (double) num < 1E-10)
        return (Vertex) null;
      float x = (float) ((double) edge1.c * (double) edge2.b - (double) edge2.c * (double) edge1.b) / num;
      float y = (float) ((double) edge2.c * (double) edge1.a - (double) edge1.c * (double) edge2.a) / num;
      Halfedge halfedge;
      Edge edge3;
      if (Voronoi.CompareByYThenX(edge1.rightSite, edge2.rightSite) < 0)
      {
        halfedge = halfedge0;
        edge3 = edge1;
      }
      else
      {
        halfedge = halfedge1;
        edge3 = edge2;
      }
      bool flag = (double) x >= (double) edge3.rightSite.x;
      Side? leftRight;
      if (flag)
      {
        leftRight = halfedge.leftRight;
        Side side = Side.LEFT;
        if (leftRight.GetValueOrDefault() == side & leftRight.HasValue)
          goto label_13;
      }
      if (!flag)
      {
        leftRight = halfedge.leftRight;
        Side side = Side.RIGHT;
        if (leftRight.GetValueOrDefault() == side & leftRight.HasValue)
          goto label_13;
      }
      return Vertex.Create(x, y);
label_13:
      return (Vertex) null;
    }

    public float x => this._coord.x;

    public float y => this._coord.y;
  }
}
