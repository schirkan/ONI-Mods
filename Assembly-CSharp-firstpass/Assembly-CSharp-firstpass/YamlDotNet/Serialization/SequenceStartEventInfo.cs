﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.SequenceStartEventInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization
{
  public sealed class SequenceStartEventInfo : ObjectEventInfo
  {
    public SequenceStartEventInfo(IObjectDescriptor source)
      : base(source)
    {
    }

    public bool IsImplicit { get; set; }

    public SequenceStyle Style { get; set; }
  }
}
