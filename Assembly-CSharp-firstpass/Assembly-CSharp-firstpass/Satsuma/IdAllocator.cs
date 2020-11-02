// Decompiled with JetBrains decompiler
// Type: Satsuma.IdAllocator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace Satsuma
{
  internal abstract class IdAllocator
  {
    private long randomSeed;
    private long lastAllocated;

    public IdAllocator()
    {
      this.randomSeed = 205891132094649L;
      this.Rewind();
    }

    private long Random() => this.randomSeed *= 3L;

    protected abstract bool IsAllocated(long id);

    public void Rewind() => this.lastAllocated = 0L;

    public long Allocate()
    {
      long id = this.lastAllocated + 1L;
      int num = 0;
      while (true)
      {
        do
        {
          if (id == 0L)
            id = 1L;
          if (!this.IsAllocated(id))
          {
            this.lastAllocated = id;
            return id;
          }
          ++id;
          ++num;
        }
        while (num < 100);
        id = this.Random();
        num = 0;
      }
    }
  }
}
