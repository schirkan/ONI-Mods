﻿// Decompiled with JetBrains decompiler
// Type: ColonyDestinationAsteroidData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ColonyDestinationAsteroidData
{
  private ProcGen.World world;
  private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();
  private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();
  private static List<Tuple<string, string, string>> survivalOptions = new List<Tuple<string, string, string>>()
  {
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
    new Tuple<string, string, string>((string) WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
  };

  public float TargetScale { get; set; }

  public float Scale { get; set; }

  public int seed { get; private set; }

  public string worldPath => this.world.filePath;

  public string sprite { get; private set; }

  public int difficulty { get; private set; }

  public string properName => (string) Strings.Get(this.world.name);

  public ColonyDestinationAsteroidData(string worldName, int seed)
  {
    this.Scale = 1f;
    this.TargetScale = 1f;
    this.world = SettingsCache.worlds.GetWorldData(worldName);
    this.ReInitialize(seed);
  }

  public void ReInitialize(int seed)
  {
    this.seed = seed;
    this.paramDescriptors.Clear();
    this.traitDescriptors.Clear();
    this.sprite = this.world.spriteName;
    this.difficulty = this.world.difficulty;
  }

  public List<AsteroidDescriptor> GetParamDescriptors()
  {
    if (this.paramDescriptors.Count == 0)
      this.paramDescriptors = this.GenerateParamDescriptors();
    return this.paramDescriptors;
  }

  public List<AsteroidDescriptor> GetTraitDescriptors()
  {
    if (this.traitDescriptors.Count == 0)
      this.traitDescriptors = this.GenerateTraitDescriptors();
    return this.traitDescriptors;
  }

  private List<AsteroidDescriptor> GenerateParamDescriptors()
  {
    List<AsteroidDescriptor> asteroidDescriptorList = new List<AsteroidDescriptor>();
    asteroidDescriptorList.Add(new AsteroidDescriptor(string.Format((string) WORLDS.SURVIVAL_CHANCE.PLANETNAME, (object) this.properName), (string) null));
    asteroidDescriptorList.Add(new AsteroidDescriptor((string) Strings.Get(this.world.description), (string) null));
    int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidData.survivalOptions.Count - 1);
    Tuple<string, string, string> survivalOption = ColonyDestinationAsteroidData.survivalOptions[index];
    asteroidDescriptorList.Add(new AsteroidDescriptor(string.Format((string) WORLDS.SURVIVAL_CHANCE.TITLE, (object) survivalOption.first, (object) survivalOption.third), (string) null));
    return asteroidDescriptorList;
  }

  private List<AsteroidDescriptor> GenerateTraitDescriptors()
  {
    List<AsteroidDescriptor> asteroidDescriptorList = new List<AsteroidDescriptor>();
    if (this.world.disableWorldTraits)
    {
      asteroidDescriptorList.Add(new AsteroidDescriptor((string) WORLD_TRAITS.NO_TRAITS.NAME, (string) WORLD_TRAITS.NO_TRAITS.DESCRIPTION));
    }
    else
    {
      foreach (string randomTrait in SettingsCache.GetRandomTraits(this.seed))
      {
        WorldTrait cachedTrait = SettingsCache.GetCachedTrait(randomTrait, true);
        asteroidDescriptorList.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", (object) Strings.Get(cachedTrait.name), (object) cachedTrait.colorHex), (string) Strings.Get(cachedTrait.description)));
      }
    }
    return asteroidDescriptorList;
  }
}
