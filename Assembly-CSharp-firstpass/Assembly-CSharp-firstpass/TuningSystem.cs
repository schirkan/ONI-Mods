// Decompiled with JetBrains decompiler
// Type: TuningSystem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

public class TuningSystem
{
  private static JsonSerializerSettings _SerializationSettings = new JsonSerializerSettings()
  {
    Formatting = Formatting.Indented
  };
  private static Dictionary<System.Type, object> _TuningValues;
  private static string _TuningPath = Path.Combine(Application.streamingAssetsPath, "Tuning.json");

  public static void Init()
  {
  }

  static TuningSystem()
  {
    TuningSystem.InitializeTuning();
    TuningSystem.ListenForFileChanges();
    TuningSystem.Load();
  }

  private static void ListenForFileChanges()
  {
    string directoryName = Path.GetDirectoryName(TuningSystem._TuningPath);
    try
    {
      FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
      fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
      fileSystemWatcher.Changed += new FileSystemEventHandler(TuningSystem.OnFileChanged);
      fileSystemWatcher.Path = directoryName;
      fileSystemWatcher.Filter = Path.GetFileName(TuningSystem._TuningPath);
      fileSystemWatcher.EnableRaisingEvents = true;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("Error when attempting to monitor path: " + directoryName + "\n" + ex.ToString()));
    }
  }

  private static void OnFileChanged(object source, FileSystemEventArgs e) => TuningSystem.Load();

  private static void InitializeTuning()
  {
    TuningSystem._TuningValues = new Dictionary<System.Type, object>();
    foreach (System.Type currentDomainType in App.GetCurrentDomainTypes())
    {
      System.Type baseType = currentDomainType.BaseType;
      if (!(baseType == (System.Type) null) && baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof (TuningData<>))
      {
        object instance = Activator.CreateInstance(baseType.GetGenericArguments()[0]);
        TuningSystem._TuningValues[instance.GetType()] = instance;
        baseType.GetField("_TuningData", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).SetValue((object) null, instance);
      }
    }
  }

  private static void Save()
  {
    if (TuningSystem._TuningValues == null)
      return;
    JsonSerializer jsonSerializer = JsonSerializer.Create(TuningSystem._SerializationSettings);
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    foreach (KeyValuePair<System.Type, object> tuningValue in TuningSystem._TuningValues)
      dictionary[tuningValue.Value.GetType().FullName] = tuningValue.Value;
    using (StreamWriter text = File.CreateText(TuningSystem._TuningPath))
      jsonSerializer.Serialize((TextWriter) text, (object) dictionary);
  }

  private static void Load()
  {
    string[] strArray = new string[2]
    {
      TuningSystem._TuningPath,
      string.Empty
    };
    if (Thread.CurrentThread == KProfiler.main_thread)
      strArray[1] = Path.Combine(Application.dataPath, "Tuning.json");
    foreach (string str in strArray)
    {
      if (!string.IsNullOrEmpty(str) && FileSystem.FileExists(str))
      {
        foreach (KeyValuePair<string, object> keyValuePair in JsonConvert.DeserializeObject<Dictionary<string, object>>(FileSystem.ConvertToText(FileSystem.ReadBytes(str))))
        {
          System.Type type = (System.Type) null;
          foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
          {
            type = assembly.GetType(keyValuePair.Key);
            if (type != (System.Type) null)
              break;
          }
          if (type != (System.Type) null)
          {
            if (TuningSystem._TuningValues.ContainsKey(type))
            {
              JsonConvert.PopulateObject(keyValuePair.Value.ToString(), TuningSystem._TuningValues[type]);
            }
            else
            {
              object obj = JsonConvert.DeserializeObject(keyValuePair.Value.ToString(), type);
              TuningSystem._TuningValues[type] = obj;
            }
          }
        }
      }
    }
  }

  private static bool IsLoaded() => TuningSystem._TuningValues != null;

  public static Dictionary<System.Type, object> GetAllTuningValues() => TuningSystem._TuningValues;
}
