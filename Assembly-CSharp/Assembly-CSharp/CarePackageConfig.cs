// Decompiled with JetBrains decompiler
// Type: CarePackageConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class CarePackageConfig : IEntityConfig
{
  public static readonly string ID = "CarePackage";

  public GameObject CreatePrefab() => EntityTemplates.CreateLooseEntity(CarePackageConfig.ID, (string) ITEMS.CARGO_CAPSULE.NAME, (string) ITEMS.CARGO_CAPSULE.DESC, 1f, true, Assets.GetAnim((HashedString) "portal_carepackage_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE);

  public void OnPrefabInit(GameObject go) => go.AddOrGet<CarePackage>();

  public void OnSpawn(GameObject go)
  {
  }
}
