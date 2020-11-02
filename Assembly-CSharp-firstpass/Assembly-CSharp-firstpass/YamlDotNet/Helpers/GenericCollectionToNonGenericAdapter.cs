// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Helpers.GenericCollectionToNonGenericAdapter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Reflection;

namespace YamlDotNet.Helpers
{
  internal sealed class GenericCollectionToNonGenericAdapter : IList, ICollection, IEnumerable
  {
    private readonly object genericCollection;
    private readonly MethodInfo addMethod;
    private readonly MethodInfo indexerSetter;
    private readonly MethodInfo countGetter;

    public GenericCollectionToNonGenericAdapter(
      object genericCollection,
      Type genericCollectionType,
      Type genericListType)
    {
      this.genericCollection = genericCollection;
      this.addMethod = genericCollectionType.GetPublicInstanceMethod("Add");
      this.countGetter = genericCollectionType.GetPublicProperty(nameof (Count)).GetGetMethod();
      if (!(genericListType != (Type) null))
        return;
      this.indexerSetter = genericListType.GetPublicProperty(nameof (Item)).GetSetMethod();
    }

    public int Add(object value)
    {
      int num = (int) this.countGetter.Invoke(this.genericCollection, (object[]) null);
      this.addMethod.Invoke(this.genericCollection, new object[1]
      {
        value
      });
      return num;
    }

    public void Clear() => throw new NotSupportedException();

    public bool Contains(object value) => throw new NotSupportedException();

    public int IndexOf(object value) => throw new NotSupportedException();

    public void Insert(int index, object value) => throw new NotSupportedException();

    public bool IsFixedSize => throw new NotSupportedException();

    public bool IsReadOnly => throw new NotSupportedException();

    public void Remove(object value) => throw new NotSupportedException();

    public void RemoveAt(int index) => throw new NotSupportedException();

    public object this[int index]
    {
      get => throw new NotSupportedException();
      set => this.indexerSetter.Invoke(this.genericCollection, new object[2]
      {
        (object) index,
        value
      });
    }

    public void CopyTo(Array array, int index) => throw new NotSupportedException();

    public int Count => throw new NotSupportedException();

    public bool IsSynchronized => throw new NotSupportedException();

    public object SyncRoot => throw new NotSupportedException();

    public IEnumerator GetEnumerator() => ((IEnumerable) this.genericCollection).GetEnumerator();
  }
}
