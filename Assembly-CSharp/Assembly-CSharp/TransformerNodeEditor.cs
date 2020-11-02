﻿// Decompiled with JetBrains decompiler
// Type: TransformerNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Transformer", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class TransformerNodeEditor : BaseNodeEditor
{
  private const string Id = "transformerNodeEditor";
  [SerializeField]
  public Transformer target = new Transformer();

  public override string GetID => "transformerNodeEditor";

  public override System.Type GetObjectType => typeof (TransformerNodeEditor);

  public override NoiseBase GetTarget() => (NoiseBase) this.target;

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    TransformerNodeEditor instance = ScriptableObject.CreateInstance<TransformerNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 300f, 200f);
    instance.name = "Transformer";
    instance.CreateInput("Source", "IModule3D", NodeSide.Left, 10f);
    instance.CreateInput("X", "IModule3D", NodeSide.Left, 30f);
    instance.CreateInput("Y", "IModule3D", NodeSide.Left, 40f);
    instance.CreateInput("Z", "IModule3D", NodeSide.Left, 50f);
    instance.CreateOutput("Next Node", "IModule3D", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    IModule3D sourceModule = this.Inputs[0].GetValue<IModule3D>();
    if (sourceModule == null)
      return false;
    IModule3D xModule = this.Inputs[1].GetValue<IModule3D>();
    IModule3D yModule = this.Inputs[2].GetValue<IModule3D>();
    IModule3D zModule = this.Inputs[3].GetValue<IModule3D>();
    if (this.target.transformerType != Transformer.TransformerType.RotatePoint && (xModule == null || yModule == null || zModule == null))
      return false;
    IModule3D module = this.target.CreateModule(sourceModule, xModule, yModule, zModule);
    if (module == null)
      return false;
    this.Outputs[0].SetValue<IModule3D>(module);
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
