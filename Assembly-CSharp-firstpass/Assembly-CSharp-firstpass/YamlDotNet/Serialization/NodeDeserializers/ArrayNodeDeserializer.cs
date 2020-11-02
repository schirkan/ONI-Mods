// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.NodeDeserializers.ArrayNodeDeserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
  public sealed class ArrayNodeDeserializer : INodeDeserializer
  {
    bool INodeDeserializer.Deserialize(
      IParser parser,
      Type expectedType,
      Func<IParser, Type, object> nestedObjectDeserializer,
      out object value)
    {
      if (!expectedType.IsArray)
      {
        value = (object) false;
        return false;
      }
      Type elementType = expectedType.GetElementType();
      ArrayNodeDeserializer.ArrayList arrayList = new ArrayNodeDeserializer.ArrayList();
      CollectionNodeDeserializer.DeserializeHelper(elementType, parser, nestedObjectDeserializer, (IList) arrayList, true);
      Array instance = Array.CreateInstance(elementType, arrayList.Count);
      arrayList.CopyTo(instance, 0);
      value = (object) instance;
      return true;
    }

    private sealed class ArrayList : IList, ICollection, IEnumerable
    {
      private object[] data;
      private int count;

      public ArrayList() => this.Clear();

      public int Add(object value)
      {
        if (this.count == this.data.Length)
          Array.Resize<object>(ref this.data, this.data.Length * 2);
        this.data[this.count] = value;
        return this.count++;
      }

      public void Clear()
      {
        this.data = new object[10];
        this.count = 0;
      }

      public bool Contains(object value) => throw new NotSupportedException();

      public int IndexOf(object value) => throw new NotSupportedException();

      public void Insert(int index, object value) => throw new NotSupportedException();

      public bool IsFixedSize => false;

      public bool IsReadOnly => false;

      public void Remove(object value) => throw new NotSupportedException();

      public void RemoveAt(int index) => throw new NotSupportedException();

      public object this[int index]
      {
        get => this.data[index];
        set => this.data[index] = value;
      }

      public void CopyTo(Array array, int index) => Array.Copy((Array) this.data, 0, array, index, this.count);

      public int Count => this.count;

      public bool IsSynchronized => false;

      public object SyncRoot => (object) this.data;

      public IEnumerator GetEnumerator()
      {
        for (int i = 0; i < this.count; ++i)
          yield return this.data[i];
      }
    }
  }
}
