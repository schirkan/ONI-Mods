// Decompiled with JetBrains decompiler
// Type: Delaunay.Voronoi
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
  public sealed class Voronoi : Delaunay.Utils.IDisposable
  {
    private SiteList _sites;
    private Dictionary<Vector2, Site> _sitesIndexedByLocation;
    private List<Triangle> _triangles;
    private List<Edge> _edges;
    private float min_weight;
    private float max_weight;
    private Rect _plotBounds;
    private float weightSum;
    private Site fortunesAlgorithm_bottomMostSite;

    public Rect plotBounds => this._plotBounds;

    public void Dispose()
    {
      if (this._sites != null)
      {
        this._sites.Dispose();
        this._sites = (SiteList) null;
      }
      if (this._triangles != null)
      {
        int count = this._triangles.Count;
        for (int index = 0; index < count; ++index)
          this._triangles[index].Dispose();
        this._triangles.Clear();
        this._triangles = (List<Triangle>) null;
      }
      if (this._edges != null)
      {
        int count = this._edges.Count;
        for (int index = 0; index < count; ++index)
          this._edges[index].Dispose();
        this._edges.Clear();
        this._edges = (List<Edge>) null;
      }
      this._sitesIndexedByLocation = (Dictionary<Vector2, Site>) null;
    }

    public Voronoi(List<Vector2> points, List<uint> colors, List<float> weights, Rect plotBounds)
    {
      this._plotBounds = plotBounds;
      this.min_weight = float.MaxValue;
      this.max_weight = float.MinValue;
      this._sites = new SiteList();
      this._sitesIndexedByLocation = new Dictionary<Vector2, Site>();
      this._triangles = new List<Triangle>();
      this._edges = new List<Edge>();
      this.AddSites(points, colors, weights);
      float num = this.max_weight - this.min_weight;
      if ((double) num > 0.0)
        this._sites.ScaleWeight(1f + num);
      this.FortunesAlgorithm();
    }

    private void AddSites(List<Vector2> points, List<uint> colors, List<float> weights)
    {
      this.weightSum = 0.0f;
      for (int index = 0; index < points.Count; ++index)
        this.AddSite(points[index], colors != null ? colors[index] : 0U, index, weights == null ? 1f : weights[index]);
    }

    private void AddSite(Vector2 p, uint color, int index, float weight = 1f)
    {
      if (this._sitesIndexedByLocation.ContainsKey(p))
        return;
      Site site = Site.Create(p, (uint) index, weight, color);
      this.min_weight = Mathf.Min(this.min_weight, weight);
      this.max_weight = Mathf.Max(this.max_weight, weight);
      this._sitesIndexedByLocation[p] = site;
      this._sites.Add(site);
      this.weightSum += site.weight;
    }

    public Site GetSiteByLocation(Vector2 p) => this._sitesIndexedByLocation[p];

    public List<Edge> Edges() => this._edges;

    public List<Vector2> Region(Vector2 p)
    {
      Site site = this._sitesIndexedByLocation[p];
      return site == null ? new List<Vector2>() : site.Region(this._plotBounds);
    }

    public List<Vector2> NeighborSitesForSite(Vector2 coord)
    {
      List<Vector2> vector2List = new List<Vector2>();
      Site site1 = this._sitesIndexedByLocation[coord];
      if (site1 == null)
        return vector2List;
      List<Site> siteList = site1.NeighborSites();
      for (int index = 0; index < siteList.Count; ++index)
      {
        Site site2 = siteList[index];
        vector2List.Add(site2.Coord);
      }
      return vector2List;
    }

    public HashSet<uint> NeighborSitesIDsForSite(Vector2 coord)
    {
      HashSet<uint> uintSet = new HashSet<uint>();
      Site site = this._sitesIndexedByLocation[coord];
      if (site == null)
        return uintSet;
      List<Site> siteList = site.NeighborSites();
      for (int index = 0; index < siteList.Count; ++index)
        uintSet.Add(siteList[index].color);
      return uintSet;
    }

    public List<uint> ListNeighborSitesIDsForSite(Vector2 coord)
    {
      List<uint> uintList = new List<uint>();
      Site site = this._sitesIndexedByLocation[coord];
      if (site == null)
        return uintList;
      List<Site> siteList = site.NeighborSites();
      for (int index = 0; index < siteList.Count; ++index)
        uintList.Add(siteList[index].color);
      return uintList;
    }

    public List<Circle> Circles() => this._sites.Circles();

    public List<LineSegment> VoronoiBoundaryForSite(Vector2 coord) => DelaunayHelpers.VisibleLineSegments(DelaunayHelpers.SelectEdgesForSitePoint(coord, this._edges));

    public List<LineSegment> DelaunayLinesForSite(Vector2 coord) => DelaunayHelpers.DelaunayLinesForEdges(DelaunayHelpers.SelectEdgesForSitePoint(coord, this._edges));

    public List<LineSegment> VoronoiDiagram() => DelaunayHelpers.VisibleLineSegments(this._edges);

    public List<LineSegment> DelaunayTriangulation() => DelaunayHelpers.DelaunayLinesForEdges(DelaunayHelpers.SelectNonIntersectingEdges(this._edges));

    public List<LineSegment> Hull() => DelaunayHelpers.DelaunayLinesForEdges(this.HullEdges());

    private List<Edge> HullEdges() => this._edges.FindAll((Predicate<Edge>) (edge => edge.IsPartOfConvexHull()));

    public List<Vector2> HullPointsInOrder()
    {
      List<Edge> origEdges = this.HullEdges();
      List<Vector2> vector2List = new List<Vector2>();
      if (origEdges.Count == 0)
        return vector2List;
      EdgeReorderer edgeReorderer = new EdgeReorderer(origEdges, VertexOrSite.SITE);
      List<Edge> edges = edgeReorderer.edges;
      List<Side> edgeOrientations = edgeReorderer.edgeOrientations;
      edgeReorderer.Dispose();
      int count = edges.Count;
      for (int index = 0; index < count; ++index)
      {
        Edge edge = edges[index];
        Side leftRight = edgeOrientations[index];
        vector2List.Add(edge.Site(leftRight).Coord);
      }
      return vector2List;
    }

    public List<LineSegment> SpanningTree(KruskalType type = KruskalType.MINIMUM) => DelaunayHelpers.Kruskal(DelaunayHelpers.DelaunayLinesForEdges(DelaunayHelpers.SelectNonIntersectingEdges(this._edges)), type);

    public List<List<Vector2>> Regions() => this._sites.Regions(this._plotBounds);

    public List<uint> SiteColors() => this._sites.SiteColors();

    public List<Vector2> SiteCoords() => this._sites.SiteCoords();

    private void FortunesAlgorithm()
    {
      Vector2 s2 = Vector2.zero;
      Rect sitesBounds = this._sites.GetSitesBounds();
      int sqrt_nsites = (int) Mathf.Sqrt((float) (this._sites.Count + 4));
      HalfedgePriorityQueue halfedgePriorityQueue = new HalfedgePriorityQueue(sitesBounds.y, sitesBounds.height, sqrt_nsites);
      EdgeList edgeList = new EdgeList(sitesBounds.x, sitesBounds.width, sqrt_nsites);
      List<Halfedge> halfedgeList = new List<Halfedge>();
      List<Vertex> vertexList = new List<Vertex>();
      this.fortunesAlgorithm_bottomMostSite = this._sites.Next();
      Site site1 = this._sites.Next();
      while (true)
      {
        Halfedge listRightNeighbor1;
        Site site0;
        Halfedge halfedge1;
        Vertex vertex1;
        do
        {
          if (!halfedgePriorityQueue.Empty())
            s2 = halfedgePriorityQueue.Min();
          if (site1 != null && (halfedgePriorityQueue.Empty() || Voronoi.CompareByYThenX(site1, s2) < 0))
          {
            Halfedge halfedge2 = edgeList.EdgeListLeftNeighbor(site1.Coord);
            Halfedge listRightNeighbor2 = halfedge2.edgeListRightNeighbor;
            Edge bisectingEdge = Edge.CreateBisectingEdge(this.FortunesAlgorithm_rightRegion(halfedge2), site1);
            this._edges.Add(bisectingEdge);
            Halfedge halfedge3 = Halfedge.Create(bisectingEdge, new Side?(Side.LEFT));
            halfedgeList.Add(halfedge3);
            edgeList.Insert(halfedge2, halfedge3);
            Vertex vertex2;
            if ((vertex2 = Vertex.Intersect(halfedge2, halfedge3)) != null)
            {
              vertexList.Add(vertex2);
              halfedgePriorityQueue.Remove(halfedge2);
              halfedge2.vertex = vertex2;
              halfedge2.ystar = vertex2.y + site1.Dist((ICoord) vertex2);
              halfedgePriorityQueue.Insert(halfedge2);
            }
            Halfedge lb = halfedge3;
            Halfedge halfedge4 = Halfedge.Create(bisectingEdge, new Side?(Side.RIGHT));
            halfedgeList.Add(halfedge4);
            edgeList.Insert(lb, halfedge4);
            Vertex vertex3;
            if ((vertex3 = Vertex.Intersect(halfedge4, listRightNeighbor2)) != null)
            {
              vertexList.Add(vertex3);
              halfedge4.vertex = vertex3;
              halfedge4.ystar = vertex3.y + site1.Dist((ICoord) vertex3);
              halfedgePriorityQueue.Insert(halfedge4);
            }
            site1 = this._sites.Next();
          }
          else if (!halfedgePriorityQueue.Empty())
          {
            Halfedge min = halfedgePriorityQueue.ExtractMin();
            Halfedge listLeftNeighbor = min.edgeListLeftNeighbor;
            Halfedge listRightNeighbor2 = min.edgeListRightNeighbor;
            listRightNeighbor1 = listRightNeighbor2.edgeListRightNeighbor;
            site0 = this.FortunesAlgorithm_leftRegion(min);
            Site site1_1 = this.FortunesAlgorithm_rightRegion(listRightNeighbor2);
            Vertex vertex2 = min.vertex;
            vertex2.SetIndex();
            min.edge.SetVertex(min.leftRight.Value, vertex2);
            listRightNeighbor2.edge.SetVertex(listRightNeighbor2.leftRight.Value, vertex2);
            edgeList.Remove(min);
            halfedgePriorityQueue.Remove(listRightNeighbor2);
            edgeList.Remove(listRightNeighbor2);
            Side leftRight = Side.LEFT;
            if ((double) site0.y > (double) site1_1.y)
            {
              Site site2 = site0;
              site0 = site1_1;
              site1_1 = site2;
              leftRight = Side.RIGHT;
            }
            Edge bisectingEdge = Edge.CreateBisectingEdge(site0, site1_1);
            this._edges.Add(bisectingEdge);
            halfedge1 = Halfedge.Create(bisectingEdge, new Side?(leftRight));
            halfedgeList.Add(halfedge1);
            edgeList.Insert(listLeftNeighbor, halfedge1);
            bisectingEdge.SetVertex(SideHelper.Other(leftRight), vertex2);
            Vertex vertex3;
            if ((vertex3 = Vertex.Intersect(listLeftNeighbor, halfedge1)) != null)
            {
              vertexList.Add(vertex3);
              halfedgePriorityQueue.Remove(listLeftNeighbor);
              listLeftNeighbor.vertex = vertex3;
              listLeftNeighbor.ystar = vertex3.y + site0.Dist((ICoord) vertex3);
              halfedgePriorityQueue.Insert(listLeftNeighbor);
            }
          }
          else
            goto label_16;
        }
        while ((vertex1 = Vertex.Intersect(halfedge1, listRightNeighbor1)) == null);
        vertexList.Add(vertex1);
        halfedge1.vertex = vertex1;
        halfedge1.ystar = vertex1.y + site0.Dist((ICoord) vertex1);
        halfedgePriorityQueue.Insert(halfedge1);
      }
label_16:
      halfedgePriorityQueue.Dispose();
      edgeList.Dispose();
      for (int index = 0; index < halfedgeList.Count; ++index)
        halfedgeList[index].ReallyDispose();
      halfedgeList.Clear();
      for (int index = 0; index < this._edges.Count; ++index)
        this._edges[index].ClipVertices(this._plotBounds);
      for (int index = 0; index < vertexList.Count; ++index)
        vertexList[index].Dispose();
      vertexList.Clear();
    }

    private Site FortunesAlgorithm_leftRegion(Halfedge he)
    {
      Edge edge = he.edge;
      return edge == null ? this.fortunesAlgorithm_bottomMostSite : edge.Site(he.leftRight.Value);
    }

    private Site FortunesAlgorithm_rightRegion(Halfedge he)
    {
      Edge edge = he.edge;
      return edge == null ? this.fortunesAlgorithm_bottomMostSite : edge.Site(SideHelper.Other(he.leftRight.Value));
    }

    public static int CompareByYThenX(Site s1, Site s2)
    {
      if ((double) s1.y < (double) s2.y)
        return -1;
      if ((double) s1.y > (double) s2.y)
        return 1;
      if ((double) s1.x < (double) s2.x)
        return -1;
      return (double) s1.x > (double) s2.x ? 1 : 0;
    }

    public static int CompareByYThenX(Site s1, Vector2 s2)
    {
      if ((double) s1.y < (double) s2.y)
        return -1;
      if ((double) s1.y > (double) s2.y)
        return 1;
      if ((double) s1.x < (double) s2.x)
        return -1;
      return (double) s1.x > (double) s2.x ? 1 : 0;
    }
  }
}
