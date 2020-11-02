// Decompiled with JetBrains decompiler
// Type: BabyPuftOxyliteConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPuftOxyliteConfig : IEntityConfig
{
  public const string ID = "PuftOxyliteBaby";

  public GameObject CreatePrefab()
  {
    GameObject puftOxylite = PuftOxyliteConfig.CreatePuftOxylite("PuftOxyliteBaby", (string) CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.NAME, (string) CREATURES.SPECIES.PUFT.VARIANT_OXYLITE.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puftOxylite, (Tag) "PuftOxylite");
    return puftOxylite;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
