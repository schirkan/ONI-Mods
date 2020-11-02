// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPICallCompleted_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(703)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamAPICallCompleted_t
  {
    public const int k_iCallback = 703;
    public SteamAPICall_t m_hAsyncCall;
    public int m_iCallback;
    public uint m_cubParam;
  }
}
