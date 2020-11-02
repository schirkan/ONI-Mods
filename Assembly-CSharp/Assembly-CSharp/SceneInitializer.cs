// Decompiled with JetBrains decompiler
// Type: SceneInitializer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
  public const int MAXDEPTH = -30000;
  public const int SCREENDEPTH = -1000;
  public GameObject prefab_NewSaveGame;
  public List<GameObject> preloadPrefabs = new List<GameObject>();
  public List<GameObject> prefabs = new List<GameObject>();

  public static SceneInitializer Instance { get; private set; }

  private void Awake()
  {
    Localization.SwapToLocalizedFont();
    string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
    string str = Application.dataPath + System.IO.Path.DirectorySeparatorChar.ToString() + "Plugins";
    if (!environmentVariable.Contains(str))
      Environment.SetEnvironmentVariable("PATH", environmentVariable + System.IO.Path.PathSeparator.ToString() + str, EnvironmentVariableTarget.Process);
    SceneInitializer.Instance = this;
    this.PreLoadPrefabs();
  }

  private void OnDestroy() => SceneInitializer.Instance = (SceneInitializer) null;

  private void PreLoadPrefabs()
  {
    foreach (GameObject preloadPrefab in this.preloadPrefabs)
    {
      if ((UnityEngine.Object) preloadPrefab != (UnityEngine.Object) null)
        Util.KInstantiate(preloadPrefab, preloadPrefab.transform.GetPosition(), Quaternion.identity, this.gameObject);
    }
  }

  public void NewSaveGamePrefab()
  {
    if (!((UnityEngine.Object) this.prefab_NewSaveGame != (UnityEngine.Object) null) || !((UnityEngine.Object) SaveGame.Instance == (UnityEngine.Object) null))
      return;
    Util.KInstantiate(this.prefab_NewSaveGame, this.gameObject);
  }

  public void PostLoadPrefabs()
  {
    foreach (GameObject prefab in this.prefabs)
    {
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        Util.KInstantiate(prefab, this.gameObject);
    }
  }
}
