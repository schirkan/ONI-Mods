// Decompiled with JetBrains decompiler
// Type: ProcGen.SettingsCache
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using ObjectCloner;
using ProcGen.Noise;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
  public static class SettingsCache
  {
    public static TerrainElementBandSettings biomes = new TerrainElementBandSettings();
    public static Worlds worlds = new Worlds();
    public static NoiseTreeFiles noise = new NoiseTreeFiles();
    private static Dictionary<string, FeatureSettings> featuresettings = new Dictionary<string, FeatureSettings>();
    private static Dictionary<string, WorldTrait> traits = new Dictionary<string, WorldTrait>();
    public static Dictionary<string, SubWorld> subworlds = new Dictionary<string, SubWorld>();
    private static string path = (string) null;
    private static Dictionary<string, BiomeSettings> biomeSettingsCache = new Dictionary<string, BiomeSettings>();
    private const string LAYERS_FILE = "layers";
    private const string RIVERS_FILE = "rivers";
    private const string ROOMS_FILE = "rooms";
    private const string TEMPERATURES_FILE = "temperatures";
    private const string BORDERS_FILE = "borders";
    private const string DEFAULTS_FILE = "defaults";
    private const string MOBS_FILE = "mobs";
    private const string TRAITS_PATH = "traits";

    public static LevelLayerSettings layers { get; private set; }

    public static ComposableDictionary<string, River> rivers { get; private set; }

    public static ComposableDictionary<string, Room> rooms { get; private set; }

    public static ComposableDictionary<Temperature.Range, Temperature> temperatures { get; private set; }

    public static ComposableDictionary<string, List<WeightedSimHash>> borders { get; private set; }

    public static DefaultSettings defaults { get; set; }

    public static MobSettings mobs { get; private set; }

    public static string GetPath()
    {
      if (SettingsCache.path == null)
        SettingsCache.path = FileSystem.Normalize(System.IO.Path.Combine(Application.streamingAssetsPath, "worldgen/"));
      return SettingsCache.path;
    }

    public static void CloneInToNewWorld(MutatedWorldData worldData)
    {
      worldData.subworlds = SerializingCloner.Copy<Dictionary<string, SubWorld>>(SettingsCache.subworlds);
      worldData.features = SerializingCloner.Copy<Dictionary<string, FeatureSettings>>(SettingsCache.featuresettings);
      worldData.biomes = SerializingCloner.Copy<TerrainElementBandSettings>(SettingsCache.biomes);
      worldData.mobs = SerializingCloner.Copy<MobSettings>(SettingsCache.mobs);
    }

    public static List<string> GetCachedFeatureNames()
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, FeatureSettings> featuresetting in SettingsCache.featuresettings)
        stringList.Add(featuresetting.Key);
      return stringList;
    }

    public static FeatureSettings GetCachedFeature(string name) => SettingsCache.featuresettings.ContainsKey(name) ? SettingsCache.featuresettings[name] : throw new Exception("Couldnt get feature from cache [" + name + "]");

    public static List<string> GetCachedTraitNames() => new List<string>((IEnumerable<string>) SettingsCache.traits.Keys);

    public static WorldTrait GetCachedTrait(string name, bool assertMissingTrait)
    {
      if (SettingsCache.traits.ContainsKey(name))
        return SettingsCache.traits[name];
      if (assertMissingTrait)
        throw new Exception("Couldnt get trait [" + name + "]");
      Debug.LogWarning((object) ("Couldnt get trait [" + name + "]"));
      return (WorldTrait) null;
    }

    public static SubWorld GetCachedSubWorld(string name) => SettingsCache.subworlds.ContainsKey(name) ? SettingsCache.subworlds[name] : throw new Exception("Couldnt get subworld [" + name + "]");

    private static bool GetPathAndName(string srcPath, string srcName, out string name)
    {
      if (FileSystem.FileExists(srcPath + srcName + ".yaml"))
      {
        name = srcName;
        return true;
      }
      string[] strArray = srcName.Split('/');
      name = strArray[0];
      for (int index = 1; index < strArray.Length - 1; ++index)
        name = name + "/" + strArray[index];
      if (FileSystem.FileExists(srcPath + name + ".yaml"))
        return true;
      name = srcName;
      return false;
    }

    private static void LoadBiome(string longName, List<YamlIO.Error> errors)
    {
      string name = "";
      if (!SettingsCache.GetPathAndName(SettingsCache.GetPath(), longName, out name) || SettingsCache.biomeSettingsCache.ContainsKey(name))
        return;
      BiomeSettings biomeSettings = SettingsCache.MergeLoad<BiomeSettings>(SettingsCache.GetPath() + name + ".yaml", errors);
      if (biomeSettings == null)
      {
        Debug.LogWarning((object) ("WorldGen: Attempting to load biome: " + name + " failed"));
      }
      else
      {
        Debug.Assert(biomeSettings.TerrainBiomeLookupTable.Count > 0, (object) longName);
        SettingsCache.biomeSettingsCache.Add(name, biomeSettings);
        foreach (KeyValuePair<string, ElementBandConfiguration> keyValuePair in biomeSettings.TerrainBiomeLookupTable)
        {
          string key = name + "/" + keyValuePair.Key;
          if (!SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(key))
            SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.Add(key, keyValuePair.Value);
        }
      }
    }

    private static string LoadFeature(string longName, List<YamlIO.Error> errors)
    {
      string name = "";
      if (!SettingsCache.GetPathAndName(SettingsCache.GetPath(), longName, out name))
      {
        Debug.LogWarning((object) ("LoadFeature GetPathAndName: Attempting to load feature: " + name + " failed"));
        return longName;
      }
      if (!SettingsCache.featuresettings.ContainsKey(name))
      {
        FeatureSettings featureSettings = YamlIO.LoadFile<FeatureSettings>(SettingsCache.GetPath() + name + ".yaml");
        if (featureSettings != null)
        {
          SettingsCache.featuresettings.Add(name, featureSettings);
          if (featureSettings.forceBiome != null)
          {
            SettingsCache.LoadBiome(featureSettings.forceBiome, errors);
            DebugUtil.Assert(SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(featureSettings.forceBiome), longName, "(feature) referenced a missing biome named", featureSettings.forceBiome);
          }
        }
        else
          Debug.LogWarning((object) ("WorldGen: Attempting to load feature: " + name + " failed"));
      }
      return name;
    }

    public static void LoadFeatures(Dictionary<string, int> features, List<YamlIO.Error> errors)
    {
      foreach (KeyValuePair<string, int> feature in features)
        SettingsCache.LoadFeature(feature.Key, errors);
    }

    public static void LoadSubworlds(List<WeightedName> subworlds, List<YamlIO.Error> errors)
    {
      foreach (WeightedName subworld in subworlds)
      {
        SubWorld subWorld1 = (SubWorld) null;
        string key = subworld.name;
        if (subworld.overrideName != null && subworld.overrideName.Length > 0)
          key = subworld.overrideName;
        if (!SettingsCache.subworlds.ContainsKey(key))
        {
          SubWorld subWorld2 = YamlIO.LoadFile<SubWorld>(SettingsCache.path + subworld.name + ".yaml");
          if (subWorld2 != null)
          {
            subWorld1 = subWorld2;
            subWorld1.name = key;
            SettingsCache.subworlds[key] = subWorld1;
            SettingsCache.noise.LoadTree(subWorld1.biomeNoise, SettingsCache.path);
            SettingsCache.noise.LoadTree(subWorld1.densityNoise, SettingsCache.path);
            SettingsCache.noise.LoadTree(subWorld1.overrideNoise, SettingsCache.path);
          }
          else
            Debug.LogWarning((object) ("WorldGen: Attempting to load subworld: " + subworld.name + " failed"));
          if (subWorld1.centralFeature != null)
            subWorld1.centralFeature.type = SettingsCache.LoadFeature(subWorld1.centralFeature.type, errors);
          foreach (WeightedBiome biome in subWorld1.biomes)
          {
            SettingsCache.LoadBiome(biome.name, errors);
            DebugUtil.Assert(SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.ContainsKey(biome.name), subWorld1.name, "(subworld) referenced a missing biome named", biome.name);
          }
          DebugUtil.Assert(subWorld1.features != null, "Features list for subworld", subWorld1.name, "was null! Either remove it from the .yaml or set it to the empty list []");
          foreach (Feature feature in subWorld1.features)
            feature.type = SettingsCache.LoadFeature(feature.type, errors);
        }
      }
    }

    public static void LoadWorldTraits(List<YamlIO.Error> errors)
    {
      List<FileHandle> fileHandleList = new List<FileHandle>();
      FileSystem.GetFiles(FileSystem.Normalize(System.IO.Path.Combine(SettingsCache.path, "traits")), "*.yaml", (ICollection<FileHandle>) fileHandleList);
      foreach (FileHandle file in fileHandleList)
        SettingsCache.LoadWorldTrait(file, errors);
    }

    public static void LoadWorldTrait(FileHandle file, List<YamlIO.Error> errors)
    {
      WorldTrait worldTrait = YamlIO.LoadFile<WorldTrait>(file, (YamlIO.ErrorHandler) ((error, force_log_as_warning) => errors.Add(error)));
      int startIndex = SettingsCache.FirstUncommonCharacter(SettingsCache.path, file.full_path);
      string path = startIndex > -1 ? file.full_path.Substring(startIndex) : file.full_path;
      string key = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetFileNameWithoutExtension(path)).Replace('\\', '/');
      if (worldTrait == null)
      {
        DebugUtil.LogWarningArgs((object) "Failed to load trait: ", (object) key);
      }
      else
      {
        SettingsCache.traits[key] = worldTrait;
        worldTrait.filePath = key;
      }
    }

    public static List<string> GetWorldNames() => SettingsCache.worlds.GetNames();

    public static void Save(string path)
    {
      YamlIO.Save<LevelLayerSettings>(SettingsCache.layers, path + "layers.yaml");
      YamlIO.Save<ComposableDictionary<string, River>>(SettingsCache.rivers, path + "rivers.yaml");
      YamlIO.Save<ComposableDictionary<string, Room>>(SettingsCache.rooms, path + "rooms.yaml");
      YamlIO.Save<ComposableDictionary<Temperature.Range, Temperature>>(SettingsCache.temperatures, path + "temperatures.yaml");
      YamlIO.Save<ComposableDictionary<string, List<WeightedSimHash>>>(SettingsCache.borders, path + "borders.yaml");
      YamlIO.Save<DefaultSettings>(SettingsCache.defaults, path + "defaults.yaml");
      YamlIO.Save<MobSettings>(SettingsCache.mobs, path + "mobs.yaml");
    }

    public static void Clear()
    {
      SettingsCache.worlds.worldCache.Clear();
      SettingsCache.layers = (LevelLayerSettings) null;
      SettingsCache.biomes.BiomeBackgroundElementBandConfigurations.Clear();
      SettingsCache.biomeSettingsCache.Clear();
      SettingsCache.rivers = (ComposableDictionary<string, River>) null;
      SettingsCache.rooms = (ComposableDictionary<string, Room>) null;
      SettingsCache.temperatures = (ComposableDictionary<Temperature.Range, Temperature>) null;
      SettingsCache.borders = (ComposableDictionary<string, List<WeightedSimHash>>) null;
      SettingsCache.noise.Clear();
      SettingsCache.defaults = (DefaultSettings) null;
      SettingsCache.mobs = (MobSettings) null;
      SettingsCache.featuresettings.Clear();
      SettingsCache.traits.Clear();
      SettingsCache.subworlds.Clear();
      DebugUtil.LogArgs((object) "World Settings cleared!");
    }

    private static T MergeLoad<T>(string filename, List<YamlIO.Error> errors) where T : class, IMerge<T>, new()
    {
      ListPool<FileHandle, WorldGenSettings>.PooledList pooledList1 = ListPool<FileHandle, WorldGenSettings>.Allocate();
      FileSystem.GetFiles(filename, (ICollection<FileHandle>) pooledList1);
      if (pooledList1.Count == 0)
      {
        pooledList1.Recycle();
        throw new Exception(string.Format("File not found in any file system: {0}", (object) filename));
      }
      pooledList1.Reverse();
      ListPool<T, WorldGenSettings>.PooledList pooledList2 = ListPool<T, WorldGenSettings>.Allocate();
      pooledList2.Add(new T());
      foreach (FileHandle filehandle in (List<FileHandle>) pooledList1)
      {
        T obj = YamlIO.LoadFile<T>(filehandle, (YamlIO.ErrorHandler) ((error, force_log_as_warning) => errors.Add(error)));
        if ((object) obj != null)
          pooledList2.Add(obj);
      }
      pooledList1.Recycle();
      T obj1 = pooledList2[0];
      for (int index = 1; index != pooledList2.Count; ++index)
        obj1.Merge(pooledList2[index]);
      pooledList2.Recycle();
      return obj1;
    }

    private static int FirstUncommonCharacter(string a, string b)
    {
      int num = Mathf.Min(a.Length, b.Length);
      int index = -1;
      do
        ;
      while (++index < num && (int) a[index] == (int) b[index]);
      return index;
    }

    public static bool LoadFiles(List<YamlIO.Error> errors)
    {
      if (SettingsCache.worlds.worldCache.Count > 0)
        return false;
      SettingsCache.worlds.LoadFiles(SettingsCache.GetPath(), errors);
      SettingsCache.LoadWorldTraits(errors);
      foreach (KeyValuePair<string, World> keyValuePair in SettingsCache.worlds.worldCache)
      {
        SettingsCache.LoadFeatures(keyValuePair.Value.globalFeatures, errors);
        SettingsCache.LoadSubworlds(keyValuePair.Value.subworldFiles, errors);
      }
      foreach (KeyValuePair<string, WorldTrait> trait in SettingsCache.traits)
      {
        SettingsCache.LoadFeatures(trait.Value.globalFeatureMods, errors);
        SettingsCache.LoadSubworlds(trait.Value.additionalSubworldFiles, errors);
      }
      SettingsCache.layers = SettingsCache.MergeLoad<LevelLayerSettings>(SettingsCache.GetPath() + "layers.yaml", errors);
      SettingsCache.layers.LevelLayers.ConvertBandSizeToMaxSize();
      SettingsCache.rivers = SettingsCache.MergeLoad<ComposableDictionary<string, River>>(SettingsCache.GetPath() + "rivers.yaml", errors);
      SettingsCache.rooms = SettingsCache.MergeLoad<ComposableDictionary<string, Room>>(SettingsCache.path + "rooms.yaml", errors);
      foreach (KeyValuePair<string, Room> room in SettingsCache.rooms)
        room.Value.name = room.Key;
      SettingsCache.temperatures = SettingsCache.MergeLoad<ComposableDictionary<Temperature.Range, Temperature>>(SettingsCache.GetPath() + "temperatures.yaml", errors);
      SettingsCache.borders = SettingsCache.MergeLoad<ComposableDictionary<string, List<WeightedSimHash>>>(SettingsCache.GetPath() + "borders.yaml", errors);
      SettingsCache.defaults = YamlIO.LoadFile<DefaultSettings>(SettingsCache.GetPath() + "defaults.yaml");
      SettingsCache.mobs = SettingsCache.MergeLoad<MobSettings>(SettingsCache.GetPath() + "mobs.yaml", errors);
      foreach (KeyValuePair<string, Mob> keyValuePair in SettingsCache.mobs.MobLookupTable)
        keyValuePair.Value.name = keyValuePair.Key;
      DebugUtil.LogArgs((object) "World settings reload complete!");
      return true;
    }

    public static List<string> GetRandomTraits(int seed)
    {
      System.Random random = new System.Random(seed);
      int num = random.Next(2, 5);
      List<string> stringList1 = new List<string>((IEnumerable<string>) SettingsCache.traits.Keys);
      stringList1.Sort();
      List<string> stringList2 = new List<string>();
      while (stringList2.Count < num && stringList1.Count > 0)
      {
        int index = random.Next(stringList1.Count);
        string name = stringList1[index];
        bool flag = false;
        foreach (string str in SettingsCache.GetCachedTrait(name, true).exclusiveWith)
        {
          if (stringList2.Contains(str))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          stringList2.Add(name);
        stringList1.RemoveAt(index);
      }
      return stringList2;
    }
  }
}
