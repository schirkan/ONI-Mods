// Decompiled with JetBrains decompiler
// Type: KInputManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class KInputManager
{
  protected List<KInputController> mControllers = new List<KInputController>();
  public static bool isMousePosLocked;
  public static Vector3 lockedMousePos;

  public static bool isFocused { get; private set; }

  public static long lastUserActionTicks { get; private set; }

  public static void SetUserActive()
  {
    if (!KInputManager.isFocused)
      return;
    KInputManager.lastUserActionTicks = DateTime.Now.Ticks;
  }

  public KInputManager()
  {
    KInputManager.lastUserActionTicks = DateTime.Now.Ticks;
    KInputManager.isFocused = true;
  }

  public void AddController(KInputController controller) => this.mControllers.Add(controller);

  public KInputController GetController(int controller_index)
  {
    DebugUtil.Assert(controller_index < this.mControllers.Count);
    return this.mControllers[controller_index];
  }

  public int GetControllerCount() => this.mControllers.Count;

  public KInputController GetDefaultController() => this.GetController(0);

  public virtual void Update()
  {
    if (!KInputManager.isFocused)
      return;
    for (int index = 0; index < this.mControllers.Count; ++index)
      this.mControllers[index].Update();
    this.Dispatch();
  }

  public virtual void Dispatch()
  {
    if (!KInputManager.isFocused)
      return;
    for (int index = 0; index < this.mControllers.Count; ++index)
      this.mControllers[index].Dispatch();
  }

  public virtual void OnApplicationFocus(bool focus)
  {
    KInputManager.isFocused = focus;
    KInputManager.SetUserActive();
    if (KInputManager.isFocused)
      return;
    foreach (KInputController mController in this.mControllers)
      mController.HandleCancelInput();
  }

  public static Vector3 GetMousePos() => KInputManager.isMousePosLocked ? KInputManager.lockedMousePos : Input.mousePosition;
}
