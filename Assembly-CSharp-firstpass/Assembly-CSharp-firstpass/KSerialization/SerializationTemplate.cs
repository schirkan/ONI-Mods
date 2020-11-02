// Decompiled with JetBrains decompiler
// Type: KSerialization.SerializationTemplate
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

namespace KSerialization
{
  public class SerializationTemplate
  {
    public Type serializableType;
    public TypeInfo typeInfo;
    public List<SerializationTemplate.SerializationField> serializableFields = new List<SerializationTemplate.SerializationField>();
    public List<SerializationTemplate.SerializationProperty> serializableProperties = new List<SerializationTemplate.SerializationProperty>();
    public MethodInfo onSerializing;
    public MethodInfo onSerialized;
    public MethodInfo customSerialize;

    private MemberSerialization GetSerializationConfig(Type type)
    {
      MemberSerialization memberSerialization = MemberSerialization.Invalid;
      Type type1 = (Type) null;
      for (; type != typeof (object); type = type.BaseType)
      {
        foreach (Attribute customAttribute in type.GetCustomAttributes(typeof (SerializationConfig), false))
        {
          if (customAttribute is SerializationConfig)
          {
            SerializationConfig serializationConfig = customAttribute as SerializationConfig;
            if (serializationConfig.MemberSerialization != memberSerialization && memberSerialization != MemberSerialization.Invalid)
            {
              string message = "Found conflicting serialization configurations on type " + type1.ToString() + " and " + type.ToString();
              Debug.LogError((object) message);
              throw new ArgumentException(message);
            }
            memberSerialization = serializationConfig.MemberSerialization;
            type1 = type.BaseType;
            break;
          }
        }
      }
      if (memberSerialization == MemberSerialization.Invalid)
        memberSerialization = MemberSerialization.OptOut;
      return memberSerialization;
    }

    public SerializationTemplate(Type type)
    {
      this.serializableType = type;
      this.typeInfo = Manager.GetTypeInfo(type);
      type.GetSerializationMethods(typeof (OnSerializingAttribute), typeof (OnSerializedAttribute), typeof (CustomSerialize), out this.onSerializing, out this.onSerialized, out this.customSerialize);
      switch (this.GetSerializationConfig(type))
      {
        case MemberSerialization.OptOut:
          for (; type != typeof (object); type = type.BaseType)
          {
            this.AddPublicFields(type);
            this.AddPublicProperties(type);
          }
          break;
        case MemberSerialization.OptIn:
          for (; type != typeof (object); type = type.BaseType)
          {
            this.AddOptInFields(type);
            this.AddOptInProperties(type);
          }
          break;
      }
    }

    public override string ToString()
    {
      string str = "Template: " + this.serializableType.ToString() + "\n";
      foreach (SerializationTemplate.SerializationField serializableField in this.serializableFields)
        str = str + "\t" + serializableField.ToString() + "\n";
      return str;
    }

    private void AddPublicFields(Type type)
    {
      foreach (FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
        this.AddValidField(field);
    }

    private void AddOptInFields(Type type)
    {
      foreach (FieldInfo field in type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        foreach (object customAttribute in field.GetCustomAttributes(false))
        {
          if (customAttribute != null && customAttribute is Serialize)
            this.AddValidField(field);
        }
      }
    }

    private void AddValidField(FieldInfo field)
    {
      object[] customAttributes = field.GetCustomAttributes(typeof (NonSerializedAttribute), false);
      if ((customAttributes == null ? 0 : ((uint) customAttributes.Length > 0U ? 1 : 0)) != 0)
        return;
      this.serializableFields.Add(new SerializationTemplate.SerializationField()
      {
        field = field,
        typeInfo = Manager.GetTypeInfo(field.FieldType)
      });
    }

    private void AddPublicProperties(Type type)
    {
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public))
        this.AddValidProperty(property);
    }

    private void AddOptInProperties(Type type)
    {
      foreach (PropertyInfo property in type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        foreach (object customAttribute in property.GetCustomAttributes(false))
        {
          if (customAttribute != null && customAttribute is Serialize)
            this.AddValidProperty(property);
        }
      }
    }

