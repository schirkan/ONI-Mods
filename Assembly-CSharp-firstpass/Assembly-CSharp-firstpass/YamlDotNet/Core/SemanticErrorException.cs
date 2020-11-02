// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.SemanticErrorException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  public class SemanticErrorException : YamlException
  {
    public SemanticErrorException()
    {
    }

    public SemanticErrorException(string message)
      : base(message)
    {
    }

    public SemanticErrorException(Mark start, Mark end, string message)
      : base(start, end, message)
    {
    }

    public SemanticErrorException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
