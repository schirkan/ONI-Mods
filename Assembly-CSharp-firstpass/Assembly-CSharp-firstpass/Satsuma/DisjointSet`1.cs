// Decompiled with JetBrains decompiler
// Type: Satsuma.DisjointSet`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

namespace Satsuma
{
  public sealed class DisjointSet<T> : IDisjointSet<T>, IReadOnlyDisjointSet<T>, IClearable
  {
    private readonly Dictionary<T, T> parent;
    private readonly Dictionary<T, T> next;
    private readonly Dictionary<T, T> last;
    private readonly List<T> tmpList;

    public DisjointSet()
    {
      this.parent = new Dictionary<T, T>();
      this.next = new Dictionary<T, T>();
      this.last = new Dictionary<T, T>();
      this.tmpList = new List<T>();
    }

    public void Clear()
    {
      this.parent.Clear();
      this.next.Clear();
      this.last.Clear();
    }

    public DisjointSetSet<T> WhereIs(T element)
    {
      T obj;
      for (; this.parent.TryGetValue(element, out obj); element = obj)
        this.tmpList.Add(element);
      foreach (T tmp in this.tmpList)
        this.parent[tmp] = element;
      this.tmpList.Clear();
      return new DisjointSetSet<T>(element);
    }

    private T GetLast(T x)
    {
      T obj;
      return this.last.TryGetValue(x, out obj) ? obj : x;
    }

    public DisjointSetSet<T> Union(DisjointSetSet<T> a, DisjointSetSet<T> b)
    {
      T representative1 = a.Representative;
      T representative2 = b.Representative;
      if (!representative1.Equals((object) representative2))
      {
        this.parent[representative1] = representative2;
        this.next[this.GetLast(representative2)] = representative1;
        this.last[representative2] = this.GetLast(representative1);
      }
      return b;
    }

    public IEnumerable<T> Elements(DisjointSetSet<T> aSet)
    {
      T element = aSet.Representative;
      do
      {
        yield return element;
      }
      while (this.next.TryGetValue(element, out element));
    }
  }
}
