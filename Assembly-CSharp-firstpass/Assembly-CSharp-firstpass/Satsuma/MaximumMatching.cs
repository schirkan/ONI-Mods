// Decompiled with JetBrains decompiler
// Type: Satsuma.MaximumMatching
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Satsuma
{
  public sealed class MaximumMatching : IClearable
  {
    private readonly Satsuma.Matching matching;
    private readonly HashSet<Node> unmatchedRedNodes;
    private Dictionary<Node, Arc> parentArc;

    public IGraph Graph { get; private set; }

    public Func<Node, bool> IsRed { get; private set; }

    public IMatching Matching => (IMatching) this.matching;

    public MaximumMatching(IGraph graph, Func<Node, bool> isRed)
    {
      this.Graph = graph;
      this.IsRed = isRed;
      this.matching = new Satsuma.Matching(this.Graph);
      this.unmatchedRedNodes = new HashSet<Node>();
      this.Clear();
    }

    public void Clear()
    {
      this.matching.Clear();
      this.unmatchedRedNodes.Clear();
      foreach (Node node in this.Graph.Nodes())
      {
        if (this.IsRed(node))
          this.unmatchedRedNodes.Add(node);
      }
    }

    public int GreedyGrow(int maxImprovements = 2147483647)
    {
      int num = 0;
      List<Node> nodeList = new List<Node>();
      foreach (Node unmatchedRedNode in this.unmatchedRedNodes)
      {
        foreach (Arc arc in this.Graph.Arcs(unmatchedRedNode))
        {
          if (!this.matching.HasNode(this.Graph.Other(arc, unmatchedRedNode)))
          {
            this.matching.Enable(arc, true);
            nodeList.Add(unmatchedRedNode);
            ++num;
            if (num < maxImprovements)
              break;
            goto label_12;
          }
        }
      }
label_12:
      foreach (Node node in nodeList)
        this.unmatchedRedNodes.Remove(node);
      return num;
    }

    public void Add(Arc arc)
    {
      if (this.matching.HasArc(arc))
        return;
      this.matching.Enable(arc, true);
      Node node = this.Graph.U(arc);
      this.unmatchedRedNodes.Remove(this.IsRed(node) ? node : this.Graph.V(arc));
    }

    private Node Traverse(Node node)
    {
      Arc arc1 = this.matching.MatchedArc(node);
      if (this.IsRed(node))
      {
        foreach (Arc arc2 in this.Graph.Arcs(node))
        {
          if (arc2 != arc1)
          {
            Node node1 = this.Graph.Other(arc2, node);
            if (!this.parentArc.ContainsKey(node1))
            {
              this.parentArc[node1] = arc2;
              if (!this.matching.HasNode(node1))
                return node1;
              Node node2 = this.Traverse(node1);
              if (node2 != Node.Invalid)
                return node2;
            }
          }
        }
      }
      else
      {
        Node node1 = this.Graph.Other(arc1, node);
        if (!this.parentArc.ContainsKey(node1))
        {
          this.parentArc[node1] = arc1;
          Node node2 = this.Traverse(node1);
          if (node2 != Node.Invalid)
            return node2;
        }
      }
      return Node.Invalid;
    }

    public void Run()
    {
      List<Node> nodeList = new List<Node>();
      this.parentArc = new Dictionary<Node, Arc>();
      foreach (Node unmatchedRedNode in this.unmatchedRedNodes)
      {
        this.parentArc.Clear();
        this.parentArc[unmatchedRedNode] = Arc.Invalid;
        Node node1 = this.Traverse(unmatchedRedNode);
        if (!(node1 == Node.Invalid))
        {
          while (true)
          {
            Arc arc1 = this.parentArc[node1];
            Node node2 = this.Graph.Other(arc1, node1);
            Arc arc2 = node2 == unmatchedRedNode ? Arc.Invalid : this.parentArc[node2];
            if (arc2 != Arc.Invalid)
              this.matching.Enable(arc2, false);
            this.matching.Enable(arc1, true);
            if (!(arc2 == Arc.Invalid))
              node1 = this.Graph.Other(arc2, node2);
            else
              break;
          }
          nodeList.Add(unmatchedRedNode);
        }
      }
      this.parentArc = (Dictionary<Node, Arc>) null;
      foreach (Node node in nodeList)
        this.unmatchedRedNodes.Remove(node);
    }
  }
}
