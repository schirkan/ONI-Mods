// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeEditorInputSystem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NodeEditorFramework
{
  public static class NodeEditorInputSystem
  {
    private static List<KeyValuePair<EventHandlerAttribute, Delegate>> eventHandlers;
    private static List<KeyValuePair<HotkeyAttribute, Delegate>> hotkeyHandlers;
    private static List<KeyValuePair<ContextEntryAttribute, PopupMenu.MenuFunctionData>> contextEntries;
    private static List<KeyValuePair<ContextFillerAttribute, Delegate>> contextFillers;
    private static NodeEditorState unfocusControlsForState;

    public static void SetupInput()
    {
      NodeEditorInputSystem.eventHandlers = new List<KeyValuePair<EventHandlerAttribute, Delegate>>();
      NodeEditorInputSystem.hotkeyHandlers = new List<KeyValuePair<HotkeyAttribute, Delegate>>();
      NodeEditorInputSystem.contextEntries = new List<KeyValuePair<ContextEntryAttribute, PopupMenu.MenuFunctionData>>();
      NodeEditorInputSystem.contextFillers = new List<KeyValuePair<ContextFillerAttribute, Delegate>>();
      foreach (Assembly assembly in ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (assembly => assembly.FullName.Contains("Assembly"))))
      {
        foreach (System.Type type1 in assembly.GetTypes())
        {
          foreach (MethodInfo method in type1.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
          {
            Delegate actionDelegate = (Delegate) null;
            foreach (object customAttribute in method.GetCustomAttributes(true))
            {
              System.Type type2 = customAttribute.GetType();
              if (type2 == typeof (EventHandlerAttribute))
              {
                if (EventHandlerAttribute.AssureValidity(method, customAttribute as EventHandlerAttribute))
                {
                  if ((object) actionDelegate == null)
                    actionDelegate = Delegate.CreateDelegate(typeof (Action<NodeEditorInputInfo>), method);
                  NodeEditorInputSystem.eventHandlers.Add(new KeyValuePair<EventHandlerAttribute, Delegate>(customAttribute as EventHandlerAttribute, actionDelegate));
                }
              }
              else if (type2 == typeof (HotkeyAttribute))
              {
                if (HotkeyAttribute.AssureValidity(method, customAttribute as HotkeyAttribute))
                {
                  if ((object) actionDelegate == null)
                    actionDelegate = Delegate.CreateDelegate(typeof (Action<NodeEditorInputInfo>), method);
                  NodeEditorInputSystem.hotkeyHandlers.Add(new KeyValuePair<HotkeyAttribute, Delegate>(customAttribute as HotkeyAttribute, actionDelegate));
                }
              }
              else if (type2 == typeof (ContextEntryAttribute))
              {
                if (ContextEntryAttribute.AssureValidity(method, customAttribute as ContextEntryAttribute))
                {
                  if ((object) actionDelegate == null)
                    actionDelegate = Delegate.CreateDelegate(typeof (Action<NodeEditorInputInfo>), method);
                  PopupMenu.MenuFunctionData menuFunctionData = (PopupMenu.MenuFunctionData) (callbackObj =>
                  {
                    if (!(callbackObj is NodeEditorInputInfo))
                      throw new UnityException("Callback Object passed by context is not of type NodeEditorMenuCallback!");
                    actionDelegate.DynamicInvoke((object) (callbackObj as NodeEditorInputInfo));
                  });
                  NodeEditorInputSystem.contextEntries.Add(new KeyValuePair<ContextEntryAttribute, PopupMenu.MenuFunctionData>(customAttribute as ContextEntryAttribute, menuFunctionData));
                }
              }
              else if (type2 == typeof (ContextFillerAttribute) && ContextFillerAttribute.AssureValidity(method, customAttribute as ContextFillerAttribute))
              {
                Delegate @delegate = Delegate.CreateDelegate(typeof (Action<NodeEditorInputInfo, GenericMenu>), method);
                NodeEditorInputSystem.contextFillers.Add(new KeyValuePair<ContextFillerAttribute, Delegate>(customAttribute as ContextFillerAttribute, @delegate));
              }
            }
          }
        }
      }
      NodeEditorInputSystem.eventHandlers.Sort((Comparison<KeyValuePair<EventHandlerAttribute, Delegate>>) ((handlerA, handlerB) => handlerA.Key.priority.CompareTo(handlerB.Key.priority)));
      NodeEditorInputSystem.hotkeyHandlers.Sort((Comparison<KeyValuePair<HotkeyAttribute, Delegate>>) ((handlerA, handlerB) => handlerA.Key.priority.CompareTo(handlerB.Key.priority)));
    }

    private static void CallEventHandlers(NodeEditorInputInfo inputInfo, bool late)
    {
      object[] objArray = new object[1]
      {
        (object) inputInfo
      };
      foreach (KeyValuePair<EventHandlerAttribute, Delegate> eventHandler in NodeEditorInputSystem.eventHandlers)
      {
        if (eventHandler.Key.handledEvent.HasValue)
        {
          EventType? handledEvent = eventHandler.Key.handledEvent;
          EventType type = inputInfo.inputEvent.type;
          if (!(handledEvent.GetValueOrDefault() == type & handledEvent.HasValue))
            continue;
        }
        if ((late ? (eventHandler.Key.priority >= 100 ? 1 : 0) : (eventHandler.Key.priority < 100 ? 1 : 0)) != 0)
        {
          eventHandler.Value.DynamicInvoke(objArray);
          if (inputInfo.inputEvent.type == EventType.Used)
            break;
        }
      }
    }

    private static void CallHotkeys(
      NodeEditorInputInfo inputInfo,
      KeyCode keyCode,
      EventModifiers mods)
    {
      object[] objArray = new object[1]
      {
        (object) inputInfo
      };
      foreach (KeyValuePair<HotkeyAttribute, Delegate> hotkeyHandler in NodeEditorInputSystem.hotkeyHandlers)
      {
        if (hotkeyHandler.Key.handledHotKey == keyCode)
        {
          EventModifiers? modifiers = hotkeyHandler.Key.modifiers;
          if (modifiers.HasValue)
          {
            modifiers = hotkeyHandler.Key.modifiers;
            EventModifiers eventModifiers = mods;
            if (!(modifiers.GetValueOrDefault() == eventModifiers & modifiers.HasValue))
              continue;
          }
          if (hotkeyHandler.Key.limitingEventType.HasValue)
          {
            EventType? limitingEventType = hotkeyHandler.Key.limitingEventType;
            EventType type = inputInfo.inputEvent.type;
            if (!(limitingEventType.GetValueOrDefault() == type & limitingEventType.HasValue))
              continue;
          }
          hotkeyHandler.Value.DynamicInvoke(objArray);
          if (inputInfo.inputEvent.type == EventType.Used)
            break;
        }
      }
    }

    private static void FillContextMenu(
      NodeEditorInputInfo inputInfo,
      GenericMenu contextMenu,
      ContextType contextType)
    {
      foreach (KeyValuePair<ContextEntryAttribute, PopupMenu.MenuFunctionData> contextEntry in NodeEditorInputSystem.contextEntries)
      {
        if (contextEntry.Key.contextType == contextType)
          contextMenu.AddItem(new GUIContent(contextEntry.Key.contextPath), false, contextEntry.Value, (object) inputInfo);
      }
      object[] objArray = new object[2]
      {
        (object) inputInfo,
        (object) contextMenu
      };
      foreach (KeyValuePair<ContextFillerAttribute, Delegate> contextFiller in NodeEditorInputSystem.contextFillers)
      {
        if (contextFiller.Key.contextType == contextType)
          contextFiller.Value.DynamicInvoke(objArray);
      }
    }

    public static void HandleInputEvents(NodeEditorState state)
    {
      if (NodeEditorInputSystem.shouldIgnoreInput(state))
        return;
      NodeEditorInputInfo inputInfo = new NodeEditorInputInfo(state);
      NodeEditorInputSystem.CallEventHandlers(inputInfo, false);
      NodeEditorInputSystem.CallHotkeys(inputInfo, Event.current.keyCode, Event.current.modifiers);
    }

    public static void HandleLateInputEvents(NodeEditorState state)
    {
      if (NodeEditorInputSystem.shouldIgnoreInput(state))
        return;
      NodeEditorInputSystem.CallEventHandlers(new NodeEditorInputInfo(state), true);
    }

    internal static bool shouldIgnoreInput(NodeEditorState state)
    {
      if (OverlayGUI.HasPopupControl() || !state.canvasRect.Contains(Event.current.mousePosition))
        return true;
      for (int index = 0; index < state.ignoreInput.Count; ++index)
      {
        if (state.ignoreInput[index].Contains(Event.current.mousePosition))
          return true;
      }
      return false;
    }

    [EventHandler(-4)]
    private static void HandleFocussing(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      editorState.focusedNode = NodeEditor.NodeAtPosition(NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos), out editorState.focusedNodeKnob);
      if (!((UnityEngine.Object) NodeEditorInputSystem.unfocusControlsForState == (UnityEngine.Object) editorState) || Event.current.type != EventType.Repaint)
        return;
      GUIUtility.hotControl = 0;
      GUIUtility.keyboardControl = 0;
      NodeEditorInputSystem.unfocusControlsForState = (NodeEditorState) null;
    }

    [EventHandler(EventType.MouseDown, -2)]
    private static void HandleSelecting(NodeEditorInputInfo inputInfo)
    {
      NodeEditorState editorState = inputInfo.editorState;
      if (inputInfo.inputEvent.button != 0 || !((UnityEngine.Object) editorState.focusedNode != (UnityEngine.Object) editorState.selectedNode))
        return;
      NodeEditorInputSystem.unfocusControlsForState = editorState;
      editorState.selectedNode = editorState.focusedNode;
      NodeEditor.RepaintClients();
    }

    [EventHandler(EventType.MouseDown, 0)]
    private static void HandleContextClicks(NodeEditorInputInfo inputInfo)
    {
      if (Event.current.button != 1)
        return;
      GenericMenu contextMenu = new GenericMenu();
      if ((UnityEngine.Object) inputInfo.editorState.focusedNode != (UnityEngine.Object) null)
        NodeEditorInputSystem.FillContextMenu(inputInfo, contextMenu, ContextType.Node);
      else
        NodeEditorInputSystem.FillContextMenu(inputInfo, contextMenu, ContextType.Canvas);
      contextMenu.Show(inputInfo.inputPos);
      Event.current.Use();
    }
  }
}
