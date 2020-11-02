// Decompiled with JetBrains decompiler
// Type: BabyPuftAlphaConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPuftAlphaConfig : IEntityConfig
{
  public const string ID = "PuftAlphaBaby";

  public GameObject CreatePrefab()
  {
    GameObject puftAlpha = PuftAlphaConfig.CreatePuftAlpha("PuftAlphaBaby", (string) CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.NAME, (string) CREATURES.SPECIES.PUFT.VARIANT_ALPHA.BABY.DESC, "baby_puft_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(puftAlpha, (Tag) "PuftAlpha");
    return puftAlpha;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => BasePuftConfig.OnSpawn(inst);
}
