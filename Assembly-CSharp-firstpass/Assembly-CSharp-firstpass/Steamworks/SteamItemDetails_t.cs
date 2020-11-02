// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamItemDetails_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct SteamItemDetails_t
  {
    public SteamItemInstanceID_t m_itemId;
    public SteamItemDef_t m_iDefinition;
    public ushort m_unQuantity;
    public ushort m_unFlags;
  }
}
