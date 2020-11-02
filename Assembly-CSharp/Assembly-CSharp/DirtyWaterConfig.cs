// Decompiled with JetBrains decompiler
// Type: DirtyWaterConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DirtyWaterConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.DirtyWater;

  public SimHashes SublimeElementID => SimHashes.ContaminatedOxygen;

  public GameObject CreatePrefab()
  {
    GameObject liquidOreEntity = EntityTemplates.CreateLiquidOreEntity(this.ElementID);
    Sublimates sublimates = liquidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
    sublimates.info = new Sublimates.Info(4.000001E-05f, 0.025f, 1.8f, 1f, this.SublimeElementID);
    return liquidOreEntity;
  }
}
