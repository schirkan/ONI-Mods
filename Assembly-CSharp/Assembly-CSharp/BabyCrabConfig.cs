﻿// Decompiled with JetBrains decompiler
// Type: BabyCrabConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyCrabConfig : IEntityConfig
{
  public const string ID = "CrabBaby";

  public GameObject CreatePrefab()
  {
    GameObject crab = CrabConfig.CreateCrab("CrabBaby", (string) CREATURES.SPECIES.CRAB.BABY.NAME, (string) CREATURES.SPECIES.CRAB.BABY.DESC, "baby_pincher_kanim", true, "BabyCrabShell");
    EntityTemplates.ExtendEntityToBeingABaby(crab, (Tag) "Crab", "BabyCrabShell");
    return crab;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
