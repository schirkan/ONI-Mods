// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.Standard.RuntimeNodeEditor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using System.IO;
using UnityEngine;

namespace NodeEditorFramework.Standard
{
  public class RuntimeNodeEditor : MonoBehaviour
  {
    public string canvasPath;
    public NodeCanvas canvas;
    private NodeEditorState state;
    public bool screenSize;
    private Rect canvasRect;
    public Rect specifiedRootRect;
    public Rect specifiedCanvasRect;
    private string sceneCanvasName = "";
    private Vector2 loadScenePos;

    public void Start()
    {
      NodeEditor.checkInit(false);
      NodeEditor.initiated = false;
      this.LoadNodeCanvas(this.canvasPath);
      FPSCounter.Create();
    }

    public void Update()
    {
      NodeEditor.Update();
      FPSCounter.Update();
    }

    public void OnGUI()
    {
      if (!((UnityEngine.Object) this.canvas != (UnityEngine.Object) null))
        return;
      if ((UnityEngine.Object) this.state == (UnityEngine.Object) null)
        this.NewEditorState();
      NodeEditor.checkInit(true);
      if (NodeEditor.InitiationError)
      {
        GUILayout.Label("Initiation failed! Check console for more information!", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      }
      else
      {
        try
        {
          if (!this.screenSize && this.specifiedRootRect.max != this.specifiedRootRect.min)
            GUI.BeginGroup(this.specifiedRootRect, NodeEditorGUI.nodeSkin.box);
          NodeEditorGUI.StartNodeGUI();
          this.canvasRect = this.screenSize ? new Rect(0.0f, 0.0f, (float) Screen.width, (float) Screen.height) : this.specifiedCanvasRect;
          this.canvasRect.width -= 200f;
          this.state.canvasRect = this.canvasRect;
          NodeEditor.DrawCanvas(this.canvas, this.state);
          GUILayout.BeginArea(new Rect(this.canvasRect.x + this.state.canvasRect.width, this.state.canvasRect.y, 200f, this.state.canvasRect.height), NodeEditorGUI.nodeSkin.box);
          this.SideGUI();
          GUILayout.EndArea();
          NodeEditorGUI.EndNodeGUI();
          if (this.screenSize || !(this.specifiedRootRect.max != this.specifiedRootRect.min))
            return;
          GUI.EndGroup();
        }
        catch (UnityException ex)
        {
          this.NewNodeCanvas();
          NodeEditor.ReInit(true);
          Debug.LogError((object) "Unloaded Canvas due to exception in Draw!");
          Debug.LogException((Exception) ex);
        }
      }
    }

    public void SideGUI()
    {
      GUILayout.Label(new GUIContent("Node Editor (" + this.canvas.name + ")", "The currently opened canvas in the Node Editor"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.screenSize = GUILayout.Toggle(this.screenSize, "Adapt to Screen", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.Label("FPS: " + (object) FPSCounter.currentFPS, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.Label(new GUIContent("Node Editor (" + this.canvas.name + ")"), NodeEditorGUI.nodeLabelBold, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      if (GUILayout.Button(new GUIContent("New Canvas", "Loads an empty Canvas"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        this.NewNodeCanvas();
      GUILayout.Space(6f);
      GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.sceneCanvasName = GUILayout.TextField(this.sceneCanvasName, GUILayout.ExpandWidth(true));
      if (GUILayout.Button(new GUIContent("Save to Scene", "Saves the Canvas to the Scene"), GUILayout.ExpandWidth(false)))
        this.SaveSceneNodeCanvas(this.sceneCanvasName);
      GUILayout.EndHorizontal();
      if (GUILayout.Button(new GUIContent("Load from Scene", "Loads the Canvas from the Scene"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
      {
        GenericMenu genericMenu = new GenericMenu();
        foreach (string sceneSave in NodeEditorSaveManager.GetSceneSaves())
          genericMenu.AddItem(new GUIContent(sceneSave), false, new PopupMenu.MenuFunctionData(this.LoadSceneCanvasCallback), (object) sceneSave);
        genericMenu.Show(this.loadScenePos);
      }
      if (UnityEngine.Event.current.type == EventType.Repaint)
      {
        Rect lastRect = GUILayoutUtility.GetLastRect();
        this.loadScenePos = new Vector2(lastRect.x + 2f, lastRect.yMax + 2f);
      }
      GUILayout.Space(6f);
      if (GUILayout.Button(new GUIContent("Recalculate All", "Initiates complete recalculate. Usually does not need to be triggered manually."), (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        NodeEditor.RecalculateAll(this.canvas);
      if (GUILayout.Button("Force Re-Init", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        NodeEditor.ReInit(true);
      NodeEditorGUI.knobSize = RTEditorGUI.IntSlider(new GUIContent("Handle Size", "The size of the Node Input/Output handles"), NodeEditorGUI.knobSize, 12, 20, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      this.state.zoom = RTEditorGUI.Slider(new GUIContent("Zoom", "Use the Mousewheel. Seriously."), this.state.zoom, 0.6f, 2f, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    }

    private void LoadSceneCanvasCallback(object save) => this.LoadSceneNodeCanvas((string) save);

    public void SaveSceneNodeCanvas(string path)
    {
      this.canvas.editorStates = new NodeEditorState[1]
      {
        this.state
      };
      NodeEditorSaveManager.SaveSceneNodeCanvas(path, ref this.canvas, true);
    }

    public void LoadSceneNodeCanvas(string path)
    {
      if ((UnityEngine.Object) (this.canvas = NodeEditorSaveManager.LoadSceneNodeCanvas(path, true)) == (UnityEngine.Object) null)
      {
        this.NewNodeCanvas();
      }
      else
      {
        this.state = NodeEditorSaveManager.ExtractEditorState(this.canvas, "MainEditorState");
        NodeEditor.RecalculateAll(this.canvas);
      }
    }

    public void LoadNodeCanvas(string path)
    {
      if (!File.Exists(path) || (UnityEngine.Object) (this.canvas = NodeEditorSaveManager.LoadNodeCanvas(path, true)) == (UnityEngine.Object) null)
      {
        this.NewNodeCanvas();
      }
      else
      {
        this.state = NodeEditorSaveManager.ExtractEditorState(this.canvas, "MainEditorState");
        NodeEditor.RecalculateAll(this.canvas);
      }
    }

    public void NewNodeCanvas()
    {
      this.canvas = ScriptableObject.CreateInstance<NodeCanvas>();
      this.canvas.name = "New Canvas";
      this.NewEditorState();
    }

    private void NewEditorState()
    {
      this.state = ScriptableObject.CreateInstance<NodeEditorState>();
      this.state.canvas = this.canvas;
      this.state.name = "MainEditorState";
      this.canvas.editorStates = new NodeEditorState[1]
      {
        this.state
      };
    }
  }
}
