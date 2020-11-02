// Decompiled with JetBrains decompiler
// Type: BaseNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using System;
using UnityEngine;

[NodeEditorFramework.Node(true, "Noise/Base Noise Node", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class BaseNodeEditor : NodeEditorFramework.Node
{
  private const string Id = "baseNodeEditor";

  public virtual System.Type GetObjectType => typeof (BaseNodeEditor);

  public override string GetID => "baseNodeEditor";

  public virtual NoiseBase GetTarget() => (NoiseBase) null;

  protected SampleSettings settings
  {
    get
    {
      NoiseNodeCanvas curNodeCanvas = NodeEditor.curNodeCanvas as NoiseNodeCanvas;
      return (UnityEngine.Object) curNodeCanvas != (UnityEngine.Object) null ? curNodeCanvas.settings : (SampleSettings) null;
    }
  }

  public override NodeEditorFramework.Node Create(Vector2 pos) => (NodeEditorFramework.Node) null;

  protected override void NodeGUI()
  {
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    if (this.Inputs != null)
    {
      GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      foreach (NodeKnob input in this.Inputs)
        input.DisplayLayout();
      GUILayout.EndVertical();
    }
    if (this.Outputs != null)
    {
      GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      foreach (NodeKnob output in this.Outputs)
        output.DisplayLayout();
      GUILayout.EndVertical();
    }
    GUILayout.EndHorizontal();
    if (!GUI.changed)
      return;
    NodeEditor.RecalculateFrom((NodeEditorFramework.Node) this);
  }
}
