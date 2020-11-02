// Decompiled with JetBrains decompiler
// Type: FMOD.TIMEUNIT
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  [Flags]
  public enum TIMEUNIT : uint
  {
    MS = 1,
    PCM = 2,
    PCMBYTES = 4,
    RAWBYTES = 8,
    PCMFRACTION = 16, // 0x00000010
    MODORDER = 256, // 0x00000100
    MODROW = 512, // 0x00000200
    MODPATTERN = 1024, // 0x00000400
  }
}
