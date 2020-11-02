// Decompiled with JetBrains decompiler
// Type: Delaunay.DelaunayHelpers
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
  public static class DelaunayHelpers
  {
    public static List<LineSegment> VisibleLineSegments(List<Edge> edges)
    {
      List<LineSegment> lineSegmentList = new List<LineSegment>();
      for (int index = 0; index < edges.Count; ++index)
      {
        Edge edge = edges[index];
        if (edge.visible)
        {
          Vector2? clippedEnd1 = edge.clippedEnds[Side.LEFT];
          Vector2? clippedEnd2 = edge.clippedEnds[Side.RIGHT];
          lineSegmentList.Add(new LineSegment(clippedEnd1, clippedEnd2));
        }
      }
      return lineSegmentList;
    }

    public static List<DelaunayHelpers.LineSegmentWithSites> VisibleLineSegmentsWithSite(
      List<Edge> edges)
    {
      List<DelaunayHelpers.LineSegmentWithSites> segmentWithSitesList = new List<DelaunayHelpers.LineSegmentWithSites>();
      for (int index = 0; index < edges.Count; ++index)
      {
        Edge edge = edges[index];
        if (edge.visible)
        {
          Vector2? clippedEnd1 = edge.clippedEnds[Side.LEFT];
          Vector2? clippedEnd2 = edge.clippedEnds[Side.RIGHT];
          segmentWithSitesList.Add(new DelaunayHelpers.LineSegmentWithSites(clippedEnd1, clippedEnd2, edge.leftSite.color, edge.rightSite.color));
        }
      }
      return segmentWithSitesList;
    }

    public static List<Edge> SelectEdgesForSitePoint(Vector2 coord, List<Edge> edgesToTest) => edgesToTest.FindAll((Predicate<Edge>) (edge =>
    {
      if (edge.leftSite != null && edge.leftSite.Coord == coord)
        return true;
      return edge.rightSite != null && edge.rightSite.Coord == coord;
    }));

    public static List<Edge> SelectNonIntersectingEdges(List<Edge> edgesToTest) => edgesToTest;

    public static List<LineSegment> DelaunayLinesForEdges(List<Edge> edges)
    {
      List<LineSegment> lineSegmentList = new List<LineSegment>();
      for (int index = 0; index < edges.Count; ++index)
      {
        Edge edge = edges[index];
        lineSegmentList.Add(edge.DelaunayLine());
      }
      return lineSegmentList;
    }

    public static List<LineSegment> Kruskal(
      List<LineSegment> lineSegments,
      KruskalType type = KruskalType.MINIMUM)
    {
      Dictionary<Vector2?, Node> dictionary = new Dictionary<Vector2?, Node>();
      List<LineSegment> lineSegmentList = new List<LineSegment>();
      Stack<Node> pool = Node.pool;
      if (type == KruskalType.MAXIMUM)
        lineSegments.Sort((Comparison<LineSegment>) ((l1, l2) => LineSegment.CompareLengths(l1, l2)));
      else
        lineSegments.Sort((Comparison<LineSegment>) ((l1, l2) => LineSegment.CompareLengths_MAX(l1, l2)));
      int count = lineSegments.Count;
      while (--count > -1)
      {
        LineSegment lineSegment = lineSegments[count];
        Node node1;
        if (!dictionary.ContainsKey(lineSegment.p0))
        {
          Node node2 = pool.Count > 0 ? pool.Pop() : new Node();
          node1 = node2.parent = node2;
          node2.treeSize = 1;
          dictionary[lineSegment.p0] = node2;
        }
        else
          node1 = DelaunayHelpers.Find(dictionary[lineSegment.p0]);
        Node node3;
        if (!dictionary.ContainsKey(lineSegment.p1))
        {
          Node node2 = pool.Count > 0 ? pool.Pop() : new Node();
          node3 = node2.parent = node2;
          node2.treeSize = 1;
          dictionary[lineSegment.p1] = node2;
        }
        else
          node3 = DelaunayHelpers.Find(dictionary[lineSegment.p1]);
        if (node1 != node3)
        {
          lineSegmentList.Add(lineSegment);
          int treeSize1 = node1.treeSize;
          int treeSize2 = node3.treeSize;
          if (treeSize1 >= treeSize2)
          {
            node3.parent = node1;
            node1.treeSize += treeSize2;
          }
          else
          {
            node1.parent = node3;
            node3.treeSize += treeSize1;
          }
        }
      }
      foreach (Node node in dictionary.Values)
        pool.Push(node);
      return lineSegmentList;
    }

    private static Node Find(Node node)
    {
      if (node.parent == node)
        return node;
      Node node1 = DelaunayHelpers.Find(node.parent);
      node.parent = node1;
      return node1;
    }

    public class LineSegmentWithSites : LineSegment
    {
      public uint id0 { get; private set; }

      public uint id1 { get; private set; }

      public LineSegmentWithSites(Vector2? p0, Vector2? p1, uint id0, uint id1)
        : base(p0, p1)
      {
        this.id0 = id0;
        this.id1 = id1;
      }
    }
  }
}
