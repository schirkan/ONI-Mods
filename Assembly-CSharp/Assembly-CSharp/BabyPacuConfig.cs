// Decompiled with JetBrains decompiler
// Type: BabyPacuConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyPacuConfig : IEntityConfig
{
  public const string ID = "PacuBaby";

  public GameObject CreatePrefab()
  {
    GameObject pacu = PacuConfig.CreatePacu("PacuBaby", (string) CREATURES.SPECIES.PACU.BABY.NAME, (string) CREATURES.SPECIES.PACU.BABY.DESC, "baby_pacu_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(pacu, (Tag) "Pacu");
    return pacu;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
