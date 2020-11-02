// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.ElementGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class ElementGrowthRule : GrowthRule
  {
    public SimHashes element;

    public ElementGrowthRule(SimHashes element) => this.element = element;

    public override bool Test(Element e) => e.id == this.element;

    public override string Name() => ElementLoader.FindElementByHash(this.element).name;
  }
}
