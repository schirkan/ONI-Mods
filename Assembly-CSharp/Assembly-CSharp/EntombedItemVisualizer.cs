// Decompiled with JetBrains decompiler
// Type: EntombedItemVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/EntombedItemVisualizer")]
public class EntombedItemVisualizer : KMonoBehaviour
{
  [SerializeField]
  private GameObject entombedItemPrefab;
  private static readonly string[] EntombedVisualizerAnims = new string[4]
  {
    "idle1",
    "idle2",
    "idle3",
    "idle4"
  };
  private ObjectPool entombedItemPool;
  private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();

  public void Clear() => this.cellEntombedCounts.Clear();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.entombedItemPool = new ObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), 32);
  }

  public bool AddItem(int cell)
  {
    bool flag = false;
    if ((UnityEngine.Object) Grid.Objects[cell, 9] == (UnityEngine.Object) null)
    {
      flag = true;
      EntombedItemVisualizer.Data data;
      this.cellEntombedCounts.TryGetValue(cell, out data);
      if (data.refCount == 0)
      {
        GameObject instance = this.entombedItemPool.GetInstance();
        instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
        instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.value * 360f);
        KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
        int index = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
        string entombedVisualizerAnim = EntombedItemVisualizer.EntombedVisualizerAnims[index];
        component.initialAnim = entombedVisualizerAnim;
        instance.SetActive(true);
        component.Play((HashedString) entombedVisualizerAnim);
        data.controller = component;
      }
      ++data.refCount;
      this.cellEntombedCounts[cell] = data;
    }
    return flag;
  }

  public void RemoveItem(int cell)
  {
    EntombedItemVisualizer.Data data;
    if (!this.cellEntombedCounts.TryGetValue(cell, out data))
      return;
    --data.refCount;
    if (data.refCount == 0)
      this.ReleaseVisualizer(cell, data);
    else
      this.cellEntombedCounts[cell] = data;
  }

  public void ForceClear(int cell)
  {
    EntombedItemVisualizer.Data data;
    if (!this.cellEntombedCounts.TryGetValue(cell, out data))
      return;
    this.ReleaseVisualizer(cell, data);
  }

  private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
  {
    if ((UnityEngine.Object) data.controller != (UnityEngine.Object) null)
    {
      data.controller.gameObject.SetActive(false);
      this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
    }
    this.cellEntombedCounts.Remove(cell);
  }

  public bool IsEntombedItem(int cell) => this.cellEntombedCounts.ContainsKey(cell) && this.cellEntombedCounts[cell].refCount > 0;

  private GameObject InstantiateEntombedObject()
  {
    GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront);
    gameObject.SetActive(false);
    return gameObject;
  }

  private struct Data
  {
    public int refCount;
    public KBatchedAnimController controller;
  }
}
