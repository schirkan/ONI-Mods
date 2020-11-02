// Decompiled with JetBrains decompiler
// Type: TUNING.CREATURES
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TUNING
{
  public class CREATURES
  {
    public const int DEFAULT_PROBING_RADIUS = 32;
    public const float FERTILITY_TIME_BY_LIFESPAN = 0.6f;
    public const float INCUBATION_TIME_BY_LIFESPAN = 0.2f;
    public const float INCUBATOR_INCUBATION_MULTIPLIER = 4f;
    public const float WILD_CALORIE_BURN_RATIO = 0.25f;
    public const float VIABILITY_LOSS_RATE = -0.01666667f;

    public class HITPOINTS
    {
      public const float TIER0 = 5f;
      public const float TIER1 = 25f;
      public const float TIER2 = 50f;
      public const float TIER3 = 100f;
      public const float TIER4 = 150f;
      public const float TIER5 = 200f;
      public const float TIER6 = 400f;
    }

    public class MASS_KG
    {
      public const float TIER0 = 5f;
      public const float TIER1 = 25f;
      public const float TIER2 = 50f;
      public const float TIER3 = 100f;
      public const float TIER4 = 200f;
      public const float TIER5 = 400f;
    }

    public class TEMPERATURE
    {
      public static float FREEZING_3 = 243f;
      public static float FREEZING_2 = 253f;
      public static float FREEZING_1 = 263f;
      public static float FREEZING = 273f;
      public static float COOL = 283f;
      public static float MODERATE = 293f;
      public static float HOT = 303f;
      public static float HOT_1 = 313f;
      public static float HOT_2 = 323f;
      public static float HOT_3 = 333f;
    }

    public class LIFESPAN
    {
      public const float TIER0 = 5f;
      public const float TIER1 = 25f;
      public const float TIER2 = 75f;
      public const float TIER3 = 100f;
      public const float TIER4 = 150f;
      public const float TIER5 = 200f;
      public const float TIER6 = 400f;
    }

    public class CONVERSION_EFFICIENCY
    {
      public static float BAD_2 = 0.1f;
      public static float BAD_1 = 0.25f;
      public static float NORMAL = 0.5f;
      public static float GOOD_1 = 0.75f;
      public static float GOOD_2 = 0.95f;
      public static float GOOD_3 = 1f;
    }

    public class SPACE_REQUIREMENTS
    {
      public static int TIER2 = 8;
      public static int TIER3 = 12;
      public static int TIER4 = 16;
    }

    public class EGG_CHANCE_MODIFIERS
    {
      public static List<System.Action> MODIFIER_CREATORS = new List<System.Action>()
      {
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchHard", "HatchHardEgg".ToTag(), SimHashes.SedimentaryRock.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchVeggie", "HatchVeggieEgg".ToTag(), SimHashes.Dirt.CreateTag(), 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("HatchMetal", "HatchMetalEgg".ToTag(), HatchMetalConfig.METAL_ORE_TAGS, 0.05f / HatchTuning.STANDARD_CALORIES_PER_CYCLE),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaBalance", "PuftAlphaEgg".ToTag(), "PuftAlpha".ToTag(), -0.00025f, true),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyOxylite", "PuftOxyliteEgg".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateNearbyCreatureModifier("PuftAlphaNearbyBleachstone", "PuftBleachstoneEgg".ToTag(), "PuftAlpha".ToTag(), 8.333333E-05f, false),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterHighTemp", "OilfloaterHighTempEgg".ToTag(), 373.15f, 523.15f, 8.333333E-05f, false),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("OilFloaterDecor", "OilfloaterDecorEgg".ToTag(), 293.15f, 333.15f, 8.333333E-05f, false),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugOrange", "LightBugOrangeEgg".ToTag(), "GrilledPrickleFruit".ToTag(), 1f / 800f),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPurple", "LightBugPurpleEgg".ToTag(), "FriedMushroom".ToTag(), 1f / 800f),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugPink", "LightBugPinkEgg".ToTag(), "SpiceBread".ToTag(), 1f / 800f),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlue", "LightBugBlueEgg".ToTag(), "Salsa".ToTag(), 1f / 800f),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugBlack", "LightBugBlackEgg".ToTag(), SimHashes.Phosphorus.CreateTag(), 1f / 800f),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("LightBugCrystal", "LightBugCrystalEgg".ToTag(), "CookedMeat".ToTag(), 1f / 800f),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuTropical", "PacuTropicalEgg".ToTag(), 308.15f, 353.15f, 8.333333E-05f, false),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateTemperatureModifier("PacuCleaner", "PacuCleanerEgg".ToTag(), 243.15f, 278.15f, 8.333333E-05f, false),
        CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier("DreckoPlastic", "DreckoPlasticEgg".ToTag(), "BasicSingleHarvestPlant".ToTag(), 0.025f / DreckoTuning.STANDARD_CALORIES_PER_CYCLE)
      };

      private static System.Action CreateDietaryModifier(
        string id,
        Tag eggTag,
        TagBits foodTags,
        float modifierPerCal)
      {
        return (System.Action) (() =>
        {
          string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.DIET.NAME;
          string desc = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.DIET.DESC;
          List<Tag> foodTagsActual = foodTags.GetTagsVerySlow();
          Db.Get().CreateFertilityModifier(id, eggTag, name, desc, (Func<string, string>) (descStr =>
          {
            string str = string.Join(", ", foodTagsActual.Select<Tag, string>((Func<Tag, string>) (t => t.ProperName())).ToArray<string>());
            descStr = string.Format(descStr, (object) str);
            return descStr;
          }), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
          {
            // ISSUE: variable of a compiler-generated type
            CREATURES.EGG_CHANCE_MODIFIERS.\u003C\u003Ec__DisplayClass0_0 cDisplayClass00 = this;
            FertilityMonitor.Instance inst1 = inst;
            Tag eggType1 = eggType;
            inst1.gameObject.Subscribe(-2038961714, (System.Action<object>) (data =>
            {
              CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent) data;
              TagBits tag_bits = new TagBits(caloriesConsumedEvent.tag);
              // ISSUE: reference to a compiler-generated field
              if (!cDisplayClass00.foodTags.HasAny(ref tag_bits))
                return;
              // ISSUE: reference to a compiler-generated field
              inst1.AddBreedingChance(eggType1, caloriesConsumedEvent.calories * cDisplayClass00.modifierPerCal);
            }));
          }));
        });
      }

      private static System.Action CreateDietaryModifier(
        string id,
        Tag eggTag,
        Tag foodTag,
        float modifierPerCal)
      {
        return CREATURES.EGG_CHANCE_MODIFIERS.CreateDietaryModifier(id, eggTag, new TagBits(foodTag), modifierPerCal);
      }

      private static System.Action CreateNearbyCreatureModifier(
        string id,
        Tag eggTag,
        Tag nearbyCreature,
        float modifierPerSecond,
        bool alsoInvert)
      {
        return (System.Action) (() =>
        {
          string name = (string) ((double) modifierPerSecond < 0.0 ? STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.NAME : STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.NAME);
          string description = (string) ((double) modifierPerSecond < 0.0 ? STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE_NEG.DESC : STRINGS.CREATURES.FERTILITY_MODIFIERS.NEARBY_CREATURE.DESC);
          Db.Get().CreateFertilityModifier(id, eggTag, name, description, (Func<string, string>) (descStr => string.Format(descStr, (object) nearbyCreature.ProperName())), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
          {
            // ISSUE: variable of a compiler-generated type
            CREATURES.EGG_CHANCE_MODIFIERS.\u003C\u003Ec__DisplayClass2_0 cDisplayClass20 = this;
            FertilityMonitor.Instance inst1 = inst;
            Tag eggType1 = eggType;
            NearbyCreatureMonitor.Instance instance = inst1.gameObject.GetSMI<NearbyCreatureMonitor.Instance>();
            if (instance == null)
            {
              instance = new NearbyCreatureMonitor.Instance(inst1.master);
              instance.StartSM();
            }
            instance.OnUpdateNearbyCreatures += (System.Action<float, List<KPrefabID>>) ((dt, creatures) =>
            {
              bool flag = false;
              foreach (KPrefabID creature in creatures)
              {
                // ISSUE: reference to a compiler-generated field
                if (creature.PrefabTag == cDisplayClass20.nearbyCreature)
                {
                  flag = true;
                  break;
                }
              }
              if (flag)
              {
                // ISSUE: reference to a compiler-generated field
                inst1.AddBreedingChance(eggType1, dt * cDisplayClass20.modifierPerSecond);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!cDisplayClass20.alsoInvert)
                  return;
                // ISSUE: reference to a compiler-generated field
                inst1.AddBreedingChance(eggType1, dt * -cDisplayClass20.modifierPerSecond);
              }
            });
          }));
        });
      }

      private static System.Action CreateTemperatureModifier(
        string id,
        Tag eggTag,
        float minTemp,
        float maxTemp,
        float modifierPerSecond,
        bool alsoInvert)
      {
        return (System.Action) (() =>
        {
          string name = (string) STRINGS.CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.NAME;
          Db.Get().CreateFertilityModifier(id, eggTag, name, (string) null, (Func<string, string>) (src => string.Format((string) STRINGS.CREATURES.FERTILITY_MODIFIERS.TEMPERATURE.DESC, (object) GameUtil.GetFormattedTemperature(minTemp), (object) GameUtil.GetFormattedTemperature(maxTemp))), (FertilityModifier.FertilityModFn) ((inst, eggType) =>
          {
            // ISSUE: variable of a compiler-generated type
            CREATURES.EGG_CHANCE_MODIFIERS.\u003C\u003Ec__DisplayClass3_0 cDisplayClass30 = this;
            FertilityMonitor.Instance inst1 = inst;
            Tag eggType1 = eggType;
            TemperatureVulnerable component = inst1.master.GetComponent<TemperatureVulnerable>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              component.OnTemperature += (System.Action<float, float>) ((dt, newTemp) =>
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((double) newTemp > (double) cDisplayClass30.minTemp && (double) newTemp < (double) cDisplayClass30.maxTemp)
                {
                  // ISSUE: reference to a compiler-generated field
                  inst1.AddBreedingChance(eggType1, dt * cDisplayClass30.modifierPerSecond);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (!cDisplayClass30.alsoInvert)
                    return;
                  // ISSUE: reference to a compiler-generated field
                  inst1.AddBreedingChance(eggType1, dt * -cDisplayClass30.modifierPerSecond);
                }
              });
            else
              DebugUtil.LogErrorArgs((object) "Ack! Trying to add temperature modifier", (object) id, (object) "to", (object) inst1.master.name, (object) "but it's not temperature vulnerable!");
          }));
        });
      }
    }
  }
}
