﻿// Decompiled with JetBrains decompiler
// Type: FMOD.DSP_STATE_FUNCTIONS
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public struct DSP_STATE_FUNCTIONS
  {
    private DSP_ALLOC_FUNC alloc;
    private DSP_REALLOC_FUNC realloc;
    private DSP_FREE_FUNC free;
    private DSP_GETSAMPLERATE_FUNC getsamplerate;
    private DSP_GETBLOCKSIZE_FUNC getblocksize;
    private IntPtr dft;
    private IntPtr pan;
    private DSP_GETSPEAKERMODE_FUNC getspeakermode;
    private DSP_GETCLOCK_FUNC getclock;
    private DSP_GETLISTENERATTRIBUTES_FUNC getlistenerattributes;
    private DSP_LOG_FUNC log;
    private DSP_GETUSERDATA_FUNC getuserdata;
  }
}
