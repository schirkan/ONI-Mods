// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeCanvas
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework
{
  [NodeCanvasType("Default")]
  public class NodeCanvas : ScriptableObject
  {
    public List<Node> nodes = new List<Node>();
    public NodeEditorState[] editorStates = new NodeEditorState[0];
    public bool livesInScene;

    public void Validate()
    {
      if (this.nodes == null)
      {
        Debug.LogWarning((object) ("NodeCanvas '" + this.name + "' nodes were erased and set to null! Automatically fixed!"));
        this.nodes = new List<Node>();
      }
      for (int index1 = 0; index1 < this.nodes.Count; ++index1)
      {
        Node node = this.nodes[index1];
        if ((UnityEngine.Object) node == (UnityEngine.Object) null)
        {
          Debug.LogWarning((object) ("NodeCanvas '" + this.name + "' contained broken (null) nodes! Automatically fixed!"));
          this.nodes.RemoveAt(index1);
          --index1;
        }
        else
        {
          for (int index2 = 0; index2 < node.Inputs.Count; ++index2)
          {
            NodeInput input = node.Inputs[index2];
            if ((UnityEngine.Object) input == (UnityEngine.Object) null)
            {
              Debug.LogWarning((object) ("NodeCanvas '" + this.name + "' Node '" + node.name + "' contained broken (null) NodeKnobs! Automatically fixed!"));
              node.Inputs.RemoveAt(index2);
              --index2;
            }
            else if ((UnityEngine.Object) input.connection != (UnityEngine.Object) null && (UnityEngine.Object) input.connection.body == (UnityEngine.Object) null)
              input.connection = (NodeOutput) null;
          }
          for (int index2 = 0; index2 < node.Outputs.Count; ++index2)
          {
            NodeOutput output = node.Outputs[index2];
            if ((UnityEngine.Object) output == (UnityEngine.Object) null)
            {
              Debug.LogWarning((object) ("NodeCanvas '" + this.name + "' Node '" + node.name + "' contained broken (null) NodeKnobs! Automatically fixed!"));
              node.Outputs.RemoveAt(index2);
              --index2;
            }
            else
            {
              for (int index3 = 0; index3 < output.connections.Count; ++index3)
              {
                NodeInput connection = output.connections[index3];
                if ((UnityEngine.Object) connection == (UnityEngine.Object) null || (UnityEngine.Object) connection.body == (UnityEngine.Object) null)
                {
                  output.connections.RemoveAt(index3);
                  --index3;
                }
              }
            }
          }
          for (int index2 = 0; index2 < node.nodeKnobs.Count; ++index2)
          {
            NodeKnob nodeKnob = node.nodeKnobs[index2];
            if ((UnityEngine.Object) nodeKnob == (UnityEngine.Object) null)
            {
              Debug.LogWarning((object) ("NodeCanvas '" + this.name + "' Node '" + node.name + "' contained broken (null) NodeKnobs! Automatically fixed!"));
              node.nodeKnobs.RemoveAt(index2);
              --index2;
            }
            else
            {
              switch (nodeKnob)
              {
                case NodeInput _:
                  NodeInput nodeInput = nodeKnob as NodeInput;
                  if ((UnityEngine.Object) nodeInput.connection != (UnityEngine.Object) null && (UnityEngine.Object) nodeInput.connection.body == (UnityEngine.Object) null)
                  {
                    nodeInput.connection = (NodeOutput) null;
                    continue;
                  }
                  continue;
                case NodeOutput _:
                  NodeOutput nodeOutput = nodeKnob as NodeOutput;
                  for (int index3 = 0; index3 < nodeOutput.connections.Count; ++index3)
                  {
                    NodeInput connection = nodeOutput.connections[index3];
                    if ((UnityEngine.Object) connection == (UnityEngine.Object) null || (UnityEngine.Object) connection.body == (UnityEngine.Object) null)
                    {
                      nodeOutput.connections.RemoveAt(index3);
                      --index3;
                    }
                  }
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      if (this.editorStates == null)
      {
        Debug.LogWarning((object) ("NodeCanvas '" + this.name + "' editorStates were erased! Automatically fixed!"));
        this.editorStates = new NodeEditorState[0];
      }
      this.editorStates = ((IEnumerable<NodeEditorState>) this.editorStates).Where<NodeEditorState>((Func<NodeEditorState, bool>) (state => (UnityEngine.Object) state != (UnityEngine.Object) null)).ToArray<NodeEditorState>();
      foreach (NodeEditorState editorState in this.editorStates)
      {
        if (!this.nodes.Contains(editorState.selectedNode))
          editorState.selectedNode = (Node) null;
      }
    }

    public virtual void BeforeSavingCanvas()
    {
    }

    public virtual void AdditionalSaveMethods(
      string sceneCanvasName,
      NodeCanvas.CompleteLoadCallback onComplete)
    {
    }

    public virtual string DrawAdditionalSettings(string sceneCanvasName) => sceneCanvasName;

    public virtual void UpdateSettings(string sceneCanvasName)
    {
    }

    public delegate void CompleteLoadCallback(string fileName, NodeCanvas canvas);
  }
}
