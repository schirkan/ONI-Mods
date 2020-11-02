// Decompiled with JetBrains decompiler
// Type: EventSystem
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventSystem
{
  private static bool ENABLE_DETAILED_EVENT_PROFILE_INFO = false;
  private int nextId;
  private int currentlyTriggering;
  private bool dirty;
  private ArrayRef<EventSystem.SubscribedEntry> subscribedEvents;
  private ArrayRef<EventSystem.Entry> entries;
  private ArrayRef<EventSystem.IntraObjectRoute> intraObjectRoutes;
  private static Dictionary<int, List<EventSystem.IntraObjectHandlerBase>> intraObjectDispatcher = new Dictionary<int, List<EventSystem.IntraObjectHandlerBase>>();
  private LoggerFIO log;

  public EventSystem() => this.log = new LoggerFIO("Events");

  public void Trigger(GameObject go, int hash, object data = null)
  {
    if (App.IsExiting)
      return;
    ++this.currentlyTriggering;
    for (int i = 0; i != this.intraObjectRoutes.size; ++i)
    {
      if (this.intraObjectRoutes[i].eventHash == hash)
        EventSystem.intraObjectDispatcher[hash][this.intraObjectRoutes[i].handlerIndex].Trigger(go, data);
    }
    int size = this.entries.size;
    if (EventSystem.ENABLE_DETAILED_EVENT_PROFILE_INFO)
    {
      for (int i = 0; i < size; ++i)
      {
        if (this.entries[i].hash == hash && this.entries[i].handler != null)
          this.entries[i].handler(data);
      }
    }
    else
    {
      for (int i = 0; i < size; ++i)
      {
        if (this.entries[i].hash == hash && this.entries[i].handler != null)
          this.entries[i].handler(data);
      }
    }
    --this.currentlyTriggering;
    if (!this.dirty || this.currentlyTriggering != 0)
      return;
    this.dirty = false;
    this.entries.RemoveAllSwap((Predicate<EventSystem.Entry>) (x => x.handler == null));
    this.intraObjectRoutes.RemoveAllSwap((Predicate<EventSystem.IntraObjectRoute>) (route => !route.IsValid()));
  }

  public void OnCleanUp()
  {
    for (int i = this.subscribedEvents.size - 1; i >= 0; --i)
    {
      EventSystem.SubscribedEntry subscribedEvent = this.subscribedEvents[i];
      if ((UnityEngine.Object) subscribedEvent.go != (UnityEngine.Object) null)
        this.Unsubscribe(subscribedEvent.go, subscribedEvent.hash, subscribedEvent.handler);
    }
    for (int i = 0; i < this.entries.size; ++i)
    {
      EventSystem.Entry entry = this.entries[i];
      entry.handler = (System.Action<object>) null;
      this.entries[i] = entry;
    }
    this.entries.Clear();
    this.subscribedEvents.Clear();
    this.intraObjectRoutes.Clear();
  }

  public void UnregisterEvent(GameObject target, int eventName, System.Action<object> handler)
  {
    for (int index = 0; index < this.subscribedEvents.size; ++index)
    {
      if (this.subscribedEvents[index].hash == eventName && this.subscribedEvents[index].handler == handler && (UnityEngine.Object) this.subscribedEvents[index].go == (UnityEngine.Object) target)
      {
        this.subscribedEvents.RemoveAt(index);
        break;
      }
    }
  }

  public void RegisterEvent(GameObject target, int eventName, System.Action<object> handler) => this.subscribedEvents.Add(new EventSystem.SubscribedEntry(target, eventName, handler));

  public int Subscribe(int hash, System.Action<object> handler)
  {
    this.entries.Add(new EventSystem.Entry(hash, handler, ++this.nextId));
    return this.nextId;
  }

  public void Unsubscribe(int hash, System.Action<object> handler)
  {
    for (int index = 0; index < this.entries.size; ++index)
    {
      if (this.entries[index].hash == hash && this.entries[index].handler == handler)
      {
        if (this.currentlyTriggering == 0)
        {
          this.entries.RemoveAt(index);
          break;
        }
        this.dirty = true;
        EventSystem.Entry entry = this.entries[index];
        entry.handler = (System.Action<object>) null;
        this.entries[index] = entry;
        break;
      }
    }
  }

  public void Unsubscribe(int id)
  {
    for (int index = 0; index < this.entries.size; ++index)
    {
      if (this.entries[index].id == id)
      {
        if (this.currentlyTriggering == 0)
        {
          this.entries.RemoveAt(index);
          break;
        }
        this.dirty = true;
        EventSystem.Entry entry = this.entries[index];
        entry.handler = (System.Action<object>) null;
        this.entries[index] = entry;
        break;
      }
    }
  }

  public int Subscribe(GameObject target, int eventName, System.Action<object> handler)
  {
    this.RegisterEvent(target, eventName, handler);
    return KObjectManager.Instance.GetOrCreateObject(target).GetEventSystem().Subscribe(eventName, handler);
  }

  public int Subscribe<ComponentType>(
    int eventName,
    EventSystem.IntraObjectHandler<ComponentType> handler)
  {
    List<EventSystem.IntraObjectHandlerBase> objectHandlerBaseList;
    if (!EventSystem.intraObjectDispatcher.TryGetValue(eventName, out objectHandlerBaseList))
    {
      objectHandlerBaseList = new List<EventSystem.IntraObjectHandlerBase>();
      EventSystem.intraObjectDispatcher.Add(eventName, objectHandlerBaseList);
    }
    int handlerIndex = objectHandlerBaseList.IndexOf((EventSystem.IntraObjectHandlerBase) handler);
    if (handlerIndex == -1)
    {
      objectHandlerBaseList.Add((EventSystem.IntraObjectHandlerBase) handler);
      handlerIndex = objectHandlerBaseList.Count - 1;
    }
    this.intraObjectRoutes.Add(new EventSystem.IntraObjectRoute(eventName, handlerIndex));
    return handlerIndex;
  }

  public void Unsubscribe(GameObject target, int eventName, System.Action<object> handler)
  {
    this.UnregisterEvent(target, eventName, handler);
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      return;
    KObjectManager.Instance.GetOrCreateObject(target).GetEventSystem().Unsubscribe(eventName, handler);
  }

  public void Unsubscribe(int eventName, int subscribeHandle, bool suppressWarnings = false)
  {
    int index = this.intraObjectRoutes.FindIndex((Predicate<EventSystem.IntraObjectRoute>) (route => route.eventHash == eventName && route.handlerIndex == subscribeHandle));
    if (index == -1)
    {
      if (suppressWarnings)
        return;
      Debug.LogWarning((object) ("Failed to Unsubscribe event handler: " + EventSystem.intraObjectDispatcher[eventName][subscribeHandle].ToString() + "\nNot subscribed to event"));
    }
    else if (this.currentlyTriggering == 0)
    {
      this.intraObjectRoutes.RemoveAtSwap(index);
    }
    else
    {
      this.dirty = true;
      this.intraObjectRoutes[index] = new EventSystem.IntraObjectRoute();
    }
  }

  public void Unsubscribe<ComponentType>(
    int eventName,
    EventSystem.IntraObjectHandler<ComponentType> handler,
    bool suppressWarnings)
  {
    List<EventSystem.IntraObjectHandlerBase> objectHandlerBaseList;
    if (!EventSystem.intraObjectDispatcher.TryGetValue(eventName, out objectHandlerBaseList))
    {
      if (suppressWarnings)
        return;
      Debug.LogWarning((object) ("Failed to Unsubscribe event handler: " + handler.ToString() + "\nNo subscriptions have been made to event"));
    }
    else
    {
      int subscribeHandle = objectHandlerBaseList.IndexOf((EventSystem.IntraObjectHandlerBase) handler);
      if (subscribeHandle == -1)
      {
        if (suppressWarnings)
          return;
        Debug.LogWarning((object) ("Failed to Unsubscribe event handler: " + handler.ToString() + "\nNot subscribed to event"));
      }
      else
        this.Unsubscribe(eventName, subscribeHandle, suppressWarnings);
    }
  }

  public void Unsubscribe(string[] eventNames, System.Action<object> handler)
  {
    foreach (string eventName in eventNames)
      this.Unsubscribe(Hash.SDBMLower(eventName), handler);
  }

  private struct Entry
  {
    public System.Action<object> handler;
    public int hash;
    public int id;

    public Entry(int hash, System.Action<object> handler, int id)
    {
      this.handler = handler;
      this.hash = hash;
      this.id = id;
    }
  }

  private struct SubscribedEntry
  {
    public System.Action<object> handler;
    public int hash;
    public GameObject go;

    public SubscribedEntry(GameObject go, int hash, System.Action<object> handler)
    {
      this.go = go;
      this.hash = hash;
      this.handler = handler;
    }
  }

  private struct IntraObjectRoute
  {
    public int eventHash;
    public int handlerIndex;

    public IntraObjectRoute(int eventHash, int handlerIndex)
    {
      this.eventHash = eventHash;
      this.handlerIndex = handlerIndex;
    }

    public bool IsValid() => (uint) this.eventHash > 0U;
  }

  public abstract class IntraObjectHandlerBase
  {
    public abstract void Trigger(GameObject gameObject, object eventData);
  }

  public class IntraObjectHandler<ComponentType> : EventSystem.IntraObjectHandlerBase
  {
    private System.Action<ComponentType, object> handler;

    public static bool IsStatic(Delegate del) => del.Target == null || del.Target.GetType().GetCustomAttributes(false).OfType<CompilerGeneratedAttribute>().Any<CompilerGeneratedAttribute>();

    public IntraObjectHandler(System.Action<ComponentType, object> handler)
    {
      Debug.Assert(EventSystem.IntraObjectHandler<ComponentType>.IsStatic((Delegate) handler));
      this.handler = handler;
    }

    public static implicit operator EventSystem.IntraObjectHandler<ComponentType>(
      System.Action<ComponentType, object> handler)
    {
      return new EventSystem.IntraObjectHandler<ComponentType>(handler);
    }

    public override void Trigger(GameObject gameObject, object eventData)
    {
      ListPool<ComponentType, EventSystem.IntraObjectHandler<ComponentType>>.PooledList pooledList = ListPool<ComponentType, EventSystem.IntraObjectHandler<ComponentType>>.Allocate();
      gameObject.GetComponents<ComponentType>((List<ComponentType>) pooledList);
      foreach (ComponentType componentType in (List<ComponentType>) pooledList)
        this.handler(componentType, eventData);
      pooledList.Recycle();
    }

    public override string ToString() => (this.handler.Target != null ? this.handler.Target.GetType().ToString() : "STATIC") + "." + this.handler.Method.ToString();
  }
}
