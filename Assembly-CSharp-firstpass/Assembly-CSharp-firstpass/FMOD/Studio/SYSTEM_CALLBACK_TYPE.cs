// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.SYSTEM_CALLBACK_TYPE
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  [Flags]
  public enum SYSTEM_CALLBACK_TYPE : uint
  {
    PREUPDATE = 1,
    POSTUPDATE = 2,
    BANK_UNLOAD = 4,
    ALL = 4294967295, // 0xFFFFFFFF
  }
}