    private void AddValidProperty(PropertyInfo property)
    {
      object[] customAttributes = property.GetCustomAttributes(typeof (NonSerializedAttribute), false);
      if ((customAttributes == null ? 0 : ((uint) customAttributes.Length > 0U ? 1 : 0)) != 0 || !(property.GetSetMethod() != (MethodInfo) null))
        return;
      this.serializableProperties.Add(new SerializationTemplate.SerializationProperty()
      {
        property = property,
        typeInfo = Manager.GetTypeInfo(property.PropertyType)
      });
    }

    public void SerializeTemplate(BinaryWriter writer)
    {
      writer.Write(this.serializableFields.Count);
      writer.Write(this.serializableProperties.Count);
      foreach (SerializationTemplate.SerializationField serializableField in this.serializableFields)
      {
        writer.WriteKleiString(serializableField.field.Name);
        Type fieldType = serializableField.field.FieldType;
        this.WriteType(writer, fieldType);
      }
      foreach (SerializationTemplate.SerializationProperty serializableProperty in this.serializableProperties)
      {
        writer.WriteKleiString(serializableProperty.property.Name);
        Type propertyType = serializableProperty.property.PropertyType;
        this.WriteType(writer, propertyType);
      }
    }

    private void WriteType(BinaryWriter writer, Type type)
    {
      SerializationTypeInfo type_info = Helper.EncodeSerializationType(type);
      writer.Write((byte) type_info);
      if (type.IsGenericType)
      {
        if (Helper.IsUserDefinedType(type_info))
          writer.WriteKleiString(type.GetKTypeString());
        Type[] genericArguments = type.GetGenericArguments();
        writer.Write((byte) genericArguments.Length);
        for (int index = 0; index < genericArguments.Length; ++index)
          this.WriteType(writer, genericArguments[index]);
      }
      else if (Helper.IsArray(type_info))
      {
        Type elementType = type.GetElementType();
        this.WriteType(writer, elementType);
      }
      else
      {
        if (!type.IsEnum && !Helper.IsUserDefinedType(type_info))
          return;
        writer.WriteKleiString(type.GetKTypeString());
      }
    }

    public void SerializeData(object obj, BinaryWriter writer)
    {
      if (this.onSerializing != (MethodInfo) null)
        this.onSerializing.Invoke(obj, (object[]) null);
      foreach (SerializationTemplate.SerializationField serializableField in this.serializableFields)
      {
        try
        {
          object obj1 = serializableField.field.GetValue(obj);
          writer.WriteValue(serializableField.typeInfo, obj1);
        }
        catch (Exception ex)
        {
          string message = string.Format("Error occurred while serializing field {0} on template {1}", (object) serializableField.field.Name, (object) this.serializableType.Name);
          Debug.LogError((object) message);
          throw new ArgumentException(message, ex);
        }
      }
      foreach (SerializationTemplate.SerializationProperty serializableProperty in this.serializableProperties)
      {
        try
        {
          object obj1 = serializableProperty.property.GetValue(obj, (object[]) null);
          writer.WriteValue(serializableProperty.typeInfo, obj1);
        }
        catch (Exception ex)
        {
          string message = string.Format("Error occurred while serializing property {0} on template {1}", (object) serializableProperty.property.Name, (object) this.serializableType.Name);
          Debug.LogError((object) message);
          throw new ArgumentException(message, ex);
        }
      }
      if (this.customSerialize != (MethodInfo) null)
        this.customSerialize.Invoke(obj, new object[1]
        {
          (object) writer
        });
      if (!(this.onSerialized != (MethodInfo) null))
        return;
      this.onSerialized.Invoke(obj, (object[]) null);
    }

    public struct SerializationField
    {
      public FieldInfo field;
      public TypeInfo typeInfo;
    }

    public struct SerializationProperty
    {
      public PropertyInfo property;
      public TypeInfo typeInfo;
    }
  }
}
