// Decompiled with JetBrains decompiler
// Type: BabyPuftConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPuftConfig : IEntityConfig
{
  public const string ID = "PuftBaby";

  public GameObject CreatePrefab()
  {
    GameObject puft = PuftConfig.CreatePuft("PuftBaby", (string) CREATURES.SPECIES.PUFT.BABY.NAME, (string) CREATURES.SPECIES.PUFT.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puft, (Tag) "Puft");
    return puft;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
