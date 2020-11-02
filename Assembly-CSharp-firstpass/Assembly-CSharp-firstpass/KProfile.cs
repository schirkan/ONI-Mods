// Decompiled with JetBrains decompiler
// Type: KProfile
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

public class KProfile : IDisposable
{
  private string name;

  public KProfile(string name, string group = "Game") => this.name = name;

  public void Dispose()
  {
  }
}
