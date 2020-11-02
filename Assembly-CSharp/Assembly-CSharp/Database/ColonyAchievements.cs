﻿// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievements
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Database
{
  public class ColonyAchievements : ResourceSet<ColonyAchievement>
  {
    public ColonyAchievement Thriving;
    public ColonyAchievement ReachedDistantPlanet;
    public ColonyAchievement Survived100Cycles;
    public ColonyAchievement ReachedSpace;
    public ColonyAchievement CompleteSkillBranch;
    public ColonyAchievement CompleteResearchTree;
    public ColonyAchievement Clothe8Dupes;
    public ColonyAchievement Build4NatureReserves;
    public ColonyAchievement Minimum20LivingDupes;
    public ColonyAchievement TameAGassyMoo;
    public ColonyAchievement CoolBuildingTo6K;
    public ColonyAchievement EatkCalFromMeatByCycle100;
    public ColonyAchievement NoFarmTilesAndKCal;
    public ColonyAchievement Generate240000kJClean;
    public ColonyAchievement BuildOutsideStartBiome;
    public ColonyAchievement Travel10000InTubes;
    public ColonyAchievement VarietyOfRooms;
    public ColonyAchievement TameAllBasicCritters;
    public ColonyAchievement SurviveOneYear;
    public ColonyAchievement ExploreOilBiome;
    public ColonyAchievement EatCookedFood;
    public ColonyAchievement BasicPumping;
    public ColonyAchievement BasicComforts;
    public ColonyAchievement PlumbedWashrooms;
    public ColonyAchievement AutomateABuilding;
    public ColonyAchievement MasterpiecePainting;
    public ColonyAchievement InspectPOI;
    public ColonyAchievement HatchACritter;
    public ColonyAchievement CuredDisease;
    public ColonyAchievement GeneratorTuneup;
    public ColonyAchievement ClearFOW;
    public ColonyAchievement HatchRefinement;
    public ColonyAchievement BunkerDoorDefense;
    public ColonyAchievement IdleDuplicants;
    public ColonyAchievement ExosuitCycles;

    public ColonyAchievements(ResourceSet parent)
      : base(nameof (ColonyAchievements), parent)
    {
      string name1 = (string) COLONY_ACHIEVEMENTS.THRIVING.NAME;
      string description1 = (string) COLONY_ACHIEVEMENTS.THRIVING.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist1 = new List<ColonyAchievementRequirement>();
      requirementChecklist1.Add((ColonyAchievementRequirement) new CycleNumber(200));
      requirementChecklist1.Add((ColonyAchievementRequirement) new MinimumMorale());
      requirementChecklist1.Add((ColonyAchievementRequirement) new NumberOfDupes(12));
      requirementChecklist1.Add((ColonyAchievementRequirement) new MonumentBuilt());
      string messageTitle1 = (string) COLONY_ACHIEVEMENTS.THRIVING.MESSAGE_TITLE;
      string messageBody1 = (string) COLONY_ACHIEVEMENTS.THRIVING.MESSAGE_BODY;
      System.Action<KMonoBehaviour> VictorySequence1 = new System.Action<KMonoBehaviour>(ThrivingSequence.Start);
      string nisGenericSnapshot = AudioMixerSnapshots.Get().VictoryNISGenericSnapshot;
      this.Thriving = this.Add(new ColonyAchievement(nameof (Thriving), "WINCONDITION_STAY", name1, description1, true, requirementChecklist1, messageTitle1, messageBody1, "victoryShorts/Stay", "victoryLoops/Stay_loop", VictorySequence1, nisGenericSnapshot, "home_sweet_home"));
      string name2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.NAME;
      string description2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.DESCRIPTION;
      List<ColonyAchievementRequirement> requirementChecklist2 = new List<ColonyAchievementRequirement>();
      requirementChecklist2.Add((ColonyAchievementRequirement) new Database.ReachedSpace(Db.Get().SpaceDestinationTypes.Wormhole));
      string messageTitle2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.MESSAGE_TITLE;
      string messageBody2 = (string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.MESSAGE_BODY;
      System.Action<KMonoBehaviour> VictorySequence2 = new System.Action<KMonoBehaviour>(ReachedDistantPlanetSequence.Start);
      string nisRocketSnapshot = AudioMixerSnapshots.Get().VictoryNISRocketSnapshot;
      this.ReachedDistantPlanet = this.Add(new ColonyAchievement(nameof (ReachedDistantPlanet), "WINCONDITION_LEAVE", name2, description2, true, requirementChecklist2, messageTitle2, messageBody2, "victoryShorts/Leave", "victoryLoops/Leave_loop", VictorySequence2, nisRocketSnapshot, "rocket"));
      this.Survived100Cycles = this.Add(new ColonyAchievement(nameof (Survived100Cycles), "SURVIVE_HUNDRED_CYCLES", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_HUNDRED_CYCLES, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_HUNDRED_CYCLES_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CycleNumber()
      }, icon: "Turn_of_the_Century"));
      this.ReachedSpace = this.Add(new ColonyAchievement(nameof (ReachedSpace), "REACH_SPACE_ANY_DESTINATION", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new Database.ReachedSpace()
      }, icon: "space_race"));
      this.CompleteSkillBranch = this.Add(new ColonyAchievement(nameof (CompleteSkillBranch), "COMPLETED_SKILL_BRANCH", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_SKILL_BRANCH, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_SKILL_BRANCH_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new SkillBranchComplete(new List<Skill>()
        {
          Db.Get().Skills.Mining3,
          Db.Get().Skills.Building3,
          Db.Get().Skills.Farming3,
          Db.Get().Skills.Ranching2,
          Db.Get().Skills.Researching3,
          Db.Get().Skills.Cooking2,
          Db.Get().Skills.Arting3,
          Db.Get().Skills.Hauling2,
          Db.Get().Skills.Technicals2,
          Db.Get().Skills.Engineering1,
          Db.Get().Skills.Basekeeping2,
          Db.Get().Skills.Astronauting2,
          Db.Get().Skills.Medicine3
        })
      }, icon: nameof (CompleteSkillBranch)));
      this.CompleteResearchTree = this.Add(new ColonyAchievement(nameof (CompleteResearchTree), "COMPLETED_RESEARCH", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_RESEARCH, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COMPLETED_RESEARCH_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new ResearchComplete()
      }, icon: "honorary_doctorate"));
      this.Clothe8Dupes = this.Add(new ColonyAchievement(nameof (Clothe8Dupes), "EQUIP_EIGHT_DUPES", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EQUIP_N_DUPES, string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EQUIP_N_DUPES_DESCRIPTION, (object) 8), false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new EquipNDupes(Db.Get().AssignableSlots.Outfit, 8)
      }, icon: "and_nowhere_to_go"));
      this.TameAllBasicCritters = this.Add(new ColonyAchievement(nameof (TameAllBasicCritters), "TAME_BASIC_CRITTERS", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_BASIC_CRITTERS, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_BASIC_CRITTERS_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CritterTypesWithTraits(new List<Tag>()
        {
          (Tag) "Drecko",
          (Tag) "Hatch",
          (Tag) "LightBug",
          (Tag) "Mole",
          (Tag) "Oilfloater",
          (Tag) "Pacu",
          (Tag) "Puft",
          (Tag) "Moo",
          (Tag) "Crab",
          (Tag) "Squirrel"
        }, false)
      }, icon: "Animal_friends"));
      this.Build4NatureReserves = this.Add(new ColonyAchievement(nameof (Build4NatureReserves), "BUILD_NATURE_RESERVES", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_NATURE_RESERVES, string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_NATURE_RESERVES_DESCRIPTION, (object) Db.Get().RoomTypes.NatureReserve.Name, (object) 4), false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new BuildNRoomTypes(Db.Get().RoomTypes.NatureReserve, 4)
      }, icon: "Some_Reservations"));
      this.Minimum20LivingDupes = this.Add(new ColonyAchievement(nameof (Minimum20LivingDupes), "TWENTY_DUPES", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TWENTY_DUPES, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TWENTY_DUPES_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new NumberOfDupes(20)
      }, icon: "no_place_like_clone"));
      this.TameAGassyMoo = this.Add(new ColonyAchievement(nameof (TameAGassyMoo), "TAME_GASSYMOO", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_GASSYMOO, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TAME_GASSYMOO_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CritterTypesWithTraits(new List<Tag>()
        {
          (Tag) "Moo"
        }, false)
      }, icon: "moovin_on_up"));
      this.CoolBuildingTo6K = this.Add(new ColonyAchievement(nameof (CoolBuildingTo6K), "SIXKELVIN_BUILDING", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SIXKELVIN_BUILDING, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SIXKELVIN_BUILDING_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CoolBuildingToXKelvin(6)
      }, icon: "not_0k"));
      this.EatkCalFromMeatByCycle100 = this.Add(new ColonyAchievement(nameof (EatkCalFromMeatByCycle100), "EAT_MEAT", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EAT_MEAT, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EAT_MEAT_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new BeforeCycleNumber(),
        (ColonyAchievementRequirement) new EatXCaloriesFromY(400000, new List<string>()
        {
          TUNING.FOOD.FOOD_TYPES.MEAT.Id,
          TUNING.FOOD.FOOD_TYPES.FISH_MEAT.Id,
          TUNING.FOOD.FOOD_TYPES.COOKED_MEAT.Id,
          TUNING.FOOD.FOOD_TYPES.COOKED_FISH.Id,
          TUNING.FOOD.FOOD_TYPES.SURF_AND_TURF.Id,
          TUNING.FOOD.FOOD_TYPES.BURGER.Id
        })
      }, icon: "Carnivore"));
      this.NoFarmTilesAndKCal = this.Add(new ColonyAchievement(nameof (NoFarmTilesAndKCal), "NO_PLANTERBOX", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.NO_PLANTERBOX, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.NO_PLANTERBOX_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new NoFarmables(),
        (ColonyAchievementRequirement) new EatXCalories(400000)
      }, icon: "Locavore"));
      this.Generate240000kJClean = this.Add(new ColonyAchievement(nameof (Generate240000kJClean), "CLEAN_ENERGY", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAN_ENERGY, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAN_ENERGY_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new ProduceXEngeryWithoutUsingYList(240000f, new List<Tag>()
        {
          (Tag) "MethaneGenerator",
          (Tag) "PetroleumGenerator",
          (Tag) "WoodGasGenerator",
          (Tag) "Generator"
        })
      }, icon: "sustainably_sustaining"));
      this.BuildOutsideStartBiome = this.Add(new ColonyAchievement(nameof (BuildOutsideStartBiome), "BUILD_OUTSIDE_BIOME", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_OUTSIDE_BIOME, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUILD_OUTSIDE_BIOME_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new Database.BuildOutsideStartBiome()
      }, icon: "build_outside"));
      this.Travel10000InTubes = this.Add(new ColonyAchievement(nameof (Travel10000InTubes), "TUBE_TRAVEL_DISTANCE", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TUBE_TRAVEL_DISTANCE, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.TUBE_TRAVEL_DISTANCE_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new TravelXUsingTransitTubes(NavType.Tube, 10000)
      }, icon: "Totally-Tubular"));
      this.VarietyOfRooms = this.Add(new ColonyAchievement(nameof (VarietyOfRooms), "VARIETY_OF_ROOMS", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.VARIETY_OF_ROOMS, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.VARIETY_OF_ROOMS_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.NatureReserve),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.Hospital),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.RecRoom),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.GreatHall),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.Bedroom),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.PlumbedBathroom),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.Farm),
        (ColonyAchievementRequirement) new BuildRoomType(Db.Get().RoomTypes.CreaturePen)
      }, icon: "Get-a-Room"));
      this.SurviveOneYear = this.Add(new ColonyAchievement(nameof (SurviveOneYear), "SURVIVE_ONE_YEAR", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_ONE_YEAR, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.SURVIVE_ONE_YEAR_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new FractionalCycleNumber(365.25f)
      }, icon: "One_year"));
      this.ExploreOilBiome = this.Add(new ColonyAchievement(nameof (ExploreOilBiome), "EXPLORE_OIL_BIOME", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXPLORE_OIL_BIOME, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXPLORE_OIL_BIOME_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new ExploreOilFieldSubZone()
      }, icon: "enter_oil_biome"));
      this.EatCookedFood = this.Add(new ColonyAchievement(nameof (EatCookedFood), "COOKED_FOOD", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COOKED_FOOD, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.COOKED_FOOD_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new EatXKCalProducedByY(1, new List<Tag>()
        {
          (Tag) "GourmetCookingStation",
          (Tag) "CookingStation"
        })
      }, icon: "its_not_raw"));
      this.BasicPumping = this.Add(new ColonyAchievement(nameof (BasicPumping), "BASIC_PUMPING", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_PUMPING, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_PUMPING_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new VentXKG(SimHashes.Oxygen, 1000f)
      }, icon: nameof (BasicPumping)));
      this.BasicComforts = this.Add(new ColonyAchievement(nameof (BasicComforts), "BASIC_COMFORTS", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_COMFORTS, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BASIC_COMFORTS_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new AtLeastOneBuildingForEachDupe(new List<Tag>()
        {
          (Tag) "FlushToilet",
          (Tag) "Outhouse"
        }),
        (ColonyAchievementRequirement) new AtLeastOneBuildingForEachDupe(new List<Tag>()
        {
          (Tag) BedConfig.ID,
          (Tag) LuxuryBedConfig.ID
        })
      }, icon: "1bed_1toilet"));
      this.PlumbedWashrooms = this.Add(new ColonyAchievement(nameof (PlumbedWashrooms), "PLUMBED_WASHROOMS", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.PLUMBED_WASHROOMS, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.PLUMBED_WASHROOMS_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new UpgradeAllBasicBuildings((Tag) "Outhouse", (Tag) "FlushToilet"),
        (ColonyAchievementRequirement) new UpgradeAllBasicBuildings((Tag) "WashBasin", (Tag) "WashSink")
      }, icon: "royal_flush"));
      this.AutomateABuilding = this.Add(new ColonyAchievement(nameof (AutomateABuilding), "AUTOMATE_A_BUILDING", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.AUTOMATE_A_BUILDING, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.AUTOMATE_A_BUILDING_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new Database.AutomateABuilding()
      }, icon: "red_light_green_light"));
      this.MasterpiecePainting = this.Add(new ColonyAchievement(nameof (MasterpiecePainting), "MASTERPIECE_PAINTING", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MASTERPIECE_PAINTING, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.MASTERPIECE_PAINTING_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CreateMasterPainting()
      }, icon: "art_underground"));
      this.InspectPOI = this.Add(new ColonyAchievement(nameof (InspectPOI), "INSPECT_POI", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.INSPECT_POI, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.INSPECT_POI_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new ActivateLorePOI()
      }, icon: "ghosts_of_gravitas"));
      this.HatchACritter = this.Add(new ColonyAchievement(nameof (HatchACritter), "HATCH_A_CRITTER", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_A_CRITTER, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_A_CRITTER_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CritterTypeExists(new List<Tag>()
        {
          (Tag) "DreckoPlasticBaby",
          (Tag) "HatchHardBaby",
          (Tag) "HatchMetalBaby",
          (Tag) "HatchVeggieBaby",
          (Tag) "LightBugBlackBaby",
          (Tag) "LightBugBlueBaby",
          (Tag) "LightBugCrystalBaby",
          (Tag) "LightBugOrangeBaby",
          (Tag) "LightBugPinkBaby",
          (Tag) "LightBugPurpleBaby",
          (Tag) "OilfloaterDecorBaby",
          (Tag) "OilfloaterHighTempBaby",
          (Tag) "PacuCleanerBaby",
          (Tag) "PacuTropicalBaby",
          (Tag) "PuftBleachstoneBaby",
          (Tag) "PuftOxyliteBaby"
        })
      }, icon: "good_egg"));
      this.CuredDisease = this.Add(new ColonyAchievement(nameof (CuredDisease), "CURED_DISEASE", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CURED_DISEASE, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CURED_DISEASE_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CureDisease()
      }, icon: "medic"));
      this.GeneratorTuneup = this.Add(new ColonyAchievement(nameof (GeneratorTuneup), "GENERATOR_TUNEUP", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GENERATOR_TUNEUP, string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.GENERATOR_TUNEUP_DESCRIPTION, (object) 100), false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new TuneUpGenerator(100f)
      }, icon: "tune_up_for_what"));
      this.ClearFOW = this.Add(new ColonyAchievement(nameof (ClearFOW), "CLEAR_FOW", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAR_FOW, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.CLEAR_FOW_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new RevealAsteriod(0.8f)
      }, icon: "pulling_back_the_veil"));
      this.HatchRefinement = this.Add(new ColonyAchievement(nameof (HatchRefinement), "HATCH_REFINEMENT", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_REFINEMENT, string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.HATCH_REFINEMENT_DESCRIPTION, (object) GameUtil.GetFormattedMass(10000f, massFormat: GameUtil.MetricMassFormat.Tonne)), false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new CreaturePoopKGProduction((Tag) "HatchMetal", 10000f)
      }, icon: "down_the_hatch"));
      this.BunkerDoorDefense = this.Add(new ColonyAchievement(nameof (BunkerDoorDefense), "BUNKER_DOOR_DEFENSE", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUNKER_DOOR_DEFENSE, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.BUNKER_DOOR_DEFENSE_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new BlockedCometWithBunkerDoor()
      }, icon: "Immovable_Object"));
      this.IdleDuplicants = this.Add(new ColonyAchievement(nameof (IdleDuplicants), "IDLE_DUPLICANTS", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.IDLE_DUPLICANTS, (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.IDLE_DUPLICANTS_DESCRIPTION, false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new DupesVsSolidTransferArmFetch(0.51f, 5)
      }, icon: "easy_livin"));
      this.ExosuitCycles = this.Add(new ColonyAchievement(nameof (ExosuitCycles), "EXOSUIT_CYCLES", (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXOSUIT_CYCLES, string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.EXOSUIT_CYCLES_DESCRIPTION, (object) 10), false, new List<ColonyAchievementRequirement>()
      {
        (ColonyAchievementRequirement) new DupesCompleteChoreInExoSuitForCycles(10)
      }, icon: "job_suitability"));
    }
  }
}
