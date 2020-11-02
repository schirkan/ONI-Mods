﻿// Decompiled with JetBrains decompiler
// Type: Database.SkillPerks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

namespace Database
{
  public class SkillPerks : ResourceSet<SkillPerk>
  {
    public SkillPerk IncreaseDigSpeedSmall;
    public SkillPerk IncreaseDigSpeedMedium;
    public SkillPerk IncreaseDigSpeedLarge;
    public SkillPerk CanDigVeryFirm;
    public SkillPerk CanDigNearlyImpenetrable;
    public SkillPerk CanDigSupersuperhard;
    public SkillPerk IncreaseConstructionSmall;
    public SkillPerk IncreaseConstructionMedium;
    public SkillPerk IncreaseConstructionLarge;
    public SkillPerk IncreaseConstructionMechatronics;
    public SkillPerk IncreaseLearningSmall;
    public SkillPerk IncreaseLearningMedium;
    public SkillPerk IncreaseLearningLarge;
    public SkillPerk IncreaseBotanySmall;
    public SkillPerk IncreaseBotanyMedium;
    public SkillPerk IncreaseBotanyLarge;
    public SkillPerk CanFarmTinker;
    public SkillPerk CanWrangleCreatures;
    public SkillPerk CanUseRanchStation;
    public SkillPerk IncreaseRanchingSmall;
    public SkillPerk IncreaseRanchingMedium;
    public SkillPerk IncreaseAthleticsSmall;
    public SkillPerk IncreaseAthleticsMedium;
    public SkillPerk IncreaseStrengthSmall;
    public SkillPerk IncreaseStrengthMedium;
    public SkillPerk IncreaseStrengthGofer;
    public SkillPerk IncreaseStrengthCourier;
    public SkillPerk IncreaseStrengthGroundskeeper;
    public SkillPerk IncreaseStrengthPlumber;
    public SkillPerk IncreaseCarryAmountSmall;
    public SkillPerk IncreaseCarryAmountMedium;
    public SkillPerk IncreaseArtSmall;
    public SkillPerk IncreaseArtMedium;
    public SkillPerk IncreaseArtLarge;
    public SkillPerk CanArt;
    public SkillPerk CanArtUgly;
    public SkillPerk CanArtOkay;
    public SkillPerk CanArtGreat;
    public SkillPerk IncreaseMachinerySmall;
    public SkillPerk IncreaseMachineryMedium;
    public SkillPerk IncreaseMachineryLarge;
    public SkillPerk ConveyorBuild;
    public SkillPerk CanPowerTinker;
    public SkillPerk CanElectricGrill;
    public SkillPerk IncreaseCookingSmall;
    public SkillPerk IncreaseCookingMedium;
    public SkillPerk IncreaseCaringSmall;
    public SkillPerk IncreaseCaringMedium;
    public SkillPerk IncreaseCaringLarge;
    public SkillPerk CanCompound;
    public SkillPerk CanDoctor;
    public SkillPerk CanAdvancedMedicine;
    public SkillPerk ExosuitExpertise;
    public SkillPerk AllowAdvancedResearch;
    public SkillPerk AllowInterstellarResearch;
    public SkillPerk CanStudyWorldObjects;
    public SkillPerk CanDoPlumbing;
    public SkillPerk CanUseRockets;
    public SkillPerk FasterSpaceFlight;
    public SkillPerk CanTrainToBeAstronaut;

