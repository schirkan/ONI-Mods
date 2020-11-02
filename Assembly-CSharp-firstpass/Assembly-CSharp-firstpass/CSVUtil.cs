// Decompiled with JetBrains decompiler
// Type: CSVUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;

public static class CSVUtil
{
  private static char[] _listSeparators = new char[5]
  {
    ',',
    ';',
    '+',
    '|',
    '\n'
  };
  private static char[] _enumSeperators = new char[5]
  {
    ',',
    ';',
    '+',
    '|',
    ' '
  };

  public static bool IsValidColumn(string[,] grid, int col) => grid[col, 0] != null && grid[col, 0] != "";

  public static void ParseData<T>(object def, string[,] grid, int row)
  {
    int length = grid.GetLength(0);
    System.Type type = typeof (T);
    for (int col = 0; col < length; ++col)
    {
      if (CSVUtil.IsValidColumn(grid, col))
      {
        try
        {
          string name = grid[col, 0];
          FieldInfo field = type.GetField(name);
          if (field != (FieldInfo) null)
          {
            string val = grid[col, row];
            if (val != null)
              CSVUtil.ParseValue(field, val, def, grid[0, row]);
          }
        }
        catch
        {
        }
      }
    }
  }

  private static void ParseValue(FieldInfo field, string val, object target, string row_name)
  {
    if (field.FieldType.IsEnum)
    {
      object obj = (object) null;
      if (val == null || !(val != "") || !CSVUtil.EnumTryParse(field.FieldType, val, out obj))
        return;
      field.SetValue(target, obj);
    }
    else if (field.FieldType == typeof (string))
      field.SetValue(target, (object) val);
    else if (field.FieldType == typeof (bool))
    {
      if (val.Contains("1"))
        field.SetValue(target, (object) true);
      else
        field.SetValue(target, (object) (val.ToLower() == "true"));
    }
    else if (field.FieldType == typeof (float))
      field.SetValue(target, (object) (float) (val == "" ? 0.0 : (double) float.Parse(val)));
    else if (field.FieldType == typeof (int))
      field.SetValue(target, (object) (val == "" ? 0 : int.Parse(val)));
    else if (field.FieldType == typeof (byte))
      field.SetValue(target, (object) byte.Parse(val));
    else if (field.FieldType == typeof (Tag))
      field.SetValue(target, (object) new Tag(val));
    else if (field.FieldType == typeof (CellOffset))
    {
      switch (val)
      {
        case "":
        case null:
          field.SetValue(target, (object) new CellOffset());
          break;
        default:
          string[] strArray1 = val.Split(',');
          field.SetValue(target, (object) new CellOffset(int.Parse(strArray1[0]), int.Parse(strArray1[1])));
          break;
      }
    }
    else if (field.FieldType == typeof (Vector3))
    {
      switch (val)
      {
        case "":
        case null:
          field.SetValue(target, (object) Vector3.zero);
          break;
        default:
          string[] strArray2 = val.Split(',');
          field.SetValue(target, (object) new Vector3(float.Parse(strArray2[0]), float.Parse(strArray2[1]), float.Parse(strArray2[2])));
          break;
      }
    }
    else
    {
      if (!typeof (Array).IsAssignableFrom(field.FieldType))
        return;
      string[] strArray3 = val.Split(CSVUtil._listSeparators);
      System.Type elementType = field.FieldType.GetElementType();
      Array.CreateInstance(elementType, strArray3.Length);
      int length = 0;
      for (int index = 0; index < strArray3.Length; ++index)
      {
        if (strArray3[index].Trim() != "")
          ++length;
      }
      Array instance = Array.CreateInstance(elementType, length);
      int index1 = 0;
      for (int index2 = 0; index2 < strArray3.Length; ++index2)
      {
        string str = strArray3[index2].Trim();
        if (str != "")
        {
          object obj = Convert.ChangeType((object) str, elementType);
          instance.SetValue(obj, index1);
          ++index1;
        }
      }
      field.SetValue(target, (object) instance);
    }
  }

