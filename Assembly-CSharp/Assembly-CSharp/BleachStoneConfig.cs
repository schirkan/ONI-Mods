// Decompiled with JetBrains decompiler
// Type: BleachStoneConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class BleachStoneConfig : IOreConfig
{
  public SimHashes ElementID => SimHashes.BleachStone;

  public SimHashes SublimeElementID => SimHashes.ChlorineGas;

  public GameObject CreatePrefab()
  {
    GameObject solidOreEntity = EntityTemplates.CreateSolidOreEntity(this.ElementID);
    Sublimates sublimates = solidOreEntity.AddOrGet<Sublimates>();
    sublimates.spawnFXHash = SpawnFXHashes.BleachStoneEmissionBubbles;
    sublimates.info = new Sublimates.Info(0.0002f, 0.0025f, 1.8f, 0.5f, this.SublimeElementID);
    return solidOreEntity;
  }
}
