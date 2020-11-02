// Decompiled with JetBrains decompiler
// Type: Satsuma.CompleteGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public sealed class CompleteGraph : IGraph, IArcLookup
  {
    private readonly int nodeCount;

    public bool Directed { get; private set; }

    public CompleteGraph(int nodeCount, Directedness directedness)
    {
      this.nodeCount = nodeCount;
      this.Directed = directedness == Directedness.Directed;
      if (nodeCount < 0)
        throw new ArgumentException("Invalid node count: " + (object) nodeCount);
      long num = (long) nodeCount * (long) (nodeCount - 1);
      if (!this.Directed)
        num /= 2L;
      if (num > (long) int.MaxValue)
        throw new ArgumentException("Too many nodes: " + (object) nodeCount);
    }

    public Node GetNode(int index) => new Node(1L + (long) index);

    public int GetNodeIndex(Node node) => (int) (node.Id - 1L);

    public Arc GetArc(Node u, Node v)
    {
      int x = this.GetNodeIndex(u);
      int y = this.GetNodeIndex(v);
      if (x == y)
        return Arc.Invalid;
      if (!this.Directed && x > y)
      {
        int num = x;
        x = y;
        y = num;
      }
      return this.GetArcInternal(x, y);
    }

    private Arc GetArcInternal(int x, int y) => new Arc(1L + (long) y * (long) this.nodeCount + (long) x);

    public Node U(Arc arc) => new Node(1L + (arc.Id - 1L) % (long) this.nodeCount);

    public Node V(Arc arc) => new Node(1L + (arc.Id - 1L) / (long) this.nodeCount);

    public bool IsEdge(Arc arc) => !this.Directed;

    public IEnumerable<Node> Nodes()
    {
      for (int i = 0; i < this.nodeCount; ++i)
        yield return this.GetNode(i);
    }

    public IEnumerable<Arc> Arcs(ArcFilter filter = ArcFilter.All)
    {
      int i;
      int j;
      if (this.Directed)
      {
        for (i = 0; i < this.nodeCount; ++i)
        {
          for (j = 0; j < this.nodeCount; ++j)
          {
            if (i != j)
              yield return this.GetArcInternal(i, j);
          }
        }
      }
      else
      {
        for (i = 0; i < this.nodeCount; ++i)
        {
          for (j = i + 1; j < this.nodeCount; ++j)
            yield return this.GetArcInternal(i, j);
        }
      }
    }

    public IEnumerable<Arc> Arcs(Node u, ArcFilter filter = ArcFilter.All)
    {
      if (this.Directed)
      {
        switch (filter)
        {
          case ArcFilter.Edge:
            yield break;
          case ArcFilter.Forward:
            break;
          default:
            foreach (Node node in this.Nodes())
            {
              if (node != u)
                yield return this.GetArc(node, u);
            }
            break;
        }
      }
      if (!this.Directed || filter != ArcFilter.Backward)
      {
        foreach (Node node in this.Nodes())
        {
          if (node != u)
            yield return this.GetArc(u, node);
        }
      }
    }

    public IEnumerable<Arc> Arcs(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      if (this.Directed)
      {
        switch (filter)
        {
          case ArcFilter.Edge:
            yield break;
          case ArcFilter.Forward:
            break;
          default:
            yield return this.GetArc(v, u);
            break;
        }
      }
      if (!this.Directed || filter != ArcFilter.Backward)
        yield return this.GetArc(u, v);
    }

    public int NodeCount() => this.nodeCount;

    public int ArcCount(ArcFilter filter = ArcFilter.All)
    {
      int num = this.nodeCount * (this.nodeCount - 1);
      if (!this.Directed)
        num /= 2;
      return num;
    }

    public int ArcCount(Node u, ArcFilter filter = ArcFilter.All)
    {
      if (!this.Directed)
        return this.nodeCount - 1;
      if (filter == ArcFilter.All)
        return 2 * (this.nodeCount - 1);
      return filter == ArcFilter.Edge ? 0 : this.nodeCount - 1;
    }

    public int ArcCount(Node u, Node v, ArcFilter filter = ArcFilter.All)
    {
      if (!this.Directed)
        return 1;
      if (filter == ArcFilter.All)
        return 2;
      return filter == ArcFilter.Edge ? 0 : 1;
    }

    public bool HasNode(Node node) => node.Id >= 1L && node.Id <= (long) this.nodeCount;

    public bool HasArc(Arc arc)
    {
      Node node1 = this.V(arc);
      if (!this.HasNode(node1))
        return false;
      Node node2 = this.U(arc);
      return this.Directed || node2.Id < node1.Id;
    }
  }
}
