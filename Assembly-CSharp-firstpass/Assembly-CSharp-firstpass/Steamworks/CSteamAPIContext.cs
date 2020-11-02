// Decompiled with JetBrains decompiler
// Type: Steamworks.CSteamAPIContext
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  internal static class CSteamAPIContext
  {
    private static IntPtr m_pSteamClient;
    private static IntPtr m_pSteamUser;
    private static IntPtr m_pSteamFriends;
    private static IntPtr m_pSteamUtils;
    private static IntPtr m_pSteamMatchmaking;
    private static IntPtr m_pSteamUserStats;
    private static IntPtr m_pSteamApps;
    private static IntPtr m_pSteamMatchmakingServers;
    private static IntPtr m_pSteamNetworking;
    private static IntPtr m_pSteamRemoteStorage;
    private static IntPtr m_pSteamScreenshots;
    private static IntPtr m_pSteamGameSearch;
    private static IntPtr m_pSteamHTTP;
    private static IntPtr m_pController;
    private static IntPtr m_pSteamUGC;
    private static IntPtr m_pSteamAppList;
    private static IntPtr m_pSteamMusic;
    private static IntPtr m_pSteamMusicRemote;
    private static IntPtr m_pSteamHTMLSurface;
    private static IntPtr m_pSteamInventory;
    private static IntPtr m_pSteamVideo;
    private static IntPtr m_pSteamParentalSettings;
    private static IntPtr m_pSteamInput;
    private static IntPtr m_pSteamParties;
    private static IntPtr m_pSteamRemotePlay;

    internal static void Clear()
    {
      CSteamAPIContext.m_pSteamClient = IntPtr.Zero;
      CSteamAPIContext.m_pSteamUser = IntPtr.Zero;
      CSteamAPIContext.m_pSteamFriends = IntPtr.Zero;
      CSteamAPIContext.m_pSteamUtils = IntPtr.Zero;
      CSteamAPIContext.m_pSteamMatchmaking = IntPtr.Zero;
      CSteamAPIContext.m_pSteamUserStats = IntPtr.Zero;
      CSteamAPIContext.m_pSteamApps = IntPtr.Zero;
      CSteamAPIContext.m_pSteamMatchmakingServers = IntPtr.Zero;
      CSteamAPIContext.m_pSteamNetworking = IntPtr.Zero;
      CSteamAPIContext.m_pSteamRemoteStorage = IntPtr.Zero;
      CSteamAPIContext.m_pSteamHTTP = IntPtr.Zero;
      CSteamAPIContext.m_pSteamScreenshots = IntPtr.Zero;
      CSteamAPIContext.m_pSteamGameSearch = IntPtr.Zero;
      CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
      CSteamAPIContext.m_pController = IntPtr.Zero;
      CSteamAPIContext.m_pSteamUGC = IntPtr.Zero;
      CSteamAPIContext.m_pSteamAppList = IntPtr.Zero;
      CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
      CSteamAPIContext.m_pSteamMusicRemote = IntPtr.Zero;
      CSteamAPIContext.m_pSteamHTMLSurface = IntPtr.Zero;
      CSteamAPIContext.m_pSteamInventory = IntPtr.Zero;
      CSteamAPIContext.m_pSteamVideo = IntPtr.Zero;
      CSteamAPIContext.m_pSteamParentalSettings = IntPtr.Zero;
      CSteamAPIContext.m_pSteamInput = IntPtr.Zero;
      CSteamAPIContext.m_pSteamParties = IntPtr.Zero;
      CSteamAPIContext.m_pSteamRemotePlay = IntPtr.Zero;
    }

    internal static bool Init()
    {
      HSteamUser hsteamUser = SteamAPI.GetHSteamUser();
      HSteamPipe hsteamPipe = SteamAPI.GetHSteamPipe();
      if (hsteamPipe == (HSteamPipe) 0)
        return false;
      using (InteropHelp.UTF8StringHandle ver = new InteropHelp.UTF8StringHandle("SteamClient019"))
        CSteamAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(ver);
      if (CSteamAPIContext.m_pSteamClient == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamUser = SteamClient.GetISteamUser(hsteamUser, hsteamPipe, "SteamUser020");
      if (CSteamAPIContext.m_pSteamUser == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamFriends = SteamClient.GetISteamFriends(hsteamUser, hsteamPipe, "SteamFriends017");
      if (CSteamAPIContext.m_pSteamFriends == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamUtils = SteamClient.GetISteamUtils(hsteamPipe, "SteamUtils009");
      if (CSteamAPIContext.m_pSteamUtils == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamMatchmaking = SteamClient.GetISteamMatchmaking(hsteamUser, hsteamPipe, "SteamMatchMaking009");
      if (CSteamAPIContext.m_pSteamMatchmaking == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(hsteamUser, hsteamPipe, "SteamMatchMakingServers002");
      if (CSteamAPIContext.m_pSteamMatchmakingServers == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamUserStats = SteamClient.GetISteamUserStats(hsteamUser, hsteamPipe, "STEAMUSERSTATS_INTERFACE_VERSION011");
      if (CSteamAPIContext.m_pSteamUserStats == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamApps = SteamClient.GetISteamApps(hsteamUser, hsteamPipe, "STEAMAPPS_INTERFACE_VERSION008");
      if (CSteamAPIContext.m_pSteamApps == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamNetworking = SteamClient.GetISteamNetworking(hsteamUser, hsteamPipe, "SteamNetworking005");
      if (CSteamAPIContext.m_pSteamNetworking == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(hsteamUser, hsteamPipe, "STEAMREMOTESTORAGE_INTERFACE_VERSION014");
      if (CSteamAPIContext.m_pSteamRemoteStorage == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamScreenshots = SteamClient.GetISteamScreenshots(hsteamUser, hsteamPipe, "STEAMSCREENSHOTS_INTERFACE_VERSION003");
      if (CSteamAPIContext.m_pSteamScreenshots == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamGameSearch = SteamClient.GetISteamGameSearch(hsteamUser, hsteamPipe, "SteamMatchGameSearch001");
      if (CSteamAPIContext.m_pSteamGameSearch == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamHTTP = SteamClient.GetISteamHTTP(hsteamUser, hsteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
      if (CSteamAPIContext.m_pSteamHTTP == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pController = SteamClient.GetISteamController(hsteamUser, hsteamPipe, "SteamController007");
      if (CSteamAPIContext.m_pController == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamUGC = SteamClient.GetISteamUGC(hsteamUser, hsteamPipe, "STEAMUGC_INTERFACE_VERSION013");
      if (CSteamAPIContext.m_pSteamUGC == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamAppList = SteamClient.GetISteamAppList(hsteamUser, hsteamPipe, "STEAMAPPLIST_INTERFACE_VERSION001");
      if (CSteamAPIContext.m_pSteamAppList == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamMusic = SteamClient.GetISteamMusic(hsteamUser, hsteamPipe, "STEAMMUSIC_INTERFACE_VERSION001");
      if (CSteamAPIContext.m_pSteamMusic == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamMusicRemote = SteamClient.GetISteamMusicRemote(hsteamUser, hsteamPipe, "STEAMMUSICREMOTE_INTERFACE_VERSION001");
      if (CSteamAPIContext.m_pSteamMusicRemote == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(hsteamUser, hsteamPipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
      if (CSteamAPIContext.m_pSteamHTMLSurface == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamInventory = SteamClient.GetISteamInventory(hsteamUser, hsteamPipe, "STEAMINVENTORY_INTERFACE_V003");
      if (CSteamAPIContext.m_pSteamInventory == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamVideo = SteamClient.GetISteamVideo(hsteamUser, hsteamPipe, "STEAMVIDEO_INTERFACE_V002");
      if (CSteamAPIContext.m_pSteamVideo == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamParentalSettings = SteamClient.GetISteamParentalSettings(hsteamUser, hsteamPipe, "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
      if (CSteamAPIContext.m_pSteamParentalSettings == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamInput = SteamClient.GetISteamInput(hsteamUser, hsteamPipe, "SteamInput001");
      if (CSteamAPIContext.m_pSteamInput == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamParties = SteamClient.GetISteamParties(hsteamUser, hsteamPipe, "SteamParties002");
      if (CSteamAPIContext.m_pSteamParties == IntPtr.Zero)
        return false;
      CSteamAPIContext.m_pSteamRemotePlay = SteamClient.GetISteamRemotePlay(hsteamUser, hsteamPipe, "STEAMREMOTEPLAY_INTERFACE_VERSION001");
      return !(CSteamAPIContext.m_pSteamRemotePlay == IntPtr.Zero);
    }

    internal static IntPtr GetSteamClient() => CSteamAPIContext.m_pSteamClient;

    internal static IntPtr GetSteamUser() => CSteamAPIContext.m_pSteamUser;

    internal static IntPtr GetSteamFriends() => CSteamAPIContext.m_pSteamFriends;

    internal static IntPtr GetSteamUtils() => CSteamAPIContext.m_pSteamUtils;

    internal static IntPtr GetSteamMatchmaking() => CSteamAPIContext.m_pSteamMatchmaking;

    internal static IntPtr GetSteamUserStats() => CSteamAPIContext.m_pSteamUserStats;

    internal static IntPtr GetSteamApps() => CSteamAPIContext.m_pSteamApps;

    internal static IntPtr GetSteamMatchmakingServers() => CSteamAPIContext.m_pSteamMatchmakingServers;

    internal static IntPtr GetSteamNetworking() => CSteamAPIContext.m_pSteamNetworking;

    internal static IntPtr GetSteamRemoteStorage() => CSteamAPIContext.m_pSteamRemoteStorage;

    internal static IntPtr GetSteamScreenshots() => CSteamAPIContext.m_pSteamScreenshots;

    internal static IntPtr GetSteamGameSearch() => CSteamAPIContext.m_pSteamGameSearch;

    internal static IntPtr GetSteamHTTP() => CSteamAPIContext.m_pSteamHTTP;

    internal static IntPtr GetSteamController() => CSteamAPIContext.m_pController;

    internal static IntPtr GetSteamUGC() => CSteamAPIContext.m_pSteamUGC;

    internal static IntPtr GetSteamAppList() => CSteamAPIContext.m_pSteamAppList;

    internal static IntPtr GetSteamMusic() => CSteamAPIContext.m_pSteamMusic;

    internal static IntPtr GetSteamMusicRemote() => CSteamAPIContext.m_pSteamMusicRemote;

    internal static IntPtr GetSteamHTMLSurface() => CSteamAPIContext.m_pSteamHTMLSurface;

    internal static IntPtr GetSteamInventory() => CSteamAPIContext.m_pSteamInventory;

    internal static IntPtr GetSteamVideo() => CSteamAPIContext.m_pSteamVideo;

    internal static IntPtr GetSteamParentalSettings() => CSteamAPIContext.m_pSteamParentalSettings;

    internal static IntPtr GetSteamInput() => CSteamAPIContext.m_pSteamInput;

    internal static IntPtr GetSteamParties() => CSteamAPIContext.m_pSteamParties;

    internal static IntPtr GetSteamRemotePlay() => CSteamAPIContext.m_pSteamRemotePlay;
  }
}
