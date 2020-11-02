// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.YamlException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  public class YamlException : Exception
  {
    public Mark Start { get; private set; }

    public Mark End { get; private set; }

    public YamlException()
    {
    }

    public YamlException(string message)
      : base(message)
    {
    }

    public YamlException(Mark start, Mark end, string message)
      : this(start, end, message, (Exception) null)
    {
    }

    public YamlException(Mark start, Mark end, string message, Exception innerException)
      : base(string.Format("({0}) - ({1}): {2}", (object) start, (object) end, (object) message), innerException)
    {
      this.Start = start;
      this.End = end;
    }

    public YamlException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
