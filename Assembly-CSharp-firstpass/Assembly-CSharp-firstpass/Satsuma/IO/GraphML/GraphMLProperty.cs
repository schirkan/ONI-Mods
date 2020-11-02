// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.GraphML.GraphMLProperty
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Xml.Linq;

namespace Satsuma.IO.GraphML
{
  public abstract class GraphMLProperty
  {
    public string Name { get; set; }

    public PropertyDomain Domain { get; set; }

    public string Id { get; set; }

    protected GraphMLProperty() => this.Domain = PropertyDomain.All;

    protected static string DomainToGraphML(PropertyDomain domain)
    {
      switch (domain)
      {
        case PropertyDomain.Node:
          return "node";
        case PropertyDomain.Arc:
          return "arc";
        case PropertyDomain.Graph:
          return "graph";
        default:
          return "all";
      }
    }

    protected static PropertyDomain ParseDomain(string s)
    {
      if (s == "node")
        return PropertyDomain.Node;
      if (s == "edge")
        return PropertyDomain.Arc;
      return s == "graph" ? PropertyDomain.Graph : PropertyDomain.All;
    }

    protected virtual void LoadFromKeyElement(XElement xKey)
    {
      XAttribute xattribute = xKey.Attribute((XName) "attr.name");
      this.Name = xattribute == null ? (string) null : xattribute.Value;
      this.Domain = GraphMLProperty.ParseDomain(xKey.Attribute((XName) "for").Value);
      this.Id = xKey.Attribute((XName) "id").Value;
      this.ReadData(Utils.ElementLocal(xKey, "default"), (object) null);
    }

    public virtual XElement GetKeyElement()
    {
      XElement xelement1 = new XElement(GraphMLFormat.xmlns + "key");
      xelement1.SetAttributeValue((XName) "attr.name", (object) this.Name);
      xelement1.SetAttributeValue((XName) "for", (object) GraphMLProperty.DomainToGraphML(this.Domain));
      xelement1.SetAttributeValue((XName) "id", (object) this.Id);
      XElement xelement2 = this.WriteData((object) null);
      if (xelement2 != null)
      {
        xelement2.Name = GraphMLFormat.xmlns + "default";
        xelement1.Add((object) xelement2);
      }
      return xelement1;
    }

    public abstract void ReadData(XElement x, object key);

    public abstract XElement WriteData(object key);
  }
}
