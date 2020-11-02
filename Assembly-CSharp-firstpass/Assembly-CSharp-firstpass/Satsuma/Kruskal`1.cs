// Decompiled with JetBrains decompiler
// Type: Satsuma.Kruskal`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Kruskal<TCost> where TCost : IComparable<TCost>
  {
    private IEnumerator<Arc> arcEnumerator;
    private int arcsToGo;
    private DisjointSet<Node> components;

    public IGraph Graph { get; private set; }

    public Func<Arc, TCost> Cost { get; private set; }

    public Func<Node, int> MaxDegree { get; private set; }

    public HashSet<Arc> Forest { get; private set; }

    public Dictionary<Node, int> Degree { get; private set; }

    public Kruskal(IGraph graph, Func<Arc, TCost> cost, Func<Node, int> maxDegree = null)
    {
      this.Graph = graph;
      this.Cost = cost;
      this.MaxDegree = maxDegree;
      this.Forest = new HashSet<Arc>();
      this.Degree = new Dictionary<Node, int>();
      foreach (Node node in this.Graph.Nodes())
        this.Degree[node] = 0;
      List<Arc> list = this.Graph.Arcs().ToList<Arc>();
      list.Sort((Comparison<Arc>) ((a, b) => this.Cost(a).CompareTo(this.Cost(b))));
      this.arcEnumerator = (IEnumerator<Arc>) list.GetEnumerator();
      this.arcsToGo = this.Graph.NodeCount() - new ConnectedComponents(this.Graph).Count;
      this.components = new DisjointSet<Node>();
    }

    public bool Step()
    {
      if (this.arcsToGo <= 0 || this.arcEnumerator == null || !this.arcEnumerator.MoveNext())
      {
        this.arcEnumerator = (IEnumerator<Arc>) null;
        return false;
      }
      this.AddArc(this.arcEnumerator.Current);
      return true;
    }

    public void Run()
    {
      do
        ;
      while (this.Step());
    }

    public bool AddArc(Arc arc)
    {
      Node node1 = this.Graph.U(arc);
      if (this.MaxDegree != null && this.Degree[node1] >= this.MaxDegree(node1))
        return false;
      DisjointSetSet<Node> a = this.components.WhereIs(node1);
      Node node2 = this.Graph.V(arc);
      if (this.MaxDegree != null && this.Degree[node2] >= this.MaxDegree(node2))
        return false;
      DisjointSetSet<Node> b = this.components.WhereIs(node2);
      if (a == b)
        return false;
      this.Forest.Add(arc);
      this.components.Union(a, b);
      this.Degree[node1]++;
      this.Degree[node2]++;
      --this.arcsToGo;
      return true;
    }
  }
}
