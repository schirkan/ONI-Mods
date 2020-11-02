// Decompiled with JetBrains decompiler
// Type: Klei.AI.Amounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class Amounts : Modifications<Amount, AmountInstance>
  {
    public Amounts(GameObject go)
      : base(go)
    {
    }

    public float GetValue(string amount_id) => this.Get(amount_id).value;

    public void SetValue(string amount_id, float value) => this.Get(amount_id).value = value;

    public override AmountInstance Add(AmountInstance instance)
    {
      instance.Activate();
      return base.Add(instance);
    }

    public override void Remove(AmountInstance instance)
    {
      instance.Deactivate();
      base.Remove(instance);
    }

    public void Cleanup()
    {
      for (int idx = 0; idx < this.Count; ++idx)
        this[idx].Deactivate();
    }
  }
}
