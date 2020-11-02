// Decompiled with JetBrains decompiler
// Type: Steamworks.EItemState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EItemState
  {
    k_EItemStateNone = 0,
    k_EItemStateSubscribed = 1,
    k_EItemStateLegacyItem = 2,
    k_EItemStateInstalled = 4,
    k_EItemStateNeedsUpdate = 8,
    k_EItemStateDownloading = 16, // 0x00000010
    k_EItemStateDownloadPending = 32, // 0x00000020
  }
}
