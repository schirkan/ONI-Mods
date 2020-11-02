// Decompiled with JetBrains decompiler
// Type: ResourceTreeLoader`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ResourceTreeLoader<T> : ResourceLoader<T> where T : ResourceTreeNode, new()
{
  public ResourceTreeLoader(TextAsset file)
    : base(file)
  {
  }

  public override void Load(TextAsset file)
  {
    Dictionary<string, ResourceTreeNode> dictionary = new Dictionary<string, ResourceTreeNode>();
    using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(file.text)))
    {
      while (xmlReader.ReadToFollowing("node"))
      {
        xmlReader.MoveToFirstAttribute();
        string key = xmlReader.Value;
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 40f;
        float num4 = 20f;
        if (xmlReader.ReadToFollowing("Geometry"))
        {
          xmlReader.MoveToAttribute("x");
          num1 = float.Parse(xmlReader.Value);
          xmlReader.MoveToAttribute("y");
          num2 = -float.Parse(xmlReader.Value);
          xmlReader.MoveToAttribute("width");
          num3 = float.Parse(xmlReader.Value);
          xmlReader.MoveToAttribute("height");
          num4 = float.Parse(xmlReader.Value);
        }
        Debug.Assert((double) num3 != 0.0 && (double) num4 != 0.0, (object) "Error parsing GRAPHML");
        if (xmlReader.ReadToFollowing("NodeLabel"))
        {
          string str = xmlReader.ReadString();
          T obj = new T();
          obj.Id = str;
          obj.Name = str;
          obj.nodeX = num1;
          obj.nodeY = num2;
          obj.width = num3;
          obj.height = num4;
          dictionary[key] = (ResourceTreeNode) obj;
          this.resources.Add(obj);
        }
      }
    }
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(file.text);
    foreach (XmlNode selectNode in xmlDocument.DocumentElement.SelectNodes("/graphml/graph/edge"))
    {
      ResourceTreeNode source = (ResourceTreeNode) null;
      dictionary.TryGetValue(selectNode.Attributes["source"].Value, out source);
      ResourceTreeNode target = (ResourceTreeNode) null;
      dictionary.TryGetValue(selectNode.Attributes["target"].Value, out target);
      if (source != null && target != null)
      {
        source.references.Add(target);
        XmlNode xmlNode = (XmlNode) null;
        foreach (XmlNode childNode in selectNode.ChildNodes)
        {
          if (childNode.HasChildNodes)
          {
            xmlNode = childNode.FirstChild;
            break;
          }
        }
        ResourceTreeNode.Edge.EdgeType edgeType = (ResourceTreeNode.Edge.EdgeType) Enum.Parse(typeof (ResourceTreeNode.Edge.EdgeType), xmlNode.Name);
        ResourceTreeNode.Edge edge = new ResourceTreeNode.Edge(source, target, edgeType);
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          if (!(childNode.Name != "Path"))
          {
            edge.sourceOffset = new Vector2f(float.Parse(childNode.Attributes["sx"].Value), -float.Parse(childNode.Attributes["sy"].Value));
            edge.targetOffset = new Vector2f(float.Parse(childNode.Attributes["tx"].Value), -float.Parse(childNode.Attributes["ty"].Value));
            IEnumerator enumerator = childNode.ChildNodes.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                XmlNode current = (XmlNode) enumerator.Current;
                Vector2f point = new Vector2f(float.Parse(current.Attributes["x"].Value), -float.Parse(current.Attributes["y"].Value));
                edge.AddToPath(point);
              }
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
          }
        }
        source.edges.Add(edge);
      }
    }
  }
}
