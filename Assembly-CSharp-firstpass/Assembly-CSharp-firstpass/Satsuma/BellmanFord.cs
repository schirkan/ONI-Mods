// Decompiled with JetBrains decompiler
// Type: Satsuma.BellmanFord
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public sealed class BellmanFord
  {
    private const string NegativeCycleMessage = "A negative cycle was found.";
    private readonly Dictionary<Node, double> distance;
    private readonly Dictionary<Node, Arc> parentArc;

    public IGraph Graph { get; private set; }

    public Func<Arc, double> Cost { get; private set; }

    public IPath NegativeCycle { get; private set; }

    public BellmanFord(IGraph graph, Func<Arc, double> cost, IEnumerable<Node> sources)
    {
      this.Graph = graph;
      this.Cost = cost;
      this.distance = new Dictionary<Node, double>();
      this.parentArc = new Dictionary<Node, Arc>();
      foreach (Node source in sources)
      {
        this.distance[source] = 0.0;
        this.parentArc[source] = Arc.Invalid;
      }
      this.Run();
    }

    private void Run()
    {
      for (int index1 = this.Graph.NodeCount(); index1 > 0; --index1)
      {
        foreach (Arc arc1 in this.Graph.Arcs())
        {
          Node node1 = this.Graph.U(arc1);
          Node node2 = this.Graph.V(arc1);
          double d = this.GetDistance(node1);
          double num1 = this.GetDistance(node2);
          double num2 = this.Cost(arc1);
          if (this.Graph.IsEdge(arc1))
          {
            if (d > num1)
            {
              Node node3 = node1;
              node1 = node2;
              node2 = node3;
              double num3 = d;
              d = num1;
              num1 = num3;
            }
            if (!double.IsPositiveInfinity(d) && num2 < 0.0)
            {
              Path path = new Path(this.Graph);
              path.Begin(node1);
              path.AddLast(arc1);
              path.AddLast(arc1);
              this.NegativeCycle = (IPath) path;
              return;
            }
          }
          if (d + num2 < num1)
          {
            this.distance[node2] = d + num2;
            this.parentArc[node2] = arc1;
            if (index1 == 0)
            {
              Node node3 = node1;
              for (int index2 = this.Graph.NodeCount() - 1; index2 > 0; --index2)
                node3 = this.Graph.Other(this.parentArc[node3], node3);
              Path path = new Path(this.Graph);
              path.Begin(node3);
              Node node4 = node3;
              do
              {
                Arc arc2 = this.parentArc[node4];
                path.AddFirst(arc2);
                node4 = this.Graph.Other(arc2, node4);
              }
              while (!(node4 == node3));
              this.NegativeCycle = (IPath) path;
              return;
            }
          }
        }
      }
    }

    public bool Reached(Node node) => this.parentArc.ContainsKey(node);

    public IEnumerable<Node> ReachedNodes => (IEnumerable<Node>) this.parentArc.Keys;

    public double GetDistance(Node node)
    {
      if (this.NegativeCycle != null)
        throw new InvalidOperationException("A negative cycle was found.");
      double num;
      return !this.distance.TryGetValue(node, out num) ? double.PositiveInfinity : num;
    }

    public Arc GetParentArc(Node node)
    {
      if (this.NegativeCycle != null)
        throw new InvalidOperationException("A negative cycle was found.");
      Arc arc;
      return !this.parentArc.TryGetValue(node, out arc) ? Arc.Invalid : arc;
    }

    public IPath GetPath(Node node)
    {
      if (this.NegativeCycle != null)
        throw new InvalidOperationException("A negative cycle was found.");
      if (!this.Reached(node))
        return (IPath) null;
      Path path = new Path(this.Graph);
      path.Begin(node);
      while (true)
      {
        Arc parentArc = this.GetParentArc(node);
        if (!(parentArc == Arc.Invalid))
        {
          path.AddFirst(parentArc);
          node = this.Graph.Other(parentArc, node);
        }
        else
          break;
      }
      return (IPath) path;
    }
  }
}