  public static bool EnumTryParse(System.Type type, string input, out object value)
  {
    if (type == (System.Type) null)
      throw new ArgumentNullException(nameof (type));
    if (!type.IsEnum)
      throw new ArgumentException((string) null, nameof (type));
    if (input == null)
    {
      value = Activator.CreateInstance(type);
      return false;
    }
    input = input.Trim();
    if (input.Length == 0)
    {
      value = Activator.CreateInstance(type);
      return false;
    }
    string[] names = Enum.GetNames(type);
    if (names.Length == 0)
    {
      value = Activator.CreateInstance(type);
      return false;
    }
    System.Type underlyingType = Enum.GetUnderlyingType(type);
    Array values = Enum.GetValues(type);
    if (!type.IsDefined(typeof (FlagsAttribute), true) && input.IndexOfAny(CSVUtil._enumSeperators) < 0)
      return CSVUtil.EnumToObject(type, underlyingType, names, values, input, out value);
    string[] strArray = input.Split(CSVUtil._enumSeperators, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 0)
    {
      value = Activator.CreateInstance(type);
      return false;
    }
    ulong num1 = 0;
    foreach (string str in strArray)
    {
      string input1 = str.Trim();
      if (input1.Length != 0)
      {
        object obj;
        if (!CSVUtil.EnumToObject(type, underlyingType, names, values, input1, out obj))
        {
          value = Activator.CreateInstance(type);
          return false;
        }
        ulong num2;
        switch (Convert.GetTypeCode(obj))
        {
          case TypeCode.SByte:
          case TypeCode.Int16:
          case TypeCode.Int32:
          case TypeCode.Int64:
            num2 = (ulong) Convert.ToInt64(obj, (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          default:
            num2 = Convert.ToUInt64(obj, (IFormatProvider) CultureInfo.InvariantCulture);
            break;
        }
        num1 |= num2;
      }
    }
    value = Enum.ToObject(type, num1);
    return true;
  }

  private static object EnumToObject(System.Type underlyingType, string input)
  {
    int result1;
    if (underlyingType == typeof (int) && int.TryParse(input, out result1))
      return (object) result1;
    uint result2;
    if (underlyingType == typeof (uint) && uint.TryParse(input, out result2))
      return (object) result2;
    ulong result3;
    if (underlyingType == typeof (ulong) && ulong.TryParse(input, out result3))
      return (object) result3;
    long result4;
    if (underlyingType == typeof (long) && long.TryParse(input, out result4))
      return (object) result4;
    short result5;
    if (underlyingType == typeof (short) && short.TryParse(input, out result5))
      return (object) result5;
    ushort result6;
    if (underlyingType == typeof (ushort) && ushort.TryParse(input, out result6))
      return (object) result6;
    byte result7;
    if (underlyingType == typeof (byte) && byte.TryParse(input, out result7))
      return (object) result7;
    sbyte result8;
    return underlyingType == typeof (sbyte) && sbyte.TryParse(input, out result8) ? (object) result8 : (object) null;
  }

  private static bool EnumToObject(
    System.Type type,
    System.Type underlyingType,
    string[] names,
    Array values,
    string input,
    out object value)
  {
    for (int index = 0; index < names.Length; ++index)
    {
      if (string.Compare(names[index], input, StringComparison.OrdinalIgnoreCase) == 0)
      {
        value = values.GetValue(index);
        return true;
      }
    }
    if (char.IsDigit(input[0]) || input[0] == '-' || input[0] == '+')
    {
      object obj = CSVUtil.EnumToObject(underlyingType, input);
      if (obj == null)
      {
        value = Activator.CreateInstance(type);
        return false;
      }
      value = obj;
      return true;
    }
    value = Activator.CreateInstance(type);
    return false;
  }

  public static bool SetValue<T>(T src, ref T dest) where T : IComparable<T>
  {
    bool flag = false;
    if (!src.Equals((object) dest))
    {
      dest = src;
      flag = true;
    }
    return flag;
  }

  public static bool SetValue<T>(T[] src, ref T[] dest) where T : IComparable<T>, new()
  {
    bool flag = false;
    if (dest == null || src.Length != dest.Length)
    {
      flag = true;
      dest = new T[src.Length];
      Array.Copy((Array) src, (Array) dest, src.Length);
    }
    else
    {
      for (int index = 0; index < src.Length; ++index)
      {
        if (src[index].CompareTo(dest[index]) != 0)
        {
          flag = true;
          dest[index] = src[index];
        }
      }
    }
    return flag;
  }
}
