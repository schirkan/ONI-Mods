// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.IYamlVisitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.RepresentationModel
{
  public interface IYamlVisitor
  {
    void Visit(YamlStream stream);

    void Visit(YamlDocument document);

    void Visit(YamlScalarNode scalar);

    void Visit(YamlSequenceNode sequence);

    void Visit(YamlMappingNode mapping);
  }
}
