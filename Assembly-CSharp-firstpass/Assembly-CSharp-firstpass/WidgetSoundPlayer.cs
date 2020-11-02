// Decompiled with JetBrains decompiler
// Type: WidgetSoundPlayer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public class WidgetSoundPlayer
{
  public bool Enabled = true;
  public static Func<string, string> getSoundPath;

  public virtual string GetDefaultPath(int idx) => "";

  public virtual WidgetSoundPlayer.WidgetSoundEvent[] widget_sound_events() => (WidgetSoundPlayer.WidgetSoundEvent[]) null;

  public void Play(int sound_event_idx)
  {
    if (!this.Enabled)
      return;
    WidgetSoundPlayer.WidgetSoundEvent widgetSoundEvent = new WidgetSoundPlayer.WidgetSoundEvent();
    for (int index = 0; index < this.widget_sound_events().Length; ++index)
    {
      if (sound_event_idx == this.widget_sound_events()[index].idx)
      {
        widgetSoundEvent = this.widget_sound_events()[sound_event_idx];
        break;
      }
    }
    if (!KInputManager.isFocused || !widgetSoundEvent.PlaySound || (widgetSoundEvent.Name == null || widgetSoundEvent.Name.Length < 0) || widgetSoundEvent.Name == "")
      return;
    KFMOD.PlayUISound(WidgetSoundPlayer.getSoundPath(widgetSoundEvent.OverrideAssetName == "" ? this.GetDefaultPath(widgetSoundEvent.idx) : widgetSoundEvent.OverrideAssetName));
  }

  [Serializable]
  public struct WidgetSoundEvent
  {
    public string Name;
    public string OverrideAssetName;
    public int idx;
    public bool PlaySound;

    public WidgetSoundEvent(int idx, string Name, string OverrideAssetName, bool PlaySound)
    {
      this.idx = idx;
      this.Name = Name;
      this.OverrideAssetName = OverrideAssetName;
      this.PlaySound = PlaySound;
    }
  }
}
