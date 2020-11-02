// Decompiled with JetBrains decompiler
// Type: BabyMoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BabyMoleConfig : IEntityConfig
{
  public const string ID = "MoleBaby";

  public GameObject CreatePrefab()
  {
    GameObject mole = MoleConfig.CreateMole("MoleBaby", (string) CREATURES.SPECIES.MOLE.BABY.NAME, (string) CREATURES.SPECIES.MOLE.BABY.DESC, "baby_driller_kanim", true);
    EntityTemplates.ExtendEntityToBeingABaby(mole, (Tag) "Mole");
    return mole;
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst) => MoleConfig.SetSpawnNavType(inst);
}
