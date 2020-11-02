// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.GraphML.StandardProperty`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using System.Xml.Linq;

namespace Satsuma.IO.GraphML
{
  public sealed class StandardProperty<T> : DictionaryProperty<T>
  {
    private static readonly StandardType Type = StandardProperty<T>.ParseType(typeof (T));
    private static readonly string TypeString = StandardProperty<T>.TypeToGraphML(StandardProperty<T>.Type);

    public StandardProperty()
    {
    }

    internal StandardProperty(XElement xKey)
      : this()
    {
      XAttribute xattribute = xKey.Attribute((XName) "attr.type");
      if (xattribute == null || xattribute.Value != StandardProperty<T>.TypeString)
        throw new ArgumentException("Key not compatible with property.");
      this.LoadFromKeyElement(xKey);
    }

    private static StandardType ParseType(System.Type t)
    {
      if (t == typeof (bool))
        return StandardType.Bool;
      if (t == typeof (double))
        return StandardType.Double;
      if (t == typeof (float))
        return StandardType.Float;
      if (t == typeof (int))
        return StandardType.Int;
      if (t == typeof (long))
        return StandardType.Long;
      if (t == typeof (string))
        return StandardType.String;
      throw new ArgumentException("Invalid type for a standard GraphML property.");
    }

    private static string TypeToGraphML(StandardType type)
    {
      switch (type)
      {
        case StandardType.Bool:
          return "boolean";
        case StandardType.Double:
          return "double";
        case StandardType.Float:
          return "float";
        case StandardType.Int:
          return "int";
        case StandardType.Long:
          return "long";
        default:
          return "string";
      }
    }

    private static object ParseValue(string value)
    {
      switch (StandardProperty<T>.Type)
      {
        case StandardType.Bool:
          return (object) (value == "true");
        case StandardType.Double:
          return (object) double.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        case StandardType.Float:
          return (object) float.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        case StandardType.Int:
          return (object) int.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        case StandardType.Long:
          return (object) long.Parse(value, (IFormatProvider) CultureInfo.InvariantCulture);
        default:
          return (object) value;
      }
    }

    public override XElement GetKeyElement()
    {
      XElement keyElement = base.GetKeyElement();
      keyElement.SetAttributeValue((XName) "attr.type", (object) StandardProperty<T>.TypeString);
      return keyElement;
    }

    protected override T ReadValue(XElement x) => (T) StandardProperty<T>.ParseValue(x.Value);

    protected override XElement WriteValue(T value) => new XElement((XName) "dummy", (object) value.ToString());
  }
}
