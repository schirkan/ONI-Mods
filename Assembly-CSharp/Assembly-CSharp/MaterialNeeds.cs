// Decompiled with JetBrains decompiler
// Type: MaterialNeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/MaterialNeeds")]
public class MaterialNeeds : KMonoBehaviour
{
  private Dictionary<Tag, float> Needs = new Dictionary<Tag, float>();
  public System.Action OnDirty;

  public static MaterialNeeds Instance { get; private set; }

  public static void DestroyInstance() => MaterialNeeds.Instance = (MaterialNeeds) null;

  protected override void OnPrefabInit() => MaterialNeeds.Instance = this;

  public void UpdateNeed(Tag tag, float amount)
  {
    float num = 0.0f;
    if (!this.Needs.TryGetValue(tag, out num))
      this.Needs[tag] = 0.0f;
    this.Needs[tag] = num + amount;
  }

  public float GetAmount(Tag tag)
  {
    float num = 0.0f;
    this.Needs.TryGetValue(tag, out num);
    return num;
  }

  public Dictionary<Tag, float> GetNeeds() => this.Needs;
}
