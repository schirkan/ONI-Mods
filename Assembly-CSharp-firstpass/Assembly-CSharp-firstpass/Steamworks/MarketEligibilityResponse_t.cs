﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.MarketEligibilityResponse_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(166)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct MarketEligibilityResponse_t
  {
    public const int k_iCallback = 166;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAllowed;
    public EMarketNotAllowedReasonFlags m_eNotAllowedReason;
    public RTime32 m_rtAllowedAtTime;
    public int m_cdaySteamGuardRequiredDays;
    public int m_cdayNewDeviceCooldown;
  }
}
