// Decompiled with JetBrains decompiler
// Type: FMODUnity.EventNotFoundException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace FMODUnity
{
  public class EventNotFoundException : Exception
  {
    public Guid Guid;
    public string Path;

    public EventNotFoundException(string path)
      : base("FMOD Studio event not found '" + path + "'")
      => this.Path = path;

    public EventNotFoundException(Guid guid)
      : base("FMOD Studio event not found " + guid.ToString("b") ?? "")
      => this.Guid = guid;
  }
}
