// Decompiled with JetBrains decompiler
// Type: BackgroundEarthConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BackgroundEarthConfig : IEntityConfig
{
  public static string ID = "BackgroundEarth";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(BackgroundEarthConfig.ID, BackgroundEarthConfig.ID);
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "earth_kanim")
    };
    kbatchedAnimController.isMovable = true;
    kbatchedAnimController.initialAnim = "idle";
    kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    entity.AddOrGet<LoopingSounds>();
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
