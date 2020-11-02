// Decompiled with JetBrains decompiler
// Type: KInputController
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KInputController : IInputHandler
{
  private List<KInputBinding> mBindings;
  private List<KInputEvent> mEvents;
  private KInputController.KeyDef[] mKeyDefs = new KInputController.KeyDef[0];
  private bool mDirtyBindings;
  private float[] mAxis;
  private Modifier mActiveModifiers;
  private bool[] mActionState;
  private bool[] mScrollState;
  private bool mIgnoreKeyboard;
  private bool mIgnoreMouse;
  private Dictionary<KInputController.KeyDefEntry, KInputController.KeyDef> mKeyDefLookup = new Dictionary<KInputController.KeyDefEntry, KInputController.KeyDef>();
  private static readonly KKeyCode[] altCodes = new KKeyCode[2]
  {
    KKeyCode.LeftAlt,
    KKeyCode.RightAlt
  };
  private static readonly KKeyCode[] ctrlCodes = new KKeyCode[2]
  {
    KKeyCode.LeftControl,
    KKeyCode.RightControl
  };
  private static readonly KKeyCode[] shiftCodes = new KKeyCode[2]
  {
    KKeyCode.LeftShift,
    KKeyCode.RightShift
  };
  private static readonly KKeyCode[] capsCodes = new KKeyCode[1]
  {
    KKeyCode.CapsLock
  };

  public string handlerName => nameof (KInputController);

  public KInputHandler inputHandler { get; set; }

  public bool IsGamepad { get; private set; }

  public KInputController(bool is_gamepad)
  {
    this.mBindings = new List<KInputBinding>();
    this.mEvents = new List<KInputEvent>();
    this.mDirtyBindings = false;
    this.IsGamepad = is_gamepad;
    this.mAxis = new float[4];
    this.mActiveModifiers = Modifier.None;
    this.mActionState = new bool[250];
    this.mScrollState = new bool[2];
    this.inputHandler = new KInputHandler((IInputHandler) this, this);
  }

  public void ClearBindings() => this.mBindings.Clear();

  public void Bind(KKeyCode key_code, Modifier modifier, Action action)
  {
    this.mBindings.Add(new KInputBinding(key_code, modifier, action));
    this.mDirtyBindings = true;
  }

  public void QueueButtonEvent(KInputController.KeyDef key_def, bool is_down)
  {
    if (!KInputManager.isFocused)
      return;
    bool[] mActionFlags = key_def.mActionFlags;
    key_def.mIsDown = is_down;
    InputEventType event_type = is_down ? InputEventType.KeyDown : InputEventType.KeyUp;
    for (int index = 0; index < mActionFlags.Length; ++index)
    {
      if (mActionFlags[index])
        this.mActionState[index] = is_down;
    }
    this.mEvents.Add((KInputEvent) new KButtonEvent(this, event_type, mActionFlags));
    KInputManager.SetUserActive();
  }

  private void GenerateActionFlagTable()
  {
    this.mKeyDefLookup.Clear();
    foreach (KInputBinding mBinding in this.mBindings)
    {
      KInputController.KeyDefEntry key = new KInputController.KeyDefEntry(mBinding.mKeyCode, mBinding.mModifier);
      KInputController.KeyDef keyDef = (KInputController.KeyDef) null;
      if (!this.mKeyDefLookup.TryGetValue(key, out keyDef))
      {
        keyDef = new KInputController.KeyDef(mBinding.mKeyCode, mBinding.mModifier);
        this.mKeyDefLookup[key] = keyDef;
      }
      keyDef.mActionFlags[(int) mBinding.mAction] = true;
    }
    this.mKeyDefs = new KInputController.KeyDef[this.mKeyDefLookup.Count];
    this.mKeyDefLookup.Values.CopyTo(this.mKeyDefs, 0);
  }

  public bool GetKeyDown(KKeyCode key_code)
  {
    bool flag = false;
    int num = (int) key_code;
    if (num < 1000)
    {
      flag = Input.GetKeyDown((KeyCode) num);
    }
    else
    {
      switch (num)
      {
        case 1001:
          flag = this.mScrollState[1];
          break;
        case 1002:
          flag = this.mScrollState[0];
          break;
      }
    }
    return flag;
  }

  public bool GetKeyUp(KKeyCode key_code)
  {
    int num = (int) key_code;
    return num < 1000 && Input.GetKeyUp((KeyCode) num);
  }

  public void CheckModifier(KKeyCode[] key_codes, Modifier modifier)
  {
    this.mActiveModifiers &= ~modifier;
    foreach (KKeyCode keyCode in key_codes)
    {
      if (this.GetKeyDown(keyCode) || Input.GetKey((KeyCode) keyCode))
      {
        this.mActiveModifiers |= modifier;
        break;
      }
    }
  }

  private void UpdateAxis()
  {
    this.mAxis[2] = Input.GetAxis("Mouse X");
    this.mAxis[3] = Input.GetAxis("Mouse Y");
  }

  private void UpdateModifiers()
  {
    this.CheckModifier(KInputController.altCodes, Modifier.Alt);
    this.CheckModifier(KInputController.ctrlCodes, Modifier.Ctrl);
    this.CheckModifier(KInputController.shiftCodes, Modifier.Shift);
    this.CheckModifier(KInputController.capsCodes, Modifier.CapsLock);
  }

  private void UpdateScrollStates()
  {
    float axis = Input.GetAxis("Mouse ScrollWheel");
    this.mScrollState[1] = (double) axis < 0.0;
    this.mScrollState[0] = (double) axis > 0.0;
  }

  public void ToggleKeyboard(bool active) => this.mIgnoreKeyboard = active;

  public void ToggleMouse(bool active) => this.mIgnoreMouse = active;

  public void Update()
  {
    if (!KInputManager.isFocused)
      return;
    if (this.mDirtyBindings)
    {
      this.GenerateActionFlagTable();
      this.mDirtyBindings = false;
    }
    if (this.IsGamepad)
      return;
    this.UpdateScrollStates();
    this.UpdateAxis();
    this.UpdateModifiers();
    foreach (KInputController.KeyDef mKeyDef in this.mKeyDefs)
    {
      int mKeyCode = (int) mKeyDef.mKeyCode;
      if ((!this.mIgnoreKeyboard || mKeyCode >= 323) && (!this.mIgnoreMouse || (mKeyCode < 323 || mKeyCode >= 330) && (mKeyCode != 1001 && mKeyCode != 1002)))
      {
        if ((!mKeyDef.mIsDown || mKeyDef.mKeyCode == KKeyCode.MouseScrollDown ? 1 : (mKeyDef.mKeyCode == KKeyCode.MouseScrollUp ? 1 : 0)) != 0 && this.GetKeyDown(mKeyDef.mKeyCode) && this.mActiveModifiers == mKeyDef.mModifier)
          this.QueueButtonEvent(mKeyDef, true);
        if (mKeyDef.mIsDown && this.GetKeyUp(mKeyDef.mKeyCode))
          this.QueueButtonEvent(mKeyDef, false);
      }
    }
  }

  public void Dispatch()
  {
    foreach (KInputEvent mEvent in this.mEvents)
      this.inputHandler.HandleEvent(mEvent);
    this.mEvents.Clear();
  }

  public bool IsActive(Action action) => this.mActionState[(int) action];

  public float GetAxis(Axis axis) => this.mAxis[(int) axis];

  public void HandleCancelInput()
  {
    foreach (KInputController.KeyDef mKeyDef in this.mKeyDefs)
    {
      if (this.IsGamepad || mKeyDef.mIsDown && mKeyDef.mKeyCode < KKeyCode.KleiKeys && !Input.GetKey((KeyCode) mKeyDef.mKeyCode))
        this.QueueButtonEvent(mKeyDef, false);
    }
    this.UpdateModifiers();
  }

  public KKeyCode GetInputForAction(Action action)
  {
    foreach (KInputBinding mBinding in this.mBindings)
    {
      if (mBinding.mAction == action)
        return mBinding.mKeyCode;
    }
    return KKeyCode.None;
  }

  private enum Scroll
  {
    Up,
    Down,
    NumStates,
  }

  public struct KeyDefEntry
  {
    private KKeyCode mKeyCode;
    private Modifier mModifier;

    public KeyDefEntry(KKeyCode key_code, Modifier modifier)
    {
      this.mKeyCode = key_code;
      this.mModifier = modifier;
    }

    private void Print() => Debug.Log((object) (this.mKeyCode.ToString() + this.mModifier.ToString()));
  }

  [DebuggerDisplay("Key: {mKeyCode} Mod: {mModifier}")]
  public class KeyDef
  {
    public KKeyCode mKeyCode;
    public Modifier mModifier;
    public bool[] mActionFlags;
    public bool mIsDown;

    public KeyDef(KKeyCode key_code, Modifier modifier)
    {
      this.mKeyCode = key_code;
      this.mModifier = modifier;
      this.mActionFlags = new bool[250];
    }
  }
}
