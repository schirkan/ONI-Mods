// Decompiled with JetBrains decompiler
// Type: PowerStationToolsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PowerStationToolsConfig : IEntityConfig
{
  public const string ID = "PowerStationTools";
  public static readonly Tag tag = TagManager.Create("PowerStationTools");
  public const float MASS = 5f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("PowerStationTools", (string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim((HashedString) "kit_electrician_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, additionalTags: new List<Tag>()
    {
      GameTags.MiscPickupable
    });
    looseEntity.AddOrGet<EntitySplitter>();
    return looseEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
