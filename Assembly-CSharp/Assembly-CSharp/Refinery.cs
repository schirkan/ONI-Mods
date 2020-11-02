// Decompiled with JetBrains decompiler
// Type: Refinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Refinery")]
public class Refinery : KMonoBehaviour
{
  protected override void OnSpawn() => base.OnSpawn();

  [Serializable]
  public struct OrderSaveData
  {
    public string id;
    public bool infinite;

    public OrderSaveData(string id, bool infinite)
    {
      this.id = id;
      this.infinite = infinite;
    }
  }
}
