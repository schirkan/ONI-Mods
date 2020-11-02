// Decompiled with JetBrains decompiler
// Type: YamlDotNet.RepresentationModel.YamlStream
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
  [Serializable]
  public class YamlStream : IEnumerable<YamlDocument>, IEnumerable
  {
    private readonly IList<YamlDocument> documents = (IList<YamlDocument>) new List<YamlDocument>();

    public IList<YamlDocument> Documents => this.documents;

    public YamlStream()
    {
    }

    public YamlStream(params YamlDocument[] documents)
      : this((IEnumerable<YamlDocument>) documents)
    {
    }

    public YamlStream(IEnumerable<YamlDocument> documents)
    {
      foreach (YamlDocument document in documents)
        this.documents.Add(document);
    }

    public void Add(YamlDocument document) => this.documents.Add(document);

    public void Load(TextReader input) => this.Load((IParser) new Parser(input));

    public void Load(IParser parser)
    {
      this.documents.Clear();
      parser.Expect<StreamStart>();
      while (!parser.Accept<StreamEnd>())
        this.documents.Add(new YamlDocument(parser));
      parser.Expect<StreamEnd>();
    }

    public void Save(TextWriter output) => this.Save(output, true);

    public void Save(TextWriter output, bool assignAnchors)
    {
      IEmitter emitter = (IEmitter) new Emitter(output);
      emitter.Emit((ParsingEvent) new StreamStart());
      foreach (YamlDocument document in (IEnumerable<YamlDocument>) this.documents)
        document.Save(emitter, assignAnchors);
      emitter.Emit((ParsingEvent) new StreamEnd());
    }

    public void Accept(IYamlVisitor visitor) => visitor.Visit(this);

    public IEnumerator<YamlDocument> GetEnumerator() => this.documents.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
