// Decompiled with JetBrains decompiler
// Type: Steamworks.FavoritesListChanged_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(502)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct FavoritesListChanged_t
  {
    public const int k_iCallback = 502;
    public uint m_nIP;
    public uint m_nQueryPort;
    public uint m_nConnPort;
    public uint m_nAppID;
    public uint m_nFlags;
    [MarshalAs(UnmanagedType.I1)]
    public bool m_bAdd;
    public AccountID_t m_unAccountId;
  }
}
