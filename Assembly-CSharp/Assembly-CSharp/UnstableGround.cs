// Decompiled with JetBrains decompiler
// Type: UnstableGround
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptOut)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGround")]
public class UnstableGround : KMonoBehaviour
{
  public SimHashes element;
  public float mass;
  public float temperature;
  public byte diseaseIdx;
  public int diseaseCount;
}
