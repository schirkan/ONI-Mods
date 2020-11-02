// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Standard.RTCanvasCalculator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework.Standard
{
  public class RTCanvasCalculator : MonoBehaviour
  {
    public string canvasPath;

    public NodeCanvas canvas { get; private set; }

    private void Start() => this.LoadCanvas(this.canvasPath);

    public void AssureCanvas()
    {
      if (!((UnityEngine.Object) this.canvas == (UnityEngine.Object) null))
        return;
      this.LoadCanvas(this.canvasPath);
      if ((UnityEngine.Object) this.canvas == (UnityEngine.Object) null)
        throw new UnityException("No canvas specified to calculate on " + this.name + "!");
    }

    public void LoadCanvas(string path)
    {
      this.canvasPath = path;
      if (!string.IsNullOrEmpty(this.canvasPath))
      {
        this.canvas = NodeEditorSaveManager.LoadNodeCanvas(this.canvasPath, true);
        this.CalculateCanvas();
      }
      else
        this.canvas = (NodeCanvas) null;
    }

    public void CalculateCanvas()
    {
      this.AssureCanvas();
      NodeEditor.RecalculateAll(this.canvas);
      this.DebugOutputResults();
    }

    private void DebugOutputResults()
    {
      this.AssureCanvas();
      foreach (NodeEditorFramework.Node outputNode in this.getOutputNodes())
      {
        string str = "(OUT) " + outputNode.name + ": ";
        if (outputNode.Outputs.Count == 0)
        {
          foreach (NodeInput input in outputNode.Inputs)
            str = str + input.typeID + " " + (input.IsValueNull ? "NULL" : input.GetValue().ToString()) + "; ";
        }
        else
        {
          foreach (NodeOutput output in outputNode.Outputs)
            str = str + output.typeID + " " + (output.IsValueNull ? "NULL" : output.GetValue().ToString()) + "; ";
        }
        Debug.Log((object) str);
      }
    }

    public List<NodeEditorFramework.Node> getInputNodes()
    {
      this.AssureCanvas();
      return this.canvas.nodes.Where<NodeEditorFramework.Node>((Func<NodeEditorFramework.Node, bool>) (node => node.Inputs.Count == 0 && node.Outputs.Count != 0 || node.Inputs.TrueForAll((Predicate<NodeInput>) (input => (UnityEngine.Object) input.connection == (UnityEngine.Object) null)))).ToList<NodeEditorFramework.Node>();
    }

    public List<NodeEditorFramework.Node> getOutputNodes()
    {
      this.AssureCanvas();
      return this.canvas.nodes.Where<NodeEditorFramework.Node>((Func<NodeEditorFramework.Node, bool>) (node => node.Outputs.Count == 0 && node.Inputs.Count != 0 || node.Outputs.TrueForAll((Predicate<NodeOutput>) (output => output.connections.Count == 0)))).ToList<NodeEditorFramework.Node>();
    }
  }
}
