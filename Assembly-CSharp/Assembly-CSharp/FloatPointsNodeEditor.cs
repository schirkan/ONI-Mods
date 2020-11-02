// Decompiled with JetBrains decompiler
// Type: FloatPointsNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Terrace Control", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class FloatPointsNodeEditor : BaseNodeEditor
{
  private const string Id = "floatPointsNodeEditor";
  [SerializeField]
  public FloatList target = new FloatList();
  private static float height = 100f;

  public override string GetID => "floatPointsNodeEditor";

  public override System.Type GetObjectType => typeof (FloatPointsNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    FloatPointsNodeEditor instance = ScriptableObject.CreateInstance<FloatPointsNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, FloatPointsNodeEditor.height);
    instance.name = "Terrace Control";
    instance.CreateOutput("Terrace", "FloatList", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    this.Outputs[0].SetValue<FloatList>(this.target);
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
