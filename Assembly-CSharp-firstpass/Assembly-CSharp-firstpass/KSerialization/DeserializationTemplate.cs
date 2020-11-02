// Decompiled with JetBrains decompiler
// Type: KSerialization.DeserializationTemplate
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace KSerialization
{
  public class DeserializationTemplate
  {
    public string typeName;
    public MethodInfo onDeserializing;
    public MethodInfo onDeserialized;
    public MethodInfo customDeserialize;
    public List<DeserializationTemplate.SerializedInfo> serializedMembers = new List<DeserializationTemplate.SerializedInfo>();

    public DeserializationTemplate(string template_type_name, IReader reader)
    {
      this.typeName = template_type_name;
      DebugLog.Output(DebugLog.Level.Info, "Loading Deserialization Template: " + template_type_name);
      System.Type type = Manager.GetType(template_type_name);
      if (type != (System.Type) null)
        type.GetSerializationMethods(typeof (OnDeserializingAttribute), typeof (OnDeserializedAttribute), typeof (CustomDeserialize), out this.onDeserializing, out this.onDeserialized, out this.customDeserialize);
      int num1 = reader.ReadInt32();
      int num2 = reader.ReadInt32();
      DeserializationTemplate.SerializedInfo serializedInfo1;
      for (int index = 0; index < num1; ++index)
      {
        DebugLog.Output(DebugLog.Level.Info, "Field " + index.ToString());
        string str = reader.ReadKleiString();
        DebugLog.Output(DebugLog.Level.Info, "Field " + index.ToString() + " == " + str);
        TypeInfo typeInfo = this.ReadType(reader);
        if (typeInfo.type == (System.Type) null)
          DebugLog.Output(DebugLog.Level.Warning, string.Format("Unknown type encountered while dserializing template {0} field {1} ({2}) at offset {3}", (object) template_type_name, (object) index, (object) str, (object) reader.Position));
        List<DeserializationTemplate.SerializedInfo> serializedMembers = this.serializedMembers;
        serializedInfo1 = new DeserializationTemplate.SerializedInfo();
        serializedInfo1.name = str;
        serializedInfo1.typeInfo = typeInfo;
        DeserializationTemplate.SerializedInfo serializedInfo2 = serializedInfo1;
        serializedMembers.Add(serializedInfo2);
      }
      for (int index = 0; index < num2; ++index)
      {
        DebugLog.Output(DebugLog.Level.Info, "Property " + index.ToString());
        string str = reader.ReadKleiString();
        DebugLog.Output(DebugLog.Level.Info, "Property " + index.ToString() + " == " + str);
        TypeInfo typeInfo = this.ReadType(reader);
        if (typeInfo.type == (System.Type) null)
          DebugLog.Output(DebugLog.Level.Info, string.Format("Unknown type encountered while dserializing template {0} property {1} ({2}) at offset {3}", (object) template_type_name, (object) index, (object) str, (object) reader.Position));
        List<DeserializationTemplate.SerializedInfo> serializedMembers = this.serializedMembers;
        serializedInfo1 = new DeserializationTemplate.SerializedInfo();
        serializedInfo1.name = str;
        serializedInfo1.typeInfo = typeInfo;
        DeserializationTemplate.SerializedInfo serializedInfo2 = serializedInfo1;
        serializedMembers.Add(serializedInfo2);
      }
      DebugLog.Output(DebugLog.Level.Info, "Finished loading template " + template_type_name);
    }

    private TypeInfo ReadType(IReader reader)
    {
      TypeInfo typeInfo = new TypeInfo();
      typeInfo.info = (SerializationTypeInfo) reader.ReadByte();
      SerializationTypeInfo serializationTypeInfo = typeInfo.info & SerializationTypeInfo.VALUE_MASK;
      if (Helper.IsGenericType(typeInfo.info))
      {
        System.Type type = (System.Type) null;
        switch (serializationTypeInfo)
        {
          case SerializationTypeInfo.UserDefined:
            string type_name = reader.ReadKleiString();
            typeInfo.type = Manager.GetType(type_name);
            break;
          case SerializationTypeInfo.Pair:
            type = typeof (KeyValuePair<,>);
            break;
          case SerializationTypeInfo.Dictionary:
            type = typeof (Dictionary<,>);
            break;
          case SerializationTypeInfo.List:
            type = typeof (List<>);
            break;
          case SerializationTypeInfo.HashSet:
            type = typeof (HashSet<>);
            break;
          case SerializationTypeInfo.Queue:
            type = typeof (Queue<>);
            break;
          default:
            throw new ArgumentException("unknown type");
        }
        byte num = reader.ReadByte();
        System.Type[] array = new System.Type[(int) num];
        typeInfo.subTypes = new TypeInfo[(int) num];
        for (int index = 0; index < (int) num; ++index)
        {
          typeInfo.subTypes[index] = this.ReadType(reader);
          array[index] = typeInfo.subTypes[index].type;
        }
        if (type != (System.Type) null)
        {
          if (array == null || Array.IndexOf<System.Type>(array, (System.Type) null) != -1)
          {
            typeInfo.type = (System.Type) null;
            return typeInfo;
          }
          typeInfo.type = type.MakeGenericType(array);
        }
        else if (typeInfo.type != (System.Type) null)
        {
          System.Type[] genericArguments = typeInfo.type.GetGenericArguments();
          if (genericArguments.Length != (int) num)
            throw new InvalidOperationException("User defined generic type mismatch");
          for (int index = 0; index < (int) num; ++index)
          {
            if (array[index] != genericArguments[index])
              throw new InvalidOperationException("User defined generic type mismatch");
          }
        }
      }
      else
      {
        switch (serializationTypeInfo)
        {
          case SerializationTypeInfo.UserDefined:
          case SerializationTypeInfo.Enumeration:
            string type_name1 = reader.ReadKleiString();
            typeInfo.type = Manager.GetType(type_name1);
            break;
          case SerializationTypeInfo.SByte:
            typeInfo.type = typeof (sbyte);
            break;
          case SerializationTypeInfo.Byte:
            typeInfo.type = typeof (byte);
            break;
          case SerializationTypeInfo.Boolean:
            typeInfo.type = typeof (bool);
            break;
          case SerializationTypeInfo.Int16:
            typeInfo.type = typeof (short);
            break;
          case SerializationTypeInfo.UInt16:
            typeInfo.type = typeof (ushort);
            break;
          case SerializationTypeInfo.Int32:
            typeInfo.type = typeof (int);
            break;
          case SerializationTypeInfo.UInt32:
            typeInfo.type = typeof (uint);
            break;
          case SerializationTypeInfo.Int64:
            typeInfo.type = typeof (long);
            break;
          case SerializationTypeInfo.UInt64:
            typeInfo.type = typeof (ulong);
            break;
          case SerializationTypeInfo.Single:
            typeInfo.type = typeof (float);
            break;
          case SerializationTypeInfo.Double:
            typeInfo.type = typeof (double);
            break;
          case SerializationTypeInfo.String:
            typeInfo.type = typeof (string);
            break;
          case SerializationTypeInfo.Vector2I:
            typeInfo.type = typeof (Vector2I);
            break;
          case SerializationTypeInfo.Vector2:
            typeInfo.type = typeof (Vector2);
            break;
          case SerializationTypeInfo.Vector3:
            typeInfo.type = typeof (Vector3);
            break;
          case SerializationTypeInfo.Array:
            typeInfo.subTypes = new TypeInfo[1];
            typeInfo.subTypes[0] = this.ReadType(reader);
            typeInfo.type = !(typeInfo.subTypes[0].type != (System.Type) null) ? (System.Type) null : typeInfo.subTypes[0].type.MakeArrayType();
            break;
          case SerializationTypeInfo.Colour:
            typeInfo.type = typeof (Color);
            break;
          default:
            throw new ArgumentException("unknown type");
        }
      }
      return typeInfo;
    }

    public struct SerializedInfo
    {
      public string name;
      public TypeInfo typeInfo;
    }
  }
}
