// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.LOAD_BANK_FLAGS
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  [Flags]
  public enum LOAD_BANK_FLAGS : uint
  {
    NORMAL = 0,
    NONBLOCKING = 1,
    DECOMPRESS_SAMPLES = 2,
  }
}
