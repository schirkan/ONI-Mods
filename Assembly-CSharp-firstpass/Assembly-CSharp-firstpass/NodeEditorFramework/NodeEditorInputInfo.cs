// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeEditorInputInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace NodeEditorFramework
{
  public class NodeEditorInputInfo
  {
    public string message;
    public NodeEditorState editorState;
    public Event inputEvent;
    public Vector2 inputPos;

    public NodeEditorInputInfo(NodeEditorState EditorState)
    {
      this.message = (string) null;
      this.editorState = EditorState;
      this.inputEvent = Event.current;
      this.inputPos = this.inputEvent.mousePosition;
    }

    public NodeEditorInputInfo(string Message, NodeEditorState EditorState)
    {
      this.message = Message;
      this.editorState = EditorState;
      this.inputEvent = Event.current;
      this.inputPos = this.inputEvent.mousePosition;
    }

    public void SetAsCurrentEnvironment()
    {
      NodeEditor.curEditorState = this.editorState;
      NodeEditor.curNodeCanvas = this.editorState.canvas;
    }
  }
}
