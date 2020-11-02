// Decompiled with JetBrains decompiler
// Type: Steamworks.ISteamMatchmakingRulesResponse
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  public class ISteamMatchmakingRulesResponse
  {
    private ISteamMatchmakingRulesResponse.VTable m_VTable;
    private IntPtr m_pVTable;
    private GCHandle m_pGCHandle;
    private ISteamMatchmakingRulesResponse.RulesResponded m_RulesResponded;
    private ISteamMatchmakingRulesResponse.RulesFailedToRespond m_RulesFailedToRespond;
    private ISteamMatchmakingRulesResponse.RulesRefreshComplete m_RulesRefreshComplete;

    public ISteamMatchmakingRulesResponse(
      ISteamMatchmakingRulesResponse.RulesResponded onRulesResponded,
      ISteamMatchmakingRulesResponse.RulesFailedToRespond onRulesFailedToRespond,
      ISteamMatchmakingRulesResponse.RulesRefreshComplete onRulesRefreshComplete)
    {
      if (onRulesResponded == null || onRulesFailedToRespond == null || onRulesRefreshComplete == null)
        throw new ArgumentNullException();
      this.m_RulesResponded = onRulesResponded;
      this.m_RulesFailedToRespond = onRulesFailedToRespond;
      this.m_RulesRefreshComplete = onRulesRefreshComplete;
      this.m_VTable = new ISteamMatchmakingRulesResponse.VTable()
      {
        m_VTRulesResponded = new ISteamMatchmakingRulesResponse.InternalRulesResponded(this.InternalOnRulesResponded),
        m_VTRulesFailedToRespond = new ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond(this.InternalOnRulesFailedToRespond),
        m_VTRulesRefreshComplete = new ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete(this.InternalOnRulesRefreshComplete)
      };
      this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (ISteamMatchmakingRulesResponse.VTable)));
      Marshal.StructureToPtr<ISteamMatchmakingRulesResponse.VTable>((M0) this.m_VTable, this.m_pVTable, false);
      this.m_pGCHandle = GCHandle.Alloc((object) this.m_pVTable, GCHandleType.Pinned);
    }

    ~ISteamMatchmakingRulesResponse()
    {
      if (this.m_pVTable != IntPtr.Zero)
        Marshal.FreeHGlobal(this.m_pVTable);
      if (!this.m_pGCHandle.IsAllocated)
        return;
      this.m_pGCHandle.Free();
    }

    private void InternalOnRulesResponded(IntPtr thisptr, IntPtr pchRule, IntPtr pchValue) => this.m_RulesResponded(InteropHelp.PtrToStringUTF8(pchRule), InteropHelp.PtrToStringUTF8(pchValue));

    private void InternalOnRulesFailedToRespond(IntPtr thisptr) => this.m_RulesFailedToRespond();

    private void InternalOnRulesRefreshComplete(IntPtr thisptr) => this.m_RulesRefreshComplete();

    public static explicit operator IntPtr(ISteamMatchmakingRulesResponse that) => that.m_pGCHandle.AddrOfPinnedObject();

    public delegate void RulesResponded(string pchRule, string pchValue);

    public delegate void RulesFailedToRespond();

    public delegate void RulesRefreshComplete();

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void InternalRulesResponded(IntPtr thisptr, IntPtr pchRule, IntPtr pchValue);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void InternalRulesFailedToRespond(IntPtr thisptr);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void InternalRulesRefreshComplete(IntPtr thisptr);

    [StructLayout(LayoutKind.Sequential)]
    private class VTable
    {
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;
      [MarshalAs(UnmanagedType.FunctionPtr)]
      [NonSerialized]
      public ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
    }
  }
}
