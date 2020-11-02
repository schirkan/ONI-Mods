// Decompiled with JetBrains decompiler
// Type: LightBugCrystalBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LightBugCrystalBabyConfig : IEntityConfig
{
  public const string ID = "LightBugCrystalBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugCrystalConfig.CreateLightBug("LightBugCrystalBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_CRYSTAL.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugCrystal");
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
