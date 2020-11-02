// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EventEmitters.JsonEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
  public sealed class JsonEventEmitter : ChainedEventEmitter
  {
    public JsonEventEmitter(IEventEmitter nextEmitter)
      : base(nextEmitter)
    {
    }

    public override void Emit(AliasEventInfo eventInfo, IEmitter emitter) => throw new NotSupportedException("Aliases are not supported in JSON");

    public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
    {
      eventInfo.IsPlainImplicit = true;
      eventInfo.Style = ScalarStyle.Plain;
      TypeCode typeCode = eventInfo.Source.Value != null ? eventInfo.Source.Type.GetTypeCode() : TypeCode.Empty;
      switch (typeCode)
      {
        case TypeCode.Empty:
          eventInfo.RenderedValue = "null";
          break;
        case TypeCode.Boolean:
          eventInfo.RenderedValue = YamlFormatter.FormatBoolean(eventInfo.Source.Value);
          break;
        case TypeCode.Char:
        case TypeCode.String:
          eventInfo.RenderedValue = eventInfo.Source.Value.ToString();
          eventInfo.Style = ScalarStyle.DoubleQuoted;
          break;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
        case TypeCode.Single:
        case TypeCode.Double:
        case TypeCode.Decimal:
          eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
          break;
        case TypeCode.DateTime:
          eventInfo.RenderedValue = YamlFormatter.FormatDateTime(eventInfo.Source.Value);
          break;
        default:
          if (!(eventInfo.Source.Type == typeof (TimeSpan)))
            throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "TypeCode.{0} is not supported.", (object) typeCode));
          eventInfo.RenderedValue = YamlFormatter.FormatTimeSpan(eventInfo.Source.Value);
          break;
      }
      base.Emit(eventInfo, emitter);
    }

    public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
    {
      eventInfo.Style = MappingStyle.Flow;
      base.Emit(eventInfo, emitter);
    }

    public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
    {
      eventInfo.Style = SequenceStyle.Flow;
      base.Emit(eventInfo, emitter);
    }
  }
}
