// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EmissionPhaseObjectGraphVisitorArgs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
  public sealed class EmissionPhaseObjectGraphVisitorArgs
  {
    private readonly IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors;

    public IObjectGraphVisitor<IEmitter> InnerVisitor { get; private set; }

    public IEventEmitter EventEmitter { get; private set; }

    public ObjectSerializer NestedObjectSerializer { get; private set; }

    public IEnumerable<IYamlTypeConverter> TypeConverters { get; private set; }

    public EmissionPhaseObjectGraphVisitorArgs(
      IObjectGraphVisitor<IEmitter> innerVisitor,
      IEventEmitter eventEmitter,
      IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors,
      IEnumerable<IYamlTypeConverter> typeConverters,
      ObjectSerializer nestedObjectSerializer)
    {
      this.InnerVisitor = innerVisitor != null ? innerVisitor : throw new ArgumentNullException(nameof (innerVisitor));
      this.EventEmitter = eventEmitter != null ? eventEmitter : throw new ArgumentNullException(nameof (eventEmitter));
      this.preProcessingPhaseVisitors = preProcessingPhaseVisitors != null ? preProcessingPhaseVisitors : throw new ArgumentNullException(nameof (preProcessingPhaseVisitors));
      this.TypeConverters = typeConverters != null ? typeConverters : throw new ArgumentNullException(nameof (typeConverters));
      this.NestedObjectSerializer = nestedObjectSerializer != null ? nestedObjectSerializer : throw new ArgumentNullException(nameof (nestedObjectSerializer));
    }

    public T GetPreProcessingPhaseObjectGraphVisitor<T>() where T : IObjectGraphVisitor<Nothing> => this.preProcessingPhaseVisitors.OfType<T>().Single<T>();
  }
}
