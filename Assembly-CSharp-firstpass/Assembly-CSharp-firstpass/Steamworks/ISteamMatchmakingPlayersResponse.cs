// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingPlayersResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public class ISteamMatchmakingPlayersResponse
  {
    private ISteamMatchmakingPlayersResponse.VTable m_VTable;
    private IntPtr m_pVTable;
    private GCHandle m_pGCHandle;
    private ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;
    private ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;
    private ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

    public ISteamMatchmakingPlayersResponse(
      ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList,
      ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond,
      ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
    {
      if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
        throw new ArgumentNullException();
      this.m_AddPlayerToList = onAddPlayerToList;
      this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
      this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
      this.m_VTable = new ISteamMatchmakingPlayersResponse.VTable()
      {
        m_VTAddPlayerToList = new ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(this.InternalOnAddPlayerToList),
        m_VTPlayersFailedToRespond = new ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
        m_VTPlayersRefreshComplete = new ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
      };
      this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingPlayersResponse.VTable)));
      Marshal.StructureToPtr<ISteamMatchmakingPlayersResponse.VTable>((M0) this.m_VTable, this.m_pVTable, false);
      this.m_pGCHandle = GCHandle.Alloc((object) this.m_pVTable, GCHandleType.Pinned);
    }

    ~ISteamMatchmakingPlayersResponse()
    {
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pGCHandle.IsAllocated)
        return;
      this.m_pGCHandle.Free();
    }

    private void InternalOnAddPlayerToList(
      IntPtr thisptr,
      IntPtr pchName,
      int nScore,
      float flTimePlayed)
    {
      this.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
    }

    private void InternalOnPlayersFailedToRespond(IntPtr thisptr) => this.m_PlayersFailedToRespond();

    private void InternalOnPlayersRefreshComplete(IntPtr thisptr) => this.m_PlayersRefreshComplete();

    public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that) => that.m_pGCHandle.AddrOfPinnedObject();

    public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

    public delegate void PlayersFailedToRespond();

    public delegate void PlayersRefreshComplete();

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void InternalAddPlayerToList(
      IntPtr thisptr,
      IntPtr pchName,
      int nScore,
      float flTimePlayed);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void InternalPlayersFailedToRespond(IntPtr thisptr);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void InternalPlayersRefreshComplete(IntPtr thisptr);

    [StructLayout(LayoutKind.Sequential)]
    private class VTable
    {
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
    }
  }
}
