// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.Events.IParsingEventVisitor
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace YamlDotNet.Core.Events
{
  public interface IParsingEventVisitor
  {
    void Visit(AnchorAlias e);

    void Visit(StreamStart e);

    void Visit(StreamEnd e);

    void Visit(DocumentStart e);

    void Visit(DocumentEnd e);

    void Visit(Scalar e);

    void Visit(SequenceStart e);

    void Visit(SequenceEnd e);

    void Visit(MappingStart e);

    void Visit(MappingEnd e);

    void Visit(Comment e);
  }
}
