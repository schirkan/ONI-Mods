// Decompiled with JetBrains decompiler
// Type: Satsuma.Bfs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class Bfs
  {
    private readonly Dictionary<Node, Arc> parentArc;
    private readonly Dictionary<Node, int> level;
    private readonly Queue<Node> queue;

    public IGraph Graph { get; private set; }

    public Bfs(IGraph graph)
    {
      this.Graph = graph;
      this.parentArc = new Dictionary<Node, Arc>();
      this.level = new Dictionary<Node, int>();
      this.queue = new Queue<Node>();
    }

    public void AddSource(Node node)
    {
      if (this.Reached(node))
        return;
      this.parentArc[node] = Arc.Invalid;
      this.level[node] = 0;
      this.queue.Enqueue(node);
    }

    public bool Step(Func<Node, bool> isTarget, out Node reachedTargetNode)
    {
      reachedTargetNode = Node.Invalid;
      if (this.queue.Count == 0)
        return false;
      Node node = this.queue.Dequeue();
      int num = this.level[node] + 1;
      foreach (Arc arc in this.Graph.Arcs(node, ArcFilter.Forward))
      {
        Node key = this.Graph.Other(arc, node);
        if (!this.parentArc.ContainsKey(key))
        {
          this.queue.Enqueue(key);
          this.level[key] = num;
          this.parentArc[key] = arc;
          if (isTarget != null && isTarget(key))
          {
            reachedTargetNode = key;
            return false;
          }
        }
      }
      return true;
    }

    public void Run()
    {
      do
        ;
      while (this.Step((Func<Node, bool>) null, out Node _));
    }

    public Node RunUntilReached(Node target)
    {
      if (this.Reached(target))
        return target;
      Node reachedTargetNode;
      do
        ;
      while (this.Step((Func<Node, bool>) (node => node == target), out reachedTargetNode));
      return reachedTargetNode;
    }

    public Node RunUntilReached(Func<Node, bool> isTarget)
    {
      Node reachedTargetNode = this.ReachedNodes.FirstOrDefault<Node>(isTarget);
      if (reachedTargetNode != Node.Invalid)
        return reachedTargetNode;
      do
        ;
      while (this.Step(isTarget, out reachedTargetNode));
      return reachedTargetNode;
    }

    public bool Reached(Node x) => this.parentArc.ContainsKey(x);

    public IEnumerable<Node> ReachedNodes => (IEnumerable<Node>) this.parentArc.Keys;

    public int GetLevel(Node node)
    {
      int num;
      return !this.level.TryGetValue(node, out num) ? -1 : num;
    }

    public Arc GetParentArc(Node node)
    {
      Arc arc;
      return !this.parentArc.TryGetValue(node, out arc) ? Arc.Invalid : arc;
    }

    public IPath GetPath(Node node)
    {
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
