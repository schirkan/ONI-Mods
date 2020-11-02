// Decompiled with JetBrains decompiler
// Type: ProcGen.MutatedWorldData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using ObjectCloner;
using System.Collections.Generic;

namespace ProcGen
{
  public class MutatedWorldData
  {
    public World world;
    public List<WorldTrait> traits;
    public Dictionary<string, SubWorld> subworlds;
    public Dictionary<string, FeatureSettings> features;
    public TerrainElementBandSettings biomes;
    public MobSettings mobs;

    public MutatedWorldData(World world, List<WorldTrait> traits)
    {
      this.world = SerializingCloner.Copy<World>(world);
      this.traits = traits != null ? new List<WorldTrait>((IEnumerable<WorldTrait>) traits) : new List<WorldTrait>();
      SettingsCache.CloneInToNewWorld(this);
      this.ApplyTraits();
      foreach (ElementBandConfiguration bandConfiguration in this.biomes.BiomeBackgroundElementBandConfigurations.Values)
        bandConfiguration.ConvertBandSizeToMaxSize();
    }

    private void ApplyTraits()
    {
      foreach (WorldTrait trait in this.traits)
        this.ApplyTrait(trait);
    }

    private void ApplyTrait(WorldTrait trait)
    {
      this.world.ModStartLocation(trait.startingBasePositionHorizontalMod, trait.startingBasePositionVerticalMod);
      foreach (WeightedName additionalSubworldFile in trait.additionalSubworldFiles)
        this.world.subworldFiles.Add(additionalSubworldFile);
      foreach (World.AllowedCellsFilter unknownCellFilter in trait.additionalUnknownCellFilters)
        this.world.unknownCellsAllowedSubworlds.Add(unknownCellFilter);
      foreach (KeyValuePair<string, int> featureTemplateMod in trait.globalFeatureTemplateMods)
      {
        if (!this.world.globalFeatureTemplates.ContainsKey(featureTemplateMod.Key))
          this.world.globalFeatureTemplates[featureTemplateMod.Key] = 0;
        this.world.globalFeatureTemplates[featureTemplateMod.Key] += featureTemplateMod.Value;
      }
      foreach (KeyValuePair<string, int> globalFeatureMod in trait.globalFeatureMods)
      {
        if (!this.world.globalFeatures.ContainsKey(globalFeatureMod.Key))
          this.world.globalFeatures[globalFeatureMod.Key] = 0;
        this.world.globalFeatures[globalFeatureMod.Key] += globalFeatureMod.Value;
      }
      foreach (KeyValuePair<string, ElementBandConfiguration> bandConfiguration in this.biomes.BiomeBackgroundElementBandConfigurations)
      {
        foreach (ElementGradient elementGradient in (List<ElementGradient>) bandConfiguration.Value)
        {
          foreach (WorldTrait.ElementBandModifier elementBandModifier in trait.elementBandModifiers)
          {
            if (elementBandModifier.element == elementGradient.content)
              elementGradient.Mod(elementBandModifier);
          }
        }
      }
    }
  }
}
