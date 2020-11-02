// Decompiled with JetBrains decompiler
// Type: KSerialization.DeserializationMapping
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace KSerialization
{
  public class DeserializationMapping
  {
    private DeserializationTemplate template;
    private List<DeserializationMapping.DeserializationInfo> deserializationInfo = new List<DeserializationMapping.DeserializationInfo>();

    public DeserializationMapping(
      DeserializationTemplate in_template,
      SerializationTemplate out_template)
    {
      this.template = in_template;
      foreach (DeserializationTemplate.SerializedInfo serializedMember in in_template.serializedMembers)
      {
        DeserializationMapping.DeserializationInfo deserializationInfo = new DeserializationMapping.DeserializationInfo();
        deserializationInfo.valid = false;
        for (int index = 0; index < out_template.serializableFields.Count; ++index)
        {
          if (out_template.serializableFields[index].field.Name == serializedMember.name)
          {
            TypeInfo typeInfo = out_template.serializableFields[index].typeInfo;
            if (serializedMember.typeInfo.Equals(typeInfo))
            {
              deserializationInfo.field = out_template.serializableFields[index].field;
              deserializationInfo.typeInfo = serializedMember.typeInfo;
              deserializationInfo.valid = true;
              break;
            }
          }
        }
        if (!deserializationInfo.valid)
        {
          for (int index = 0; index < out_template.serializableProperties.Count; ++index)
          {
            if (out_template.serializableProperties[index].property.Name == serializedMember.name)
            {
              TypeInfo typeInfo = out_template.serializableProperties[index].typeInfo;
              if (serializedMember.typeInfo.Equals(typeInfo))
              {
                PropertyInfo property = out_template.serializableProperties[index].property;
                deserializationInfo.property = property;
                deserializationInfo.typeInfo = serializedMember.typeInfo;
                deserializationInfo.valid = true;
                break;
              }
            }
          }
        }
        deserializationInfo.valid = deserializationInfo.valid && deserializationInfo.typeInfo.type != (Type) null;
        if (deserializationInfo.valid)
          deserializationInfo.typeInfo.BuildGenericArgs();
        else
          deserializationInfo.typeInfo = serializedMember.typeInfo;
        if (deserializationInfo.typeInfo.type == (Type) null)
          DebugLog.Output(DebugLog.Level.Warning, string.Format("Tried to deserialize field '{0}' on type {1} but it no longer exists", (object) serializedMember.name, (object) in_template.typeName));
        this.deserializationInfo.Add(deserializationInfo);
      }
    }

    public bool Deserialize(object obj, IReader reader)
    {
      if (obj == null)
        throw new ArgumentException("obj cannot be null");
      if (this.template.onDeserializing != (MethodInfo) null)
        this.template.onDeserializing.Invoke(obj, (object[]) null);
      foreach (DeserializationMapping.DeserializationInfo deserializationInfo in this.deserializationInfo)
      {
        if (deserializationInfo.valid)
        {
          if (deserializationInfo.field != (FieldInfo) null)
          {
            try
            {
              object base_value = deserializationInfo.field.GetValue(obj);
              object obj1 = this.ReadValue(deserializationInfo.typeInfo, reader, base_value);
              deserializationInfo.field.SetValue(obj, obj1);
            }
            catch (Exception ex)
            {
              string str = string.Format("Exception occurred while attempting to deserialize field {0}({1}) on object {2}({3}).\n{4}", (object) deserializationInfo.field, (object) deserializationInfo.field.FieldType, obj, (object) obj.GetType(), (object) ex.ToString());
              DebugLog.Output(DebugLog.Level.Error, str);
              throw new Exception(str, ex);
            }
          }
          else
          {
            if (!(deserializationInfo.property != (PropertyInfo) null))
              throw new Exception("????");
            try
            {
              object base_value = deserializationInfo.property.GetValue(obj, (object[]) null);
              object obj1 = this.ReadValue(deserializationInfo.typeInfo, reader, base_value);
              deserializationInfo.property.SetValue(obj, obj1, (object[]) null);
            }
            catch (Exception ex)
            {
              string str = string.Format("Exception occurred while attempting to deserialize property {0}({1}) on object {2}({3}).\n{4}", (object) deserializationInfo.property, (object) deserializationInfo.property.PropertyType, obj, (object) obj.GetType(), (object) ex.ToString());
              DebugLog.Output(DebugLog.Level.Error, str);
              throw new Exception(str, ex);
            }
          }
        }
        else
        {
          SerializationTypeInfo type_info = deserializationInfo.typeInfo.info & SerializationTypeInfo.VALUE_MASK;
          switch (type_info)
          {
            case SerializationTypeInfo.UserDefined:
            case SerializationTypeInfo.Pair:
              int length1 = reader.ReadInt32();
              if (length1 > 0)
              {
                reader.SkipBytes(length1);
                continue;
              }
              continue;
            case SerializationTypeInfo.Array:
            case SerializationTypeInfo.Dictionary:
            case SerializationTypeInfo.List:
            case SerializationTypeInfo.HashSet:
            case SerializationTypeInfo.Queue:
              int length2 = reader.ReadInt32();
              reader.ReadInt32();
              if (length2 > 0)
              {
                reader.SkipBytes(length2);
                continue;
              }
              continue;
            default:
              this.SkipValue(type_info, reader);
              continue;
          }
        }
      }
      if (this.template.customDeserialize != (MethodInfo) null)
        this.template.customDeserialize.Invoke(obj, new object[1]
        {
          (object) reader
        });
      if (this.template.onDeserialized != (MethodInfo) null)
        this.template.onDeserialized.Invoke(obj, (object[]) null);
      return true;
    }

    private object ReadValue(TypeInfo type_info, IReader reader, object base_value)
    {
      object obj1 = (object) null;
      SerializationTypeInfo serializationTypeInfo = type_info.info & SerializationTypeInfo.VALUE_MASK;
      Type type1 = type_info.type;
      switch (serializationTypeInfo)
      {
        case SerializationTypeInfo.UserDefined:
          if (reader.ReadInt32() >= 0)
          {
            Type type2 = type_info.type;
            obj1 = base_value != null ? base_value : (!(type2.GetConstructor(Type.EmptyTypes) != (ConstructorInfo) null) ? FormatterServices.GetUninitializedObject(type2) : Activator.CreateInstance(type2));
            Manager.GetDeserializationMapping(type2).Deserialize(obj1, reader);
            break;
          }
          break;
        case SerializationTypeInfo.SByte:
          obj1 = (object) reader.ReadSByte();
          break;
        case SerializationTypeInfo.Byte:
          obj1 = (object) reader.ReadByte();
          break;
        case SerializationTypeInfo.Boolean:
          obj1 = (object) (reader.ReadByte() == (byte) 1);
          break;
        case SerializationTypeInfo.Int16:
          obj1 = (object) reader.ReadInt16();
          break;
        case SerializationTypeInfo.UInt16:
          obj1 = (object) reader.ReadUInt16();
          break;
        case SerializationTypeInfo.Int32:
          obj1 = (object) reader.ReadInt32();
          break;
        case SerializationTypeInfo.UInt32:
          obj1 = (object) reader.ReadUInt32();
          break;
        case SerializationTypeInfo.Int64:
          obj1 = (object) reader.ReadInt64();
          break;
        case SerializationTypeInfo.UInt64:
          obj1 = (object) reader.ReadUInt64();
          break;
        case SerializationTypeInfo.Single:
          obj1 = (object) reader.ReadSingle();
          break;
        case SerializationTypeInfo.Double:
          obj1 = (object) reader.ReadDouble();
          break;
        case SerializationTypeInfo.String:
          obj1 = (object) reader.ReadKleiString();
          break;
        case SerializationTypeInfo.Enumeration:
          int num1 = reader.ReadInt32();
          obj1 = Enum.ToObject(type_info.type, num1);
          break;
        case SerializationTypeInfo.Vector2I:
          obj1 = (object) reader.ReadVector2I();
          break;
        case SerializationTypeInfo.Vector2:
          obj1 = (object) reader.ReadVector2();
          break;
        case SerializationTypeInfo.Vector3:
          obj1 = (object) reader.ReadVector3();
          break;
        case SerializationTypeInfo.Array:
          reader.ReadInt32();
          int num2 = reader.ReadInt32();
          if (num2 >= 0)
          {
            obj1 = Activator.CreateInstance(type1, (object) num2);
            Array dest_array = obj1 as Array;
            TypeInfo subType = type_info.subTypes[0];
            if (Helper.IsPOD(subType.info))
            {
              this.ReadArrayFast(dest_array, subType, reader);
              break;
            }
            if (Helper.IsValueType(subType.info))
            {
              DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(subType.type);
              object instance = Activator.CreateInstance(subType.type);
              for (int index = 0; index < num2; ++index)
              {
                deserializationMapping.Deserialize(instance, reader);
                dest_array.SetValue(instance, index);
              }
              break;
            }
            for (int index = 0; index < num2; ++index)
            {
              object obj2 = this.ReadValue(subType, reader, (object) null);
              dest_array.SetValue(obj2, index);
            }
            break;
          }
          break;
        case SerializationTypeInfo.Pair:
          if (reader.ReadInt32() >= 0)
          {
            TypeInfo subType1 = type_info.subTypes[0];
            TypeInfo subType2 = type_info.subTypes[1];
            object obj2 = this.ReadValue(subType1, reader, (object) null);
            object obj3 = this.ReadValue(subType2, reader, (object) null);
            obj1 = Activator.CreateInstance(type_info.genericInstantiationType, obj2, obj3);
            break;
          }
          break;
        case SerializationTypeInfo.Dictionary:
          reader.ReadInt32();
          int length1 = reader.ReadInt32();
          if (length1 >= 0)
          {
            obj1 = Activator.CreateInstance(type_info.genericInstantiationType);
            IDictionary dictionary = obj1 as IDictionary;
            TypeInfo subType1 = type_info.subTypes[1];
            Array instance1 = Array.CreateInstance(subType1.type, length1);
            for (int index = 0; index < length1; ++index)
            {
              object obj2 = this.ReadValue(subType1, reader, (object) null);
              instance1.SetValue(obj2, index);
            }
            TypeInfo subType2 = type_info.subTypes[0];
            Array instance2 = Array.CreateInstance(subType2.type, length1);
            for (int index = 0; index < length1; ++index)
            {
              object obj2 = this.ReadValue(subType2, reader, (object) null);
              instance2.SetValue(obj2, index);
            }
            for (int index = 0; index < length1; ++index)
              dictionary.Add(instance2.GetValue(index), instance1.GetValue(index));
            break;
          }
          break;
        case SerializationTypeInfo.List:
          reader.ReadInt32();
          int length2 = reader.ReadInt32();
          if (length2 >= 0)
          {
            TypeInfo subType = type_info.subTypes[0];
            Array instance1 = Array.CreateInstance(subType.type, length2);
            if (Helper.IsPOD(subType.info))
              this.ReadArrayFast(instance1, subType, reader);
            else if (Helper.IsValueType(subType.info))
            {
              DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(subType.type);
              object instance2 = Activator.CreateInstance(subType.type);
              for (int index = 0; index < length2; ++index)
              {
                deserializationMapping.Deserialize(instance2, reader);
                instance1.SetValue(instance2, index);
              }
            }
            else
            {
              for (int index = 0; index < length2; ++index)
              {
                object obj2 = this.ReadValue(subType, reader, (object) null);
                instance1.SetValue(obj2, index);
              }
            }
            obj1 = Activator.CreateInstance(type_info.genericInstantiationType, (object) instance1);
            break;
          }
          break;
        case SerializationTypeInfo.HashSet:
          reader.ReadInt32();
          int length3 = reader.ReadInt32();
          if (length3 >= 0)
          {
            TypeInfo subType = type_info.subTypes[0];
            Array instance1 = Array.CreateInstance(subType.type, length3);
            if (Helper.IsValueType(subType.info))
            {
              DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(subType.type);
              object instance2 = Activator.CreateInstance(subType.type);
              for (int index = 0; index < length3; ++index)
              {
                deserializationMapping.Deserialize(instance2, reader);
                instance1.SetValue(instance2, index);
              }
            }
            else
            {
              for (int index = 0; index < length3; ++index)
              {
                object obj2 = this.ReadValue(subType, reader, (object) null);
                instance1.SetValue(obj2, index);
              }
            }
            obj1 = Activator.CreateInstance(type_info.genericInstantiationType, (object) instance1);
            break;
          }
          break;
        case SerializationTypeInfo.Queue:
          reader.ReadInt32();
          int length4 = reader.ReadInt32();
          if (length4 >= 0)
          {
            TypeInfo subType = type_info.subTypes[0];
            Array instance1 = Array.CreateInstance(subType.type, length4);
            if (Helper.IsPOD(subType.info))
              this.ReadArrayFast(instance1, subType, reader);
            else if (Helper.IsValueType(subType.info))
            {
              DeserializationMapping deserializationMapping = Manager.GetDeserializationMapping(subType.type);
              object instance2 = Activator.CreateInstance(subType.type);
              for (int index = 0; index < length4; ++index)
              {
                deserializationMapping.Deserialize(instance2, reader);
                instance1.SetValue(instance2, index);
              }
            }
            else
            {
              for (int index = 0; index < length4; ++index)
              {
                object obj2 = this.ReadValue(subType, reader, (object) null);
                instance1.SetValue(obj2, index);
              }
            }
            obj1 = Activator.CreateInstance(type_info.genericInstantiationType, (object) instance1);
            break;
          }
          break;
        case SerializationTypeInfo.Colour:
          obj1 = (object) reader.ReadColour();
          break;
        default:
          throw new ArgumentException("unknown type");
      }
      return obj1;
    }

    private void ReadArrayFast(Array dest_array, TypeInfo elem_type_info, IReader reader)
    {
      byte[] numArray = reader.RawBytes();
      int position = reader.Position;
      int length = dest_array.Length;
      int num;
      switch (elem_type_info.info)
      {
        case SerializationTypeInfo.SByte:
        case SerializationTypeInfo.Byte:
          num = length;
          break;
        case SerializationTypeInfo.Int16:
        case SerializationTypeInfo.UInt16:
          num = length * 2;
          break;
        case SerializationTypeInfo.Int32:
        case SerializationTypeInfo.UInt32:
        case SerializationTypeInfo.Single:
          num = length * 4;
          break;
        case SerializationTypeInfo.Int64:
        case SerializationTypeInfo.UInt64:
        case SerializationTypeInfo.Double:
          num = length * 8;
          break;
        default:
          throw new Exception("unknown pod type");
      }
      Buffer.BlockCopy((Array) numArray, position, dest_array, 0, num);
      reader.SkipBytes(num);
    }

    private void SkipValue(SerializationTypeInfo type_info, IReader reader)
    {
      switch (type_info)
      {
        case SerializationTypeInfo.SByte:
        case SerializationTypeInfo.Byte:
        case SerializationTypeInfo.Boolean:
          reader.SkipBytes(1);
          break;
        case SerializationTypeInfo.Int16:
        case SerializationTypeInfo.UInt16:
          reader.SkipBytes(2);
          break;
        case SerializationTypeInfo.Int32:
        case SerializationTypeInfo.UInt32:
        case SerializationTypeInfo.Single:
        case SerializationTypeInfo.Enumeration:
          reader.SkipBytes(4);
          break;
        case SerializationTypeInfo.Int64:
        case SerializationTypeInfo.UInt64:
        case SerializationTypeInfo.Double:
        case SerializationTypeInfo.Vector2I:
        case SerializationTypeInfo.Vector2:
          reader.SkipBytes(8);
          break;
        case SerializationTypeInfo.String:
          int length = reader.ReadInt32();
          if (length <= 0)
            break;
          reader.SkipBytes(length);
          break;
        case SerializationTypeInfo.Vector3:
          reader.SkipBytes(12);
          break;
        case SerializationTypeInfo.Colour:
          reader.SkipBytes(4);
          break;
        default:
          throw new ArgumentException("Unhandled type. Not sure how to skip by");
      }
    }

    private struct DeserializationInfo
    {
      public bool valid;
      public FieldInfo field;
      public PropertyInfo property;
      public TypeInfo typeInfo;
    }
  }
}
