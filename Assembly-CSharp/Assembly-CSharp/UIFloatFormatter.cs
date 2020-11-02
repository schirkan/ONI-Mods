// Decompiled with JetBrains decompiler
// Type: UIFloatFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class UIFloatFormatter
{
  private int activeStringCount;
  private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();

  public string Format(string format, float value) => this.Replace(format, "{0}", value);

  private string Replace(string format, string key, float value)
  {
    UIFloatFormatter.Entry entry = new UIFloatFormatter.Entry();
    if (this.activeStringCount >= this.entries.Count)
    {
      entry.format = format;
      entry.key = key;
      entry.value = value;
      entry.result = entry.format.Replace(key, value.ToString());
      this.entries.Add(entry);
    }
    else
    {
      entry = this.entries[this.activeStringCount];
      if (entry.format != format || entry.key != key || (double) entry.value != (double) value)
      {
        entry.format = format;
        entry.key = key;
        entry.value = value;
        entry.result = entry.format.Replace(key, value.ToString());
        this.entries[this.activeStringCount] = entry;
      }
    }
    ++this.activeStringCount;
    return entry.result;
  }

  public void BeginDrawing() => this.activeStringCount = 0;

  public void EndDrawing()
  {
  }

  private struct Entry
  {
    public string format;
    public string key;
    public float value;
    public string result;
  }
}
