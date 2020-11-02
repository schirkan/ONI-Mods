// Decompiled with JetBrains decompiler
// Type: ProcGen.MobReference
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace ProcGen
{
  [Serializable]
  public class MobReference
  {
    public string type { get; private set; }

    public MinMax count { get; private set; }
  }
}
