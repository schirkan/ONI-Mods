// Decompiled with JetBrains decompiler
// Type: Steamworks.ERemoteStoragePlatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum ERemoteStoragePlatform
  {
    k_ERemoteStoragePlatformNone = 0,
    k_ERemoteStoragePlatformWindows = 1,
    k_ERemoteStoragePlatformOSX = 2,
    k_ERemoteStoragePlatformPS3 = 4,
    k_ERemoteStoragePlatformLinux = 8,
    k_ERemoteStoragePlatformReserved2 = 16, // 0x00000010
    k_ERemoteStoragePlatformAndroid = 32, // 0x00000020
    k_ERemoteStoragePlatformIOS = 64, // 0x00000040
    k_ERemoteStoragePlatformAll = -1, // 0xFFFFFFFF
  }
}
