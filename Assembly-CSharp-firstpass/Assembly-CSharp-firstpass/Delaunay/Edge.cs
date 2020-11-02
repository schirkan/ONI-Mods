// Decompiled with JetBrains decompiler
// Type: Delaunay.Edge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using Delaunay.LR;
using System.Collections.Generic;
using UnityEngine;

namespace Delaunay
{
  public sealed class Edge
  {
    private static Stack<Edge> _pool = new Stack<Edge>();
    private static int _nedges = 0;
    public static readonly Edge DELETED = new Edge();
    public float a;
    public float b;
    public float c;
    private Delaunay.Vertex _leftVertex;
    private Delaunay.Vertex _rightVertex;
    private Dictionary<Side, Vector2?> _clippedVertices;
    private Dictionary<Side, Delaunay.Site> _sites;
    private int _edgeIndex;

    public static Edge CreateBisectingEdge(Delaunay.Site site0, Delaunay.Site site1)
    {
      Vector2 coord1 = site1.Coord;
      Vector2 coord2 = site0.Coord;
      float num1 = coord2.x - coord1.x;
      float num2 = coord2.y - coord1.y;
      double num3 = (double) num1 > 0.0 ? (double) num1 : -(double) num1;
      float num4 = (double) num2 > 0.0 ? num2 : -num2;
      float num5 = (float) ((double) coord1.x * (double) num1 + (double) coord1.y * (double) num2 + ((double) num1 * (double) num1 + (double) num2 * (double) num2) * 0.5);
      double num6 = (double) num4;
      float num7;
      float num8;
      float num9;
      if (num3 > num6)
      {
        num7 = 1f;
        num8 = num2 / num1;
        num9 = num5 / num1;
      }
      else
      {
        num8 = 1f;
        num7 = num1 / num2;
        num9 = num5 / num2;
      }
      Edge edge = Edge.Create();
      edge.leftSite = site0;
      edge.rightSite = site1;
      site0.AddEdge(edge);
      site1.AddEdge(edge);
      edge._leftVertex = (Delaunay.Vertex) null;
      edge._rightVertex = (Delaunay.Vertex) null;
      edge.a = num7;
      edge.b = num8;
      edge.c = num9;
      return edge;
    }

    private static Edge Create()
    {
      Edge edge;
      if (Edge._pool.Count > 0)
      {
        edge = Edge._pool.Pop();
        edge.Init();
      }
      else
        edge = new Edge();
      return edge;
    }

    public LineSegment DelaunayLine() => new LineSegment(new Vector2?(this.leftSite.Coord), new Vector2?(this.rightSite.Coord));

    public LineSegment VoronoiEdge() => !this.visible ? new LineSegment(new Vector2?(), new Vector2?()) : new LineSegment(this._clippedVertices[Side.LEFT], this._clippedVertices[Side.RIGHT]);

    public Delaunay.Vertex leftVertex => this._leftVertex;

    public Delaunay.Vertex rightVertex => this._rightVertex;

    public Delaunay.Vertex Vertex(Side leftRight) => leftRight != Side.LEFT ? this._rightVertex : this._leftVertex;

    public void SetVertex(Side leftRight, Delaunay.Vertex v)
    {
      if (leftRight == Side.LEFT)
        this._leftVertex = v;
      else
        this._rightVertex = v;
    }

    public bool IsPartOfConvexHull() => this._leftVertex == null || this._rightVertex == null;

    public float SitesDistance() => Vector2.Distance(this.leftSite.Coord, this.rightSite.Coord) + (float) (((double) this.leftSite.weight + (double) this.rightSite.weight) * ((double) this.leftSite.weight + (double) this.rightSite.weight));

    public static int CompareSitesDistances_MAX(Edge edge0, Edge edge1)
    {
      float num1 = edge0.SitesDistance();
      float num2 = edge1.SitesDistance();
      if ((double) num1 < (double) num2)
        return 1;
      return (double) num1 > (double) num2 ? -1 : 0;
    }

    public static int CompareSitesDistances(Edge edge0, Edge edge1) => -Edge.CompareSitesDistances_MAX(edge0, edge1);

    public Dictionary<Side, Vector2?> clippedEnds => this._clippedVertices;

    public bool visible => this._clippedVertices != null;

    public Delaunay.Site leftSite
    {
      get => this._sites[Side.LEFT];
      set => this._sites[Side.LEFT] = value;
    }

    public Delaunay.Site rightSite
    {
      get => this._sites[Side.RIGHT];
      set => this._sites[Side.RIGHT] = value;
    }

    public Delaunay.Site Site(Side leftRight) => this._sites[leftRight];

    public void Dispose()
    {
      this._leftVertex = (Delaunay.Vertex) null;
      this._rightVertex = (Delaunay.Vertex) null;
      if (this._clippedVertices != null)
      {
        this._clippedVertices[Side.LEFT] = new Vector2?();
        this._clippedVertices[Side.RIGHT] = new Vector2?();
        this._clippedVertices = (Dictionary<Side, Vector2?>) null;
      }
      this._sites[Side.LEFT] = (Delaunay.Site) null;
      this._sites[Side.RIGHT] = (Delaunay.Site) null;
      this._sites = (Dictionary<Side, Delaunay.Site>) null;
      Edge._pool.Push(this);
    }

