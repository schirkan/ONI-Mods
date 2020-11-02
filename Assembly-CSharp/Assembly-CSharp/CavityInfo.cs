// Decompiled with JetBrains decompiler
// Type: CavityInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CavityInfo
{
  public HandleVector<int>.Handle handle;
  public bool dirty;
  public int numCells;
  public int maxX;
  public int maxY;
  public int minX;
  public int minY;
  public Room room;
  public List<KPrefabID> buildings = new List<KPrefabID>();
  public List<KPrefabID> plants = new List<KPrefabID>();
  public List<KPrefabID> creatures = new List<KPrefabID>();
  public List<KPrefabID> eggs = new List<KPrefabID>();

  public CavityInfo()
  {
    this.handle = HandleVector<int>.InvalidHandle;
    this.dirty = true;
  }

  public void AddBuilding(KPrefabID bc)
  {
    this.buildings.Add(bc);
    this.dirty = true;
  }

  public void AddPlants(KPrefabID plant)
  {
    this.plants.Add(plant);
    this.dirty = true;
  }

  public void OnEnter(object data)
  {
    foreach (KPrefabID building in this.buildings)
    {
      if ((Object) building != (Object) null)
        building.Trigger(-832141045, data);
    }
  }
}
