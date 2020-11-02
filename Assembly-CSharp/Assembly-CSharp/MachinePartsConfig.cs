// Decompiled with JetBrains decompiler
// Type: MachinePartsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class MachinePartsConfig : IEntityConfig
{
  public const string ID = "MachineParts";
  public static readonly Tag TAG = TagManager.Create("MachineParts");
  public const float MASS = 5f;

  public GameObject CreatePrefab() => EntityTemplates.CreateLooseEntity("MachineParts", (string) ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.MACHINE_PARTS.DESC, 5f, true, Assets.GetAnim((HashedString) "buildingrelocate_kanim"), "idle", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.CIRCLE, 0.35f, 0.35f, true);

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
