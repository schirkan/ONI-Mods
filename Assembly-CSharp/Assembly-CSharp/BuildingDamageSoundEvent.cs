// Decompiled with JetBrains decompiler
// Type: BuildingDamageSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public class BuildingDamageSoundEvent : SoundEvent
{
  public BuildingDamageSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, false)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 sound_pos = behaviour.GetComponent<Transform>().GetPosition();
    sound_pos.z = 0.0f;
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject);
    if (this.objectIsSelectedAndVisible)
      sound_pos = SoundEvent.AudioHighlightListenerPosition(sound_pos);
    Worker component1 = behaviour.GetComponent<Worker>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
    {
      string sound = GlobalAssets.GetSound("Building_Dmg_Metal");
      if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, this.looping, this.isDynamic))
      {
        SoundEvent.PlayOneShot(this.sound, sound_pos, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
        return;
      }
    }
    Workable workable = component1.workable;
    if (!((UnityEngine.Object) workable != (UnityEngine.Object) null))
      return;
    Building component2 = workable.GetComponent<Building>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
      return;
    string sound1 = GlobalAssets.GetSound(StringFormatter.Combine(this.name, "_", component2.Def.AudioCategory)) ?? GlobalAssets.GetSound("Building_Dmg_Metal");
    if (sound1 == null || !this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, sound1, this.looping, this.isDynamic))
      return;
    SoundEvent.PlayOneShot(sound1, sound_pos, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
  }
}
