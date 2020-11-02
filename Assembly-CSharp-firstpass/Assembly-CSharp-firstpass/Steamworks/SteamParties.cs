// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamParties
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public static class SteamParties
  {
    public static uint GetNumActiveBeacons()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParties_GetNumActiveBeacons(CSteamAPIContext.GetSteamParties());
    }

    public static PartyBeaconID_t GetBeaconByIndex(uint unIndex)
    {
      InteropHelp.TestIfAvailableClient();
      return (PartyBeaconID_t) NativeMethods.ISteamParties_GetBeaconByIndex(CSteamAPIContext.GetSteamParties(), unIndex);
    }

    public static bool GetBeaconDetails(
      PartyBeaconID_t ulBeaconID,
      out CSteamID pSteamIDBeaconOwner,
      out SteamPartyBeaconLocation_t pLocation,
      out string pchMetadata,
      int cchMetadata)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cchMetadata);
      bool beaconDetails = NativeMethods.ISteamParties_GetBeaconDetails(CSteamAPIContext.GetSteamParties(), ulBeaconID, out pSteamIDBeaconOwner, out pLocation, num, cchMetadata);
      pchMetadata = beaconDetails ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return beaconDetails;
    }

    public static SteamAPICall_t JoinParty(PartyBeaconID_t ulBeaconID)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamParties_JoinParty(CSteamAPIContext.GetSteamParties(), ulBeaconID);
    }

    public static bool GetNumAvailableBeaconLocations(out uint puNumLocations)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParties_GetNumAvailableBeaconLocations(CSteamAPIContext.GetSteamParties(), out puNumLocations);
    }

    public static bool GetAvailableBeaconLocations(
      SteamPartyBeaconLocation_t[] pLocationList,
      uint uMaxNumLocations)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParties_GetAvailableBeaconLocations(CSteamAPIContext.GetSteamParties(), pLocationList, uMaxNumLocations);
    }

    public static SteamAPICall_t CreateBeacon(
      uint unOpenSlots,
      ref SteamPartyBeaconLocation_t pBeaconLocation,
      string pchConnectString,
      string pchMetadata)
    {
      InteropHelp.TestIfAvailableClient();
      using (InteropHelp.UTF8StringHandle pchConnectString1 = new InteropHelp.UTF8StringHandle(pchConnectString))
      {
        using (InteropHelp.UTF8StringHandle pchMetadata1 = new InteropHelp.UTF8StringHandle(pchMetadata))
          return (SteamAPICall_t) NativeMethods.ISteamParties_CreateBeacon(CSteamAPIContext.GetSteamParties(), unOpenSlots, ref pBeaconLocation, pchConnectString1, pchMetadata1);
      }
    }

    public static void OnReservationCompleted(PartyBeaconID_t ulBeacon, CSteamID steamIDUser)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamParties_OnReservationCompleted(CSteamAPIContext.GetSteamParties(), ulBeacon, steamIDUser);
    }

    public static void CancelReservation(PartyBeaconID_t ulBeacon, CSteamID steamIDUser)
    {
      InteropHelp.TestIfAvailableClient();
      NativeMethods.ISteamParties_CancelReservation(CSteamAPIContext.GetSteamParties(), ulBeacon, steamIDUser);
    }

    public static SteamAPICall_t ChangeNumOpenSlots(
      PartyBeaconID_t ulBeacon,
      uint unOpenSlots)
    {
      InteropHelp.TestIfAvailableClient();
      return (SteamAPICall_t) NativeMethods.ISteamParties_ChangeNumOpenSlots(CSteamAPIContext.GetSteamParties(), ulBeacon, unOpenSlots);
    }

    public static bool DestroyBeacon(PartyBeaconID_t ulBeacon)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParties_DestroyBeacon(CSteamAPIContext.GetSteamParties(), ulBeacon);
    }

    public static bool GetBeaconLocationData(
      SteamPartyBeaconLocation_t BeaconLocation,
      ESteamPartyBeaconLocationData eData,
      out string pchDataStringOut,
      int cchDataStringOut)
    {
      InteropHelp.TestIfAvailableClient();
      IntPtr num = Marshal.AllocHGlobal(cchDataStringOut);
      bool beaconLocationData = NativeMethods.ISteamParties_GetBeaconLocationData(CSteamAPIContext.GetSteamParties(), BeaconLocation, eData, num, cchDataStringOut);
      pchDataStringOut = beaconLocationData ? InteropHelp.PtrToStringUTF8(num) : (string) null;
      Marshal.FreeHGlobal(num);
      return beaconLocationData;
    }
  }
}