    private Edge()
    {
      this._edgeIndex = Edge._nedges++;
      this.Init();
    }

    private void Init() => this._sites = new Dictionary<Side, Delaunay.Site>();

    public override string ToString() => "Edge " + this._edgeIndex.ToString() + "; sites " + this._sites[Side.LEFT].ToString() + ", " + this._sites[Side.RIGHT].ToString() + "; endVertices " + (this._leftVertex != null ? this._leftVertex.vertexIndex.ToString() : "null") + ", " + (this._rightVertex != null ? this._rightVertex.vertexIndex.ToString() : "null") + "::";

    public void ClipVertices(Rect bounds)
    {
      float xMin = bounds.xMin;
      float yMin = bounds.yMin;
      float xMax = bounds.xMax;
      float yMax = bounds.yMax;
      Delaunay.Vertex vertex1;
      Delaunay.Vertex vertex2;
      if ((double) this.a == 1.0 && (double) this.b >= 0.0)
      {
        vertex1 = this._rightVertex;
        vertex2 = this._leftVertex;
      }
      else
      {
        vertex1 = this._leftVertex;
        vertex2 = this._rightVertex;
      }
      float y1;
      float x1;
      float y2;
      float x2;
      if ((double) this.a == 1.0)
      {
        y1 = yMin;
        if (vertex1 != null && (double) vertex1.y > (double) yMin)
          y1 = vertex1.y;
        if ((double) y1 > (double) yMax)
          return;
        x1 = this.c - this.b * y1;
        y2 = yMax;
        if (vertex2 != null && (double) vertex2.y < (double) yMax)
          y2 = vertex2.y;
        if ((double) y2 < (double) yMin)
          return;
        x2 = this.c - this.b * y2;
        if ((double) x1 > (double) xMax && (double) x2 > (double) xMax || (double) x1 < (double) xMin && (double) x2 < (double) xMin)
          return;
        if ((double) x1 > (double) xMax)
        {
          x1 = xMax;
          y1 = (this.c - x1) / this.b;
        }
        else if ((double) x1 < (double) xMin)
        {
          x1 = xMin;
          y1 = (this.c - x1) / this.b;
        }
        if ((double) x2 > (double) xMax)
        {
          x2 = xMax;
          y2 = (this.c - x2) / this.b;
        }
        else if ((double) x2 < (double) xMin)
        {
          x2 = xMin;
          y2 = (this.c - x2) / this.b;
        }
      }
      else
      {
        x1 = xMin;
        if (vertex1 != null && (double) vertex1.x > (double) xMin)
          x1 = vertex1.x;
        if ((double) x1 > (double) xMax)
          return;
        y1 = this.c - this.a * x1;
        x2 = xMax;
        if (vertex2 != null && (double) vertex2.x < (double) xMax)
          x2 = vertex2.x;
        if ((double) x2 < (double) xMin)
          return;
        y2 = this.c - this.a * x2;
        if ((double) y1 > (double) yMax && (double) y2 > (double) yMax || (double) y1 < (double) yMin && (double) y2 < (double) yMin)
          return;
        if ((double) y1 > (double) yMax)
        {
          y1 = yMax;
          x1 = (this.c - y1) / this.a;
        }
        else if ((double) y1 < (double) yMin)
        {
          y1 = yMin;
          x1 = (this.c - y1) / this.a;
        }
        if ((double) y2 > (double) yMax)
        {
          y2 = yMax;
          x2 = (this.c - y2) / this.a;
        }
        else if ((double) y2 < (double) yMin)
        {
          y2 = yMin;
          x2 = (this.c - y2) / this.a;
        }
      }
      this._clippedVertices = new Dictionary<Side, Vector2?>();
      if (vertex1 == this._leftVertex)
      {
        this._clippedVertices[Side.LEFT] = new Vector2?(new Vector2(x1, y1));
        this._clippedVertices[Side.RIGHT] = new Vector2?(new Vector2(x2, y2));
      }
      else
      {
        this._clippedVertices[Side.RIGHT] = new Vector2?(new Vector2(x1, y1));
        this._clippedVertices[Side.LEFT] = new Vector2?(new Vector2(x2, y2));
      }
    }

    public void ClipVertices(Polygon bounds)
    {
      LineSegment segment = new LineSegment(new Vector2?(), new Vector2?());
      int num = (double) this.a != 1.0 ? 0 : ((double) this.b >= 0.0 ? 1 : 0);
      if (num != 0)
      {
        segment.p0 = new Vector2?(this._rightVertex.Coord);
        segment.p1 = new Vector2?(this._leftVertex.Coord);
      }
      else
      {
        segment.p0 = new Vector2?(this._leftVertex.Coord);
        segment.p1 = new Vector2?(this._rightVertex.Coord);
      }
      LineSegment intersectingSegment = new LineSegment(new Vector2?(), new Vector2?());
      bounds.ClipSegment(segment, ref intersectingSegment);
      this._clippedVertices = new Dictionary<Side, Vector2?>();
      if (num == 0)
      {
        this._clippedVertices[Side.LEFT] = intersectingSegment.p0;
        this._clippedVertices[Side.RIGHT] = intersectingSegment.p1;
      }
      else
      {
        this._clippedVertices[Side.RIGHT] = intersectingSegment.p0;
        this._clippedVertices[Side.LEFT] = intersectingSegment.p1;
      }
    }
  }
}
