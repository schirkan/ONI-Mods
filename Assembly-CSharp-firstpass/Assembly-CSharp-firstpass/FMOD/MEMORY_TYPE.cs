﻿// Decompiled with JetBrains decompiler
// Type: FMOD.MEMORY_TYPE
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  [Flags]
  public enum MEMORY_TYPE : uint
  {
    NORMAL = 0,
    STREAM_FILE = 1,
    STREAM_DECODE = 2,
    SAMPLEDATA = 4,
    DSP_BUFFER = 8,
    PLUGIN = 16, // 0x00000010
    XBOX360_PHYSICAL = 1048576, // 0x00100000
    PERSISTENT = 2097152, // 0x00200000
    SECONDARY = 4194304, // 0x00400000
    ALL = 4294967295, // 0xFFFFFFFF
  }
}