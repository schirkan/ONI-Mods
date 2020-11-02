// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamParentalSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Steamworks
{
  public static class SteamParentalSettings
  {
    public static bool BIsParentalLockEnabled()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParentalSettings_BIsParentalLockEnabled(CSteamAPIContext.GetSteamParentalSettings());
    }

    public static bool BIsParentalLockLocked()
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParentalSettings_BIsParentalLockLocked(CSteamAPIContext.GetSteamParentalSettings());
    }

    public static bool BIsAppBlocked(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParentalSettings_BIsAppBlocked(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
    }

    public static bool BIsAppInBlockList(AppId_t nAppID)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParentalSettings_BIsAppInBlockList(CSteamAPIContext.GetSteamParentalSettings(), nAppID);
    }

    public static bool BIsFeatureBlocked(EParentalFeature eFeature)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParentalSettings_BIsFeatureBlocked(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
    }

    public static bool BIsFeatureInBlockList(EParentalFeature eFeature)
    {
      InteropHelp.TestIfAvailableClient();
      return NativeMethods.ISteamParentalSettings_BIsFeatureInBlockList(CSteamAPIContext.GetSteamParentalSettings(), eFeature);
    }
  }
}
