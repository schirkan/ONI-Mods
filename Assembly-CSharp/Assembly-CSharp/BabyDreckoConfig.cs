// Decompiled with JetBrains decompiler
// Type: BabyDreckoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyDreckoConfig : IEntityConfig
{
  public const string ID = "DreckoBaby";

  public GameObject CreatePrefab()
  {
    GameObject drecko = DreckoConfig.CreateDrecko("DreckoBaby", (string) CREATURES.SPECIES.DRECKO.BABY.NAME, (string) CREATURES.SPECIES.DRECKO.BABY.DESC, "baby_drecko_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(drecko, (Tag) "Drecko");
    return drecko;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
