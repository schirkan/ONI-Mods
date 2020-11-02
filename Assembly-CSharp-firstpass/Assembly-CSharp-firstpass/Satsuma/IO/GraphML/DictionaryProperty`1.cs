// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.GraphML.DictionaryProperty`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.Xml.Linq;

namespace Satsuma.IO.GraphML
{
  public abstract class DictionaryProperty<T> : GraphMLProperty, IClearable
  {
    public bool HasDefaultValue { get; set; }

    public T DefaultValue { get; set; }

    public Dictionary<object, T> Values { get; private set; }

    protected DictionaryProperty()
    {
      this.HasDefaultValue = false;
      this.Values = new Dictionary<object, T>();
    }

    public void Clear()
    {
      this.HasDefaultValue = false;
      this.Values.Clear();
    }

    public bool TryGetValue(object key, out T result)
    {
      if (this.Values.TryGetValue(key, out result))
        return true;
      if (this.HasDefaultValue)
      {
        result = this.DefaultValue;
        return true;
      }
      result = default (T);
      return false;
    }

    public override void ReadData(XElement x, object key)
    {
      if (x == null)
      {
        if (key == null)
          this.HasDefaultValue = false;
        else
          this.Values.Remove(key);
      }
      else
      {
        T obj = this.ReadValue(x);
        if (key == null)
        {
          this.HasDefaultValue = true;
          this.DefaultValue = obj;
        }
        else
          this.Values[key] = obj;
      }
    }

    public override XElement WriteData(object key)
    {
      T obj;
      return key == null ? (!this.HasDefaultValue ? (XElement) null : this.WriteValue(this.DefaultValue)) : (!this.Values.TryGetValue(key, out obj) ? (XElement) null : this.WriteValue(obj));
    }

    protected abstract T ReadValue(XElement x);

    protected abstract XElement WriteValue(T value);
  }
}
