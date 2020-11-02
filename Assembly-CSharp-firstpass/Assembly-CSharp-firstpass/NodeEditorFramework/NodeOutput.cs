// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeOutput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace NodeEditorFramework
{
  public class NodeOutput : NodeKnob
  {
    private static GUIStyle _defaultStyle;
    public List<NodeInput> connections = new List<NodeInput>();
    [FormerlySerializedAs("type")]
    public string typeID;
    private TypeData _typeData;
    [NonSerialized]
    private object value;
    public bool calculationBlockade;

    protected override NodeSide defaultSide => NodeSide.Right;

    protected override GUIStyle defaultLabelStyle
    {
      get
      {
        if (NodeOutput._defaultStyle == null)
        {
          NodeOutput._defaultStyle = new GUIStyle(GUI.skin.label);
          NodeOutput._defaultStyle.alignment = TextAnchor.MiddleRight;
        }
        return NodeOutput._defaultStyle;
      }
    }

    internal TypeData typeData
    {
      get
      {
        this.CheckType();
        return this._typeData;
      }
    }

    public static NodeOutput Create(Node nodeBody, string outputName, string outputType) => NodeOutput.Create(nodeBody, outputName, outputType, NodeSide.Right, 20f);

    public static NodeOutput Create(
      Node nodeBody,
      string outputName,
      string outputType,
      NodeSide nodeSide)
    {
      return NodeOutput.Create(nodeBody, outputName, outputType, nodeSide, 20f);
    }

    public static NodeOutput Create(
      Node nodeBody,
      string outputName,
      string outputType,
      NodeSide nodeSide,
      float sidePosition)
    {
      NodeOutput instance = ScriptableObject.CreateInstance<NodeOutput>();
      instance.typeID = outputType;
      instance.InitBase(nodeBody, nodeSide, sidePosition, outputName);
      nodeBody.Outputs.Add(instance);
      return instance;
    }

    public override void Delete()
    {
      while (this.connections.Count > 0)
        this.connections[0].RemoveConnection();
      this.body.Outputs.Remove(this);
      base.Delete();
    }

    protected internal override void CopyScriptableObjects(
      Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
    {
      for (int index = 0; index < this.connections.Count; ++index)
        this.connections[index] = replaceSerializableObject((ScriptableObject) this.connections[index]) as NodeInput;
    }

    protected override void ReloadTexture()
    {
      this.CheckType();
      this.knobTexture = this.typeData.OutKnobTex;
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

    public bool IsValueNull => this.value == null;

    public object GetValue() => this.value;

    public object GetValue(System.Type type)
    {
      if (type == (System.Type) null)
        throw new UnityException("Trying to get value of " + this.name + " with null type!");
      this.CheckType();
      if (type.IsAssignableFrom(this.typeData.Type))
        return this.value;
      Debug.LogError((object) ("Trying to GetValue<" + type.FullName + "> for Output Type: " + this.typeData.Type.FullName));
      return (object) null;
    }

    public void SetValue(object Value)
    {
      this.CheckType();
      if (Value == null || this.typeData.Type.IsAssignableFrom(Value.GetType()))
        this.value = Value;
      else
        Debug.LogError((object) ("Trying to SetValue of type " + Value.GetType().FullName + " for Output Type: " + this.typeData.Type.FullName));
    }

    public T GetValue<T>()
    {
      this.CheckType();
      if (typeof (T).IsAssignableFrom(this.typeData.Type))
        return (T) (this.value ?? (this.value = (object) NodeOutput.GetDefault<T>()));
      Debug.LogError((object) ("Trying to GetValue<" + typeof (T).FullName + "> for Output Type: " + this.typeData.Type.FullName));
      return NodeOutput.GetDefault<T>();
    }

    public void SetValue<T>(T Value)
    {
      this.CheckType();
      if (this.typeData.Type.IsAssignableFrom(typeof (T)))
        this.value = (object) Value;
      else
        Debug.LogError((object) ("Trying to SetValue<" + typeof (T).FullName + "> for Output Type: " + this.typeData.Type.FullName));
    }

    public void ResetValue() => this.value = (object) null;

    public static T GetDefault<T>() => typeof (T).GetConstructor(System.Type.EmptyTypes) != (ConstructorInfo) null ? Activator.CreateInstance<T>() : default (T);

    public static object GetDefault(System.Type type) => type.GetConstructor(System.Type.EmptyTypes) != (ConstructorInfo) null ? Activator.CreateInstance(type) : (object) null;

    public override Node GetNodeAcrossConnection() => this.connections.Count <= 0 ? (Node) null : this.connections[0].body;
  }
}
