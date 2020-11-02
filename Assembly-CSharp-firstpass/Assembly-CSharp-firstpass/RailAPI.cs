// Decompiled with JetBrains decompiler
// Type: RailAPI
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

public static class RailAPI
{
  [DllImport("RailAPI")]
  public static extern bool RestartAppIfNecessary(ulong gameId);

  [DllImport("RailAPI")]
  public static extern bool Initialize();

  [DllImport("RailAPI")]
  public static extern bool NotifyWindowAntiAddiction();

  [DllImport("RailAPI")]
  public static extern void Shutdown();

  [DllImport("RailAPI")]
  public static extern void FireEvents();

  [DllImport("RailAPI")]
  public static extern void RegisterEventHandler(
    RailAPI.Event eventId,
    RailAPI.EventHandler handler);

  [DllImport("RailAPI")]
  public static extern void UnregisterEventHandler(
    RailAPI.Event eventId,
    RailAPI.EventHandler handler);

  [DllImport("RailAPI")]
  public static extern void ApplyWordFilter(string strIn, StringBuilder strOut, ref int strOutLen);

  [DllImport("RailAPI")]
  public static extern void GetRailPlatformId(ref int idOut);

  [DllImport("RailAPI")]
  public static extern void GetLocalUserName(StringBuilder strOut, ref int strOutLen);

  [DllImport("RailAPI")]
  public static extern void GetLocalUserId(ref ulong idOut);

  [DllImport("RailAPI")]
  public static extern void RequestAuthTicket();

  public enum Platform
  {
    TGP = 1,
    QQGame = 2,
  }

  public enum Event
  {
    EventSystemChanged = 2,
    AuthTicketAcquired = 13001, // 0x000032C9
  }

  [Serializable]
  public struct AuthTicketResponse
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
    public string ticket;
  }

  [Serializable]
  public struct RailSystemStateChanged
  {
    public bool mRequestExit;
  }

  public delegate void EventHandler(RailAPI.Event eventId, IntPtr data);
}
