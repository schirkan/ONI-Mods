﻿// Decompiled with JetBrains decompiler
// Type: SceneInitializerLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SceneInitializerLoader : MonoBehaviour
{
  public SceneInitializer sceneInitializer;
  public static SceneInitializerLoader.DeferredError deferred_error;
  public static SceneInitializerLoader.DeferredErrorDelegate ReportDeferredError;

  private void Awake()
  {
    foreach (Behaviour behaviour in Object.FindObjectsOfType<Camera>())
      behaviour.enabled = false;
    KMonoBehaviour.isLoadingScene = false;
    Singleton<StateMachineManager>.Instance.Clear();
    Util.KInstantiate((Component) this.sceneInitializer);
    if (SceneInitializerLoader.ReportDeferredError == null || !SceneInitializerLoader.deferred_error.IsValid)
      return;
    SceneInitializerLoader.ReportDeferredError(SceneInitializerLoader.deferred_error);
    SceneInitializerLoader.deferred_error = new SceneInitializerLoader.DeferredError();
  }

  public struct DeferredError
  {
    public string msg;
    public string stack_trace;

    public bool IsValid => !string.IsNullOrEmpty(this.msg);
  }

  public delegate void DeferredErrorDelegate(
    SceneInitializerLoader.DeferredError deferred_error);
}
