// Decompiled with JetBrains decompiler
// Type: SpriteSheet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[Serializable]
public struct SpriteSheet
{
  public string name;
  public int numFrames;
  public int numXFrames;
  public Vector2 uvFrameSize;
  public int renderLayer;
  public Material material;
  public Texture2D texture;
}
