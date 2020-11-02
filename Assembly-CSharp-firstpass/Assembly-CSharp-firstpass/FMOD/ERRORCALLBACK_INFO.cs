// Decompiled with JetBrains decompiler
// Type: FMOD.ERRORCALLBACK_INFO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMOD
{
  public struct ERRORCALLBACK_INFO
  {
    public RESULT result;
    public ERRORCALLBACK_INSTANCETYPE instancetype;
    public IntPtr instance;
    public StringWrapper functionname;
    public StringWrapper functionparams;
  }
}
