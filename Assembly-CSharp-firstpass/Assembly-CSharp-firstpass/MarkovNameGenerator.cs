// Decompiled with JetBrains decompiler
// Type: MarkovNameGenerator
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class MarkovNameGenerator
{
  private Dictionary<string, List<char>> _chains = new Dictionary<string, List<char>>();
  private List<string> _samples = new List<string>();
  private List<string> _used = new List<string>();
  private Random _rnd = new Random();
  private int _order;
  private int _minLength;

  public MarkovNameGenerator(IEnumerable<string> sampleNames, int order, int minLength)
  {
    if (order < 1)
      order = 1;
    if (minLength < 1)
      minLength = 1;
    this._order = order;
    this._minLength = minLength;
    foreach (string sampleName in sampleNames)
    {
      char[] chArray = new char[1]{ ',' };
      foreach (string str in sampleName.Split(chArray))
      {
        string upper = str.Trim().ToUpper();
        if (upper.Length >= order + 1)
          this._samples.Add(upper);
      }
    }
    foreach (string sample in this._samples)
    {
      for (int startIndex = 0; startIndex < sample.Length - order; ++startIndex)
      {
        string key = sample.Substring(startIndex, order);
        List<char> charList;
        if (this._chains.ContainsKey(key))
        {
          charList = this._chains[key];
        }
        else
        {
          charList = new List<char>();
          this._chains[key] = charList;
        }
        charList.Add(sample[startIndex + order]);
      }
    }
  }

  public string NextName
  {
    get
    {
      string str1;
      do
      {
        int index1 = this._rnd.Next(this._samples.Count);
        int length = this._samples[index1].Length;
        string str2;
        string token;
        for (str2 = this._samples[index1].Substring(this._rnd.Next(0, this._samples[index1].Length - this._order), this._order); str2.Length < length; str2 += this.GetLetter(token).ToString())
        {
          token = str2.Substring(str2.Length - this._order, this._order);
          if (this.GetLetter(token) == '?')
            break;
        }
        if (str2.Contains(" "))
        {
          string[] strArray = str2.Split(' ');
          str1 = "";
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            if (!(strArray[index2] == ""))
            {
              strArray[index2] = strArray[index2].Length != 1 ? strArray[index2].Substring(0, 1) + strArray[index2].Substring(1).ToLower() : strArray[index2].ToUpper();
              if (str1 != "")
                str1 += " ";
              str1 += strArray[index2];
            }
          }
        }
        else
          str1 = str2.Substring(0, 1) + str2.Substring(1).ToLower();
      }
      while (this._used.Contains(str1) || str1.Length < this._minLength);
      this._used.Add(str1);
      return str1;
    }
  }

  public void Reset() => this._used.Clear();

  private char GetLetter(string token)
  {
    if (!this._chains.ContainsKey(token))
      return '?';
    List<char> chain = this._chains[token];
    int index = this._rnd.Next(chain.Count);
    return chain[index];
  }
}
