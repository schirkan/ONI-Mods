// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.SerializeObjectGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Samples.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.Samples
{
  public class SerializeObjectGraph
  {
    private readonly ITestOutputHelper output;

    public SerializeObjectGraph(ITestOutputHelper output) => this.output = output;

    [Sample(Description = "Shows how to convert an object to its YAML representation.", Title = "Serializing an object graph")]
    public void Main()
    {
      Address address = new Address()
      {
        street = "123 Tornado Alley\nSuite 16",
        city = "East Westville",
        state = "KS"
      };
      Receipt receipt1 = new Receipt();
      receipt1.receipt = "Oz-Ware Purchase Invoice";
      receipt1.date = new DateTime(2007, 8, 6);
      receipt1.customer = new Customer()
      {
        given = "Dorothy",
        family = "Gale"
      };
      receipt1.items = new Item[2]
      {
        new Item()
        {
          part_no = "A4786",
          descrip = "Water Bucket (Filled)",
          price = 1.47M,
          quantity = 4
        },
        new Item()
        {
          part_no = "E1628",
          descrip = "High Heeled \"Ruby\" Slippers",
          price = 100.27M,
          quantity = 1
        }
      };
      receipt1.bill_to = address;
      receipt1.ship_to = address;
      receipt1.specialDelivery = "Follow the Yellow Brick\nRoad to the Emerald City.\nPay no attention to the\nman behind the curtain.";
      Receipt receipt2 = receipt1;
      this.output.WriteLine(new SerializerBuilder().Build().Serialize((object) receipt2));
    }
  }
}
