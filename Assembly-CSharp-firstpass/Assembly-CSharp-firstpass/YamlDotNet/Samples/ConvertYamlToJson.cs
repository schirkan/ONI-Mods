// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.ConvertYamlToJson
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.IO;
using YamlDotNet.Samples.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.Samples
{
  public class ConvertYamlToJson
  {
    private readonly ITestOutputHelper output;

    public ConvertYamlToJson(ITestOutputHelper output) => this.output = output;

    [Sample(Description = "Shows how to convert a YAML document to JSON.", Title = "Convert YAML to JSON")]
    public void Main()
    {
      StringReader stringReader = new StringReader("\nscalar: a scalar\nsequence:\n  - one\n  - two\n");
      object graph = new DeserializerBuilder().Build().Deserialize((TextReader) stringReader);
      this.output.WriteLine(new SerializerBuilder().JsonCompatible().Build().Serialize(graph));
    }
  }
}
