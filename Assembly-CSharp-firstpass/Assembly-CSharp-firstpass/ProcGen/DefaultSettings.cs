// Decompiled with JetBrains decompiler
// Type: ProcGen.DefaultSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class DefaultSettings
  {
    public BaseLocation baseData { get; private set; }

    public Dictionary<string, object> data { get; private set; }

    public List<string> defaultMoveTags { get; private set; }

    public List<string> overworldAddTags { get; private set; }
  }
}
