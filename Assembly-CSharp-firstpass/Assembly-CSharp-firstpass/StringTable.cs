// Decompiled with JetBrains decompiler
// Type: StringTable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;

public class StringTable
{
  private Dictionary<int, string> KeyNames = new Dictionary<int, string>();
  private Dictionary<int, StringTable> SubTables = new Dictionary<int, StringTable>();
  private Dictionary<int, StringEntry> Entries = new Dictionary<int, StringEntry>();

  public StringEntry Get(StringKey key0)
  {
    int hash = key0.Hash;
    StringEntry stringEntry = (StringEntry) null;
    this.Entries.TryGetValue(hash, out stringEntry);
    return stringEntry;
  }

  public void Add(int idx, string[] value)
  {
    string str = value[idx];
    int hashCode = str.GetHashCode();
    this.KeyNames[hashCode] = str;
    if (idx == value.Length - 2)
    {
      StringEntry stringEntry = new StringEntry(value[idx + 1]);
      this.Entries[hashCode] = stringEntry;
    }
    else
    {
      StringTable stringTable = (StringTable) null;
      if (!this.SubTables.TryGetValue(hashCode, out stringTable))
      {
        stringTable = new StringTable();
        this.SubTables[hashCode] = stringTable;
      }
      stringTable.Add(idx + 1, value);
    }
  }

  public void Print(string parent_path)
  {
    foreach (KeyValuePair<int, StringEntry> entry in this.Entries)
      Debug.Log((object) (parent_path + "." + this.KeyNames[entry.Key] + "." + entry.Value.String));
    string str = parent_path;
    if (str != "")
      str += ".";
    foreach (KeyValuePair<int, StringTable> subTable in this.SubTables)
      subTable.Value.Print(str + this.KeyNames[subTable.Key]);
  }
}
