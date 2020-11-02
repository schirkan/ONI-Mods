// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.Receipt
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace YamlDotNet.Samples
{
  public class Receipt
  {
    public string receipt { get; set; }

    public DateTime date { get; set; }

    public Customer customer { get; set; }

    public Item[] items { get; set; }

    public Address bill_to { get; set; }

    public Address ship_to { get; set; }

    public string specialDelivery { get; set; }
  }
}
