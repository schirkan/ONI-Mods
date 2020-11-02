// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamGameSearch
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamGameSearch
  {
    public static EGameSearchErrorCode_t AddGameSearchParams(
      string pchKeyToFind,
      string pchValuesToFind)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKeyToFind1 = new InteropHelp.UTF8StringHandle(pchKeyToFind))
      {
        using (InteropHelp.UTF8StringHandle pchValuesToFind1 = new InteropHelp.UTF8StringHandle(pchValuesToFind))
          return NativeMethods.ISteamGameSearch_AddGameSearchParams(CSteamAPIContext.GetSteamGameSearch(), pchKeyToFind1, pchValuesToFind1);
      }
    }

    public static EGameSearchErrorCode_t SearchForGameWithLobby(
      CSteamID steamIDLobby,
      int nPlayerMin,
      int nPlayerMax)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_SearchForGameWithLobby(CSteamAPIContext.GetSteamGameSearch(), steamIDLobby, nPlayerMin, nPlayerMax);
    }

    public static EGameSearchErrorCode_t SearchForGameSolo(
      int nPlayerMin,
      int nPlayerMax)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_SearchForGameSolo(CSteamAPIContext.GetSteamGameSearch(), nPlayerMin, nPlayerMax);
    }

    public static EGameSearchErrorCode_t AcceptGame()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_AcceptGame(CSteamAPIContext.GetSteamGameSearch());
    }

    public static EGameSearchErrorCode_t DeclineGame()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_DeclineGame(CSteamAPIContext.GetSteamGameSearch());
    }

    public static EGameSearchErrorCode_t RetrieveConnectionDetails(
      CSteamID steamIDHost,
      out string pchConnectionDetails,
      int cubConnectionDetails)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cubConnectionDetails);
      EGameSearchErrorCode_t searchErrorCodeT = NativeMethods.ISteamGameSearch_RetrieveConnectionDetails(CSteamAPIContext.GetSteamGameSearch(), steamIDHost, num, cubConnectionDetails);
      pchConnectionDetails = searchErrorCodeT != (EGameSearchErrorCode_t) 0 ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return searchErrorCodeT;
    }

    public static EGameSearchErrorCode_t EndGameSearch()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_EndGameSearch(CSteamAPIContext.GetSteamGameSearch());
    }

    public static EGameSearchErrorCode_t SetGameHostParams(
      string pchKey,
      string pchValue)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchKey1 = new InteropHelp.UTF8StringHandle(pchKey))
      {
        using (InteropHelp.UTF8StringHandle pchValue1 = new InteropHelp.UTF8StringHandle(pchValue))
          return NativeMethods.ISteamGameSearch_SetGameHostParams(CSteamAPIContext.GetSteamGameSearch(), pchKey1, pchValue1);
      }
    }

    public static EGameSearchErrorCode_t SetConnectionDetails(
      string pchConnectionDetails,
      int cubConnectionDetails)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchConnectionDetails1 = new InteropHelp.UTF8StringHandle(pchConnectionDetails))
        return NativeMethods.ISteamGameSearch_SetConnectionDetails(CSteamAPIContext.GetSteamGameSearch(), pchConnectionDetails1, cubConnectionDetails);
    }

    public static EGameSearchErrorCode_t RequestPlayersForGame(
      int nPlayerMin,
      int nPlayerMax,
      int nMaxTeamSize)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_RequestPlayersForGame(CSteamAPIContext.GetSteamGameSearch(), nPlayerMin, nPlayerMax, nMaxTeamSize);
    }

    public static EGameSearchErrorCode_t HostConfirmGameStart(
      ulong ullUniqueGameID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_HostConfirmGameStart(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID);
    }

    public static EGameSearchErrorCode_t CancelRequestPlayersForGame()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_CancelRequestPlayersForGame(CSteamAPIContext.GetSteamGameSearch());
    }

    public static EGameSearchErrorCode_t SubmitPlayerResult(
      ulong ullUniqueGameID,
      CSteamID steamIDPlayer,
      EPlayerResult_t EPlayerResult)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_SubmitPlayerResult(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID, steamIDPlayer, EPlayerResult);
    }

    public static EGameSearchErrorCode_t EndGame(ulong ullUniqueGameID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamGameSearch_EndGame(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID);
    }
  }
}
