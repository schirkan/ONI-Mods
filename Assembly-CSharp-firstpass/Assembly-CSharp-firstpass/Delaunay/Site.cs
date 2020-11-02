// Decompiled with JetBrains decompiler
// Type: Delaunay.Site
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using Delaunay.LR;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
  public sealed class Site : ICoord, IComparable
  {
    private static Stack<Site> _pool = new Stack<Site>();
    private static readonly float EPSILON = 0.005f;
    private Vector2 _coord;
    public float scaled_weight;
    private uint _siteIndex;
    private List<Edge> _edges;
    private List<Side> _edgeOrientations;
    private List<Vector2> _region;

    public uint color { get; private set; }

    public float weight { get; private set; }

    internal List<Edge> edges => this._edges;

    public float x => this._coord.x;

    internal float y => this._coord.y;

    public Vector2 Coord => this._coord;

    public float Dist(ICoord p) => Vector2.Distance(p.Coord, this._coord);

    public override string ToString() => "Site " + this._siteIndex.ToString() + ": " + this.Coord.ToString();

    public static Site Create(Vector2 p, uint index, float weight, uint color) => Site._pool.Count > 0 ? Site._pool.Pop().Init(p, index, weight, color) : new Site(p, index, weight, color);

    internal static void SortSites(List<Site> sites) => sites.Sort();

    public int CompareTo(object obj)
    {
      Site s2 = (Site) obj;
      int num = Voronoi.CompareByYThenX(this, s2);
      switch (num)
      {
        case -1:
          if (this._siteIndex > s2._siteIndex)
          {
            uint siteIndex = this._siteIndex;
            this._siteIndex = s2._siteIndex;
            s2._siteIndex = siteIndex;
            break;
          }
          break;
        case 1:
          if (s2._siteIndex > this._siteIndex)
          {
            uint siteIndex = s2._siteIndex;
            s2._siteIndex = this._siteIndex;
            this._siteIndex = siteIndex;
            break;
          }
          break;
      }
      return num;
    }

    private static bool CloseEnough(Vector2 p0, Vector2 p1) => (double) Vector2.Distance(p0, p1) < (double) Site.EPSILON;

    private Site(Vector2 p, uint index, float weight, uint color) => this.Init(p, index, weight, color);

    private Site Init(Vector2 p, uint index, float weight, uint color)
    {
      this.scaled_weight = -1f;
      this._coord = p;
      this._siteIndex = index;
      this.weight = weight;
      this.color = color;
      this._edges = new List<Edge>();
      this._region = (List<Vector2>) null;
      return this;
    }

    private void Move(Vector2 p)
    {
      this.Clear();
      this._coord = p;
    }

    public void Dispose()
    {
      this.Clear();
      Site._pool.Push(this);
    }

    private void Clear()
    {
      if (this._edges != null)
      {
        this._edges.Clear();
        this._edges = (List<Edge>) null;
      }
      if (this._edgeOrientations != null)
      {
        this._edgeOrientations.Clear();
        this._edgeOrientations = (List<Side>) null;
      }
      if (this._region == null)
        return;
      this._region.Clear();
      this._region = (List<Vector2>) null;
    }

    public void AddEdge(Edge edge) => this._edges.Add(edge);

    public Vector2 GetClosestPt(Vector2 p) => this._coord + (p - this._coord).normalized * this.weight;

    public Edge NearestEdge()
    {
      this._edges.Sort((Comparison<Edge>) ((a, b) => Edge.CompareSitesDistances(a, b)));
      return this._edges[0];
    }

    public List<Site> NeighborSites()
    {
      if (this._edges == null || this._edges.Count == 0)
        return new List<Site>();
      if (this._edgeOrientations == null)
        this.ReorderEdges();
      List<Site> siteList = new List<Site>();
      for (int index = 0; index < this._edges.Count; ++index)
      {
        Edge edge = this._edges[index];
        siteList.Add(this.NeighborSite(edge));
      }
      return siteList;
    }

    private Site NeighborSite(Edge edge)
    {
      if (this == edge.leftSite)
        return edge.rightSite;
      return this == edge.rightSite ? edge.leftSite : (Site) null;
    }

    internal List<Vector2> Region(Rect clippingBounds)
    {
      if (this._edges == null || this._edges.Count == 0)
        return new List<Vector2>();
      if (this._edgeOrientations == null)
      {
        this.ReorderEdges();
        this._region = this.ClipToBounds(clippingBounds);
        if (new Polygon(this._region).Winding() == Winding.CLOCKWISE)
          this._region.Reverse();
      }
      return this._region;
    }

    internal List<Vector2> Region(Polygon clippingBounds)
    {
      if (this._edges == null || this._edges.Count == 0)
        return new List<Vector2>();
      if (this._edgeOrientations == null)
      {
        this.ReorderEdges();
        this._region = this.ClipToBounds(clippingBounds);
        if (new Polygon(this._region).Winding() == Winding.CLOCKWISE)
          this._region.Reverse();
      }
      return this._region;
    }

    private void ReorderEdges()
    {
      EdgeReorderer edgeReorderer = new EdgeReorderer(this._edges, VertexOrSite.VERTEX);
      this._edges = edgeReorderer.edges;
      this._edgeOrientations = edgeReorderer.edgeOrientations;
      edgeReorderer.Dispose();
    }

    private List<Vector2> ClipToBounds(Rect bounds)
    {
      List<Vector2> points = new List<Vector2>();
      int count = this._edges.Count;
      int num = 0;
      while (num < count && !this._edges[num].visible)
        ++num;
      if (num == count)
        return new List<Vector2>();
      Edge edge = this._edges[num];
      Side edgeOrientation = this._edgeOrientations[num];
      Vector2? clippedEnd = edge.clippedEnds[edgeOrientation];
      if (!clippedEnd.HasValue)
        Debug.LogError((object) "XXX: Null detected when there should be a Vector2!");
      clippedEnd = edge.clippedEnds[SideHelper.Other(edgeOrientation)];
      if (!clippedEnd.HasValue)
        Debug.LogError((object) "XXX: Null detected when there should be a Vector2!");
      List<Vector2> vector2List1 = points;
      clippedEnd = edge.clippedEnds[edgeOrientation];
      Vector2 vector2_1 = clippedEnd.Value;
      vector2List1.Add(vector2_1);
      List<Vector2> vector2List2 = points;
      clippedEnd = edge.clippedEnds[SideHelper.Other(edgeOrientation)];
      Vector2 vector2_2 = clippedEnd.Value;
      vector2List2.Add(vector2_2);
      for (int index = num + 1; index < count; ++index)
      {
        if (this._edges[index].visible)
          this.Connect(points, index, bounds);
      }
      this.Connect(points, num, bounds, true);
      return points;
    }

    private List<Vector2> ClipToBounds(Polygon bounds) => this.ClipToBounds(bounds.bounds);

    private void Connect(List<Vector2> points, int j, Rect bounds, bool closingUp = false)
    {
      Vector2 point = points[points.Count - 1];
      Edge edge = this._edges[j];
      Side edgeOrientation = this._edgeOrientations[j];
      if (!edge.clippedEnds[edgeOrientation].HasValue)
        Debug.LogError((object) "XXX: Null detected when there should be a Vector2!");
      Vector2 vector2 = edge.clippedEnds[edgeOrientation].Value;
      if (!Site.CloseEnough(point, vector2))
      {
        if ((double) point.x != (double) vector2.x && (double) point.y != (double) vector2.y)
        {
          int num1 = BoundsCheck.Check(point, bounds);
          int num2 = BoundsCheck.Check(vector2, bounds);
          if ((num1 & BoundsCheck.RIGHT) != 0)
          {
            float xMax = bounds.xMax;
            if ((num2 & BoundsCheck.BOTTOM) != 0)
            {
              float yMax = bounds.yMax;
              points.Add(new Vector2(xMax, yMax));
            }
            else if ((num2 & BoundsCheck.TOP) != 0)
            {
              float yMin = bounds.yMin;
              points.Add(new Vector2(xMax, yMin));
            }
            else if ((num2 & BoundsCheck.LEFT) != 0)
            {
              float y = (double) point.y - (double) bounds.y + (double) vector2.y - (double) bounds.y >= (double) bounds.height ? bounds.yMax : bounds.yMin;
              points.Add(new Vector2(xMax, y));
              points.Add(new Vector2(bounds.xMin, y));
            }
          }
          else if ((num1 & BoundsCheck.LEFT) != 0)
          {
            float xMin = bounds.xMin;
            if ((num2 & BoundsCheck.BOTTOM) != 0)
            {
              float yMax = bounds.yMax;
              points.Add(new Vector2(xMin, yMax));
            }
            else if ((num2 & BoundsCheck.TOP) != 0)
            {
              float yMin = bounds.yMin;
              points.Add(new Vector2(xMin, yMin));
            }
            else if ((num2 & BoundsCheck.RIGHT) != 0)
            {
              float y = (double) point.y - (double) bounds.y + (double) vector2.y - (double) bounds.y >= (double) bounds.height ? bounds.yMax : bounds.yMin;
              points.Add(new Vector2(xMin, y));
              points.Add(new Vector2(bounds.xMax, y));
            }
          }
          else if ((num1 & BoundsCheck.TOP) != 0)
          {
            float yMin = bounds.yMin;
            if ((num2 & BoundsCheck.RIGHT) != 0)
            {
              float xMax = bounds.xMax;
              points.Add(new Vector2(xMax, yMin));
            }
            else if ((num2 & BoundsCheck.LEFT) != 0)
            {
              float xMin = bounds.xMin;
              points.Add(new Vector2(xMin, yMin));
            }
            else if ((num2 & BoundsCheck.BOTTOM) != 0)
            {
              float x = (double) point.x - (double) bounds.x + (double) vector2.x - (double) bounds.x >= (double) bounds.width ? bounds.xMax : bounds.xMin;
              points.Add(new Vector2(x, yMin));
              points.Add(new Vector2(x, bounds.yMax));
            }
          }
          else if ((num1 & BoundsCheck.BOTTOM) != 0)
          {
            float yMax = bounds.yMax;
            if ((num2 & BoundsCheck.RIGHT) != 0)
            {
              float xMax = bounds.xMax;
              points.Add(new Vector2(xMax, yMax));
            }
            else if ((num2 & BoundsCheck.LEFT) != 0)
            {
              float xMin = bounds.xMin;
              points.Add(new Vector2(xMin, yMax));
            }
            else if ((num2 & BoundsCheck.TOP) != 0)
            {
              float x = (double) point.x - (double) bounds.x + (double) vector2.x - (double) bounds.x >= (double) bounds.width ? bounds.xMax : bounds.xMin;
              points.Add(new Vector2(x, yMax));
              points.Add(new Vector2(x, bounds.yMin));
            }
          }
        }
        if (closingUp)
          return;
        points.Add(vector2);
      }
      Vector2? clippedEnd = edge.clippedEnds[SideHelper.Other(edgeOrientation)];
      if (!clippedEnd.HasValue)
        Debug.LogError((object) "XXX: Null detected when there should be a Vector2!");
      clippedEnd = edge.clippedEnds[SideHelper.Other(edgeOrientation)];
      Vector2 p1 = clippedEnd.Value;
      if (Site.CloseEnough(points[0], p1))
        return;
      points.Add(p1);
    }
  }
}
