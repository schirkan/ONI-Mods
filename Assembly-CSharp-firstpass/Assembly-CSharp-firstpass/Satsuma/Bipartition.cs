// Decompiled with JetBrains decompiler
// Type: Satsuma.Bipartition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class Bipartition
  {
    public IGraph Graph { get; private set; }

    public bool Bipartite { get; private set; }

    public HashSet<Node> RedNodes { get; private set; }

    public HashSet<Node> BlueNodes { get; private set; }

    public Bipartition(IGraph graph, Bipartition.Flags flags = Bipartition.Flags.None)
    {
      this.Graph = graph;
      if ((flags & Bipartition.Flags.CreateRedNodes) != Bipartition.Flags.None)
        this.RedNodes = new HashSet<Node>();
      if ((flags & Bipartition.Flags.CreateBlueNodes) != Bipartition.Flags.None)
        this.BlueNodes = new HashSet<Node>();
      new Bipartition.MyDfs() { Parent = this }.Run(graph);
    }

    [System.Flags]
    public enum Flags
    {
      None = 0,
      CreateRedNodes = 1,
      CreateBlueNodes = 2,
    }

    private class MyDfs : Dfs
    {
      public Bipartition Parent;
      private HashSet<Node> redNodes;

      protected override void Start(out Dfs.Direction direction)
      {
        direction = Dfs.Direction.Undirected;
        this.Parent.Bipartite = true;
        this.redNodes = this.Parent.RedNodes ?? new HashSet<Node>();
      }

      protected override bool NodeEnter(Node node, Arc arc)
      {
        if ((this.Level & 1) == 0)
          this.redNodes.Add(node);
        else if (this.Parent.BlueNodes != null)
          this.Parent.BlueNodes.Add(node);
        return true;
      }

      protected override bool BackArc(Node node, Arc arc)
      {
        Node node1 = this.Graph.Other(arc, node);
        if (this.redNodes.Contains(node) != this.redNodes.Contains(node1))
          return true;
        this.Parent.Bipartite = false;
        if (this.Parent.RedNodes != null)
          this.Parent.RedNodes.Clear();
        if (this.Parent.BlueNodes != null)
          this.Parent.BlueNodes.Clear();
        return false;
      }
    }
  }
}
