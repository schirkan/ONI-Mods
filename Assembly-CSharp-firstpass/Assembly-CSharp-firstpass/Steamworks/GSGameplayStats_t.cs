// Decompiled with JetBrains decompiler
// Type: Steamworks.GSGameplayStats_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(207)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct GSGameplayStats_t
  {
    public const int k_iCallback = 207;
    public EResult m_eResult;
    public int m_nRank;
    public uint m_unTotalConnects;
    public uint m_unTotalMinutesPlayed;
  }
}
