// Decompiled with JetBrains decompiler
// Type: ListPool`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

public static class ListPool<ObjectType, PoolIdentifier>
{
  private static ContainerPool<ListPool<ObjectType, PoolIdentifier>.PooledList, PoolIdentifier> pool = new ContainerPool<ListPool<ObjectType, PoolIdentifier>.PooledList, PoolIdentifier>();

  public static ListPool<ObjectType, PoolIdentifier>.PooledList Allocate(
    List<ObjectType> objects)
  {
    ListPool<ObjectType, PoolIdentifier>.PooledList pooledList = ListPool<ObjectType, PoolIdentifier>.pool.Allocate();
    pooledList.AddRange((IEnumerable<ObjectType>) objects);
    return pooledList;
  }

  public static ListPool<ObjectType, PoolIdentifier>.PooledList Allocate() => ListPool<ObjectType, PoolIdentifier>.pool.Allocate();

  private static void Free(
    ListPool<ObjectType, PoolIdentifier>.PooledList list)
  {
    list.Clear();
    ListPool<ObjectType, PoolIdentifier>.pool.Free(list);
  }

  public static ContainerPool GetPool() => (ContainerPool) ListPool<ObjectType, PoolIdentifier>.pool;

  [DebuggerDisplay("Count={Count}")]
  public class PooledList : List<ObjectType>
  {
    public void Recycle() => ListPool<ObjectType, PoolIdentifier>.Free(this);
  }
}
