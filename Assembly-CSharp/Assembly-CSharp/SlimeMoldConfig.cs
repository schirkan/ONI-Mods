// Decompiled with JetBrains decompiler
// Type: SlimeMoldConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SlimeMoldConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.SlimeMold;

  public SimHashes SublimeElementID => SimHashes.ContaminatedOxygen;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.ContaminatedOxygenBubble;
    sublimates.info = new Sublimates.Info(0.025f, 0.125f, 1.8f, 0.0f, this.SublimeElementID);
    return solidOreEntity;
  }
}
