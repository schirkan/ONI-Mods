// Decompiled with JetBrains decompiler
// Type: Satsuma.Dfs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public abstract class Dfs
  {
    private HashSet<Node> traversed;
    private ArcFilter arcFilter;

    protected IGraph Graph { get; private set; }

    protected int Level { get; private set; }

    public void Run(IGraph graph, IEnumerable<Node> roots = null)
    {
      this.Graph = graph;
      Dfs.Direction direction;
      this.Start(out direction);
      switch (direction)
      {
        case Dfs.Direction.Undirected:
          this.arcFilter = ArcFilter.All;
          break;
        case Dfs.Direction.Forward:
          this.arcFilter = ArcFilter.Forward;
          break;
        default:
          this.arcFilter = ArcFilter.Backward;
          break;
      }
      this.traversed = new HashSet<Node>();
      foreach (Node node in roots ?? this.Graph.Nodes())
      {
        if (!this.traversed.Contains(node))
        {
          this.Level = 0;
          if (!this.Traverse(node, Arc.Invalid))
            break;
        }
      }
      this.traversed = (HashSet<Node>) null;
      this.StopSearch();
    }

    private bool Traverse(Node node, Arc arc)
    {
      this.traversed.Add(node);
      if (!this.NodeEnter(node, arc))
        return false;
      foreach (Arc arc1 in this.Graph.Arcs(node, this.arcFilter))
      {
        if (!(arc1 == arc))
        {
          Node node1 = this.Graph.Other(arc1, node);
          if (this.traversed.Contains(node1))
          {
            if (!this.BackArc(node, arc1))
              return false;
          }
          else
          {
            ++this.Level;
            if (!this.Traverse(node1, arc1))
              return false;
            --this.Level;
          }
        }
      }
      return this.NodeExit(node, arc);
    }

    protected abstract void Start(out Dfs.Direction direction);

    protected virtual bool NodeEnter(Node node, Arc arc) => true;

    protected virtual bool NodeExit(Node node, Arc arc) => true;

    protected virtual bool BackArc(Node node, Arc arc) => true;

    protected virtual void StopSearch()
    {
    }

    public enum Direction
    {
      Undirected,
      Forward,
      Backward,
    }
  }
}
