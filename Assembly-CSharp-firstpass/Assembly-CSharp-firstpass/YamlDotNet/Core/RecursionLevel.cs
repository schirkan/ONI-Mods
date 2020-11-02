// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.RecursionLevel
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Core
{
  internal class RecursionLevel
  {
    private int current;

    public int Maximum { get; private set; }

    public RecursionLevel(int maximum) => this.Maximum = maximum;

    public void Increment()
    {
      if (!this.TryIncrement())
        throw new MaximumRecursionLevelReachedException();
    }

    public bool TryIncrement()
    {
      if (this.current >= this.Maximum)
        return false;
      ++this.current;
      return true;
    }

    public void Decrement()
    {
      if (this.current == 0)
        throw new InvalidOperationException("Attempted to decrement RecursionLevel to a negative value");
      --this.current;
    }
  }
}
