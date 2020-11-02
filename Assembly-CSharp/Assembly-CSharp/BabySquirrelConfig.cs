// Decompiled with JetBrains decompiler
// Type: BabySquirrelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabySquirrelConfig : IEntityConfig
{
  public const string ID = "SquirrelBaby";

  public GameObject CreatePrefab()
  {
    GameObject squirrel = SquirrelConfig.CreateSquirrel("SquirrelBaby", (string) CREATURES.SPECIES.SQUIRREL.BABY.NAME, (string) CREATURES.SPECIES.SQUIRREL.BABY.DESC, "baby_squirrel_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(squirrel, (Tag) "Squirrel");
    return squirrel;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
