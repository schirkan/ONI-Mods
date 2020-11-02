// Decompiled with JetBrains decompiler
// Type: KleiMetrics
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using UnityEngine;

public class KleiMetrics : ThreadedHttps<KleiMetrics>
{
  private const string SessionIDKey = "SESSION_ID";
  private const string GameIDKey = "GAME_ID";
  private const string InstallTimeStampKey = "INSTALL_TIMESTAMP";
  private const string UserIDFieldName = "user";
  private const string SessionIDFieldName = "SessionID";
  private const string GameIDFieldName = "GameID";
  private const string InstallTimeStampFieldName = "InstallTimeStamp";
  private const string KleiUserFieldName = "KU";
  private const string StartSessionFieldName = "StartSession";
  private const string EndSessionFieldName = "EndSession";
  private const string EndSessionCrashedFieldName = "EndSessionCrashed";
  private const string SessionStartTimeStampFieldName = "SessionStartTimeStamp";
  private const string SessionTimeFieldName = "SessionTimeSeconds";
  private const string NewGameFieldName = "NewGame";
  private const string EndGameFieldName = "EndGame";
  public const string GameTimeFieldName = "GameTimeSeconds";
  private const string LevelFieldName = "Level";
  public const string BuildBranchName = "Branch";
  public const string BuildFieldName = "Build";
  private const int EDITOR_BUILD_ID = -1;
  private const string HeartBeatFieldName = "HeartBeat";
  private const string HeartBeatTimeOutFieldName = "HeartBeatTimeOut";
  private const string LastUserActionFieldName = "LastUA";
  public const string SaveFolderWriteTest = "SaveFolderWriteTest";
  private string PlatformUserIDFieldName;
  private static int sessionID = -1;
  private static int gameID = -1;
  private static string installTimeStamp = (string) null;
  private static System.Timers.Timer heartbeatTimer;
  private int HeartBeatInSeconds = 180;
  private int HeartBeatTimeOutInSeconds = 1200;
  private long currentSessionTicks = DateTime.Now.Ticks;
  private float timeSinceLastUserAction;
  private long lastHeartBeatTicks = DateTime.Now.Ticks;
  private long startTimeTicks = DateTime.Now.Ticks;
  private bool shouldEndSession;
  private bool shouldStartSession;
  private bool hasStarted;
  private Dictionary<string, object> userSession = new Dictionary<string, object>();
  private System.Action<Dictionary<string, object>> SetDynamicSessionVariables;
  private System.Action SetStaticSessionVariables;
  private bool sessionStarted;
  private long sessionStartUtcTicks = DateTime.UtcNow.Ticks;

  public KleiMetrics()
  {
    this.LIVE_ENDPOINT = "oni.metrics.klei.com/write";
    this.serviceName = nameof (KleiMetrics);
    this.CLIENT_KEY = DistributionPlatform.Inst.MetricsClientKey;
    this.PlatformUserIDFieldName = DistributionPlatform.Inst.MetricsUserIDField;
    KleiMetrics.sessionID = -1;
    this.enabled = !KPrivacyPrefs.instance.disableDataCollection;
    KleiMetrics.GameID();
    this.isMultiThreaded = true;
  }

  public bool isMultiThreaded { get; protected set; }

  public bool enabled { get; private set; }

  public void SetEnabled(bool enabled) => this.enabled = enabled;

