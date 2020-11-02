// Decompiled with JetBrains decompiler
// Type: Satsuma.TopologicalOrder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class TopologicalOrder
  {
    public IGraph Graph { get; private set; }

    public bool Acyclic { get; private set; }

    public List<Node> Order { get; private set; }

    public TopologicalOrder(IGraph graph, TopologicalOrder.Flags flags = TopologicalOrder.Flags.None)
    {
      this.Graph = graph;
      if ((flags & TopologicalOrder.Flags.CreateOrder) != TopologicalOrder.Flags.None)
        this.Order = new List<Node>();
      new TopologicalOrder.MyDfs() { Parent = this }.Run(graph);
    }

    [System.Flags]
    public enum Flags
    {
      None = 0,
      CreateOrder = 1,
    }

    private class MyDfs : Dfs
    {
      public TopologicalOrder Parent;
      private HashSet<Node> exited;

      protected override void Start(out Dfs.Direction direction)
      {
        direction = Dfs.Direction.Forward;
        this.Parent.Acyclic = true;
        this.exited = new HashSet<Node>();
      }

      protected override bool NodeEnter(Node node, Arc arc)
      {
        if (!(arc != Arc.Invalid) || !this.Graph.IsEdge(arc))
          return true;
        this.Parent.Acyclic = false;
        return false;
      }

      protected override bool NodeExit(Node node, Arc arc)
      {
        if (this.Parent.Order != null)
          this.Parent.Order.Add(node);
        this.exited.Add(node);
        return true;
      }

      protected override bool BackArc(Node node, Arc arc)
      {
        if (this.exited.Contains(this.Graph.Other(arc, node)))
          return true;
        this.Parent.Acyclic = false;
        return false;
      }

      protected override void StopSearch()
      {
        if (this.Parent.Order == null)
          return;
        if (this.Parent.Acyclic)
          this.Parent.Order.Reverse();
        else
          this.Parent.Order.Clear();
      }
    }
  }
}
