﻿// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.AnchorNotFoundException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  public class AnchorNotFoundException : YamlException
  {
    public AnchorNotFoundException()
    {
    }

    public AnchorNotFoundException(string message)
      : base(message)
    {
    }

    public AnchorNotFoundException(Mark start, Mark end, string message)
      : base(start, end, message)
    {
    }

    public AnchorNotFoundException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
