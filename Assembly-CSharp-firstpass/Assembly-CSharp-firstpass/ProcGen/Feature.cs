// Decompiled with JetBrains decompiler
// Type: ProcGen.Feature
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class Feature
  {
    public string type { get; set; }

    public List<string> tags { get; private set; }

    public List<string> excludesTags { get; private set; }

    public Feature()
    {
      this.tags = new List<string>();
      this.excludesTags = new List<string>();
    }
  }
}
