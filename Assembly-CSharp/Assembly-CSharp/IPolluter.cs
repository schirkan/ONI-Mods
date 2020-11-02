// Decompiled with JetBrains decompiler
// Type: IPolluter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public interface IPolluter
{
  int GetRadius();

  int GetNoise();

  GameObject GetGameObject();

  void SetAttributes(Vector2 pos, int dB, GameObject go, string name = null);

  string GetName();

  Vector2 GetPosition();

  void Clear();

  void SetSplat(NoiseSplat splat);
}
