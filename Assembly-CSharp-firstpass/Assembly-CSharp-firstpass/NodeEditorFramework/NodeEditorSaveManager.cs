// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeEditorSaveManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework
{
  public static class NodeEditorSaveManager
  {
    private static GameObject sceneSaveHolder;

    private static void FetchSceneSaveHolder()
    {
      if (!((UnityEngine.Object) NodeEditorSaveManager.sceneSaveHolder == (UnityEngine.Object) null))
        return;
      NodeEditorSaveManager.sceneSaveHolder = GameObject.Find("NodeEditor_SceneSaveHolder");
      if ((UnityEngine.Object) NodeEditorSaveManager.sceneSaveHolder == (UnityEngine.Object) null)
        NodeEditorSaveManager.sceneSaveHolder = new GameObject("NodeEditor_SceneSaveHolder");
      NodeEditorSaveManager.sceneSaveHolder.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    }

    public static string[] GetSceneSaves()
    {
      NodeEditorSaveManager.FetchSceneSaveHolder();
      return ((IEnumerable<NodeCanvasSceneSave>) NodeEditorSaveManager.sceneSaveHolder.GetComponents<NodeCanvasSceneSave>()).Select<NodeCanvasSceneSave, string>((Func<NodeCanvasSceneSave, string>) (save => save.savedNodeCanvas.name)).ToArray<string>();
    }

    private static NodeCanvasSceneSave FindSceneSave(string saveName)
    {
      NodeEditorSaveManager.FetchSceneSaveHolder();
      return ((IEnumerable<NodeCanvasSceneSave>) NodeEditorSaveManager.sceneSaveHolder.GetComponents<NodeCanvasSceneSave>()).ToList<NodeCanvasSceneSave>().Find((Predicate<NodeCanvasSceneSave>) (save => save.savedNodeCanvas.name == saveName));
    }

    public static void SaveSceneNodeCanvas(
      string saveName,
      ref NodeCanvas nodeCanvas,
      bool createWorkingCopy)
    {
      if (string.IsNullOrEmpty(saveName))
      {
        Debug.LogError((object) "Cannot save Canvas to scene: No save name specified!");
      }
      else
      {
        nodeCanvas.livesInScene = true;
        nodeCanvas.name = saveName;
        NodeCanvasSceneSave nodeCanvasSceneSave = NodeEditorSaveManager.FindSceneSave(saveName);
        if ((UnityEngine.Object) nodeCanvasSceneSave == (UnityEngine.Object) null)
          nodeCanvasSceneSave = NodeEditorSaveManager.sceneSaveHolder.AddComponent<NodeCanvasSceneSave>();
        nodeCanvasSceneSave.savedNodeCanvas = nodeCanvas;
        if (!createWorkingCopy)
          return;
        nodeCanvasSceneSave.savedNodeCanvas = NodeEditorSaveManager.CreateWorkingCopy(nodeCanvasSceneSave.savedNodeCanvas, true);
        NodeEditorSaveManager.Compress(ref nodeCanvasSceneSave.savedNodeCanvas);
      }
    }

    public static NodeCanvas LoadSceneNodeCanvas(string saveName, bool createWorkingCopy)
    {
      if (string.IsNullOrEmpty(saveName))
      {
        Debug.LogError((object) "Cannot load Canvas from scene: No save name specified!");
        return (NodeCanvas) null;
      }
      NodeCanvasSceneSave sceneSave = NodeEditorSaveManager.FindSceneSave(saveName);
      if ((UnityEngine.Object) sceneSave == (UnityEngine.Object) null)
        return (NodeCanvas) null;
      NodeCanvas nodeCanvas = sceneSave.savedNodeCanvas;
      nodeCanvas.livesInScene = true;
      if (createWorkingCopy)
        nodeCanvas = NodeEditorSaveManager.CreateWorkingCopy(nodeCanvas, true);
      NodeEditorSaveManager.Uncompress(ref nodeCanvas);
      return nodeCanvas;
    }

    public static void SaveNodeCanvas(string path, NodeCanvas nodeCanvas, bool createWorkingCopy) => throw new NotImplementedException();

    public static NodeCanvas LoadNodeCanvas(string path, bool createWorkingCopy)
    {
      NodeCanvas nodeCanvas = File.Exists(path) ? ResourceManager.LoadResource<NodeCanvas>(path) : throw new UnityException("Cannot Load NodeCanvas: File '" + path + "' deos not exist!");
      if ((UnityEngine.Object) nodeCanvas == (UnityEngine.Object) null)
        throw new UnityException("Cannot Load NodeCanvas: The file at the specified path '" + path + "' is no valid save file as it does not contain a NodeCanvas!");
      if (createWorkingCopy)
        nodeCanvas = NodeEditorSaveManager.CreateWorkingCopy(nodeCanvas, true);
      else
        nodeCanvas.Validate();
      NodeEditorSaveManager.Uncompress(ref nodeCanvas);
      NodeEditorCallbacks.IssueOnLoadCanvas(nodeCanvas);
      return nodeCanvas;
    }

    public static void Compress(ref NodeCanvas nodeCanvas)
    {
    }

    public static void Uncompress(ref NodeCanvas nodeCanvas)
    {
      for (int index1 = 0; index1 < nodeCanvas.nodes.Count; ++index1)
      {
        Node node = nodeCanvas.nodes[index1];
        if (node.Inputs == null || node.Inputs.Count == 0 || (node.Outputs == null || node.Outputs.Count == 0))
        {
          node.Inputs = new List<NodeInput>();
          node.Outputs = new List<NodeOutput>();
          for (int index2 = 0; index2 < node.nodeKnobs.Count; ++index2)
          {
            NodeKnob nodeKnob = node.nodeKnobs[index2];
            switch (nodeKnob)
            {
              case NodeInput _:
                node.Inputs.Add(nodeKnob as NodeInput);
                break;
              case NodeOutput _:
                node.Outputs.Add(nodeKnob as NodeOutput);
                break;
            }
          }
        }
      }
    }

    public static NodeCanvas CreateWorkingCopy(NodeCanvas nodeCanvas, bool editorStates)
    {
      nodeCanvas.Validate();
      nodeCanvas = NodeEditorSaveManager.Clone<NodeCanvas>(nodeCanvas);
      List<ScriptableObject> allSOs = new List<ScriptableObject>();
      List<ScriptableObject> clonedSOs = new List<ScriptableObject>();
      for (int index = 0; index < nodeCanvas.nodes.Count; ++index)
      {
        Node node1 = nodeCanvas.nodes[index];
        node1.CheckNodeKnobMigration();
        Node node2 = NodeEditorSaveManager.AddClonedSO<Node>(allSOs, clonedSOs, node1);
        NodeEditorSaveManager.AddClonedSOs(allSOs, clonedSOs, node2.GetScriptableObjects());
        foreach (NodeKnob nodeKnob in node2.nodeKnobs)
        {
          NodeEditorSaveManager.AddClonedSO<NodeKnob>(allSOs, clonedSOs, nodeKnob);
          NodeEditorSaveManager.AddClonedSOs(allSOs, clonedSOs, nodeKnob.GetScriptableObjects());
        }
      }
      for (int index1 = 0; index1 < nodeCanvas.nodes.Count; ++index1)
      {
        Node node1 = nodeCanvas.nodes[index1];
        Node node2 = nodeCanvas.nodes[index1] = NodeEditorSaveManager.ReplaceSO<Node>(allSOs, clonedSOs, node1);
        node2.CopyScriptableObjects((Func<ScriptableObject, ScriptableObject>) (so => NodeEditorSaveManager.ReplaceSO<ScriptableObject>(allSOs, clonedSOs, so)));
        for (int index2 = 0; index2 < node2.nodeKnobs.Count; ++index2)
        {
          NodeKnob nodeKnob = node2.nodeKnobs[index2] = NodeEditorSaveManager.ReplaceSO<NodeKnob>(allSOs, clonedSOs, node2.nodeKnobs[index2]);
          nodeKnob.body = node2;
          nodeKnob.CopyScriptableObjects((Func<ScriptableObject, ScriptableObject>) (so => NodeEditorSaveManager.ReplaceSO<ScriptableObject>(allSOs, clonedSOs, so)));
        }
        for (int index2 = 0; index2 < node2.Inputs.Count; ++index2)
          (node2.Inputs[index2] = NodeEditorSaveManager.ReplaceSO<NodeInput>(allSOs, clonedSOs, node2.Inputs[index2])).body = node2;
        for (int index2 = 0; index2 < node2.Outputs.Count; ++index2)
          (node2.Outputs[index2] = NodeEditorSaveManager.ReplaceSO<NodeOutput>(allSOs, clonedSOs, node2.Outputs[index2])).body = node2;
      }
      if (editorStates)
      {
        nodeCanvas.editorStates = NodeEditorSaveManager.CreateWorkingCopy(nodeCanvas.editorStates, nodeCanvas);
        foreach (NodeEditorState editorState in nodeCanvas.editorStates)
          editorState.selectedNode = NodeEditorSaveManager.ReplaceSO<Node>(allSOs, clonedSOs, editorState.selectedNode);
      }
      else
      {
        foreach (NodeEditorState editorState in nodeCanvas.editorStates)
          editorState.selectedNode = (Node) null;
      }
      return nodeCanvas;
    }

    private static NodeEditorState[] CreateWorkingCopy(
      NodeEditorState[] editorStates,
      NodeCanvas associatedNodeCanvas)
    {
      if (editorStates == null)
        return new NodeEditorState[0];
      editorStates = (NodeEditorState[]) editorStates.Clone();
      for (int index = 0; index < editorStates.Length; ++index)
      {
        if (!((UnityEngine.Object) editorStates[index] == (UnityEngine.Object) null))
        {
          NodeEditorState nodeEditorState = editorStates[index] = NodeEditorSaveManager.Clone<NodeEditorState>(editorStates[index]);
          if ((UnityEngine.Object) nodeEditorState == (UnityEngine.Object) null)
            Debug.LogError((object) ("Failed to create a working copy for an NodeEditorState during the loading process of " + associatedNodeCanvas.name + "!"));
          else
            nodeEditorState.canvas = associatedNodeCanvas;
        }
      }
      associatedNodeCanvas.editorStates = editorStates;
      return editorStates;
    }

    private static T Clone<T>(T SO) where T : ScriptableObject
    {
      string name = SO.name;
      SO = UnityEngine.Object.Instantiate<T>(SO);
      SO.name = name;
      return SO;
    }

    private static void AddClonedSOs(
      List<ScriptableObject> scriptableObjects,
      List<ScriptableObject> clonedScriptableObjects,
      ScriptableObject[] initialSOs)
    {
      scriptableObjects.AddRange((IEnumerable<ScriptableObject>) initialSOs);
      clonedScriptableObjects.AddRange(((IEnumerable<ScriptableObject>) initialSOs).Select<ScriptableObject, ScriptableObject>((Func<ScriptableObject, ScriptableObject>) (so => NodeEditorSaveManager.Clone<ScriptableObject>(so))));
    }

    private static T AddClonedSO<T>(
      List<ScriptableObject> scriptableObjects,
      List<ScriptableObject> clonedScriptableObjects,
      T initialSO)
      where T : ScriptableObject
    {
      if ((UnityEngine.Object) initialSO == (UnityEngine.Object) null)
        return default (T);
      scriptableObjects.Add((ScriptableObject) initialSO);
      T obj = NodeEditorSaveManager.Clone<T>(initialSO);
      clonedScriptableObjects.Add((ScriptableObject) obj);
      return obj;
    }

    private static T ReplaceSO<T>(
      List<ScriptableObject> scriptableObjects,
      List<ScriptableObject> clonedScriptableObjects,
      T initialSO)
      where T : ScriptableObject
    {
      if ((UnityEngine.Object) initialSO == (UnityEngine.Object) null)
        return default (T);
      int index = scriptableObjects.IndexOf((ScriptableObject) initialSO);
      if (index == -1)
        Debug.LogError((object) ("GetWorkingCopy: ScriptableObject " + initialSO.name + " was not copied before! It will be null!"));
      return index != -1 ? (T) clonedScriptableObjects[index] : default (T);
    }

    public static NodeEditorState ExtractEditorState(
      NodeCanvas canvas,
      string stateName)
    {
      NodeEditorState nodeEditorState = (NodeEditorState) null;
      if (canvas.editorStates.Length != 0)
      {
        nodeEditorState = ((IEnumerable<NodeEditorState>) canvas.editorStates).First<NodeEditorState>((Func<NodeEditorState, bool>) (s => s.name == stateName));
        if ((UnityEngine.Object) nodeEditorState == (UnityEngine.Object) null)
          nodeEditorState = canvas.editorStates[0];
      }
      if ((UnityEngine.Object) nodeEditorState == (UnityEngine.Object) null)
      {
        nodeEditorState = ScriptableObject.CreateInstance<NodeEditorState>();
        nodeEditorState.canvas = canvas;
        canvas.editorStates = new NodeEditorState[1]
        {
          nodeEditorState
        };
      }
      nodeEditorState.name = stateName;
      return nodeEditorState;
    }
  }
}
