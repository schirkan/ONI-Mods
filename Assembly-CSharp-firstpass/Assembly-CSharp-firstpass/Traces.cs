// Decompiled with JetBrains decompiler
// Type: Traces
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Plugins/Traces")]
public class Traces : KMonoBehaviour
{
  public List<Traces.Entry> DestroyTraces = new List<Traces.Entry>();

  public static Traces Instance { get; private set; }

  public static void DestroyInstance() => Traces.Instance = (Traces) null;

  protected override void OnPrefabInit() => Traces.Instance = this;

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Traces.Instance = (Traces) null;
  }

  public void TraceDestroy(GameObject go, StackTrace stack_trace)
  {
    if (this.DestroyTraces.Count > 99)
      this.DestroyTraces.RemoveAt(0);
    this.DestroyTraces.Add(new Traces.Entry()
    {
      Name = Time.frameCount.ToString() + " " + go.name + " [" + (object) go.GetInstanceID() + "]",
      StackTrace = stack_trace
    });
  }

  [Serializable]
  public class Entry
  {
    public string Name;
    public StackTrace StackTrace;
    public bool Foldout;
  }
}
