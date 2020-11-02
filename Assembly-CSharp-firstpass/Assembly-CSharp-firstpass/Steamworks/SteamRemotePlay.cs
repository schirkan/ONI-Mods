// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamRemotePlay
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamRemotePlay
  {
    public static uint GetSessionCount()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemotePlay_GetSessionCount(CSteamAPIContext.GetSteamRemotePlay());
    }

    public static uint GetSessionID(int iSessionIndex)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemotePlay_GetSessionID(CSteamAPIContext.GetSteamRemotePlay(), iSessionIndex);
    }

    public static CSteamID GetSessionSteamID(uint unSessionID)
    {
      InteropHelp.TestIfAvailableClient();
      return (CSteamID) NativeMethods.ISteamRemotePlay_GetSessionSteamID(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
    }

    public static string GetSessionClientName(uint unSessionID)
    {
      InteropHelp.TestIfAvailableClient();
      return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamRemotePlay_GetSessionClientName(CSteamAPIContext.GetSteamRemotePlay(), unSessionID));
    }

    public static ESteamDeviceFormFactor GetSessionClientFormFactor(
      uint unSessionID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemotePlay_GetSessionClientFormFactor(CSteamAPIContext.GetSteamRemotePlay(), unSessionID);
    }

    public static bool BGetSessionClientResolution(
      uint unSessionID,
      out int pnResolutionX,
      out int pnResolutionY)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamRemotePlay_BGetSessionClientResolution(CSteamAPIContext.GetSteamRemotePlay(), unSessionID, out pnResolutionX, out pnResolutionY);
    }
  }
}
