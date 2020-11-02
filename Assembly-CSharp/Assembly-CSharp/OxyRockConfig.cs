// Decompiled with JetBrains decompiler
// Type: OxyRockConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OxyRockConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.OxyRock;

  public SimHashes SublimeElementID => SimHashes.Oxygen;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.OxygenEmissionBubbles;
    sublimates.info = new Sublimates.Info(0.01f, 0.005f, 1.8f, 0.7f, this.SublimeElementID);
    return solidOreEntity;
  }
}
