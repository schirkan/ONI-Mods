﻿// Decompiled with JetBrains decompiler
// Type: InputInit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class InputInit : MonoBehaviour
{
  private void Awake()
  {
    GameInputManager inputManager = Global.Instance.GetInputManager();
    for (int controller_index = 0; controller_index < inputManager.GetControllerCount(); ++controller_index)
    {
      KInputController controller = inputManager.GetController(controller_index);
      if (controller.IsGamepad)
        KInputHandler.Add((IInputHandler) controller, this.gameObject);
    }
    KInputHandler.Add((IInputHandler) inputManager.GetDefaultController(), (IInputHandler) KScreenManager.Instance, 10);
    DebugHandler debugHandler = new DebugHandler();
    KInputHandler.Add((IInputHandler) inputManager.GetDefaultController(), (IInputHandler) debugHandler, -1);
  }
}
