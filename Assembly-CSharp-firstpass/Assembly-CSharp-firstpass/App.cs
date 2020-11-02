// Decompiled with JetBrains decompiler
// Type: App
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
  public static App instance;
  public static bool IsExiting = false;
  public static System.Action OnPreLoadScene;
  public static System.Action OnPostLoadScene;
  public static bool isLoading = false;
  public static bool hasFocus = true;
  public static string loadingSceneName = (string) null;
  private static string currentSceneName = (string) null;
  private float lastSuspendTime;
  private const string PIPE_NAME = "KLEI_ONI_EXIT_CODE_PIPE";
  private const string RESTART_FILENAME = "Restarter.exe";
  private static List<System.Type> types = new List<System.Type>();
  private static float[] sleepIntervals = new float[3]
  {
    8.333333f,
    16.66667f,
    33.33333f
  };

  public static string GetCurrentSceneName() => App.currentSceneName;

  private void OnApplicationQuit() => App.IsExiting = true;

  public void Restart()
  {
    string fileName = Process.GetCurrentProcess().MainModule.FileName;
    string fullPath = Path.GetFullPath(fileName);
    string directoryName = Path.GetDirectoryName(fullPath);
    Debug.LogFormat("Restarting\n\texe ({0})\n\tfull ({1})\n\tdir ({2})", (object) fileName, (object) fullPath, (object) directoryName);
    Process.Start(new ProcessStartInfo(Path.Combine(directoryName, "Restarter.exe"))
    {
      UseShellExecute = true,
      CreateNoWindow = true,
      Arguments = string.Format("\"{0}\"", (object) fullPath)
    });
    App.Quit();
  }

  static App()
  {
    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
    {
      try
      {
        foreach (System.Type type in assembly.GetTypes())
          App.types.Add(type);
      }
      catch (Exception ex)
      {
      }
    }
  }

  public static void Quit() => Application.Quit();

  private void Awake() => App.instance = this;

  public static void LoadScene(string scene_name)
  {
    Debug.Assert(!App.isLoading, (object) ("Scene [" + App.loadingSceneName + "] is already being loaded!"));
    KMonoBehaviour.isLoadingScene = true;
    App.isLoading = true;
    App.loadingSceneName = scene_name;
  }

  private void OnApplicationFocus(bool focus)
  {
    App.hasFocus = focus;
    this.lastSuspendTime = Time.realtimeSinceStartup;
  }

  public void LateUpdate()
  {
    if (App.isLoading)
    {
      KObjectManager.Instance.Cleanup();
      KMonoBehaviour.lastGameObject = (GameObject) null;
      KMonoBehaviour.lastObj = (KObject) null;
      if (SimAndRenderScheduler.instance != null)
        SimAndRenderScheduler.instance.Reset();
      Resources.UnloadUnusedAssets();
      GC.Collect();
      if (App.OnPreLoadScene != null)
        App.OnPreLoadScene();
      SceneManager.LoadScene(App.loadingSceneName);
      if (App.OnPostLoadScene != null)
        App.OnPostLoadScene();
      App.isLoading = false;
      App.currentSceneName = App.loadingSceneName;
      App.loadingSceneName = (string) null;
    }
    if (App.hasFocus || !GenericGameSettings.instance.sleepWhenOutOfFocus)
      return;
    float num1 = (float) (((double) Time.realtimeSinceStartup - (double) this.lastSuspendTime) * 1000.0);
    float num2 = 0.0f;
    for (int index = 0; index < App.sleepIntervals.Length; ++index)
    {
      num2 = App.sleepIntervals[index];
      if ((double) num2 > (double) num1)
        break;
    }
    Thread.Sleep((int) Mathf.Max(0.0f, num2 - num1));
    this.lastSuspendTime = Time.realtimeSinceStartup;
  }

  private void OnDestroy() => GlobalJobManager.Cleanup();

  public static List<System.Type> GetCurrentDomainTypes() => App.types;
}
