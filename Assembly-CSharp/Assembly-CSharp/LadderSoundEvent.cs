// Decompiled with JetBrains decompiler
// Type: LadderSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LadderSoundEvent : SoundEvent
{
  public LadderSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, true)
  {
  }

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject);
    if (!this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.looping, this.isDynamic))
      return;
    Vector3 vector3 = behaviour.GetComponent<Transform>().GetPosition();
    vector3.z = 0.0f;
    float volume = 1f;
    if (this.objectIsSelectedAndVisible)
    {
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
      volume = SoundEvent.GetVolume(this.objectIsSelectedAndVisible);
    }
    int cell = Grid.PosToCell(vector3);
    BuildingDef buildingDef = (BuildingDef) null;
    if (Grid.IsValidCell(cell))
    {
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((Object) gameObject != (Object) null && (Object) gameObject.GetComponent<Ladder>() != (Object) null)
      {
        Building component = (Building) gameObject.GetComponent<BuildingComplete>();
        if ((Object) component != (Object) null)
          buildingDef = component.Def;
      }
    }
    if (!((Object) buildingDef != (Object) null))
      return;
    string sound = GlobalAssets.GetSound(buildingDef.PrefabID == "LadderFast" ? StringFormatter.Combine(this.name, "_Plastic") : this.name);
    if (sound == null)
      return;
    SoundEvent.PlayOneShot(sound, vector3, volume);
  }
}
