// Decompiled with JetBrains decompiler
// Type: ProcGen.Graph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using KSerialization;
using Satsuma;
using Satsuma.Drawing;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Graph
  {
    [Serialize]
    public List<Node> nodeList;
    [Serialize]
    public List<Arc> arcList;
    private SeededRandom myRandom;

    public List<Node> nodes => this.nodeList;

    public List<Arc> arcs => this.arcList;

    public CustomGraph baseGraph { get; private set; }

    public void SetSeed(int seed) => this.myRandom = new SeededRandom(seed);

    public Graph(int seed)
    {
      this.SetSeed(seed);
      this.nodeList = new List<Node>();
      this.arcList = new List<Arc>();
      this.baseGraph = new CustomGraph();
    }

    public Node AddNode(string type)
    {
      Node node = new Node(this.baseGraph.AddNode(), type);
      this.nodeList.Add(node);
      return node;
    }

    public void Remove(Node n)
    {
      this.baseGraph.DeleteNode(n.node);
      this.nodes.Remove(n);
    }

    public Arc AddArc(Node nodeA, Node nodeB, string type)
    {
      Arc arc = new Arc(this.baseGraph.AddArc(nodeA.node, nodeB.node, Directedness.Undirected), type);
      this.arcList.Add(arc);
      return arc;
    }

    public Node FindNodeByID(uint id) => this.nodeList.Find((Predicate<Node>) (node => node.node.Id == (long) id));

    public Arc FindArcByID(uint id) => this.arcList.Find((Predicate<Arc>) (arc => arc.arc.Id == (long) id));

    public Node FindNode(Predicate<Node> pred) => this.nodeList.Find(pred);

    public Arc FindArc(Predicate<Arc> pred) => this.arcList.Find(pred);

    public int GetDistanceToTagSetFromNode(Node node, TagSet tagset)
    {
      List<Node> withAtLeastOneTag = this.GetNodesWithAtLeastOneTag(tagset);
      if (withAtLeastOneTag.Count <= 0)
        return -1;
      Dijkstra dijkstra = new Dijkstra((IGraph) this.baseGraph, (Func<Satsuma.Arc, double>) (arc => 1.0), DijkstraMode.Sum);
      for (int index = 0; index < withAtLeastOneTag.Count; ++index)
        dijkstra.AddSource(withAtLeastOneTag[index].node);
      dijkstra.RunUntilFixed(node.node);
      return (int) dijkstra.GetDistance(node.node);
    }

    public int GetDistanceToTagFromNode(Node node, Tag tag)
    {
      List<Node> nodesWithTag = this.GetNodesWithTag(tag);
      if (nodesWithTag.Count <= 0)
        return -1;
      Dijkstra dijkstra = new Dijkstra((IGraph) this.baseGraph, (Func<Satsuma.Arc, double>) (arc => 1.0), DijkstraMode.Sum);
      for (int index = 0; index < nodesWithTag.Count; ++index)
        dijkstra.AddSource(nodesWithTag[index].node);
      dijkstra.RunUntilFixed(node.node);
      return (int) dijkstra.GetDistance(node.node);
    }

    public Dictionary<uint, int> GetDistanceToTag(Tag tag)
    {
      List<Node> nodesWithTag = this.GetNodesWithTag(tag);
      if (nodesWithTag.Count <= 0)
        return (Dictionary<uint, int>) null;
      Dijkstra dijkstra = new Dijkstra((IGraph) this.baseGraph, (Func<Satsuma.Arc, double>) (arc => 1.0), DijkstraMode.Sum);
      for (int index = 0; index < nodesWithTag.Count; ++index)
        dijkstra.AddSource(nodesWithTag[index].node);
      Dictionary<uint, int> dictionary = new Dictionary<uint, int>();
      for (int index = 0; index < this.nodes.Count; ++index)
      {
        dijkstra.RunUntilFixed(this.nodes[index].node);
        dictionary[(uint) this.nodes[index].node.Id] = (int) dijkstra.GetDistance(this.nodes[index].node);
      }
      return dictionary;
    }

    public List<Node> GetNodesWithAtLeastOneTag(TagSet tagset) => this.nodeList.FindAll((Predicate<Node>) (node => node.tags.ContainsOne(tagset)));

    public List<Node> GetNodesWithTag(Tag tag) => this.nodeList.FindAll((Predicate<Node>) (node => node.tags.Contains(tag)));

    public List<Arc> GetArcsWithTag(Tag tag) => this.arcList.FindAll((Predicate<Arc>) (arc => arc.tags.Contains(tag)));

    [OnDeserialized]
    internal void OnDeserializedMethod()
    {
      try
      {
        for (int index = 0; index < this.nodeList.Count; ++index)
        {
          Node node = new Node(this.baseGraph.AddNode(), this.nodeList[index].type);
          node.SetPosition(this.nodeList[index].position);
          this.nodeList[index] = node;
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Error deserialising " + ex.Message + "\n" + ex.StackTrace));
      }
    }

    public static PointD GetForceForBoundry(PointD particle, Polygon bounds)
    {
      Vector2 point = new Vector2((float) particle.X, (float) particle.Y);
      List<KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>>> edgesWithinDistance = bounds.GetEdgesWithinDistance(point);
      double x = 0.0;
      double y = 0.0;
      for (int index = 0; index < edgesWithinDistance.Count; ++index)
      {
        KeyValuePair<MathUtil.Pair<float, float>, MathUtil.Pair<Vector2, Vector2>> keyValuePair = edgesWithinDistance[index];
        MathUtil.Pair<Vector2, Vector2> pair = keyValuePair.Value;
        float second = keyValuePair.Key.Second;
        double first = (double) keyValuePair.Key.First;
        Vector2 vector2 = pair.First + (pair.Second - pair.First) * second;
        PointD pointD = new PointD((double) vector2.x, (double) vector2.y);
        double num = 1.0 / (first * first);
        x += (particle.X - pointD.X) / first * num;
        y += (particle.Y - pointD.Y) / first * num;
      }
      return bounds.Contains(point) ? new PointD(x, y) : new PointD(-x, -y);
    }

    public PointD GetPositionForNode(Satsuma.Node node)
    {
      Node node1 = this.nodeList.Find((Predicate<Node>) (n => n.node == node));
      return new PointD((double) node1.position.x, (double) node1.position.y);
    }

    public void SetInitialNodePositions(Polygon bounds)
    {
      List<Vector2> randomPoints = PointGenerator.GetRandomPoints(bounds, 50f, 0.0f, (List<Vector2>) null, PointGenerator.SampleBehaviour.PoissonDisk, true, this.myRandom);
      int num = 0;
      for (int index = 0; index < this.nodeList.Count; ++index)
      {
        if (num == randomPoints.Count - 1)
        {
          randomPoints = PointGenerator.GetRandomPoints(bounds, 10f, 20f, randomPoints, PointGenerator.SampleBehaviour.PoissonDisk, true, this.myRandom);
          num = 0;
        }
        this.nodeList[index].SetPosition(randomPoints[num++]);
      }
    }

    public bool Layout(Polygon bounds = null)
    {
      bool flag = false;
      int num1 = 0;
      Vector2 vector2 = new Vector2();
      for (; !flag && num1 < 100; ++num1)
      {
        flag = true;
        Func<Satsuma.Node, PointD> func = (Func<Satsuma.Node, PointD>) (n => this.GetPositionForNode(n));
        CustomGraph baseGraph = this.baseGraph;
        int num2 = num1;
        Func<Satsuma.Node, PointD> initialPositions = func;
        int seed = num2;
        ForceDirectedLayout forceDirectedLayout = new ForceDirectedLayout((IGraph) baseGraph, initialPositions, seed);
        forceDirectedLayout.ExternalForce = (Func<PointD, PointD>) (point => Graph.GetForceForBoundry(point, bounds));
        forceDirectedLayout.Run();
        IEnumerator<Satsuma.Node> enumerator = this.baseGraph.Nodes().GetEnumerator();
        int num3 = 0;
        while (enumerator.MoveNext())
        {
          Satsuma.Node node = enumerator.Current;
          Node node1 = this.nodeList.Find((Predicate<Node>) (n => n.node == node));
          if (node1 != null)
          {
            ref Vector2 local1 = ref vector2;
            PointD nodePosition = forceDirectedLayout.NodePositions[node];
            double x = nodePosition.X;
            local1.x = (float) x;
            ref Vector2 local2 = ref vector2;
            nodePosition = forceDirectedLayout.NodePositions[node];
            double y = nodePosition.Y;
            local2.y = (float) y;
            if (!bounds.Contains(vector2))
            {
              flag = false;
              Debug.LogWarning((object) "Re-doing layout - cell was off map");
              break;
            }
            node1.SetPosition(vector2);
          }
          if (flag)
            ++num3;
          else
            break;
        }
      }
      if (num1 >= 10)
        Debug.LogWarning((object) ("Re-ran layout " + (object) num1 + " times"));
      return flag;
    }
  }
}
