// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeEditorCallbacks
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace NodeEditorFramework
{
  public static class NodeEditorCallbacks
  {
    private static int receiverCount;
    private static List<NodeEditorCallbackReceiver> callbackReceiver;
    public static Action OnEditorStartUp;
    public static Action<NodeCanvas> OnLoadCanvas;
    public static Action<NodeEditorState> OnLoadEditorState;
    public static Action<NodeCanvas> OnSaveCanvas;
    public static Action<NodeEditorState> OnSaveEditorState;
    public static Action<Node> OnAddNode;
    public static Action<Node> OnDeleteNode;
    public static Action<Node> OnMoveNode;
    public static Action<NodeKnob> OnAddNodeKnob;
    public static Action<NodeInput> OnAddConnection;
    public static Action<NodeInput> OnRemoveConnection;

    public static void SetupReceivers()
    {
      NodeEditorCallbacks.callbackReceiver = new List<NodeEditorCallbackReceiver>((IEnumerable<NodeEditorCallbackReceiver>) UnityEngine.Object.FindObjectsOfType<NodeEditorCallbackReceiver>());
      NodeEditorCallbacks.receiverCount = NodeEditorCallbacks.callbackReceiver.Count;
    }

    public static void IssueOnEditorStartUp()
    {
      if (NodeEditorCallbacks.OnEditorStartUp != null)
        NodeEditorCallbacks.OnEditorStartUp();
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnEditorStartUp();
      }
    }

    public static void IssueOnLoadCanvas(NodeCanvas canvas)
    {
      if (NodeEditorCallbacks.OnLoadCanvas != null)
        NodeEditorCallbacks.OnLoadCanvas(canvas);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnLoadCanvas(canvas);
      }
    }

    public static void IssueOnLoadEditorState(NodeEditorState editorState)
    {
      if (NodeEditorCallbacks.OnLoadEditorState != null)
        NodeEditorCallbacks.OnLoadEditorState(editorState);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnLoadEditorState(editorState);
      }
    }

    public static void IssueOnSaveCanvas(NodeCanvas canvas)
    {
      if (NodeEditorCallbacks.OnSaveCanvas != null)
        NodeEditorCallbacks.OnSaveCanvas(canvas);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnSaveCanvas(canvas);
      }
    }

    public static void IssueOnSaveEditorState(NodeEditorState editorState)
    {
      if (NodeEditorCallbacks.OnSaveEditorState != null)
        NodeEditorCallbacks.OnSaveEditorState(editorState);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnSaveEditorState(editorState);
      }
    }

    public static void IssueOnAddNode(Node node)
    {
      if (NodeEditorCallbacks.OnAddNode != null)
        NodeEditorCallbacks.OnAddNode(node);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnAddNode(node);
      }
    }

    public static void IssueOnDeleteNode(Node node)
    {
      if (NodeEditorCallbacks.OnDeleteNode != null)
        NodeEditorCallbacks.OnDeleteNode(node);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
        {
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        }
        else
        {
          NodeEditorCallbacks.callbackReceiver[index].OnDeleteNode(node);
          node.OnDelete();
        }
      }
    }

    public static void IssueOnMoveNode(Node node)
    {
      if (NodeEditorCallbacks.OnMoveNode != null)
        NodeEditorCallbacks.OnMoveNode(node);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnMoveNode(node);
      }
    }

    public static void IssueOnAddNodeKnob(NodeKnob nodeKnob)
    {
      if (NodeEditorCallbacks.OnAddNodeKnob != null)
        NodeEditorCallbacks.OnAddNodeKnob(nodeKnob);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnAddNodeKnob(nodeKnob);
      }
    }

    public static void IssueOnAddConnection(NodeInput input)
    {
      if (NodeEditorCallbacks.OnAddConnection != null)
        NodeEditorCallbacks.OnAddConnection(input);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnAddConnection(input);
      }
    }

    public static void IssueOnRemoveConnection(NodeInput input)
    {
      if (NodeEditorCallbacks.OnRemoveConnection != null)
        NodeEditorCallbacks.OnRemoveConnection(input);
      for (int index = 0; index < NodeEditorCallbacks.receiverCount; ++index)
      {
        if ((UnityEngine.Object) NodeEditorCallbacks.callbackReceiver[index] == (UnityEngine.Object) null)
          NodeEditorCallbacks.callbackReceiver.RemoveAt(index--);
        else
          NodeEditorCallbacks.callbackReceiver[index].OnRemoveConnection(input);
      }
    }
  }
}
