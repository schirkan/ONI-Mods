// Decompiled with JetBrains decompiler
// Type: TintedSprite
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
  [ReadOnly]
  public string name;
  public Sprite sprite;
  public Color color;

  public void OnAfterDeserialize()
  {
  }

  public void OnBeforeSerialize()
  {
    if (!((UnityEngine.Object) this.sprite != (UnityEngine.Object) null))
      return;
    this.name = this.sprite.name;
  }
}
