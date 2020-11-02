// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatSteamIDInstanceFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EChatSteamIDInstanceFlags
  {
    k_EChatAccountInstanceMask = 4095, // 0x00000FFF
    k_EChatInstanceFlagClan = 524288, // 0x00080000
    k_EChatInstanceFlagLobby = 262144, // 0x00040000
    k_EChatInstanceFlagMMSLobby = 131072, // 0x00020000
  }
}
