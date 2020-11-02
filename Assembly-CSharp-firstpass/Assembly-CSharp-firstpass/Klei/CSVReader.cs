// Decompiled with JetBrains decompiler
// Type: Klei.CSVReader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Klei
{
  public class CSVReader
  {
    private static Regex regex = new Regex("(((?<x>(?=[,\\r\\n]+))|\"(?<x>([^\"]|\"\")+)\"|(?<x>[^,\\r\\n]+)),?)", RegexOptions.ExplicitCapture);

    public static void DebugOutputGrid(string[,] grid)
    {
      string str = "";
      for (int index1 = 0; index1 < grid.GetUpperBound(1); ++index1)
      {
        for (int index2 = 0; index2 < grid.GetUpperBound(0); ++index2)
          str = str + grid[index2, index1] + "|";
        str += "\n";
      }
      Debug.Log((object) str);
    }

    public static string[,] SplitCsvGrid(string csvText, string csv_name)
    {
      string[] strArray1 = csvText.Split('\n', '\r');
      List<string> stringList1 = new List<string>();
      foreach (string str in strArray1)
      {
        if (str.Length != 0 && !str.StartsWith("#"))
          stringList1.Add(str);
      }
      List<string> stringList2 = new List<string>();
      for (int index1 = 0; index1 < stringList1.Count; ++index1)
      {
        string str1 = stringList1[index1];
        int num = 0;
        for (int index2 = 0; index2 < str1.Length; ++index2)
        {
          if (str1[index2] == '"')
            ++num;
        }
        if (num % 2 == 1)
        {
          string str2 = stringList1[index1] + "\n" + stringList1[index1 + 1];
          stringList1[index1 + 1] = str2;
        }
        else
          stringList2.Add(str1);
      }
      stringList2.RemoveAll((Predicate<string>) (x => x.StartsWith("#")));
      string[][] strArray2 = new string[stringList2.Count][];
      for (int index = 0; index < stringList2.Count; ++index)
        strArray2[index] = CSVReader.SplitCsvLine(stringList2[index]);
      int a = 0;
      for (int index = 0; index < strArray2.Length; ++index)
        a = Mathf.Max(a, strArray2[index].Length);
      string[,] strArray3 = new string[a + 1, strArray2.Length + 1];
      for (int index1 = 0; index1 < strArray2.Length; ++index1)
      {
        string[] strArray4 = strArray2[index1];
        for (int index2 = 0; index2 < strArray4.Length; ++index2)
        {
          strArray3[index2, index1] = strArray4[index2];
          strArray3[index2, index1] = strArray3[index2, index1].Replace("\"\"", "\"");
        }
      }
      return strArray3;
    }

    public static string[] SplitCsvLine(string line)
    {
      line = line.Replace("\n\n", "\n");
      return CSVReader.regex.Matches(line).Cast<Match>().Select<Match, string>((Func<Match, string>) (m => m.Groups[1].Value)).ToArray<string>();
    }

    private struct ParseWorkItem : IWorkItem<object>
    {
      public string line;
      public string[] row;

      public void Run(object shared_data) => this.row = CSVReader.SplitCsvLine(this.line);
    }
  }
}
