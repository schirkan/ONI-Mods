// Decompiled with JetBrains decompiler
// Type: Klei.GenericGameSettings
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

namespace Klei
{
  public class GenericGameSettings
  {
    private static GenericGameSettings _instance;

    public static GenericGameSettings instance
    {
      get
      {
        if (GenericGameSettings._instance == null)
        {
          try
          {
            GenericGameSettings._instance = YamlIO.LoadFile<GenericGameSettings>(GenericGameSettings.Path);
            Debug.Assert(GenericGameSettings._instance != null, (object) ("Loading " + GenericGameSettings.Path + " returned null, the file may be corrupted"));
          }
          catch
          {
            GenericGameSettings._instance = new GenericGameSettings();
          }
        }
        return GenericGameSettings._instance;
      }
    }

    public bool demoMode { get; private set; }

    public bool sleepWhenOutOfFocus { get; private set; }

    public int demoTime { get; private set; }

    public bool showDemoTimer { get; private set; }

    public bool debugEnable { get; private set; }

    public bool developerDebugEnable { get; private set; }

    public bool disableGameOver { get; private set; }

    public bool disablePopFx { get; private set; }

    public bool autoResumeGame { get; private set; }

    public bool disableFogOfWar { get; private set; }

    public bool acceleratedLifecycle { get; private set; }

    public bool enableEditorCrashReporting { get; private set; }

    public bool allowInsufficientMaterialBuild { get; private set; }

    public bool keepAllAutosaves { get; private set; }

    public bool takeSaveScreenshots { get; private set; }

    public bool disableAutosave { get; private set; }

    public GenericGameSettings.PerformanceCapture performanceCapture { get; set; }

    private static string Path => System.IO.Path.GetDirectoryName(Application.dataPath) + "/settings.yml";

    public GenericGameSettings()
    {
      this.demoMode = false;
      this.demoTime = 300;
      this.showDemoTimer = true;
      this.sleepWhenOutOfFocus = true;
      this.debugEnable = false;
      this.developerDebugEnable = false;
      this.performanceCapture = new GenericGameSettings.PerformanceCapture();
      GenericGameSettings._instance = this;
    }

    public void SaveSettings()
    {
      try
      {
        YamlIO.Save<GenericGameSettings>(this, GenericGameSettings.Path);
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ("Failed to save settings.yml: " + ex.ToString()));
      }
    }

    public class PerformanceCapture
    {
      public string saveGame { get; set; }

      public float waitTime { get; set; }

      public bool gcStats { get; set; }
    }
  }
}
