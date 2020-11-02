// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.ForwardAnchorNotSupportedException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  public class ForwardAnchorNotSupportedException : YamlException
  {
    public ForwardAnchorNotSupportedException()
    {
    }

    public ForwardAnchorNotSupportedException(string message)
      : base(message)
    {
    }

    public ForwardAnchorNotSupportedException(Mark start, Mark end, string message)
      : base(start, end, message)
    {
    }

    public ForwardAnchorNotSupportedException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
