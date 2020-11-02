// Decompiled with JetBrains decompiler
// Type: Satsuma.Prim`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Prim<TCost> where TCost : IComparable<TCost>
  {
    public IGraph Graph { get; private set; }

    public Func<Arc, TCost> Cost { get; private set; }

    public HashSet<Arc> Forest { get; private set; }

    public Prim(IGraph graph, Func<Arc, TCost> cost)
    {
      this.Graph = graph;
      this.Cost = cost;
      this.Forest = new HashSet<Arc>();
      this.Run();
    }

    private void Run()
    {
      this.Forest.Clear();
      PriorityQueue<Node, TCost> priorityQueue = new PriorityQueue<Node, TCost>();
      HashSet<Node> nodeSet = new HashSet<Node>();
      Dictionary<Node, Arc> dictionary = new Dictionary<Node, Arc>();
      foreach (IEnumerable<Node> component in new ConnectedComponents(this.Graph, ConnectedComponents.Flags.CreateComponents).Components)
      {
        Node node1 = component.First<Node>();
        nodeSet.Add(node1);
        foreach (Arc arc in this.Graph.Arcs(node1))
        {
          Node node2 = this.Graph.Other(arc, node1);
          dictionary[node2] = arc;
          priorityQueue[node2] = this.Cost(arc);
        }
      }
      while (priorityQueue.Count != 0)
      {
        Node node1 = priorityQueue.Peek();
        priorityQueue.Pop();
        nodeSet.Add(node1);
        this.Forest.Add(dictionary[node1]);
        foreach (Arc arc in this.Graph.Arcs(node1))
        {
          Node node2 = this.Graph.Other(arc, node1);
          if (!nodeSet.Contains(node2))
          {
            TCost cost = this.Cost(arc);
            if (cost.CompareTo(priorityQueue[node2]) < 0)
            {
              priorityQueue[node2] = cost;
              dictionary[node2] = arc;
            }
          }
        }
      }
    }
  }
}
