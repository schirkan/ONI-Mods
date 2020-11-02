// Decompiled with JetBrains decompiler
// Type: Steamworks.DurationControl_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(167)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct DurationControl_t
  {
    public const int k_iCallback = 167;
    public EResult m_eResult;
    public AppId_t m_appid;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bApplicable;
    public int m_csecsLast5h;
    public EDurationControlProgress m_progress;
    public EDurationControlNotification m_notification;
  }
}
