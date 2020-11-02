// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.GraphML.NodeGraphics
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using System.Xml.Linq;

namespace Satsuma.IO.GraphML
{
  public sealed class NodeGraphics
  {
    private readonly string[] nodeShapeToString = new string[11]
    {
      "rectangle",
      "roundrectangle",
      "ellipse",
      "parallelogram",
      "hexagon",
      "triangle",
      "rectangle3d",
      "octagon",
      "diamond",
      "trapezoid",
      "trapezoid2"
    };

    public double X { get; set; }

    public double Y { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public NodeShape Shape { get; set; }

    public NodeGraphics()
    {
      this.X = this.Y = 0.0;
      this.Width = this.Height = 10.0;
      this.Shape = NodeShape.Rectangle;
    }

    private NodeShape ParseShape(string s) => (NodeShape) Math.Max(0, Array.IndexOf<string>(this.nodeShapeToString, s));

    private string ShapeToGraphML(NodeShape shape) => this.nodeShapeToString[(int) shape];

    public NodeGraphics(XElement xData)
    {
      XElement xelement1 = Utils.ElementLocal(xData, "Geometry");
      if (xelement1 != null)
      {
        this.X = double.Parse(xelement1.Attribute((XName) "x").Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this.Y = double.Parse(xelement1.Attribute((XName) "y").Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this.Width = double.Parse(xelement1.Attribute((XName) "width").Value, (IFormatProvider) CultureInfo.InvariantCulture);
        this.Height = double.Parse(xelement1.Attribute((XName) "height").Value, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      XElement xelement2 = Utils.ElementLocal(xData, nameof (Shape));
      if (xelement2 == null)
        return;
      this.Shape = this.ParseShape(xelement2.Attribute((XName) "type").Value);
    }

    public XElement ToXml() => new XElement((XName) "dummy", (object) new XElement(GraphMLFormat.xmlnsY + "ShapeNode", new object[2]
    {
      (object) new XElement(GraphMLFormat.xmlnsY + "Geometry", new object[4]
      {
        (object) new XAttribute((XName) "x", (object) this.X.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XAttribute((XName) "y", (object) this.Y.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XAttribute((XName) "width", (object) this.Width.ToString((IFormatProvider) CultureInfo.InvariantCulture)),
        (object) new XAttribute((XName) "height", (object) this.Height.ToString((IFormatProvider) CultureInfo.InvariantCulture))
      }),
      (object) new XElement(GraphMLFormat.xmlnsY + "Shape", (object) new XAttribute((XName) "type", (object) this.ShapeToGraphML(this.Shape)))
    }));

    public override string ToString() => this.ToXml().ToString();
  }
}
