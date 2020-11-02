// Decompiled with JetBrains decompiler
// Type: HashSetPool`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

public static class HashSetPool<ObjectType, PoolIdentifier>
{
  private static ContainerPool<HashSetPool<ObjectType, PoolIdentifier>.PooledHashSet, PoolIdentifier> pool = new ContainerPool<HashSetPool<ObjectType, PoolIdentifier>.PooledHashSet, PoolIdentifier>();

  public static HashSetPool<ObjectType, PoolIdentifier>.PooledHashSet Allocate() => HashSetPool<ObjectType, PoolIdentifier>.pool.Allocate();

  private static void Free(
    HashSetPool<ObjectType, PoolIdentifier>.PooledHashSet hash_set)
  {
    hash_set.Clear();
    HashSetPool<ObjectType, PoolIdentifier>.pool.Free(hash_set);
  }

  public static ContainerPool GetPool() => (ContainerPool) HashSetPool<ObjectType, PoolIdentifier>.pool;

  [DebuggerDisplay("Count={Count}")]
  public class PooledHashSet : HashSet<ObjectType>
  {
    public void Recycle() => HashSetPool<ObjectType, PoolIdentifier>.Free(this);
  }
}
