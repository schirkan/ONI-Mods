// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Tokens.FlowSequenceStart
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core.Tokens
{
  [Serializable]
  public class FlowSequenceStart : Token
  {
    public FlowSequenceStart()
      : this(Mark.Empty, Mark.Empty)
    {
    }

    public FlowSequenceStart(Mark start, Mark end)
      : base(start, end)
    {
    }
  }
}
