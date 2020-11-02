// Decompiled with JetBrains decompiler
// Type: Klei.AI.PrefabAttributeModifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
  [AddComponentMenu("KMonoBehaviour/scripts/PrefabAttributeModifiers")]
  public class PrefabAttributeModifiers : KMonoBehaviour
  {
    public List<AttributeModifier> descriptors = new List<AttributeModifier>();

    protected override void OnPrefabInit() => base.OnPrefabInit();

    public void AddAttributeDescriptor(AttributeModifier modifier) => this.descriptors.Add(modifier);

    public void RemovePrefabAttribute(AttributeModifier modifier) => this.descriptors.Remove(modifier);
  }
}
