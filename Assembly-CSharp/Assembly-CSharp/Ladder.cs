// Decompiled with JetBrains decompiler
// Type: Ladder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Ladder")]
public class Ladder : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public float upwardsMovementSpeedMultiplier = 1f;
  public float downwardsMovementSpeedMultiplier = 1f;
  public bool isPole;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    Grid.HasPole[cell] = this.isPole;
    Grid.HasLadder[cell] = !this.isPole;
    this.GetComponent<KPrefabID>().AddTag(GameTags.Ladders);
    Components.Ladders.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    if ((Object) Grid.Objects[cell, 24] == (Object) null)
    {
      Grid.HasPole[cell] = false;
      Grid.HasLadder[cell] = false;
    }
    Components.Ladders.Remove(this);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = (List<Descriptor>) null;
    if ((double) this.upwardsMovementSpeedMultiplier != 1.0)
    {
      descriptorList = new List<Descriptor>();
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.upwardsMovementSpeedMultiplier * 100.0 - 100.0))), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.upwardsMovementSpeedMultiplier * 100.0 - 100.0))));
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }
}
