// Decompiled with JetBrains decompiler
// Type: KSerialization.Deserializer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace KSerialization
{
  public class Deserializer
  {
    public IReader reader;

    public Deserializer(IReader reader) => this.reader = reader;

    public bool Deserialize(object obj) => Deserializer.Deserialize(obj, this.reader);

    public static bool Deserialize(object obj, IReader reader)
    {
      string str = reader.ReadKleiString();
      Type type = obj.GetType();
      return type.GetKTypeString() == str && Deserializer.DeserializeTypeless(type, obj, reader);
    }

    public static bool DeserializeTypeless(Type type, object obj, IReader reader)
    {
      DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(type);
      try
      {
        return deserializationMapping.Deserialize(obj, reader);
      }
      catch (Exception ex)
      {
        string str = string.Format("Exception occurred while attempting to deserialize object {0}({1}).\n{2}", obj, (object) obj.GetType(), (object) ex.ToString());
        DebugLog.Output(DebugLog.Level.Error, str);
        throw new Exception(str, ex);
      }
    }

    public static bool DeserializeTypeless(object obj, IReader reader)
    {
      DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(obj.GetType());
      try
      {
        return deserializationMapping.Deserialize(obj, reader);
      }
      catch (Exception ex)
      {
        string str = string.Format("Exception occurred while attempting to deserialize object {0}({1}).\n{2}", obj, (object) obj.GetType(), (object) ex.ToString());
        DebugLog.Output(DebugLog.Level.Error, str);
        throw new Exception(str, ex);
      }
    }

    public static bool Deserialize(Type type, IReader reader, out object result)
    {
      DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(type);
      bool flag;
      try
      {
        object instance = Activator.CreateInstance(type);
        flag = deserializationMapping.Deserialize(instance, reader);
        result = instance;
      }
      catch (Exception ex)
      {
        string str = string.Format("Exception occurred while attempting to deserialize into object of type {0}.\n{1}", (object) type.ToString(), (object) ex.ToString());
        DebugLog.Output(DebugLog.Level.Error, str);
        throw new Exception(str, ex);
      }
      return flag;
    }
  }
}
