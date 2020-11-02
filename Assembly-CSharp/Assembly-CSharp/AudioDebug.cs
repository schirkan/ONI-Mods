// Decompiled with JetBrains decompiler
// Type: AudioDebug
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
  private static AudioDebug instance;
  public bool musicEnabled;
  public bool debugSoundEvents;
  public bool debugFloorSounds;
  public bool debugGameEventSounds;
  public bool debugNotificationSounds;
  public bool debugVoiceSounds;

  public static AudioDebug Get() => AudioDebug.instance;

  protected override void OnPrefabInit() => AudioDebug.instance = this;

  public void ToggleMusic()
  {
    if ((Object) Game.Instance != (Object) null)
      Game.Instance.SetMusicEnabled(this.musicEnabled);
    this.musicEnabled = !this.musicEnabled;
  }
}
