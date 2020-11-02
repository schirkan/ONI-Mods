// Decompiled with JetBrains decompiler
// Type: ProcGen.WorldGenSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  public class WorldGenSettings
  {
    private MutatedWorldData mutatedWorldData;
    public const string defaultWorldName = "worlds/SandstoneDefault";

    public World world => this.mutatedWorldData.world;

    public WorldGenSettings(string worldName, List<string> traits, bool assertMissingTraits)
    {
      if (!SettingsCache.worlds.HasWorld(worldName))
      {
        DebugUtil.LogWarningArgs((object) string.Format("Failed to get worldGen data for {0}. Using {1} instead", (object) worldName, (object) "worlds/SandstoneDefault"));
        DebugUtil.Assert(SettingsCache.worlds.HasWorld("worlds/SandstoneDefault"));
        worldName = "worlds/SandstoneDefault";
      }
      World worldData = SettingsCache.worlds.GetWorldData(worldName);
      List<WorldTrait> traits1 = new List<WorldTrait>();
      if (!worldData.disableWorldTraits && traits != null)
      {
        DebugUtil.LogArgs((object) "Generating a world with the traits:", (object) string.Join(", ", traits.ToArray()));
        foreach (string trait in traits)
        {
          WorldTrait cachedTrait = SettingsCache.GetCachedTrait(trait, assertMissingTraits);
          if (cachedTrait != null)
            traits1.Add(cachedTrait);
        }
      }
      else
        Debug.Log((object) "Generating a world without traits. Either this world has traits disabled or none were specified.");
      this.mutatedWorldData = new MutatedWorldData(worldData, traits1);
      Debug.Log((object) ("Set world to [" + worldName + "] " + SettingsCache.GetPath()));
    }

    public BaseLocation GetBaseLocation()
    {
      if (this.world == null || this.world.defaultsOverrides == null || this.world.defaultsOverrides.baseData == null)
        return SettingsCache.defaults.baseData;
      DebugUtil.LogArgs((object) string.Format("World '{0}' is overriding baseData", (object) this.world.name));
      return this.world.defaultsOverrides.baseData;
    }

    public List<string> GetOverworldAddTags()
    {
      if (this.world == null || this.world.defaultsOverrides == null || this.world.defaultsOverrides.overworldAddTags == null)
        return SettingsCache.defaults.overworldAddTags;
      DebugUtil.LogArgs((object) string.Format("World '{0}' is overriding overworldAddTags", (object) this.world.name));
      return this.world.defaultsOverrides.overworldAddTags;
    }

    public List<string> GetDefaultMoveTags()
    {
      if (this.world == null || this.world.defaultsOverrides == null || this.world.defaultsOverrides.defaultMoveTags == null)
        return SettingsCache.defaults.defaultMoveTags;
      DebugUtil.LogArgs((object) string.Format("World '{0}' is overriding defaultMoveTags", (object) this.world.name));
      return this.world.defaultsOverrides.defaultMoveTags;
    }

    public string[] GetTraitIDs()
    {
      if (this.mutatedWorldData.traits == null || this.mutatedWorldData.traits.Count <= 0)
        return new string[0];
      string[] strArray = new string[this.mutatedWorldData.traits.Count];
      for (int index = 0; index < this.mutatedWorldData.traits.Count; ++index)
        strArray[index] = this.mutatedWorldData.traits[index].filePath;
      return strArray;
    }

    private bool GetSetting<T>(
      DefaultSettings set,
      string target,
      WorldGenSettings.ParserFn<T> parser,
      out T res)
    {
      if (set == null || set.data == null || !set.data.ContainsKey(target))
      {
        res = default (T);
        return false;
      }
      object obj = set.data[target];
      if (obj.GetType() == typeof (T))
      {
        res = (T) obj;
        return true;
      }
      int num = parser(obj as string, out res) ? 1 : 0;
      if (num == 0)
        return num != 0;
      set.data[target] = (object) res;
      return num != 0;
    }

    private T GetSetting<T>(string target, WorldGenSettings.ParserFn<T> parser)
    {
      T res;
      if (this.world != null)
      {
        if (!this.GetSetting<T>(this.world.defaultsOverrides, target, parser, out res))
          this.GetSetting<T>(SettingsCache.defaults, target, parser, out res);
        else
          DebugUtil.LogArgs((object) string.Format("World '{0}' is overriding setting '{1}'", (object) this.world.name, (object) target));
      }
      else if (!this.GetSetting<T>(SettingsCache.defaults, target, parser, out res))
        DebugUtil.LogWarningArgs((object) string.Format("Couldn't find setting '{0}' in default settings!", (object) target));
      return res;
    }

    public bool GetBoolSetting(string target) => this.GetSetting<bool>(target, new WorldGenSettings.ParserFn<bool>(bool.TryParse));

    private bool TryParseString(string input, out string res)
    {
      res = input;
      return true;
    }

    public string GetStringSetting(string target) => this.GetSetting<string>(target, new WorldGenSettings.ParserFn<string>(this.TryParseString));

    public float GetFloatSetting(string target) => this.GetSetting<float>(target, new WorldGenSettings.ParserFn<float>(float.TryParse));

    public int GetIntSetting(string target) => this.GetSetting<int>(target, new WorldGenSettings.ParserFn<int>(int.TryParse));

    public E GetEnumSetting<E>(string target) where E : struct => this.GetSetting<E>(target, new WorldGenSettings.ParserFn<E>(WorldGenSettings.TryParseEnum<E>));

    private static bool TryParseEnum<E>(string value, out E result) where E : struct
    {
      try
      {
        result = (E) Enum.Parse(typeof (E), value);
        return true;
      }
      catch (Exception ex)
      {
        result = new E();
      }
      return false;
    }

    public bool HasFeature(string name) => this.mutatedWorldData.features.ContainsKey(name);

    public FeatureSettings GetFeature(string name)
    {
      if (this.mutatedWorldData.features.ContainsKey(name))
        return this.mutatedWorldData.features[name];
      throw new Exception("Couldnt get feature from active world data [" + name + "]");
    }

    public FeatureSettings TryGetFeature(string name)
    {
      FeatureSettings featureSettings;
      this.mutatedWorldData.features.TryGetValue(name, out featureSettings);
      return featureSettings;
    }

    public bool HasSubworld(string name) => this.mutatedWorldData.subworlds.ContainsKey(name);

    public SubWorld GetSubWorld(string name)
    {
      if (this.mutatedWorldData.subworlds.ContainsKey(name))
        return this.mutatedWorldData.subworlds[name];
      throw new Exception("Couldnt get subworld from active world data [" + name + "]");
    }

    public SubWorld TryGetSubWorld(string name)
    {
      SubWorld subWorld;
      this.mutatedWorldData.subworlds.TryGetValue(name, out subWorld);
      return subWorld;
    }

    public List<WeightedSubWorld> GetSubworldsForWorld(
      List<WeightedName> subworldList)
    {
      List<WeightedSubWorld> weightedSubWorldList = new List<WeightedSubWorld>();
      foreach (KeyValuePair<string, SubWorld> subworld1 in this.mutatedWorldData.subworlds)
      {
        foreach (WeightedName subworld2 in subworldList)
        {
          if (subworld1.Key == subworld2.name)
            weightedSubWorldList.Add(new WeightedSubWorld(subworld2.weight, subworld1.Value));
        }
      }
      return weightedSubWorldList;
    }

    public bool HasMob(string id) => this.mutatedWorldData.mobs.HasMob(id);

    public Mob GetMob(string id) => this.mutatedWorldData.mobs.GetMob(id);

    public ElementBandConfiguration GetElementBandForBiome(string name)
    {
      ElementBandConfiguration bandConfiguration;
      return this.mutatedWorldData.biomes.BiomeBackgroundElementBandConfigurations.TryGetValue(name, out bandConfiguration) ? bandConfiguration : (ElementBandConfiguration) null;
    }

    private delegate bool ParserFn<T>(string input, out T res);
  }
}
