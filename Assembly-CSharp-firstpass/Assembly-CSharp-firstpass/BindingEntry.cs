// Decompiled with JetBrains decompiler
// Type: BindingEntry
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public struct BindingEntry : IEquatable<BindingEntry>
{
  [JsonIgnore]
  public string mGroup;
  [JsonIgnore]
  public bool mRebindable;
  [JsonIgnore]
  public bool mIgnoreRootConflics;
  [JsonConverter(typeof (StringEnumConverter))]
  public GamepadButton mButton;
  [JsonConverter(typeof (StringEnumConverter))]
  public KKeyCode mKeyCode;
  [JsonConverter(typeof (StringEnumConverter))]
  public Action mAction;
  [JsonConverter(typeof (StringEnumConverter))]
  public Modifier mModifier;

  public static KKeyCode GetGamepadKeyCode(int gamepad_number, GamepadButton button)
  {
    switch (gamepad_number)
    {
      case 0:
        return (KKeyCode) (button + 350);
      case 1:
        return (KKeyCode) (button + 370);
      case 2:
        return (KKeyCode) (button + 390);
      case 3:
        return (KKeyCode) (button + 410);
      default:
        DebugUtil.Assert(false);
        return KKeyCode.None;
    }
  }

  public BindingEntry(
    string group,
    GamepadButton button,
    KKeyCode key_code,
    Modifier modifier,
    Action action,
    bool rebindable = true,
    bool ignore_root_conflicts = false)
  {
    this.mGroup = group;
    this.mButton = button;
    this.mKeyCode = key_code;
    this.mAction = action;
    this.mModifier = modifier;
    this.mRebindable = rebindable;
    this.mIgnoreRootConflics = ignore_root_conflicts;
  }

  public bool Equals(BindingEntry other) => this == other;

  public static bool operator ==(BindingEntry a, BindingEntry b) => a.mGroup == b.mGroup && a.mButton == b.mButton && (a.mKeyCode == b.mKeyCode && a.mAction == b.mAction) && a.mModifier == b.mModifier && a.mRebindable == b.mRebindable;

  public bool IsBindingEqual(BindingEntry other) => this.mButton == other.mButton && this.mKeyCode == other.mKeyCode && this.mModifier == other.mModifier;

  public static bool operator !=(BindingEntry a, BindingEntry b) => !(a == b);

  public override bool Equals(object o) => o is BindingEntry bindingEntry && this == bindingEntry;

  public override int GetHashCode() => (int) (this.mButton ^ (GamepadButton) this.mKeyCode ^ (GamepadButton) this.mAction);
}
