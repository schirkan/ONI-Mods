﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.GlobalStatsReceived_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1112)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GlobalStatsReceived_t
  {
    public const int k_iCallback = 1112;
    public ulong m_nGameID;
    public EResult m_eResult;
  }
}
