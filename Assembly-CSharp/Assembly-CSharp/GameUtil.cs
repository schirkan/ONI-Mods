// Decompiled with JetBrains decompiler
// Type: GameUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public static class GameUtil
{
  public static GameUtil.TemperatureUnit temperatureUnit;
  public static GameUtil.MassUnit massUnit;
  private static string[] adjectives;
  [ThreadStatic]
  public static Queue<GameUtil.FloodFillInfo> FloodFillNext = new Queue<GameUtil.FloodFillInfo>();
  [ThreadStatic]
  public static HashSet<int> FloodFillVisited = new HashSet<int>();
  public static TagSet foodTags = new TagSet(new string[10]
  {
    "BasicPlantFood",
    "MushBar",
    "ColdWheatSeed",
    "ColdWheatSeed",
    "SpiceNut",
    "PrickleFruit",
    "Meat",
    "Mushroom",
    "ColdWheat",
    GameTags.Compostable.Name
  });
  public static TagSet solidTags = new TagSet(new string[5]
  {
    "Filter",
    "Coal",
    "BasicFabric",
    "SwampLilyFlower",
    "RefinedMetal"
  });

  public static string GetTemperatureUnitSuffix()
  {
    string str;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        str = (string) UI.UNITSUFFIXES.TEMPERATURE.CELSIUS;
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        str = (string) UI.UNITSUFFIXES.TEMPERATURE.FAHRENHEIT;
        break;
      default:
        str = (string) UI.UNITSUFFIXES.TEMPERATURE.KELVIN;
        break;
    }
    return str;
  }

  private static string AddTemperatureUnitSuffix(string text) => text + GameUtil.GetTemperatureUnitSuffix();

  public static float GetTemperatureConvertedFromKelvin(
    float temperature,
    GameUtil.TemperatureUnit targetUnit)
  {
    if (targetUnit == GameUtil.TemperatureUnit.Celsius)
      return temperature - 273.15f;
    return targetUnit == GameUtil.TemperatureUnit.Fahrenheit ? (float) ((double) temperature * 1.79999995231628 - 459.670013427734) : temperature;
  }

  public static float GetConvertedTemperature(float temperature, bool roundOutput = false)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        float f1 = temperature - 273.15f;
        return !roundOutput ? f1 : Mathf.Round(f1);
      case GameUtil.TemperatureUnit.Fahrenheit:
        float f2 = (float) ((double) temperature * 1.79999995231628 - 459.670013427734);
        return !roundOutput ? f2 : Mathf.Round(f2);
      default:
        return !roundOutput ? temperature : Mathf.Round(temperature);
    }
  }

  public static float GetTemperatureConvertedToKelvin(
    float temperature,
    GameUtil.TemperatureUnit fromUnit)
  {
    if (fromUnit == GameUtil.TemperatureUnit.Celsius)
      return temperature + 273.15f;
    return fromUnit == GameUtil.TemperatureUnit.Fahrenheit ? (float) (((double) temperature + 459.670013427734) * 5.0 / 9.0) : temperature;
  }

  public static float GetTemperatureConvertedToKelvin(float temperature)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return temperature + 273.15f;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return (float) (((double) temperature + 459.670013427734) * 5.0 / 9.0);
      default:
        return temperature;
    }
  }

  private static float GetConvertedTemperatureDelta(float kelvin_delta)
  {
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        return kelvin_delta;
      case GameUtil.TemperatureUnit.Fahrenheit:
        return kelvin_delta * 1.8f;
      case GameUtil.TemperatureUnit.Kelvin:
        return kelvin_delta;
      default:
        return kelvin_delta;
    }
  }

  public static float ApplyTimeSlice(float val, GameUtil.TimeSlice timeSlice) => timeSlice == GameUtil.TimeSlice.PerCycle ? val * 600f : val;

  public static string AddTimeSliceText(string text, GameUtil.TimeSlice timeSlice)
  {
    switch (timeSlice)
    {
      case GameUtil.TimeSlice.PerSecond:
        return text + (string) UI.UNITSUFFIXES.PERSECOND;
      case GameUtil.TimeSlice.PerCycle:
        return text + (string) UI.UNITSUFFIXES.PERCYCLE;
      default:
        return text;
    }
  }

  public static string AddPositiveSign(string text, bool positive) => positive ? string.Format((string) UI.POSITIVE_FORMAT, (object) text) : text;

  public static float AttributeSkillToAlpha(AttributeInstance attributeInstance) => Mathf.Min(attributeInstance.GetTotalValue() / 10f, 1f);

  public static float AttributeSkillToAlpha(float attributeSkill) => Mathf.Min(attributeSkill / 10f, 1f);

  public static float AptitudeToAlpha(float aptitude) => Mathf.Min(aptitude / 10f, 1f);

  public static float GetThermalEnergy(PrimaryElement pe) => pe.Temperature * pe.Mass * pe.Element.specificHeatCapacity;

  public static float CalculateTemperatureChange(float shc, float mass, float kilowatts) => kilowatts / (shc * mass);

  public static void DeltaThermalEnergy(
    PrimaryElement pe,
    float kilowatts,
    float targetTemperature)
  {
    float temperatureChange = GameUtil.CalculateTemperatureChange(pe.Element.specificHeatCapacity, pe.Mass, kilowatts);
    float num1 = pe.Temperature + temperatureChange;
    float num2 = (double) targetTemperature <= (double) pe.Temperature ? Mathf.Clamp(num1, targetTemperature, pe.Temperature) : Mathf.Clamp(num1, pe.Temperature, targetTemperature);
    pe.Temperature = num2;
  }

  public static BindingEntry ActionToBinding(Action action)
  {
    foreach (BindingEntry keyBinding in GameInputMapping.KeyBindings)
    {
      if (keyBinding.mAction == action)
        return keyBinding;
    }
    throw new ArgumentException(action.ToString() + " is not bound in GameInputBindings");
  }

  public static string GetIdentityDescriptor(GameObject go)
  {
    if ((bool) (UnityEngine.Object) go.GetComponent<MinionIdentity>())
      return (string) DUPLICANTS.STATS.SUBJECTS.DUPLICANT;
    return (bool) (UnityEngine.Object) go.GetComponent<CreatureBrain>() ? (string) DUPLICANTS.STATS.SUBJECTS.CREATURE : (string) DUPLICANTS.STATS.SUBJECTS.PLANT;
  }

  public static float GetEnergyInPrimaryElement(PrimaryElement element) => (float) (1.0 / 1000.0 * ((double) element.Temperature * ((double) element.Mass * 1000.0 * (double) element.Element.specificHeatCapacity)));

  public static float EnergyToTemperatureDelta(float kilojoules, PrimaryElement element)
  {
    Debug.Assert((double) element.Mass > 0.0);
    double num1 = (double) Mathf.Max(GameUtil.GetEnergyInPrimaryElement(element) - kilojoules, 1f);
    float temperature = element.Temperature;
    double num2 = 1.0 / 1000.0 * ((double) element.Mass * ((double) element.Element.specificHeatCapacity * 1000.0));
    return (float) (num1 / num2) - temperature;
  }

  public static float CalculateEnergyDeltaForElement(
    PrimaryElement element,
    float startTemp,
    float endTemp)
  {
    return GameUtil.CalculateEnergyDeltaForElementChange(element.Mass, element.Element.specificHeatCapacity, startTemp, endTemp);
  }

  public static float CalculateEnergyDeltaForElementChange(
    float mass,
    float shc,
    float startTemp,
    float endTemp)
  {
    return (endTemp - startTemp) * mass * shc;
  }

  public static float GetFinalTemperature(float t1, float m1, float t2, float m2)
  {
    float num1 = m1 + m2;
    float num2 = (float) ((double) t1 * (double) m1 + (double) t2 * (double) m2) / num1;
    float min = Mathf.Min(t1, t2);
    float max = Mathf.Max(t1, t2);
    float f = Mathf.Clamp(num2, min, max);
    if (float.IsNaN(f) || float.IsInfinity(f))
      Debug.LogError((object) string.Format("Calculated an invalid temperature: t1={0}, m1={1}, t2={2}, m2={3}, min_temp={4}, max_temp={5}", (object) t1, (object) m1, (object) t2, (object) m2, (object) min, (object) max));
    return f;
  }

  public static void ForceTotalConduction(PrimaryElement a, PrimaryElement b)
  {
    float num1 = a.Temperature * a.Element.specificHeatCapacity * a.Mass;
    float temperature1 = a.Temperature;
    float num2 = b.Temperature * b.Element.specificHeatCapacity * b.Mass;
    float temperature2 = b.Temperature;
    float num3 = num2 / (num1 + num2);
    a.Temperature = (temperature2 - temperature1) * num3 + temperature1;
    b.Temperature = (float) (((double) temperature1 - (double) temperature2) * 1.0) - num3 + temperature2;
  }

  public static string FloatToString(float f, string format = null)
  {
    if (float.IsPositiveInfinity(f))
      return (string) UI.POS_INFINITY;
    return float.IsNegativeInfinity(f) ? (string) UI.NEG_INFINITY : f.ToString(format);
  }

  public static string GetUnitFormattedName(GameObject go, bool upperName = false)
  {
    KPrefabID component1 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && Assets.IsTagCountable(component1.PrefabTag))
    {
      PrimaryElement component2 = go.GetComponent<PrimaryElement>();
      return GameUtil.GetUnitFormattedName(go.GetProperName(), component2.Units, upperName);
    }
    return !upperName ? go.GetProperName() : StringFormatter.ToUpper(go.GetProperName());
  }

  public static string GetUnitFormattedName(string name, float count, bool upperName = false)
  {
    if (upperName)
      name = name.ToUpper();
    return StringFormatter.Replace((string) UI.NAME_WITH_UNITS, "{0}", name).Replace("{1}", string.Format("{0:0.##}", (object) count));
  }

  public static string GetFormattedUnits(
    float units,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool displaySuffix = true)
  {
    string str = (string) ((double) units == 1.0 ? UI.UNITSUFFIXES.UNIT : UI.UNITSUFFIXES.UNITS);
    units = GameUtil.ApplyTimeSlice(units, timeSlice);
    string text = (double) units != 0.0 ? ((double) Mathf.Abs(units) >= 1.0 ? ((double) Mathf.Abs(units) >= 10.0 ? GameUtil.FloatToString(units, "#,###") : GameUtil.FloatToString(units, "#,###.#")) : GameUtil.FloatToString(units, "#,##0.#")) : "0";
    if (displaySuffix)
      text += str;
    return GameUtil.AddTimeSliceText(text, timeSlice);
  }

  public static string ApplyBoldString(string source) => "<b>" + source + "</b>";

  public static float GetRoundedTemperatureInKelvin(float kelvin)
  {
    float num = 0.0f;
    switch (GameUtil.temperatureUnit)
    {
      case GameUtil.TemperatureUnit.Celsius:
        num = GameUtil.GetTemperatureConvertedToKelvin(Mathf.Round(GameUtil.GetConvertedTemperature(Mathf.Round(kelvin), true)));
        break;
      case GameUtil.TemperatureUnit.Fahrenheit:
        num = GameUtil.GetTemperatureConvertedToKelvin((float) Mathf.RoundToInt(GameUtil.GetTemperatureConvertedFromKelvin(kelvin, GameUtil.TemperatureUnit.Fahrenheit)), GameUtil.TemperatureUnit.Fahrenheit);
        break;
      case GameUtil.TemperatureUnit.Kelvin:
        num = (float) Mathf.RoundToInt(kelvin);
        break;
    }
    return num;
  }

  public static string GetFormattedTemperature(
    float temp,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.TemperatureInterpretation interpretation = GameUtil.TemperatureInterpretation.Absolute,
    bool displayUnits = true,
    bool roundInDestinationFormat = false)
  {
    if (interpretation != GameUtil.TemperatureInterpretation.Absolute)
    {
      if (interpretation == GameUtil.TemperatureInterpretation.Relative)
        ;
      temp = GameUtil.GetConvertedTemperatureDelta(temp);
    }
    else
      temp = GameUtil.GetConvertedTemperature(temp, roundInDestinationFormat);
    temp = GameUtil.ApplyTimeSlice(temp, timeSlice);
    string text = (double) Mathf.Abs(temp) >= 0.100000001490116 ? GameUtil.FloatToString(temp, "##0.#") : GameUtil.FloatToString(temp, "##0.####");
    if (displayUnits)
      text = GameUtil.AddTemperatureUnitSuffix(text);
    return GameUtil.AddTimeSliceText(text, timeSlice);
  }

  public static string GetFormattedCaloriesForItem(
    Tag tag,
    float amount,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(tag.Name);
    return GameUtil.GetFormattedCalories(foodInfo != null ? foodInfo.CaloriesPerUnit * amount : -1f, timeSlice, forceKcal);
  }

  public static string GetFormattedCalories(
    float calories,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    bool forceKcal = true)
  {
    string str = (string) UI.UNITSUFFIXES.CALORIES.CALORIE;
    if ((double) Mathf.Abs(calories) >= 1000.0 | forceKcal)
    {
      calories /= 1000f;
      str = (string) UI.UNITSUFFIXES.CALORIES.KILOCALORIE;
    }
    calories = GameUtil.ApplyTimeSlice(calories, timeSlice);
    return GameUtil.AddTimeSliceText((double) calories != 0.0 ? ((double) Mathf.Abs(calories) >= 1.0 ? ((double) Mathf.Abs(calories) >= 10.0 ? GameUtil.FloatToString(calories, "#,###") + str : GameUtil.FloatToString(calories, "#,###.#") + str) : GameUtil.FloatToString(calories, "#,##0.#") + str) : "0" + str, timeSlice);
  }

  public static string GetFormattedPlantGrowth(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
    string format = (double) Mathf.Abs(percent) != 0.0 ? ((double) Mathf.Abs(percent) >= 0.100000001490116 ? ((double) Mathf.Abs(percent) >= 1.0 ? "##0" : "##0.#") : "##0.##") : "0";
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(percent, format) + (string) UI.UNITSUFFIXES.PERCENT + " " + (string) UI.UNITSUFFIXES.GROWTH, timeSlice);
  }

  public static string GetFormattedPercent(float percent, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    percent = GameUtil.ApplyTimeSlice(percent, timeSlice);
    string format = (double) Mathf.Abs(percent) != 0.0 ? ((double) Mathf.Abs(percent) >= 0.100000001490116 ? ((double) Mathf.Abs(percent) >= 1.0 ? "##0" : "##0.#") : "##0.##") : "0";
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(percent, format) + (string) UI.UNITSUFFIXES.PERCENT, timeSlice);
  }

  public static string GetFormattedRoundedJoules(float joules) => (double) Mathf.Abs(joules) > 1000.0 ? GameUtil.FloatToString(joules / 1000f, "F1") + (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE : GameUtil.FloatToString(joules, "F1") + (string) UI.UNITSUFFIXES.ELECTRICAL.JOULE;

  public static string GetFormattedJoules(
    float joules,
    string floatFormat = "F1",
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    joules = GameUtil.ApplyTimeSlice(joules, timeSlice);
    return GameUtil.AddTimeSliceText((double) Math.Abs(joules) <= 1000000.0 ? ((double) Mathf.Abs(joules) <= 1000.0 ? GameUtil.FloatToString(joules, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.JOULE : GameUtil.FloatToString(joules / 1000f, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.KILOJOULE) : GameUtil.FloatToString(joules / 1000000f, floatFormat) + (string) UI.UNITSUFFIXES.ELECTRICAL.MEGAJOULE, timeSlice);
  }

  public static string GetFormattedWattage(
    float watts,
    GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Automatic,
    bool displayUnits = true)
  {
    LocString locString = (LocString) "";
    switch (unit)
    {
      case GameUtil.WattageFormatterUnit.Watts:
        locString = UI.UNITSUFFIXES.ELECTRICAL.WATT;
        break;
      case GameUtil.WattageFormatterUnit.Kilowatts:
        watts /= 1000f;
        locString = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
        break;
      case GameUtil.WattageFormatterUnit.Automatic:
        if ((double) Mathf.Abs(watts) > 1000.0)
        {
          watts /= 1000f;
          locString = UI.UNITSUFFIXES.ELECTRICAL.KILOWATT;
          break;
        }
        locString = UI.UNITSUFFIXES.ELECTRICAL.WATT;
        break;
    }
    return displayUnits ? GameUtil.FloatToString(watts, "###0.##") + (string) locString : GameUtil.FloatToString(watts, "###0.##");
  }

  public static string GetFormattedHeatEnergy(float dtu, GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    LocString locString1 = (LocString) "";
    LocString locString2;
    string format;
    switch (unit)
    {
      case GameUtil.HeatEnergyFormatterUnit.DTU_S:
        locString2 = UI.UNITSUFFIXES.HEAT.DTU;
        format = "###0.";
        break;
      case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
        dtu /= 1000f;
        locString2 = UI.UNITSUFFIXES.HEAT.KDTU;
        format = "###0.##";
        break;
      default:
        if ((double) Mathf.Abs(dtu) > 1000.0)
        {
          dtu /= 1000f;
          locString2 = UI.UNITSUFFIXES.HEAT.KDTU;
          format = "###0.##";
          break;
        }
        locString2 = UI.UNITSUFFIXES.HEAT.DTU;
        format = "###0.";
        break;
    }
    return GameUtil.FloatToString(dtu, format) + (string) locString2;
  }

  public static string GetFormattedHeatEnergyRate(
    float dtu_s,
    GameUtil.HeatEnergyFormatterUnit unit = GameUtil.HeatEnergyFormatterUnit.Automatic)
  {
    LocString locString = (LocString) "";
    switch (unit)
    {
      case GameUtil.HeatEnergyFormatterUnit.DTU_S:
        locString = UI.UNITSUFFIXES.HEAT.DTU_S;
        break;
      case GameUtil.HeatEnergyFormatterUnit.KDTU_S:
        dtu_s /= 1000f;
        locString = UI.UNITSUFFIXES.HEAT.KDTU_S;
        break;
      case GameUtil.HeatEnergyFormatterUnit.Automatic:
        if ((double) Mathf.Abs(dtu_s) > 1000.0)
        {
          dtu_s /= 1000f;
          locString = UI.UNITSUFFIXES.HEAT.KDTU_S;
          break;
        }
        locString = UI.UNITSUFFIXES.HEAT.DTU_S;
        break;
    }
    return GameUtil.FloatToString(dtu_s, "###0.##") + (string) locString;
  }

  public static string GetFormattedInt(float num, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    num = GameUtil.ApplyTimeSlice(num, timeSlice);
    return GameUtil.AddTimeSliceText(GameUtil.FloatToString(num, "F0"), timeSlice);
  }

  public static string GetFormattedSimple(
    float num,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    string formatString = null)
  {
    num = GameUtil.ApplyTimeSlice(num, timeSlice);
    return GameUtil.AddTimeSliceText(formatString == null ? ((double) num != 0.0 ? ((double) Mathf.Abs(num) >= 1.0 ? ((double) Mathf.Abs(num) >= 10.0 ? GameUtil.FloatToString(num, "#,###.##") : GameUtil.FloatToString(num, "#,###.##")) : GameUtil.FloatToString(num, "#,##0.##")) : "0") : GameUtil.FloatToString(num, formatString), timeSlice);
  }

  public static string GetFormattedLux(int lux) => lux.ToString() + (string) UI.UNITSUFFIXES.LIGHT.LUX;

  public static string GetLightDescription(int lux)
  {
    if (lux == 0)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.NO_LIGHT;
    if (lux < 100)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.VERY_LOW_LIGHT;
    if (lux < 1000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.LOW_LIGHT;
    if (lux < 10000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.MEDIUM_LIGHT;
    if (lux < 50000)
      return (string) UI.OVERLAYS.LIGHTING.RANGES.HIGH_LIGHT;
    return lux < 100000 ? (string) UI.OVERLAYS.LIGHTING.RANGES.VERY_HIGH_LIGHT : (string) UI.OVERLAYS.LIGHTING.RANGES.MAX_LIGHT;
  }

  public static string GetFormattedByTag(Tag tag, float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    if (GameTags.DisplayAsCalories.Contains(tag))
      return GameUtil.GetFormattedCaloriesForItem(tag, amount, timeSlice);
    return GameTags.DisplayAsUnits.Contains(tag) ? GameUtil.GetFormattedUnits(amount, timeSlice) : GameUtil.GetFormattedMass(amount, timeSlice);
  }

  public static string GetFormattedFoodQuality(int quality)
  {
    if (GameUtil.adjectives == null)
      GameUtil.adjectives = LocString.GetStrings(typeof (DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVES));
    LocString locString = quality >= 0 ? DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_POSITIVE : DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_FORMAT_NEGATIVE;
    int index = Mathf.Clamp(quality - DUPLICANTS.NEEDS.FOOD_QUALITY.ADJECTIVE_INDEX_OFFSET, 0, GameUtil.adjectives.Length);
    return string.Format((string) locString, (object) GameUtil.adjectives[index], (object) GameUtil.AddPositiveSign(quality.ToString(), quality > 0));
  }

  public static string GetFormattedInfomation(float amount, GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None)
  {
    amount = GameUtil.ApplyTimeSlice(amount, timeSlice);
    string str = "";
    if ((double) amount < 1024.0)
      str = (string) UI.UNITSUFFIXES.INFORMATION.KILOBYTE;
    else if ((double) amount < 1048576.0)
    {
      amount /= 1000f;
      str = (string) UI.UNITSUFFIXES.INFORMATION.MEGABYTE;
    }
    else if ((double) amount < 1073741824.0)
    {
      amount /= 1048576f;
      str = (string) UI.UNITSUFFIXES.INFORMATION.GIGABYTE;
    }
    return GameUtil.AddTimeSliceText(amount.ToString() + str, timeSlice);
  }

  public static LocString GetCurrentMassUnit(bool useSmallUnit = false)
  {
    LocString locString = (LocString) null;
    switch (GameUtil.massUnit)
    {
      case GameUtil.MassUnit.Kilograms:
        locString = !useSmallUnit ? UI.UNITSUFFIXES.MASS.KILOGRAM : UI.UNITSUFFIXES.MASS.GRAM;
        break;
      case GameUtil.MassUnit.Pounds:
        locString = UI.UNITSUFFIXES.MASS.POUND;
        break;
    }
    return locString;
  }

  public static string GetFormattedMass(
    float mass,
    GameUtil.TimeSlice timeSlice = GameUtil.TimeSlice.None,
    GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.UseThreshold,
    bool includeSuffix = true,
    string floatFormat = "{0:0.#}")
  {
    if ((double) mass == -3.40282346638529E+38)
      return (string) UI.CALCULATING;
    mass = GameUtil.ApplyTimeSlice(mass, timeSlice);
    string str;
    if (GameUtil.massUnit == GameUtil.MassUnit.Kilograms)
    {
      str = (string) UI.UNITSUFFIXES.MASS.TONNE;
      switch (massFormat)
      {
        case GameUtil.MetricMassFormat.UseThreshold:
          float num = Mathf.Abs(mass);
          if (0.0 < (double) num)
          {
            if ((double) num < 4.99999987368938E-06)
            {
              str = (string) UI.UNITSUFFIXES.MASS.MICROGRAM;
              mass = Mathf.Floor(mass * 1E+09f);
              break;
            }
            if ((double) num < 0.00499999988824129)
            {
              mass *= 1000000f;
              str = (string) UI.UNITSUFFIXES.MASS.MILLIGRAM;
              break;
            }
            if ((double) Mathf.Abs(mass) < 5.0)
            {
              mass *= 1000f;
              str = (string) UI.UNITSUFFIXES.MASS.GRAM;
              break;
            }
            if ((double) Mathf.Abs(mass) < 5000.0)
            {
              str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
              break;
            }
            mass /= 1000f;
            str = (string) UI.UNITSUFFIXES.MASS.TONNE;
            break;
          }
          str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
          break;
        case GameUtil.MetricMassFormat.Kilogram:
          str = (string) UI.UNITSUFFIXES.MASS.KILOGRAM;
          break;
        case GameUtil.MetricMassFormat.Gram:
          mass *= 1000f;
          str = (string) UI.UNITSUFFIXES.MASS.GRAM;
          break;
        case GameUtil.MetricMassFormat.Tonne:
          mass /= 1000f;
          str = (string) UI.UNITSUFFIXES.MASS.TONNE;
          break;
      }
    }
    else
    {
      mass /= 2.2f;
      str = (string) UI.UNITSUFFIXES.MASS.POUND;
      if (massFormat == GameUtil.MetricMassFormat.UseThreshold)
      {
        float num = Mathf.Abs(mass);
        if ((double) num < 5.0 && (double) num > 1.0 / 1000.0)
        {
          mass *= 256f;
          str = (string) UI.UNITSUFFIXES.MASS.DRACHMA;
        }
        else
        {
          mass *= 7000f;
          str = (string) UI.UNITSUFFIXES.MASS.GRAIN;
        }
      }
    }
    if (!includeSuffix)
    {
      str = "";
      timeSlice = GameUtil.TimeSlice.None;
    }
    return GameUtil.AddTimeSliceText(string.Format(floatFormat, (object) mass) + str, timeSlice);
  }

  public static string GetFormattedTime(float seconds, string floatFormat = "F0") => string.Format((string) UI.FORMATSECONDS, (object) seconds.ToString(floatFormat));

  public static string GetFormattedEngineEfficiency(float amount) => amount.ToString() + " km /" + (string) UI.UNITSUFFIXES.MASS.KILOGRAM;

  public static string GetFormattedDistance(float meters)
  {
    if ((double) Mathf.Abs(meters) < 1.0)
    {
      string str1 = (meters * 100f).ToString();
      string str2 = str1.Substring(0, str1.LastIndexOf('.') + Mathf.Min(3, str1.Length - str1.LastIndexOf('.')));
      if (str2 == "-0.0")
        str2 = "0";
      return str2 + " cm";
    }
    return (double) meters < 1000.0 ? meters.ToString() + " m" : Util.FormatOneDecimalPlace(meters / 1000f) + " km";
  }

  public static string GetFormattedCycles(float seconds, string formatString = "F1", bool forceCycles = false) => forceCycles || (double) Mathf.Abs(seconds) > 100.0 ? string.Format((string) UI.FORMATDAY, (object) GameUtil.FloatToString(seconds / 600f, formatString)) : GameUtil.GetFormattedTime(seconds);

  public static float GetDisplaySHC(float shc)
  {
    if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
      shc /= 1.8f;
    return shc;
  }

  public static string GetSHCSuffix() => string.Format("(DTU/g)/{0}", (object) GameUtil.GetTemperatureUnitSuffix());

  public static string GetFormattedSHC(float shc)
  {
    shc = GameUtil.GetDisplaySHC(shc);
    return string.Format("{0} (DTU/g)/{1}", (object) shc.ToString("0.000"), (object) GameUtil.GetTemperatureUnitSuffix());
  }

  public static float GetDisplayThermalConductivity(float tc)
  {
    if (GameUtil.temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
      tc /= 1.8f;
    return tc;
  }

  public static string GetThermalConductivitySuffix() => string.Format("(DTU/(m*s))/{0}", (object) GameUtil.GetTemperatureUnitSuffix());

  public static string GetFormattedThermalConductivity(float tc)
  {
    tc = GameUtil.GetDisplayThermalConductivity(tc);
    return string.Format("{0} (DTU/(m*s))/{1}", (object) tc.ToString("0.000"), (object) GameUtil.GetTemperatureUnitSuffix());
  }

  public static string GetElementNameByElementHash(SimHashes elementHash) => ElementLoader.FindElementByHash(elementHash).tag.ProperName();

  public static bool HasTrait(GameObject go, string traitName)
  {
    Traits component = go.GetComponent<Traits>();
    return !((UnityEngine.Object) component == (UnityEngine.Object) null) && component.HasTrait(traitName);
  }

  public static HashSet<int> GetFloodFillCavity(int startCell, bool allowLiquid)
  {
    HashSet<int> intSet = new HashSet<int>();
    return !allowLiquid ? GameUtil.FloodCollectCells(startCell, (Func<int, bool>) (cell => Grid.Element[cell].IsVacuum || Grid.Element[cell].IsGas)) : GameUtil.FloodCollectCells(startCell, (Func<int, bool>) (cell => !Grid.Solid[cell]));
  }

  public static HashSet<int> FloodCollectCells(
    int start_cell,
    Func<int, bool> is_valid,
    int maxSize = 300,
    HashSet<int> AddInvalidCellsToSet = null,
    bool clearOversizedResults = true)
  {
    HashSet<int> cells = new HashSet<int>();
    HashSet<int> invalidCells = new HashSet<int>();
    GameUtil.probeFromCell(start_cell, is_valid, cells, invalidCells, maxSize);
    if (AddInvalidCellsToSet != null)
    {
      AddInvalidCellsToSet.UnionWith((IEnumerable<int>) invalidCells);
      if (cells.Count > maxSize)
        AddInvalidCellsToSet.UnionWith((IEnumerable<int>) cells);
    }
    if (cells.Count > maxSize & clearOversizedResults)
      cells.Clear();
    return cells;
  }

  public static HashSet<int> FloodCollectCells(
    HashSet<int> results,
    int start_cell,
    Func<int, bool> is_valid,
    int maxSize = 300,
    HashSet<int> AddInvalidCellsToSet = null,
    bool clearOversizedResults = true)
  {
    HashSet<int> invalidCells = new HashSet<int>();
    GameUtil.probeFromCell(start_cell, is_valid, results, invalidCells, maxSize);
    if (AddInvalidCellsToSet != null)
    {
      AddInvalidCellsToSet.UnionWith((IEnumerable<int>) invalidCells);
      if (results.Count > maxSize)
        AddInvalidCellsToSet.UnionWith((IEnumerable<int>) results);
    }
    if (results.Count > maxSize & clearOversizedResults)
      results.Clear();
    return results;
  }

  private static void probeFromCell(
    int start_cell,
    Func<int, bool> is_valid,
    HashSet<int> cells,
    HashSet<int> invalidCells,
    int maxSize = 300)
  {
    if (cells.Count > maxSize || !Grid.IsValidCell(start_cell) || (invalidCells.Contains(start_cell) || cells.Contains(start_cell)) || !is_valid(start_cell))
    {
      invalidCells.Add(start_cell);
    }
    else
    {
      cells.Add(start_cell);
      GameUtil.probeFromCell(Grid.CellLeft(start_cell), is_valid, cells, invalidCells, maxSize);
      GameUtil.probeFromCell(Grid.CellRight(start_cell), is_valid, cells, invalidCells, maxSize);
      GameUtil.probeFromCell(Grid.CellAbove(start_cell), is_valid, cells, invalidCells, maxSize);
      GameUtil.probeFromCell(Grid.CellBelow(start_cell), is_valid, cells, invalidCells, maxSize);
    }
  }

  public static bool FloodFillCheck<ArgType>(
    Func<int, ArgType, bool> fn,
    ArgType arg,
    int start_cell,
    int max_depth,
    bool stop_at_solid,
    bool stop_at_liquid)
  {
    return GameUtil.FloodFillFind<ArgType>(fn, arg, start_cell, max_depth, stop_at_solid, stop_at_liquid) != -1;
  }

  public static int FloodFillFind<ArgType>(
    Func<int, ArgType, bool> fn,
    ArgType arg,
    int start_cell,
    int max_depth,
    bool stop_at_solid,
    bool stop_at_liquid)
  {
    GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
    {
      cell = start_cell,
      depth = 0
    });
    int num = -1;
    while (GameUtil.FloodFillNext.Count > 0)
    {
      GameUtil.FloodFillInfo floodFillInfo1 = GameUtil.FloodFillNext.Dequeue();
      if (floodFillInfo1.depth < max_depth && Grid.IsValidCell(floodFillInfo1.cell))
      {
        Element element = Grid.Element[floodFillInfo1.cell];
        if ((!stop_at_solid || !element.IsSolid) && (!stop_at_liquid || !element.IsLiquid) && !GameUtil.FloodFillVisited.Contains(floodFillInfo1.cell))
        {
          GameUtil.FloodFillVisited.Add(floodFillInfo1.cell);
          if (fn(floodFillInfo1.cell, arg))
          {
            num = floodFillInfo1.cell;
            break;
          }
          Queue<GameUtil.FloodFillInfo> floodFillNext1 = GameUtil.FloodFillNext;
          GameUtil.FloodFillInfo floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellLeft(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo3 = floodFillInfo2;
          floodFillNext1.Enqueue(floodFillInfo3);
          Queue<GameUtil.FloodFillInfo> floodFillNext2 = GameUtil.FloodFillNext;
          floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellRight(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo4 = floodFillInfo2;
          floodFillNext2.Enqueue(floodFillInfo4);
          Queue<GameUtil.FloodFillInfo> floodFillNext3 = GameUtil.FloodFillNext;
          floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellAbove(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo5 = floodFillInfo2;
          floodFillNext3.Enqueue(floodFillInfo5);
          Queue<GameUtil.FloodFillInfo> floodFillNext4 = GameUtil.FloodFillNext;
          floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellBelow(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo6 = floodFillInfo2;
          floodFillNext4.Enqueue(floodFillInfo6);
        }
      }
    }
    GameUtil.FloodFillVisited.Clear();
    GameUtil.FloodFillNext.Clear();
    return num;
  }

  public static void FloodFillConditional(
    int start_cell,
    Func<int, bool> condition,
    ICollection<int> visited_cells,
    ICollection<int> valid_cells = null)
  {
    GameUtil.FloodFillNext.Enqueue(new GameUtil.FloodFillInfo()
    {
      cell = start_cell,
      depth = 0
    });
    GameUtil.FloodFillConditional(GameUtil.FloodFillNext, condition, visited_cells, valid_cells);
  }

  public static void FloodFillConditional(
    Queue<GameUtil.FloodFillInfo> queue,
    Func<int, bool> condition,
    ICollection<int> visited_cells,
    ICollection<int> valid_cells = null,
    int max_depth = 10000)
  {
    while (queue.Count > 0)
    {
      GameUtil.FloodFillInfo floodFillInfo1 = queue.Dequeue();
      if (floodFillInfo1.depth < max_depth && Grid.IsValidCell(floodFillInfo1.cell) && !visited_cells.Contains(floodFillInfo1.cell))
      {
        visited_cells.Add(floodFillInfo1.cell);
        if (condition(floodFillInfo1.cell))
        {
          valid_cells?.Add(floodFillInfo1.cell);
          Queue<GameUtil.FloodFillInfo> floodFillInfoQueue1 = queue;
          GameUtil.FloodFillInfo floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellLeft(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo3 = floodFillInfo2;
          floodFillInfoQueue1.Enqueue(floodFillInfo3);
          Queue<GameUtil.FloodFillInfo> floodFillInfoQueue2 = queue;
          floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellRight(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo4 = floodFillInfo2;
          floodFillInfoQueue2.Enqueue(floodFillInfo4);
          Queue<GameUtil.FloodFillInfo> floodFillInfoQueue3 = queue;
          floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellAbove(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo5 = floodFillInfo2;
          floodFillInfoQueue3.Enqueue(floodFillInfo5);
          Queue<GameUtil.FloodFillInfo> floodFillInfoQueue4 = queue;
          floodFillInfo2 = new GameUtil.FloodFillInfo();
          floodFillInfo2.cell = Grid.CellBelow(floodFillInfo1.cell);
          floodFillInfo2.depth = floodFillInfo1.depth + 1;
          GameUtil.FloodFillInfo floodFillInfo6 = floodFillInfo2;
          floodFillInfoQueue4.Enqueue(floodFillInfo6);
        }
      }
    }
    queue.Clear();
  }

  public static string GetHardnessString(Element element, bool addColor = true)
  {
    if (!element.IsSolid)
      return (string) ELEMENTS.HARDNESS.NA;
    Color firmColor = GameUtil.Hardness.firmColor;
    Color c;
    string str;
    if (element.hardness >= byte.MaxValue)
    {
      c = GameUtil.Hardness.ImpenetrableColor;
      str = string.Format((string) ELEMENTS.HARDNESS.IMPENETRABLE, (object) element.hardness);
    }
    else if (element.hardness >= (byte) 150)
    {
      c = GameUtil.Hardness.nearlyImpenetrableColor;
      str = string.Format((string) ELEMENTS.HARDNESS.NEARLYIMPENETRABLE, (object) element.hardness);
    }
    else if (element.hardness >= (byte) 50)
    {
      c = GameUtil.Hardness.veryFirmColor;
      str = string.Format((string) ELEMENTS.HARDNESS.VERYFIRM, (object) element.hardness);
    }
    else if (element.hardness >= (byte) 25)
    {
      c = GameUtil.Hardness.firmColor;
      str = string.Format((string) ELEMENTS.HARDNESS.FIRM, (object) element.hardness);
    }
    else if (element.hardness >= (byte) 10)
    {
      c = GameUtil.Hardness.softColor;
      str = string.Format((string) ELEMENTS.HARDNESS.SOFT, (object) element.hardness);
    }
    else
    {
      c = GameUtil.Hardness.verySoftColor;
      str = string.Format((string) ELEMENTS.HARDNESS.VERYSOFT, (object) element.hardness);
    }
    if (addColor)
      str = string.Format("<color=#{0}>{1}</color>", (object) c.ToHexString(), (object) str);
    return str;
  }

  public static string GetGermResistanceModifierString(float modifier, bool addColor = true)
  {
    Color c = Color.black;
    string str = "";
    if ((double) modifier > 0.0)
    {
      if ((double) modifier >= 5.0)
      {
        c = GameUtil.GermResistanceValues.PositiveLargeColor;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_LARGE, (object) modifier);
      }
      else if ((double) modifier >= 2.0)
      {
        c = GameUtil.GermResistanceValues.PositiveMediumColor;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_MEDIUM, (object) modifier);
      }
      else if ((double) modifier > 0.0)
      {
        c = GameUtil.GermResistanceValues.PositiveSmallColor;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.POSITIVE_SMALL, (object) modifier);
      }
    }
    else if ((double) modifier < 0.0)
    {
      if ((double) modifier <= -5.0)
      {
        c = GameUtil.GermResistanceValues.NegativeLargeColor;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_LARGE, (object) modifier);
      }
      else if ((double) modifier <= -2.0)
      {
        c = GameUtil.GermResistanceValues.NegativeMediumColor;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_MEDIUM, (object) modifier);
      }
      else if ((double) modifier < 0.0)
      {
        c = GameUtil.GermResistanceValues.NegativeSmallColor;
        str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NEGATIVE_SMALL, (object) modifier);
      }
    }
    else
    {
      addColor = false;
      str = string.Format((string) DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.MODIFIER_DESCRIPTORS.NONE, (object) modifier);
    }
    if (addColor)
      str = string.Format("<color=#{0}>{1}</color>", (object) c.ToHexString(), (object) str);
    return str;
  }

  public static string GetThermalConductivityString(Element element, bool addColor = true, bool addValue = true)
  {
    Color conductivityColor1 = GameUtil.ThermalConductivityValues.mediumConductivityColor;
    Color conductivityColor2;
    string str;
    if ((double) element.thermalConductivity >= 50.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.veryHighConductivityColor;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_HIGH_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 10.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.highConductivityColor;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.HIGH_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 2.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.mediumConductivityColor;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.MEDIUM_CONDUCTIVITY;
    }
    else if ((double) element.thermalConductivity >= 1.0)
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.lowConductivityColor;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.LOW_CONDUCTIVITY;
    }
    else
    {
      conductivityColor2 = GameUtil.ThermalConductivityValues.veryLowConductivityColor;
      str = (string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VERY_LOW_CONDUCTIVITY;
    }
    if (addColor)
      str = string.Format("<color=#{0}>{1}</color>", (object) conductivityColor2.ToHexString(), (object) str);
    if (addValue)
      str = string.Format((string) UI.ELEMENTAL.THERMALCONDUCTIVITY.ADJECTIVES.VALUE_WITH_ADJECTIVE, (object) element.thermalConductivity.ToString(), (object) str);
    return str;
  }

  public static string GetBreathableString(Element element, float Mass)
  {
    if (!element.IsGas && !element.IsVacuum)
      return "";
    Color positiveColor = GameUtil.BreathableValues.positiveColor;
    Color c;
    LocString locString;
    switch (element.id)
    {
      case SimHashes.Oxygen:
        if ((double) Mass >= (double) SimDebugView.optimallyBreathable)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND1;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable + ((double) SimDebugView.optimallyBreathable - (double) SimDebugView.minimumBreathable) / 2.0)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND2;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable)
        {
          c = GameUtil.BreathableValues.warningColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND3;
          break;
        }
        c = GameUtil.BreathableValues.negativeColor;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
      case SimHashes.ContaminatedOxygen:
        if ((double) Mass >= (double) SimDebugView.optimallyBreathable)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND1;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable + ((double) SimDebugView.optimallyBreathable - (double) SimDebugView.minimumBreathable) / 2.0)
        {
          c = GameUtil.BreathableValues.positiveColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND2;
          break;
        }
        if ((double) Mass >= (double) SimDebugView.minimumBreathable)
        {
          c = GameUtil.BreathableValues.warningColor;
          locString = UI.OVERLAYS.OXYGEN.LEGEND3;
          break;
        }
        c = GameUtil.BreathableValues.negativeColor;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
      default:
        c = GameUtil.BreathableValues.negativeColor;
        locString = UI.OVERLAYS.OXYGEN.LEGEND4;
        break;
    }
    return string.Format((string) ELEMENTS.BREATHABLEDESC, (object) c.ToHexString(), (object) locString);
  }

  public static string GetWireLoadColor(float load, float maxLoad, float potentialLoad) => ((double) load <= (double) maxLoad ? ((double) potentialLoad <= (double) maxLoad || (double) load / (double) maxLoad < 0.75 ? Color.white : GameUtil.WireLoadValues.warningColor) : GameUtil.WireLoadValues.negativeColor).ToHexString();

  public static string AppendHotkeyString(string template, Action action) => template + UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]");

  public static string ReplaceHotkeyString(string template, Action action) => template.Replace("{Hotkey}", UI.FormatAsHotkey("[" + GameUtil.GetActionString(action) + "]"));

  public static string ReplaceHotkeyString(string template, Action action1, Action action2) => template.Replace("{Hotkey}", UI.FormatAsHotkey("[" + GameUtil.GetActionString(action1) + "]") + UI.FormatAsHotkey("[" + GameUtil.GetActionString(action2) + "]"));

  public static string GetKeycodeLocalized(KKeyCode key_code)
  {
    string str = key_code.ToString();
    switch (key_code)
    {
      case KKeyCode.None:
        return str;
      case KKeyCode.Backspace:
        str = (string) INPUT.BACKSPACE;
        goto case KKeyCode.None;
      case KKeyCode.Tab:
        str = (string) INPUT.TAB;
        goto case KKeyCode.None;
      case KKeyCode.Return:
        str = (string) INPUT.ENTER;
        goto case KKeyCode.None;
      case KKeyCode.Escape:
        str = (string) INPUT.ESCAPE;
        goto case KKeyCode.None;
      case KKeyCode.Space:
        str = (string) INPUT.SPACE;
        goto case KKeyCode.None;
      case KKeyCode.Plus:
        str = "+";
        goto case KKeyCode.None;
      case KKeyCode.Comma:
        str = ",";
        goto case KKeyCode.None;
      case KKeyCode.Minus:
        str = "-";
        goto case KKeyCode.None;
      case KKeyCode.Period:
        str = (string) INPUT.PERIOD;
        goto case KKeyCode.None;
      case KKeyCode.Slash:
        str = "/";
        goto case KKeyCode.None;
      case KKeyCode.Colon:
        str = ":";
        goto case KKeyCode.None;
      case KKeyCode.Semicolon:
        str = ";";
        goto case KKeyCode.None;
      case KKeyCode.Equals:
        str = "=";
        goto case KKeyCode.None;
      case KKeyCode.LeftBracket:
        str = "[";
        goto case KKeyCode.None;
      case KKeyCode.Backslash:
        str = "\\";
        goto case KKeyCode.None;
      case KKeyCode.RightBracket:
        str = "]";
        goto case KKeyCode.None;
      case KKeyCode.BackQuote:
        str = (string) INPUT.BACKQUOTE;
        goto case KKeyCode.None;
      case KKeyCode.Keypad0:
        str = (string) INPUT.NUM + " 0";
        goto case KKeyCode.None;
      case KKeyCode.Keypad1:
        str = (string) INPUT.NUM + " 1";
        goto case KKeyCode.None;
      case KKeyCode.Keypad2:
        str = (string) INPUT.NUM + " 2";
        goto case KKeyCode.None;
      case KKeyCode.Keypad3:
        str = (string) INPUT.NUM + " 3";
        goto case KKeyCode.None;
      case KKeyCode.Keypad4:
        str = (string) INPUT.NUM + " 4";
        goto case KKeyCode.None;
      case KKeyCode.Keypad5:
        str = (string) INPUT.NUM + " 5";
        goto case KKeyCode.None;
      case KKeyCode.Keypad6:
        str = (string) INPUT.NUM + " 6";
        goto case KKeyCode.None;
      case KKeyCode.Keypad7:
        str = (string) INPUT.NUM + " 7";
        goto case KKeyCode.None;
      case KKeyCode.Keypad8:
        str = (string) INPUT.NUM + " 8";
        goto case KKeyCode.None;
      case KKeyCode.Keypad9:
        str = (string) INPUT.NUM + " 9";
        goto case KKeyCode.None;
      case KKeyCode.KeypadPeriod:
        str = (string) INPUT.NUM + " " + (string) INPUT.PERIOD;
        goto case KKeyCode.None;
      case KKeyCode.KeypadDivide:
        str = (string) INPUT.NUM + " /";
        goto case KKeyCode.None;
      case KKeyCode.KeypadMultiply:
        str = (string) INPUT.NUM + " *";
        goto case KKeyCode.None;
      case KKeyCode.KeypadMinus:
        str = (string) INPUT.NUM + " -";
        goto case KKeyCode.None;
      case KKeyCode.KeypadPlus:
        str = (string) INPUT.NUM + " +";
        goto case KKeyCode.None;
      case KKeyCode.KeypadEnter:
        str = (string) INPUT.NUM + " " + (string) INPUT.ENTER;
        goto case KKeyCode.None;
      case KKeyCode.Insert:
        str = (string) INPUT.INSERT;
        goto case KKeyCode.None;
      case KKeyCode.RightShift:
        str = (string) INPUT.RIGHT_SHIFT;
        goto case KKeyCode.None;
      case KKeyCode.LeftShift:
        str = (string) INPUT.LEFT_SHIFT;
        goto case KKeyCode.None;
      case KKeyCode.RightControl:
        str = (string) INPUT.RIGHT_CTRL;
        goto case KKeyCode.None;
      case KKeyCode.LeftControl:
        str = (string) INPUT.LEFT_CTRL;
        goto case KKeyCode.None;
      case KKeyCode.RightAlt:
        str = (string) INPUT.RIGHT_ALT;
        goto case KKeyCode.None;
      case KKeyCode.LeftAlt:
        str = (string) INPUT.LEFT_ALT;
        goto case KKeyCode.None;
      case KKeyCode.Mouse0:
        str = (string) INPUT.MOUSE + " 0";
        goto case KKeyCode.None;
      case KKeyCode.Mouse1:
        str = (string) INPUT.MOUSE + " 1";
        goto case KKeyCode.None;
      case KKeyCode.Mouse2:
        str = (string) INPUT.MOUSE + " 2";
        goto case KKeyCode.None;
      case KKeyCode.Mouse3:
        str = (string) INPUT.MOUSE + " 3";
        goto case KKeyCode.None;
      case KKeyCode.Mouse4:
        str = (string) INPUT.MOUSE + " 4";
        goto case KKeyCode.None;
      case KKeyCode.Mouse5:
        str = (string) INPUT.MOUSE + " 5";
        goto case KKeyCode.None;
      case KKeyCode.Mouse6:
        str = (string) INPUT.MOUSE + " 6";
        goto case KKeyCode.None;
      case KKeyCode.MouseScrollDown:
        str = (string) INPUT.MOUSE_SCROLL_DOWN;
        goto case KKeyCode.None;
      case KKeyCode.MouseScrollUp:
        str = (string) INPUT.MOUSE_SCROLL_UP;
        goto case KKeyCode.None;
      default:
        if (KKeyCode.A <= key_code && key_code <= KKeyCode.Z)
        {
          str = ((char) (65 + (key_code - 97))).ToString();
          goto case KKeyCode.None;
        }
        else if (KKeyCode.Alpha0 <= key_code && key_code <= KKeyCode.Alpha9)
        {
          str = ((char) (48 + (key_code - 48))).ToString();
          goto case KKeyCode.None;
        }
        else if (KKeyCode.F1 <= key_code && key_code <= KKeyCode.F12)
        {
          str = "F" + ((int) (key_code - 282 + 1)).ToString();
          goto case KKeyCode.None;
        }
        else
        {
          Debug.LogWarning((object) ("Unable to find proper string for KKeyCode: " + key_code.ToString() + " using key_code.ToString()"));
          goto case KKeyCode.None;
        }
    }
  }

  public static string GetActionString(Action action)
  {
    string str1 = "";
    if (action == Action.NumActions)
      return str1;
    BindingEntry binding = GameUtil.ActionToBinding(action);
    KKeyCode mKeyCode = binding.mKeyCode;
    if (binding.mModifier == Modifier.None)
      return GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
    string str2 = "";
    switch (binding.mModifier)
    {
      case Modifier.Alt:
        str2 = GameUtil.GetKeycodeLocalized(KKeyCode.LeftAlt).ToUpper();
        break;
      case Modifier.Ctrl:
        str2 = GameUtil.GetKeycodeLocalized(KKeyCode.LeftControl).ToUpper();
        break;
      case Modifier.Shift:
        str2 = GameUtil.GetKeycodeLocalized(KKeyCode.LeftShift).ToUpper();
        break;
      case Modifier.CapsLock:
        str2 = GameUtil.GetKeycodeLocalized(KKeyCode.CapsLock).ToUpper();
        break;
    }
    return str2 + " + " + GameUtil.GetKeycodeLocalized(mKeyCode).ToUpper();
  }

  public static void CreateExplosion(Vector3 explosion_pos)
  {
    Vector2 vector2 = new Vector2(explosion_pos.x, explosion_pos.y);
    double num1 = 5.0;
    float num2 = (float) (num1 * num1);
    foreach (Health health in Components.Health.Items)
    {
      Vector3 position = health.transform.GetPosition();
      float sqrMagnitude = (new Vector2(position.x, position.y) - vector2).sqrMagnitude;
      if ((double) num2 >= (double) sqrMagnitude && (UnityEngine.Object) health != (UnityEngine.Object) null)
        health.Damage(health.maxHitPoints);
    }
  }

  private static void GetNonSolidCells(
    int x,
    int y,
    List<int> cells,
    int min_x,
    int min_y,
    int max_x,
    int max_y)
  {
    int cell = Grid.XYToCell(x, y);
    if (!Grid.IsValidCell(cell) || Grid.Solid[cell] || (Grid.DupePassable[cell] || x < min_x) || (x > max_x || y < min_y || (y > max_y || cells.Contains(cell))))
      return;
    cells.Add(cell);
    GameUtil.GetNonSolidCells(x + 1, y, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x - 1, y, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x, y + 1, cells, min_x, min_y, max_x, max_y);
    GameUtil.GetNonSolidCells(x, y - 1, cells, min_x, min_y, max_x, max_y);
  }

  public static void GetNonSolidCells(int cell, int radius, List<int> cells)
  {
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    GameUtil.GetNonSolidCells(x, y, cells, x - radius, y - radius, x + radius, y + radius);
  }

  public static float GetMaxStress()
  {
    if (Components.LiveMinionIdentities.Count <= 0)
      return 0.0f;
    float a = 0.0f;
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      a = Mathf.Max(a, Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).value);
    return a;
  }

  public static float GetAverageStress()
  {
    if (Components.LiveMinionIdentities.Count <= 0)
      return 0.0f;
    float num = 0.0f;
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      num += Db.Get().Amounts.Stress.Lookup((Component) minionIdentity).value;
    return num / (float) Components.LiveMinionIdentities.Count;
  }

  public static string MigrateFMOD(FMODAsset asset)
  {
    if ((UnityEngine.Object) asset == (UnityEngine.Object) null)
      return (string) null;
    return asset.path == null ? asset.name : asset.path;
  }

  private static void SortGameObjectDescriptors(List<IGameObjectEffectDescriptor> descriptorList) => descriptorList.Sort((Comparison<IGameObjectEffectDescriptor>) ((e1, e2) => TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e1.GetType()).CompareTo(TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.IndexOf(e2.GetType()))));

  public static void IndentListOfDescriptors(List<Descriptor> list, int indentCount = 1)
  {
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      Descriptor descriptor = list[index1];
      for (int index2 = 0; index2 < indentCount; ++index2)
        descriptor.IncreaseIndent();
      list[index1] = descriptor;
    }
  }

  public static List<Descriptor> GetAllDescriptors(
    GameObject go,
    bool simpleInfoScreen = false)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList2 = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList2.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList2);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if (!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen)
            descriptorList1.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalRequirements != null)
    {
      foreach (Descriptor additionalRequirement in component2.AdditionalRequirements)
      {
        if (!additionalRequirement.onlyForSimpleInfoScreen || simpleInfoScreen)
          descriptorList1.Add(additionalRequirement);
      }
    }
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalEffects != null)
    {
      foreach (Descriptor additionalEffect in component2.AdditionalEffects)
      {
        if (!additionalEffect.onlyForSimpleInfoScreen || simpleInfoScreen)
          descriptorList1.Add(additionalEffect);
      }
    }
    return descriptorList1;
  }

  public static List<Descriptor> GetDetailDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Detail)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetRequirementDescriptors(
    List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Requirement)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetEffectDescriptors(List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetInformationDescriptors(
    List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor in descriptors)
    {
      if (descriptor.type == Descriptor.DescriptorType.Lifecycle)
        list.Add(descriptor);
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetCropOptimumConditionDescriptors(
    List<Descriptor> descriptors)
  {
    List<Descriptor> list = new List<Descriptor>();
    foreach (Descriptor descriptor1 in descriptors)
    {
      if (descriptor1.type == Descriptor.DescriptorType.Lifecycle)
      {
        Descriptor descriptor2 = descriptor1;
        descriptor2.text = "• " + descriptor2.text;
        list.Add(descriptor2);
      }
    }
    GameUtil.IndentListOfDescriptors(list);
    return list;
  }

  public static List<Descriptor> GetGameObjectRequirements(GameObject go)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList2 = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList2.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList2);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if (descriptor.type == Descriptor.DescriptorType.Requirement)
            descriptorList1.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if (component2.AdditionalRequirements != null)
      descriptorList1.AddRange((IEnumerable<Descriptor>) component2.AdditionalRequirements);
    return descriptorList1;
  }

  public static List<Descriptor> GetGameObjectEffects(
    GameObject go,
    bool simpleInfoScreen = false)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    List<IGameObjectEffectDescriptor> descriptorList2 = new List<IGameObjectEffectDescriptor>((IEnumerable<IGameObjectEffectDescriptor>) go.GetComponents<IGameObjectEffectDescriptor>());
    StateMachineController component1 = go.GetComponent<StateMachineController>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      descriptorList2.AddRange((IEnumerable<IGameObjectEffectDescriptor>) component1.GetDescriptors());
    GameUtil.SortGameObjectDescriptors(descriptorList2);
    foreach (IGameObjectEffectDescriptor effectDescriptor in descriptorList2)
    {
      List<Descriptor> descriptors = effectDescriptor.GetDescriptors(go);
      if (descriptors != null)
      {
        foreach (Descriptor descriptor in descriptors)
        {
          if ((!descriptor.onlyForSimpleInfoScreen || simpleInfoScreen) && (descriptor.type == Descriptor.DescriptorType.Effect || descriptor.type == Descriptor.DescriptorType.DiseaseSource))
            descriptorList1.Add(descriptor);
        }
      }
    }
    KPrefabID component2 = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.AdditionalEffects != null)
    {
      foreach (Descriptor additionalEffect in component2.AdditionalEffects)
      {
        if (!additionalEffect.onlyForSimpleInfoScreen || simpleInfoScreen)
          descriptorList1.Add(additionalEffect);
      }
    }
    return descriptorList1;
  }

  public static List<Descriptor> GetPlantRequirementDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(GameUtil.GetAllDescriptors(go));
    if (requirementDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTREQUIREMENTS, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTREQUIREMENTS, Descriptor.DescriptorType.Requirement);
      descriptorList.Add(descriptor);
      descriptorList.AddRange((IEnumerable<Descriptor>) requirementDescriptors);
    }
    return descriptorList;
  }

  public static List<Descriptor> GetPlantLifeCycleDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<Descriptor> informationDescriptors = GameUtil.GetInformationDescriptors(GameUtil.GetAllDescriptors(go));
    if (informationDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.LIFECYCLE, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTLIFECYCLE, Descriptor.DescriptorType.Lifecycle);
      descriptorList.Add(descriptor);
      descriptorList.AddRange((IEnumerable<Descriptor>) informationDescriptors);
    }
    return descriptorList;
  }

  public static List<Descriptor> GetPlantEffectDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList1 = new List<Descriptor>();
    if ((UnityEngine.Object) go.GetComponent<Growing>() == (UnityEngine.Object) null)
      return descriptorList1;
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(go);
    List<Descriptor> descriptorList2 = new List<Descriptor>();
    descriptorList2.AddRange((IEnumerable<Descriptor>) GameUtil.GetEffectDescriptors(allDescriptors));
    if (descriptorList2.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.UISIDESCREENS.PLANTERSIDESCREEN.PLANTEFFECTS, (string) UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANTEFFECTS);
      descriptorList1.Add(descriptor);
      descriptorList1.AddRange((IEnumerable<Descriptor>) descriptorList2);
    }
    return descriptorList1;
  }

  public static string GetGameObjectEffectsTooltipString(GameObject go)
  {
    string str = "";
    List<Descriptor> gameObjectEffects = GameUtil.GetGameObjectEffects(go);
    if (gameObjectEffects.Count > 0)
      str = str + (string) UI.BUILDINGEFFECTS.OPERATIONEFFECTS + "\n";
    foreach (Descriptor descriptor in gameObjectEffects)
      str = str + descriptor.IndentedText() + "\n";
    return str;
  }

  public static List<Descriptor> GetEquipmentEffects(EquipmentDef def)
  {
    Debug.Assert((UnityEngine.Object) def != (UnityEngine.Object) null);
    List<Descriptor> descriptorList = new List<Descriptor>();
    List<AttributeModifier> attributeModifiers = def.AttributeModifiers;
    if (attributeModifiers != null)
    {
      foreach (AttributeModifier attributeModifier in attributeModifiers)
      {
        string name = Db.Get().Attributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString((GameObject) null);
        string newValue = (double) attributeModifier.Value >= 0.0 ? "produced" : "consumed";
        string str = UI.GAMEOBJECTEFFECTS.EQUIPMENT_MODS.text.Replace("{Attribute}", name).Replace("{Style}", newValue).Replace("{Value}", formattedString);
        descriptorList.Add(new Descriptor(str, str));
      }
    }
    return descriptorList;
  }

  public static string GetRecipeDescription(Recipe recipe)
  {
    string str = (string) null;
    if (recipe != null)
      str = recipe.recipeDescription;
    if (str == null)
    {
      str = "MISSING RECIPEDESCRIPTION";
      Debug.LogWarning((object) "Missing recipeDescription");
    }
    return str;
  }

  public static int GetCurrentCycle() => GameClock.Instance.GetCycle() + 1;

  public static GameObject GetTelepad() => Components.Telepads.Count > 0 ? Components.Telepads[0].gameObject : (GameObject) null;

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original, position, sceneLayer, (GameObject) null, name, gameLayer);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Vector3 position,
    Grid.SceneLayer sceneLayer,
    GameObject parent,
    string name = null,
    int gameLayer = 0)
  {
    position.z = Grid.GetLayerZ(sceneLayer);
    return Util.KInstantiate(original, position, Quaternion.identity, parent, name, gameLayer: gameLayer);
  }

  public static GameObject KInstantiate(
    GameObject original,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original, Vector3.zero, sceneLayer, name, gameLayer);
  }

  public static GameObject KInstantiate(
    Component original,
    Grid.SceneLayer sceneLayer,
    string name = null,
    int gameLayer = 0)
  {
    return GameUtil.KInstantiate(original.gameObject, Vector3.zero, sceneLayer, name, gameLayer);
  }

  public static unsafe void IsEmissionBlocked(
    int cell,
    out bool all_not_gaseous,
    out bool all_over_pressure)
  {
    int* numPtr = stackalloc int[4];
    numPtr[0] = Grid.CellBelow(cell);
    numPtr[1] = Grid.CellLeft(cell);
    numPtr[2] = Grid.CellRight(cell);
    numPtr[3] = Grid.CellAbove(cell);
    all_not_gaseous = true;
    all_over_pressure = true;
    for (int index1 = 0; index1 < 4; ++index1)
    {
      int index2 = numPtr[index1];
      if (Grid.IsValidCell(index2))
      {
        Element element = Grid.Element[index2];
        all_not_gaseous = all_not_gaseous && (!element.IsGas && !element.IsVacuum);
        all_over_pressure = all_over_pressure && (!element.IsGas && !element.IsVacuum || (double) Grid.Mass[index2] >= 1.79999995231628);
      }
    }
  }

  public static float GetDecorAtCell(int cell)
  {
    float num = 0.0f;
    if (!Grid.Solid[cell])
      num = Grid.Decor[cell] + (float) DecorProvider.GetLightDecorBonus(cell);
    return num;
  }

  public static string GetKeywordStyle(Tag tag)
  {
    Element element = ElementLoader.GetElement(tag);
    return element == null ? (!GameUtil.foodTags.Contains(tag) ? (!GameUtil.solidTags.Contains(tag) ? (string) null : "solid") : "food") : GameUtil.GetKeywordStyle(element);
  }

  public static string GetKeywordStyle(SimHashes hash)
  {
    Element elementByHash = ElementLoader.FindElementByHash(hash);
    return elementByHash != null ? GameUtil.GetKeywordStyle(elementByHash) : (string) null;
  }

  public static string GetKeywordStyle(Element element)
  {
    if (element.id == SimHashes.Oxygen)
      return "oxygen";
    if (element.IsSolid)
      return "solid";
    if (element.IsLiquid)
      return "liquid";
    if (element.IsGas)
      return "gas";
    return element.IsVacuum ? "vacuum" : (string) null;
  }

  public static string GetKeywordStyle(GameObject go)
  {
    string str = "";
    Edible component1 = go.GetComponent<Edible>();
    Equippable component2 = go.GetComponent<Equippable>();
    MedicinalPill component3 = go.GetComponent<MedicinalPill>();
    ResearchPointObject component4 = go.GetComponent<ResearchPointObject>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      str = "food";
    else if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      str = "equipment";
    else if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      str = "medicine";
    else if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      str = "research";
    return str;
  }

  public static string GenerateRandomDuplicantName()
  {
    string str1 = "";
    string str2 = "";
    bool flag = (double) UnityEngine.Random.Range(0.0f, 1f) >= 0.5;
    List<string> tList1 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.NB)));
    tList1.AddRange(flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.MALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.NAME.FEMALE)));
    string random = tList1.GetRandom<string>();
    if ((double) UnityEngine.Random.Range(0.0f, 1f) > 0.699999988079071)
    {
      List<string> tList2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.NB)));
      tList2.AddRange(flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.MALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.PREFIX.FEMALE)));
      str1 = tList2.GetRandom<string>();
    }
    if (!string.IsNullOrEmpty(str1))
      str1 += " ";
    if ((double) UnityEngine.Random.Range(0.0f, 1f) >= 0.899999976158142)
    {
      List<string> tList2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.NB)));
      tList2.AddRange(flag ? (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.MALE)) : (IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.DUPLICANT.SUFFIX.FEMALE)));
      str2 = tList2.GetRandom<string>();
    }
    if (!string.IsNullOrEmpty(str2))
      str2 = " " + str2;
    return str1 + random + str2;
  }

  public static string GenerateRandomRocketName()
  {
    string newValue1 = "";
    string newValue2 = "";
    string newValue3 = "";
    int num1 = 1;
    int num2 = 2;
    int num3 = 4;
    string random = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.NOUN))).GetRandom<string>();
    int num4 = 0;
    if ((double) UnityEngine.Random.value > 0.699999988079071)
    {
      newValue1 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.PREFIX))).GetRandom<string>();
      num4 |= num1;
    }
    if ((double) UnityEngine.Random.value > 0.5)
    {
      newValue2 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.ADJECTIVE))).GetRandom<string>();
      num4 |= num2;
    }
    if ((double) UnityEngine.Random.value > 0.100000001490116)
    {
      newValue3 = new List<string>((IEnumerable<string>) LocString.GetStrings(typeof (NAMEGEN.ROCKET.SUFFIX))).GetRandom<string>();
      num4 |= num3;
    }
    string str = num4 != (num1 | num2 | num3) ? (num4 != (num2 | num3) ? (num4 != (num1 | num3) ? (num4 != num3 ? (num4 != (num1 | num2) ? (num4 != num1 ? (num4 != num2 ? (string) NAMEGEN.ROCKET.FMT_NOUN : (string) NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN) : (string) NAMEGEN.ROCKET.FMT_PREFIX_NOUN) : (string) NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN) : (string) NAMEGEN.ROCKET.FMT_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_PREFIX_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_ADJECTIVE_NOUN_SUFFIX) : (string) NAMEGEN.ROCKET.FMT_PREFIX_ADJECTIVE_NOUN_SUFFIX;
    DebugUtil.LogArgs((object) "Rocket name bits:", (object) Convert.ToString(num4, 2));
    return str.Replace("{Prefix}", newValue1).Replace("{Adjective}", newValue2).Replace("{Noun}", random).Replace("{Suffix}", newValue3);
  }

  public static float GetThermalComfort(int cell, float tolerance = -0.08368001f)
  {
    float num1 = tolerance;
    float num2 = 0.0f;
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Creature);
    if ((double) Grid.Element[cell].thermalConductivity != 0.0)
      num2 = SimUtil.CalculateEnergyFlowCreatures(cell, 310.15f, elementByHash.specificHeatCapacity, elementByHash.thermalConductivity, creature_surface_thickness: 0.0045f);
    return (num2 - num1) * 1000f;
  }

  public static string GetFormattedDiseaseName(byte idx, bool color = false)
  {
    Klei.AI.Disease disease = Db.Get().Diseases[(int) idx];
    return color ? string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT, (object) disease.Name, (object) GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName))) : string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_NAME_FORMAT_NO_COLOR, (object) disease.Name);
  }

  public static string GetFormattedDisease(byte idx, int units, bool color = false)
  {
    if (idx == byte.MaxValue || units <= 0)
      return (string) UI.OVERLAYS.DISEASE.NO_DISEASE;
    Klei.AI.Disease disease = Db.Get().Diseases[(int) idx];
    return color ? string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_FORMAT, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units), (object) GameUtil.ColourToHex(GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName))) : string.Format((string) UI.OVERLAYS.DISEASE.DISEASE_FORMAT_NO_COLOR, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units));
  }

  public static string GetFormattedDiseaseAmount(int units) => units.ToString("#,##0") + (string) UI.UNITSUFFIXES.DISEASE.UNITS;

  public static string ColourizeString(Color32 colour, string str) => string.Format("<color=#{0}>{1}</color>", (object) GameUtil.ColourToHex(colour), (object) str);

  public static string ColourToHex(Color32 colour) => string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", (object) colour.r, (object) colour.g, (object) colour.b, (object) colour.a);

  public static string GetFormattedDecor(float value, bool enforce_max = false)
  {
    string str = "";
    LocString locString = (double) value > (double) DecorMonitor.MAXIMUM_DECOR_VALUE & enforce_max ? UI.OVERLAYS.DECOR.MAXIMUM_DECOR : UI.OVERLAYS.DECOR.VALUE;
    if (enforce_max)
      value = Math.Min(value, DecorMonitor.MAXIMUM_DECOR_VALUE);
    if ((double) value > 0.0)
      str = "+";
    else if ((double) value >= 0.0)
      locString = UI.OVERLAYS.DECOR.VALUE_ZERO;
    return string.Format((string) locString, (object) str, (object) value);
  }

  public static Color GetDecorColourFromValue(int decor)
  {
    Color black = Color.black;
    float f = (float) decor / 100f;
    return (double) f <= 0.0 ? Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(1f, 0.0f, 0.0f), Mathf.Abs(f)) : Color.Lerp(new Color(0.15f, 0.0f, 0.0f), new Color(0.0f, 1f, 0.0f), Mathf.Abs(f));
  }

  public static List<Descriptor> GetMaterialDescriptors(Element element)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (element.attributeModifiers.Count > 0)
    {
      foreach (AttributeModifier attributeModifier in element.attributeModifiers)
      {
        string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
        string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
        Descriptor descriptor = new Descriptor();
        descriptor.SetupDescriptor(txt, tooltip);
        descriptor.IncreaseIndent();
        descriptorList.Add(descriptor);
      }
    }
    descriptorList.AddRange((IEnumerable<Descriptor>) GameUtil.GetSignificantMaterialPropertyDescriptors(element));
    return descriptorList;
  }

  public static string GetMaterialTooltips(Element element)
  {
    string str = element.tag.ProperName();
    foreach (AttributeModifier attributeModifier in element.attributeModifiers)
    {
      string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
      string formattedString = attributeModifier.GetFormattedString((GameObject) null);
      str = str + "\n    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
    }
    return str + GameUtil.GetSignificantMaterialPropertyTooltips(element);
  }

  public static string GetSignificantMaterialPropertyTooltips(Element element)
  {
    string str = "";
    List<Descriptor> propertyDescriptors = GameUtil.GetSignificantMaterialPropertyDescriptors(element);
    if (propertyDescriptors.Count > 0)
    {
      str += "\n";
      for (int index = 0; index < propertyDescriptors.Count; ++index)
        str = str + "    • " + Util.StripTextFormatting(propertyDescriptors[index].text) + "\n";
    }
    return str;
  }

  public static List<Descriptor> GetSignificantMaterialPropertyDescriptors(
    Element element)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if ((double) element.thermalConductivity > 10.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.HIGH_THERMAL_CONDUCTIVITY, (object) GameUtil.GetThermalConductivityString(element, false, false)), string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_THERMAL_CONDUCTIVITY, (object) element.name, (object) element.thermalConductivity.ToString("0.#####")));
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    if ((double) element.thermalConductivity < 1.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.LOW_THERMAL_CONDUCTIVITY, (object) GameUtil.GetThermalConductivityString(element, false, false)), string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_THERMAL_CONDUCTIVITY, (object) element.name, (object) element.thermalConductivity.ToString("0.#####")));
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    if ((double) element.specificHeatCapacity <= 0.200000002980232)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.LOW_SPECIFIC_HEAT_CAPACITY, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.LOW_SPECIFIC_HEAT_CAPACITY, (object) element.name, (object) (float) ((double) element.specificHeatCapacity * 1.0)));
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    if ((double) element.specificHeatCapacity >= 1.0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.HIGH_SPECIFIC_HEAT_CAPACITY, string.Format((string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.HIGH_SPECIFIC_HEAT_CAPACITY, (object) element.name, (object) (float) ((double) element.specificHeatCapacity * 1.0)));
      descriptor.IncreaseIndent();
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public static int NaturalBuildingCell(this KMonoBehaviour cmp) => Grid.PosToCell(cmp.transform.GetPosition());

  public static List<Descriptor> GetMaterialDescriptors(Tag tag)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      if (element.attributeModifiers.Count > 0)
      {
        foreach (AttributeModifier attributeModifier in element.attributeModifiers)
        {
          string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
          string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + attributeModifier.AttributeId.ToUpper())), (object) attributeModifier.GetFormattedString((GameObject) null));
          Descriptor descriptor = new Descriptor();
          descriptor.SetupDescriptor(txt, tooltip);
          descriptor.IncreaseIndent();
          descriptorList.Add(descriptor);
        }
      }
      descriptorList.AddRange((IEnumerable<Descriptor>) GameUtil.GetSignificantMaterialPropertyDescriptors(element));
    }
    else
    {
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrefabAttributeModifiers component = prefab.GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor1 in component.descriptors)
          {
            string txt = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS." + descriptor1.AttributeId.ToUpper())), (object) descriptor1.GetFormattedString((GameObject) null));
            string tooltip = string.Format((string) Strings.Get(new StringKey("STRINGS.ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP." + descriptor1.AttributeId.ToUpper())), (object) descriptor1.GetFormattedString((GameObject) null));
            Descriptor descriptor2 = new Descriptor();
            descriptor2.SetupDescriptor(txt, tooltip);
            descriptor2.IncreaseIndent();
            descriptorList.Add(descriptor2);
          }
        }
      }
    }
    return descriptorList;
  }

  public static string GetMaterialTooltips(Tag tag)
  {
    string str = tag.ProperName();
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      foreach (AttributeModifier attributeModifier in element.attributeModifiers)
      {
        string name = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId).Name;
        string formattedString = attributeModifier.GetFormattedString((GameObject) null);
        str = str + "\n    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
      }
      str += GameUtil.GetSignificantMaterialPropertyTooltips(element);
    }
    else
    {
      GameObject prefab = Assets.TryGetPrefab(tag);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        PrefabAttributeModifiers component = prefab.GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor in component.descriptors)
          {
            string name = Db.Get().BuildingAttributes.Get(descriptor.AttributeId).Name;
            string formattedString = descriptor.GetFormattedString((GameObject) null);
            str = str + "\n    • " + string.Format((string) DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, (object) name, (object) formattedString);
          }
        }
      }
    }
    return str;
  }

  public static bool AreChoresUIMergeable(
    Chore.Precondition.Context choreA,
    Chore.Precondition.Context choreB)
  {
    if (choreA.chore.target.isNull || choreB.chore.target.isNull)
      return false;
    ChoreType choreType1 = choreB.chore.choreType;
    ChoreType choreType2 = choreA.chore.choreType;
    return choreA.chore.choreType == choreB.chore.choreType && choreA.chore.target.GetComponent<KPrefabID>().PrefabTag == choreB.chore.target.GetComponent<KPrefabID>().PrefabTag || choreA.chore.choreType == Db.Get().ChoreTypes.Dig && choreB.chore.choreType == Db.Get().ChoreTypes.Dig || choreA.chore.choreType == Db.Get().ChoreTypes.Relax && choreB.chore.choreType == Db.Get().ChoreTypes.Relax || ((choreType2 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType2 == Db.Get().ChoreTypes.ReturnSuitUrgent) && (choreType1 == Db.Get().ChoreTypes.ReturnSuitIdle || choreType1 == Db.Get().ChoreTypes.ReturnSuitUrgent) || (UnityEngine.Object) choreA.chore.target.gameObject == (UnityEngine.Object) choreB.chore.target.gameObject && choreA.chore.choreType == choreB.chore.choreType);
  }

  public static string GetChoreName(Chore chore, object choreData)
  {
    string str = "";
    if (chore.choreType == Db.Get().ChoreTypes.Fetch || chore.choreType == Db.Get().ChoreTypes.MachineFetch || (chore.choreType == Db.Get().ChoreTypes.FabricateFetch || chore.choreType == Db.Get().ChoreTypes.FetchCritical) || chore.choreType == Db.Get().ChoreTypes.PowerFetch)
      str = chore.GetReportName(chore.gameObject.GetProperName());
    else if (chore.choreType == Db.Get().ChoreTypes.StorageFetch || chore.choreType == Db.Get().ChoreTypes.FoodFetch)
    {
      FetchChore fetchChore = chore as FetchChore;
      if (chore is FetchAreaChore fetchAreaChore)
      {
        GameObject getFetchTarget = fetchAreaChore.GetFetchTarget;
        KMonoBehaviour cmp = choreData as KMonoBehaviour;
        str = !((UnityEngine.Object) getFetchTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) ? chore.GetReportName() : chore.GetReportName(cmp.GetProperName())) : chore.GetReportName(getFetchTarget.GetProperName());
      }
      else if (fetchChore != null)
      {
        Pickupable fetchTarget = fetchChore.fetchTarget;
        KMonoBehaviour cmp = choreData as KMonoBehaviour;
        str = !((UnityEngine.Object) fetchTarget != (UnityEngine.Object) null) ? (!((UnityEngine.Object) cmp != (UnityEngine.Object) null) ? chore.GetReportName() : chore.GetReportName(cmp.GetProperName())) : chore.GetReportName(fetchTarget.GetProperName());
      }
    }
    else
      str = chore.GetReportName();
    return str;
  }

  public static string ChoreGroupsForChoreType(ChoreType choreType)
  {
    if (choreType.groups == null || choreType.groups.Length == 0)
      return (string) null;
    string str = "";
    for (int index = 0; index < choreType.groups.Length; ++index)
    {
      if (index != 0)
        str += (string) UI.UISIDESCREENS.MINIONTODOSIDESCREEN.CHORE_GROUP_SEPARATOR;
      str += choreType.groups[index].Name;
    }
    return str;
  }

  public static bool IsCapturingTimeLapse() => (UnityEngine.Object) Game.Instance != (UnityEngine.Object) null && (UnityEngine.Object) Game.Instance.timelapser != (UnityEngine.Object) null && Game.Instance.timelapser.CapturingTimelapseScreenshot;

  public static ExposureType GetExposureTypeForDisease(Klei.AI.Disease disease)
  {
    for (int index = 0; index < GERM_EXPOSURE.TYPES.Length; ++index)
    {
      if (disease.id == (HashedString) GERM_EXPOSURE.TYPES[index].germ_id)
        return GERM_EXPOSURE.TYPES[index];
    }
    return (ExposureType) null;
  }

  public static Sickness GetSicknessForDisease(Klei.AI.Disease disease)
  {
    for (int index = 0; index < GERM_EXPOSURE.TYPES.Length; ++index)
    {
      if (disease.id == (HashedString) GERM_EXPOSURE.TYPES[index].germ_id)
        return Db.Get().Sicknesses.Get(GERM_EXPOSURE.TYPES[index].sickness_id);
    }
    return (Sickness) null;
  }

  public static Color32 GetLogicColourOn() => (Color32) Color.green;

  public static Color32 GetLogicColourOff() => (Color32) Color.red;

  public static Color32 GetLogicColourDisconnected() => (Color32) Color.white;

  public static void SubscribeToTags<T>(T target, EventSystem.IntraObjectHandler<T> handler) where T : KMonoBehaviour
  {
    handler.Trigger(target.gameObject, (object) null);
    target.Subscribe<T>(-1582839653, handler);
  }

  public static EventSystem.IntraObjectHandler<T> CreateHasTagHandler<T>(
    Tag tag,
    System.Action<T, object> callback)
    where T : KMonoBehaviour
  {
    return new EventSystem.IntraObjectHandler<T>((System.Action<T, object>) ((component, data) =>
    {
      KPrefabID component1 = component.GetComponent<KPrefabID>();
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || !component1.HasTag(tag))
        return;
      callback(component, data);
    }));
  }

  public static EventSystem.IntraObjectHandler<T> CreateDoesntHaveTagHandler<T>(
    Tag tag,
    System.Action<T, object> callback)
    where T : KMonoBehaviour
  {
    return new EventSystem.IntraObjectHandler<T>((System.Action<T, object>) ((component, data) =>
    {
      KPrefabID component1 = component.GetComponent<KPrefabID>();
      if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null) || component1.HasTag(tag))
        return;
      callback(component, data);
    }));
  }

  public enum UnitClass
  {
    SimpleFloat,
    SimpleInteger,
    Temperature,
    Mass,
    Calories,
    Percent,
    Distance,
    Disease,
  }

  public enum TemperatureUnit
  {
    Celsius,
    Fahrenheit,
    Kelvin,
  }

  public enum MassUnit
  {
    Kilograms,
    Pounds,
  }

  public enum MetricMassFormat
  {
    UseThreshold,
    Kilogram,
    Gram,
    Tonne,
  }

  public enum TemperatureInterpretation
  {
    Absolute,
    Relative,
  }

  public enum TimeSlice
  {
    None,
    ModifyOnly,
    PerSecond,
    PerCycle,
  }

  public enum MeasureUnit
  {
    mass,
    kcal,
    quantity,
  }

  public enum WattageFormatterUnit
  {
    Watts,
    Kilowatts,
    Automatic,
  }

  public enum HeatEnergyFormatterUnit
  {
    DTU_S,
    KDTU_S,
    Automatic,
  }

  public struct FloodFillInfo
  {
    public int cell;
    public int depth;
  }

  public static class Hardness
  {
    public const int VERY_SOFT = 0;
    public const int SOFT = 10;
    public const int FIRM = 25;
    public const int VERY_FIRM = 50;
    public const int NEARLY_IMPENETRABLE = 150;
    public const int SUPER_HARD = 200;
    public const int IMPENETRABLE = 255;
    public static Color ImpenetrableColor = new Color(0.8313726f, 0.2862745f, 0.282353f);
    public static Color nearlyImpenetrableColor = new Color(0.7411765f, 0.3490196f, 0.4980392f);
    public static Color veryFirmColor = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    public static Color firmColor = new Color(0.5254902f, 0.4196078f, 0.6470588f);
    public static Color softColor = new Color(0.427451f, 0.4823529f, 0.7568628f);
    public static Color verySoftColor = new Color(0.4431373f, 0.6705883f, 0.8117647f);
  }

  public static class GermResistanceValues
  {
    public const float MEDIUM = 2f;
    public const float LARGE = 5f;
    public static Color NegativeLargeColor = new Color(0.8313726f, 0.2862745f, 0.282353f);
    public static Color NegativeMediumColor = new Color(0.7411765f, 0.3490196f, 0.4980392f);
    public static Color NegativeSmallColor = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    public static Color PositiveSmallColor = new Color(0.5254902f, 0.4196078f, 0.6470588f);
    public static Color PositiveMediumColor = new Color(0.427451f, 0.4823529f, 0.7568628f);
    public static Color PositiveLargeColor = new Color(0.4431373f, 0.6705883f, 0.8117647f);
  }

  public static class ThermalConductivityValues
  {
    public const float VERY_HIGH = 50f;
    public const float HIGH = 10f;
    public const float MEDIUM = 2f;
    public const float LOW = 1f;
    public static Color veryLowConductivityColor = new Color(0.8313726f, 0.2862745f, 0.282353f);
    public static Color lowConductivityColor = new Color(0.7411765f, 0.3490196f, 0.4980392f);
    public static Color mediumConductivityColor = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    public static Color highConductivityColor = new Color(0.5254902f, 0.4196078f, 0.6470588f);
    public static Color veryHighConductivityColor = new Color(0.427451f, 0.4823529f, 0.7568628f);
  }

  public static class BreathableValues
  {
    public static Color positiveColor = new Color(0.4431373f, 0.6705883f, 0.8117647f);
    public static Color warningColor = new Color(0.6392157f, 0.3921569f, 0.6039216f);
    public static Color negativeColor = new Color(0.8313726f, 0.2862745f, 0.282353f);
  }

  public static class WireLoadValues
  {
    public static Color warningColor = new Color(0.9843137f, 0.6901961f, 0.2313726f);
    public static Color negativeColor = new Color(1f, 0.1921569f, 0.1921569f);
  }
}
