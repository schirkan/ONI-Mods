// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.StateGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class StateGrowthRule : GrowthRule
  {
    public Element.State state;

    public StateGrowthRule(Element.State state) => this.state = state;

    public override bool Test(Element e) => e.IsState(this.state);

    public override string Name() => Element.GetStateString(this.state);
  }
}
