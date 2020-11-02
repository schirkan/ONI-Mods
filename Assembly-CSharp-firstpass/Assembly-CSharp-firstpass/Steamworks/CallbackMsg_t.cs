// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackMsg_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct CallbackMsg_t
  {
    public int m_hSteamUser;
    public int m_iCallback;
    public IntPtr m_pubParam;
    public int m_cubParam;
  }
}
