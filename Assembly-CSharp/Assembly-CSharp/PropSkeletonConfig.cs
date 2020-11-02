// Decompiled with JetBrains decompiler
// Type: PropSkeletonConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class PropSkeletonConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.PROPSKELETON.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.PROPSKELETON.DESC;
    EffectorValues tieR5 = TUNING.BUILDINGS.DECOR.BONUS.TIER5;
    EffectorValues tieR0 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "skeleton_poi_kanim");
    EffectorValues decor = tieR5;
    EffectorValues noise = tieR0;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("PropSkeleton", name, desc, 50f, anim, "off", Grid.SceneLayer.Building, 1, 2, decor, noise);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Creature);
    component.Temperature = 294.15f;
    return placedEntity;
  }

  public void OnPrefabInit(GameObject inst) => inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[1]
  {
    ObjectLayer.Building
  };

  public void OnSpawn(GameObject inst)
  {
  }
}
