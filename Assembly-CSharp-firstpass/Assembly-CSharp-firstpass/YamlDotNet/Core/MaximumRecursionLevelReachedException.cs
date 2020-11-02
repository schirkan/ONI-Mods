// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.MaximumRecursionLevelReachedException
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  [Serializable]
  public class MaximumRecursionLevelReachedException : YamlException
  {
    public MaximumRecursionLevelReachedException()
    {
    }

    public MaximumRecursionLevelReachedException(string message)
      : base(message)
    {
    }

    public MaximumRecursionLevelReachedException(Mark start, Mark end, string message)
      : base(start, end, message)
    {
    }

    public MaximumRecursionLevelReachedException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
