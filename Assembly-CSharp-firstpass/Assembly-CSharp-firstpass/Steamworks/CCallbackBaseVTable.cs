// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBaseVTable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBaseVTable
  {
    private const CallingConvention cc = CallingConvention.Cdecl;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCRDel m_RunCallResult;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCBDel m_RunCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RunCBDel(IntPtr thisptr, IntPtr pvParam);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RunCRDel(
      IntPtr thisptr,
      IntPtr pvParam,
      [MarshalAs(UnmanagedType.I1)] bool bIOFailure,
      ulong hSteamAPICall);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetCallbackSizeBytesDel(IntPtr thisptr);
  }
}
