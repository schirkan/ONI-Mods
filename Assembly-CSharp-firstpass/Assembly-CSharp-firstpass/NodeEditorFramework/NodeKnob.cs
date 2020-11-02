// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeKnob
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework
{
  [Serializable]
  public class NodeKnob : ScriptableObject
  {
    public Node body;
    [NonSerialized]
    protected internal Texture2D knobTexture;
    public NodeSide side;
    public float sidePosition;
    public float sideOffset;

    protected virtual GUIStyle defaultLabelStyle => GUI.skin.label;

    protected virtual NodeSide defaultSide => NodeSide.Right;

    protected void InitBase(
      Node nodeBody,
      NodeSide nodeSide,
      float nodeSidePosition,
      string knobName)
    {
      this.body = nodeBody;
      this.side = nodeSide;
      this.sidePosition = nodeSidePosition;
      this.name = knobName;
      nodeBody.nodeKnobs.Add(this);
      this.ReloadKnobTexture();
    }

    public virtual void Delete()
    {
      this.body.nodeKnobs.Remove(this);
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this, true);
    }

    internal void Check()
    {
      if (this.side == (NodeSide) 0)
        this.side = this.defaultSide;
      if (!((UnityEngine.Object) this.knobTexture == (UnityEngine.Object) null))
        return;
      this.ReloadKnobTexture();
    }

    protected void ReloadKnobTexture()
    {
      this.ReloadTexture();
      if ((UnityEngine.Object) this.knobTexture == (UnityEngine.Object) null)
        throw new UnityException("Knob texture of " + this.name + " could not be loaded!");
      if (this.side == this.defaultSide)
        return;
      ResourceManager.SetDefaultResourcePath(NodeEditor.editorPath + "Resources/");
      int rotationStepsAntiCw = NodeKnob.getRotationStepsAntiCW(this.defaultSide, this.side);
      ResourceManager.MemoryTexture inMemory = ResourceManager.FindInMemory(this.knobTexture);
      if (inMemory != null)
      {
        string[] strArray = new string[inMemory.modifications.Length + 1];
        inMemory.modifications.CopyTo((Array) strArray, 0);
        strArray[strArray.Length - 1] = "Rotation:" + (object) rotationStepsAntiCw;
        Texture2D texture = ResourceManager.GetTexture(inMemory.path, strArray);
        if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
        {
          this.knobTexture = texture;
        }
        else
        {
          this.knobTexture = RTEditorGUI.RotateTextureCCW(this.knobTexture, rotationStepsAntiCw);
          ResourceManager.AddTextureToMemory(inMemory.path, this.knobTexture, ((IEnumerable<string>) strArray).ToArray<string>());
        }
      }
      else
        this.knobTexture = RTEditorGUI.RotateTextureCCW(this.knobTexture, rotationStepsAntiCw);
    }

    protected virtual void ReloadTexture() => this.knobTexture = RTEditorGUI.ColorToTex(1, Color.red);

    public virtual ScriptableObject[] GetScriptableObjects() => new ScriptableObject[0];

    protected internal virtual void CopyScriptableObjects(
      Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
    {
    }

    public virtual void DrawKnob() => GUI.DrawTexture(this.GetGUIKnob(), (Texture) this.knobTexture);

    public void DisplayLayout() => this.DisplayLayout(new GUIContent(this.name), this.defaultLabelStyle);

    public void DisplayLayout(GUIStyle style) => this.DisplayLayout(new GUIContent(this.name), style);

    public void DisplayLayout(GUIContent content) => this.DisplayLayout(content, this.defaultLabelStyle);

    public void DisplayLayout(GUIContent content, GUIStyle style)
    {
      GUILayout.Label(content, style, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      if (Event.current.type != EventType.Repaint)
        return;
      this.SetPosition();
    }

    public void SetPosition(float position, NodeSide nodeSide)
    {
      if (this.side != nodeSide)
      {
        this.side = nodeSide;
        this.ReloadKnobTexture();
      }
      this.SetPosition(position);
    }

    public void SetPosition(float position) => this.sidePosition = position;

    public void SetPosition()
    {
      Vector2 vector2 = GUILayoutUtility.GetLastRect().center + this.body.contentOffset;
      this.sidePosition = this.side == NodeSide.Bottom || this.side == NodeSide.Top ? vector2.x : vector2.y;
    }

    public Rect GetGUIKnob()
    {
      Rect canvasSpaceKnob = this.GetCanvasSpaceKnob();
      canvasSpaceKnob.position += NodeEditor.curEditorState.zoomPanAdjust + NodeEditor.curEditorState.panOffset;
      return canvasSpaceKnob;
    }

    public Rect GetCanvasSpaceKnob()
    {
      this.Check();
      Vector2 knobSize = new Vector2((float) (this.knobTexture.width / this.knobTexture.height * NodeEditorGUI.knobSize), (float) (this.knobTexture.height / this.knobTexture.width * NodeEditorGUI.knobSize));
      Vector2 knobCenter = this.GetKnobCenter(knobSize);
      return new Rect(knobCenter.x - knobSize.x / 2f, knobCenter.y - knobSize.y / 2f, knobSize.x, knobSize.y);
    }

    private Vector2 GetKnobCenter(Vector2 knobSize)
    {
      if (this.side == NodeSide.Left)
        return this.body.rect.position + new Vector2((float) (-(double) this.sideOffset - (double) knobSize.x / 2.0), this.sidePosition);
      if (this.side == NodeSide.Right)
        return this.body.rect.position + new Vector2(this.sideOffset + knobSize.x / 2f + this.body.rect.width, this.sidePosition);
      return this.side == NodeSide.Bottom ? this.body.rect.position + new Vector2(this.sidePosition, this.sideOffset + knobSize.y / 2f + this.body.rect.height) : this.body.rect.position + new Vector2(this.sidePosition, (float) (-(double) this.sideOffset - (double) knobSize.y / 2.0));
    }

    public Vector2 GetDirection()
    {
      if (this.side == NodeSide.Right)
        return Vector2.right;
      if (this.side == NodeSide.Bottom)
        return Vector2.up;
      return this.side != NodeSide.Top ? Vector2.left : Vector2.down;
    }

    private static int getRotationStepsAntiCW(NodeSide sideA, NodeSide sideB) => sideB - sideA + (sideA > sideB ? 4 : 0);

    public virtual Node GetNodeAcrossConnection() => (Node) null;
  }
}
