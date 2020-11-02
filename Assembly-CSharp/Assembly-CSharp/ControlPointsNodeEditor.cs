// Decompiled with JetBrains decompiler
// Type: ControlPointsNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Curve Control", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class ControlPointsNodeEditor : BaseNodeEditor
{
  private const string Id = "controlPointsNodeEditor";
  [SerializeField]
  public ControlPointList target = new ControlPointList();
  private static float height = 100f;

  public override string GetID => "controlPointsNodeEditor";

  public override System.Type GetObjectType => typeof (ControlPointsNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    ControlPointsNodeEditor instance = ScriptableObject.CreateInstance<ControlPointsNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, ControlPointsNodeEditor.height);
    instance.name = "Curve Control";
    instance.CreateOutput("Curve", "ControlPoints", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    this.Outputs[0].SetValue<ControlPointList>(this.target);
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
