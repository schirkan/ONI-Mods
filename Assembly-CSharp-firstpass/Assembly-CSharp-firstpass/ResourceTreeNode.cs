// Decompiled with JetBrains decompiler
// Type: ResourceTreeNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTreeNode : Resource
{
  public float nodeX;
  public float nodeY;
  public float width;
  public float height;
  public List<ResourceTreeNode> references = new List<ResourceTreeNode>();
  public List<ResourceTreeNode.Edge> edges = new List<ResourceTreeNode.Edge>();

  public Vector2 position => new Vector2(this.nodeX, this.nodeY);

  public Vector2 center => this.position + new Vector2(this.width / 2f, (float) (-(double) this.height / 2.0));

  public class Edge
  {
    public Vector2f sourceOffset = new Vector2f(0, 0);
    public Vector2f targetOffset = new Vector2f(0, 0);

    public ResourceTreeNode.Edge.EdgeType edgeType { get; private set; }

    public ResourceTreeNode source { get; private set; }

    public ResourceTreeNode target { get; private set; }

    private Vector2 SourcePos() => this.source.center + (Vector2) this.sourceOffset;

    private Vector2 TargetPos() => this.target.center + (Vector2) this.targetOffset;

    public List<Vector2> SrcTarget => new List<Vector2>()
    {
      this.SourcePos(),
      this.TargetPos()
    };

    public List<Vector2> path { get; private set; }

    public Edge(
      ResourceTreeNode source,
      ResourceTreeNode target,
      ResourceTreeNode.Edge.EdgeType edgeType)
    {
      this.edgeType = edgeType;
      this.source = source;
      this.target = target;
      this.path = (List<Vector2>) null;
    }

    public void AddToPath(Vector2f point)
    {
      if (this.path == null)
        this.path = new List<Vector2>();
      this.path.Add((Vector2) point);
    }

    public void Render(Rect rect, float width, Color colour)
    {
      int edgeType = (int) this.edgeType;
      RTEditorGUI.DrawLine(rect, this.SourcePos(), this.TargetPos(), colour, (Texture2D) null, width);
    }

    public enum EdgeType
    {
      PolyLineEdge,
      QuadCurveEdge,
      ArcEdge,
      SplineEdge,
      BezierEdge,
      GenericEdge,
    }
  }
}
