// Decompiled with JetBrains decompiler
// Type: DistributionPlatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class DistributionPlatform : MonoBehaviour
{
  private static DistributionPlatform.Implementation sImpl;

  public static bool Initialized => DistributionPlatform.Impl.Initialized;

  public static DistributionPlatform.Implementation Inst => DistributionPlatform.Impl;

  public static void Initialize()
  {
    if (DistributionPlatform.sImpl != null)
      return;
    DistributionPlatform.sImpl = (DistributionPlatform.Implementation) new GameObject(nameof (DistributionPlatform)).AddComponent<SteamDistributionPlatform>();
    if (SteamManager.Initialized)
      return;
    Debug.LogError((object) "Steam not initialized in time.");
  }

  public static event System.Action onExitRequest;

  public static void RequestExit()
  {
    if (DistributionPlatform.onExitRequest == null)
      return;
    DistributionPlatform.onExitRequest();
  }

  private static DistributionPlatform.Implementation Impl => DistributionPlatform.sImpl;

  public interface Implementation
  {
    bool Initialized { get; }

    string Name { get; }

    string Platform { get; }

    string AccountLoginEndpoint { get; }

    string MetricsClientKey { get; }

    string MetricsUserIDField { get; }

    DistributionPlatform.User LocalUser { get; }

    bool IsArchiveBranch { get; }

    string ApplyWordFilter(string text);

    void GetAuthTicket(DistributionPlatform.AuthTicketHandler callback);
  }

  public delegate void AuthTicketHandler(byte[] ticket);

  public abstract class UserId
  {
    public abstract ulong ToInt64();
  }

  public abstract class User
  {
    public abstract DistributionPlatform.UserId Id { get; }

    public abstract string Name { get; }
  }
}
