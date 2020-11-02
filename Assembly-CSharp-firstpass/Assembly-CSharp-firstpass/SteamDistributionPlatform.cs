// Decompiled with JetBrains decompiler
// Type: SteamDistributionPlatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Steamworks;
using System;
using UnityEngine;

internal class SteamDistributionPlatform : MonoBehaviour, DistributionPlatform.Implementation
{
  private SteamDistributionPlatform.SteamUser mLocalUser;

  public bool Initialized => SteamManager.Initialized;

  public string Name => "Steam";

  public string Platform => "Steam";

  public string AccountLoginEndpoint => "/login/LoginViaSteam";

  public string MetricsClientKey => "2Ehpf6QcWdCXV8eqbbiJBkrqD6xc8waX";

  public string MetricsUserIDField => "SteamUserID";

  public DistributionPlatform.User LocalUser
  {
    get
    {
      if (this.mLocalUser == null)
        this.InitializeLocalUser();
      return (DistributionPlatform.User) this.mLocalUser;
    }
  }

  public bool IsArchiveBranch
  {
    get
    {
      string pchName;
      SteamApps.GetCurrentBetaName(out pchName, 100);
      Debug.Log((object) ("Checking which steam branch we're on. Got: [" + pchName + "]"));
      return !(pchName == "default") && !(pchName == "release");
    }
  }

  public string ApplyWordFilter(string text) => text;

  public void GetAuthTicket(DistributionPlatform.AuthTicketHandler handler)
  {
    uint pcbTicket = 0;
    byte[] pTicket = new byte[2048];
    Steamworks.SteamUser.GetAuthSessionTicket(pTicket, pTicket.Length, out pcbTicket);
    byte[] ticket = new byte[(int) pcbTicket];
    if (0U < pcbTicket)
      Array.Copy((Array) pTicket, (Array) ticket, (long) pcbTicket);
    handler(ticket);
  }

  private void InitializeLocalUser()
  {
    if (!SteamManager.Initialized)
      return;
    this.mLocalUser = new SteamDistributionPlatform.SteamUser(Steamworks.SteamUser.GetSteamID(), SteamFriends.GetPersonaName());
  }

  public class SteamUserId : DistributionPlatform.UserId
  {
    private CSteamID mSteamId;

    public SteamUserId(CSteamID id) => this.mSteamId = id;

    public override string ToString() => this.mSteamId.ToString();

    public override ulong ToInt64() => this.mSteamId.m_SteamID;
  }

  public class SteamUser : DistributionPlatform.User
  {
    private SteamDistributionPlatform.SteamUserId mId;
    private string mName;

    public SteamUser(CSteamID id, string name)
    {
      this.mId = new SteamDistributionPlatform.SteamUserId(id);
      this.mName = name;
    }

    public override DistributionPlatform.UserId Id => (DistributionPlatform.UserId) this.mId;

    public override string Name => this.mName;
  }
}
