﻿// Decompiled with JetBrains decompiler
// Type: FilterNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Filter", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class FilterNodeEditor : BaseNodeEditor
{
  private const string Id = "filterNodeEditor";
  [SerializeField]
  public ProcGen.Noise.Filter target = new ProcGen.Noise.Filter();

  public override string GetID => "filterNodeEditor";

  public override System.Type GetObjectType => typeof (FilterNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    FilterNodeEditor instance = ScriptableObject.CreateInstance<FilterNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, 200f);
    instance.name = "Filter";
    instance.CreateInput("Source Node", "IModule3D", NodeSide.Left, 30f);
    instance.CreateOutput("Next Node", "IModule3D", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    if (!this.allInputsReady())
      return false;
    IModule3D module3D = this.Inputs[0].GetValue<IModule3D>();
    if (module3D == null)
      return false;
    IModule3D module = this.target.CreateModule();
    if (module == null)
      return false;
    ((FilterModule) module).Primitive3D = module3D;
    this.Outputs[0].SetValue<IModule3D>(module);
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
