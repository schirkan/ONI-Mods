// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeInput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NodeEditorFramework
{
  public class NodeInput : NodeKnob
  {
    public NodeOutput connection;
    [FormerlySerializedAs("type")]
    public string typeID;
    private TypeData _typeData;

    protected override NodeSide defaultSide => NodeSide.Left;

    internal TypeData typeData
    {
      get
      {
        this.CheckType();
        return this._typeData;
      }
    }

    public static NodeInput Create(Node nodeBody, string inputName, string inputType) => NodeInput.Create(nodeBody, inputName, inputType, NodeSide.Left, 20f);

    public static NodeInput Create(
      Node nodeBody,
      string inputName,
      string inputType,
      NodeSide nodeSide)
    {
      return NodeInput.Create(nodeBody, inputName, inputType, nodeSide, 20f);
    }

    public static NodeInput Create(
      Node nodeBody,
      string inputName,
      string inputType,
      NodeSide nodeSide,
      float sidePosition)
    {
      NodeInput instance = ScriptableObject.CreateInstance<NodeInput>();
      instance.typeID = inputType;
      instance.InitBase(nodeBody, nodeSide, sidePosition, inputName);
      nodeBody.Inputs.Add(instance);
      return instance;
    }

    public override void Delete()
    {
      this.RemoveConnection();
      this.body.Inputs.Remove(this);
      base.Delete();
    }

    protected internal override void CopyScriptableObjects(
      Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
    {
      this.connection = replaceSerializableObject((ScriptableObject) this.connection) as NodeOutput;
    }

    protected override void ReloadTexture()
    {
      this.CheckType();
      this.knobTexture = this.typeData.InKnobTex;
    }

    private void CheckType()
    {
      if (this._typeData == null || !this._typeData.isValid())
        this._typeData = ConnectionTypes.GetTypeData(this.typeID);
      if (this._typeData != null && this._typeData.isValid())
        return;
      ConnectionTypes.FetchTypes();
      this._typeData = ConnectionTypes.GetTypeData(this.typeID);
      if (this._typeData == null || !this._typeData.isValid())
        throw new UnityException("Could not find type " + this.typeID + "!");
    }

    public bool IsValueNull => !((UnityEngine.Object) this.connection != (UnityEngine.Object) null) || this.connection.IsValueNull;

    public object GetValue() => !((UnityEngine.Object) this.connection != (UnityEngine.Object) null) ? (object) null : this.connection.GetValue();

    public object GetValue(System.Type type) => !((UnityEngine.Object) this.connection != (UnityEngine.Object) null) ? (object) null : this.connection.GetValue(type);

    public void SetValue(object value)
    {
      if (!((UnityEngine.Object) this.connection != (UnityEngine.Object) null))
        return;
      this.connection.SetValue(value);
    }

    public T GetValue<T>() => !((UnityEngine.Object) this.connection != (UnityEngine.Object) null) ? NodeOutput.GetDefault<T>() : this.connection.GetValue<T>();

    public void SetValue<T>(T value)
    {
      if (!((UnityEngine.Object) this.connection != (UnityEngine.Object) null))
        return;
      this.connection.SetValue<T>(value);
    }

    public bool TryApplyConnection(NodeOutput output)
    {
      if (!this.CanApplyConnection(output))
        return false;
      this.ApplyConnection(output);
      return true;
    }

    public bool CanApplyConnection(NodeOutput output)
    {
      if ((UnityEngine.Object) output == (UnityEngine.Object) null || (UnityEngine.Object) this.body == (UnityEngine.Object) output.body || ((UnityEngine.Object) this.connection == (UnityEngine.Object) output || !this.typeData.Type.IsAssignableFrom(output.typeData.Type)))
        return false;
      if (!output.body.isChildOf(this.body) || output.body.allowsLoopRecursion(this.body))
        return true;
      Debug.LogWarning((object) "Cannot apply connection: Recursion detected!");
      return false;
    }

    public void ApplyConnection(NodeOutput output)
    {
      if ((UnityEngine.Object) output == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) this.connection != (UnityEngine.Object) null)
      {
        NodeEditorCallbacks.IssueOnRemoveConnection(this);
        this.connection.connections.Remove(this);
      }
      this.connection = output;
      output.connections.Add(this);
      if (!output.body.calculated)
        NodeEditor.RecalculateFrom(output.body);
      else
        NodeEditor.RecalculateFrom(this.body);
      output.body.OnAddOutputConnection(output);
      this.body.OnAddInputConnection(this);
      NodeEditorCallbacks.IssueOnAddConnection(this);
    }

    public void RemoveConnection()
    {
      if ((UnityEngine.Object) this.connection == (UnityEngine.Object) null)
        return;
      NodeEditorCallbacks.IssueOnRemoveConnection(this);
      this.connection.connections.Remove(this);
      this.connection = (NodeOutput) null;
      NodeEditor.RecalculateFrom(this.body);
    }

    public override Node GetNodeAcrossConnection() => !((UnityEngine.Object) this.connection != (UnityEngine.Object) null) ? (Node) null : this.connection.body;
  }
}
