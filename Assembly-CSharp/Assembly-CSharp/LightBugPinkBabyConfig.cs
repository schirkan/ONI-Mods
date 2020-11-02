// Decompiled with JetBrains decompiler
// Type: LightBugPinkBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LightBugPinkBabyConfig : IEntityConfig
{
  public const string ID = "LightBugPinkBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugPinkConfig.CreateLightBug("LightBugPinkBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PINK.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugPink");
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
