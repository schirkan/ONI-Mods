// Decompiled with JetBrains decompiler
// Type: DiseaseInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseInfoScreen : TargetScreen
{
  private CollapsibleDetailContentPanel infectionPanel;
  private CollapsibleDetailContentPanel immuneSystemPanel;
  private CollapsibleDetailContentPanel diseaseSourcePanel;
  private CollapsibleDetailContentPanel currentGermsPanel;
  private CollapsibleDetailContentPanel infoPanel;
  private static readonly EventSystem.IntraObjectHandler<DiseaseInfoScreen> OnRefreshDataDelegate = new EventSystem.IntraObjectHandler<DiseaseInfoScreen>((System.Action<DiseaseInfoScreen, object>) ((component, data) => component.OnRefreshData(data)));

  public override bool IsValidForTarget(GameObject target) => (UnityEngine.Object) target.GetComponent<CellSelectionObject>() != (UnityEngine.Object) null || (UnityEngine.Object) target.GetComponent<PrimaryElement>() != (UnityEngine.Object) null;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.diseaseSourcePanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject).GetComponent<CollapsibleDetailContentPanel>();
    this.diseaseSourcePanel.SetTitle((string) UI.DETAILTABS.DISEASE.DISEASE_SOURCE);
    this.immuneSystemPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject).GetComponent<CollapsibleDetailContentPanel>();
    this.immuneSystemPanel.SetTitle((string) UI.DETAILTABS.DISEASE.IMMUNE_SYSTEM);
    this.currentGermsPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject).GetComponent<CollapsibleDetailContentPanel>();
    this.currentGermsPanel.SetTitle((string) UI.DETAILTABS.DISEASE.CURRENT_GERMS);
    this.infoPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject).GetComponent<CollapsibleDetailContentPanel>();
    this.infoPanel.SetTitle((string) UI.DETAILTABS.DISEASE.GERMS_INFO);
    this.infectionPanel = Util.KInstantiateUI(ScreenPrefabs.Instance.CollapsableContentPanel, this.gameObject).GetComponent<CollapsibleDetailContentPanel>();
    this.infectionPanel.SetTitle((string) UI.DETAILTABS.DISEASE.INFECTION_INFO);
    this.Subscribe<DiseaseInfoScreen>(-1514841199, DiseaseInfoScreen.OnRefreshDataDelegate);
  }

  private void LateUpdate() => this.Refresh();

  private void OnRefreshData(object obj) => this.Refresh();

  private void Refresh()
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
      return;
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(this.selectedTarget, true);
    Sicknesses sicknesses = this.selectedTarget.GetSicknesses();
    if (sicknesses != null)
    {
      for (int idx = 0; idx < sicknesses.Count; ++idx)
        allDescriptors.AddRange((IEnumerable<Descriptor>) sicknesses[idx].GetDescriptors());
    }
    List<Descriptor> all = allDescriptors.FindAll((Predicate<Descriptor>) (e => e.type == Descriptor.DescriptorType.DiseaseSource));
    if (all.Count > 0)
    {
      for (int index = 0; index < all.Count; ++index)
        this.diseaseSourcePanel.SetLabel("source_" + index.ToString(), all[index].text, all[index].tooltipText);
    }
    this.CreateImmuneInfo();
    if (!this.CreateDiseaseInfo())
    {
      this.currentGermsPanel.SetTitle((string) UI.DETAILTABS.DISEASE.NO_CURRENT_GERMS);
      this.currentGermsPanel.SetLabel("nodisease", (string) UI.DETAILTABS.DISEASE.DETAILS.NODISEASE, (string) UI.DETAILTABS.DISEASE.DETAILS.NODISEASE_TOOLTIP);
    }
    this.diseaseSourcePanel.Commit();
    this.immuneSystemPanel.Commit();
    this.currentGermsPanel.Commit();
    this.infoPanel.Commit();
    this.infectionPanel.Commit();
  }

  private bool CreateImmuneInfo()
  {
    GermExposureMonitor.Instance smi = this.selectedTarget.GetSMI<GermExposureMonitor.Instance>();
    if (smi == null)
      return false;
    this.immuneSystemPanel.SetTitle((string) UI.DETAILTABS.DISEASE.CONTRACTION_RATES);
    this.immuneSystemPanel.SetLabel("germ_resistance", Db.Get().Attributes.GermResistance.Name + ": " + (object) smi.GetGermResistance(), (string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.DESC);
    for (int idx = 0; idx < Db.Get().Diseases.Count; ++idx)
    {
      Klei.AI.Disease disease = Db.Get().Diseases[idx];
      ExposureType exposureTypeForDisease = GameUtil.GetExposureTypeForDisease(disease);
      Sickness sicknessForDisease = GameUtil.GetSicknessForDisease(disease);
      bool flag1 = true;
      List<string> stringList1 = new List<string>();
      if (exposureTypeForDisease.required_traits != null && exposureTypeForDisease.required_traits.Count > 0)
      {
        for (int index = 0; index < exposureTypeForDisease.required_traits.Count; ++index)
        {
          if (!this.selectedTarget.GetComponent<Traits>().HasTrait(exposureTypeForDisease.required_traits[index]))
            stringList1.Add(exposureTypeForDisease.required_traits[index]);
        }
        if (stringList1.Count > 0)
          flag1 = false;
      }
      bool flag2 = false;
      List<string> stringList2 = new List<string>();
      if (exposureTypeForDisease.excluded_effects != null && exposureTypeForDisease.excluded_effects.Count > 0)
      {
        for (int index = 0; index < exposureTypeForDisease.excluded_effects.Count; ++index)
        {
          if (this.selectedTarget.GetComponent<Effects>().HasEffect(exposureTypeForDisease.excluded_effects[index]))
            stringList2.Add(exposureTypeForDisease.excluded_effects[index]);
        }
        if (stringList2.Count > 0)
          flag2 = true;
      }
      bool flag3 = false;
      List<string> stringList3 = new List<string>();
      if (exposureTypeForDisease.excluded_traits != null && exposureTypeForDisease.excluded_traits.Count > 0)
      {
        for (int index = 0; index < exposureTypeForDisease.excluded_traits.Count; ++index)
        {
          if (this.selectedTarget.GetComponent<Traits>().HasTrait(exposureTypeForDisease.excluded_traits[index]))
            stringList3.Add(exposureTypeForDisease.excluded_traits[index]);
        }
        if (stringList3.Count > 0)
          flag3 = true;
      }
      string str1 = "";
      float num;
      if (!flag1)
      {
        num = 0.0f;
        string str2 = "";
        for (int index = 0; index < stringList1.Count; ++index)
        {
          if (str2 != "")
            str2 += ", ";
          str2 += Db.Get().traits.Get(stringList1[index]).Name;
        }
        str1 += string.Format((string) DUPLICANTS.DISEASES.IMMUNE_FROM_MISSING_REQUIRED_TRAIT, (object) str2);
      }
      else if (flag3)
      {
        num = 0.0f;
        string str2 = "";
        for (int index = 0; index < stringList3.Count; ++index)
        {
          if (str2 != "")
            str2 += ", ";
          str2 += Db.Get().traits.Get(stringList3[index]).Name;
        }
        if (str1 != "")
          str1 += "\n";
        str1 += string.Format((string) DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT, (object) str2);
      }
      else if (flag2)
      {
        num = 0.0f;
        string str2 = "";
        for (int index = 0; index < stringList2.Count; ++index)
        {
          if (str2 != "")
            str2 += ", ";
          str2 += Db.Get().effects.Get(stringList2[index]).Name;
        }
        if (str1 != "")
          str1 += "\n";
        str1 += string.Format((string) DUPLICANTS.DISEASES.IMMUNE_FROM_HAVING_EXCLUDED_EFFECT, (object) str2);
      }
      else
        num = !exposureTypeForDisease.infect_immediately ? GermExposureMonitor.GetContractionChance(smi.GetResistanceToExposureType(exposureTypeForDisease, 3f)) : 1f;
      string str3 = str1 != "" ? str1 : string.Format((string) DUPLICANTS.DISEASES.CONTRACTION_PROBABILITY, (object) GameUtil.GetFormattedPercent(num * 100f), (object) this.selectedTarget.GetProperName(), (object) sicknessForDisease.Name);
      this.immuneSystemPanel.SetLabel("disease_" + disease.Id, "    • " + disease.Name + ": " + GameUtil.GetFormattedPercent(num * 100f), string.Format((string) DUPLICANTS.DISEASES.RESISTANCES_PANEL_TOOLTIP, (object) str3, (object) sicknessForDisease.Name));
    }
    return true;
  }

  private bool CreateDiseaseInfo()
  {
    if ((UnityEngine.Object) this.selectedTarget.GetComponent<PrimaryElement>() != (UnityEngine.Object) null)
      return this.CreateDiseaseInfo_PrimaryElement();
    CellSelectionObject component = this.selectedTarget.GetComponent<CellSelectionObject>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && this.CreateDiseaseInfo_CellSelectionObject(component);
  }

  private string GetFormattedHalfLife(float hl) => this.GetFormattedGrowthRate(Klei.AI.Disease.HalfLifeToGrowthRate(hl, 600f));

  private string GetFormattedGrowthRate(float rate)
  {
    if ((double) rate < 1.0)
      return string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT, (object) GameUtil.GetFormattedPercent((float) (100.0 * (1.0 - (double) rate))), (object) UI.DETAILTABS.DISEASE.DETAILS.DEATH_FORMAT_TOOLTIP);
    return (double) rate > 1.0 ? string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT, (object) GameUtil.GetFormattedPercent((float) (100.0 * ((double) rate - 1.0))), (object) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FORMAT_TOOLTIP) : string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT, (object) UI.DETAILTABS.DISEASE.DETAILS.NEUTRAL_FORMAT_TOOLTIP);
  }

  private string GetFormattedGrowthEntry(
    string name,
    float halfLife,
    string dyingFormat,
    string growingFormat,
    string neutralFormat)
  {
    return string.Format((double) halfLife != double.PositiveInfinity ? ((double) halfLife <= 0.0 ? growingFormat : dyingFormat) : neutralFormat, (object) name, (object) this.GetFormattedHalfLife(halfLife));
  }

  private void BuildFactorsStrings(
    int diseaseCount,
    int elementIdx,
    int environmentCell,
    float environmentMass,
    float temperature,
    HashSet<Tag> tags,
    Klei.AI.Disease disease)
  {
    this.currentGermsPanel.SetTitle(string.Format((string) UI.DETAILTABS.DISEASE.CURRENT_GERMS, (object) disease.Name.ToUpper()));
    this.currentGermsPanel.SetLabel("currentgerms", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(diseaseCount)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.DISEASE_AMOUNT_TOOLTIP, (object) GameUtil.GetFormattedDiseaseAmount(diseaseCount)));
    Element element = ElementLoader.elements[elementIdx];
    CompositeGrowthRule growthRuleForElement = disease.GetGrowthRuleForElement(element);
    float tags_multiplier_base = 1f;
    if (tags != null && tags.Count > 0)
      tags_multiplier_base = disease.GetGrowthRateForTags(tags, (double) diseaseCount > (double) growthRuleForElement.maxCountPerKG * (double) environmentMass);
    float delta = DiseaseContainers.CalculateDelta(diseaseCount, elementIdx, environmentMass, environmentCell, temperature, tags_multiplier_base, disease, 1f);
    this.currentGermsPanel.SetLabel("finaldelta", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE, (object) GameUtil.GetFormattedSimple(delta, GameUtil.TimeSlice.PerSecond, "F0")), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.RATE_OF_CHANGE_TOOLTIP, (object) GameUtil.GetFormattedSimple(delta, GameUtil.TimeSlice.PerSecond, "F0")));
    float halfLife = Klei.AI.Disease.GrowthRateToHalfLife((float) (1.0 - (double) delta / (double) diseaseCount));
    if ((double) halfLife > 0.0)
      this.currentGermsPanel.SetLabel("finalhalflife", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG, (object) GameUtil.GetFormattedCycles(halfLife)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEG_TOOLTIP, (object) GameUtil.GetFormattedCycles(halfLife)));
    else if ((double) halfLife < 0.0)
      this.currentGermsPanel.SetLabel("finalhalflife", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS, (object) GameUtil.GetFormattedCycles(-halfLife)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_POS_TOOLTIP, (object) GameUtil.GetFormattedCycles(halfLife)));
    else
      this.currentGermsPanel.SetLabel("finalhalflife", (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.HALF_LIFE_NEUTRAL_TOOLTIP);
    this.currentGermsPanel.SetLabel("factors", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TITLE, (object[]) Array.Empty<object>()), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TOOLTIP);
    bool flag = false;
    if ((double) diseaseCount < (double) growthRuleForElement.minCountPerKG * (double) environmentMass)
    {
      this.currentGermsPanel.SetLabel("critical_status", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TITLE, (object) this.GetFormattedGrowthRate(-growthRuleForElement.underPopulationDeathRate)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.DYING_OFF.TOOLTIP, (object) GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.minCountPerKG * environmentMass)), (object) GameUtil.GetFormattedMass(environmentMass), (object) growthRuleForElement.minCountPerKG));
      flag = true;
    }
    else if ((double) diseaseCount > (double) growthRuleForElement.maxCountPerKG * (double) environmentMass)
    {
      this.currentGermsPanel.SetLabel("critical_status", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TITLE, (object) this.GetFormattedHalfLife(growthRuleForElement.overPopulationHalfLife)), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.OVERPOPULATED.TOOLTIP, (object) GameUtil.GetFormattedDiseaseAmount(Mathf.RoundToInt(growthRuleForElement.maxCountPerKG * environmentMass)), (object) GameUtil.GetFormattedMass(environmentMass), (object) growthRuleForElement.maxCountPerKG));
      flag = true;
    }
    if (!flag)
      this.currentGermsPanel.SetLabel("substrate", this.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), this.GetFormattedGrowthEntry(growthRuleForElement.Name(), growthRuleForElement.populationHalfLife, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
    int num = 0;
    if (tags != null)
    {
      foreach (Tag tag in tags)
      {
        TagGrowthRule growthRuleForTag = disease.GetGrowthRuleForTag(tag);
        if (growthRuleForTag != null)
          this.currentGermsPanel.SetLabel("tag_" + (object) num, this.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL), this.GetFormattedGrowthEntry(growthRuleForTag.Name(), growthRuleForTag.populationHalfLife.Value, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.DIE_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.GROW_TOOLTIP, (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.SUBSTRATE.NEUTRAL_TOOLTIP));
        ++num;
      }
    }
    if (Grid.IsValidCell(environmentCell))
    {
      CompositeExposureRule exposureRuleForElement = disease.GetExposureRuleForElement(Grid.Element[environmentCell]);
      if (exposureRuleForElement != null && (double) exposureRuleForElement.populationHalfLife != double.PositiveInfinity)
      {
        if ((double) exposureRuleForElement.GetHalfLifeForCount(diseaseCount) > 0.0)
          this.currentGermsPanel.SetLabel("environment", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, (object) exposureRuleForElement.Name(), (object) this.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.DIE_TOOLTIP);
        else
          this.currentGermsPanel.SetLabel("environment", string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.TITLE, (object) exposureRuleForElement.Name(), (object) this.GetFormattedHalfLife(exposureRuleForElement.GetHalfLifeForCount(diseaseCount))), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.ENVIRONMENT.GROW_TOOLTIP);
      }
    }
    float temperatureHalfLife = disease.CalculateTemperatureHalfLife(temperature);
    if ((double) temperatureHalfLife == double.PositiveInfinity)
      return;
    if ((double) temperatureHalfLife > 0.0)
      this.currentGermsPanel.SetLabel(nameof (temperature), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, (object) GameUtil.GetFormattedTemperature(temperature), (object) this.GetFormattedHalfLife(temperatureHalfLife)), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.DIE_TOOLTIP);
    else
      this.currentGermsPanel.SetLabel(nameof (temperature), string.Format((string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.TITLE, (object) GameUtil.GetFormattedTemperature(temperature), (object) this.GetFormattedHalfLife(temperatureHalfLife)), (string) UI.DETAILTABS.DISEASE.DETAILS.GROWTH_FACTORS.TEMPERATURE.GROW_TOOLTIP);
  }

  private bool CreateDiseaseInfo_PrimaryElement()
  {
    if ((UnityEngine.Object) this.selectedTarget == (UnityEngine.Object) null)
      return false;
    PrimaryElement component1 = this.selectedTarget.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null || component1.DiseaseIdx == byte.MaxValue || component1.DiseaseCount <= 0)
      return false;
    Klei.AI.Disease disease = Db.Get().Diseases[(int) component1.DiseaseIdx];
    int cell = Grid.PosToCell(component1.transform.GetPosition());
    KPrefabID component2 = component1.GetComponent<KPrefabID>();
    this.BuildFactorsStrings(component1.DiseaseCount, (int) component1.Element.idx, cell, component1.Mass, component1.Temperature, component2.Tags, disease);
    return true;
  }

  private bool CreateDiseaseInfo_CellSelectionObject(CellSelectionObject cso)
  {
    if (cso.diseaseIdx == byte.MaxValue || cso.diseaseCount <= 0)
      return false;
    Klei.AI.Disease disease = Db.Get().Diseases[(int) cso.diseaseIdx];
    int idx = (int) cso.element.idx;
    this.BuildFactorsStrings(cso.diseaseCount, idx, -1, cso.Mass, cso.temperature, (HashSet<Tag>) null, disease);
    return true;
  }
}
