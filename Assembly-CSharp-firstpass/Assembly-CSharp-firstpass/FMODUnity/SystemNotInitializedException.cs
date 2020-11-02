// Decompiled with JetBrains decompiler
// Type: FMODUnity.SystemNotInitializedException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD;
using System;

namespace FMODUnity
{
  public class SystemNotInitializedException : Exception
  {
    public RESULT Result;
    public string Location;

    public SystemNotInitializedException(RESULT result, string location)
      : base(string.Format("FMOD Studio initialization failed : {2} : {0} : {1}", (object) result.ToString(), (object) Error.String(result), (object) location))
    {
      this.Result = result;
      this.Location = location;
    }

    public SystemNotInitializedException(Exception inner)
      : base("FMOD Studio initialization failed", inner)
    {
    }
  }
}
