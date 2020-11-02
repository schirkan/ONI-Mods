// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Node
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework
{
  public abstract class Node : ScriptableObject
  {
    public Rect rect;
    internal Vector2 contentOffset = Vector2.zero;
    [SerializeField]
    public List<NodeKnob> nodeKnobs = new List<NodeKnob>();
    [SerializeField]
    public List<NodeInput> Inputs = new List<NodeInput>();
    [SerializeField]
    public List<NodeOutput> Outputs = new List<NodeOutput>();
    [HideInInspector]
    [NonSerialized]
    internal bool calculated = true;
    [NonSerialized]
    private List<Node> recursiveSearchSurpassed;
    [NonSerialized]
    private Node startRecursiveSearchNode;

    protected internal void InitBase()
    {
      NodeEditor.RecalculateFrom(this);
      if ((UnityEngine.Object) NodeEditor.curNodeCanvas == (UnityEngine.Object) null || NodeEditor.curNodeCanvas.nodes == null)
        return;
      if (!NodeEditor.curNodeCanvas.nodes.Contains(this))
        NodeEditor.curNodeCanvas.nodes.Add(this);
      NodeEditor.RepaintClients();
    }

    public void Delete()
    {
      if (!NodeEditor.curNodeCanvas.nodes.Contains(this))
        throw new UnityException("The Node " + this.name + " does not exist on the Canvas " + NodeEditor.curNodeCanvas.name + "!");
      NodeEditorCallbacks.IssueOnDeleteNode(this);
      NodeEditor.curNodeCanvas.nodes.Remove(this);
      for (int index = 0; index < this.Outputs.Count; ++index)
      {
        NodeOutput output = this.Outputs[index];
        while (output.connections.Count != 0)
          output.connections[0].RemoveConnection();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) output, true);
      }
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        NodeInput input = this.Inputs[index];
        if ((UnityEngine.Object) input.connection != (UnityEngine.Object) null)
          input.connection.connections.Remove(input);
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) input, true);
      }
      for (int index = 0; index < this.nodeKnobs.Count; ++index)
      {
        if ((UnityEngine.Object) this.nodeKnobs[index] != (UnityEngine.Object) null)
          UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.nodeKnobs[index], true);
      }
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this, true);
    }

    public static Node Create(string nodeID, Vector2 position) => Node.Create(nodeID, position, (NodeOutput) null);

    public static Node Create(string nodeID, Vector2 position, NodeOutput connectingOutput)
    {
      Node defaultNode = NodeTypes.getDefaultNode(nodeID);
      Node node = !((UnityEngine.Object) defaultNode == (UnityEngine.Object) null) ? defaultNode.Create(position) : throw new UnityException("Cannot create Node with id " + nodeID + " as no such Node type is registered!");
      node.InitBase();
      if ((UnityEngine.Object) connectingOutput != (UnityEngine.Object) null)
      {
        foreach (NodeInput input in node.Inputs)
        {
          if (input.TryApplyConnection(connectingOutput))
            break;
        }
      }
      NodeEditorCallbacks.IssueOnAddNode(node);
      return node;
    }

    internal void CheckNodeKnobMigration()
    {
      if (this.nodeKnobs.Count != 0 || this.Inputs.Count == 0 && this.Outputs.Count == 0)
        return;
      this.nodeKnobs.AddRange(this.Inputs.Cast<NodeKnob>());
      this.nodeKnobs.AddRange(this.Outputs.Cast<NodeKnob>());
    }

    public abstract string GetID { get; }

    public abstract Node Create(Vector2 pos);

    protected internal abstract void NodeGUI();

    public virtual void DrawNodePropertyEditor()
    {
    }

    public virtual bool Calculate() => true;

    public virtual bool AllowRecursion => false;

    public virtual bool ContinueCalculation => true;

    protected internal virtual void OnDelete()
    {
    }

    protected internal virtual void OnAddInputConnection(NodeInput input)
    {
    }

    protected internal virtual void OnAddOutputConnection(NodeOutput output)
    {
    }

    public virtual ScriptableObject[] GetScriptableObjects() => new ScriptableObject[0];

    protected internal virtual void CopyScriptableObjects(
      Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
    {
    }

    public void SerializeInputsAndOutputs(
      Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
    {
    }

    protected internal virtual void DrawNode()
    {
      Rect rect1 = this.rect;
      rect1.position += NodeEditor.curEditorState.zoomPanAdjust + NodeEditor.curEditorState.panOffset;
      this.contentOffset = new Vector2(0.0f, 20f);
      GUI.Label(new Rect(rect1.x, rect1.y, rect1.width, this.contentOffset.y), this.name, (UnityEngine.Object) NodeEditor.curEditorState.selectedNode == (UnityEngine.Object) this ? NodeEditorGUI.nodeBoxBold : NodeEditorGUI.nodeBox);
      Rect rect2 = new Rect(rect1.x, rect1.y + this.contentOffset.y, rect1.width, rect1.height - this.contentOffset.y);
      GUI.BeginGroup(rect2, GUI.skin.box);
      rect2.position = Vector2.zero;
      GUILayout.BeginArea(rect2, GUI.skin.box);
      GUI.changed = false;
      this.NodeGUI();
      GUILayout.EndArea();
      GUI.EndGroup();
    }

    protected internal virtual void DrawKnobs()
    {
      this.CheckNodeKnobMigration();
      for (int index = 0; index < this.nodeKnobs.Count; ++index)
        this.nodeKnobs[index].DrawKnob();
    }

    protected internal virtual void DrawConnections()
    {
      this.CheckNodeKnobMigration();
      if (Event.current.type != EventType.Repaint)
        return;
      for (int index1 = 0; index1 < this.Outputs.Count; ++index1)
      {
        NodeOutput output = this.Outputs[index1];
        Rect guiKnob = output.GetGUIKnob();
        Vector2 center1 = guiKnob.center;
        Vector2 direction1 = output.GetDirection();
        for (int index2 = 0; index2 < output.connections.Count; ++index2)
        {
          NodeInput connection = output.connections[index2];
          Vector2 startPos = center1;
          Vector2 startDir = direction1;
          guiKnob = connection.GetGUIKnob();
          Vector2 center2 = guiKnob.center;
          Vector2 direction2 = connection.GetDirection();
          Color color = output.typeData.Color;
          NodeEditorGUI.DrawConnection(startPos, startDir, center2, direction2, color);
        }
      }
    }

    protected internal bool allInputsReady()
    {
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        if ((UnityEngine.Object) this.Inputs[index].connection == (UnityEngine.Object) null || this.Inputs[index].connection.IsValueNull)
          return false;
      }
      return true;
    }

    protected internal bool hasUnassignedInputs()
    {
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        if ((UnityEngine.Object) this.Inputs[index].connection == (UnityEngine.Object) null)
          return true;
      }
      return false;
    }

    protected internal bool descendantsCalculated()
    {
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        if ((UnityEngine.Object) this.Inputs[index].connection != (UnityEngine.Object) null && !this.Inputs[index].connection.body.calculated)
          return false;
      }
      return true;
    }

    protected internal bool isInput()
    {
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        if ((UnityEngine.Object) this.Inputs[index].connection != (UnityEngine.Object) null)
          return false;
      }
      return true;
    }

    public NodeOutput CreateOutput(string outputName, string outputType) => NodeOutput.Create(this, outputName, outputType);

    public NodeOutput CreateOutput(
      string outputName,
      string outputType,
      NodeSide nodeSide)
    {
      return NodeOutput.Create(this, outputName, outputType, nodeSide);
    }

    public NodeOutput CreateOutput(
      string outputName,
      string outputType,
      NodeSide nodeSide,
      float sidePosition)
    {
      return NodeOutput.Create(this, outputName, outputType, nodeSide, sidePosition);
    }

    protected void OutputKnob(int outputIdx)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.Outputs[outputIdx].SetPosition();
    }

    public NodeInput CreateInput(string inputName, string inputType) => NodeInput.Create(this, inputName, inputType);

    public NodeInput CreateInput(string inputName, string inputType, NodeSide nodeSide) => NodeInput.Create(this, inputName, inputType, nodeSide);

    public NodeInput CreateInput(
      string inputName,
      string inputType,
      NodeSide nodeSide,
      float sidePosition)
    {
      return NodeInput.Create(this, inputName, inputType, nodeSide, sidePosition);
    }

    protected void InputKnob(int inputIdx)
    {
      if (Event.current.type != EventType.Repaint)
        return;
      this.Inputs[inputIdx].SetPosition();
    }

    protected static void ReassignOutputType(ref NodeOutput output, System.Type newOutputType)
    {
      Node body = output.body;
      string name = output.name;
      IEnumerable<NodeInput> nodeInputs = output.connections.Where<NodeInput>((Func<NodeInput, bool>) (connection => connection.typeData.Type.IsAssignableFrom(newOutputType)));
      output.Delete();
      NodeEditorCallbacks.IssueOnAddNodeKnob((NodeKnob) NodeOutput.Create(body, name, newOutputType.AssemblyQualifiedName));
      output = body.Outputs[body.Outputs.Count - 1];
      foreach (NodeInput nodeInput in nodeInputs)
        nodeInput.ApplyConnection(output);
    }

    protected static void ReassignInputType(ref NodeInput input, System.Type newInputType)
    {
      Node body = input.body;
      string name = input.name;
      NodeOutput output = (NodeOutput) null;
      if ((UnityEngine.Object) input.connection != (UnityEngine.Object) null && newInputType.IsAssignableFrom(input.connection.typeData.Type))
        output = input.connection;
      input.Delete();
      NodeEditorCallbacks.IssueOnAddNodeKnob((NodeKnob) NodeInput.Create(body, name, newInputType.AssemblyQualifiedName));
      input = body.Inputs[body.Inputs.Count - 1];
      if (!((UnityEngine.Object) output != (UnityEngine.Object) null))
        return;
      input.ApplyConnection(output);
    }

    public bool isChildOf(Node otherNode)
    {
      if ((UnityEngine.Object) otherNode == (UnityEngine.Object) null || (UnityEngine.Object) otherNode == (UnityEngine.Object) this || this.BeginRecursiveSearchLoop())
        return false;
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        NodeOutput connection = this.Inputs[index].connection;
        if ((UnityEngine.Object) connection != (UnityEngine.Object) null && (UnityEngine.Object) connection.body != (UnityEngine.Object) this.startRecursiveSearchNode && ((UnityEngine.Object) connection.body == (UnityEngine.Object) otherNode || connection.body.isChildOf(otherNode)))
        {
          this.StopRecursiveSearchLoop();
          return true;
        }
      }
      this.EndRecursiveSearchLoop();
      return false;
    }

    internal bool isInLoop()
    {
      if (this.BeginRecursiveSearchLoop())
        return (UnityEngine.Object) this == (UnityEngine.Object) this.startRecursiveSearchNode;
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        NodeOutput connection = this.Inputs[index].connection;
        if ((UnityEngine.Object) connection != (UnityEngine.Object) null && connection.body.isInLoop())
        {
          this.StopRecursiveSearchLoop();
          return true;
        }
      }
      this.EndRecursiveSearchLoop();
      return false;
    }

    internal bool allowsLoopRecursion(Node otherNode)
    {
      if (this.AllowRecursion)
        return true;
      if ((UnityEngine.Object) otherNode == (UnityEngine.Object) null || this.BeginRecursiveSearchLoop())
        return false;
      for (int index = 0; index < this.Inputs.Count; ++index)
      {
        NodeOutput connection = this.Inputs[index].connection;
        if ((UnityEngine.Object) connection != (UnityEngine.Object) null && connection.body.allowsLoopRecursion(otherNode))
        {
          this.StopRecursiveSearchLoop();
          return true;
        }
      }
      this.EndRecursiveSearchLoop();
      return false;
    }

    public void ClearCalculation()
    {
      if (this.BeginRecursiveSearchLoop())
        return;
      this.calculated = false;
      for (int index1 = 0; index1 < this.Outputs.Count; ++index1)
      {
        NodeOutput output = this.Outputs[index1];
        for (int index2 = 0; index2 < output.connections.Count; ++index2)
          output.connections[index2].body.ClearCalculation();
      }
      this.EndRecursiveSearchLoop();
    }

    internal bool BeginRecursiveSearchLoop()
    {
      if ((UnityEngine.Object) this.startRecursiveSearchNode == (UnityEngine.Object) null || this.recursiveSearchSurpassed == null)
      {
        this.recursiveSearchSurpassed = new List<Node>();
        this.startRecursiveSearchNode = this;
      }
      if (this.recursiveSearchSurpassed.Contains(this))
        return true;
      this.recursiveSearchSurpassed.Add(this);
      return false;
    }

    internal void EndRecursiveSearchLoop()
    {
      if (!((UnityEngine.Object) this.startRecursiveSearchNode == (UnityEngine.Object) this))
        return;
      this.recursiveSearchSurpassed = (List<Node>) null;
      this.startRecursiveSearchNode = (Node) null;
    }

    internal void StopRecursiveSearchLoop()
    {
      this.recursiveSearchSurpassed = (List<Node>) null;
      this.startRecursiveSearchNode = (Node) null;
    }
  }
}
