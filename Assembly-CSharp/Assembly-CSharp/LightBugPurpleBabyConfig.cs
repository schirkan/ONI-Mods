// Decompiled with JetBrains decompiler
// Type: LightBugPurpleBabyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LightBugPurpleBabyConfig : IEntityConfig
{
  public const string ID = "LightBugPurpleBaby";

  public GameObject CreatePrefab()
  {
    GameObject lightBug = LightBugPurpleConfig.CreateLightBug("LightBugPurpleBaby", (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.NAME, (string) CREATURES.SPECIES.LIGHTBUG.VARIANT_PURPLE.BABY.DESC, "baby_lightbug_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(lightBug, (Tag) "LightBugPurple");
    return lightBug;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
