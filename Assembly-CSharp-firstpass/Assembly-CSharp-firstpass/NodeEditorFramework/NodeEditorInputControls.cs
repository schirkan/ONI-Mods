// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeEditorInputControls
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
  public static class NodeEditorInputControls
  {
    [ContextFiller(ContextType.Canvas)]
    private static void FillAddNodes(NodeEditorInputInfo inputInfo, GenericMenu canvasContextMenu)
    {
      NodeEditorState editorState = inputInfo.editorState;
      List<Node> displayedNodes = (UnityEngine.Object) editorState.connectOutput != (UnityEngine.Object) null ? NodeTypes.getCompatibleNodes(editorState.connectOutput) : NodeTypes.nodes.Keys.ToList<Node>();
      NodeEditorInputControls.DeCafList(ref displayedNodes, editorState.canvas);
      foreach (Node key in displayedNodes)
        canvasContextMenu.AddItem(new GUIContent("Add " + NodeTypes.nodes[key].adress), false, new PopupMenu.MenuFunctionData(NodeEditorInputControls.CreateNodeCallback), (object) new NodeEditorInputInfo(key.GetID, editorState));
    }

    private static void DeCafList(ref List<Node> displayedNodes, NodeCanvas canvas)
    {
      for (int index = 0; index < displayedNodes.Count; ++index)
      {
        if (!((IEnumerable<System.Type>) NodeTypes.nodes[displayedNodes[index]].typeOfNodeCanvas).Contains<System.Type>(canvas.GetType()))
        {
          displayedNodes.RemoveAt(index);
          --index;
        }
      }
    }

    private static void CreateNodeCallback(object infoObj)
    {
      if (!(infoObj is NodeEditorInputInfo nodeEditorInputInfo))
        throw new UnityException("Callback Object passed by context is not of type NodeEditorInputInfo!");
      nodeEditorInputInfo.SetAsCurrentEnvironment();
      Node.Create(nodeEditorInputInfo.message, NodeEditor.ScreenToCanvasSpace(nodeEditorInputInfo.inputPos), nodeEditorInputInfo.editorState.connectOutput);
      nodeEditorInputInfo.editorState.connectOutput = (NodeOutput) null;
      NodeEditor.RepaintClients();
    }

    [ContextEntry(ContextType.Node, "Delete Node")]
    private static void DeleteNode(NodeEditorInputInfo inputInfo)
    {
      inputInfo.SetAsCurrentEnvironment();
      if (!((UnityEngine.Object) inputInfo.editorState.focusedNode != (UnityEngine.Object) null))
        return;
      inputInfo.editorState.focusedNode.Delete();
      inputInfo.inputEvent.Use();
    }

    [ContextEntry(ContextType.Node, "Duplicate Node")]
    private static void DuplicateNode(NodeEditorInputInfo inputInfo)
    {
      inputInfo.SetAsCurrentEnvironment();
      NodeEditorState editorState = inputInfo.editorState;
      if (!((UnityEngine.Object) editorState.focusedNode != (UnityEngine.Object) null))
        return;
      Node node = Node.Create(editorState.focusedNode.GetID, NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos), editorState.connectOutput);
      editorState.selectedNode = editorState.focusedNode = node;
      editorState.connectOutput = (NodeOutput) null;
      inputInfo.inputEvent.Use();
    }

    [Hotkey(KeyCode.UpArrow, EventType.KeyDown)]
    [Hotkey(KeyCode.LeftArrow, EventType.KeyDown)]
    [Hotkey(KeyCode.RightArrow, EventType.KeyDown)]
    [Hotkey(KeyCode.DownArrow, EventType.KeyDown)]
    private static void KB_MoveNode(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if ((UnityEngine.Object) editorState.selectedNode != (UnityEngine.Object) null)
      {
        Vector2 vector2 = editorState.selectedNode.rect.position;
        int num = !inputInfo.inputEvent.shift ? 5 : 10;
        if (inputInfo.inputEvent.keyCode == KeyCode.RightArrow)
          vector2 = new Vector2(vector2.x + (float) num, vector2.y);
        else if (inputInfo.inputEvent.keyCode == KeyCode.LeftArrow)
          vector2 = new Vector2(vector2.x - (float) num, vector2.y);
        else if (inputInfo.inputEvent.keyCode == KeyCode.DownArrow)
          vector2 = new Vector2(vector2.x, vector2.y + (float) num);
        else if (inputInfo.inputEvent.keyCode == KeyCode.UpArrow)
          vector2 = new Vector2(vector2.x, vector2.y - (float) num);
        editorState.selectedNode.rect.position = vector2;
        inputInfo.inputEvent.Use();
      }
      NodeEditor.RepaintClients();
    }

    [EventHandler(EventType.MouseDown, 110)]
    private static void HandleNodeDraggingStart(NodeEditorInputInfo inputInfo)
    {
      if (GUIUtility.hotControl > 0)
        return;
      NodeEditorState editorState = inputInfo.editorState;
      if (inputInfo.inputEvent.button != 0 || !((UnityEngine.Object) editorState.focusedNode != (UnityEngine.Object) null) || (!((UnityEngine.Object) editorState.focusedNode == (UnityEngine.Object) editorState.selectedNode) || !((UnityEngine.Object) editorState.focusedNodeKnob == (UnityEngine.Object) null)))
        return;
      editorState.dragNode = true;
      editorState.dragStart = inputInfo.inputPos;
      editorState.dragPos = editorState.focusedNode.rect.position;
      editorState.dragOffset = Vector2.zero;
      inputInfo.inputEvent.delta = Vector2.zero;
    }

    [EventHandler(EventType.MouseDrag)]
    private static void HandleNodeDragging(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if (!editorState.dragNode)
        return;
      if ((UnityEngine.Object) editorState.selectedNode != (UnityEngine.Object) null && GUIUtility.hotControl == 0)
      {
        editorState.dragOffset = inputInfo.inputPos - editorState.dragStart;
        editorState.selectedNode.rect.position = editorState.dragPos + editorState.dragOffset * editorState.zoom;
        NodeEditorCallbacks.IssueOnMoveNode(editorState.selectedNode);
        NodeEditor.RepaintClients();
      }
      else
        editorState.dragNode = false;
    }

    [EventHandler(EventType.MouseDown)]
    [EventHandler(EventType.MouseUp)]
    private static void HandleNodeDraggingEnd(NodeEditorInputInfo inputInfo) => inputInfo.editorState.dragNode = false;

    [EventHandler(EventType.MouseDown, 100)]
    private static void HandleWindowPanningStart(NodeEditorInputInfo inputInfo)
    {
      if (GUIUtility.hotControl > 0)
        return;
      NodeEditorState editorState = inputInfo.editorState;
      if (inputInfo.inputEvent.button != 0 && inputInfo.inputEvent.button != 2 || !((UnityEngine.Object) editorState.focusedNode == (UnityEngine.Object) null))
        return;
      editorState.panWindow = true;
      editorState.dragStart = inputInfo.inputPos;
      editorState.dragOffset = Vector2.zero;
    }

    [EventHandler(EventType.MouseDrag)]
    private static void HandleWindowPanning(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if (!editorState.panWindow)
        return;
      Vector2 dragOffset = editorState.dragOffset;
      editorState.dragOffset = inputInfo.inputPos - editorState.dragStart;
      Vector2 vector2 = (editorState.dragOffset - dragOffset) * editorState.zoom;
      editorState.panOffset += vector2;
      NodeEditor.RepaintClients();
    }

    [EventHandler(EventType.MouseDown)]
    [EventHandler(EventType.MouseUp)]
    private static void HandleWindowPanningEnd(NodeEditorInputInfo inputInfo) => inputInfo.editorState.panWindow = false;

    [EventHandler(EventType.MouseDown)]
    private static void HandleConnectionDrawing(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if (inputInfo.inputEvent.button != 0 || !((UnityEngine.Object) editorState.focusedNodeKnob != (UnityEngine.Object) null))
        return;
      if (editorState.focusedNodeKnob is NodeOutput)
      {
        editorState.connectOutput = (NodeOutput) editorState.focusedNodeKnob;
        inputInfo.inputEvent.Use();
      }
      else
      {
        if (!(editorState.focusedNodeKnob is NodeInput))
          return;
        NodeInput focusedNodeKnob = (NodeInput) editorState.focusedNodeKnob;
        if (!((UnityEngine.Object) focusedNodeKnob.connection != (UnityEngine.Object) null))
          return;
        editorState.connectOutput = focusedNodeKnob.connection;
        focusedNodeKnob.RemoveConnection();
        inputInfo.inputEvent.Use();
      }
    }

    [EventHandler(EventType.MouseUp)]
    private static void HandleApplyConnection(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if (inputInfo.inputEvent.button == 0 && (UnityEngine.Object) editorState.connectOutput != (UnityEngine.Object) null && ((UnityEngine.Object) editorState.focusedNode != (UnityEngine.Object) null && (UnityEngine.Object) editorState.focusedNodeKnob != (UnityEngine.Object) null) && editorState.focusedNodeKnob is NodeInput)
      {
        (editorState.focusedNodeKnob as NodeInput).TryApplyConnection(editorState.connectOutput);
        inputInfo.inputEvent.Use();
      }
      editorState.connectOutput = (NodeOutput) null;
    }

    [EventHandler(EventType.ScrollWheel)]
    private static void HandleZooming(NodeEditorInputInfo inputInfo)
    {
      inputInfo.editorState.zoom = (float) Math.Round(Math.Min(2.0, Math.Max(0.6, (double) inputInfo.editorState.zoom + (double) inputInfo.inputEvent.delta.y / 15.0)), 2);
      NodeEditor.RepaintClients();
    }

    [Hotkey(KeyCode.N, EventType.KeyDown)]
    private static void HandleStartNavigating(NodeEditorInputInfo inputInfo) => inputInfo.editorState.navigate = true;

    [Hotkey(KeyCode.N, EventType.KeyUp)]
    private static void HandleEndNavigating(NodeEditorInputInfo inputInfo) => inputInfo.editorState.navigate = false;

    [Hotkey(KeyCode.LeftControl, EventType.KeyDown, 60)]
    [Hotkey(KeyCode.LeftControl, EventType.KeyUp, 60)]
    private static void HandleNodeSnap(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if ((UnityEngine.Object) editorState.selectedNode != (UnityEngine.Object) null)
      {
        Vector2 vector2 = editorState.selectedNode.rect.position;
        vector2 = new Vector2((float) (Mathf.RoundToInt(vector2.x / 10f) * 10), (float) (Mathf.RoundToInt(vector2.y / 10f) * 10));
        editorState.selectedNode.rect.position = vector2;
        inputInfo.inputEvent.Use();
      }
      NodeEditor.RepaintClients();
    }
  }
}
