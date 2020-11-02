// Decompiled with JetBrains decompiler
// Type: PrimitiveNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Primitive", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class PrimitiveNodeEditor : BaseNodeEditor
{
  private const string Id = "primitiveNodeEditor";
  public Primitive target = new Primitive();

  public override string GetID => "primitiveNodeEditor";

  public override System.Type GetObjectType => typeof (PrimitiveNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    PrimitiveNodeEditor instance = ScriptableObject.CreateInstance<PrimitiveNodeEditor>();
    instance.target = new Primitive();
    instance.rect = new Rect(pos.x, pos.y, 250f, 125f);
    instance.name = "Primative";
    instance.CreateOutput("Next Node", "IModule3D", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    this.Outputs[0].SetValue<IModule3D>(this.target.CreateModule(0));
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
