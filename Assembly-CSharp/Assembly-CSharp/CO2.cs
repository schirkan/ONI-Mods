// Decompiled with JetBrains decompiler
// Type: CO2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
  [Serialize]
  [NonSerialized]
  public Vector3 velocity = Vector3.zero;
  [Serialize]
  [NonSerialized]
  public float mass;
  [Serialize]
  [NonSerialized]
  public float temperature;
  [Serialize]
  [NonSerialized]
  public float lifetimeRemaining;

  public void StartLoop()
  {
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Play((HashedString) "exhale_pre");
    component.Play((HashedString) "exhale_loop", KAnim.PlayMode.Loop);
  }

  public void TriggerDestroy() => this.GetComponent<KBatchedAnimController>().Play((HashedString) "exhale_pst");
}
