// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.TagGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class TagGrowthRule : GrowthRule
  {
    public Tag tag;

    public TagGrowthRule(Tag tag) => this.tag = tag;

    public override bool Test(Element e) => e.HasTag(this.tag);

    public override string Name() => this.tag.ProperName();
  }
}
