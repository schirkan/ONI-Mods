// Decompiled with JetBrains decompiler
// Type: VoronoiTree.Tree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Geo;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoronoiTree
{
  public class Tree : Node
  {
    protected List<Node> children;
    public bool dontRelaxChildren;

    public SeededRandom myRandom { get; private set; }

    public Tree()
      : base(Node.NodeType.Internal)
    {
      this.children = new List<Node>();
      this.SetSeed(0);
    }

    public Tree(int seed = 0)
      : base(Node.NodeType.Internal)
    {
      this.children = new List<Node>();
      this.SetSeed(seed);
    }

    public Tree(Diagram.Site site, Tree parent, int seed = 0)
      : base(site, Node.NodeType.Internal, parent)
    {
      this.children = new List<Node>();
      this.SetSeed(seed);
    }

    public Tree(Diagram.Site site, List<Node> children, Tree parent, int seed = 0)
      : base(site, Node.NodeType.Internal, parent)
    {
      if (children == null)
        children = new List<Node>();
      this.children = children;
      this.SetSeed(seed);
    }

    public void SetSeed(int seed) => this.myRandom = new SeededRandom(seed);

    public Node GetChildByID(uint id) => this.children.Find((Predicate<Node>) (s => (int) s.site.id == (int) id));

    public int ChildCount() => this.children == null ? 0 : this.children.Count;

    public Tree GetChildContainingLeaf(Leaf leaf)
    {
      Vector2 point = leaf.site.poly.Centroid();
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] is Tree child && child.site.poly.Contains(point))
          return child;
      }
      return (Tree) null;
    }

    public Node GetChild(int childIndex) => childIndex < this.children.Count ? this.children[childIndex] : (Node) null;

    public void AddChild(Node child)
    {
      if (child.site.id > Node.maxIndex)
        Node.maxIndex = child.site.id;
      this.children.Add(child);
      child.SetParent(this);
    }

    public Node AddSite(Diagram.Site site, Node.NodeType type)
    {
      Node child = type != Node.NodeType.Internal ? (Node) new Leaf(site, this) : (Node) new Tree(site, this, this.myRandom.seed + this.ChildCount());
      this.AddChild(child);
      return child;
    }

    public bool ComputeChildrenRecursive(int depth, bool pd = false)
    {
      if (depth > Node.maxDepth || this.site.poly == null || this.children == null)
        return false;
      List<Diagram.Site> siteList = new List<Diagram.Site>();
      for (int index = 0; index < this.children.Count; ++index)
        siteList.Add(this.children[index].site);
      this.PlaceSites(siteList, depth);
      if (pd)
      {
        for (int index = 0; index < siteList.Count; ++index)
        {
          if (!this.site.poly.Contains(siteList[index].position))
            Debug.LogErrorFormat("Cant feed points [{0}] to powerdiagram that are outside its area [{1}] ", (object) siteList[index].id, (object) siteList[index].position);
        }
        if (this.ComputeNodePD(siteList))
        {
          for (int index = 0; index < this.children.Count; ++index)
          {
            if (this.children[index].type == Node.NodeType.Internal && !(this.children[index] as Tree).ComputeChildrenRecursive(depth + 1, pd))
              return false;
          }
        }
      }
      else if (this.ComputeNode(siteList))
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal && !(this.children[index] as Tree).ComputeChildrenRecursive(depth + 1))
            return false;
        }
      }
      return true;
    }

    public bool ComputeChildren(int seed, bool place = false, bool pd = false)
    {
      if (this.site.poly == null || this.children == null)
        return false;
      List<Diagram.Site> siteList = new List<Diagram.Site>();
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (place || !this.site.poly.Contains(this.children[index].site.position))
          Debug.LogErrorFormat("Cant feed points [{0}] to powerdiagram that are outside its area [{1}] ", (object) this.children[index].site.id, (object) this.children[index].site.position);
        siteList.Add(this.children[index].site);
      }
      if (place)
        this.PlaceSites(siteList, seed);
      if (pd)
        this.ComputeNodePD(siteList);
      else
        this.ComputeNode(siteList);
      return true;
    }

    public int Count()
    {
      if (this.children == null || this.children.Count == 0)
        return 0;
      int count = this.children.Count;
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].type == Node.NodeType.Internal)
        {
          Tree child = this.children[index] as Tree;
          count += child.Count();
        }
      }
      return count;
    }

    public void Reset()
    {
      if (this.children != null)
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal)
            (this.children[index] as Tree).Reset();
        }
      }
      this.Reset((List<Diagram.Site>) null);
    }

    public int MaxDepth(int depth = 0)
    {
      if (this.children == null || this.children.Count == 0)
        return depth;
      int num1 = depth + 1;
      int depth1 = num1;
      for (int index = 0; index < this.children.Count; ++index)
      {
        int num2 = depth1 + 1;
        if (this.children[index].type == Node.NodeType.Internal)
          num2 = (this.children[index] as Tree).MaxDepth(depth1);
        if (num2 > num1)
          num1 = num2;
      }
      return num1;
    }

    public void RelaxRecursive(int depth, int iterations = -1, float minEnergy = 1f, bool pd = false)
    {
      if (this.dontRelaxChildren || this.site.poly == null || (this.children == null || this.children.Count == 0))
      {
        this.visited = Node.VisitedType.MissingData;
      }
      else
      {
        List<Diagram.Site> siteList = new List<Diagram.Site>();
        for (int index = 0; index < this.children.Count; ++index)
          siteList.Add(this.children[index].site);
        float num1 = float.MaxValue;
        for (int index1 = 0; index1 < iterations && (double) num1 > (double) minEnergy; ++index1)
        {
          float num2 = 0.0f;
          for (int index2 = 0; index2 < this.children.Count; ++index2)
          {
            num2 += Vector2.Distance(this.children[index2].site.position, siteList[index2].poly.Centroid());
            this.children[index2].site.position = siteList[index2].poly.Centroid();
          }
          num1 = num2;
          this.PlaceSites(siteList, depth);
          if (pd)
          {
            if (!this.ComputeNodePD(siteList))
            {
              this.visited = Node.VisitedType.Error;
              return;
            }
          }
          else if (!this.ComputeNode(siteList))
          {
            this.visited = Node.VisitedType.Error;
            return;
          }
        }
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal)
          {
            Tree child = this.children[index] as Tree;
            if (child.ComputeChildren(depth))
              child.RelaxRecursive(depth + 1, iterations, minEnergy);
          }
        }
        this.visited = Node.VisitedType.VisitedSuccess;
      }
    }

    public float Relax(int depth, int relaxDepth, bool pd = false)
    {
      if (this.dontRelaxChildren || depth > Node.maxDepth || (depth > relaxDepth || this.site.poly == null) || (this.children == null || this.children.Count == 0))
        return 0.0f;
      float num = 0.0f;
      if (depth < relaxDepth)
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal)
          {
            Tree child = this.children[index] as Tree;
            num += child.Relax(depth + 1, relaxDepth);
          }
        }
        return num;
      }
      if (depth == relaxDepth)
      {
        List<Diagram.Site> siteList = new List<Diagram.Site>();
        for (int index = 0; index < this.children.Count; ++index)
          siteList.Add(this.children[index].site);
        if (pd)
        {
          if (!this.ComputeNodePD(siteList))
            return 0.0f;
        }
        else
        {
          this.PlaceSites(siteList, depth);
          if (!this.ComputeNode(siteList))
            return 0.0f;
        }
        for (int index = 0; index < this.children.Count; ++index)
        {
          num += Vector2.Distance(this.children[index].site.position, siteList[index].poly.Centroid());
          this.children[index].site.position = siteList[index].poly.Centroid();
          if (this.children[index].type == Node.NodeType.Internal && !(this.children[index] as Tree).ComputeChildren(depth))
            return 0.0f;
        }
      }
      return num;
    }

    public Node GetNodeForPoint(Vector2 point, bool stopAtFirstChild = false)
    {
      if (this.site.poly == null)
        return (Node) null;
      if (this.children == null || this.children.Count == 0)
        return (Node) this;
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].site.poly.Contains(point))
        {
          if (this.children[index].type != Node.NodeType.Internal)
            return this.children[index];
          Tree child = this.children[index] as Tree;
          return stopAtFirstChild ? this.children[index] : child.GetNodeForPoint(point);
        }
      }
      return this.site.poly.Contains(point) ? (Node) this : (Node) null;
    }

    public Node GetNodeForSite(Diagram.Site target)
    {
      if (this.site == target)
        return (Node) this;
      if (this.site.poly == null || this.children == null || this.children.Count == 0)
        return (Node) null;
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].site == target)
          return this.children[index];
        if (this.children[index].site.poly.Contains(target.position))
          return this.children[index].type == Node.NodeType.Internal ? (this.children[index] as Tree).GetNodeForSite(target) : this.children[index];
      }
      return (Node) null;
    }

    public void GetIntersectingLeafSites(LineSegment edge, List<Diagram.Site> intersectingSites)
    {
      LineSegment intersectingSegment = new LineSegment(new Vector2?(), new Vector2?());
      if (!(this.site.poly.Contains(edge.p0.Value) | this.site.poly.Contains(edge.p1.Value)) && !this.site.poly.ClipSegment(edge, ref intersectingSegment))
        return;
      if (this.children.Count == 0)
      {
        intersectingSites.Add(this.site);
      }
      else
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal)
            ((Tree) this.children[index]).GetIntersectingLeafSites(edge, intersectingSites);
          else
            ((Leaf) this.children[index]).GetIntersectingSites(edge, intersectingSites);
        }
      }
    }

    public void GetIntersectingLeafNodes(LineSegment edge, List<Leaf> intersectingNodes)
    {
      LineSegment intersectingSegment = new LineSegment(new Vector2?(), new Vector2?());
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].site.poly.Contains(edge.p0.Value) | this.children[index].site.poly.Contains(edge.p1.Value) || this.children[index].site.poly.ClipSegment(edge, ref intersectingSegment))
        {
          if (this.children[index].type == Node.NodeType.Internal)
            ((Tree) this.children[index]).GetIntersectingLeafNodes(edge, intersectingNodes);
          else
            intersectingNodes.Add((Leaf) this.children[index]);
        }
      }
    }

    public override Tree Split(Node.SplitCommand cmd)
    {
      if (cmd.SplitFunction != null)
        cmd.SplitFunction(this, cmd);
      this.ComputeChildrenRecursive(0);
      this.RelaxRecursive(0, 3);
      return this;
    }

    public Tree ReplaceLeafWithTree(Leaf leaf)
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] == leaf)
        {
          this.children[index] = (Node) new Tree(leaf.site, this, this.myRandom.seed + index);
          this.children[index].log = leaf.log;
          if (leaf.tags != null)
            this.children[index].SetTags(leaf.tags);
          return this.children[index] as Tree;
        }
      }
      return (Tree) null;
    }

    public Leaf ReplaceTreeWithLeaf(Tree tree)
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] == tree)
        {
          this.children[index] = (Node) new Leaf(tree.site, this);
          this.children[index].log = tree.log;
          if (tree.tags != null)
            this.children[index].SetTags(tree.tags);
          return this.children[index] as Leaf;
        }
      }
      return (Leaf) null;
    }

    public void ForceLowestToLeaf()
    {
      List<Node> nodes = new List<Node>();
      this.GetLeafNodes(nodes);
      for (int index = 0; index < nodes.Count; ++index)
      {
        if (nodes[index].type == Node.NodeType.Internal)
          nodes[index].parent.ReplaceTreeWithLeaf(nodes[index] as Tree);
      }
    }

    public void Collapse()
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index] != null && this.children[index].type == Node.NodeType.Internal)
        {
          Tree child1 = (Tree) this.children[index];
          if (child1.ChildCount() > 1)
          {
            child1.Collapse();
          }
          else
          {
            Node child2 = this.children[index];
            this.children[index] = (Node) new Leaf(child1.site, this);
            this.children[index].log = child2.log;
            if (child1.tags != null)
              this.children[index].SetTags(child1.tags);
          }
        }
      }
    }

    public void VisitAll(System.Action<Node> action)
    {
      action((Node) this);
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].type == Node.NodeType.Internal)
          (this.children[index] as Tree).VisitAll(action);
        else
          action(this.children[index]);
      }
    }

    public void GetLeafNodes(List<Node> nodes, Tree.LeafNodeTest test = null)
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].type == Node.NodeType.Internal)
        {
          Tree child = (Tree) this.children[index];
          if (child.ChildCount() > 0)
            child.GetLeafNodes(nodes, test);
          else if (test == null || test(this.children[index]))
            nodes.Add(this.children[index]);
        }
        else if (test == null || test(this.children[index]))
          nodes.Add(this.children[index]);
      }
    }

    public void GetInternalNodes(List<Tree> nodes)
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].type == Node.NodeType.Internal)
        {
          Tree child = (Tree) this.children[index];
          nodes.Add(child);
          if (child.ChildCount() > 0)
            child.GetInternalNodes(nodes);
        }
      }
    }

    public void ResetParentPointer()
    {
      for (int index = 0; index < this.children.Count; ++index)
      {
        if (this.children[index].type == Node.NodeType.Internal)
        {
          Tree child = (Tree) this.children[index];
          if (child.ChildCount() > 0)
            child.ResetParentPointer();
        }
        this.children[index].SetParent(this);
      }
    }

    public void AddTagToChildren(Tag tag)
    {
      for (int index = 0; index < this.children.Count; ++index)
        this.children[index].AddTag(tag);
    }

    public void GetNodesWithTag(Tag tag, List<Node> nodes)
    {
      if (this.children.Count == 0 && this.tags.Contains(tag))
      {
        nodes.Add((Node) this);
      }
      else
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal)
            ((Tree) this.children[index]).GetNodesWithTag(tag, nodes);
          else if (this.children[index].tags.Contains(tag))
            nodes.Add(this.children[index]);
        }
      }
    }

    public void GetNodesWithoutTag(Tag tag, List<Node> nodes)
    {
      if (this.children.Count == 0 && !this.tags.Contains(tag))
      {
        nodes.Add((Node) this);
      }
      else
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (this.children[index].type == Node.NodeType.Internal)
            ((Tree) this.children[index]).GetNodesWithoutTag(tag, nodes);
          else if (!this.children[index].tags.Contains(tag))
            nodes.Add(this.children[index]);
        }
      }
    }

    public delegate bool LeafNodeTest(Node node);
  }
}
