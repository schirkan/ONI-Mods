// Decompiled with JetBrains decompiler
// Type: ButtonSoundPlayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public class ButtonSoundPlayer : WidgetSoundPlayer
{
  public static string[] default_values = new string[3]
  {
    "HUD_Click_Open",
    "HUD_Mouseover",
    "Negative"
  };
  public Func<bool> AcceptClickCondition;
  public WidgetSoundPlayer.WidgetSoundEvent[] button_widget_sound_events = new WidgetSoundPlayer.WidgetSoundEvent[3]
  {
    new WidgetSoundPlayer.WidgetSoundEvent(0, "On Use", "", true),
    new WidgetSoundPlayer.WidgetSoundEvent(1, "On Pointer Enter", "", true),
    new WidgetSoundPlayer.WidgetSoundEvent(2, "On Use Rejected", "", true)
  };

  public override string GetDefaultPath(int idx) => ButtonSoundPlayer.default_values[idx];

  public override WidgetSoundPlayer.WidgetSoundEvent[] widget_sound_events() => this.button_widget_sound_events;

  public enum SoundEvents
  {
    OnClick,
    OnPointerEnter,
    OnClick_Rejected,
  }
}
