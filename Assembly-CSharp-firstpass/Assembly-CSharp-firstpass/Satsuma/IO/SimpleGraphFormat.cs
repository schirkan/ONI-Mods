// Decompiled with JetBrains decompiler
// Type: Satsuma.IO.SimpleGraphFormat
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Satsuma.IO
{
  public sealed class SimpleGraphFormat
  {
    public IGraph Graph { get; set; }

    public IList<Dictionary<Arc, string>> Extensions { get; private set; }

    public int StartIndex { get; set; }

    public SimpleGraphFormat() => this.Extensions = (IList<Dictionary<Arc, string>>) new List<Dictionary<Arc, string>>();

    public Satsuma.Node[] Load(TextReader reader, Directedness directedness)
    {
      if (this.Graph == null)
        this.Graph = (IGraph) new CustomGraph();
      IBuildableGraph graph = (IBuildableGraph) this.Graph;
      graph.Clear();
      Regex regex = new Regex("\\s+");
      string[] strArray1 = regex.Split(reader.ReadLine());
      int length = int.Parse(strArray1[0], (IFormatProvider) CultureInfo.InvariantCulture);
      int num1 = int.Parse(strArray1[1], (IFormatProvider) CultureInfo.InvariantCulture);
      Satsuma.Node[] nodeArray = new Satsuma.Node[length];
      for (int index = 0; index < length; ++index)
        nodeArray[index] = graph.AddNode();
      this.Extensions.Clear();
      for (int index1 = 0; index1 < num1; ++index1)
      {
        string[] strArray2 = regex.Split(reader.ReadLine());
        int index2 = (int) (long.Parse(strArray2[0], (IFormatProvider) CultureInfo.InvariantCulture) - (long) this.StartIndex);
        int index3 = (int) (long.Parse(strArray2[1], (IFormatProvider) CultureInfo.InvariantCulture) - (long) this.StartIndex);
        Arc key = graph.AddArc(nodeArray[index2], nodeArray[index3], directedness);
        int num2 = strArray2.Length - 2;
        for (int index4 = 0; index4 < num2 - this.Extensions.Count; ++index4)
          this.Extensions.Add(new Dictionary<Arc, string>());
        for (int index4 = 0; index4 < num2; ++index4)
          this.Extensions[index4][key] = strArray2[2 + index4];
      }
      return nodeArray;
    }

    public Satsuma.Node[] Load(string filename, Directedness directedness)
    {
      using (StreamReader streamReader = new StreamReader(filename))
        return this.Load((TextReader) streamReader, directedness);
    }

    public void Save(TextWriter writer)
    {
      Regex regex = new Regex("\\s");
      writer.WriteLine(this.Graph.NodeCount().ToString() + " " + (object) this.Graph.ArcCount());
      Dictionary<Satsuma.Node, long> dictionary = new Dictionary<Satsuma.Node, long>();
      long startIndex = (long) this.StartIndex;
      foreach (Arc arc in this.Graph.Arcs())
      {
        Satsuma.Node key1 = this.Graph.U(arc);
        long num1;
        if (!dictionary.TryGetValue(key1, out num1))
          dictionary[key1] = num1 = startIndex++;
        Satsuma.Node key2 = this.Graph.V(arc);
        long num2;
        if (!dictionary.TryGetValue(key2, out num2))
          dictionary[key2] = num2 = startIndex++;
        writer.Write(num1.ToString() + " " + (object) num2);
        foreach (Dictionary<Arc, string> extension in (IEnumerable<Dictionary<Arc, string>>) this.Extensions)
        {
          string input;
          extension.TryGetValue(arc, out input);
          if (string.IsNullOrEmpty(input) || regex.IsMatch(input))
            throw new ArgumentException("Extension value is empty or contains whitespaces.");
          writer.Write(" " + extension[arc]);
        }
        writer.WriteLine();
      }
    }

    public void Save(string filename)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
        this.Save((TextWriter) streamWriter);
    }
  }
}
