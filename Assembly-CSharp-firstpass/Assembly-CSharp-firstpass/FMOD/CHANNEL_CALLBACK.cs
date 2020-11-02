// Decompiled with JetBrains decompiler
// Type: FMOD.CHANNEL_CALLBACK
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public delegate RESULT CHANNEL_CALLBACK(
    IntPtr channelraw,
    CHANNELCONTROL_TYPE controltype,
    CHANNELCONTROL_CALLBACK_TYPE type,
    IntPtr commanddata1,
    IntPtr commanddata2);
}
