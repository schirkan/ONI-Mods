// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.EmitterState
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace YamlDotNet.RepresentationModel
{
  internal class EmitterState
  {
    private readonly HashSet<string> emittedAnchors = new HashSet<string>();

    public HashSet<string> EmittedAnchors => this.emittedAnchors;
  }
}
