// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.BANK_INFO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD.Studio
{
  public struct BANK_INFO
  {
    public int size;
    public IntPtr userdata;
    public int userdatalength;
    public FILE_OPENCALLBACK opencallback;
    public FILE_CLOSECALLBACK closecallback;
    public FILE_READCALLBACK readcallback;
    public FILE_SEEKCALLBACK seekcallback;
  }
}
