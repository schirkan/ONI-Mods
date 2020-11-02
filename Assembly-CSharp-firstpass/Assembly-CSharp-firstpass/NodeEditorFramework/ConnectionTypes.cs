// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.ConnectionTypes
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NodeEditorFramework
{
  public static class ConnectionTypes
  {
    private static Dictionary<string, TypeData> types;

    private static System.Type NullType => typeof (ConnectionTypes);

    public static System.Type GetType(string typeName)
    {
      System.Type type = ConnectionTypes.GetTypeData(typeName).Type;
      return (object) type != null ? type : ConnectionTypes.NullType;
    }

    public static TypeData GetTypeData(string typeName)
    {
      if (ConnectionTypes.types == null || ConnectionTypes.types.Count == 0)
        ConnectionTypes.FetchTypes();
      TypeData typeData;
      if (!ConnectionTypes.types.TryGetValue(typeName, out typeData))
      {
        System.Type type = System.Type.GetType(typeName);
        if (type == (System.Type) null)
        {
          typeData = ConnectionTypes.types.First<KeyValuePair<string, TypeData>>().Value;
          Debug.LogError((object) ("No TypeData defined for: " + typeName + " and type could not be found either"));
        }
        else
        {
          typeData = ConnectionTypes.types.Values.Count <= 0 ? (TypeData) null : ConnectionTypes.types.Values.First<TypeData>((Func<TypeData, bool>) (data => data.isValid() && data.Type == type));
          if (typeData == null)
            ConnectionTypes.types.Add(typeName, typeData = new TypeData(type));
        }
      }
      return typeData;
    }

    public static TypeData GetTypeData(System.Type type)
    {
      if (ConnectionTypes.types == null || ConnectionTypes.types.Count == 0)
        ConnectionTypes.FetchTypes();
      TypeData typeData = ConnectionTypes.types.Values.Count <= 0 ? (TypeData) null : ConnectionTypes.types.Values.First<TypeData>((Func<TypeData, bool>) (data => data.isValid() && data.Type == type));
      if (typeData == null)
        ConnectionTypes.types.Add(type.Name, typeData = new TypeData(type));
      return typeData;
    }

    internal static void FetchTypes()
    {
      ConnectionTypes.types = new Dictionary<string, TypeData>()
      {
        {
          "None",
          new TypeData(typeof (object))
        }
      };
      foreach (Assembly assembly in ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (assembly => assembly.FullName.Contains("Assembly"))))
      {
        foreach (System.Type type in ((IEnumerable<System.Type>) assembly.GetTypes()).Where<System.Type>((Func<System.Type, bool>) (T => T.IsClass && !T.IsAbstract && ((IEnumerable<System.Type>) T.GetInterfaces()).Contains<System.Type>(typeof (IConnectionTypeDeclaration)))))
        {
          if (!(assembly.CreateInstance(type.FullName) is IConnectionTypeDeclaration instance))
            throw new UnityException("Error with Type Declaration " + type.FullName);
          ConnectionTypes.types.Add(instance.Identifier, new TypeData(instance));
        }
      }
    }
  }
}
