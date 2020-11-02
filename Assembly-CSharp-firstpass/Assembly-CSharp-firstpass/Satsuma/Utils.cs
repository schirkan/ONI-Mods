// Decompiled with JetBrains decompiler
// Type: Satsuma.Utils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Satsuma
{
  internal static class Utils
  {
    public static double LargestPowerOfTwo(double d)
    {
      long num = BitConverter.DoubleToInt64Bits(d) & 9218868437227405312L;
      if (num == 9218868437227405312L)
        num = 9214364837600034816L;
      return BitConverter.Int64BitsToDouble(num);
    }

    public static V MakeEntry<K, V>(Dictionary<K, V> dict, K key) where V : new()
    {
      V v;
      return dict.TryGetValue(key, out v) ? v : (dict[key] = new V());
    }

    public static void RemoveAll<T>(HashSet<T> set, Func<T, bool> condition)
    {
      foreach (T obj in set.Where<T>(condition).ToList<T>())
        set.Remove(obj);
    }

    public static void RemoveAll<K, V>(Dictionary<K, V> dict, Func<K, bool> condition)
    {
      foreach (K key in dict.Keys.Where<K>(condition).ToList<K>())
        dict.Remove(key);
    }

    public static void RemoveLast<T>(List<T> list, T element) where T : IEquatable<T>
    {
      for (int index = list.Count - 1; index >= 0; --index)
      {
        if (element.Equals(list[index]))
        {
          list.RemoveAt(index);
          break;
        }
      }
    }

    public static IEnumerable<XElement> ElementsLocal(
      XElement xParent,
      string localName)
    {
      return xParent.Elements().Where<XElement>((Func<XElement, bool>) (x => x.Name.LocalName == localName));
    }

    public static XElement ElementLocal(XElement xParent, string localName) => Utils.ElementsLocal(xParent, localName).FirstOrDefault<XElement>();
  }
}
