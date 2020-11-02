// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeEditor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework
{
  public static class NodeEditor
  {
    public static string editorPath = "Assets/Plugins/Node_Editor/";
    public static NodeCanvas curNodeCanvas;
    public static NodeEditorState curEditorState;
    internal static System.Action NEUpdate;
    public static System.Action ClientRepaints;
    public static bool initiated;
    public static bool InitiationError;
    public static List<Node> workList;
    private static int calculationCount;

    public static void Update()
    {
      if (NodeEditor.NEUpdate == null)
        return;
      NodeEditor.NEUpdate();
    }

    public static void RepaintClients()
    {
      if (NodeEditor.ClientRepaints == null)
        return;
      NodeEditor.ClientRepaints();
    }

    public static void checkInit(bool GUIFunction)
    {
      if (NodeEditor.initiated || NodeEditor.InitiationError)
        return;
      NodeEditor.ReInit(GUIFunction);
    }

    public static void ReInit(bool GUIFunction)
    {
      NodeEditor.CheckEditorPath();
      ResourceManager.SetDefaultResourcePath(NodeEditor.editorPath + "Resources/");
      if (!NodeEditorGUI.Init(GUIFunction))
      {
        NodeEditor.InitiationError = true;
      }
      else
      {
        ConnectionTypes.FetchTypes();
        NodeTypes.FetchNodes();
        NodeCanvasManager.GetAllCanvasTypes();
        NodeEditorCallbacks.SetupReceivers();
        NodeEditorCallbacks.IssueOnEditorStartUp();
        GUIScaleUtility.CheckInit();
        NodeEditorInputSystem.SetupInput();
        NodeEditor.initiated = GUIFunction;
      }
    }

    public static void CheckEditorPath()
    {
    }

    public static void DrawCanvas(NodeCanvas nodeCanvas, NodeEditorState editorState)
    {
      if (!editorState.drawing)
        return;
      NodeEditor.checkInit(true);
      NodeEditor.DrawSubCanvas(nodeCanvas, editorState);
    }

    private static void DrawSubCanvas(NodeCanvas nodeCanvas, NodeEditorState editorState)
    {
      if (!editorState.drawing)
        return;
      NodeCanvas curNodeCanvas = NodeEditor.curNodeCanvas;
      NodeEditorState curEditorState = NodeEditor.curEditorState;
      NodeEditor.curNodeCanvas = nodeCanvas;
      NodeEditor.curEditorState = editorState;
      if (UnityEngine.Event.current.type == EventType.Repaint)
      {
        float num1 = NodeEditor.curEditorState.zoom / (float) NodeEditorGUI.Background.width;
        float num2 = NodeEditor.curEditorState.zoom / (float) NodeEditorGUI.Background.height;
        Vector2 vector2 = NodeEditor.curEditorState.zoomPos + NodeEditor.curEditorState.panOffset / NodeEditor.curEditorState.zoom;
        Rect texCoords = new Rect(-vector2.x * num1, (vector2.y - NodeEditor.curEditorState.canvasRect.height) * num2, NodeEditor.curEditorState.canvasRect.width * num1, NodeEditor.curEditorState.canvasRect.height * num2);
        GUI.DrawTextureWithTexCoords(NodeEditor.curEditorState.canvasRect, (Texture) NodeEditorGUI.Background, texCoords);
      }
      NodeEditorInputSystem.HandleInputEvents(NodeEditor.curEditorState);
      if (UnityEngine.Event.current.type != EventType.Layout)
        NodeEditor.curEditorState.ignoreInput = new List<Rect>();
      Rect canvasRect = NodeEditor.curEditorState.canvasRect;
      NodeEditor.curEditorState.zoomPanAdjust = GUIScaleUtility.BeginScale(ref canvasRect, NodeEditor.curEditorState.zoomPos, NodeEditor.curEditorState.zoom, false);
      if (NodeEditor.curEditorState.navigate)
      {
        RTEditorGUI.DrawLine(((UnityEngine.Object) NodeEditor.curEditorState.selectedNode != (UnityEngine.Object) null ? NodeEditor.curEditorState.selectedNode.rect.center : NodeEditor.curEditorState.panOffset) + NodeEditor.curEditorState.zoomPanAdjust, UnityEngine.Event.current.mousePosition, Color.green, (Texture2D) null, 3f);
        NodeEditor.RepaintClients();
      }
      if ((UnityEngine.Object) NodeEditor.curEditorState.connectOutput != (UnityEngine.Object) null)
      {
        NodeOutput connectOutput = NodeEditor.curEditorState.connectOutput;
        Vector2 center = connectOutput.GetGUIKnob().center;
        Vector2 direction = connectOutput.GetDirection();
        Vector2 mousePosition = UnityEngine.Event.current.mousePosition;
        Vector2 connectionVector = NodeEditorGUI.GetSecondConnectionVector(center, mousePosition, direction);
        NodeEditorGUI.DrawConnection(center, direction, mousePosition, connectionVector, connectOutput.typeData.Color);
        NodeEditor.RepaintClients();
      }
      if (UnityEngine.Event.current.type == EventType.Layout && (UnityEngine.Object) NodeEditor.curEditorState.selectedNode != (UnityEngine.Object) null)
      {
        NodeEditor.curNodeCanvas.nodes.Remove(NodeEditor.curEditorState.selectedNode);
        NodeEditor.curNodeCanvas.nodes.Add(NodeEditor.curEditorState.selectedNode);
      }
      for (int index = 0; index < NodeEditor.curNodeCanvas.nodes.Count; ++index)
        NodeEditor.curNodeCanvas.nodes[index].DrawConnections();
      for (int index = 0; index < NodeEditor.curNodeCanvas.nodes.Count; ++index)
      {
        Node node = NodeEditor.curNodeCanvas.nodes[index];
        node.DrawNode();
        if (UnityEngine.Event.current.type == EventType.Repaint)
          node.DrawKnobs();
      }
      GUIScaleUtility.EndScale();
      NodeEditorInputSystem.HandleLateInputEvents(NodeEditor.curEditorState);
      NodeEditor.curNodeCanvas = curNodeCanvas;
      NodeEditor.curEditorState = curEditorState;
    }

    public static Node NodeAtPosition(Vector2 canvasPos) => NodeEditor.NodeAtPosition(NodeEditor.curEditorState, canvasPos, out NodeKnob _);

    public static Node NodeAtPosition(Vector2 canvasPos, out NodeKnob focusedKnob) => NodeEditor.NodeAtPosition(NodeEditor.curEditorState, canvasPos, out focusedKnob);

    public static Node NodeAtPosition(
      NodeEditorState editorState,
      Vector2 canvasPos,
      out NodeKnob focusedKnob)
    {
      focusedKnob = (NodeKnob) null;
      if (NodeEditorInputSystem.shouldIgnoreInput(editorState))
        return (Node) null;
      NodeCanvas canvas = editorState.canvas;
      for (int index1 = canvas.nodes.Count - 1; index1 >= 0; --index1)
      {
        Node node = canvas.nodes[index1];
        if (node.rect.Contains(canvasPos))
          return node;
        for (int index2 = 0; index2 < node.nodeKnobs.Count; ++index2)
        {
          if (node.nodeKnobs[index2].GetCanvasSpaceKnob().Contains(canvasPos))
          {
            focusedKnob = node.nodeKnobs[index2];
            return node;
          }
        }
      }
      return (Node) null;
    }

    public static Vector2 ScreenToCanvasSpace(Vector2 screenPos) => NodeEditor.ScreenToCanvasSpace(NodeEditor.curEditorState, screenPos);

    public static Vector2 ScreenToCanvasSpace(NodeEditorState editorState, Vector2 screenPos) => (screenPos - editorState.canvasRect.position - editorState.zoomPos) * editorState.zoom - editorState.panOffset;

    public static void RecalculateAll(NodeCanvas nodeCanvas)
    {
      NodeEditor.workList = new List<Node>();
      foreach (Node node in nodeCanvas.nodes)
      {
        if (node.isInput())
        {
          node.ClearCalculation();
          NodeEditor.workList.Add(node);
        }
      }
      NodeEditor.StartCalculation();
    }

    public static void RecalculateFrom(Node node)
    {
      node.ClearCalculation();
      NodeEditor.workList = new List<Node>()
      {
        node
      };
      NodeEditor.StartCalculation();
    }

    public static void StartCalculation()
    {
      NodeEditor.checkInit(false);
      if (NodeEditor.InitiationError || NodeEditor.workList == null || NodeEditor.workList.Count == 0)
        return;
      NodeEditor.calculationCount = 0;
      bool flag = false;
      int num = 0;
      while (!flag)
      {
        flag = true;
        for (int index = 0; index < NodeEditor.workList.Count; ++index)
        {
          if (NodeEditor.ContinueCalculation(NodeEditor.workList[index]))
            flag = false;
        }
        ++num;
      }
    }

    private static bool ContinueCalculation(Node node)
    {
      if (node.calculated)
        return false;
      if ((node.descendantsCalculated() || node.isInLoop()) && node.Calculate())
      {
        node.calculated = true;
        ++NodeEditor.calculationCount;
        NodeEditor.workList.Remove(node);
        if (node.ContinueCalculation && NodeEditor.calculationCount < 1000)
        {
          for (int index1 = 0; index1 < node.Outputs.Count; ++index1)
          {
            NodeOutput output = node.Outputs[index1];
            if (!output.calculationBlockade)
            {
              for (int index2 = 0; index2 < output.connections.Count; ++index2)
                NodeEditor.ContinueCalculation(output.connections[index2].body);
            }
          }
        }
        else if (NodeEditor.calculationCount >= 1000)
          Debug.LogError((object) "Stopped calculation because of suspected Recursion. Maximum calculation iteration is currently at 1000!");
        return true;
      }
      if (!NodeEditor.workList.Contains(node))
        NodeEditor.workList.Add(node);
      return false;
    }
  }
}
