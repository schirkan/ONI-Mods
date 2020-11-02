// Decompiled with JetBrains decompiler
// Type: Room
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Room : IAssignableIdentity
{
  public CavityInfo cavity;
  public RoomType roomType;
  private List<KPrefabID> primary_buildings = new List<KPrefabID>();
  private List<Ownables> current_owners = new List<Ownables>();

  public List<KPrefabID> buildings => this.cavity.buildings;

  public List<KPrefabID> plants => this.cavity.plants;

  public string GetProperName() => this.roomType.Name;

  public List<Ownables> GetOwners()
  {
    this.current_owners.Clear();
    foreach (KPrefabID primaryEntity in this.GetPrimaryEntities())
    {
      if ((Object) primaryEntity != (Object) null)
      {
        Ownable component = primaryEntity.GetComponent<Ownable>();
        if ((Object) component != (Object) null && component.assignee != null && component.assignee != this)
        {
          foreach (Ownables owner in component.assignee.GetOwners())
          {
            if (!this.current_owners.Contains(owner))
              this.current_owners.Add(owner);
          }
        }
      }
    }
    return this.current_owners;
  }

  public Ownables GetSoleOwner()
  {
    List<Ownables> owners = this.GetOwners();
    return owners.Count <= 0 ? (Ownables) null : owners[0];
  }

  public List<KPrefabID> GetPrimaryEntities()
  {
    this.primary_buildings.Clear();
    RoomType roomType = this.roomType;
    if (roomType.primary_constraint != null)
    {
      foreach (KPrefabID building in this.buildings)
      {
        if ((Object) building != (Object) null && roomType.primary_constraint.building_criteria(building))
          this.primary_buildings.Add(building);
      }
      foreach (KPrefabID plant in this.plants)
      {
        if ((Object) plant != (Object) null && roomType.primary_constraint.building_criteria(plant))
          this.primary_buildings.Add(plant);
      }
    }
    return this.primary_buildings;
  }

  public void RetriggerBuildings()
  {
    foreach (KPrefabID building in this.buildings)
    {
      if (!((Object) building == (Object) null))
        building.Trigger(144050788, (object) this);
    }
    foreach (KPrefabID plant in this.plants)
    {
      if (!((Object) plant == (Object) null))
        plant.Trigger(144050788, (object) this);
    }
  }

  public bool IsNull() => false;

  public void CleanUp() => Game.Instance.assignmentManager.RemoveFromAllGroups((IAssignableIdentity) this);
}
