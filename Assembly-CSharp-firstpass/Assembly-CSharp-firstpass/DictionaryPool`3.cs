// Decompiled with JetBrains decompiler
// Type: DictionaryPool`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Diagnostics;

public static class DictionaryPool<KeyType, ObjectType, PoolIdentifier>
{
  private static ContainerPool<DictionaryPool<KeyType, ObjectType, PoolIdentifier>.PooledDictionary, PoolIdentifier> pool = new ContainerPool<DictionaryPool<KeyType, ObjectType, PoolIdentifier>.PooledDictionary, PoolIdentifier>();

  public static DictionaryPool<KeyType, ObjectType, PoolIdentifier>.PooledDictionary Allocate() => DictionaryPool<KeyType, ObjectType, PoolIdentifier>.pool.Allocate();

  private static void Free(
    DictionaryPool<KeyType, ObjectType, PoolIdentifier>.PooledDictionary dictionary)
  {
    dictionary.Clear();
    DictionaryPool<KeyType, ObjectType, PoolIdentifier>.pool.Free(dictionary);
  }

  public static ContainerPool GetPool() => (ContainerPool) DictionaryPool<KeyType, ObjectType, PoolIdentifier>.pool;

  [DebuggerDisplay("Count={Count}")]
  public class PooledDictionary : Dictionary<KeyType, ObjectType>
  {
    public void Recycle() => DictionaryPool<KeyType, ObjectType, PoolIdentifier>.Free(this);
  }
}
