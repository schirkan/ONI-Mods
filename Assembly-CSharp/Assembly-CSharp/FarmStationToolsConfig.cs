// Decompiled with JetBrains decompiler
// Type: FarmStationToolsConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class FarmStationToolsConfig : IEntityConfig
{
  public const string ID = "FarmStationTools";
  public static readonly Tag tag = TagManager.Create("FarmStationTools");
  public const float MASS = 5f;

  public GameObject CreatePrefab()
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity("FarmStationTools", (string) ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME, (string) ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.DESC, 5f, true, Assets.GetAnim((HashedString) "kit_planttender_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.6f, true, additionalTags: new List<Tag>()
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
