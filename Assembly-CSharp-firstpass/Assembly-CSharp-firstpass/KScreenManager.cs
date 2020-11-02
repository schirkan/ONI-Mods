// Decompiled with JetBrains decompiler
// Type: KScreenManager
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Plugins/KScreenManager")]
public class KScreenManager : KMonoBehaviour, IInputHandler
{
  private static bool quitting;
  private static bool inputDisabled;
  private List<KScreen> screenStack = new List<KScreen>();
  private UnityEngine.EventSystems.EventSystem evSys;
  private KButtonEvent lastConsumedEvent;
  private KScreen lastConsumedEventScreen;

  public static KScreenManager Instance { get; private set; }

  public string handlerName => this.gameObject.name;

  public KInputHandler inputHandler { get; set; }

  private void OnApplicationQuit() => KScreenManager.quitting = true;

  public void DisableInput(bool disable) => KScreenManager.inputDisabled = disable;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    KScreenManager.Instance = this;
  }

  protected override void OnCleanUp() => KScreenManager.Instance = (KScreenManager) null;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.evSys = UnityEngine.EventSystems.EventSystem.current;
  }

  protected override void OnCmpDisable()
  {
    if (!KScreenManager.quitting)
      return;
    for (int index = this.screenStack.Count - 1; index >= 0; --index)
      this.screenStack[index].Deactivate();
  }

  public GameObject ActivateScreen(GameObject screen, GameObject parent)
  {
    KScreenManager.AddExistingChild(parent, screen);
    screen.GetComponent<KScreen>().Activate();
    return screen;
  }

  public KScreen InstantiateScreen(GameObject screenPrefab, GameObject parent) => KScreenManager.AddChild(parent, screenPrefab).GetComponent<KScreen>();

  public KScreen StartScreen(GameObject screenPrefab, GameObject parent)
  {
    KScreen component = KScreenManager.AddChild(parent, screenPrefab).GetComponent<KScreen>();
    component.Activate();
    return component;
  }

  public void PushScreen(KScreen screen)
  {
    this.screenStack.Add(screen);
    this.RefreshStack();
  }

  public void RefreshStack() => this.screenStack = this.screenStack.Where<KScreen>((Func<KScreen, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).OrderBy<KScreen, float>((Func<KScreen, float>) (x => x.GetSortKey())).ToList<KScreen>();

  public KScreen PopScreen(KScreen screen)
  {
    KScreen kscreen = (KScreen) null;
    int index = this.screenStack.IndexOf(screen);
    if (index >= 0)
    {
      kscreen = this.screenStack[index];
      this.screenStack.RemoveAt(index);
    }
    this.screenStack = this.screenStack.Where<KScreen>((Func<KScreen, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).OrderBy<KScreen, float>((Func<KScreen, float>) (x => x.GetSortKey())).ToList<KScreen>();
    return kscreen;
  }

  public KScreen PopScreen()
  {
    KScreen screen = this.screenStack[this.screenStack.Count - 1];
    this.screenStack.RemoveAt(this.screenStack.Count - 1);
    return screen;
  }

  public string DebugScreenStack()
  {
    string str = "";
    foreach (KScreen screen in this.screenStack)
      str = str + screen.name + "\n";
    return str;
  }

  private void Update()
  {
    bool topLevel = true;
    for (int index = this.screenStack.Count - 1; index >= 0; --index)
    {
      KScreen screen = this.screenStack[index];
      if ((UnityEngine.Object) screen != (UnityEngine.Object) null && screen.isActiveAndEnabled)
        screen.ScreenUpdate(topLevel);
      if (topLevel && screen.IsModal())
        topLevel = false;
    }
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (KScreenManager.inputDisabled)
      return;
    for (int index = this.screenStack.Count - 1; index >= 0; --index)
    {
      KScreen screen = this.screenStack[index];
      if ((UnityEngine.Object) screen != (UnityEngine.Object) null && screen.isActiveAndEnabled)
      {
        screen.OnKeyDown(e);
        if (e.Consumed || screen.IsModal())
        {
          this.lastConsumedEvent = e;
          this.lastConsumedEventScreen = screen;
          break;
        }
      }
    }
  }

  public void OnKeyUp(KButtonEvent e)
  {
    if (KScreenManager.inputDisabled)
      return;
    for (int index = this.screenStack.Count - 1; index >= 0; --index)
    {
      KScreen screen = this.screenStack[index];
      if ((UnityEngine.Object) screen != (UnityEngine.Object) null && screen.isActiveAndEnabled)
      {
        screen.OnKeyUp(e);
        if (e.Consumed || screen.IsModal())
        {
          this.lastConsumedEvent = e;
          this.lastConsumedEventScreen = screen;
          break;
        }
      }
    }
  }

  public void SetEventSystemEnabled(bool state)
  {
    if ((UnityEngine.Object) this.evSys == (UnityEngine.Object) null)
    {
      this.evSys = UnityEngine.EventSystems.EventSystem.current;
      if ((UnityEngine.Object) this.evSys == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "Cannot enable/disable null UI event system");
        return;
      }
    }
    if (this.evSys.enabled == state)
      return;
    this.evSys.enabled = state;
  }

  public void SetNavigationEventsEnabled(bool state)
  {
    if ((UnityEngine.Object) this.evSys == (UnityEngine.Object) null)
      return;
    this.evSys.sendNavigationEvents = state;
  }

  public static GameObject AddExistingChild(GameObject parent, GameObject go)
  {
    if ((UnityEngine.Object) go != (UnityEngine.Object) null && (UnityEngine.Object) parent != (UnityEngine.Object) null)
    {
      go.transform.SetParent(parent.transform, false);
      go.layer = parent.layer;
    }
    return go;
  }

  public static GameObject AddChild(GameObject parent, GameObject prefab) => Util.KInstantiateUI(prefab, parent);
}
