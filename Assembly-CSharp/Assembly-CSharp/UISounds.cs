// Decompiled with JetBrains decompiler
// Type: UISounds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/UISounds")]
public class UISounds : KMonoBehaviour
{
  [SerializeField]
  private bool logSounds;
  [SerializeField]
  private UISounds.SoundData[] soundData;

  public static UISounds Instance { get; private set; }

  public static void DestroyInstance() => UISounds.Instance = (UISounds) null;

  protected override void OnPrefabInit() => UISounds.Instance = this;

  public static void PlaySound(UISounds.Sound sound) => UISounds.Instance.PlaySoundInternal(sound);

  private void PlaySoundInternal(UISounds.Sound sound)
  {
    for (int index = 0; index < this.soundData.Length; ++index)
    {
      if (this.soundData[index].sound == sound)
      {
        if (this.logSounds)
          DebugUtil.LogArgs((object) "Play sound", (object) this.soundData[index].name);
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[index].name));
      }
    }
  }

  public enum Sound
  {
    NegativeNotification,
    PositiveNotification,
    Select,
    Negative,
    Back,
    ClickObject,
    HUD_Mouseover,
    Object_Mouseover,
    ClickHUD,
  }

  [Serializable]
  private struct SoundData
  {
    public string name;
    public UISounds.Sound sound;
  }
}
