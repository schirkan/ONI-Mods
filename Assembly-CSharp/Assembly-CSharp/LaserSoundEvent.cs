// Decompiled with JetBrains decompiler
// Type: LaserSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class LaserSoundEvent : SoundEvent
{
  public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval)
    : base(file_name, sound_name, frame, true, true, min_interval, false)
    => this.noiseValues = SoundEventVolumeCache.instance.GetVolume(nameof (LaserSoundEvent), sound_name);
}
