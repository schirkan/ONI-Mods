// Decompiled with JetBrains decompiler
// Type: Satsuma.CheapestLinkTsp`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public sealed class CheapestLinkTsp<TNode> : ITsp<TNode>
  {
    private List<TNode> tour;

    public IList<TNode> Nodes { get; private set; }

    public Func<TNode, TNode, double> Cost { get; private set; }

    public IEnumerable<TNode> Tour => (IEnumerable<TNode>) this.tour;

    public double TourCost { get; private set; }

    public CheapestLinkTsp(IList<TNode> nodes, Func<TNode, TNode, double> cost)
    {
      this.Nodes = nodes;
      this.Cost = cost;
      this.tour = new List<TNode>();
      this.Run();
    }

    private void Run()
    {
      CompleteGraph graph = new CompleteGraph(this.Nodes.Count, Directedness.Undirected);
      Func<Arc, double> cost = (Func<Arc, double>) (arc => this.Cost(this.Nodes[graph.GetNodeIndex(graph.U(arc))], this.Nodes[graph.GetNodeIndex(graph.V(arc))]));
      Kruskal<double> kruskal = new Kruskal<double>((IGraph) graph, cost, (Func<Node, int>) (_ => 2));
      kruskal.Run();
      Dictionary<Node, Arc> dictionary1 = new Dictionary<Node, Arc>();
      Dictionary<Node, Arc> dictionary2 = new Dictionary<Node, Arc>();
      foreach (Arc arc in kruskal.Forest)
      {
        Node key1 = graph.U(arc);
        (dictionary1.ContainsKey(key1) ? dictionary2 : dictionary1)[key1] = arc;
        Node key2 = graph.V(arc);
        (dictionary1.ContainsKey(key2) ? dictionary2 : dictionary1)[key2] = arc;
      }
      foreach (Node node1 in graph.Nodes())
      {
        if (kruskal.Degree[node1] == 1)
        {
          Arc arc1 = Arc.Invalid;
          Node node2 = node1;
          while (true)
          {
            this.tour.Add(this.Nodes[graph.GetNodeIndex(node2)]);
            if (!(arc1 != Arc.Invalid) || kruskal.Degree[node2] != 1)
            {
              Arc arc2 = dictionary1[node2];
              arc1 = arc2 != arc1 ? arc2 : dictionary2[node2];
              node2 = graph.Other(arc1, node2);
            }
            else
              break;
          }
          this.tour.Add(this.Nodes[graph.GetNodeIndex(node1)]);
          break;
        }
      }
      this.TourCost = TspUtils.GetTourCost<TNode>((IEnumerable<TNode>) this.tour, this.Cost);
    }
  }
}
