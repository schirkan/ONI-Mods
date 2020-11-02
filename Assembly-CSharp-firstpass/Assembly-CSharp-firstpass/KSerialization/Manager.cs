// Decompiled with JetBrains decompiler
// Type: KSerialization.Manager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace KSerialization
{
  public class Manager
  {
    private static Dictionary<string, SerializationTemplate> serializationTemplatesByTypeName = new Dictionary<string, SerializationTemplate>();
    private static Dictionary<string, DeserializationTemplate> deserializationTemplatesByTypeName = new Dictionary<string, DeserializationTemplate>();
    private static Dictionary<Type, SerializationTemplate> serializationTemplatesByType = new Dictionary<Type, SerializationTemplate>();
    private static Dictionary<Type, DeserializationTemplate> deserializationTemplatesByType = new Dictionary<Type, DeserializationTemplate>();
    private static Dictionary<DeserializationTemplate, KeyValuePair<SerializationTemplate, DeserializationMapping>> deserializationMappings = new Dictionary<DeserializationTemplate, KeyValuePair<SerializationTemplate, DeserializationMapping>>();
    private static Dictionary<Type, TypeInfo> typeInfoMap = new Dictionary<Type, TypeInfo>();
    private static Assembly[] assemblies = (Assembly[]) null;

    public static void Initialize() => Manager.assemblies = AppDomain.CurrentDomain.GetAssemblies();

    public static Type GetType(string type_name)
    {
      Type type = Type.GetType(type_name);
      if (type == (Type) null)
      {
        foreach (Assembly assembly in Manager.assemblies)
        {
          type = assembly.GetType(type_name);
          if (type != (Type) null)
            break;
        }
      }
      if (type == (Type) null)
        DebugLog.Output(DebugLog.Level.Warning, "Failed to find type named: " + type_name);
      return type;
    }

    public static TypeInfo GetTypeInfo(Type type)
    {
      TypeInfo typeInfo;
      if (!Manager.typeInfoMap.TryGetValue(type, out typeInfo))
        typeInfo = Manager.EncodeTypeInfo(type);
      return typeInfo;
    }

    public static SerializationTemplate GetSerializationTemplate(Type type)
    {
      if (type == (Type) null)
        throw new InvalidOperationException("Invalid type encountered when serializing");
      SerializationTemplate serializationTemplate = (SerializationTemplate) null;
      if (!Manager.serializationTemplatesByType.TryGetValue(type, out serializationTemplate))
      {
        serializationTemplate = new SerializationTemplate(type);
        Manager.serializationTemplatesByType[type] = serializationTemplate;
        Manager.serializationTemplatesByTypeName[type.GetKTypeString()] = serializationTemplate;
      }
      return serializationTemplate;
    }

    public static SerializationTemplate GetSerializationTemplate(
      string type_name)
    {
      switch (type_name)
      {
        case "":
        case null:
          throw new InvalidOperationException("Invalid type name encountered when serializing");
        default:
          SerializationTemplate serializationTemplate = (SerializationTemplate) null;
          if (!Manager.serializationTemplatesByTypeName.TryGetValue(type_name, out serializationTemplate))
          {
            Type type = Manager.GetType(type_name);
            if (type != (Type) null)
            {
              serializationTemplate = new SerializationTemplate(type);
              Manager.serializationTemplatesByType[type] = serializationTemplate;
              Manager.serializationTemplatesByTypeName[type_name] = serializationTemplate;
            }
          }
          return serializationTemplate;
      }
    }

    public static DeserializationTemplate GetDeserializationTemplate(
      Type type)
    {
      DeserializationTemplate deserializationTemplate = (DeserializationTemplate) null;
      Manager.deserializationTemplatesByType.TryGetValue(type, out deserializationTemplate);
      return deserializationTemplate;
    }

    public static DeserializationTemplate GetDeserializationTemplate(
      string type_name)
    {
      DeserializationTemplate deserializationTemplate = (DeserializationTemplate) null;
      Manager.deserializationTemplatesByTypeName.TryGetValue(type_name, out deserializationTemplate);
      return deserializationTemplate;
    }

    public static void SerializeDirectory(BinaryWriter writer)
    {
      writer.Write(Manager.serializationTemplatesByTypeName.Count);
      foreach (KeyValuePair<string, SerializationTemplate> keyValuePair in Manager.serializationTemplatesByTypeName)
      {
        string key = keyValuePair.Key;
        SerializationTemplate serializationTemplate = keyValuePair.Value;
        try
        {
          writer.WriteKleiString(key);
          serializationTemplate.SerializeTemplate(writer);
        }
        catch (Exception ex)
        {
          DebugUtil.LogErrorArgs((object) ("Error serializing template " + key + "\n"), (object) ex.Message, (object) ex.StackTrace);
        }
      }
    }

    public static void DeserializeDirectory(IReader reader)
    {
      Manager.deserializationTemplatesByTypeName.Clear();
      Manager.deserializationTemplatesByType.Clear();
      Manager.deserializationMappings.Clear();
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
      {
        string str = reader.ReadKleiString();
        try
        {
          DeserializationTemplate deserializationTemplate = new DeserializationTemplate(str, reader);
          Manager.deserializationTemplatesByTypeName[str] = deserializationTemplate;
          Type type = Manager.GetType(str);
          if (type != (Type) null)
            Manager.deserializationTemplatesByType[type] = deserializationTemplate;
        }
        catch (Exception ex)
        {
          string message = "Error deserializing template " + str + "\n" + ex.Message + "\n" + ex.StackTrace;
          Debug.LogError((object) message);
          throw new Exception(message, ex);
        }
      }
    }

    public static void Clear()
    {
      Manager.serializationTemplatesByTypeName.Clear();
      Manager.serializationTemplatesByType.Clear();
      Manager.deserializationTemplatesByTypeName.Clear();
      Manager.deserializationTemplatesByType.Clear();
      Manager.deserializationMappings.Clear();
      Manager.typeInfoMap.Clear();
      Helper.ClearTypeInfoMask();
    }

    public static bool HasDeserializationMapping(Type type) => Manager.GetDeserializationTemplate(type) != null && Manager.GetSerializationTemplate(type) != null;

    public static DeserializationMapping GetDeserializationMapping(Type type) => Manager.GetMapping(Manager.GetDeserializationTemplate(type) ?? throw new ArgumentException("Tried to deserialize a class named: " + type.GetKTypeString() + " but no such class exists"), Manager.GetSerializationTemplate(type) ?? throw new ArgumentException("Tried to deserialize into a class named: " + type.GetKTypeString() + " but no such class exists"));

    public static DeserializationMapping GetDeserializationMapping(
      string type_name)
    {
      return Manager.GetMapping(Manager.GetDeserializationTemplate(type_name) ?? throw new ArgumentException("Tried to deserialize a class named: " + type_name + " but no such class exists"), Manager.GetSerializationTemplate(type_name) ?? throw new ArgumentException("Tried to deserialize into a class named: " + type_name + " but no such class exists"));
    }

    private static DeserializationMapping GetMapping(
      DeserializationTemplate dtemplate,
      SerializationTemplate stemplate)
    {
      KeyValuePair<SerializationTemplate, DeserializationMapping> keyValuePair;
      DeserializationMapping deserializationMapping;
      if (Manager.deserializationMappings.TryGetValue(dtemplate, out keyValuePair))
      {
        deserializationMapping = keyValuePair.Value;
      }
      else
      {
        deserializationMapping = new DeserializationMapping(dtemplate, stemplate);
        keyValuePair = new KeyValuePair<SerializationTemplate, DeserializationMapping>(stemplate, deserializationMapping);
        Manager.deserializationMappings[dtemplate] = keyValuePair;
      }
      return deserializationMapping;
    }

    private static TypeInfo EncodeTypeInfo(Type type)
    {
      TypeInfo typeInfo = new TypeInfo();
      typeInfo.type = type;
      typeInfo.info = Helper.EncodeSerializationType(type);
      if (type.IsGenericType)
      {
        typeInfo.genericTypeArgs = type.GetGenericArguments();
        typeInfo.subTypes = new TypeInfo[typeInfo.genericTypeArgs.Length];
        for (int index = 0; index < typeInfo.genericTypeArgs.Length; ++index)
          typeInfo.subTypes[index] = Manager.GetTypeInfo(typeInfo.genericTypeArgs[index]);
      }
      else if (typeof (Array).IsAssignableFrom(type))
      {
        Type elementType = type.GetElementType();
        typeInfo.subTypes = new TypeInfo[1];
        typeInfo.subTypes[0] = Manager.GetTypeInfo(elementType);
      }
      return typeInfo;
    }
  }
}
