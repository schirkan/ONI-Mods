// Decompiled with JetBrains decompiler
// Type: StickerBombConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class StickerBombConfig : IEntityConfig
{
  public const string ID = "StickerBomb";

  public GameObject CreatePrefab()
  {
    GameObject basicEntity = EntityTemplates.CreateBasicEntity("StickerBomb", (string) STRINGS.BUILDINGS.PREFABS.STICKERBOMB.NAME, (string) STRINGS.BUILDINGS.PREFABS.STICKERBOMB.DESC, 1f, true, Assets.GetAnim((HashedString) "sticker_kanim"), "off", Grid.SceneLayer.Backwall);
    EntityTemplates.AddCollision(basicEntity, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
    basicEntity.AddOrGet<StickerBomb>();
    return basicEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
    inst.AddOrGet<OccupyArea>().OccupiedCellsOffsets = new CellOffset[1];
    inst.AddComponent<Modifiers>();
    inst.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER2);
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
