// Decompiled with JetBrains decompiler
// Type: Satsuma.IntegerPreflow
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class IntegerPreflow : IFlow<long>
  {
    private readonly Dictionary<Arc, long> flow;
    private readonly Dictionary<Node, long> excess;
    private readonly Dictionary<Node, long> label;
    private readonly PriorityQueue<Node, long> active;

    public IGraph Graph { get; private set; }

    public Func<Arc, long> Capacity { get; private set; }

    public Node Source { get; private set; }

    public Node Target { get; private set; }

    public long FlowSize { get; private set; }

    public IntegerPreflow(IGraph graph, Func<Arc, long> capacity, Node source, Node target)
    {
      this.Graph = graph;
      this.Capacity = capacity;
      this.Source = source;
      this.Target = target;
      this.flow = new Dictionary<Arc, long>();
      this.excess = new Dictionary<Node, long>();
      this.label = new Dictionary<Node, long>();
      this.active = new PriorityQueue<Node, long>();
      this.Run();
      this.excess = (Dictionary<Node, long>) null;
      this.label = (Dictionary<Node, long>) null;
      this.active = (PriorityQueue<Node, long>) null;
    }

    private void Run()
    {
      foreach (Node node in this.Graph.Nodes())
      {
        this.label[node] = node == this.Source ? (long) -this.Graph.NodeCount() : 0L;
        this.excess[node] = 0L;
      }
      long num1 = 0;
      foreach (Arc arc in this.Graph.Arcs(this.Source, ArcFilter.Forward))
      {
        Node node = this.Graph.Other(arc, this.Source);
        if (!(node == this.Source))
        {
          long num2 = this.Graph.U(arc) == this.Source ? this.Capacity(arc) : -this.Capacity(arc);
          if (num2 != 0L)
          {
            this.flow[arc] = num2;
            long num3 = Math.Abs(num2);
            checked { num1 += num3; }
            this.excess[node] += num3;
            if (node != this.Target)
              this.active[node] = 0L;
          }
        }
      }
      this.excess[this.Source] = -num1;
      while (this.active.Count > 0)
      {
        long priority;
        Node node1 = this.active.Peek(out priority);
        this.active.Pop();
        long num2 = this.excess[node1];
        long val1 = long.MinValue;
        foreach (Arc arc in this.Graph.Arcs(node1))
        {
          Node node2 = this.Graph.U(arc);
          Node node3 = this.Graph.V(arc);
          if (!(node2 == node3))
          {
            Node key = node1 == node2 ? node3 : node2;
            int num3 = this.Graph.IsEdge(arc) ? 1 : 0;
            long num4;
            this.flow.TryGetValue(arc, out num4);
            long num5 = this.Capacity(arc);
            long num6 = num3 != 0 ? -this.Capacity(arc) : 0L;
            if (node2 == node1)
            {
              if (num4 != num5)
              {
                long num7 = this.label[key];
                if (num7 <= priority)
                {
                  val1 = Math.Max(val1, num7 - 1L);
                }
                else
                {
                  long num8 = (long) Math.Min((ulong) num2, (ulong) (num5 - num4));
                  this.flow[arc] = num4 + num8;
                  this.excess[node3] += num8;
                  if (node3 != this.Source && node3 != this.Target)
                    this.active[node3] = this.label[node3];
                  num2 -= num8;
                  if (num2 == 0L)
                    break;
                }
              }
            }
            else if (num4 != num6)
            {
              long num7 = this.label[key];
              if (num7 <= priority)
              {
                val1 = Math.Max(val1, num7 - 1L);
              }
              else
              {
                long num8 = (long) Math.Min((ulong) num2, (ulong) (num4 - num6));
                this.flow[arc] = num4 - num8;
                this.excess[node2] += num8;
                if (node2 != this.Source && node2 != this.Target)
                  this.active[node2] = this.label[node2];
                num2 -= num8;
                if (num2 == 0L)
                  break;
              }
            }
          }
        }
        this.excess[node1] = num2;
        if (num2 > 0L)
        {
          if (val1 == long.MinValue)
            throw new InvalidOperationException("Internal error.");
          PriorityQueue<Node, long> active = this.active;
          Node element = node1;
          Dictionary<Node, long> label = this.label;
          Node key = node1;
          long num3;
          priority = num3 = val1;
          long num4 = num3;
          label[key] = num3;
          long num5 = num4;
          active[element] = num5;
        }
      }
      this.FlowSize = 0L;
      foreach (Arc arc in this.Graph.Arcs(this.Source))
      {
        Node node1 = this.Graph.U(arc);
        Node node2 = this.Graph.V(arc);
        long num2;
        if (!(node1 == node2) && this.flow.TryGetValue(arc, out num2))
        {
          if (node1 == this.Source)
            this.FlowSize += num2;
          else
            this.FlowSize -= num2;
        }
      }
    }

    public IEnumerable<KeyValuePair<Arc, long>> NonzeroArcs => this.flow.Where<KeyValuePair<Arc, long>>((Func<KeyValuePair<Arc, long>, bool>) (kv => (ulong) kv.Value > 0UL));

    public long Flow(Arc arc)
    {
      long num;
      this.flow.TryGetValue(arc, out num);
      return num;
    }
  }
}
