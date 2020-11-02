﻿// Decompiled with JetBrains decompiler
// Type: LiquidSourceManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/LiquidSourceManager")]
public class LiquidSourceManager : KMonoBehaviour, IChunkManager
{
  public static LiquidSourceManager Instance;

  protected override void OnPrefabInit() => LiquidSourceManager.Instance = this;

  public SubstanceChunk CreateChunk(
    SimHashes element_id,
    float mass,
    float temperature,
    byte diseaseIdx,
    int diseaseCount,
    Vector3 position)
  {
    return this.CreateChunk(ElementLoader.FindElementByHash(element_id), mass, temperature, diseaseIdx, diseaseCount, position);
  }

  public SubstanceChunk CreateChunk(
    Element element,
    float mass,
    float temperature,
    byte diseaseIdx,
    int diseaseCount,
    Vector3 position)
  {
    return GeneratedOre.CreateChunk(element, mass, temperature, diseaseIdx, diseaseCount, position);
  }
}
