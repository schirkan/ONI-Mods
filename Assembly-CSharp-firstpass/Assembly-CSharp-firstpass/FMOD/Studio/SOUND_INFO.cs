// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.SOUND_INFO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  public struct SOUND_INFO
  {
    public IntPtr name_or_data;
    public MODE mode;
    public CREATESOUNDEXINFO exinfo;
    public int subsoundindex;

    public string name
    {
      get
      {
        using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
          return (this.mode & (MODE.OPENMEMORY | MODE.OPENMEMORY_POINT)) == MODE.DEFAULT ? freeHelper.stringFromNative(this.name_or_data) : string.Empty;
      }
    }
  }
}
