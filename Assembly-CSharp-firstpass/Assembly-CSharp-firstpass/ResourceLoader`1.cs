// Decompiled with JetBrains decompiler
// Type: ResourceLoader`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader<T> where T : Resource, new()
{
  public List<T> resources = new List<T>();

  public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this.resources.GetEnumerator();

  public ResourceLoader()
  {
  }

  public ResourceLoader(TextAsset file) => this.Load(file);

  public ResourceLoader(string text, string name) => this.Load(text, name);

  public void Load(string text, string name)
  {
    string[,] grid = CSVReader.SplitCsvGrid(text, name);
    int length = grid.GetLength(1);
    for (int row = 1; row < length; ++row)
    {
      if (!grid[0, row].IsNullOrWhiteSpace())
      {
        T obj = new T();
        CSVUtil.ParseData<T>((object) obj, grid, row);
        if (!obj.Disabled)
          this.resources.Add(obj);
      }
    }
  }

  public virtual void Load(TextAsset file)
  {
    if ((Object) file == (Object) null)
      Debug.LogWarning((object) ("Missing resource file of type: " + typeof (T).Name));
    else
      this.Load(file.text, file.name);
  }
}
