// Decompiled with JetBrains decompiler
// Type: KMod.DLLLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KMod
{
  internal static class DLLLoader
  {
    private const string managed_path = "Managed";

    public static bool LoadUserModLoaderDLL()
    {
      try
      {
        string path = System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "Managed"), "ModLoader.dll");
        if (!File.Exists(path))
          return false;
        Assembly assembly = Assembly.LoadFile(path);
        if (assembly == (Assembly) null)
          return false;
        System.Type type = assembly.GetType("ModLoader.ModLoader");
        if (type == (System.Type) null)
          return false;
        MethodInfo method = type.GetMethod("Start");
        if (method == (MethodInfo) null)
          return false;
        method.Invoke((object) null, (object[]) null);
        Debug.Log((object) "Successfully started ModLoader.dll");
        return true;
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.ToString());
      }
      return false;
    }

    public static LoadedModData LoadDLLs(string harmonyId, string path)
    {
      LoadedModData loadedModData = new LoadedModData();
      try
      {
        if (Testing.dll_loading == Testing.DLLLoading.Fail || Testing.dll_loading == Testing.DLLLoading.UseModLoaderDLLExclusively)
          return (LoadedModData) null;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists)
          return (LoadedModData) null;
        List<Assembly> assemblyList = new List<Assembly>();
        foreach (FileInfo file in directoryInfo.GetFiles())
        {
          if (file.Name.ToLower().EndsWith(".dll"))
          {
            Debug.Log((object) string.Format("Loading MOD dll: {0}", (object) file.Name));
            Assembly assembly = Assembly.LoadFrom(file.FullName);
            if (assembly != (Assembly) null)
              assemblyList.Add(assembly);
          }
        }
        if (assemblyList.Count == 0)
          return (LoadedModData) null;
        ListPool<MethodInfo, Manager>.PooledList pooledList1 = ListPool<MethodInfo, Manager>.Allocate();
        ListPool<MethodInfo, Manager>.PooledList pooledList2 = ListPool<MethodInfo, Manager>.Allocate();
        ListPool<MethodInfo, Manager>.PooledList pooledList3 = ListPool<MethodInfo, Manager>.Allocate();
        ListPool<MethodInfo, Manager>.PooledList pooledList4 = ListPool<MethodInfo, Manager>.Allocate();
        System.Type[] types1 = new System.Type[0];
        System.Type[] types2 = new System.Type[1]
        {
          typeof (string)
        };
        System.Type[] types3 = new System.Type[1]
        {
          typeof (HarmonyInstance)
        };
        loadedModData.dlls = (ICollection<Assembly>) new HashSet<Assembly>();
        foreach (Assembly assembly in assemblyList)
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (!(type == (System.Type) null))
            {
              MethodInfo method1 = type.GetMethod("OnLoad", types1);
              if (method1 != (MethodInfo) null)
                pooledList3.Add(method1);
              MethodInfo method2 = type.GetMethod("OnLoad", types2);
              if (method2 != (MethodInfo) null)
                pooledList4.Add(method2);
              MethodInfo method3 = type.GetMethod("PrePatch", types3);
              if (method3 != (MethodInfo) null)
                pooledList1.Add(method3);
              MethodInfo method4 = type.GetMethod("PostPatch", types3);
              if (method4 != (MethodInfo) null)
                pooledList2.Add(method4);
            }
          }
          loadedModData.dlls.Add(assembly);
        }
        HarmonyInstance harmony = HarmonyInstance.Create(harmonyId);
        if (harmony != null)
        {
          object[] parameters = new object[1]
          {
            (object) harmony
          };
          foreach (MethodBase methodBase in (List<MethodInfo>) pooledList1)
            methodBase.Invoke((object) null, parameters);
          foreach (Assembly assembly in assemblyList)
            harmony.PatchAll(assembly);
          foreach (MethodBase methodBase in (List<MethodInfo>) pooledList2)
            methodBase.Invoke((object) null, parameters);
        }
        pooledList1.Recycle();
        pooledList2.Recycle();
        loadedModData.patched_methods = (ICollection<MethodBase>) harmony.GetPatchedMethods().Where<MethodBase>((Func<MethodBase, bool>) (method => harmony.GetPatchInfo(method).Owners.Contains(harmonyId))).ToList<MethodBase>();
        foreach (MethodBase methodBase in (List<MethodInfo>) pooledList3)
          methodBase.Invoke((object) null, (object[]) null);
        object[] parameters1 = new object[1]
        {
          (object) path
        };
        foreach (MethodBase methodBase in (List<MethodInfo>) pooledList4)
          methodBase.Invoke((object) null, parameters1);
        pooledList3.Recycle();
        pooledList4.Recycle();
        return loadedModData;
      }
      catch (Exception ex)
      {
        DebugUtil.LogException((UnityEngine.Object) null, "Exception while loading mod " + harmonyId + " at " + path + ".", ex);
        return (LoadedModData) null;
      }
    }
  }
}
