// Decompiled with JetBrains decompiler
// Type: VendingMachineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class VendingMachineConfig : IEntityConfig
{
  public GameObject CreatePrefab()
  {
    string name = (string) STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.NAME;
    string desc = (string) STRINGS.BUILDINGS.PREFABS.VENDINGMACHINE.DESC;
    EffectorValues tieR0_1 = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
    EffectorValues tieR0_2 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    KAnimFile anim = Assets.GetAnim((HashedString) "vendingmachine_kanim");
    EffectorValues decor = tieR0_1;
    EffectorValues noise = tieR0_2;
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity("VendingMachine", name, desc, 100f, anim, "on", Grid.SceneLayer.Building, 2, 3, decor, noise);
    PrimaryElement component = placedEntity.GetComponent<PrimaryElement>();
    component.SetElement(SimHashes.Unobtanium);
    component.Temperature = 294.15f;
    Workable workable = placedEntity.AddOrGet<Workable>();
    workable.synchronizeAnims = false;
    workable.resetProgressOnStop = true;
    SetLocker setLocker = placedEntity.AddOrGet<SetLocker>();
    setLocker.machineSound = "VendingMachine_LP";
    setLocker.overrideAnim = "anim_break_kanim";
    setLocker.dropOffset = new Vector2I(1, 1);
    setLocker.possible_contents_ids = new string[1]
    {
      "FieldRation"
    };
    placedEntity.AddOrGet<LoreBearer>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[1]
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
