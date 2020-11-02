﻿// Decompiled with JetBrains decompiler
// Type: TerminalNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Terminus", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class TerminalNodeEditor : BaseNodeEditor
{
  private const string Id = "terminalNodeEditor";

  public override string GetID => "terminalNodeEditor";

  public override System.Type GetObjectType => typeof (TerminalNodeEditor);

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    TerminalNodeEditor instance = ScriptableObject.CreateInstance<TerminalNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 250f, 125f);
    instance.name = "Terminus";
    instance.CreateInput("Final Node", "IModule3D", NodeSide.Top, 10f);
    instance.CreateOutput("Display", "IModule3D", NodeSide.Right, 30f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    if (!this.allInputsReady())
      return false;
    this.Outputs[0].SetValue<IModule3D>(this.Inputs[0].GetValue<IModule3D>());
    return true;
  }

  protected override void NodeGUI() => base.NodeGUI();
}
