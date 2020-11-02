// Decompiled with JetBrains decompiler
// Type: MixManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MixManager : MonoBehaviour
{
  private void Update()
  {
    if (AudioMixer.instance == null || !AudioMixer.instance.persistentSnapshotsActive)
      return;
    AudioMixer.instance.UpdatePersistentSnapshotParameters();
  }

  private void OnApplicationFocus(bool hasFocus)
  {
    if (AudioMixer.instance == null || (Object) AudioMixerSnapshots.Get() == (Object) null)
      return;
    if (!hasFocus && KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1)
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
    else
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().GameNotFocusedSnapshot);
  }
}
