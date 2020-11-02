// Decompiled with JetBrains decompiler
// Type: SaveGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SaveGame")]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  private int speed;
  [Serialize]
  public List<Tag> expandedResourceTags = new List<Tag>();
  [Serialize]
  public int minGermCountForDisinfect = 10000;
  [Serialize]
  public bool enableAutoDisinfect = true;
  [Serialize]
  public bool sandboxEnabled;
  [Serialize]
  private int autoSaveCycleInterval = 1;
  [Serialize]
  private Vector2I timelapseResolution = new Vector2I(512, 768);
  private string baseName;
  public static SaveGame Instance;
  public EntombedItemManager entombedItemManager;
  public WorldGenSpawner worldGenSpawner;
  [MyCmpReq]
  public MaterialSelectorSerializer materialSelectorSerializer;
  public WorldGen worldGen;

  public int AutoSaveCycleInterval
  {
    get => this.autoSaveCycleInterval;
    set => this.autoSaveCycleInterval = value;
  }

  public Vector2I TimelapseResolution
  {
    get => this.timelapseResolution;
    set => this.timelapseResolution = value;
  }

  public string BaseName => this.baseName;

  public static void DestroyInstance() => SaveGame.Instance = (SaveGame) null;

  protected override void OnPrefabInit()
  {
    SaveGame.Instance = this;
    new ColonyRationMonitor.Instance((IStateMachineTarget) this).StartSM();
    new VignetteManager.Instance((IStateMachineTarget) this).StartSM();
    this.entombedItemManager = this.gameObject.AddComponent<EntombedItemManager>();
    this.worldGen = SaveLoader.Instance.worldGen;
    this.worldGenSpawner = this.gameObject.AddComponent<WorldGenSpawner>();
  }

  [OnSerializing]
  private void OnSerialize() => this.speed = SpeedControlScreen.Instance.GetSpeed();

  [OnDeserializing]
  private void OnDeserialize() => this.baseName = SaveLoader.Instance.GameInfo.baseName;

  public int GetSpeed() => this.speed;

  public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, isAutoSave, SaveLoader.GetActiveSaveFilePath(), SaveLoader.Instance.GameInfo.worldID, SaveLoader.Instance.GameInfo.worldTraits, SaveLoader.Instance.GameInfo.colonyGuid, this.sandboxEnabled)));
    header = new SaveGame.Header();
    header.buildVersion = 420700U;
    header.headerSize = bytes.Length;
    header.headerVersion = 1U;
    header.compression = isCompressed ? 1 : 0;
    return bytes;
  }

  public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header)
  {
    header = new SaveGame.Header();
    header.buildVersion = br.ReadUInt32();
    header.headerSize = br.ReadInt32();
    header.headerVersion = br.ReadUInt32();
    if (1U <= header.headerVersion)
      header.compression = br.ReadInt32();
    SaveGame.GameInfo gameInfo = SaveGame.GetGameInfo(br.ReadBytes(header.headerSize));
    if (gameInfo.IsVersionOlderThan(7, 14) && gameInfo.worldTraits != null)
    {
      string[] worldTraits = gameInfo.worldTraits;
      for (int index = 0; index < worldTraits.Length; ++index)
        worldTraits[index] = worldTraits[index].Replace('\\', '/');
    }
    return gameInfo;
  }

  public static SaveGame.GameInfo GetGameInfo(byte[] data) => JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));

  public void SetBaseName(string newBaseName)
  {
    if (string.IsNullOrEmpty(newBaseName))
      Debug.LogWarning((object) "Cannot give the base an empty name");
    else
      this.baseName = newBaseName;
  }

  protected override void OnSpawn()
  {
    ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
    Game.Instance.Trigger(-1917495436, (object) null);
  }

  public List<Tuple<string, ScriptableObject>> GetColonyToolTip()
  {
    List<Tuple<string, ScriptableObject>> tupleList = new List<Tuple<string, ScriptableObject>>();
    tupleList.Add(new Tuple<string, ScriptableObject>(this.baseName, (ScriptableObject) ToolTipScreen.Instance.defaultTooltipHeaderStyle));
    if ((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null)
    {
      tupleList.Add(new Tuple<string, ScriptableObject>(" ", (ScriptableObject) null));
      tupleList.Add(new Tuple<string, ScriptableObject>(string.Format((string) UI.ASTEROIDCLOCK.CYCLES_OLD, (object) GameUtil.GetCurrentCycle()), (ScriptableObject) ToolTipScreen.Instance.defaultTooltipHeaderStyle));
      tupleList.Add(new Tuple<string, ScriptableObject>(string.Format((string) UI.ASTEROIDCLOCK.TIME_PLAYED, (object) (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), (ScriptableObject) ToolTipScreen.Instance.defaultTooltipBodyStyle));
    }
    ProcGen.World worldData = SettingsCache.worlds.GetWorldData(SaveLoader.Instance.GameInfo.worldID);
    tupleList.Add(new Tuple<string, ScriptableObject>(" ", (ScriptableObject) null));
    tupleList.Add(new Tuple<string, ScriptableObject>((string) Strings.Get(worldData.name), (ScriptableObject) ToolTipScreen.Instance.defaultTooltipHeaderStyle));
    if (SaveLoader.Instance.GameInfo.worldTraits != null)
    {
      foreach (string worldTrait in SaveLoader.Instance.GameInfo.worldTraits)
      {
        WorldTrait cachedTrait = SettingsCache.GetCachedTrait(worldTrait, false);
        if (cachedTrait != null)
          tupleList.Add(new Tuple<string, ScriptableObject>((string) Strings.Get(cachedTrait.name), (ScriptableObject) ToolTipScreen.Instance.defaultTooltipBodyStyle));
        else
          tupleList.Add(new Tuple<string, ScriptableObject>((string) WORLD_TRAITS.MISSING_TRAIT, (ScriptableObject) ToolTipScreen.Instance.defaultTooltipBodyStyle));
      }
    }
    return tupleList;
  }

  public struct Header
  {
    public uint buildVersion;
    public int headerSize;
    public uint headerVersion;
    public int compression;

    public bool IsCompressed => (uint) this.compression > 0U;
  }

  public struct GameInfo
  {
    public int numberOfCycles;
    public int numberOfDuplicants;
    public string baseName;
    public bool isAutoSave;
    public string originalSaveName;
    public int saveMajorVersion;
    public int saveMinorVersion;
    public string worldID;
    public string[] worldTraits;
    public bool sandboxEnabled;
    public Guid colonyGuid;

    public GameInfo(
      int numberOfCycles,
      int numberOfDuplicants,
      string baseName,
      bool isAutoSave,
      string originalSaveName,
      string worldID,
      string[] worldTraits,
      Guid colonyGuid,
      bool sandboxEnabled = false)
    {
      this.numberOfCycles = numberOfCycles;
      this.numberOfDuplicants = numberOfDuplicants;
      this.baseName = baseName;
      this.isAutoSave = isAutoSave;
      this.originalSaveName = originalSaveName;
      this.worldID = worldID;
      this.worldTraits = worldTraits;
      this.colonyGuid = colonyGuid;
      this.sandboxEnabled = sandboxEnabled;
      this.saveMajorVersion = 7;
      this.saveMinorVersion = 17;
    }

    public bool IsVersionOlderThan(int major, int minor)
    {
      if (this.saveMajorVersion < major)
        return true;
      return this.saveMajorVersion == major && this.saveMinorVersion < minor;
    }

    public bool IsVersionExactly(int major, int minor) => this.saveMajorVersion == major && this.saveMinorVersion == minor;
  }
}
