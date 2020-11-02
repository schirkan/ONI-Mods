// Decompiled with JetBrains decompiler
// Type: VoronoiTree.Node
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoronoiTree
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Node
  {
    public static int maxDepth;
    public static uint maxIndex;
    [Serialize]
    public Node.NodeType type;
    public Node.VisitedType visited;
    public LoggerSSF log;
    [Serialize]
    public Diagram.Site site;
    [Serialize]
    public TagSet tags;
    public Dictionary<Tag, int> minDistanceToTag = new Dictionary<Tag, int>();

    public Tree parent { get; private set; }

    public PowerDiagram debug_LastPD { get; private set; }

    public void SetParent(Tree newParent) => this.parent = newParent;

    public Node()
    {
      this.type = Node.NodeType.Unknown;
      this.log = new LoggerSSF("VoronoiNode");
    }

    public Node(Node.NodeType type)
    {
      this.type = type;
      this.tags = new TagSet();
      this.log = new LoggerSSF("VoronoiNode");
    }

    protected Node(Diagram.Site site, Node.NodeType type, Tree parent)
    {
      this.tags = new TagSet();
      this.site = site;
      this.type = type;
      this.parent = parent;
      this.log = new LoggerSSF("VoronoiNode");
    }

    public Node GetNeighbour(uint id)
    {
      HashSet<KeyValuePair<uint, int>>.Enumerator enumerator = this.site.neighbours.GetEnumerator();
      while (enumerator.MoveNext())
      {
        if ((int) enumerator.Current.Key == (int) id)
          return this.GetSibling(id);
      }
      return (Node) null;
    }

    public List<Node> GetNeighbors()
    {
      List<Node> nodeList = new List<Node>();
      if (this.site.neighbours != null)
      {
        HashSet<KeyValuePair<uint, int>>.Enumerator enumerator = this.site.neighbours.GetEnumerator();
        while (enumerator.MoveNext())
          nodeList.Add(this.GetSibling(enumerator.Current.Key));
      }
      return nodeList;
    }

    public List<KeyValuePair<Node, LineSegment>> GetNeighborsByEdge()
    {
      List<KeyValuePair<Node, LineSegment>> keyValuePairList = new List<KeyValuePair<Node, LineSegment>>();
      for (int edgeIndex = 0; edgeIndex < this.site.poly.Vertices.Count; ++edgeIndex)
      {
        if (this.site.neighbours != null)
        {
          LineSegment edge = this.site.poly.GetEdge(edgeIndex);
          Node key = (Node) null;
          HashSet<KeyValuePair<uint, int>>.Enumerator enumerator = this.site.neighbours.GetEnumerator();
          while (enumerator.MoveNext())
          {
            KeyValuePair<uint, int> current = enumerator.Current;
            if (current.Value == edgeIndex)
            {
              current = enumerator.Current;
              key = this.GetSibling(current.Key);
            }
          }
          if (key != null)
            keyValuePairList.Add(new KeyValuePair<Node, LineSegment>(key, edge));
        }
      }
      return keyValuePairList;
    }

    public Node GetSibling(uint siteId) => this.parent.GetChildByID(siteId);

    public List<Node> GetSiblings()
    {
      List<Node> nodeList = new List<Node>();
      for (int childIndex = 0; childIndex < this.parent.ChildCount(); ++childIndex)
      {
        Node child = this.parent.GetChild(childIndex);
        if (child != this)
          nodeList.Add(child);
      }
      return nodeList;
    }

    public void PlaceSites(List<Diagram.Site> sites, int seed)
    {
      SeededRandom rnd = new SeededRandom(seed);
      List<Vector2> vector2List = (List<Vector2>) null;
      List<Vector2> avoidPoints = new List<Vector2>();
      for (int index = 0; index < sites.Count; ++index)
        avoidPoints.Add(sites[index].position);
      int num = 0;
      for (int index = 0; index < sites.Count; ++index)
      {
        if (!this.site.poly.Contains(sites[index].position))
        {
          if (vector2List == null)
            vector2List = PointGenerator.GetRandomPoints(this.site.poly, 5f, 1f, avoidPoints, PointGenerator.SampleBehaviour.PoissonDisk, true, rnd);
          if (num >= vector2List.Count - 1)
          {
            avoidPoints.AddRange((IEnumerable<Vector2>) vector2List);
            vector2List = PointGenerator.GetRandomPoints(this.site.poly, 0.5f, 0.5f, avoidPoints, PointGenerator.SampleBehaviour.PoissonDisk, true, rnd);
            num = 0;
          }
          sites[index].position = vector2List.Count != 0 ? vector2List[num++] : sites[0].position + Vector2.one * rnd.RandomValue();
        }
      }
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      for (int index = 0; index < sites.Count; ++index)
      {
        if (vector2Set.Contains(sites[index].position))
        {
          this.visited = Node.VisitedType.Error;
          sites[index].position += new Vector2((float) rnd.RandomRange(0, 1), (float) rnd.RandomRange(0, 1));
        }
        vector2Set.Add(sites[index].position);
        sites[index].poly = (Polygon) null;
      }
    }

    public bool ComputeNode(List<Diagram.Site> diagramSites)
    {
      if (this.site.poly == null || diagramSites == null || diagramSites.Count == 0)
      {
        this.visited = Node.VisitedType.MissingData;
        return false;
      }
      this.visited = Node.VisitedType.VisitedSuccess;
      if (diagramSites.Count == 1)
      {
        diagramSites[0].poly = this.site.poly;
        diagramSites[0].position = diagramSites[0].poly.Centroid();
        return true;
      }
      HashSet<Diagram.Site> siteSet1 = new HashSet<Diagram.Site>();
      for (int index = 0; index < diagramSites.Count; ++index)
        siteSet1.Add(new Diagram.Site(diagramSites[index].id, diagramSites[index].position, diagramSites[index].weight));
      siteSet1.Add(new Diagram.Site(Node.maxIndex + 1U, new Vector2(this.site.poly.bounds.xMin - 500f, this.site.poly.bounds.yMin + this.site.poly.bounds.height / 2f)));
      HashSet<Diagram.Site> siteSet2 = siteSet1;
      int num1 = (int) Node.maxIndex + 2;
      Rect bounds1 = this.site.poly.bounds;
      double num2 = (double) bounds1.xMax + 500.0;
      bounds1 = this.site.poly.bounds;
      double yMin = (double) bounds1.yMin;
      bounds1 = this.site.poly.bounds;
      double num3 = (double) bounds1.height / 2.0;
      double num4 = yMin + num3;
      Vector2 pos1 = new Vector2((float) num2, (float) num4);
      Diagram.Site site1 = new Diagram.Site((uint) num1, pos1);
      siteSet2.Add(site1);
      HashSet<Diagram.Site> siteSet3 = siteSet1;
      int num5 = (int) Node.maxIndex + 3;
      double xMin1 = (double) this.site.poly.bounds.xMin;
      Rect bounds2 = this.site.poly.bounds;
      double num6 = (double) bounds2.width / 2.0;
      double num7 = xMin1 + num6;
      bounds2 = this.site.poly.bounds;
      double num8 = (double) bounds2.yMin - 500.0;
      Vector2 pos2 = new Vector2((float) num7, (float) num8);
      Diagram.Site site2 = new Diagram.Site((uint) num5, pos2);
      siteSet3.Add(site2);
      HashSet<Diagram.Site> siteSet4 = siteSet1;
      int num9 = (int) Node.maxIndex + 4;
      double xMin2 = (double) this.site.poly.bounds.xMin;
      Rect bounds3 = this.site.poly.bounds;
      double num10 = (double) bounds3.width / 2.0;
      double num11 = xMin2 + num10;
      bounds3 = this.site.poly.bounds;
      double num12 = (double) bounds3.yMax + 500.0;
      Vector2 pos3 = new Vector2((float) num11, (float) num12);
      Diagram.Site site3 = new Diagram.Site((uint) num9, pos3);
      siteSet4.Add(site3);
      Diagram diagram = new Diagram(new Rect(this.site.poly.bounds.xMin - 500f, this.site.poly.bounds.yMin - 500f, this.site.poly.bounds.width + 500f, this.site.poly.bounds.height + 500f), (IEnumerable<Diagram.Site>) siteSet1);
      for (int index = 0; index < diagramSites.Count; ++index)
      {
        if (diagramSites[index].id <= Node.maxIndex)
        {
          List<Vector2> verts = diagram.diagram.Region(diagramSites[index].position);
          if (verts == null)
          {
            if (this.type != Node.NodeType.Leaf)
            {
              this.visited = Node.VisitedType.Error;
              return false;
            }
          }
          else
          {
            Polygon polygon = new Polygon(verts).Clip(this.site.poly);
            if (polygon == null || polygon.Vertices.Count < 3)
            {
              if (this.type != Node.NodeType.Leaf)
              {
                this.visited = Node.VisitedType.Error;
                return false;
              }
            }
            else
              diagramSites[index].poly = polygon;
          }
        }
      }
      for (int index = 0; index < diagramSites.Count; ++index)
      {
        if (diagramSites[index].id <= Node.maxIndex)
        {
          HashSet<uint> neighbours = diagram.diagram.NeighborSitesIDsForSite(diagramSites[index].position);
          Node.FilterNeighbours(diagramSites[index], neighbours, diagramSites);
          diagramSites[index].position = diagramSites[index].poly.Centroid();
        }
      }
      return true;
    }

    public bool ComputeNodePD(List<Diagram.Site> diagramSites, int maxIters = 500, float threshold = 0.2f)
    {
      if (this.site.poly == null || diagramSites == null || diagramSites.Count == 0)
      {
        this.visited = Node.VisitedType.MissingData;
        return false;
      }
      this.visited = Node.VisitedType.VisitedSuccess;
      List<Site> siteList = new List<Site>();
      for (int index = 0; index < diagramSites.Count; ++index)
      {
        Site site = new Site(diagramSites[index].id, diagramSites[index].position, diagramSites[index].weight);
        siteList.Add(site);
      }
      PowerDiagram powerDiagram = new PowerDiagram(this.site.poly, (IEnumerable<Site>) siteList);
      powerDiagram.ComputeVD();
      powerDiagram.ComputePowerDiagram(maxIters, threshold);
      for (int index = 0; index < diagramSites.Count; ++index)
      {
        diagramSites[index].poly = siteList[index].poly;
        if (diagramSites[index].poly == null)
          Debug.LogErrorFormat("Site [{0}] at index [{1}]: Poly shouldnt be null here ever", (object) diagramSites[index].id, (object) index);
      }
      for (int index1 = 0; index1 < diagramSites.Count; ++index1)
      {
        HashSet<uint> neighbours = new HashSet<uint>();
        for (int index2 = 0; index2 < siteList[index1].neighbours.Count; ++index2)
        {
          if (!siteList[index1].neighbours[index2].dummy)
            neighbours.Add((uint) siteList[index1].neighbours[index2].id);
        }
        Node.FilterNeighbours(diagramSites[index1], neighbours, diagramSites);
        diagramSites[index1].position = diagramSites[index1].poly.Centroid();
      }
      this.debug_LastPD = powerDiagram;
      return true;
    }

    private static void FilterNeighbours(
      Diagram.Site home,
      HashSet<uint> neighbours,
      List<Diagram.Site> sites)
    {
      if (home == null)
        Debug.LogError((object) "FilterNeighbours home == null");
      HashSet<KeyValuePair<uint, int>> keyValuePairSet = new HashSet<KeyValuePair<uint, int>>();
      HashSet<uint>.Enumerator niter = neighbours.GetEnumerator();
      while (niter.MoveNext())
      {
        Diagram.Site site = sites.Find((Predicate<Diagram.Site>) (s => (int) s.id == (int) niter.Current));
        if (site != null)
        {
          if (site.poly == null)
            Debug.LogError((object) "FilterNeighbours neighbour.poly == null");
          int edgeIdx = -1;
          if (home.poly.SharesEdge(site.poly, ref edgeIdx) == Polygon.Commonality.Edge)
            keyValuePairSet.Add(new KeyValuePair<uint, int>(niter.Current, edgeIdx));
        }
      }
      home.neighbours = keyValuePairSet;
    }

    public void Reset(List<Diagram.Site> sites = null)
    {
      this.visited = Node.VisitedType.NotVisited;
      if (sites == null)
        return;
      HashSet<Vector2> vector2Set = new HashSet<Vector2>();
      for (int index = 0; index < sites.Count; ++index)
      {
        if (vector2Set.Contains(sites[index].position))
        {
          this.visited = Node.VisitedType.Error;
          break;
        }
        vector2Set.Add(sites[index].position);
      }
    }

    public void SetTags(TagSet originalTags) => this.tags = new TagSet(originalTags);

    public void AddTag(Tag tag)
    {
      if (this.tags == null)
        this.tags = new TagSet();
      this.tags.Add(tag);
    }

    public void AddTagToNeighbors(Tag tag)
    {
      HashSet<KeyValuePair<uint, int>>.Enumerator enumerator = this.site.neighbours.GetEnumerator();
      while (enumerator.MoveNext())
        this.GetNeighbour(enumerator.Current.Key).AddTag(tag);
    }

    public virtual Tree Split(Node.SplitCommand cmd = null) => (Tree) null;

    public enum NodeType
    {
      Unknown,
      Internal,
      Leaf,
    }

    public enum VisitedType
    {
      MissingData = -2, // 0xFFFFFFFE
      Error = -1, // 0xFFFFFFFF
      NotVisited = 0,
      VisitedSuccess = 1,
    }

    public class SplitCommand
    {
      public Node.SplitCommand.SplitType splitType;
      public TagSet dontCopyTags;
      public TagSet moveTags;
      public int minChildCount = 2;
      public Node.SplitCommand.NodeTypeOverride typeOverride;
      public System.Action<Tree, Node.SplitCommand> SplitFunction;

      public enum SplitType
      {
        KeepParentAsCentroid = 1,
        ChildrenDuplicateParent = 2,
        ChildrenChosenFromLayer = 4,
      }

      public delegate string NodeTypeOverride(Vector2 position);
    }
  }
}
