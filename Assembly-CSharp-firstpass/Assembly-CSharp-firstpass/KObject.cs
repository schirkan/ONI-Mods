// Decompiled with JetBrains decompiler
// Type: KObject
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

public class KObject
{
  private EventSystem eventSystem;

  public KObject(GameObject go) => this.id = go.GetInstanceID();

  ~KObject() => this.OnCleanUp();

  public void OnCleanUp()
  {
    if (this.eventSystem == null)
      return;
    this.eventSystem.OnCleanUp();
    this.eventSystem = (EventSystem) null;
  }

  public EventSystem GetEventSystem()
  {
    if (this.eventSystem == null)
      this.eventSystem = new EventSystem();
    return this.eventSystem;
  }

  public int id { get; private set; }

  public bool hasEventSystem => this.eventSystem != null;
}
