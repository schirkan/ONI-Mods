// Decompiled with JetBrains decompiler
// Type: GarbageCollectionProfiler
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using UnityEngine;

public class GarbageCollectionProfiler : MonoBehaviour
{
  public int _ObjectCount = 100000;
  private GarbageCollectionProfiler.Test[] _Items;

  private void Update()
  {
    if (this._Items == null || this._Items.Length != this._ObjectCount)
    {
      this._Items = new GarbageCollectionProfiler.Test[this._ObjectCount];
      for (int index = 0; index < this._ObjectCount; ++index)
        this._Items[index] = (GarbageCollectionProfiler.Test) new GarbageCollectionProfiler.DelegateWithSingleHandler();
    }
    GC.Collect();
  }

  private class Test
  {
  }

  private class StringTest : GarbageCollectionProfiler.Test
  {
    private string _String;
  }

  private class ObjectTest : GarbageCollectionProfiler.Test
  {
    private object _Object;
  }

  private class DelegateTest : GarbageCollectionProfiler.Test
  {
    private System.Action _Delegate;
  }

  private class DelegateWithSingleHandler : GarbageCollectionProfiler.Test
  {
    private System.Action _Delegate;

    public DelegateWithSingleHandler() => this._Delegate += new System.Action(this.DoNothing);

    private void DoNothing()
    {
    }
  }
}