    public SkillPerks(ResourceSet parent)
      : base(nameof (SkillPerks), parent)
    {
      this.IncreaseDigSpeedSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseDigSpeedSmall), Db.Get().Attributes.Digging.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_MINER.NAME));
      this.IncreaseDigSpeedMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseDigSpeedMedium), Db.Get().Attributes.Digging.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.MINER.NAME));
      this.IncreaseDigSpeedLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseDigSpeedLarge), Db.Get().Attributes.Digging.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.SENIOR_MINER.NAME));
      this.CanDigVeryFirm = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanDigVeryFirm), (string) UI.ROLES_SCREEN.PERKS.CAN_DIG_VERY_FIRM.DESCRIPTION));
      this.CanDigNearlyImpenetrable = this.Add((SkillPerk) new SimpleSkillPerk("CanDigAbyssalite", (string) UI.ROLES_SCREEN.PERKS.CAN_DIG_NEARLY_IMPENETRABLE.DESCRIPTION));
      this.CanDigSupersuperhard = this.Add((SkillPerk) new SimpleSkillPerk("CanDigDiamondAndObsidan", (string) UI.ROLES_SCREEN.PERKS.CAN_DIG_SUPER_SUPER_HARD.DESCRIPTION));
      this.IncreaseConstructionSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseConstructionSmall), Db.Get().Attributes.Construction.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME));
      this.IncreaseConstructionMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseConstructionMedium), Db.Get().Attributes.Construction.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.BUILDER.NAME));
      this.IncreaseConstructionLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseConstructionLarge), Db.Get().Attributes.Construction.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.SENIOR_BUILDER.NAME));
      this.IncreaseConstructionMechatronics = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseConstructionMechatronics), Db.Get().Attributes.Construction.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
      this.IncreaseLearningSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseLearningSmall), Db.Get().Attributes.Learning.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME));
      this.IncreaseLearningMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseLearningMedium), Db.Get().Attributes.Learning.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.RESEARCHER.NAME));
      this.IncreaseLearningLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseLearningLarge), Db.Get().Attributes.Learning.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME));
      this.IncreaseBotanySmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseBotanySmall), Db.Get().Attributes.Botanist.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_FARMER.NAME));
      this.IncreaseBotanyMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseBotanyMedium), Db.Get().Attributes.Botanist.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.FARMER.NAME));
      this.IncreaseBotanyLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseBotanyLarge), Db.Get().Attributes.Botanist.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.SENIOR_FARMER.NAME));
      this.CanFarmTinker = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanFarmTinker), (string) UI.ROLES_SCREEN.PERKS.CAN_FARM_TINKER.DESCRIPTION));
      this.IncreaseRanchingSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseRanchingSmall), Db.Get().Attributes.Ranching.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.RANCHER.NAME));
      this.IncreaseRanchingMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseRanchingMedium), Db.Get().Attributes.Ranching.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.SENIOR_RANCHER.NAME));
      this.CanWrangleCreatures = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanWrangleCreatures), (string) UI.ROLES_SCREEN.PERKS.CAN_WRANGLE_CREATURES.DESCRIPTION));
      this.CanUseRanchStation = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanUseRanchStation), (string) UI.ROLES_SCREEN.PERKS.CAN_USE_RANCH_STATION.DESCRIPTION));
      this.IncreaseAthleticsSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseAthleticsSmall), Db.Get().Attributes.Athletics.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.HAULER.NAME));
      this.IncreaseAthleticsMedium = this.Add((SkillPerk) new SkillAttributePerk("IncreaseAthletics", Db.Get().Attributes.Athletics.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.SUIT_EXPERT.NAME));
      this.IncreaseStrengthGofer = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseStrengthGofer), Db.Get().Attributes.Strength.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.HAULER.NAME));
      this.IncreaseStrengthCourier = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseStrengthCourier), Db.Get().Attributes.Strength.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
      this.IncreaseStrengthGroundskeeper = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseStrengthGroundskeeper), Db.Get().Attributes.Strength.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.HANDYMAN.NAME));
      this.IncreaseStrengthPlumber = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseStrengthPlumber), Db.Get().Attributes.Strength.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.PLUMBER.NAME));
      this.IncreaseCarryAmountSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCarryAmountSmall), Db.Get().Attributes.CarryAmount.Id, 400f, (string) DUPLICANTS.ROLES.HAULER.NAME));
      this.IncreaseCarryAmountMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCarryAmountMedium), Db.Get().Attributes.CarryAmount.Id, 800f, (string) DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME));
      this.IncreaseArtSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseArtSmall), Db.Get().Attributes.Art.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME));
      this.IncreaseArtMedium = this.Add((SkillPerk) new SkillAttributePerk("IncreaseArt", Db.Get().Attributes.Art.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.ARTIST.NAME));
      this.IncreaseArtLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseArtLarge), Db.Get().Attributes.Art.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.MASTER_ARTIST.NAME));
      this.CanArt = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanArt), (string) UI.ROLES_SCREEN.PERKS.CAN_ART.DESCRIPTION));
      this.CanArtUgly = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanArtUgly), (string) UI.ROLES_SCREEN.PERKS.CAN_ART_UGLY.DESCRIPTION));
      this.CanArtOkay = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanArtOkay), (string) UI.ROLES_SCREEN.PERKS.CAN_ART_OKAY.DESCRIPTION));
      this.CanArtGreat = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanArtGreat), (string) UI.ROLES_SCREEN.PERKS.CAN_ART_GREAT.DESCRIPTION));
      this.IncreaseMachinerySmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseMachinerySmall), Db.Get().Attributes.Machinery.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME));
      this.IncreaseMachineryMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseMachineryMedium), Db.Get().Attributes.Machinery.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME));
      this.IncreaseMachineryLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseMachineryLarge), Db.Get().Attributes.Machinery.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME));
      this.ConveyorBuild = this.Add((SkillPerk) new SimpleSkillPerk(nameof (ConveyorBuild), (string) UI.ROLES_SCREEN.PERKS.CONVEYOR_BUILD.DESCRIPTION));
      this.CanPowerTinker = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanPowerTinker), (string) UI.ROLES_SCREEN.PERKS.CAN_POWER_TINKER.DESCRIPTION));
      this.CanElectricGrill = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanElectricGrill), (string) UI.ROLES_SCREEN.PERKS.CAN_ELECTRIC_GRILL.DESCRIPTION));
      this.IncreaseCookingSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCookingSmall), Db.Get().Attributes.Cooking.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_COOK.NAME));
      this.IncreaseCookingMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCookingMedium), Db.Get().Attributes.Cooking.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.COOK.NAME));
      this.IncreaseCaringSmall = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCaringSmall), Db.Get().Attributes.Caring.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_FIRST, (string) DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME));
      this.IncreaseCaringMedium = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCaringMedium), Db.Get().Attributes.Caring.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_SECOND, (string) DUPLICANTS.ROLES.MEDIC.NAME));
      this.IncreaseCaringLarge = this.Add((SkillPerk) new SkillAttributePerk(nameof (IncreaseCaringLarge), Db.Get().Attributes.Caring.Id, (float) TUNING.ROLES.ATTRIBUTE_BONUS_THIRD, (string) DUPLICANTS.ROLES.SENIOR_MEDIC.NAME));
      this.CanCompound = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanCompound), (string) UI.ROLES_SCREEN.PERKS.CAN_COMPOUND.DESCRIPTION));
      this.CanDoctor = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanDoctor), (string) UI.ROLES_SCREEN.PERKS.CAN_DOCTOR.DESCRIPTION));
      this.CanAdvancedMedicine = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanAdvancedMedicine), (string) UI.ROLES_SCREEN.PERKS.CAN_ADVANCED_MEDICINE.DESCRIPTION));
      this.ExosuitExpertise = this.Add((SkillPerk) new SimpleSkillPerk(nameof (ExosuitExpertise), (string) UI.ROLES_SCREEN.PERKS.EXOSUIT_EXPERTISE.DESCRIPTION));
      this.AllowAdvancedResearch = this.Add((SkillPerk) new SimpleSkillPerk(nameof (AllowAdvancedResearch), (string) UI.ROLES_SCREEN.PERKS.ADVANCED_RESEARCH.DESCRIPTION));
      this.AllowInterstellarResearch = this.Add((SkillPerk) new SimpleSkillPerk("AllowInterStellarResearch", (string) UI.ROLES_SCREEN.PERKS.INTERSTELLAR_RESEARCH.DESCRIPTION));
      this.CanStudyWorldObjects = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanStudyWorldObjects), (string) UI.ROLES_SCREEN.PERKS.CAN_STUDY_WORLD_OBJECTS.DESCRIPTION));
      this.CanDoPlumbing = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanDoPlumbing), (string) UI.ROLES_SCREEN.PERKS.CAN_DO_PLUMBING.DESCRIPTION));
      this.CanUseRockets = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanUseRockets), (string) UI.ROLES_SCREEN.PERKS.CAN_USE_ROCKETS.DESCRIPTION));
      this.FasterSpaceFlight = this.Add((SkillPerk) new SkillAttributePerk(nameof (FasterSpaceFlight), Db.Get().Attributes.SpaceNavigation.Id, 0.1f, (string) DUPLICANTS.ROLES.ASTRONAUT.NAME));
      this.CanTrainToBeAstronaut = this.Add((SkillPerk) new SimpleSkillPerk(nameof (CanTrainToBeAstronaut), (string) UI.ROLES_SCREEN.PERKS.CAN_DO_ASTRONAUT_TRAINING.DESCRIPTION));
    }
  }
}