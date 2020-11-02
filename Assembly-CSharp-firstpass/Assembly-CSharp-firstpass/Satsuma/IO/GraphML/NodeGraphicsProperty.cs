// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.GraphML.NodeGraphicsProperty
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Xml.Linq;

namespace Satsuma.IO.GraphML
{
  public sealed class NodeGraphicsProperty : DictionaryProperty<NodeGraphics>
  {
    public NodeGraphicsProperty() => this.Domain = PropertyDomain.Node;

    internal NodeGraphicsProperty(XElement xKey)
      : this()
    {
      XAttribute xattribute = xKey.Attribute((XName) "yfiles.type");
      if (xattribute == null || xattribute.Value != "nodegraphics")
        throw new ArgumentException("Key not compatible with property.");
      this.LoadFromKeyElement(xKey);
    }

    public override XElement GetKeyElement()
    {
      XElement keyElement = base.GetKeyElement();
      keyElement.SetAttributeValue((XName) "yfiles.type", (object) "nodegraphics");
      return keyElement;
    }

    protected override NodeGraphics ReadValue(XElement x) => new NodeGraphics(x);

    protected override XElement WriteValue(NodeGraphics value) => value.ToXml();
  }
}
