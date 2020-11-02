// Decompiled with JetBrains decompiler
// Type: Database.Amounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class Amounts : ResourceSet<Amount>
  {
    public Amount Stamina;
    public Amount Calories;
    public Amount ImmuneLevel;
    public Amount ExternalTemperature;
    public Amount Breath;
    public Amount Stress;
    public Amount Toxicity;
    public Amount Bladder;
    public Amount Decor;
    public Amount Temperature;
    public Amount HitPoints;
    public Amount AirPressure;
    public Amount Maturity;
    public Amount OldAge;
    public Amount Age;
    public Amount Fertilization;
    public Amount Illumination;
    public Amount Irrigation;
    public Amount CreatureCalories;
    public Amount Fertility;
    public Amount Viability;
    public Amount Wildness;
    public Amount Incubation;
    public Amount ScaleGrowth;
    public Amount InternalBattery;
    public Amount Rot;

    public void Load()
    {
      this.Stamina = this.CreateAmount("Stamina", 0.0f, 100f, false, Units.Flat, 0.35f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_stamina", "attribute_stamina");
      this.Stamina.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
      this.Calories = this.CreateAmount("Calories", 0.0f, 0.0f, false, Units.Flat, 4000f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_calories", "attribute_calories");
      this.Calories.SetDisplayer((IAmountDisplayer) new CaloriesDisplayer());
      this.Temperature = this.CreateAmount("Temperature", 0.0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_temperature");
      this.Temperature.SetDisplayer((IAmountDisplayer) new DuplicantTemperatureDeltaAsEnergyAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
      this.ExternalTemperature = this.CreateAmount("ExternalTemperature", 0.0f, 10000f, false, Units.Kelvin, 0.5f, true, "STRINGS.DUPLICANTS.STATS");
      this.ExternalTemperature.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Temperature, GameUtil.TimeSlice.PerSecond));
      this.Breath = this.CreateAmount("Breath", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_breath");
      this.Breath.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond));
      this.Stress = this.CreateAmount("Stress", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_stress", "attribute_stress");
      this.Stress.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Toxicity = this.CreateAmount("Toxicity", 0.0f, 100f, true, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS");
      this.Toxicity.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
      this.Bladder = this.CreateAmount("Bladder", 0.0f, 100f, false, Units.Flat, 0.5f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_bladder");
      this.Bladder.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerCycle));
      this.Decor = this.CreateAmount("Decor", -1000f, 1000f, false, Units.Flat, 0.01666667f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_decor");
      this.Decor.SetDisplayer((IAmountDisplayer) new DecorDisplayer());
      this.Maturity = this.CreateAmount("Maturity", 0.0f, 0.0f, true, Units.Flat, 0.0009166667f, true, "STRINGS.CREATURES.STATS", "ui_icon_maturity");
      this.Maturity.SetDisplayer((IAmountDisplayer) new MaturityDisplayer());
      this.OldAge = this.CreateAmount("OldAge", 0.0f, 0.0f, false, Units.Flat, 0.0f, false, "STRINGS.CREATURES.STATS");
      this.Fertilization = this.CreateAmount("Fertilization", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS");
      this.Fertilization.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond));
      this.Fertility = this.CreateAmount("Fertility", 0.0f, 100f, true, Units.Flat, 0.008375f, true, "STRINGS.CREATURES.STATS", "ui_icon_fertility");
      this.Fertility.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Wildness = this.CreateAmount("Wildness", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", "ui_icon_wildness");
      this.Wildness.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Incubation = this.CreateAmount("Incubation", 0.0f, 100f, true, Units.Flat, 0.01675f, true, "STRINGS.CREATURES.STATS", "ui_icon_incubation");
      this.Incubation.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Viability = this.CreateAmount("Viability", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS");
      this.Viability.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Age = this.CreateAmount("Age", 0.0f, 0.0f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", "ui_icon_age");
      this.Age.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle));
      this.Irrigation = this.CreateAmount("Irrigation", 0.0f, 1f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS");
      this.Irrigation.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Percent, GameUtil.TimeSlice.PerSecond));
      this.HitPoints = this.CreateAmount("HitPoints", 0.0f, 0.0f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_hitpoints", "attribute_hitpoints");
      this.HitPoints.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleInteger, GameUtil.TimeSlice.PerCycle));
      this.ImmuneLevel = this.CreateAmount("ImmuneLevel", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.DUPLICANTS.STATS", "ui_icon_immunelevel", "attribute_immunelevel");
      this.ImmuneLevel.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.Rot = this.CreateAmount("Rot", 0.0f, 0.0f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES.STATS");
      this.Rot.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.AirPressure = this.CreateAmount("AirPressure", 0.0f, 1E+09f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES.STATS");
      this.AirPressure.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.Mass, GameUtil.TimeSlice.PerSecond));
      this.Illumination = this.CreateAmount("Illumination", 0.0f, 1f, false, Units.Flat, 0.0f, true, "STRINGS.CREATURES.STATS");
      this.Illumination.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None));
      this.ScaleGrowth = this.CreateAmount("ScaleGrowth", 0.0f, 100f, true, Units.Flat, 0.1675f, true, "STRINGS.CREATURES.STATS", "ui_icon_scale_growth");
      this.ScaleGrowth.SetDisplayer((IAmountDisplayer) new AsPercentAmountDisplayer(GameUtil.TimeSlice.PerCycle));
      this.InternalBattery = this.CreateAmount("InternalBattery", 0.0f, 0.0f, false, Units.Flat, 4000f, true, "STRINGS.ROBOTS.STATS.", "ui_icon_stress", "attribute_stress");
      this.InternalBattery.SetDisplayer((IAmountDisplayer) new StandardAmountDisplayer(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None));
    }

    public Amount CreateAmount(
      string id,
      float min,
      float max,
      bool show_max,
      Units units,
      float delta_threshold,
      bool show_in_ui,
      string string_root,
      string uiSprite = null,
      string thoughtSprite = null)
    {
      string name1 = (string) Strings.Get(string.Format("{1}.{0}.NAME", (object) id.ToUpper(), (object) string_root.ToUpper()));
      string description = (string) Strings.Get(string.Format("{1}.{0}.TOOLTIP", (object) id.ToUpper(), (object) string_root.ToUpper()));
      Attribute attribute1 = new Attribute(id + "Min", "Minimum" + name1, "", "", min, Attribute.Display.Normal, false);
      Attribute attribute2 = new Attribute(id + "Max", "Maximum" + name1, "", "", max, Attribute.Display.Normal, false);
      string id1 = id + "Delta";
      string name2 = (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.ATTRIBUTES.{0}.NAME", (object) id1.ToUpper()));
      string attribute_description = (string) Strings.Get(string.Format("STRINGS.DUPLICANTS.ATTRIBUTES.{0}.DESC", (object) id1.ToUpper()));
      Attribute attribute3 = new Attribute(id1, name2, "", attribute_description, 0.0f, Attribute.Display.Normal, false);
      Amount resource = new Amount(id, name1, description, attribute1, attribute2, attribute3, show_max, units, delta_threshold, show_in_ui, uiSprite, thoughtSprite);
      Db.Get().Attributes.Add(attribute1);
      Db.Get().Attributes.Add(attribute2);
      Db.Get().Attributes.Add(attribute3);
      this.Add(resource);
      return resource;
    }
  }
}
