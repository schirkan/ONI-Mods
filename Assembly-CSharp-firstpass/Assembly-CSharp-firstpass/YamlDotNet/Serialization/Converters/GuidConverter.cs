// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Converters.GuidConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.Converters
{
  public class GuidConverter : IYamlTypeConverter
  {
    private readonly bool jsonCompatible;

    public GuidConverter(bool jsonCompatible) => this.jsonCompatible = jsonCompatible;

    public bool Accepts(Type type) => type == typeof (Guid);

    public object ReadYaml(IParser parser, Type type)
    {
      string g = ((Scalar) parser.Current).Value;
      parser.MoveNext();
      return (object) new Guid(g);
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
      Guid guid = (Guid) value;
      emitter.Emit((ParsingEvent) new Scalar((string) null, (string) null, guid.ToString("D"), this.jsonCompatible ? ScalarStyle.DoubleQuoted : ScalarStyle.Any, true, false));
    }
  }
}
