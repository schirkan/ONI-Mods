// Decompiled with JetBrains decompiler
// Type: AsyncCsvLoader`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public abstract class AsyncCsvLoader<LoaderType, CsvEntryType> : GlobalAsyncLoader<LoaderType>
  where LoaderType : class
  where CsvEntryType : Resource, new()
{
  private string text;
  private string name;
  public CsvEntryType[] entries;

  public AsyncCsvLoader(TextAsset asset)
  {
    this.text = asset.text;
    this.name = asset.name;
  }

  public override void Run()
  {
    this.entries = new ResourceLoader<CsvEntryType>(this.text, this.name).resources.ToArray();
    this.text = (string) null;
    this.name = (string) null;
  }
}
