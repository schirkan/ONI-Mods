// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamUGCQueryCompleted_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(3401)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamUGCQueryCompleted_t
  {
    public const int k_iCallback = 3401;
    public UGCQueryHandle_t m_handle;
    public EResult m_eResult;
    public uint m_unNumResultsReturned;
    public uint m_unTotalMatchingResults;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bCachedData;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string m_rgchNextCursor;
  }
}
