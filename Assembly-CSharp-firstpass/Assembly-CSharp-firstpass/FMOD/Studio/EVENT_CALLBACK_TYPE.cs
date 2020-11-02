﻿// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.EVENT_CALLBACK_TYPE
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  [Flags]
  public enum EVENT_CALLBACK_TYPE : uint
  {
    CREATED = 1,
    DESTROYED = 2,
    STARTING = 4,
    STARTED = 8,
    RESTARTED = 16, // 0x00000010
    STOPPED = 32, // 0x00000020
    START_FAILED = 64, // 0x00000040
    CREATE_PROGRAMMER_SOUND = 128, // 0x00000080
    DESTROY_PROGRAMMER_SOUND = 256, // 0x00000100
    PLUGIN_CREATED = 512, // 0x00000200
    PLUGIN_DESTROYED = 1024, // 0x00000400
    TIMELINE_MARKER = 2048, // 0x00000800
    TIMELINE_BEAT = 4096, // 0x00001000
    SOUND_PLAYED = 8192, // 0x00002000
    SOUND_STOPPED = 16384, // 0x00004000
    ALL = 4294967295, // 0xFFFFFFFF
  }
}
