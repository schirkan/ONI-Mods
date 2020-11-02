// Decompiled with JetBrains decompiler
// Type: Steamworks.RemoteStorageFileReadAsyncComplete_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(1332)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct RemoteStorageFileReadAsyncComplete_t
  {
    public const int k_iCallback = 1332;
    public SteamAPICall_t m_hFileReadAsync;
    public EResult m_eResult;
    public uint m_nOffset;
    public uint m_cubRead;
  }
}
