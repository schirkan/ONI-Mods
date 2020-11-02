// Decompiled with JetBrains decompiler
// Type: SelectorModuleNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Select", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class SelectorModuleNodeEditor : BaseNodeEditor
{
  private const string Id = "selectorModuleNodeEditor";
  [SerializeField]
  public Selector target = new Selector();

  public override string GetID => "selectorModuleNodeEditor";

  public override System.Type GetObjectType => typeof (SelectorModuleNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    SelectorModuleNodeEditor instance = ScriptableObject.CreateInstance<SelectorModuleNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, 100f);
    instance.name = "Selector";
    instance.CreateOutput("Next Node", "IModule3D", NodeSide.Right, 30f);
    instance.CreateInput("Selector", "IModule3D", NodeSide.Top, 10f);
    instance.CreateInput("Left", "IModule3D", NodeSide.Left, 10f);
    instance.CreateInput("Right", "IModule3D", NodeSide.Left, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    if (!this.allInputsReady())
      return false;
    IModule3D selectModule = this.Inputs[0].GetValue<IModule3D>();
    if (selectModule == null)
      return false;
    IModule3D rightModule = this.Inputs[1].GetValue<IModule3D>();
    if (rightModule == null)
      return false;
    IModule3D leftModule = this.Inputs[2].GetValue<IModule3D>();
    if (leftModule == null)
      return false;
    IModule3D module = this.target.CreateModule(selectModule, leftModule, rightModule);
    if (module == null)
      return false;
    this.Outputs[0].SetValue<IModule3D>(module);
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
