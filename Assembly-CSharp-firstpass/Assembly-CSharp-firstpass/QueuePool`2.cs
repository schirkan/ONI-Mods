// Decompiled with JetBrains decompiler
// Type: QueuePool`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

public static class QueuePool<ObjectType, PoolIdentifier>
{
  private static ContainerPool<QueuePool<ObjectType, PoolIdentifier>.PooledQueue, PoolIdentifier> pool = new ContainerPool<QueuePool<ObjectType, PoolIdentifier>.PooledQueue, PoolIdentifier>();

  public static QueuePool<ObjectType, PoolIdentifier>.PooledQueue Allocate() => QueuePool<ObjectType, PoolIdentifier>.pool.Allocate();

  private static void Free(
    QueuePool<ObjectType, PoolIdentifier>.PooledQueue queue)
  {
    queue.Clear();
    QueuePool<ObjectType, PoolIdentifier>.pool.Free(queue);
  }

  public static ContainerPool GetPool() => (ContainerPool) QueuePool<ObjectType, PoolIdentifier>.pool;

  [DebuggerDisplay("Count={Count}")]
  public class PooledQueue : Queue<ObjectType>
  {
    public void Recycle() => QueuePool<ObjectType, PoolIdentifier>.Free(this);
  }
}
