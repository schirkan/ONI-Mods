﻿// Decompiled with JetBrains decompiler
// Type: TemplateSelectionInfoPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/TemplateSelectionInfoPanel")]
public class TemplateSelectionInfoPanel : KMonoBehaviour, IRender1000ms
{
  [SerializeField]
  private GameObject prefab_detail_label;
  [SerializeField]
  private GameObject current_detail_container;
  [SerializeField]
  private LocText saved_detail_label;
  [SerializeField]
  private KButton save_button;
  private Func<List<int>, string>[] details = new Func<List<int>, string>[6]
  {
    new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalMass),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageMass),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.AverageTemperature),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.TotalJoules),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.JoulesPerKilogram),
    new Func<List<int>, string>(TemplateSelectionInfoPanel.MassPerElement)
  };
  private static List<Tuple<Element, float>> mass_per_element = new List<Tuple<Element, float>>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    for (int index = 0; index < this.details.Length; ++index)
      Util.KInstantiateUI(this.prefab_detail_label, this.current_detail_container, true);
    this.RefreshDetails();
    this.save_button.onClick += new System.Action(this.SaveCurrentDetails);
  }

  public void SaveCurrentDetails()
  {
    string str = "";
    for (int index = 0; index < this.details.Length; ++index)
      str = str + this.details[index](DebugBaseTemplateButton.Instance.SelectedCells) + "\n";
    this.saved_detail_label.text = str + UI.HORIZONTAL_BR_RULE + this.saved_detail_label.text;
  }

  public void Render1000ms(float dt) => this.RefreshDetails();

  public void RefreshDetails()
  {
    for (int index = 0; index < this.details.Length; ++index)
      this.current_detail_container.transform.GetChild(index).GetComponent<LocText>().text = this.details[index](DebugBaseTemplateButton.Instance.SelectedCells);
  }

  private static string TotalMass(List<int> cells)
  {
    float mass = 0.0f;
    foreach (int cell in cells)
      mass += Grid.Mass[cell];
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_MASS, (object) GameUtil.GetFormattedMass(mass));
  }

  private static string AverageMass(List<int> cells)
  {
    float num = 0.0f;
    foreach (int cell in cells)
      num += Grid.Mass[cell];
    float mass = num / (float) cells.Count;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_MASS, (object) GameUtil.GetFormattedMass(mass));
  }

  private static string AverageTemperature(List<int> cells)
  {
    float num = 0.0f;
    foreach (int cell in cells)
      num += Grid.Temperature[cell];
    float temp = num / (float) cells.Count;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.AVERAGE_TEMPERATURE, (object) GameUtil.GetFormattedTemperature(temp));
  }

  private static string TotalJoules(List<int> cells)
  {
    float joules = 0.0f;
    foreach (int cell in cells)
      joules += (float) ((double) Grid.Element[cell].specificHeatCapacity * (double) Grid.Temperature[cell] * ((double) Grid.Mass[cell] * 1000.0));
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.TOTAL_JOULES, (object) GameUtil.GetFormattedJoules(joules));
  }

  private static string JoulesPerKilogram(List<int> cells)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (int cell in cells)
    {
      num1 += (float) ((double) Grid.Element[cell].specificHeatCapacity * (double) Grid.Temperature[cell] * ((double) Grid.Mass[cell] * 1000.0));
      num2 += Grid.Mass[cell];
    }
    float joules = num1 / num2;
    return string.Format((string) UI.DEBUG_TOOLS.SAVE_BASE_TEMPLATE.SELECTION_INFO_PANEL.JOULES_PER_KILOGRAM, (object) GameUtil.GetFormattedJoules(joules));
  }

  private static string MassPerElement(List<int> cells)
  {
    TemplateSelectionInfoPanel.mass_per_element.Clear();
    foreach (int cell in cells)
    {
      bool flag = false;
      for (int index = 0; index < TemplateSelectionInfoPanel.mass_per_element.Count; ++index)
      {
        if (TemplateSelectionInfoPanel.mass_per_element[index].first == Grid.Element[cell])
        {
          TemplateSelectionInfoPanel.mass_per_element[index].second += Grid.Mass[cell];
          flag = true;
          break;
        }
      }
      if (!flag)
        TemplateSelectionInfoPanel.mass_per_element.Add(new Tuple<Element, float>(Grid.Element[cell], Grid.Mass[cell]));
    }
    TemplateSelectionInfoPanel.mass_per_element.Sort((Comparison<Tuple<Element, float>>) ((a, b) =>
    {
      if ((double) a.second > (double) b.second)
        return -1;
      return (double) b.second > (double) a.second ? 1 : 0;
    }));
    string str = "";
    foreach (Tuple<Element, float> tuple in TemplateSelectionInfoPanel.mass_per_element)
      str = str + tuple.first.name + ": " + GameUtil.GetFormattedMass(tuple.second) + "\n";
    return str;
  }
}
