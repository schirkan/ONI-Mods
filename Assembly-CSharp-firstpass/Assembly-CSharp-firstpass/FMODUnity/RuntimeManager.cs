// Decompiled with JetBrains decompiler
// Type: FMODUnity.RuntimeManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD;
using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FMODUnity
{
  [AddComponentMenu("")]
  public class RuntimeManager : MonoBehaviour
  {
    private static SystemNotInitializedException initException = (SystemNotInitializedException) null;
    private static RuntimeManager instance;
    private static bool isQuitting = false;
    [SerializeField]
    private FMODPlatform fmodPlatform;
    private FMOD.Studio.System studioSystem;
    private FMOD.System lowlevelSystem;
    private DSP mixerHead;
    [SerializeField]
    private long[] cachedPointers = new long[2];
    private Dictionary<string, RuntimeManager.LoadedBank> loadedBanks = new Dictionary<string, RuntimeManager.LoadedBank>();
    private Dictionary<string, uint> loadedPlugins = new Dictionary<string, uint>();
    private Dictionary<Guid, EventDescription> cachedDescriptions = new Dictionary<Guid, EventDescription>((IEqualityComparer<Guid>) new RuntimeManager.GuidComparer());
    private List<RuntimeManager.AttachedInstance> attachedInstances = new List<RuntimeManager.AttachedInstance>(128);
    private bool listenerWarningIssued;
    private string lastDebugText;
    private float lastDebugUpdate;
    public static bool[] HasListener = new bool[8];

    public bool initializedSuccessfully { get; private set; }

    private static RuntimeManager Instance
    {
      get
      {
        if (RuntimeManager.initException != null)
          throw RuntimeManager.initException;
        if (RuntimeManager.isQuitting)
          throw new Exception("FMOD Studio attempted access by script to RuntimeManager while application is quitting");
        if ((UnityEngine.Object) RuntimeManager.instance == (UnityEngine.Object) null)
        {
          RuntimeManager objectOfType = UnityEngine.Object.FindObjectOfType(typeof (RuntimeManager)) as RuntimeManager;
          if ((UnityEngine.Object) objectOfType != (UnityEngine.Object) null && objectOfType.cachedPointers[0] != 0L)
          {
            RuntimeManager.instance = objectOfType;
            RuntimeManager.instance.studioSystem.handle = (IntPtr) RuntimeManager.instance.cachedPointers[0];
            RuntimeManager.instance.lowlevelSystem.handle = (IntPtr) RuntimeManager.instance.cachedPointers[1];
            return RuntimeManager.instance;
          }
          GameObject gameObject = new GameObject("FMOD.UnityIntegration.RuntimeManager");
          RuntimeManager.instance = gameObject.AddComponent<RuntimeManager>();
          UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) gameObject);
          gameObject.hideFlags = HideFlags.HideInHierarchy;
          RESULT result;
          try
          {
            RuntimeUtils.EnforceLibraryOrder();
            result = RuntimeManager.instance.Initialize();
          }
          catch (Exception ex)
          {
            RuntimeManager.initException = ex as SystemNotInitializedException;
            RuntimeManager.initException = RuntimeManager.initException == null ? new SystemNotInitializedException(ex) : throw RuntimeManager.initException;
          }
          if (result != RESULT.OK)
            throw new SystemNotInitializedException(result, "Output forced to NO SOUND mode");
        }
        return RuntimeManager.instance;
      }
    }

    public static FMOD.Studio.System StudioSystem => RuntimeManager.Instance.studioSystem;

    public static FMOD.System LowlevelSystem => RuntimeManager.Instance.lowlevelSystem;

    private void CheckInitResult(RESULT result, string cause)
    {
      if (result != RESULT.OK)
      {
        int num = this.studioSystem.isValid() ? (int) this.studioSystem.release() : throw new SystemNotInitializedException(result, cause);
        this.studioSystem.clearHandle();
      }
    }

    private RESULT Initialize()
    {
      this.initializedSuccessfully = false;
      RESULT result1 = RESULT.OK;
      Settings instance = Settings.Instance;
      this.fmodPlatform = RuntimeUtils.GetCurrentPlatform();
      int sampleRate = instance.GetSampleRate(this.fmodPlatform);
      int numsoftwarechannels = Math.Min(instance.GetRealChannels(this.fmodPlatform), 256);
      int virtualChannels = instance.GetVirtualChannels(this.fmodPlatform);
      SPEAKERMODE speakerMode = (SPEAKERMODE) instance.GetSpeakerMode(this.fmodPlatform);
      OUTPUTTYPE output = OUTPUTTYPE.AUTODETECT;
      FMOD.ADVANCEDSETTINGS settings = new FMOD.ADVANCEDSETTINGS();
      settings.randomSeed = (uint) DateTime.Now.Ticks;
      settings.maxVorbisCodecs = numsoftwarechannels;
      FMOD.Studio.INITFLAGS studioFlags = FMOD.Studio.INITFLAGS.DEFERRED_CALLBACKS;
      if (instance.IsLiveUpdateEnabled(this.fmodPlatform))
        studioFlags |= FMOD.Studio.INITFLAGS.LIVEUPDATE;
      RESULT result2;
      while (true)
      {
        this.CheckInitResult(FMOD.Studio.System.create(out this.studioSystem), "FMOD.Studio.System.create");
        this.CheckInitResult(this.studioSystem.getLowLevelSystem(out this.lowlevelSystem), "FMOD.Studio.System.getLowLevelSystem");
        this.CheckInitResult(this.lowlevelSystem.setOutput(output), "FMOD.System.setOutput");
        this.CheckInitResult(this.lowlevelSystem.setSoftwareChannels(numsoftwarechannels), "FMOD.System.setSoftwareChannels");
        this.CheckInitResult(this.lowlevelSystem.setSoftwareFormat(sampleRate, speakerMode, 0), "FMOD.System.setSoftwareFormat");
        this.CheckInitResult(this.lowlevelSystem.setAdvancedSettings(ref settings), "FMOD.System.setAdvancedSettings");
        result2 = this.studioSystem.initialize(virtualChannels, studioFlags, FMOD.INITFLAGS.NORMAL, IntPtr.Zero);
        if (result2 != RESULT.OK && result1 == RESULT.OK)
        {
          result1 = result2;
          output = OUTPUTTYPE.NOSOUND;
          Debug.LogWarningFormat("FMOD Studio: Studio::System::initialize returned {0}, defaulting to no-sound mode.", (object) result2.ToString());
        }
        else
        {
          this.CheckInitResult(result2, "Studio::System::initialize");
          if ((studioFlags & FMOD.Studio.INITFLAGS.LIVEUPDATE) != FMOD.Studio.INITFLAGS.NORMAL)
          {
            int num = (int) this.studioSystem.flushCommands();
            result2 = this.studioSystem.update();
            if (result2 == RESULT.ERR_NET_SOCKET_ERROR)
            {
              studioFlags &= ~FMOD.Studio.INITFLAGS.LIVEUPDATE;
              Debug.LogWarning((object) "FMOD Studio: Cannot open network port for Live Update (in-use), restarting with Live Update disabled.");
              this.CheckInitResult(this.studioSystem.release(), "FMOD.Studio.System.Release");
            }
            else
              break;
          }
          else
            break;
        }
      }
      this.LoadPlugins(instance);
      this.LoadBanks(instance);
      this.initializedSuccessfully = result2 == RESULT.OK;
      return result1;
    }

    private void Update()
    {
      if (!this.studioSystem.isValid() || !RuntimeManager.IsInitialized)
        return;
      int num1 = (int) this.studioSystem.update();
      bool flag1 = false;
      bool flag2 = false;
      int numlisteners = 0;
      for (int index = 7; index >= 0; --index)
      {
        if (!flag1 && RuntimeManager.HasListener[index])
        {
          numlisteners = index + 1;
          flag1 = true;
          flag2 = true;
        }
        if (!RuntimeManager.HasListener[index] & flag1)
          flag2 = false;
      }
      if (flag1)
      {
        int num2 = (int) this.studioSystem.setNumListeners(numlisteners);
      }
      if (!flag2 && !this.listenerWarningIssued)
        this.listenerWarningIssued = true;
      for (int index = 0; index < this.attachedInstances.Count; ++index)
      {
        PLAYBACK_STATE state = PLAYBACK_STATE.STOPPED;
        int playbackState = (int) this.attachedInstances[index].instance.getPlaybackState(out state);
        if (!this.attachedInstances[index].instance.isValid() || state == PLAYBACK_STATE.STOPPED || (UnityEngine.Object) this.attachedInstances[index].transform == (UnityEngine.Object) null)
        {
          this.attachedInstances.RemoveAt(index);
          --index;
        }
        else if ((bool) (UnityEngine.Object) this.attachedInstances[index].rigidBody)
        {
          int num3 = (int) this.attachedInstances[index].instance.set3DAttributes(RuntimeUtils.To3DAttributes(this.attachedInstances[index].transform, this.attachedInstances[index].rigidBody));
        }
        else
        {
          int num4 = (int) this.attachedInstances[index].instance.set3DAttributes(RuntimeUtils.To3DAttributes(this.attachedInstances[index].transform, this.attachedInstances[index].rigidBody2D));
        }
      }
    }

    public static void AttachInstanceToGameObject(
      EventInstance instance,
      Transform transform,
      Rigidbody rigidBody)
    {
      RuntimeManager.Instance.attachedInstances.Add(new RuntimeManager.AttachedInstance()
      {
        transform = transform,
        instance = instance,
        rigidBody = rigidBody
      });
    }

    public static void AttachInstanceToGameObject(
      EventInstance instance,
      Transform transform,
      Rigidbody2D rigidBody2D)
    {
      RuntimeManager.Instance.attachedInstances.Add(new RuntimeManager.AttachedInstance()
      {
        transform = transform,
        instance = instance,
        rigidBody2D = rigidBody2D,
        rigidBody = (Rigidbody) null
      });
    }

    public static void DetachInstanceFromGameObject(EventInstance instance)
    {
      RuntimeManager instance1 = RuntimeManager.Instance;
      for (int index = 0; index < instance1.attachedInstances.Count; ++index)
      {
        if (instance1.attachedInstances[index].instance.handle == instance.handle)
        {
          instance1.attachedInstances.RemoveAt(index);
          break;
        }
      }
    }

    private void DrawDebugOverlay(int windowID)
    {
      if ((double) this.lastDebugUpdate + 0.25 < (double) Time.unscaledTime)
      {
        if (RuntimeManager.initException != null)
        {
          this.lastDebugText = RuntimeManager.initException.Message;
        }
        else
        {
          if (!this.mixerHead.hasHandle())
          {
            ChannelGroup channelgroup;
            int masterChannelGroup = (int) this.lowlevelSystem.getMasterChannelGroup(out channelgroup);
            int dsp = (int) channelgroup.getDSP(0, out this.mixerHead);
            int num = (int) this.mixerHead.setMeteringEnabled(false, true);
          }
          StringBuilder stringBuilder = new StringBuilder();
          CPU_USAGE usage;
          int cpuUsage = (int) this.studioSystem.getCPUUsage(out usage);
          stringBuilder.AppendFormat("CPU: dsp = {0:F1}%, studio = {1:F1}%\n", (object) usage.dspusage, (object) usage.studiousage);
          int currentalloced;
          int maxalloced;
          int stats = (int) Memory.GetStats(out currentalloced, out maxalloced);
          stringBuilder.AppendFormat("MEMORY: cur = {0}MB, max = {1}MB\n", (object) (currentalloced >> 20), (object) (maxalloced >> 20));
          int channels;
          int realchannels;
          int channelsPlaying = (int) this.lowlevelSystem.getChannelsPlaying(out channels, out realchannels);
          stringBuilder.AppendFormat("CHANNELS: real = {0}, total = {1}\n", (object) realchannels, (object) channels);
          DSP_METERING_INFO outputInfo;
          int meteringInfo = (int) this.mixerHead.getMeteringInfo(IntPtr.Zero, out outputInfo);
          float num1 = 0.0f;
          for (int index = 0; index < (int) outputInfo.numchannels; ++index)
            num1 += outputInfo.rmslevel[index] * outputInfo.rmslevel[index];
          float num2 = Mathf.Sqrt(num1 / (float) outputInfo.numchannels);
          float num3 = (double) num2 > 0.0 ? 20f * Mathf.Log10(num2 * Mathf.Sqrt(2f)) : -80f;
          if ((double) num3 > 10.0)
            num3 = 10f;
          stringBuilder.AppendFormat("VOLUME: RMS = {0:f2}db\n", (object) num3);
          this.lastDebugText = stringBuilder.ToString();
          this.lastDebugUpdate = Time.unscaledTime;
        }
      }
      GUI.Label(new Rect(10f, 20f, 290f, 100f), this.lastDebugText);
      GUI.DragWindow();
    }

    private void OnDisable()
    {
      this.cachedPointers[0] = (long) this.studioSystem.handle;
      this.cachedPointers[1] = (long) this.lowlevelSystem.handle;
    }

    private void OnDestroy()
    {
      if (this.studioSystem.isValid())
      {
        int num = (int) this.studioSystem.release();
        this.studioSystem.clearHandle();
      }
      RuntimeManager.initException = (SystemNotInitializedException) null;
      RuntimeManager.instance = (RuntimeManager) null;
      RuntimeManager.isQuitting = true;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
      if (!this.studioSystem.isValid())
        return;
      if (this.loadedBanks.Count > 1)
        RuntimeManager.PauseAllEvents(pauseStatus);
      if (pauseStatus)
      {
        int num1 = (int) this.lowlevelSystem.mixerSuspend();
      }
      else
      {
        int num2 = (int) this.lowlevelSystem.mixerResume();
      }
    }

    private void loadedBankRegister(
      RuntimeManager.LoadedBank loadedBank,
      string bankPath,
      string bankName,
      bool loadSamples,
      RESULT loadResult)
    {
      if (loadResult == RESULT.OK)
      {
        loadedBank.RefCount = 1;
        if (loadSamples)
        {
          int num = (int) loadedBank.Bank.loadSampleData();
        }
        RuntimeManager.Instance.loadedBanks.Add(bankName, loadedBank);
      }
      else
      {
        if (loadResult != RESULT.ERR_EVENT_ALREADY_LOADED)
          throw new BankLoadException(bankPath, loadResult);
        loadedBank.RefCount = 2;
        RuntimeManager.Instance.loadedBanks.Add(bankName, loadedBank);
      }
    }

    public static void LoadBank(string bankName, bool loadSamples = false)
    {
      if (RuntimeManager.Instance.loadedBanks.ContainsKey(bankName))
      {
        RuntimeManager.LoadedBank loadedBank = RuntimeManager.Instance.loadedBanks[bankName];
        ++loadedBank.RefCount;
        if (loadSamples)
        {
          int num = (int) loadedBank.Bank.loadSampleData();
        }
        RuntimeManager.Instance.loadedBanks[bankName] = loadedBank;
      }
      else
      {
        string bankPath = RuntimeUtils.GetBankPath(bankName);
        RuntimeManager.LoadedBank loadedBank = new RuntimeManager.LoadedBank();
        RESULT loadResult = RuntimeManager.Instance.studioSystem.loadBankFile(bankPath, LOAD_BANK_FLAGS.NORMAL, out loadedBank.Bank);
        RuntimeManager.Instance.loadedBankRegister(loadedBank, bankPath, bankName, loadSamples, loadResult);
      }
    }

    public static void LoadBank(TextAsset asset, bool loadSamples = false)
    {
      string name = asset.name;
      if (RuntimeManager.Instance.loadedBanks.ContainsKey(name))
      {
        RuntimeManager.LoadedBank loadedBank = RuntimeManager.Instance.loadedBanks[name];
        ++loadedBank.RefCount;
        if (!loadSamples)
          return;
        int num = (int) loadedBank.Bank.loadSampleData();
      }
      else
      {
        RuntimeManager.LoadedBank loadedBank = new RuntimeManager.LoadedBank();
        RESULT result = RuntimeManager.Instance.studioSystem.loadBankMemory(asset.bytes, LOAD_BANK_FLAGS.NORMAL, out loadedBank.Bank);
        switch (result)
        {
          case RESULT.OK:
            loadedBank.RefCount = 1;
            RuntimeManager.Instance.loadedBanks.Add(name, loadedBank);
            if (!loadSamples)
              break;
            int num = (int) loadedBank.Bank.loadSampleData();
            break;
          case RESULT.ERR_EVENT_ALREADY_LOADED:
            loadedBank.RefCount = 2;
            RuntimeManager.Instance.loadedBanks.Add(name, loadedBank);
            break;
          default:
            throw new BankLoadException(name, result);
        }
      }
    }

    private void LoadBanks(Settings fmodSettings)
    {
      if (fmodSettings.ImportType != ImportType.StreamingAssets)
        return;
      try
      {
        RuntimeManager.LoadBank(fmodSettings.MasterBank + ".strings", fmodSettings.AutomaticSampleLoading);
        if (!fmodSettings.AutomaticEventLoading)
          return;
        RuntimeManager.LoadBank(fmodSettings.MasterBank, fmodSettings.AutomaticSampleLoading);
        foreach (string bank in fmodSettings.Banks)
          RuntimeManager.LoadBank(bank, fmodSettings.AutomaticSampleLoading);
        RuntimeManager.WaitForAllLoads();
      }
      catch (BankLoadException ex)
      {
        Debug.LogException((Exception) ex);
      }
    }

    public static void UnloadBank(string bankName)
    {
      RuntimeManager.LoadedBank loadedBank;
      if (!RuntimeManager.Instance.loadedBanks.TryGetValue(bankName, out loadedBank))
        return;
      --loadedBank.RefCount;
      if (loadedBank.RefCount == 0)
      {
        int num = (int) loadedBank.Bank.unload();
        RuntimeManager.Instance.loadedBanks.Remove(bankName);
      }
      else
        RuntimeManager.Instance.loadedBanks[bankName] = loadedBank;
    }

    public static bool AnyBankLoading()
    {
      bool flag = false;
      foreach (RuntimeManager.LoadedBank loadedBank in RuntimeManager.Instance.loadedBanks.Values)
      {
        LOADING_STATE state;
        int sampleLoadingState = (int) loadedBank.Bank.getSampleLoadingState(out state);
        flag |= state == LOADING_STATE.LOADING;
      }
      return flag;
    }

    public static void WaitForAllLoads()
    {
      int num = (int) RuntimeManager.Instance.studioSystem.flushSampleLoading();
    }

    public static Guid PathToGUID(string path)
    {
      Guid empty = Guid.Empty;
      if (path.StartsWith("{"))
      {
        int id = (int) FMOD.Studio.Util.ParseID(path, out empty);
      }
      else if (RuntimeManager.Instance.studioSystem.lookupID(path, out empty) == RESULT.ERR_EVENT_NOTFOUND)
        throw new EventNotFoundException(path);
      return empty;
    }

    public static EventInstance CreateInstance(string path)
    {
      try
      {
        return RuntimeManager.CreateInstance(RuntimeManager.PathToGUID(path));
      }
      catch (EventNotFoundException ex)
      {
        throw new EventNotFoundException(path);
      }
    }

    public static EventInstance CreateInstance(Guid guid)
    {
      EventInstance instance1;
      int instance2 = (int) RuntimeManager.GetEventDescription(guid).createInstance(out instance1);
      return instance1;
    }

    public static void PlayOneShot(string path, Vector3 position = default (Vector3))
    {
      try
      {
        RuntimeManager.PlayOneShot(RuntimeManager.PathToGUID(path), position);
      }
      catch (EventNotFoundException ex)
      {
        Debug.LogWarning((object) ("FMOD Event not found: " + path));
      }
    }

    public static void PlayOneShot(Guid guid, Vector3 position = default (Vector3))
    {
      EventInstance instance = RuntimeManager.CreateInstance(guid);
      int num1 = (int) instance.set3DAttributes(position.To3DAttributes());
      int num2 = (int) instance.start();
      int num3 = (int) instance.release();
    }

    public static void PlayOneShotAttached(string path, GameObject gameObject)
    {
      try
      {
        RuntimeManager.PlayOneShotAttached(RuntimeManager.PathToGUID(path), gameObject);
      }
      catch (EventNotFoundException ex)
      {
        Debug.LogWarning((object) ("FMOD Event not found: " + path));
      }
    }

    public static void PlayOneShotAttached(Guid guid, GameObject gameObject)
    {
      EventInstance instance = RuntimeManager.CreateInstance(guid);
      RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, gameObject.GetComponent<Rigidbody>());
      int num1 = (int) instance.start();
      int num2 = (int) instance.release();
    }

    public static EventDescription GetEventDescription(string path)
    {
      try
      {
        return RuntimeManager.GetEventDescription(RuntimeManager.PathToGUID(path));
      }
      catch (EventNotFoundException ex)
      {
        throw new EventNotFoundException(path);
      }
    }

    public static EventDescription GetEventDescription(Guid guid)
    {
      EventDescription _event;
      if (RuntimeManager.Instance.cachedDescriptions.ContainsKey(guid) && RuntimeManager.Instance.cachedDescriptions[guid].isValid())
      {
        _event = RuntimeManager.Instance.cachedDescriptions[guid];
      }
      else
      {
        if (RuntimeManager.Instance.studioSystem.getEventByID(guid, out _event) != RESULT.OK)
          throw new EventNotFoundException(guid);
        if (_event.isValid())
          RuntimeManager.Instance.cachedDescriptions[guid] = _event;
      }
      return _event;
    }

    public static void SetListenerLocation(GameObject gameObject, Rigidbody rigidBody = null)
    {
      int num = (int) RuntimeManager.Instance.studioSystem.setListenerAttributes(0, RuntimeUtils.To3DAttributes(gameObject, rigidBody));
    }

    public static void SetListenerLocation(GameObject gameObject, Rigidbody2D rigidBody2D)
    {
      int num = (int) RuntimeManager.Instance.studioSystem.setListenerAttributes(0, RuntimeUtils.To3DAttributes(gameObject, rigidBody2D));
    }

    public static void SetListenerLocation(Transform transform)
    {
      int num = (int) RuntimeManager.Instance.studioSystem.setListenerAttributes(0, transform.To3DAttributes());
    }

    public static void SetListenerLocation(
      int listenerIndex,
      GameObject gameObject,
      Rigidbody rigidBody = null)
    {
      int num = (int) RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, RuntimeUtils.To3DAttributes(gameObject, rigidBody));
    }

    public static void SetListenerLocation(
      int listenerIndex,
      GameObject gameObject,
      Rigidbody2D rigidBody2D)
    {
      int num = (int) RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, RuntimeUtils.To3DAttributes(gameObject, rigidBody2D));
    }

    public static void SetListenerLocation(int listenerIndex, Transform transform)
    {
      int num = (int) RuntimeManager.Instance.studioSystem.setListenerAttributes(listenerIndex, transform.To3DAttributes());
    }

    public static Bus GetBus(string path)
    {
      Bus bus;
      if (RuntimeManager.StudioSystem.getBus(path, out bus) != RESULT.OK)
        throw new BusNotFoundException(path);
      return bus;
    }

    public static VCA GetVCA(string path)
    {
      VCA vca;
      if (RuntimeManager.StudioSystem.getVCA(path, out vca) != RESULT.OK)
        throw new VCANotFoundException(path);
      return vca;
    }

    public static void PauseAllEvents(bool paused)
    {
      int num = (int) RuntimeManager.GetBus("bus:/").setPaused(paused);
    }

    public static void MuteAllEvents(bool muted)
    {
      int num = (int) RuntimeManager.GetBus("bus:/").setMute(muted);
    }

    public static bool IsInitialized => (UnityEngine.Object) RuntimeManager.instance != (UnityEngine.Object) null && RuntimeManager.instance.studioSystem.isValid() && RuntimeManager.instance.initializedSuccessfully;

    public static bool HasBanksLoaded => RuntimeManager.Instance.loadedBanks.Count > 1;

    public static bool HasBankLoaded(string loadedBank) => RuntimeManager.instance.loadedBanks.ContainsKey(loadedBank);

    private void LoadPlugins(Settings fmodSettings)
    {
      foreach (string plugin in fmodSettings.Plugins)
      {
        if (!string.IsNullOrEmpty(plugin))
        {
          string pluginPath = RuntimeUtils.GetPluginPath(plugin);
          uint handle;
          RESULT result = this.lowlevelSystem.loadPlugin(pluginPath, out handle);
          switch (result)
          {
            case RESULT.ERR_FILE_BAD:
            case RESULT.ERR_FILE_NOTFOUND:
              result = this.lowlevelSystem.loadPlugin(RuntimeUtils.GetPluginPath(plugin + "64"), out handle);
              break;
          }
          this.CheckInitResult(result, string.Format("Loading plugin '{0}' from '{1}'", (object) plugin, (object) pluginPath));
          this.loadedPlugins.Add(plugin, handle);
        }
      }
    }

    private struct LoadedBank
    {
      public Bank Bank;
      public int RefCount;
    }

    private class GuidComparer : IEqualityComparer<Guid>
    {
      bool IEqualityComparer<Guid>.Equals(Guid x, Guid y) => x.Equals(y);

      int IEqualityComparer<Guid>.GetHashCode(Guid obj) => obj.GetHashCode();
    }

    private class AttachedInstance
    {
      public EventInstance instance;
      public Transform transform;
      public Rigidbody rigidBody;
      public Rigidbody2D rigidBody2D;
    }
  }
}