  protected string PostMetricData(Dictionary<string, object> data, string debug_source)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) new KleiMetrics.PostData(this.CLIENT_KEY, data)));
    if (!this.isMultiThreaded)
      return this.Send(bytes);
    this.PutPacket(bytes);
    return "OK";
  }

  public static string PlatformUserID()
  {
    DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
    return localUser == null ? "" : localUser.Id.ToString();
  }

  public static string UserID()
  {
    DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
    return localUser == null ? "" : localUser.Id.ToString();
  }

  private void IncrementSessionCount()
  {
    KleiMetrics.sessionID = KleiMetrics.SessionID() + 1;
    KPlayerPrefs.SetInt("SESSION_ID", KleiMetrics.sessionID);
  }

  public static int SessionID()
  {
    if (KleiMetrics.sessionID == -1)
      KleiMetrics.sessionID = KPlayerPrefs.GetInt("SESSION_ID", -1);
    return KleiMetrics.sessionID;
  }

  private void IncrementGameCount()
  {
    KleiMetrics.gameID = KleiMetrics.GameID() + 1;
    KleiMetrics.SetGameID(KleiMetrics.gameID);
  }

  public static int GameID()
  {
    if (KleiMetrics.gameID == -1)
      KleiMetrics.gameID = KPlayerPrefs.GetInt("GAME_ID", -1);
    return KleiMetrics.gameID;
  }

  public static void SetGameID(int id) => KPlayerPrefs.SetInt("GAME_ID", id);

  public static string GetInstallTimeStamp()
  {
    if (KleiMetrics.installTimeStamp == null)
    {
      KleiMetrics.installTimeStamp = KPlayerPrefs.GetString("INSTALL_TIMESTAMP", (string) null);
      if (KleiMetrics.installTimeStamp == null || KleiMetrics.installTimeStamp == "")
      {
        KleiMetrics.installTimeStamp = DateTime.UtcNow.Ticks.ToString();
        KPlayerPrefs.SetString("INSTALL_TIMESTAMP", KleiMetrics.installTimeStamp);
      }
    }
    return KleiMetrics.installTimeStamp;
  }

  public static string CurrentLevel() => (string) null;

  public void SetLastUserAction(long lastUserActionTicks)
  {
    if (!this.enabled || !this.sessionStarted)
      return;
    this.currentSessionTicks = DateTime.Now.Ticks;
    if (this.shouldEndSession)
    {
      this.EndSession();
      this.shouldEndSession = false;
      this.shouldStartSession = true;
    }
    else if (this.shouldStartSession && lastUserActionTicks > this.lastHeartBeatTicks)
    {
      this.StartSession();
      this.shouldStartSession = false;
    }
    this.timeSinceLastUserAction = (float) TimeSpan.FromTicks(this.currentSessionTicks - lastUserActionTicks).TotalSeconds;
  }

  private void StopHeartBeat()
  {
    if (KleiMetrics.heartbeatTimer == null)
      return;
    KleiMetrics.heartbeatTimer.Stop();
    KleiMetrics.heartbeatTimer.Dispose();
    KleiMetrics.heartbeatTimer = (System.Timers.Timer) null;
  }

  private void StartHeartBeat()
  {
    if (!this.enabled || !this.sessionStarted)
      return;
    this.StopHeartBeat();
    KleiMetrics.heartbeatTimer = new System.Timers.Timer((double) (this.HeartBeatInSeconds * 1000));
    KleiMetrics.heartbeatTimer.Elapsed += new ElapsedEventHandler(this.SendHeartBeat);
    KleiMetrics.heartbeatTimer.AutoReset = true;
    KleiMetrics.heartbeatTimer.Enabled = true;
    this.lastHeartBeatTicks = DateTime.Now.Ticks;
  }

  private uint GetSessionTime()
  {
    int totalSeconds = (int) TimeSpan.FromTicks(this.currentSessionTicks - this.startTimeTicks).TotalSeconds;
    if (totalSeconds >= 0)
      return (uint) totalSeconds;
    Debug.LogWarning((object) "Session time is < 0");
    return (uint) totalSeconds;
  }

  private void SendHeartBeat(object source, ElapsedEventArgs e)
  {
    if (!this.enabled || !this.sessionStarted)
      return;
    Dictionary<string, object> userSession = this.GetUserSession();
    userSession.Add("LastUA", (object) (int) this.timeSinceLastUserAction);
    if ((double) this.timeSinceLastUserAction > (double) this.HeartBeatTimeOutInSeconds)
    {
      userSession.Add("HeartBeatTimeOut", (object) true);
      KleiMetrics.heartbeatTimer.Stop();
      this.shouldEndSession = true;
    }
    DateTime now = DateTime.Now;
    long num = now.Ticks - this.lastHeartBeatTicks;
    userSession.Add("HeartBeat", (object) (int) TimeSpan.FromTicks(num).TotalSeconds);
    this.PostMetricData(userSession, nameof (SendHeartBeat));
    now = DateTime.Now;
    this.lastHeartBeatTicks = now.Ticks;
  }

  private void StartThread()
  {
    if (this.hasStarted)
      return;
    if (this.isMultiThreaded)
      this.Start();
    this.hasStarted = true;
  }

  private void EndThread()
  {
    if (!this.hasStarted)
      return;
    if (this.isMultiThreaded)
      this.End();
    this.hasStarted = false;
  }

  public void SetStaticSessionVariable(string name, object var)
  {
    if (this.userSession.ContainsKey(name))
      this.userSession[name] = var;
    else
      this.userSession.Add(name, var);
  }

  public void RemoveStaticSessionVariable(string name)
  {
    if (!this.userSession.ContainsKey(name))
      return;
    this.userSession.Remove(name);
  }

  public void AddDefaultSessionVariables()
  {
    this.userSession.Clear();
    this.SetStaticSessionVariable("InstallTimeStamp", (object) KleiMetrics.GetInstallTimeStamp());
    this.SetStaticSessionVariable("user", (object) KleiMetrics.UserID());
    this.SetStaticSessionVariable("SessionID", (object) KleiMetrics.SessionID());
    this.SetStaticSessionVariable("SessionStartTimeStamp", (object) this.sessionStartUtcTicks.ToString());
    if (KleiAccount.KleiUserID == null)
      return;
    this.SetStaticSessionVariable("KU", (object) KleiAccount.KleiUserID);
  }

  private Dictionary<string, object> GetUserSession()
  {
    Debug.Assert(this.enabled);
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    if (!this.sessionStarted)
      return dictionary;
    foreach (KeyValuePair<string, object> keyValuePair in this.userSession)
      dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    dictionary.Add("SessionTimeSeconds", (object) this.GetSessionTime());
    if (KleiMetrics.GameID() != -1)
      dictionary.Add("GameID", (object) KleiMetrics.GameID());
    string str = KleiMetrics.CurrentLevel();
    if (str != null)
      dictionary.Add("Level", (object) str);
    if (this.SetDynamicSessionVariables != null)
    {
      try
      {
        this.SetDynamicSessionVariables(dictionary);
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("Dynamic session variables may be set from a thread. " + ex.Message + "\n" + ex.StackTrace));
      }
    }
    return dictionary;
  }

  public void SetCallBacks(
    System.Action setStaticSessionVariables,
    System.Action<Dictionary<string, object>> setDynamicSessionVariables)
  {
    this.SetDynamicSessionVariables = setDynamicSessionVariables;
    this.SetStaticSessionVariables = setStaticSessionVariables;
  }

  private void SetStartTime()
  {
    this.sessionStartUtcTicks = DateTime.UtcNow.Ticks;
    this.startTimeTicks = DateTime.Now.Ticks;
    this.currentSessionTicks = DateTime.Now.Ticks;
    this.sessionStarted = true;
  }

  public void StartSession()
  {
    if (!this.enabled)
      return;
    if (this.sessionStarted)
      this.EndSession();
    this.StartThread();
    this.SetStartTime();
    this.IncrementSessionCount();
    this.AddDefaultSessionVariables();
    if (this.SetStaticSessionVariables != null)
      this.SetStaticSessionVariables();
    Dictionary<string, object> userSession = this.GetUserSession();
    userSession.Add(nameof (StartSession), (object) true);
    string str = KleiMetrics.PlatformUserID();
    if (str != null)
      userSession.Add(this.PlatformUserIDFieldName, (object) str);
    if (this.shouldStartSession)
      userSession.Add("HeartBeatTimeOut", (object) false);
    foreach (KeyValuePair<string, object> hardwareStat in KleiMetrics.GetHardwareStats())
      userSession.Add(hardwareStat.Key, hardwareStat.Value);
    this.PostMetricData(userSession, nameof (StartSession));
    this.StartHeartBeat();
  }

  public void EndSession(bool crashed = false)
  {
    if (!this.enabled || !this.sessionStarted)
      return;
    Dictionary<string, object> userSession = this.GetUserSession();
    userSession.Add(nameof (EndSession), (object) true);
    if (crashed)
      userSession.Add("EndSessionCrashed", (object) true);
    if (this.shouldEndSession)
      userSession.Add("HeartBeatTimeOut", (object) true);
    this.PostMetricData(userSession, nameof (EndSession));
    this.sessionStarted = false;
    this.StopHeartBeat();
    this.EndThread();
  }

  public void StartNewGame()
  {
    if (!this.enabled)
      return;
    if (!this.sessionStarted)
      this.StartSession();
    this.IncrementGameCount();
    Dictionary<string, object> userSession = this.GetUserSession();
    userSession.Add("NewGame", (object) true);
    this.PostMetricData(userSession, nameof (StartNewGame));
  }

  public void EndGame()
  {
    if (!this.enabled || !this.sessionStarted)
      return;
    Dictionary<string, object> userSession = this.GetUserSession();
    userSession.Add(nameof (EndGame), (object) true);
    this.PostMetricData(userSession, nameof (EndGame));
  }

  public void SendEvent(Dictionary<string, object> eventData, string debug_event_name)
  {
    if (!this.enabled)
      return;
    if (!this.sessionStarted)
      this.StartSession();
    Dictionary<string, object> userSession = this.GetUserSession();
    foreach (KeyValuePair<string, object> keyValuePair in eventData)
      userSession.Add(keyValuePair.Key, keyValuePair.Value);
    this.PostMetricData(userSession, "SendEvent:" + debug_event_name);
  }

  public bool SendProfileStats() => this.enabled && ThreadedHttps<KleiMetrics>.Instance.PostMetricData(this.GetUserSession(), nameof (SendProfileStats)) == "OK";

  public static Dictionary<string, object> GetHardwareStats() => new Dictionary<string, object>()
  {
    {
      "Platform",
      (object) Application.platform.ToString()
    },
    {
      "OSname",
      (object) SystemInfo.operatingSystem
    },
    {
      "OSversion",
      (object) Environment.OSVersion.Version.ToString()
    },
    {
      "CPUmodel",
      (object) SystemInfo.deviceModel
    },
    {
      "CPUdeviceType",
      (object) SystemInfo.deviceType.ToString()
    },
    {
      "CPUarch",
      (object) Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")
    },
    {
      "ProcBits",
      (object) (IntPtr.Size == 4 ? 32 : 64)
    },
    {
      "CPUcount",
      (object) SystemInfo.processorCount
    },
    {
      "CPUtype",
      (object) SystemInfo.processorType
    },
    {
      "SystemMemoryMegs",
      (object) SystemInfo.systemMemorySize
    },
    {
      "GPUgraphicsDeviceID",
      (object) SystemInfo.graphicsDeviceID
    },
    {
      "GPUname",
      (object) SystemInfo.graphicsDeviceName
    },
    {
      "GPUgraphicsDeviceType",
      (object) SystemInfo.graphicsDeviceType.ToString()
    },
    {
      "GPUgraphicsDeviceVendor",
      (object) SystemInfo.graphicsDeviceVendor
    },
    {
      "GPUgraphicsDeviceVendorID",
      (object) SystemInfo.graphicsDeviceVendorID
    },
    {
      "GPUgraphicsDeviceVersion",
      (object) SystemInfo.graphicsDeviceVersion
    },
    {
      "GPUmemoryMegs",
      (object) SystemInfo.graphicsMemorySize
    },
    {
      "GPUgraphicsMultiThreaded",
      (object) SystemInfo.graphicsMultiThreaded
    },
    {
      "GPUgraphicsShaderLevel",
      (object) SystemInfo.graphicsShaderLevel
    },
    {
      "GPUmaxTextureSize",
      (object) SystemInfo.maxTextureSize
    },
    {
      "GPUnpotSupport",
      (object) SystemInfo.npotSupport.ToString()
    },
    {
      "GPUsupportedRenderTargetCount",
      (object) SystemInfo.supportedRenderTargetCount
    },
    {
      "GPUsupports2DArrayTextures",
      (object) SystemInfo.supports2DArrayTextures
    },
    {
      "GPUsupports3DTextures",
      (object) SystemInfo.supports3DTextures
    },
    {
      "GPUsupportsComputeShaders",
      (object) SystemInfo.supportsComputeShaders
    },
    {
      "GPUsupportsImageEffects",
      (object) SystemInfo.supportsImageEffects
    },
    {
      "GPUsupportsInstancing",
      (object) SystemInfo.supportsInstancing
    },
    {
      "GPUsupportsRenderToCubemap",
      (object) SystemInfo.supportsRenderToCubemap
    },
    {
      "GPUsupportsShadows",
      (object) SystemInfo.supportsShadows
    },
    {
      "GPUsupportsSparseTextures",
      (object) SystemInfo.supportsSparseTextures
    }
  };

  protected struct PostData
  {
    public string clientKey;
    public Dictionary<string, object> metricData;

    public PostData(string key, Dictionary<string, object> data)
    {
      this.clientKey = key;
      this.metricData = data;
    }
  }
}
