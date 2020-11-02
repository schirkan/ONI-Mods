// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.LemonGraphFormat
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Satsuma.IO
{
  public sealed class LemonGraphFormat
  {
    public IGraph Graph { get; set; }

    public Dictionary<string, Dictionary<Satsuma.Node, string>> NodeMaps { get; private set; }

    public Dictionary<string, Dictionary<Arc, string>> ArcMaps { get; private set; }

    public Dictionary<string, string> Attributes { get; private set; }

    public LemonGraphFormat()
    {
      this.NodeMaps = new Dictionary<string, Dictionary<Satsuma.Node, string>>();
      this.ArcMaps = new Dictionary<string, Dictionary<Arc, string>>();
      this.Attributes = new Dictionary<string, string>();
    }

    private static string Escape(string s)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char ch in s)
      {
        switch (ch)
        {
          case '\t':
            stringBuilder.Append("\\t");
            break;
          case '\n':
            stringBuilder.Append("\\n");
            break;
          case '\r':
            stringBuilder.Append("\\r");
            break;
          case '"':
            stringBuilder.Append("\\\"");
            break;
          case '\\':
            stringBuilder.Append("\\\\");
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      return stringBuilder.ToString();
    }

    private static string Unescape(string s)
    {
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      foreach (char ch in s)
      {
        if (flag)
        {
          switch (ch)
          {
            case 'n':
              stringBuilder.Append('\n');
              break;
            case 'r':
              stringBuilder.Append('\r');
              break;
            case 't':
              stringBuilder.Append('\t');
              break;
            default:
              stringBuilder.Append(ch);
              break;
          }
          flag = false;
        }
        else
        {
          flag = ch == '\\';
          if (!flag)
            stringBuilder.Append(ch);
        }
      }
      return stringBuilder.ToString();
    }

    public void Load(TextReader reader, Directedness? directedness)
    {
      if (this.Graph == null)
        this.Graph = (IGraph) new CustomGraph();
      IBuildableGraph graph = (IBuildableGraph) this.Graph;
      graph.Clear();
      this.NodeMaps.Clear();
      Dictionary<string, Satsuma.Node> dictionary = new Dictionary<string, Satsuma.Node>();
      this.ArcMaps.Clear();
      this.Attributes.Clear();
      Regex regex = new Regex("\\s*(?:(\"(?:\\\"|.)*\")|(\\S+))\\s*", RegexOptions.None);
      string str1 = "";
      Directedness directedness1 = Directedness.Directed;
      bool flag = false;
      List<string> stringList = (List<string>) null;
      int num = -1;
      while (true)
      {
        string input;
        do
        {
          string str2 = reader.ReadLine();
          if (str2 != null)
            input = str2.Trim();
          else
            goto label_33;
        }
        while (input == "" || input[0] == '#');
        List<string> list = regex.Matches(input).Cast<Match>().Select<Match, string>((Func<Match, string>) (m =>
        {
          string str2 = m.Value;
          if (str2 == "" || str2[0] != '"' || str2[str2.Length - 1] != '"')
            return str2;
          str2 = LemonGraphFormat.Unescape(str2.Substring(1, str2.Length - 2));
          return str2;
        })).ToList<string>();
        string str3 = list.First<string>();
        if (input[0] == '@')
        {
          str1 = str3.Substring(1);
          Directedness? nullable = directedness;
          directedness1 = nullable.HasValue ? nullable.GetValueOrDefault() : (str1 == "arcs" ? Directedness.Directed : Directedness.Undirected);
          flag = true;
        }
        else
        {
          if (!(str1 == "nodes") && !(str1 == "red_nodes") && !(str1 == "blue_nodes"))
          {
            if (!(str1 == "arcs") && !(str1 == "edges"))
            {
              if (str1 == "attributes")
                this.Attributes[list[0]] = list[1];
            }
            else if (flag)
            {
              stringList = list;
              foreach (string key in stringList)
              {
                if (!this.ArcMaps.ContainsKey(key))
                  this.ArcMaps[key] = new Dictionary<Arc, string>();
              }
            }
            else
            {
              Satsuma.Node u = dictionary[list[0]];
              Satsuma.Node v = dictionary[list[1]];
              Arc key = graph.AddArc(u, v, directedness1);
              for (int index = 2; index < list.Count; ++index)
                this.ArcMaps[stringList[index - 2]][key] = list[index];
            }
          }
          else if (flag)
          {
            stringList = list;
            for (int index = 0; index < stringList.Count; ++index)
            {
              string key = stringList[index];
              if (key == "label")
                num = index;
              if (!this.NodeMaps.ContainsKey(key))
                this.NodeMaps[key] = new Dictionary<Satsuma.Node, string>();
            }
          }
          else
          {
            Satsuma.Node key = graph.AddNode();
            for (int index = 0; index < list.Count; ++index)
            {
              this.NodeMaps[stringList[index]][key] = list[index];
              if (index == num)
                dictionary[list[index]] = key;
            }
          }
          flag = false;
        }
      }
label_33:;
    }

    public void Load(string filename, Directedness? directedness)
    {
      using (StreamReader streamReader = new StreamReader(filename))
        this.Load((TextReader) streamReader, directedness);
    }

    public void Save(TextWriter writer, IEnumerable<string> comment = null)
    {
      if (comment != null)
      {
        foreach (string str in comment)
          writer.WriteLine("# " + str);
      }
      writer.WriteLine("@nodes");
      writer.Write("label");
      foreach (KeyValuePair<string, Dictionary<Satsuma.Node, string>> nodeMap in this.NodeMaps)
      {
        if (nodeMap.Key != "label")
          writer.Write(" " + nodeMap.Key);
      }
      writer.WriteLine();
      foreach (Satsuma.Node node in this.Graph.Nodes())
      {
        writer.Write(node.Id);
        foreach (KeyValuePair<string, Dictionary<Satsuma.Node, string>> nodeMap in this.NodeMaps)
        {
          if (nodeMap.Key != "label")
          {
            string s;
            if (!nodeMap.Value.TryGetValue(node, out s))
              s = "";
            writer.Write(" \"" + LemonGraphFormat.Escape(s) + "\"");
          }
        }
        writer.WriteLine();
      }
      writer.WriteLine();
      for (int index = 0; index < 2; ++index)
      {
        IEnumerable<Arc> arcs = index == 0 ? this.Graph.Arcs().Where<Arc>((Func<Arc, bool>) (arc => !this.Graph.IsEdge(arc))) : this.Graph.Arcs(ArcFilter.Edge);
        writer.WriteLine(index == 0 ? "@arcs" : "@edges");
        if (this.ArcMaps.Count == 0)
        {
          writer.WriteLine('-');
        }
        else
        {
          foreach (KeyValuePair<string, Dictionary<Arc, string>> arcMap in this.ArcMaps)
            writer.Write(arcMap.Key + " ");
          writer.WriteLine();
        }
        foreach (Arc arc in arcs)
        {
          writer.Write(this.Graph.U(arc).Id + 32L + this.Graph.V(arc).Id);
          foreach (KeyValuePair<string, Dictionary<Arc, string>> arcMap in this.ArcMaps)
          {
            string s;
            if (!arcMap.Value.TryGetValue(arc, out s))
              s = "";
            writer.Write(" \"" + LemonGraphFormat.Escape(s) + "\"");
          }
          writer.WriteLine();
        }
        writer.WriteLine();
      }
      if (this.Attributes.Count <= 0)
        return;
      writer.WriteLine("@attributes");
      foreach (KeyValuePair<string, string> attribute in this.Attributes)
        writer.WriteLine("\"" + LemonGraphFormat.Escape(attribute.Key) + "\" \"" + LemonGraphFormat.Escape(attribute.Value) + "\"");
      writer.WriteLine();
    }

    public void Save(string filename, IEnumerable<string> comment = null)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
        this.Save((TextWriter) streamWriter, comment);
    }
  }
}
