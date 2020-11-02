// Decompiled with JetBrains decompiler
// Type: ToggleSoundPlayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public class ToggleSoundPlayer : WidgetSoundPlayer
{
  public static readonly string[] default_values = new string[4]
  {
    "HUD_Click",
    "HUD_Click_Deselect",
    "HUD_Mouseover",
    "Negative"
  };
  public Func<bool> AcceptClickCondition;
  public WidgetSoundPlayer.WidgetSoundEvent[] toggle_widget_sound_events = new WidgetSoundPlayer.WidgetSoundEvent[4]
  {
    new WidgetSoundPlayer.WidgetSoundEvent(0, "On Use On", "", true),
    new WidgetSoundPlayer.WidgetSoundEvent(1, "On Use Off", "", true),
    new WidgetSoundPlayer.WidgetSoundEvent(2, "On Pointer Enter", "", true),
    new WidgetSoundPlayer.WidgetSoundEvent(3, "On Use Rejected", "", true)
  };

  public override string GetDefaultPath(int idx) => ToggleSoundPlayer.default_values[idx];

  public override WidgetSoundPlayer.WidgetSoundEvent[] widget_sound_events() => this.toggle_widget_sound_events;

  public enum SoundEvents
  {
    OnClick_On,
    OnClick_Off,
    OnPointerEnter,
    OnClick_Rejected,
  }
}
