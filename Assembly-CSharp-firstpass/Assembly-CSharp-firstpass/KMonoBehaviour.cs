// Decompiled with JetBrains decompiler
// Type: KMonoBehaviour
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class KMonoBehaviour : MonoBehaviour, IStateMachineTarget, ISaveLoadable, IUniformGridObject
{
  public static GameObject lastGameObject;
  public static KObject lastObj;
  public static bool isPoolPreInit;
  public static bool isLoadingScene;
  private KObject obj;
  private bool isInitialized;
  protected bool autoRegisterSimRender = true;
  protected bool simRenderLoadBalance;

  public bool isSpawned { get; private set; }

  public new Transform transform => base.transform;

  public bool isNull => (UnityEngine.Object) this == (UnityEngine.Object) null;

  public void Awake()
  {
    if (App.IsExiting)
      return;
    this.InitializeComponent();
  }

  public void InitializeComponent()
  {
    if (this.isInitialized)
      return;
    if (!KMonoBehaviour.isPoolPreInit && Application.isPlaying && (UnityEngine.Object) KMonoBehaviour.lastGameObject != (UnityEngine.Object) this.gameObject)
    {
      KMonoBehaviour.lastGameObject = this.gameObject;
      KMonoBehaviour.lastObj = KObjectManager.Instance.GetOrCreateObject(this.gameObject);
    }
    this.obj = KMonoBehaviour.lastObj;
    this.isInitialized = true;
    MyCmp.OnAwake(this);
    if (KMonoBehaviour.isPoolPreInit)
      return;
    try
    {
      this.OnPrefabInit();
    }
    catch (Exception ex)
    {
      DebugUtil.LogException((UnityEngine.Object) this, "Error in " + this.name + "." + this.GetType().Name + ".OnPrefabInit", ex);
    }
  }

  private void OnEnable()
  {
    if (App.IsExiting)
      return;
    this.OnCmpEnable();
  }

  private void OnDisable()
  {
    if (App.IsExiting || KMonoBehaviour.isLoadingScene)
      return;
    this.OnCmpDisable();
  }

  public bool IsInitialized() => this.isInitialized;

  public void OnDestroy()
  {
    this.OnForcedCleanUp();
    if (App.IsExiting)
      return;
    if (KMonoBehaviour.isLoadingScene)
    {
      this.OnLoadLevel();
    }
    else
    {
      if ((UnityEngine.Object) KObjectManager.Instance != (UnityEngine.Object) null)
        KObjectManager.Instance.QueueDestroy(this.obj);
      this.OnCleanUp();
      SimAndRenderScheduler.instance.Remove((object) this);
    }
  }

  public void Start()
  {
    if (App.IsExiting)
      return;
    this.Spawn();
  }

  public void Spawn()
  {
    if (this.isSpawned)
      return;
    if (!this.isInitialized)
    {
      Debug.LogError((object) (this.name + "." + this.GetType().Name + " is not initialized."));
    }
    else
    {
      this.isSpawned = true;
      if (this.autoRegisterSimRender)
        SimAndRenderScheduler.instance.Add((object) this, this.simRenderLoadBalance);
      MyCmp.OnStart(this);
      try
      {
        this.OnSpawn();
      }
      catch (Exception ex)
      {
        DebugUtil.LogException((UnityEngine.Object) this, "Error in " + this.name + "." + this.GetType().Name + ".OnSpawn", ex);
      }
    }
  }

  protected virtual void OnPrefabInit()
  {
  }

  protected virtual void OnSpawn()
  {
  }

  protected virtual void OnCmpEnable()
  {
  }

  protected virtual void OnCmpDisable()
  {
  }

  protected virtual void OnCleanUp()
  {
  }

  protected virtual void OnForcedCleanUp()
  {
  }

  protected virtual void OnLoadLevel()
  {
  }

  public virtual void CreateDef()
  {
  }

  public T FindOrAdd<T>() where T : KMonoBehaviour => this.FindOrAddComponent<T>();

  public void FindOrAdd<T>(ref T c) where T : KMonoBehaviour => c = this.FindOrAdd<T>();

  public T Require<T>() where T : Component => this.RequireComponent<T>();

  public int Subscribe(int hash, System.Action<object> handler) => this.obj.GetEventSystem().Subscribe(hash, handler);

  public int Subscribe(GameObject target, int hash, System.Action<object> handler) => this.obj.GetEventSystem().Subscribe(target, hash, handler);

  public int Subscribe<ComponentType>(
    int hash,
    EventSystem.IntraObjectHandler<ComponentType> handler)
  {
    return this.obj.GetEventSystem().Subscribe<ComponentType>(hash, handler);
  }

  public void Unsubscribe(int hash, System.Action<object> handler)
  {
    if (this.obj == null)
      return;
    this.obj.GetEventSystem().Unsubscribe(hash, handler);
  }

  public void Unsubscribe(int id) => this.obj.GetEventSystem().Unsubscribe(id);

  public void Unsubscribe(GameObject target, int hash, System.Action<object> handler) => this.obj.GetEventSystem().Unsubscribe(target, hash, handler);

  public void Unsubscribe<ComponentType>(
    int hash,
    EventSystem.IntraObjectHandler<ComponentType> handler,
    bool suppressWarnings = false)
  {
    if (this.obj == null)
      return;
    this.obj.GetEventSystem().Unsubscribe<ComponentType>(hash, handler, suppressWarnings);
  }

  public void Trigger(int hash, object data = null)
  {
    if (this.obj == null || !this.obj.hasEventSystem)
      return;
    this.obj.GetEventSystem().Trigger(this.gameObject, hash, data);
  }

  public static void PlaySound(string sound)
  {
    if (sound == null)
      return;
    try
    {
      if ((UnityEngine.Object) SoundListenerController.Instance == (UnityEngine.Object) null)
        KFMOD.PlayUISound(sound);
      else
        KFMOD.PlayOneShot(sound, SoundListenerController.Instance.transform.GetPosition());
    }
    catch
    {
      DebugUtil.LogWarningArgs((object) ("AUDIOERROR: Missing [" + sound + "]"));
    }
  }

  public static void PlaySound3DAtLocation(string sound, Vector3 location)
  {
    if (!((UnityEngine.Object) SoundListenerController.Instance != (UnityEngine.Object) null))
      return;
    try
    {
      KFMOD.PlayOneShot(sound, location);
    }
    catch
    {
      DebugUtil.LogWarningArgs((object) ("AUDIOERROR: Missing [" + sound + "]"));
    }
  }

  public void PlaySound3D(string asset)
  {
    try
    {
      KFMOD.PlayOneShot(asset, this.transform.GetPosition());
    }
    catch
    {
      DebugUtil.LogWarningArgs((object) ("AUDIOERROR: Missing [" + asset + "]"));
    }
  }

  public virtual Vector2 PosMin() => (Vector2) this.transform.GetPosition();

  public virtual Vector2 PosMax() => (Vector2) this.transform.GetPosition();

  ComponentType IStateMachineTarget.GetComponent<ComponentType>() => this.GetComponent<ComponentType>();

  [SpecialName]
  GameObject IStateMachineTarget.get_gameObject() => this.gameObject;

  [SpecialName]
  string IStateMachineTarget.get_name() => this.name;
}
