// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Utilities.TypeConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace YamlDotNet.Serialization.Utilities
{
  public static class TypeConverter
  {
    public static T ChangeType<T>(object value) => (T) TypeConverter.ChangeType(value, typeof (T));

    public static T ChangeType<T>(object value, IFormatProvider provider) => (T) TypeConverter.ChangeType(value, typeof (T), provider);

    public static T ChangeType<T>(object value, CultureInfo culture) => (T) TypeConverter.ChangeType(value, typeof (T), culture);

    public static object ChangeType(object value, Type destinationType) => TypeConverter.ChangeType(value, destinationType, CultureInfo.InvariantCulture);

    public static object ChangeType(object value, Type destinationType, IFormatProvider provider) => TypeConverter.ChangeType(value, destinationType, (CultureInfo) new CultureInfoAdapter(CultureInfo.CurrentCulture, provider));

    public static object ChangeType(object value, Type destinationType, CultureInfo culture)
    {
      switch (value)
      {
        case null:
        case DBNull _:
          return !destinationType.IsValueType() ? (object) null : Activator.CreateInstance(destinationType);
        default:
          Type type1 = value.GetType();
          if (destinationType.IsAssignableFrom(type1))
            return value;
          if (destinationType.IsGenericType() && destinationType.GetGenericTypeDefinition() == typeof (Nullable<>))
          {
            Type genericArgument = destinationType.GetGenericArguments()[0];
            object obj = TypeConverter.ChangeType(value, genericArgument, culture);
            return Activator.CreateInstance(destinationType, obj);
          }
          if (destinationType.IsEnum())
            return !(value is string str) ? value : Enum.Parse(destinationType, str, true);
          if (destinationType == typeof (bool))
          {
            if ("0".Equals(value))
              return (object) false;
            if ("1".Equals(value))
              return (object) true;
          }
          System.ComponentModel.TypeConverter converter1 = TypeDescriptor.GetConverter(value);
          if (converter1 != null && converter1.CanConvertTo(destinationType))
            return converter1.ConvertTo((ITypeDescriptorContext) null, culture, value, destinationType);
          System.ComponentModel.TypeConverter converter2 = TypeDescriptor.GetConverter(destinationType);
          if (converter2 != null && converter2.CanConvertFrom(type1))
            return converter2.ConvertFrom((ITypeDescriptorContext) null, culture, value);
          Type[] typeArray = new Type[2]
          {
            type1,
            destinationType
          };
          foreach (Type type2 in typeArray)
          {
            foreach (MethodInfo publicStaticMethod in type2.GetPublicStaticMethods())
            {
              if ((!publicStaticMethod.IsSpecialName || !(publicStaticMethod.Name == "op_Implicit") && !(publicStaticMethod.Name == "op_Explicit") ? 0 : (destinationType.IsAssignableFrom(publicStaticMethod.ReturnParameter.ParameterType) ? 1 : 0)) != 0)
              {
                ParameterInfo[] parameters = publicStaticMethod.GetParameters();
                if ((parameters.Length != 1 ? 0 : (parameters[0].ParameterType.IsAssignableFrom(type1) ? 1 : 0)) != 0)
                {
                  try
                  {
                    return publicStaticMethod.Invoke((object) null, new object[1]
                    {
                      value
                    });
                  }
                  catch (TargetInvocationException ex)
                  {
                    throw ex.Unwrap();
                  }
                }
              }
            }
          }
          if (type1 == typeof (string))
          {
            try
            {
              MethodInfo publicStaticMethod1 = destinationType.GetPublicStaticMethod("Parse", typeof (string), typeof (IFormatProvider));
              if (publicStaticMethod1 != (MethodInfo) null)
                return publicStaticMethod1.Invoke((object) null, new object[2]
                {
                  value,
                  (object) culture
                });
              MethodInfo publicStaticMethod2 = destinationType.GetPublicStaticMethod("Parse", typeof (string));
              if (publicStaticMethod2 != (MethodInfo) null)
                return publicStaticMethod2.Invoke((object) null, new object[1]
                {
                  value
                });
            }
            catch (TargetInvocationException ex)
            {
              throw ex.Unwrap();
            }
          }
          return destinationType == typeof (TimeSpan) ? (object) TimeSpan.Parse((string) TypeConverter.ChangeType(value, typeof (string), CultureInfo.InvariantCulture)) : Convert.ChangeType(value, destinationType, (IFormatProvider) CultureInfo.InvariantCulture);
      }
    }
  }
}
