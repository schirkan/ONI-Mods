// Decompiled with JetBrains decompiler
// Type: GermResistanceAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
  public GermResistanceAttributeFormatter()
    : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
  {
  }

  public override string GetFormattedModifier(
    AttributeModifier modifier,
    GameObject parent_instance)
  {
    return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
  }
}
