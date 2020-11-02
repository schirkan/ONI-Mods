// Decompiled with JetBrains decompiler
// Type: SoundEventVolumeCache
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
  public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();

  public static SoundEventVolumeCache instance => Singleton<SoundEventVolumeCache>.Instance;

  public void AddVolume(string animFile, string eventName, EffectorValues vals)
  {
    HashedString key = new HashedString(animFile + ":" + eventName);
    if (!this.volumeCache.ContainsKey(key))
      this.volumeCache.Add(key, vals);
    else
      this.volumeCache[key] = vals;
  }

  public EffectorValues GetVolume(string animFile, string eventName)
  {
    HashedString key = new HashedString(animFile + ":" + eventName);
    return !this.volumeCache.ContainsKey(key) ? new EffectorValues() : this.volumeCache[key];
  }
}
