// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUGCRequestUGCDetailsResult_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(3402)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamUGCRequestUGCDetailsResult_t
  {
    public const int k_iCallback = 3402;
    public SteamUGCDetails_t m_details;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bCachedData;
  }
}
