﻿// Decompiled with JetBrains decompiler
// Type: PropFacilityHangingLightConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PropFacilityHangingLightConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPFACILITYLAMP.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "gravitas_light_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropFacilityHangingLight", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 1, 4, decor, noise);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Steel);
    component.Temperature = 294.15f;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    OccupyArea component = inst.GetComponent<OccupyArea>();
    component.objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    int cell = Grid.PosToCell(inst);
    foreach (CellOffset occupiedCellsOffset in component.OccupiedCellsOffsets)
      Grid.GravitasFacility[Grid.OffsetCell(cell, occupiedCellsOffset)] = true;
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
