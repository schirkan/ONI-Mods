// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.INITFLAGS
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  [Flags]
  public enum INITFLAGS : uint
  {
    NORMAL = 0,
    LIVEUPDATE = 1,
    ALLOW_MISSING_PLUGINS = 2,
    SYNCHRONOUS_UPDATE = 4,
    DEFERRED_CALLBACKS = 8,
    LOAD_FROM_UPDATE = 16, // 0x00000010
  }
}
