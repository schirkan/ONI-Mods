// Decompiled with JetBrains decompiler
// Type: KPrivacyPrefs
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using System;
using System.IO;

public class KPrivacyPrefs
{
  private static KPrivacyPrefs _instance;
  public static readonly string FILENAME = "kprivacyprefs.yaml";

  public bool disableDataCollection { get; set; }

  public static KPrivacyPrefs instance
  {
    get
    {
      if (KPrivacyPrefs._instance == null)
        KPrivacyPrefs.Load();
      return KPrivacyPrefs._instance;
    }
  }

  public static string GetPath() => Path.Combine(KPrivacyPrefs.GetDirectory(), KPrivacyPrefs.FILENAME);

  public static string GetDirectory() => Path.Combine(Path.Combine(Util.GetKleiRootPath(), "Agreements"), Util.GetTitleFolderName());

  public static void Save()
  {
    try
    {
      if (!Directory.Exists(KPrivacyPrefs.GetDirectory()))
        Directory.CreateDirectory(KPrivacyPrefs.GetDirectory());
      YamlIO.SaveOrWarnUser<KPrivacyPrefs>(KPrivacyPrefs.instance, KPrivacyPrefs.GetPath());
    }
    catch (Exception ex)
    {
      KPrivacyPrefs.LogError(ex.ToString());
    }
  }

  public static void Load()
  {
    try
    {
      if (KPrivacyPrefs._instance == null)
        KPrivacyPrefs._instance = new KPrivacyPrefs();
      string path = KPrivacyPrefs.GetPath();
      if (!File.Exists(path))
        return;
      KPrivacyPrefs._instance = YamlIO.LoadFile<KPrivacyPrefs>(path);
      if (KPrivacyPrefs._instance != null)
        return;
      KPrivacyPrefs.LogError("Exception while loading privacy prefs:" + path);
      KPrivacyPrefs._instance = new KPrivacyPrefs();
    }
    catch (Exception ex)
    {
      KPrivacyPrefs.LogError(ex.ToString());
    }
  }

  private static void LogError(string msg) => Debug.LogWarning((object) msg);
}
