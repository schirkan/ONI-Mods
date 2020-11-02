// Decompiled with JetBrains decompiler
// Type: VitalsTableScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VitalsTableScreen : TableScreen
{
  protected override void OnActivate()
  {
    this.has_default_duplicant_row = false;
    this.title = (string) UI.VITALS;
    base.OnActivate();
    this.AddPortraitColumn("Portrait", new System.Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_portrait), (Comparison<IAssignableIdentity>) null);
    this.AddButtonLabelColumn("Names", new System.Action<IAssignableIdentity, GameObject>(((TableScreen) this).on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(((TableScreen) this).get_value_name_label), (System.Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectMinion()), (System.Action<GameObject>) (widget_go => this.GetWidgetRow(widget_go).SelectAndFocusMinion()), new Comparison<IAssignableIdentity>(((TableScreen) this).compare_rows_alphabetical), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new System.Action<IAssignableIdentity, GameObject, ToolTip>(((TableScreen) this).on_tooltip_sort_alphabetically));
    this.AddLabelColumn("Stress", new System.Action<IAssignableIdentity, GameObject>(this.on_load_stress), new Func<IAssignableIdentity, GameObject, string>(this.get_value_stress_label), new Comparison<IAssignableIdentity>(this.compare_rows_stress), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_stress), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_stress), 64, true);
    this.AddLabelColumn("QOLExpectations", new System.Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_expectations_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), should_refresh_columns: true);
    this.AddLabelColumn("Fullness", new System.Action<IAssignableIdentity, GameObject>(this.on_load_fullness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_fullness_label), new Comparison<IAssignableIdentity>(this.compare_rows_fullness), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_fullness), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_fullness), 96, true);
    this.AddLabelColumn("EatenToday", new System.Action<IAssignableIdentity, GameObject>(this.on_load_eaten_today), new Func<IAssignableIdentity, GameObject, string>(this.get_value_eaten_today_label), new Comparison<IAssignableIdentity>(this.compare_rows_eaten_today), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_eaten_today), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_eaten_today), 96, true);
    this.AddLabelColumn("Health", new System.Action<IAssignableIdentity, GameObject>(this.on_load_health), new Func<IAssignableIdentity, GameObject, string>(this.get_value_health_label), new Comparison<IAssignableIdentity>(this.compare_rows_health), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_health), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_health), 64, true);
    this.AddLabelColumn("Immunity", new System.Action<IAssignableIdentity, GameObject>(this.on_load_sickness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_sickness_label), new Comparison<IAssignableIdentity>(this.compare_rows_sicknesses), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sicknesses), new System.Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_sicknesses), 192, true);
  }

  private void on_load_stress(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN.STRESS.ToString();
  }

  private string get_value_stress_label(IAssignableIdentity identity, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity minionIdentity = identity as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
        return Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).GetValueString();
    }
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      return (string) UI.TABLESCREENS.NA;
    return "";
  }

  private int compare_rows_stress(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minionIdentity1 = a as MinionIdentity;
    MinionIdentity minionIdentity2 = b as MinionIdentity;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null && (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 1;
    float num = Db.Get().Amounts.Stress.Lookup((Component) minionIdentity1).value;
    return Db.Get().Amounts.Stress.Lookup((Component) minionIdentity2).value.CompareTo(num);
  }

  protected void on_tooltip_stress(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = minion as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).GetTooltip(), (ScriptableObject) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_stress(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_STRESS, (ScriptableObject) null);
        break;
    }
  }

  private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString();
  }

  private string get_value_qualityoflife_expectations_label(
    IAssignableIdentity identity,
    GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity minionIdentity = identity as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
        return Db.Get().Attributes.QualityOfLife.Lookup((Component) minionIdentity).GetFormattedValue();
    }
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      return (string) UI.TABLESCREENS.NA;
    return "";
  }

  private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minionIdentity1 = a as MinionIdentity;
    MinionIdentity minionIdentity2 = b as MinionIdentity;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null && (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null)
      return -1;
    return (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null ? 1 : Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) minionIdentity1).GetTotalValue().CompareTo(Db.Get().Attributes.QualityOfLifeExpectation.Lookup((Component) minionIdentity2).GetTotalValue());
  }

  protected void on_tooltip_qualityoflife_expectations(
    IAssignableIdentity identity,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = identity as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup((Component) minionIdentity).GetAttributeValueTooltip(), (ScriptableObject) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(identity, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_qualityoflife_expectations(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, (ScriptableObject) null);
        break;
    }
  }

  private void on_load_health(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : (componentInChildren.text = UI.VITALSSCREEN_HEALTH.ToString());
  }

  private string get_value_health_label(IAssignableIdentity minion, GameObject widget_go)
  {
    if (minion != null)
    {
      TableRow widgetRow = this.GetWidgetRow(widget_go);
      if (widgetRow.rowType == TableRow.RowType.Minion && (UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null)
        return Db.Get().Amounts.HitPoints.Lookup((Component) (minion as MinionIdentity)).GetValueString();
      if (widgetRow.rowType == TableRow.RowType.StoredMinon)
        return (string) UI.TABLESCREENS.NA;
    }
    return "";
  }

  private int compare_rows_health(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minionIdentity1 = a as MinionIdentity;
    MinionIdentity minionIdentity2 = b as MinionIdentity;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null && (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 1;
    float num = Db.Get().Amounts.HitPoints.Lookup((Component) minionIdentity1).value;
    return Db.Get().Amounts.HitPoints.Lookup((Component) minionIdentity2).value.CompareTo(num);
  }

  protected void on_tooltip_health(
    IAssignableIdentity identity,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = identity as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Amounts.HitPoints.Lookup((Component) minionIdentity).GetTooltip(), (ScriptableObject) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(identity, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_health(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_HITPOINTS, (ScriptableObject) null);
        break;
    }
  }

  private void on_load_sickness(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_SICKNESS.ToString();
  }

  private string get_value_sickness_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
    {
      MinionIdentity minionIdentity = minion as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
      {
        Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
        if (!sicknesses.IsInfected())
          return (string) UI.VITALSSCREEN.NO_SICKNESSES;
        string str = "";
        if (sicknesses.Count > 1)
        {
          float seconds = 0.0f;
          foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
            seconds = Mathf.Min(sicknessInstance.GetInfectedTimeRemaining());
          str += string.Format((string) UI.VITALSSCREEN.MULTIPLE_SICKNESSES, (object) GameUtil.GetFormattedCycles(seconds));
        }
        else
        {
          foreach (SicknessInstance sicknessInstance in (Modifications<Sickness, SicknessInstance>) sicknesses)
          {
            if (!string.IsNullOrEmpty(str))
              str += "\n";
            str += string.Format((string) UI.VITALSSCREEN.SICKNESS_REMAINING, (object) sicknessInstance.modifier.Name, (object) GameUtil.GetFormattedCycles(sicknessInstance.GetInfectedTimeRemaining()));
          }
        }
        return str;
      }
    }
    else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
      return (string) UI.TABLESCREENS.NA;
    return "";
  }

  private int compare_rows_sicknesses(IAssignableIdentity a, IAssignableIdentity b) => 0.0f.CompareTo(0.0f);

  protected void on_tooltip_sicknesses(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = minion as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
        if (sicknesses.IsInfected())
        {
          using (IEnumerator<SicknessInstance> enumerator = sicknesses.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              SicknessInstance current = enumerator.Current;
              tooltip.AddMultiStringTooltip(UI.HORIZONTAL_RULE, (ScriptableObject) null);
              tooltip.AddMultiStringTooltip(current.modifier.Name, (ScriptableObject) null);
              StatusItem statusItem = current.GetStatusItem();
              tooltip.AddMultiStringTooltip(statusItem.GetTooltip((object) current.ExposureInfo), (ScriptableObject) null);
            }
            break;
          }
        }
        else
        {
          tooltip.AddMultiStringTooltip((string) UI.VITALSSCREEN.NO_SICKNESSES, (ScriptableObject) null);
          break;
        }
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_sicknesses(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_SICKNESSES, (ScriptableObject) null);
        break;
    }
  }

  private void on_load_fullness(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_CALORIES.ToString();
  }

  private string get_value_fullness_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion && (UnityEngine.Object) (minion as MinionIdentity) != (UnityEngine.Object) null)
      return Db.Get().Amounts.Calories.Lookup((Component) (minion as MinionIdentity)).GetValueString();
    return widgetRow.rowType == TableRow.RowType.StoredMinon ? (string) UI.TABLESCREENS.NA : "";
  }

  private int compare_rows_fullness(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minionIdentity1 = a as MinionIdentity;
    MinionIdentity minionIdentity2 = b as MinionIdentity;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null && (UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minionIdentity1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) minionIdentity2 == (UnityEngine.Object) null)
      return 1;
    float num = Db.Get().Amounts.Calories.Lookup((Component) minionIdentity1).value;
    return Db.Get().Amounts.Calories.Lookup((Component) minionIdentity2).value.CompareTo(num);
  }

  protected void on_tooltip_fullness(
    IAssignableIdentity identity,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        MinionIdentity minionIdentity = identity as MinionIdentity;
        if (!((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null))
          break;
        tooltip.AddMultiStringTooltip(Db.Get().Amounts.Calories.Lookup((Component) minionIdentity).GetTooltip(), (ScriptableObject) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(identity, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_fullness(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_FULLNESS, (ScriptableObject) null);
        break;
    }
  }

  protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        if (minion == null)
          break;
        tooltip.AddMultiStringTooltip(string.Format((string) UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, (object) minion.GetProperName()), (ScriptableObject) null);
        break;
    }
  }

  private void on_load_eaten_today(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
    if (minion != null)
      componentInChildren.text = (this.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
    else
      componentInChildren.text = widgetRow.isDefault ? "" : UI.VITALSSCREEN_EATENTODAY.ToString();
  }

  private static float RationsEatenToday(MinionIdentity minion)
  {
    float num = 0.0f;
    if ((UnityEngine.Object) minion != (UnityEngine.Object) null)
    {
      RationMonitor.Instance smi = minion.GetSMI<RationMonitor.Instance>();
      if (smi != null)
        num = smi.GetRationsAteToday();
    }
    return num;
  }

  private string get_value_eaten_today_label(IAssignableIdentity minion, GameObject widget_go)
  {
    TableRow widgetRow = this.GetWidgetRow(widget_go);
    if (widgetRow.rowType == TableRow.RowType.Minion)
      return GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minion as MinionIdentity));
    return widgetRow.rowType == TableRow.RowType.StoredMinon ? (string) UI.TABLESCREENS.NA : "";
  }

  private int compare_rows_eaten_today(IAssignableIdentity a, IAssignableIdentity b)
  {
    MinionIdentity minion1 = a as MinionIdentity;
    MinionIdentity minion2 = b as MinionIdentity;
    if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null && (UnityEngine.Object) minion2 == (UnityEngine.Object) null)
      return 0;
    if ((UnityEngine.Object) minion1 == (UnityEngine.Object) null)
      return -1;
    if ((UnityEngine.Object) minion2 == (UnityEngine.Object) null)
      return 1;
    float num = VitalsTableScreen.RationsEatenToday(minion1);
    return VitalsTableScreen.RationsEatenToday(minion2).CompareTo(num);
  }

  protected void on_tooltip_eaten_today(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Minion:
        if (minion == null)
          break;
        float calories = VitalsTableScreen.RationsEatenToday(minion as MinionIdentity);
        tooltip.AddMultiStringTooltip(string.Format((string) UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, (object) GameUtil.GetFormattedCalories(calories)), (ScriptableObject) null);
        break;
      case TableRow.RowType.StoredMinon:
        this.StoredMinionTooltip(minion, tooltip);
        break;
    }
  }

  protected void on_tooltip_sort_eaten_today(
    IAssignableIdentity minion,
    GameObject widget_go,
    ToolTip tooltip)
  {
    tooltip.ClearMultiStringTooltip();
    switch (this.GetWidgetRow(widget_go).rowType)
    {
      case TableRow.RowType.Header:
        tooltip.AddMultiStringTooltip((string) UI.TABLESCREENS.COLUMN_SORT_BY_EATEN_TODAY, (ScriptableObject) null);
        break;
    }
  }

  private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
  {
    if (minion == null || !((UnityEngine.Object) (minion as StoredMinionIdentity) != (UnityEngine.Object) null))
      return;
    tooltip.AddMultiStringTooltip(string.Format((string) UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (object) (minion as StoredMinionIdentity).GetStorageReason(), (object) minion.GetProperName()), (ScriptableObject) null);
  }
}
