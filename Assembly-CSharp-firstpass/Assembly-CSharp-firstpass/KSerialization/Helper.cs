// Decompiled with JetBrains decompiler
// Type: KSerialization.Helper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace KSerialization
{
  public static class Helper
  {
    private static SerializationTypeInfo TYPE_INFO_MASK = SerializationTypeInfo.VALUE_MASK | SerializationTypeInfo.IS_GENERIC_TYPE | SerializationTypeInfo.IS_VALUE_TYPE;

    public static bool IsUserDefinedType(SerializationTypeInfo type_info) => (type_info & SerializationTypeInfo.VALUE_MASK) == SerializationTypeInfo.UserDefined;

    public static bool IsArray(SerializationTypeInfo type_info) => (type_info & SerializationTypeInfo.VALUE_MASK) == SerializationTypeInfo.Array;

    public static bool IsGenericType(SerializationTypeInfo type_info) => (uint) (type_info & SerializationTypeInfo.IS_GENERIC_TYPE) > 0U;

    public static bool IsValueType(SerializationTypeInfo type_info) => (uint) (type_info & SerializationTypeInfo.IS_VALUE_TYPE) > 0U;

    public static SerializationTypeInfo EncodeSerializationType(System.Type type)
    {
      SerializationTypeInfo serializationTypeInfo1;
      if (type == typeof (sbyte))
        serializationTypeInfo1 = SerializationTypeInfo.SByte;
      else if (type == typeof (byte))
        serializationTypeInfo1 = SerializationTypeInfo.Byte;
      else if (type == typeof (bool))
        serializationTypeInfo1 = SerializationTypeInfo.Boolean;
      else if (type == typeof (short))
        serializationTypeInfo1 = SerializationTypeInfo.Int16;
      else if (type == typeof (ushort))
        serializationTypeInfo1 = SerializationTypeInfo.UInt16;
      else if (type == typeof (int))
        serializationTypeInfo1 = SerializationTypeInfo.Int32;
      else if (type == typeof (uint))
        serializationTypeInfo1 = SerializationTypeInfo.UInt32;
      else if (type == typeof (long))
        serializationTypeInfo1 = SerializationTypeInfo.Int64;
      else if (type == typeof (ulong))
        serializationTypeInfo1 = SerializationTypeInfo.UInt64;
      else if (type == typeof (float))
        serializationTypeInfo1 = SerializationTypeInfo.Single;
      else if (type == typeof (double))
        serializationTypeInfo1 = SerializationTypeInfo.Double;
      else if (type == typeof (string))
        serializationTypeInfo1 = SerializationTypeInfo.String;
      else if (type == typeof (Vector2I))
        serializationTypeInfo1 = SerializationTypeInfo.Vector2I;
      else if (type == typeof (Vector2))
        serializationTypeInfo1 = SerializationTypeInfo.Vector2;
      else if (type == typeof (Vector3))
        serializationTypeInfo1 = SerializationTypeInfo.Vector3;
      else if (type == typeof (Color))
        serializationTypeInfo1 = SerializationTypeInfo.Colour;
      else if (typeof (Array).IsAssignableFrom(type))
        serializationTypeInfo1 = SerializationTypeInfo.Array;
      else if (type.IsEnum)
        serializationTypeInfo1 = SerializationTypeInfo.Enumeration;
      else if (type.IsGenericType)
      {
        SerializationTypeInfo serializationTypeInfo2 = SerializationTypeInfo.IS_GENERIC_TYPE;
        System.Type genericTypeDefinition = type.GetGenericTypeDefinition();
        serializationTypeInfo1 = !(genericTypeDefinition == typeof (List<>)) ? (!(genericTypeDefinition == typeof (Dictionary<,>)) ? (!(genericTypeDefinition == typeof (HashSet<>)) ? (!(genericTypeDefinition == typeof (KeyValuePair<,>)) ? (!(genericTypeDefinition == typeof (Queue<>)) ? serializationTypeInfo2 | SerializationTypeInfo.UserDefined : serializationTypeInfo2 | SerializationTypeInfo.Queue) : serializationTypeInfo2 | SerializationTypeInfo.Pair) : serializationTypeInfo2 | SerializationTypeInfo.HashSet) : serializationTypeInfo2 | SerializationTypeInfo.Dictionary) : serializationTypeInfo2 | SerializationTypeInfo.List;
      }
      else
      {
        serializationTypeInfo1 = SerializationTypeInfo.UserDefined;
        if (type.IsValueType)
          serializationTypeInfo1 |= SerializationTypeInfo.IS_VALUE_TYPE;
      }
      return serializationTypeInfo1 & Helper.TYPE_INFO_MASK;
    }

    public static void WriteValue(this BinaryWriter writer, TypeInfo type_info, object value)
    {
      switch (type_info.info & SerializationTypeInfo.VALUE_MASK)
      {
        case SerializationTypeInfo.UserDefined:
          if (value != null)
          {
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            long position2 = writer.BaseStream.Position;
            Manager.GetSerializationTemplate(type_info.type).SerializeData(value, writer);
            long position3 = writer.BaseStream.Position;
            long num = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(-1);
          break;
        case SerializationTypeInfo.SByte:
          writer.Write((sbyte) value);
          break;
        case SerializationTypeInfo.Byte:
          writer.Write((byte) value);
          break;
        case SerializationTypeInfo.Boolean:
          writer.Write((bool) value ? (byte) 1 : (byte) 0);
          break;
        case SerializationTypeInfo.Int16:
          writer.Write((short) value);
          break;
        case SerializationTypeInfo.UInt16:
          writer.Write((ushort) value);
          break;
        case SerializationTypeInfo.Int32:
          writer.Write((int) value);
          break;
        case SerializationTypeInfo.UInt32:
          writer.Write((uint) value);
          break;
        case SerializationTypeInfo.Int64:
          writer.Write((long) value);
          break;
        case SerializationTypeInfo.UInt64:
          writer.Write((ulong) value);
          break;
        case SerializationTypeInfo.Single:
          writer.WriteSingleFast((float) value);
          break;
        case SerializationTypeInfo.Double:
          writer.Write((double) value);
          break;
        case SerializationTypeInfo.String:
          writer.WriteKleiString((string) value);
          break;
        case SerializationTypeInfo.Enumeration:
          writer.Write((int) value);
          break;
        case SerializationTypeInfo.Vector2I:
          Vector2I vector2I = (Vector2I) value;
          writer.Write(vector2I.x);
          writer.Write(vector2I.y);
          break;
        case SerializationTypeInfo.Vector2:
          Vector2 vector2 = (Vector2) value;
          writer.WriteSingleFast(vector2.x);
          writer.WriteSingleFast(vector2.y);
          break;
        case SerializationTypeInfo.Vector3:
          Vector3 vector3 = (Vector3) value;
          writer.WriteSingleFast(vector3.x);
          writer.WriteSingleFast(vector3.y);
          writer.WriteSingleFast(vector3.z);
          break;
        case SerializationTypeInfo.Array:
          if (value != null)
          {
            Array array = value as Array;
            TypeInfo subType = type_info.subTypes[0];
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(array.Length);
            long position2 = writer.BaseStream.Position;
            if (Helper.IsPOD(subType.info))
              Helper.WriteArrayFast(writer, subType, array);
            else if (Helper.IsValueType(subType.info))
            {
              SerializationTemplate serializationTemplate = Manager.GetSerializationTemplate(subType.type);
              for (int index = 0; index < array.Length; ++index)
                serializationTemplate.SerializeData(array.GetValue(index), writer);
            }
            else
            {
              for (int index = 0; index < array.Length; ++index)
                writer.WriteValue(subType, array.GetValue(index));
            }
            long position3 = writer.BaseStream.Position;
            long num = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(4);
          writer.Write(-1);
          break;
        case SerializationTypeInfo.Pair:
          if (value != null)
          {
            PropertyInfo property1 = type_info.type.GetProperty("Key");
            PropertyInfo property2 = type_info.type.GetProperty("Value");
            object obj1 = value;
            object obj2 = property1.GetValue(obj1, (object[]) null);
            object obj3 = property2.GetValue(value, (object[]) null);
            TypeInfo subType1 = type_info.subTypes[0];
            TypeInfo subType2 = type_info.subTypes[1];
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            long position2 = writer.BaseStream.Position;
            writer.WriteValue(subType1, obj2);
            writer.WriteValue(subType2, obj3);
            long position3 = writer.BaseStream.Position;
            long num = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(4);
          writer.Write(-1);
          break;
        case SerializationTypeInfo.Dictionary:
          if (value != null)
          {
            TypeInfo subType1 = type_info.subTypes[0];
            TypeInfo subType2 = type_info.subTypes[1];
            IDictionary dictionary = value as IDictionary;
            ICollection keys = dictionary.Keys;
            ICollection values = dictionary.Values;
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(values.Count);
            long position2 = writer.BaseStream.Position;
            foreach (object obj in (IEnumerable) values)
              writer.WriteValue(subType2, obj);
            foreach (object obj in (IEnumerable) keys)
              writer.WriteValue(subType1, obj);
            long position3 = writer.BaseStream.Position;
            long num = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(4);
          writer.Write(-1);
          break;
        case SerializationTypeInfo.List:
          if (value != null)
          {
            TypeInfo subType = type_info.subTypes[0];
            ICollection collection = value as ICollection;
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(collection.Count);
            long position2 = writer.BaseStream.Position;
            if (Helper.IsPOD(subType.info))
              Helper.WriteListPOD(writer, subType, collection);
            else if (Helper.IsValueType(subType.info))
            {
              SerializationTemplate serializationTemplate = Manager.GetSerializationTemplate(subType.type);
              foreach (object obj in (IEnumerable) collection)
                serializationTemplate.SerializeData(obj, writer);
            }
            else
            {
              foreach (object obj in (IEnumerable) collection)
                writer.WriteValue(subType, obj);
            }
            long position3 = writer.BaseStream.Position;
            long num = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(4);
          writer.Write(-1);
          break;
        case SerializationTypeInfo.HashSet:
          if (value != null)
          {
            TypeInfo subType = type_info.subTypes[0];
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(0);
            long position2 = writer.BaseStream.Position;
            int num1 = 0;
            IEnumerable enumerable = value as IEnumerable;
            if (Helper.IsValueType(subType.info))
            {
              SerializationTemplate serializationTemplate = Manager.GetSerializationTemplate(subType.type);
              foreach (object obj in enumerable)
              {
                serializationTemplate.SerializeData(obj, writer);
                ++num1;
              }
            }
            else
            {
              foreach (object obj in enumerable)
              {
                writer.WriteValue(subType, obj);
                ++num1;
              }
            }
            long position3 = writer.BaseStream.Position;
            long num2 = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num2);
            writer.Write(num1);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(4);
          writer.Write(-1);
          break;
        case SerializationTypeInfo.Queue:
          if (value != null)
          {
            TypeInfo subType = type_info.subTypes[0];
            ICollection collection = value as ICollection;
            long position1 = writer.BaseStream.Position;
            writer.Write(0);
            writer.Write(collection.Count);
            long position2 = writer.BaseStream.Position;
            if (Helper.IsPOD(subType.info))
              Helper.WriteListPOD(writer, subType, collection);
            else if (Helper.IsValueType(subType.info))
            {
              SerializationTemplate serializationTemplate = Manager.GetSerializationTemplate(subType.type);
              foreach (object obj in (IEnumerable) collection)
                serializationTemplate.SerializeData(obj, writer);
            }
            else
            {
              foreach (object obj in (IEnumerable) collection)
                writer.WriteValue(subType, obj);
            }
            long position3 = writer.BaseStream.Position;
            long num = position3 - position2;
            writer.BaseStream.Position = position1;
            writer.Write((int) num);
            writer.BaseStream.Position = position3;
            break;
          }
          writer.Write(4);
          writer.Write(-1);
          break;
        case SerializationTypeInfo.Colour:
          Color color = (Color) value;
          writer.Write((byte) ((double) color.r * (double) byte.MaxValue));
          writer.Write((byte) ((double) color.g * (double) byte.MaxValue));
          writer.Write((byte) ((double) color.b * (double) byte.MaxValue));
          writer.Write((byte) ((double) color.a * (double) byte.MaxValue));
          break;
        default:
          throw new ArgumentException("Don't know how to serialize type: " + type_info.type.ToString());
      }
    }

    private static void WriteArrayFast(BinaryWriter writer, TypeInfo elem_type_info, Array array)
    {
      switch (elem_type_info.info)
      {
        case SerializationTypeInfo.SByte:
          sbyte[] numArray1 = (sbyte[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray1[index]);
          break;
        case SerializationTypeInfo.Byte:
          writer.Write((byte[]) array);
          break;
        case SerializationTypeInfo.Int16:
          short[] numArray2 = (short[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray2[index]);
          break;
        case SerializationTypeInfo.UInt16:
          ushort[] numArray3 = (ushort[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray3[index]);
          break;
        case SerializationTypeInfo.Int32:
          int[] numArray4 = (int[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray4[index]);
          break;
        case SerializationTypeInfo.UInt32:
          uint[] numArray5 = (uint[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray5[index]);
          break;
        case SerializationTypeInfo.Int64:
          long[] numArray6 = (long[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray6[index]);
          break;
        case SerializationTypeInfo.UInt64:
          ulong[] numArray7 = (ulong[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray7[index]);
          break;
        case SerializationTypeInfo.Single:
          float[] numArray8 = (float[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.WriteSingleFast(numArray8[index]);
          break;
        case SerializationTypeInfo.Double:
          double[] numArray9 = (double[]) array;
          for (int index = 0; index < array.Length; ++index)
            writer.Write(numArray9[index]);
          break;
        default:
          throw new Exception("unknown pod type");
      }
    }

    private static void WriteListPOD(
      BinaryWriter writer,
      TypeInfo elem_type_info,
      ICollection collection)
    {
      switch (elem_type_info.info)
      {
        case SerializationTypeInfo.SByte:
          IEnumerator enumerator1 = collection.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              object current = enumerator1.Current;
              writer.Write((sbyte) current);
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.Byte:
          IEnumerator enumerator2 = collection.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              object current = enumerator2.Current;
              writer.Write((byte) current);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.Int16:
          IEnumerator enumerator3 = collection.GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              object current = enumerator3.Current;
              writer.Write((short) current);
            }
            break;
          }
          finally
          {
            if (enumerator3 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.UInt16:
          IEnumerator enumerator4 = collection.GetEnumerator();
          try
          {
            while (enumerator4.MoveNext())
            {
              object current = enumerator4.Current;
              writer.Write((ushort) current);
            }
            break;
          }
          finally
          {
            if (enumerator4 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.Int32:
          IEnumerator enumerator5 = collection.GetEnumerator();
          try
          {
            while (enumerator5.MoveNext())
            {
              object current = enumerator5.Current;
              writer.Write((int) current);
            }
            break;
          }
          finally
          {
            if (enumerator5 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.UInt32:
          IEnumerator enumerator6 = collection.GetEnumerator();
          try
          {
            while (enumerator6.MoveNext())
            {
              object current = enumerator6.Current;
              writer.Write((uint) current);
            }
            break;
          }
          finally
          {
            if (enumerator6 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.Int64:
          IEnumerator enumerator7 = collection.GetEnumerator();
          try
          {
            while (enumerator7.MoveNext())
            {
              object current = enumerator7.Current;
              writer.Write((long) current);
            }
            break;
          }
          finally
          {
            if (enumerator7 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.UInt64:
          IEnumerator enumerator8 = collection.GetEnumerator();
          try
          {
            while (enumerator8.MoveNext())
            {
              object current = enumerator8.Current;
              writer.Write((ulong) current);
            }
            break;
          }
          finally
          {
            if (enumerator8 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.Single:
          IEnumerator enumerator9 = collection.GetEnumerator();
          try
          {
            while (enumerator9.MoveNext())
            {
              object current = enumerator9.Current;
              writer.WriteSingleFast((float) current);
            }
            break;
          }
          finally
          {
            if (enumerator9 is IDisposable disposable)
              disposable.Dispose();
          }
        case SerializationTypeInfo.Double:
          IEnumerator enumerator10 = collection.GetEnumerator();
          try
          {
            while (enumerator10.MoveNext())
            {
              object current = enumerator10.Current;
              writer.Write((double) current);
            }
            break;
          }
          finally
          {
            if (enumerator10 is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }

    public static void GetSerializationMethods(
      this System.Type type,
      System.Type type_a,
      System.Type type_b,
      System.Type type_c,
      out MethodInfo method_a,
      out MethodInfo method_b,
      out MethodInfo method_c)
    {
      method_a = (MethodInfo) null;
      method_b = (MethodInfo) null;
      method_c = (MethodInfo) null;
      foreach (MethodInfo method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        object[] customAttributes = method.GetCustomAttributes(false);
        for (int index = 0; index < customAttributes.Length; ++index)
        {
          if (customAttributes[index].GetType() == type_a)
            method_a = method;
          else if (customAttributes[index].GetType() == type_b)
            method_b = method;
          else if (customAttributes[index].GetType() == type_c)
            method_c = method;
        }
      }
    }

    public static bool IsPOD(SerializationTypeInfo info)
    {
      switch (info)
      {
        case SerializationTypeInfo.SByte:
        case SerializationTypeInfo.Byte:
        case SerializationTypeInfo.Int16:
        case SerializationTypeInfo.UInt16:
        case SerializationTypeInfo.Int32:
        case SerializationTypeInfo.UInt32:
        case SerializationTypeInfo.Int64:
        case SerializationTypeInfo.UInt64:
        case SerializationTypeInfo.Single:
        case SerializationTypeInfo.Double:
          return true;
        default:
          return false;
      }
    }

    public static bool IsPOD(System.Type type) => type == typeof (int) || type == typeof (uint) || (type == typeof (byte) || type == typeof (sbyte)) || (type == typeof (float) || type == typeof (double) || (type == typeof (short) || type == typeof (ushort))) || (type == typeof (long) || type == typeof (ulong));

    public static string GetKTypeString(this System.Type type) => type.FullName;

    public static void ClearTypeInfoMask() => Helper.TYPE_INFO_MASK = SerializationTypeInfo.VALUE_MASK | SerializationTypeInfo.IS_GENERIC_TYPE | SerializationTypeInfo.IS_VALUE_TYPE;

    public static void SetTypeInfoMask(SerializationTypeInfo mask) => Helper.TYPE_INFO_MASK = mask;
  }
}
