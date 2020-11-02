// Decompiled with JetBrains decompiler
// Type: CombinerModuleNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Combine", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class CombinerModuleNodeEditor : BaseNodeEditor
{
  private const string Id = "combinerModuleNodeEditor";
  [SerializeField]
  public Combiner target = new Combiner();

  public override string GetID => "combinerModuleNodeEditor";

  public override System.Type GetObjectType => typeof (CombinerModuleNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    CombinerModuleNodeEditor instance = ScriptableObject.CreateInstance<CombinerModuleNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, 200f);
    instance.name = "Combine";
    instance.CreateInput("Source A", "IModule3D", NodeSide.Left, 10f);
    instance.CreateInput("Source B", "IModule3D", NodeSide.Left, 30f);
    instance.CreateOutput("Next Node", "IModule3D", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    if (!this.allInputsReady())
      return false;
    IModule3D leftModule = this.Inputs[0].GetValue<IModule3D>();
    if (leftModule == null)
      return false;
    IModule3D rightModule = this.Inputs[1].GetValue<IModule3D>();
    if (rightModule == null)
      return false;
    IModule3D module = this.target.CreateModule(leftModule, rightModule);
    if (module == null)
      return false;
    this.Outputs[0].SetValue<IModule3D>(module);
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
