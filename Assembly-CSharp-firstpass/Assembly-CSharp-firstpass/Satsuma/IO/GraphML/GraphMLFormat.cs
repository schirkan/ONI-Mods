// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.GraphML.GraphMLFormat
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Satsuma.IO.GraphML
{
  public sealed class GraphMLFormat
  {
    internal static readonly XNamespace xmlns = (XNamespace) "http://graphml.graphdrawing.org/xmlns";
    private static readonly XNamespace xmlnsXsi = (XNamespace) "http://www.w3.org/2001/XMLSchema-instance";
    internal static readonly XNamespace xmlnsY = (XNamespace) "http://www.yworks.com/xml/graphml";
    private static readonly XNamespace xmlnsYed = (XNamespace) "http://www.yworks.com/xml/yed/3";
    private const string xsiSchemaLocation = "http://graphml.graphdrawing.org/xmlns\nhttp://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd";
    private readonly List<Func<XElement, GraphMLProperty>> PropertyLoaders;

    public IGraph Graph { get; set; }

    public IList<GraphMLProperty> Properties { get; private set; }

    public GraphMLFormat()
    {
      this.Properties = (IList<GraphMLProperty>) new List<GraphMLProperty>();
      this.PropertyLoaders = new List<Func<XElement, GraphMLProperty>>()
      {
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new StandardProperty<bool>(x)),
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new StandardProperty<double>(x)),
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new StandardProperty<float>(x)),
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new StandardProperty<int>(x)),
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new StandardProperty<long>(x)),
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new StandardProperty<string>(x)),
        (Func<XElement, GraphMLProperty>) (x => (GraphMLProperty) new NodeGraphicsProperty(x))
      };
    }

    public void RegisterPropertyLoader(Func<XElement, GraphMLProperty> loader) => this.PropertyLoaders.Add(loader);

    private static void ReadProperties(
      Dictionary<string, GraphMLProperty> propertyById,
      XElement x,
      object obj)
    {
      foreach (XElement xelement in Utils.ElementsLocal(x, "data"))
      {
        GraphMLProperty graphMlProperty;
        if (propertyById.TryGetValue(xelement.Attribute((XName) "key").Value, out graphMlProperty))
          graphMlProperty.ReadData(x, obj);
      }
    }

    public void Load(XDocument doc)
    {
      if (this.Graph == null)
        this.Graph = (IGraph) new CustomGraph();
      IBuildableGraph graph = (IBuildableGraph) this.Graph;
      graph.Clear();
      XElement root = doc.Root;
      this.Properties.Clear();
      Dictionary<string, GraphMLProperty> propertyById = new Dictionary<string, GraphMLProperty>();
      foreach (XElement xelement in Utils.ElementsLocal(root, "key"))
      {
        foreach (Func<XElement, GraphMLProperty> propertyLoader in this.PropertyLoaders)
        {
          try
          {
            GraphMLProperty graphMlProperty = propertyLoader(xelement);
            this.Properties.Add(graphMlProperty);
            propertyById[graphMlProperty.Id] = graphMlProperty;
            break;
          }
          catch (ArgumentException ex)
          {
          }
        }
      }
      XElement xelement1 = Utils.ElementLocal(root, "graph");
      Directedness directedness1 = xelement1.Attribute((XName) "edgedefault").Value == "directed" ? Directedness.Directed : Directedness.Undirected;
      GraphMLFormat.ReadProperties(propertyById, xelement1, (object) this.Graph);
      Dictionary<string, Satsuma.Node> dictionary = new Dictionary<string, Satsuma.Node>();
      foreach (XElement x in Utils.ElementsLocal(xelement1, "node"))
      {
        Satsuma.Node node = graph.AddNode();
        dictionary[x.Attribute((XName) "id").Value] = node;
        GraphMLFormat.ReadProperties(propertyById, x, (object) node);
      }
      foreach (XElement x in Utils.ElementsLocal(xelement1, "edge"))
      {
        Satsuma.Node u = dictionary[x.Attribute((XName) "source").Value];
        Satsuma.Node v = dictionary[x.Attribute((XName) "target").Value];
        Directedness directedness2 = directedness1;
        XAttribute xattribute = x.Attribute((XName) "directed");
        if (xattribute != null)
          directedness2 = xattribute.Value == "true" ? Directedness.Directed : Directedness.Undirected;
        Arc arc = graph.AddArc(u, v, directedness2);
        GraphMLFormat.ReadProperties(propertyById, x, (object) arc);
      }
    }

    public void Load(XmlReader xml) => this.Load(XDocument.Load(xml));

    public void Load(TextReader reader)
    {
      using (XmlReader xml = XmlReader.Create(reader))
        this.Load(xml);
    }

    public void Load(string filename)
    {
      using (StreamReader streamReader = new StreamReader(filename))
        this.Load((TextReader) streamReader);
    }

    private void DefinePropertyValues(XmlWriter xml, object obj)
    {
      foreach (GraphMLProperty property in (IEnumerable<GraphMLProperty>) this.Properties)
      {
        XElement xelement = property.WriteData(obj);
        if (xelement != null)
        {
          xelement.Name = GraphMLFormat.xmlns + "data";
          xelement.SetAttributeValue((XName) "key", (object) property.Id);
          xelement.WriteTo(xml);
        }
      }
    }

    private void Save(XmlWriter xml)
    {
      xml.WriteStartDocument();
      xml.WriteStartElement("graphml", GraphMLFormat.xmlns.NamespaceName);
      xml.WriteAttributeString("xmlns", "xsi", (string) null, GraphMLFormat.xmlnsXsi.NamespaceName);
      xml.WriteAttributeString("xmlns", "y", (string) null, GraphMLFormat.xmlnsY.NamespaceName);
      xml.WriteAttributeString("xmlns", "yed", (string) null, GraphMLFormat.xmlnsYed.NamespaceName);
      xml.WriteAttributeString("xsi", "schemaLocation", (string) null, "http://graphml.graphdrawing.org/xmlns\nhttp://graphml.graphdrawing.org/xmlns/1.0/graphml.xsd");
      for (int index = 0; index < this.Properties.Count; ++index)
      {
        GraphMLProperty property = this.Properties[index];
        property.Id = "d" + (object) index;
        property.GetKeyElement().WriteTo(xml);
      }
      xml.WriteStartElement("graph", GraphMLFormat.xmlns.NamespaceName);
      xml.WriteAttributeString("id", "G");
      xml.WriteAttributeString("edgedefault", "directed");
      xml.WriteAttributeString("parse.nodes", this.Graph.NodeCount().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      xml.WriteAttributeString("parse.edges", this.Graph.ArcCount().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      xml.WriteAttributeString("parse.order", "nodesfirst");
      this.DefinePropertyValues(xml, (object) this.Graph);
      foreach (Satsuma.Node node in this.Graph.Nodes())
      {
        xml.WriteStartElement("node", GraphMLFormat.xmlns.NamespaceName);
        xml.WriteAttributeString("id", node.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        this.DefinePropertyValues(xml, (object) node);
        xml.WriteEndElement();
      }
      foreach (Arc arc in this.Graph.Arcs())
      {
        xml.WriteStartElement("edge", GraphMLFormat.xmlns.NamespaceName);
        XmlWriter xmlWriter1 = xml;
        long id = arc.Id;
        string str1 = id.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        xmlWriter1.WriteAttributeString("id", str1);
        if (this.Graph.IsEdge(arc))
          xml.WriteAttributeString("directed", "false");
        XmlWriter xmlWriter2 = xml;
        id = this.Graph.U(arc).Id;
        string str2 = id.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        xmlWriter2.WriteAttributeString("source", str2);
        XmlWriter xmlWriter3 = xml;
        id = this.Graph.V(arc).Id;
        string str3 = id.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        xmlWriter3.WriteAttributeString("target", str3);
        this.DefinePropertyValues(xml, (object) arc);
        xml.WriteEndElement();
      }
      xml.WriteEndElement();
      xml.WriteEndElement();
    }

    public void Save(TextWriter writer)
    {
      using (XmlWriter xml = XmlWriter.Create(writer))
        this.Save(xml);
    }

    public void Save(string filename)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
        this.Save((TextWriter) streamWriter);
    }
  }
}
