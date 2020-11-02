// Decompiled with JetBrains decompiler
// Type: Steamworks.EMarketNotAllowedReasonFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EMarketNotAllowedReasonFlags
  {
    k_EMarketNotAllowedReason_None = 0,
    k_EMarketNotAllowedReason_TemporaryFailure = 1,
    k_EMarketNotAllowedReason_AccountDisabled = 2,
    k_EMarketNotAllowedReason_AccountLockedDown = 4,
    k_EMarketNotAllowedReason_AccountLimited = 8,
    k_EMarketNotAllowedReason_TradeBanned = 16, // 0x00000010
    k_EMarketNotAllowedReason_AccountNotTrusted = 32, // 0x00000020
    k_EMarketNotAllowedReason_SteamGuardNotEnabled = 64, // 0x00000040
    k_EMarketNotAllowedReason_SteamGuardOnlyRecentlyEnabled = 128, // 0x00000080
    k_EMarketNotAllowedReason_RecentPasswordReset = 256, // 0x00000100
    k_EMarketNotAllowedReason_NewPaymentMethod = 512, // 0x00000200
    k_EMarketNotAllowedReason_InvalidCookie = 1024, // 0x00000400
    k_EMarketNotAllowedReason_UsingNewDevice = 2048, // 0x00000800
    k_EMarketNotAllowedReason_RecentSelfRefund = 4096, // 0x00001000
    k_EMarketNotAllowedReason_NewPaymentMethodCannotBeVerified = 8192, // 0x00002000
    k_EMarketNotAllowedReason_NoRecentPurchases = 16384, // 0x00004000
    k_EMarketNotAllowedReason_AcceptedWalletGift = 32768, // 0x00008000
  }
}
