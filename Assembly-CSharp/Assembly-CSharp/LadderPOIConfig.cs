// Decompiled with JetBrains decompiler
// Type: LadderPOIConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LadderPOIConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    int width1 = 1;
    int height1 = 1;
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPLADDER.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPLADDER.DESC;
    int num1 = width1;
    int num2 = height1;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "ladder_poi_kanim");
    int width2 = num1;
    int height2 = num2;
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropLadder", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, width2, height2, decor, noise);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Polypropylene);
    component.Temperature = 294.15f;
    Ladder ladder = placedEntity.AddOrGet<Ladder>();
    ladder.upwardsMovementSpeedMultiplier = 1.5f;
    ladder.downwardsMovementSpeedMultiplier = 1.5f;
    placedEntity.AddOrGet<AnimTileable>();
    Object.DestroyImmediate((Object) placedEntity.AddOrGet<OccupyArea>());
    OccupyArea occupyArea = placedEntity.AddOrGet<OccupyArea>();
    occupyArea.OccupiedCellsOffsets = EntityTemplates.GenerateOffsets(width1, height1);
    occupyArea.objectLayers = new ObjectLayer[1]
    {
      ObjectLayer.Building
    };
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
