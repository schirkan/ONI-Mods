// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.EVENT_CALLBACK
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  public delegate RESULT EVENT_CALLBACK(
    EVENT_CALLBACK_TYPE type,
    EventInstance eventInstance,
    IntPtr parameters);
}
