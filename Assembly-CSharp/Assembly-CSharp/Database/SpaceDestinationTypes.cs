﻿// Decompiled with JetBrains decompiler
// Type: Database.SpaceDestinationTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Database
{
  public class SpaceDestinationTypes : ResourceSet<SpaceDestinationType>
  {
    public SpaceDestinationType Satellite;
    public SpaceDestinationType MetallicAsteroid;
    public SpaceDestinationType RockyAsteroid;
    public SpaceDestinationType CarbonaceousAsteroid;
    public SpaceDestinationType IcyDwarf;
    public SpaceDestinationType OrganicDwarf;
    public SpaceDestinationType DustyMoon;
    public SpaceDestinationType TerraPlanet;
    public SpaceDestinationType VolcanoPlanet;
    public SpaceDestinationType GasGiant;
    public SpaceDestinationType IceGiant;
    public SpaceDestinationType Wormhole;
    public SpaceDestinationType SaltDwarf;
    public SpaceDestinationType RustPlanet;
    public SpaceDestinationType ForestPlanet;
    public SpaceDestinationType RedDwarf;
    public SpaceDestinationType GoldAsteroid;
    public SpaceDestinationType HydrogenGiant;
    public SpaceDestinationType OilyAsteroid;
    public SpaceDestinationType ShinyPlanet;
    public SpaceDestinationType ChlorinePlanet;
    public SpaceDestinationType SaltDesertPlanet;
    public SpaceDestinationType Earth;
    public static Dictionary<SimHashes, MathUtil.MinMax> extendedElementTable = new Dictionary<SimHashes, MathUtil.MinMax>()
    {
      {
        SimHashes.Niobium,
        new MathUtil.MinMax(10f, 20f)
      },
      {
        SimHashes.Katairite,
        new MathUtil.MinMax(50f, 100f)
      },
      {
        SimHashes.Isoresin,
        new MathUtil.MinMax(30f, 60f)
      },
      {
        SimHashes.Fullerene,
        new MathUtil.MinMax(0.5f, 1f)
      }
    };

    public SpaceDestinationTypes(ResourceSet parent)
      : base("SpaceDestinations", parent)
    {
      ResourceSet parent1 = parent;
      string name1 = (string) UI.SPACEDESTINATIONS.DEBRIS.SATELLITE.NAME;
      string description1 = (string) UI.SPACEDESTINATIONS.DEBRIS.SATELLITE.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable1 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable1.Add(SimHashes.Steel, new MathUtil.MinMax(100f, 200f));
      elementTable1.Add(SimHashes.Copper, new MathUtil.MinMax(100f, 200f));
      elementTable1.Add(SimHashes.Glass, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate bad1 = Db.Get().ArtifactDropRates.Bad;
      this.Satellite = this.Add(new SpaceDestinationType(nameof (Satellite), parent1, name1, description1, 16, "asteroid", elementTable1, artifactDropRate: bad1, cycles: 18));
      ResourceSet parent2 = parent;
      string name2 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.METALLICASTEROID.NAME;
      string description2 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.METALLICASTEROID.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable2 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable2.Add(SimHashes.Iron, new MathUtil.MinMax(100f, 200f));
      elementTable2.Add(SimHashes.Copper, new MathUtil.MinMax(100f, 200f));
      elementTable2.Add(SimHashes.Obsidian, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities1 = new Dictionary<string, int>();
      recoverableEntities1.Add("HatchMetal", 3);
      ArtifactDropRate mediocre1 = Db.Get().ArtifactDropRates.Mediocre;
      this.MetallicAsteroid = this.Add(new SpaceDestinationType(nameof (MetallicAsteroid), parent2, name2, description2, 32, "nebula", elementTable2, recoverableEntities1, mediocre1, 128000000, 127988000, 12));
      ResourceSet parent3 = parent;
      string name3 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.ROCKYASTEROID.NAME;
      string description3 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.ROCKYASTEROID.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable3 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable3.Add(SimHashes.Cuprite, new MathUtil.MinMax(100f, 200f));
      elementTable3.Add(SimHashes.SedimentaryRock, new MathUtil.MinMax(100f, 200f));
      elementTable3.Add(SimHashes.IgneousRock, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities2 = new Dictionary<string, int>();
      recoverableEntities2.Add("HatchHard", 3);
      ArtifactDropRate good1 = Db.Get().ArtifactDropRates.Good;
      this.RockyAsteroid = this.Add(new SpaceDestinationType(nameof (RockyAsteroid), parent3, name3, description3, 32, "new_12", elementTable3, recoverableEntities2, good1, 128000000, 127988000, 18));
      ResourceSet parent4 = parent;
      string name4 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.CARBONACEOUSASTEROID.NAME;
      string description4 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.CARBONACEOUSASTEROID.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable4 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable4.Add(SimHashes.RefinedCarbon, new MathUtil.MinMax(100f, 200f));
      elementTable4.Add(SimHashes.Carbon, new MathUtil.MinMax(100f, 200f));
      elementTable4.Add(SimHashes.Diamond, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate mediocre2 = Db.Get().ArtifactDropRates.Mediocre;
      this.CarbonaceousAsteroid = this.Add(new SpaceDestinationType(nameof (CarbonaceousAsteroid), parent4, name4, description4, 32, "new_08", elementTable4, artifactDropRate: mediocre2, max: 128000000, min: 127988000));
      ResourceSet parent5 = parent;
      string name5 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ICYDWARF.NAME;
      string description5 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ICYDWARF.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable5 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable5.Add(SimHashes.Ice, new MathUtil.MinMax(100f, 200f));
      elementTable5.Add(SimHashes.SolidCarbonDioxide, new MathUtil.MinMax(100f, 200f));
      elementTable5.Add(SimHashes.SolidOxygen, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities3 = new Dictionary<string, int>();
      recoverableEntities3.Add("ColdBreatherSeed", 3);
      recoverableEntities3.Add("ColdWheatSeed", 4);
      ArtifactDropRate great1 = Db.Get().ArtifactDropRates.Great;
      this.IcyDwarf = this.Add(new SpaceDestinationType(nameof (IcyDwarf), parent5, name5, description5, 64, "icyMoon", elementTable5, recoverableEntities3, great1, 256000000, 255982000, 24));
      ResourceSet parent6 = parent;
      string name6 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ORGANICDWARF.NAME;
      string description6 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.ORGANICDWARF.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable6 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable6.Add(SimHashes.SlimeMold, new MathUtil.MinMax(100f, 200f));
      elementTable6.Add(SimHashes.Algae, new MathUtil.MinMax(100f, 200f));
      elementTable6.Add(SimHashes.ContaminatedOxygen, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities4 = new Dictionary<string, int>();
      recoverableEntities4.Add("Moo", 1);
      recoverableEntities4.Add("GasGrassSeed", 4);
      ArtifactDropRate great2 = Db.Get().ArtifactDropRates.Great;
      this.OrganicDwarf = this.Add(new SpaceDestinationType(nameof (OrganicDwarf), parent6, name6, description6, 64, "organicAsteroid", elementTable6, recoverableEntities4, great2, 256000000, 255982000, 30));
      ResourceSet parent7 = parent;
      string name7 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.DUSTYDWARF.NAME;
      string description7 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.DUSTYDWARF.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable7 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable7.Add(SimHashes.Regolith, new MathUtil.MinMax(100f, 200f));
      elementTable7.Add(SimHashes.MaficRock, new MathUtil.MinMax(100f, 200f));
      elementTable7.Add(SimHashes.SedimentaryRock, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate amazing1 = Db.Get().ArtifactDropRates.Amazing;
      this.DustyMoon = this.Add(new SpaceDestinationType(nameof (DustyMoon), parent7, name7, description7, 64, "new_05", elementTable7, artifactDropRate: amazing1, max: 256000000, min: 255982000, cycles: 42));
      ResourceSet parent8 = parent;
      string name8 = (string) UI.SPACEDESTINATIONS.PLANETS.TERRAPLANET.NAME;
      string description8 = (string) UI.SPACEDESTINATIONS.PLANETS.TERRAPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable8 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable8.Add(SimHashes.Water, new MathUtil.MinMax(100f, 200f));
      elementTable8.Add(SimHashes.Algae, new MathUtil.MinMax(100f, 200f));
      elementTable8.Add(SimHashes.Oxygen, new MathUtil.MinMax(100f, 200f));
      elementTable8.Add(SimHashes.Dirt, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities5 = new Dictionary<string, int>();
      recoverableEntities5.Add("PrickleFlowerSeed", 4);
      recoverableEntities5.Add("PacuEgg", 4);
      ArtifactDropRate amazing2 = Db.Get().ArtifactDropRates.Amazing;
      this.TerraPlanet = this.Add(new SpaceDestinationType(nameof (TerraPlanet), parent8, name8, description8, 96, "terra", elementTable8, recoverableEntities5, amazing2, 384000000, 383980000, 54));
      ResourceSet parent9 = parent;
      string name9 = (string) UI.SPACEDESTINATIONS.PLANETS.VOLCANOPLANET.NAME;
      string description9 = (string) UI.SPACEDESTINATIONS.PLANETS.VOLCANOPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable9 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable9.Add(SimHashes.Magma, new MathUtil.MinMax(100f, 200f));
      elementTable9.Add(SimHashes.IgneousRock, new MathUtil.MinMax(100f, 200f));
      elementTable9.Add(SimHashes.Katairite, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate amazing3 = Db.Get().ArtifactDropRates.Amazing;
      this.VolcanoPlanet = this.Add(new SpaceDestinationType(nameof (VolcanoPlanet), parent9, name9, description9, 96, "planet", elementTable9, artifactDropRate: amazing3, max: 384000000, min: 383980000, cycles: 54));
      ResourceSet parent10 = parent;
      string name10 = (string) UI.SPACEDESTINATIONS.GIANTS.GASGIANT.NAME;
      string description10 = (string) UI.SPACEDESTINATIONS.GIANTS.GASGIANT.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable10 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable10.Add(SimHashes.Methane, new MathUtil.MinMax(100f, 200f));
      elementTable10.Add(SimHashes.Hydrogen, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate perfect1 = Db.Get().ArtifactDropRates.Perfect;
      this.GasGiant = this.Add(new SpaceDestinationType(nameof (GasGiant), parent10, name10, description10, 96, "gasGiant", elementTable10, artifactDropRate: perfect1, max: 384000000, min: 383980000, cycles: 60));
      ResourceSet parent11 = parent;
      string name11 = (string) UI.SPACEDESTINATIONS.GIANTS.ICEGIANT.NAME;
      string description11 = (string) UI.SPACEDESTINATIONS.GIANTS.ICEGIANT.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable11 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable11.Add(SimHashes.Ice, new MathUtil.MinMax(100f, 200f));
      elementTable11.Add(SimHashes.SolidCarbonDioxide, new MathUtil.MinMax(100f, 200f));
      elementTable11.Add(SimHashes.SolidOxygen, new MathUtil.MinMax(100f, 200f));
      elementTable11.Add(SimHashes.SolidMethane, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate perfect2 = Db.Get().ArtifactDropRates.Perfect;
      this.IceGiant = this.Add(new SpaceDestinationType(nameof (IceGiant), parent11, name11, description11, 96, "icyMoon", elementTable11, artifactDropRate: perfect2, max: 384000000, min: 383980000, cycles: 60));
      ResourceSet parent12 = parent;
      string name12 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.SALTDWARF.NAME;
      string description12 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.SALTDWARF.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable12 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable12.Add(SimHashes.SaltWater, new MathUtil.MinMax(100f, 200f));
      elementTable12.Add(SimHashes.SolidCarbonDioxide, new MathUtil.MinMax(100f, 200f));
      elementTable12.Add(SimHashes.Brine, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities6 = new Dictionary<string, int>();
      recoverableEntities6.Add("SaltPlantSeed", 3);
      ArtifactDropRate bad2 = Db.Get().ArtifactDropRates.Bad;
      this.SaltDwarf = this.Add(new SpaceDestinationType(nameof (SaltDwarf), parent12, name12, description12, 64, "new_01", elementTable12, recoverableEntities6, bad2, 256000000, 255982000, 30));
      ResourceSet parent13 = parent;
      string name13 = (string) UI.SPACEDESTINATIONS.PLANETS.RUSTPLANET.NAME;
      string description13 = (string) UI.SPACEDESTINATIONS.PLANETS.RUSTPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable13 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable13.Add(SimHashes.Rust, new MathUtil.MinMax(100f, 200f));
      elementTable13.Add(SimHashes.SolidCarbonDioxide, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate perfect3 = Db.Get().ArtifactDropRates.Perfect;
      this.RustPlanet = this.Add(new SpaceDestinationType(nameof (RustPlanet), parent13, name13, description13, 96, "new_06", elementTable13, artifactDropRate: perfect3, max: 384000000, min: 383980000, cycles: 60));
      ResourceSet parent14 = parent;
      string name14 = (string) UI.SPACEDESTINATIONS.PLANETS.FORESTPLANET.NAME;
      string description14 = (string) UI.SPACEDESTINATIONS.PLANETS.FORESTPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable14 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable14.Add(SimHashes.AluminumOre, new MathUtil.MinMax(100f, 200f));
      elementTable14.Add(SimHashes.SolidOxygen, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities7 = new Dictionary<string, int>();
      recoverableEntities7.Add("Squirrel", 1);
      recoverableEntities7.Add("ForestTreeSeed", 4);
      ArtifactDropRate mediocre3 = Db.Get().ArtifactDropRates.Mediocre;
      this.ForestPlanet = this.Add(new SpaceDestinationType(nameof (ForestPlanet), parent14, name14, description14, 96, "new_07", elementTable14, recoverableEntities7, mediocre3, 384000000, 383980000, 24));
      ResourceSet parent15 = parent;
      string name15 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.REDDWARF.NAME;
      string description15 = (string) UI.SPACEDESTINATIONS.DWARFPLANETS.REDDWARF.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable15 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable15.Add(SimHashes.Aluminum, new MathUtil.MinMax(100f, 200f));
      elementTable15.Add(SimHashes.LiquidMethane, new MathUtil.MinMax(100f, 200f));
      elementTable15.Add(SimHashes.Fossil, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate amazing4 = Db.Get().ArtifactDropRates.Amazing;
      this.RedDwarf = this.Add(new SpaceDestinationType(nameof (RedDwarf), parent15, name15, description15, 64, "sun", elementTable15, artifactDropRate: amazing4, max: 256000000, min: 255982000, cycles: 42));
      ResourceSet parent16 = parent;
      string name16 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.GOLDASTEROID.NAME;
      string description16 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.GOLDASTEROID.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable16 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable16.Add(SimHashes.Gold, new MathUtil.MinMax(100f, 200f));
      elementTable16.Add(SimHashes.Fullerene, new MathUtil.MinMax(100f, 200f));
      elementTable16.Add(SimHashes.FoolsGold, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate bad3 = Db.Get().ArtifactDropRates.Bad;
      this.GoldAsteroid = this.Add(new SpaceDestinationType(nameof (GoldAsteroid), parent16, name16, description16, 32, "new_02", elementTable16, artifactDropRate: bad3, max: 128000000, min: 127988000, cycles: 90));
      ResourceSet parent17 = parent;
      string name17 = (string) UI.SPACEDESTINATIONS.GIANTS.HYDROGENGIANT.NAME;
      string description17 = (string) UI.SPACEDESTINATIONS.GIANTS.HYDROGENGIANT.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable17 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable17.Add(SimHashes.LiquidHydrogen, new MathUtil.MinMax(100f, 200f));
      elementTable17.Add(SimHashes.Water, new MathUtil.MinMax(100f, 200f));
      elementTable17.Add(SimHashes.Niobium, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate mediocre4 = Db.Get().ArtifactDropRates.Mediocre;
      this.HydrogenGiant = this.Add(new SpaceDestinationType("HeliumGiant", parent17, name17, description17, 96, "new_11", elementTable17, artifactDropRate: mediocre4, max: 384000000, min: 383980000, cycles: 78));
      ResourceSet parent18 = parent;
      string name18 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.OILYASTEROID.NAME;
      string description18 = (string) UI.SPACEDESTINATIONS.ASTEROIDS.OILYASTEROID.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable18 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable18.Add(SimHashes.SolidMethane, new MathUtil.MinMax(100f, 200f));
      elementTable18.Add(SimHashes.SolidCarbonDioxide, new MathUtil.MinMax(100f, 200f));
      elementTable18.Add(SimHashes.CrudeOil, new MathUtil.MinMax(100f, 200f));
      elementTable18.Add(SimHashes.Petroleum, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate mediocre5 = Db.Get().ArtifactDropRates.Mediocre;
      this.OilyAsteroid = this.Add(new SpaceDestinationType("OilyAsteriod", parent18, name18, description18, 32, "new_09", elementTable18, artifactDropRate: mediocre5, max: 128000000, min: 127988000, cycles: 12));
      ResourceSet parent19 = parent;
      string name19 = (string) UI.SPACEDESTINATIONS.PLANETS.SHINYPLANET.NAME;
      string description19 = (string) UI.SPACEDESTINATIONS.PLANETS.SHINYPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable19 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable19.Add(SimHashes.Tungsten, new MathUtil.MinMax(100f, 200f));
      elementTable19.Add(SimHashes.Wolframite, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate good2 = Db.Get().ArtifactDropRates.Good;
      this.ShinyPlanet = this.Add(new SpaceDestinationType(nameof (ShinyPlanet), parent19, name19, description19, 96, "new_04", elementTable19, artifactDropRate: good2, max: 384000000, min: 383980000, cycles: 84));
      ResourceSet parent20 = parent;
      string name20 = (string) UI.SPACEDESTINATIONS.PLANETS.CHLORINEPLANET.NAME;
      string description20 = (string) UI.SPACEDESTINATIONS.PLANETS.CHLORINEPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable20 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable20.Add(SimHashes.SolidChlorine, new MathUtil.MinMax(100f, 200f));
      elementTable20.Add(SimHashes.BleachStone, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate bad4 = Db.Get().ArtifactDropRates.Bad;
      this.ChlorinePlanet = this.Add(new SpaceDestinationType(nameof (ChlorinePlanet), parent20, name20, description20, 96, "new_10", elementTable20, artifactDropRate: bad4, max: 256000000, min: 255982000, cycles: 90));
      ResourceSet parent21 = parent;
      string name21 = (string) UI.SPACEDESTINATIONS.PLANETS.SALTDESERTPLANET.NAME;
      string description21 = (string) UI.SPACEDESTINATIONS.PLANETS.SALTDESERTPLANET.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable21 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable21.Add(SimHashes.Salt, new MathUtil.MinMax(100f, 200f));
      elementTable21.Add(SimHashes.CrushedRock, new MathUtil.MinMax(100f, 200f));
      Dictionary<string, int> recoverableEntities8 = new Dictionary<string, int>();
      recoverableEntities8.Add("Crab", 1);
      ArtifactDropRate bad5 = Db.Get().ArtifactDropRates.Bad;
      this.SaltDesertPlanet = this.Add(new SpaceDestinationType(nameof (SaltDesertPlanet), parent21, name21, description21, 96, "new_10", elementTable21, recoverableEntities8, bad5, 384000000, 383980000, 60));
      ResourceSet parent22 = parent;
      string name22 = (string) UI.SPACEDESTINATIONS.WORMHOLE.NAME;
      string description22 = (string) UI.SPACEDESTINATIONS.WORMHOLE.DESCRIPTION;
      Dictionary<SimHashes, MathUtil.MinMax> elementTable22 = new Dictionary<SimHashes, MathUtil.MinMax>();
      elementTable22.Add(SimHashes.Vacuum, new MathUtil.MinMax(100f, 200f));
      ArtifactDropRate perfect4 = Db.Get().ArtifactDropRates.Perfect;
      this.Wormhole = this.Add(new SpaceDestinationType(nameof (Wormhole), parent22, name22, description22, 96, "new_03", elementTable22, artifactDropRate: perfect4, max: 0, min: 0, cycles: 0));
      this.Earth = this.Add(new SpaceDestinationType(nameof (Earth), parent, (string) UI.SPACEDESTINATIONS.PLANETS.SHATTEREDPLANET.NAME, (string) UI.SPACEDESTINATIONS.PLANETS.SHATTEREDPLANET.DESCRIPTION, 96, "earth", new Dictionary<SimHashes, MathUtil.MinMax>(), artifactDropRate: Db.Get().ArtifactDropRates.None, max: 0, min: 0, cycles: 0, visitable: false));
    }
  }
}