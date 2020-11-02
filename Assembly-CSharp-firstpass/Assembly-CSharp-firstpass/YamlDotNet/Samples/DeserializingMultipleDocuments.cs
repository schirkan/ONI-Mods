// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Samples.DeserializingMultipleDocuments
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Samples.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.Samples
{
  public class DeserializingMultipleDocuments
  {
    private readonly ITestOutputHelper output;
    private const string Document = "---\n- Prisoner\n- Goblet\n- Phoenix\n---\n- Memoirs\n- Snow \n- Ghost\t\t\n...";

    public DeserializingMultipleDocuments(ITestOutputHelper output) => this.output = output;

    [Sample(Description = "Explains how to load multiple YAML documents from a stream.", Title = "Deserializing multiple documents")]
    public void Main()
    {
      StringReader stringReader = new StringReader("---\n- Prisoner\n- Goblet\n- Phoenix\n---\n- Memoirs\n- Snow \n- Ghost\t\t\n...");
      Deserializer deserializer = new DeserializerBuilder().Build();
      Parser parser = new Parser((TextReader) stringReader);
      parser.Expect<StreamStart>();
      while (parser.Accept<DocumentStart>())
      {
        List<string> stringList = deserializer.Deserialize<List<string>>((IParser) parser);
        this.output.WriteLine("## Document");
        foreach (string str in stringList)
          this.output.WriteLine(str);
      }
    }
  }
}
