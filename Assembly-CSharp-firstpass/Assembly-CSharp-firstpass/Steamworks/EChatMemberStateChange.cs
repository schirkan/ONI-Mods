// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatMemberStateChange
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EChatMemberStateChange
  {
    k_EChatMemberStateChangeEntered = 1,
    k_EChatMemberStateChangeLeft = 2,
    k_EChatMemberStateChangeDisconnected = 4,
    k_EChatMemberStateChangeKicked = 8,
    k_EChatMemberStateChangeBanned = 16, // 0x00000010
  }
}
