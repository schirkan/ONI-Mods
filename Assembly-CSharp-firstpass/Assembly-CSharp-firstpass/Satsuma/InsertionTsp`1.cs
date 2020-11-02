// Decompiled with JetBrains decompiler
// Type: Satsuma.InsertionTsp`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Satsuma
{
  public sealed class InsertionTsp<TNode> : ITsp<TNode>
  {
    private LinkedList<TNode> tour;
    private Dictionary<TNode, LinkedListNode<TNode>> tourNodes;
    private HashSet<TNode> insertableNodes;
    private PriorityQueue<TNode, double> insertableNodeQueue;

    public IEnumerable<TNode> Nodes { get; private set; }

    public Func<TNode, TNode, double> Cost { get; private set; }

    public TspSelectionRule SelectionRule { get; private set; }

    public IEnumerable<TNode> Tour => (IEnumerable<TNode>) this.tour;

    public double TourCost { get; private set; }

    public InsertionTsp(
      IEnumerable<TNode> nodes,
      Func<TNode, TNode, double> cost,
      TspSelectionRule selectionRule = TspSelectionRule.Farthest)
    {
      this.Nodes = nodes;
      this.Cost = cost;
      this.SelectionRule = selectionRule;
      this.tour = new LinkedList<TNode>();
      this.tourNodes = new Dictionary<TNode, LinkedListNode<TNode>>();
      this.insertableNodes = new HashSet<TNode>();
      this.insertableNodeQueue = new PriorityQueue<TNode, double>();
      this.Clear();
    }

    private double PriorityFromCost(double c) => this.SelectionRule == TspSelectionRule.Farthest ? -c : c;

    public void Clear()
    {
      this.tour.Clear();
      this.TourCost = 0.0;
      this.tourNodes.Clear();
      this.insertableNodes.Clear();
      this.insertableNodeQueue.Clear();
      if (!this.Nodes.Any<TNode>())
        return;
      TNode key = this.Nodes.First<TNode>();
      this.tour.AddFirst(key);
      this.tourNodes[key] = this.tour.AddFirst(key);
      foreach (TNode node in this.Nodes)
      {
        if (!node.Equals((object) key))
        {
          this.insertableNodes.Add(node);
          this.insertableNodeQueue[node] = this.PriorityFromCost(this.Cost(key, node));
        }
      }
    }

    public bool Insert(TNode node)
    {
      if (!this.insertableNodes.Contains(node))
        return false;
      this.insertableNodes.Remove(node);
      this.insertableNodeQueue.Remove(node);
      LinkedListNode<TNode> node1 = (LinkedListNode<TNode>) null;
      double num1 = double.PositiveInfinity;
      for (LinkedListNode<TNode> linkedListNode = this.tour.First; linkedListNode != this.tour.Last; linkedListNode = linkedListNode.Next)
      {
        LinkedListNode<TNode> next = linkedListNode.Next;
        double num2 = this.Cost(linkedListNode.Value, node) + this.Cost(node, next.Value);
        if (linkedListNode != next)
          num2 -= this.Cost(linkedListNode.Value, next.Value);
        if (num2 < num1)
        {
          num1 = num2;
          node1 = linkedListNode;
        }
      }
      this.tourNodes[node] = this.tour.AddAfter(node1, node);
      this.TourCost += num1;
      foreach (TNode insertableNode in this.insertableNodes)
      {
        double num2 = this.PriorityFromCost(this.Cost(node, insertableNode));
        if (num2 < this.insertableNodeQueue[insertableNode])
          this.insertableNodeQueue[insertableNode] = num2;
      }
      return true;
    }

    public bool Insert()
    {
      if (this.insertableNodes.Count == 0)
        return false;
      this.Insert(this.insertableNodeQueue.Peek());
      return true;
    }

    public void Run()
    {
      do
        ;
      while (this.Insert());
    }
  }
}
