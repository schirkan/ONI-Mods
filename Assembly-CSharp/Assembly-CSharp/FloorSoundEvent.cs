// Decompiled with JetBrains decompiler
// Type: FloorSoundEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{Name}")]
public class FloorSoundEvent : SoundEvent
{
  public static float IDLE_WALKING_VOLUME_REDUCTION = 0.55f;

  public FloorSoundEvent(string file_name, string sound_name, int frame)
    : base(file_name, sound_name, frame, false, false, (float) SoundEvent.IGNORE_INTERVAL, true)
    => this.noiseValues = SoundEventVolumeCache.instance.GetVolume(nameof (FloorSoundEvent), sound_name);

  public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
  {
    Vector3 pos = behaviour.GetComponent<Transform>().GetPosition();
    KBatchedAnimController component = behaviour.GetComponent<KBatchedAnimController>();
    if ((Object) component != (Object) null)
      pos = component.GetPivotSymbolPosition();
    int cell = Grid.PosToCell(pos);
    string str = GlobalAssets.GetSound(StringFormatter.Combine(FloorSoundEvent.GetAudioCategory(Grid.CellBelow(cell)), "_", this.name), true) ?? GlobalAssets.GetSound(StringFormatter.Combine("Rock_", this.name), true) ?? GlobalAssets.GetSound(this.name, true);
    this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject);
    if (SoundEvent.IsLowPrioritySound(str) && !this.objectIsSelectedAndVisible)
      return;
    Vector3 vector3 = SoundEvent.GetCameraScaledPosition(pos);
    vector3.z = 0.0f;
    if (this.objectIsSelectedAndVisible)
      vector3 = SoundEvent.AudioHighlightListenerPosition(vector3);
    if (Grid.Element == null)
      return;
    int num1 = Grid.Element[cell].IsLiquid ? 1 : 0;
    float num2 = 0.0f;
    if (num1 != 0)
    {
      num2 = SoundUtil.GetLiquidDepth(cell);
      string sound = GlobalAssets.GetSound("Liquid_footstep", true);
      if (sound != null && (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, this.looping, this.isDynamic)))
      {
        FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector3, SoundEvent.GetVolume(this.objectIsSelectedAndVisible));
        if ((double) num2 > 0.0)
        {
          int num3 = (int) instance.setParameterValue("liquidDepth", num2);
        }
        SoundEvent.EndOneShot(instance);
      }
    }
    if (str == null || !this.objectIsSelectedAndVisible && !SoundEvent.ShouldPlaySound(behaviour.controller, str, this.looping, this.isDynamic))
      return;
    FMOD.Studio.EventInstance instance1 = SoundEvent.BeginOneShot(str, vector3);
    if (!instance1.isValid())
      return;
    if ((double) num2 > 0.0)
    {
      int num4 = (int) instance1.setParameterValue("liquidDepth", num2);
    }
    if (behaviour.currentAnimFile != null && behaviour.currentAnimFile.Contains("anim_loco_walk"))
    {
      int num5 = (int) instance1.setVolume(FloorSoundEvent.IDLE_WALKING_VOLUME_REDUCTION);
    }
    SoundEvent.EndOneShot(instance1);
  }

  private static string GetAudioCategory(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return "Rock";
    Element element = Grid.Element[cell];
    if (Grid.Foundation[cell])
    {
      BuildingDef buildingDef = (BuildingDef) null;
      GameObject gameObject = Grid.Objects[cell, 1];
      if ((Object) gameObject != (Object) null)
      {
        Building component = (Building) gameObject.GetComponent<BuildingComplete>();
        if ((Object) component != (Object) null)
          buildingDef = component.Def;
      }
      string str = "";
      if ((Object) buildingDef != (Object) null)
      {
        string prefabId = buildingDef.PrefabID;
        str = !(prefabId == "PlasticTile") ? (!(prefabId == "GlassTile") ? (!(prefabId == "BunkerTile") ? (!(prefabId == "MetalTile") ? (!(prefabId == "CarpetTile") ? "Tile" : "Carpet") : "TileMetal") : "TileBunker") : "TileGlass") : "TilePlastic";
      }
      return str;
    }
    string eventAudioCategory = element.substance.GetFloorEventAudioCategory();
    if (eventAudioCategory != null)
      return eventAudioCategory;
    if (element.HasTag(GameTags.RefinedMetal))
      return "RefinedMetal";
    return element.HasTag(GameTags.Metal) ? "RawMetal" : "Rock";
  }
}
