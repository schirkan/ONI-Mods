// Decompiled with JetBrains decompiler
// Type: Steamworks.EMarketingMessageFlags
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum EMarketingMessageFlags
  {
    k_EMarketingMessageFlagsNone = 0,
    k_EMarketingMessageFlagsHighPriority = 1,
    k_EMarketingMessageFlagsPlatformWindows = 2,
    k_EMarketingMessageFlagsPlatformMac = 4,
    k_EMarketingMessageFlagsPlatformLinux = 8,
    k_EMarketingMessageFlagsPlatformRestrictions = k_EMarketingMessageFlagsPlatformLinux | k_EMarketingMessageFlagsPlatformMac | k_EMarketingMessageFlagsPlatformWindows, // 0x0000000E
  }
}
