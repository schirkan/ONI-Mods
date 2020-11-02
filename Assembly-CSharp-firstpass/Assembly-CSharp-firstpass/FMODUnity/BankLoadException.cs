// Decompiled with JetBrains decompiler
// Type: FMODUnity.BankLoadException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using FMOD;
using System;

namespace FMODUnity
{
  public class BankLoadException : Exception
  {
    public string Path;
    public RESULT Result;

    public BankLoadException(string path, RESULT result)
      : base(string.Format("FMOD Studio could not load bank '{0}' : {1} : {2}", (object) path, (object) result.ToString(), (object) Error.String(result)))
    {
      this.Path = path;
      this.Result = result;
    }

    public BankLoadException(string path, string error)
      : base(string.Format("FMOD Studio could not load bank '{0}' : {1}", (object) path, (object) error))
    {
      this.Path = path;
      this.Result = RESULT.ERR_INTERNAL;
    }
  }
}
