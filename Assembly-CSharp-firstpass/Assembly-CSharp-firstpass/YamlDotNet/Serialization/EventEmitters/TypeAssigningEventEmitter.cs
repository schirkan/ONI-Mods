// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.EventEmitters.TypeAssigningEventEmitter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.EventEmitters
{
  public sealed class TypeAssigningEventEmitter : ChainedEventEmitter
  {
    private readonly bool _assignTypeWhenDifferent;

    public TypeAssigningEventEmitter(IEventEmitter nextEmitter, bool assignTypeWhenDifferent)
      : base(nextEmitter)
      => this._assignTypeWhenDifferent = assignTypeWhenDifferent;

    public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
    {
      ScalarStyle scalarStyle = ScalarStyle.Plain;
      TypeCode typeCode = eventInfo.Source.Value != null ? eventInfo.Source.Type.GetTypeCode() : TypeCode.Empty;
      switch (typeCode)
      {
        case TypeCode.Empty:
          eventInfo.Tag = "tag:yaml.org,2002:null";
          eventInfo.RenderedValue = "";
          break;
        case TypeCode.Boolean:
          eventInfo.Tag = "tag:yaml.org,2002:bool";
          eventInfo.RenderedValue = YamlFormatter.FormatBoolean(eventInfo.Source.Value);
          break;
        case TypeCode.Char:
        case TypeCode.String:
          eventInfo.Tag = "tag:yaml.org,2002:str";
          eventInfo.RenderedValue = eventInfo.Source.Value.ToString();
          scalarStyle = ScalarStyle.Any;
          break;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          eventInfo.Tag = "tag:yaml.org,2002:int";
          eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
          break;
        case TypeCode.Single:
          eventInfo.Tag = "tag:yaml.org,2002:float";
          eventInfo.RenderedValue = YamlFormatter.FormatNumber((float) eventInfo.Source.Value);
          break;
        case TypeCode.Double:
          eventInfo.Tag = "tag:yaml.org,2002:float";
          eventInfo.RenderedValue = YamlFormatter.FormatNumber((double) eventInfo.Source.Value);
          break;
        case TypeCode.Decimal:
          eventInfo.Tag = "tag:yaml.org,2002:float";
          eventInfo.RenderedValue = YamlFormatter.FormatNumber(eventInfo.Source.Value);
          break;
        case TypeCode.DateTime:
          eventInfo.Tag = "tag:yaml.org,2002:timestamp";
          eventInfo.RenderedValue = YamlFormatter.FormatDateTime(eventInfo.Source.Value);
          break;
        default:
          if (!(eventInfo.Source.Type == typeof (TimeSpan)))
            throw new NotSupportedException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "TypeCode.{0} is not supported.", (object) typeCode));
          eventInfo.RenderedValue = YamlFormatter.FormatTimeSpan(eventInfo.Source.Value);
          break;
      }
      eventInfo.IsPlainImplicit = true;
      if (eventInfo.Style == ScalarStyle.Any)
        eventInfo.Style = scalarStyle;
      base.Emit(eventInfo, emitter);
    }

    public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
    {
      this.AssignTypeIfDifferent((ObjectEventInfo) eventInfo);
      base.Emit(eventInfo, emitter);
    }

    public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
    {
      this.AssignTypeIfDifferent((ObjectEventInfo) eventInfo);
      base.Emit(eventInfo, emitter);
    }

    private void AssignTypeIfDifferent(ObjectEventInfo eventInfo)
    {
      if (!this._assignTypeWhenDifferent || eventInfo.Source.Value == null || !(eventInfo.Source.Type != eventInfo.Source.StaticType))
        return;
      eventInfo.Tag = "!" + eventInfo.Source.Type.AssemblyQualifiedName;
    }
  }
}
