// Decompiled with JetBrains decompiler
// Type: GameInputManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public class GameInputManager : KInputManager
{
  public KInputController AddKeyboardMouseController()
  {
    KInputController controller = new KInputController(false);
    foreach (BindingEntry bindingEntry in GameInputMapping.GetBindingEntries())
      controller.Bind(bindingEntry.mKeyCode, bindingEntry.mModifier, bindingEntry.mAction);
    this.AddController(controller);
    return controller;
  }

  public KInputController AddGamepadController(int gamepad_index)
  {
    KInputController controller = new KInputController(true);
    foreach (BindingEntry bindingEntry in GameInputMapping.GetBindingEntries())
      controller.Bind(BindingEntry.GetGamepadKeyCode(gamepad_index, bindingEntry.mButton), Modifier.None, bindingEntry.mAction);
    this.AddController(controller);
    return controller;
  }

  public GameInputManager(BindingEntry[] default_keybindings)
  {
    GameInputMapping.SetDefaultKeyBindings(default_keybindings);
    GameInputMapping.LoadBindings();
    this.AddKeyboardMouseController();
  }

  public void RebindControls()
  {
    foreach (KInputController mController in this.mControllers)
    {
      mController.ClearBindings();
      foreach (BindingEntry bindingEntry in GameInputMapping.GetBindingEntries())
        mController.Bind(bindingEntry.mKeyCode, bindingEntry.mModifier, bindingEntry.mAction);
      mController.HandleCancelInput();
    }
  }

  public override void Update()
  {
    if (!KInputManager.isFocused)
      return;
    base.Update();
  }

  public override void OnApplicationFocus(bool focusStatus) => base.OnApplicationFocus(focusStatus);
}
