// Decompiled with JetBrains decompiler
// Type: Steamworks.ESteamItemFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum ESteamItemFlags
  {
    k_ESteamItemNoTrade = 1,
    k_ESteamItemRemoved = 256, // 0x00000100
    k_ESteamItemConsumed = 512, // 0x00000200
  }
}
