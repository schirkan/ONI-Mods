// Decompiled with JetBrains decompiler
// Type: KInputHandler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class KInputHandler
{
  private List<System.Action<KButtonEvent>> mOnKeyDownDelegates = new List<System.Action<KButtonEvent>>();
  private List<System.Action<KButtonEvent>> mOnKeyUpDelegates = new List<System.Action<KButtonEvent>>();
  private List<KInputHandler.HandlerInfo> mChildren;
  private KInputController mController;
  private string name;
  private KButtonEvent lastConsumedEventDown;
  private KButtonEvent lastConsumedEventUp;

  public KInputHandler(IInputHandler obj, KInputController controller)
    : this(obj)
    => this.mController = controller;

  public KInputHandler(IInputHandler obj)
  {
    this.name = obj.handlerName;
    MethodInfo method1 = obj.GetType().GetMethod("OnKeyDown");
    if (method1 != (MethodInfo) null)
      this.mOnKeyDownDelegates.Add((System.Action<KButtonEvent>) Delegate.CreateDelegate(typeof (System.Action<KButtonEvent>), (object) obj, method1));
    MethodInfo method2 = obj.GetType().GetMethod("OnKeyUp");
    if (!(method2 != (MethodInfo) null))
      return;
    this.mOnKeyUpDelegates.Add((System.Action<KButtonEvent>) Delegate.CreateDelegate(typeof (System.Action<KButtonEvent>), (object) obj, method2));
  }

  private void SetController(KInputController controller)
  {
    this.mController = controller;
    if (this.mChildren == null)
      return;
    foreach (KInputHandler.HandlerInfo mChild in this.mChildren)
      mChild.handler.SetController(controller);
  }

  public void AddInputHandler(KInputHandler handler, int priority)
  {
    if (this.mChildren == null)
      this.mChildren = new List<KInputHandler.HandlerInfo>();
    handler.SetController(this.mController);
    this.mChildren.Add(new KInputHandler.HandlerInfo()
    {
      priority = priority,
      handler = handler
    });
    this.mChildren.Sort((Comparison<KInputHandler.HandlerInfo>) ((a, b) => b.priority.CompareTo(a.priority)));
  }

  public void RemoveInputHandler(KInputHandler handler)
  {
    if (this.mChildren == null)
      return;
    for (int index = 0; index < this.mChildren.Count; ++index)
    {
      if (this.mChildren[index].handler == handler)
      {
        this.mChildren.RemoveAt(index);
        break;
      }
    }
  }

  public void PushInputHandler(KInputHandler handler)
  {
    if (this.mChildren == null)
      this.mChildren = new List<KInputHandler.HandlerInfo>();
    handler.SetController(this.mController);
    this.mChildren.Insert(0, new KInputHandler.HandlerInfo()
    {
      priority = int.MaxValue,
      handler = handler
    });
  }

  public void PopInputHandler()
  {
    if (this.mChildren == null)
      return;
    this.mChildren.RemoveAt(0);
  }

  public void HandleEvent(KInputEvent e)
  {
    if (e.Type == InputEventType.KeyDown)
    {
      this.HandleKeyDown((KButtonEvent) e);
    }
    else
    {
      if (InputEventType.KeyUp != e.Type)
        return;
      this.HandleKeyUp((KButtonEvent) e);
    }
  }

  public void HandleKeyDown(KButtonEvent e)
  {
    this.lastConsumedEventDown = (KButtonEvent) null;
    foreach (System.Action<KButtonEvent> onKeyDownDelegate in this.mOnKeyDownDelegates)
    {
      onKeyDownDelegate(e);
      if (e.Consumed)
        this.lastConsumedEventDown = e;
    }
    if (e.Consumed || this.mChildren == null)
      return;
    foreach (KInputHandler.HandlerInfo mChild in this.mChildren)
    {
      mChild.handler.HandleKeyDown(e);
      if (e.Consumed)
        break;
    }
  }

  public void HandleKeyUp(KButtonEvent e)
  {
    this.lastConsumedEventUp = (KButtonEvent) null;
    foreach (System.Action<KButtonEvent> mOnKeyUpDelegate in this.mOnKeyUpDelegates)
    {
      mOnKeyUpDelegate(e);
      if (e.Consumed)
        this.lastConsumedEventUp = e;
    }
    if (e.Consumed || this.mChildren == null)
      return;
    foreach (KInputHandler.HandlerInfo mChild in this.mChildren)
    {
      mChild.handler.HandleKeyUp(e);
      if (e.Consumed)
        break;
    }
  }

  public static KInputHandler GetInputHandler(IInputHandler handler)
  {
    if (handler.inputHandler == null)
      handler.inputHandler = new KInputHandler(handler);
    return handler.inputHandler;
  }

  public static void Add(IInputHandler parent, GameObject child)
  {
    foreach (Component component in child.GetComponents<Component>())
    {
      if (component is IInputHandler child1)
        KInputHandler.Add(parent, child1);
    }
  }

  public static void Add(IInputHandler parent, IInputHandler child, int priority = 0) => KInputHandler.GetInputHandler(parent).AddInputHandler(KInputHandler.GetInputHandler(child), priority);

  public static void Push(IInputHandler parent, IInputHandler child) => KInputHandler.GetInputHandler(parent).PushInputHandler(KInputHandler.GetInputHandler(child));

  public static void Remove(IInputHandler parent, IInputHandler child) => KInputHandler.GetInputHandler(parent).RemoveInputHandler(KInputHandler.GetInputHandler(child));

  public bool IsActive(Action action) => this.mController != null && this.mController.IsActive(action);

  public float GetAxis(Axis axis) => this.mController != null ? this.mController.GetAxis(axis) : 0.0f;

  public bool IsGamepad() => this.mController != null && this.mController.IsGamepad;

  public delegate void KButtonEventHandler(KButtonEvent e);

  public delegate void KCancelInputHandler();

  private struct HandlerInfo
  {
    public int priority;
    public KInputHandler handler;
  }
}
