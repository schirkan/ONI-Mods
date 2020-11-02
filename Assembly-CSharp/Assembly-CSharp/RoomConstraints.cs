// Decompiled with JetBrains decompiler
// Type: RoomConstraints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

public static class RoomConstraints
{
  public static RoomConstraints.Constraint CEILING_HEIGHT_4 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 4), name: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.NAME, (object) "4"), description: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, (object) "4"));
  public static RoomConstraints.Constraint CEILING_HEIGHT_6 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 6), name: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.NAME, (object) "6"), description: string.Format((string) ROOMS.CRITERIA.CEILING_HEIGHT.DESCRIPTION, (object) "6"));
  public static RoomConstraints.Constraint MINIMUM_SIZE_12 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells >= 12), name: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "12"), description: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "12"));
  public static RoomConstraints.Constraint MINIMUM_SIZE_32 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells >= 32), name: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.NAME, (object) "32"), description: string.Format((string) ROOMS.CRITERIA.MINIMUM_SIZE.DESCRIPTION, (object) "32"));
  public static RoomConstraints.Constraint MAXIMUM_SIZE_64 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells <= 64), name: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "64"), description: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "64"));
  public static RoomConstraints.Constraint MAXIMUM_SIZE_96 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells <= 96), name: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "96"), description: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "96"));
  public static RoomConstraints.Constraint MAXIMUM_SIZE_120 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.numCells <= 120), name: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.NAME, (object) "120"), description: string.Format((string) ROOMS.CRITERIA.MAXIMUM_SIZE.DESCRIPTION, (object) "120"));
  public static RoomConstraints.Constraint NO_INDUSTRIAL_MACHINERY = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.IndustrialMachinery))
        return false;
    }
    return true;
  }), name: ((string) ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.NAME), description: ((string) ROOMS.CRITERIA.NO_INDUSTRIAL_MACHINERY.DESCRIPTION));
  public static RoomConstraints.Constraint NO_COTS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.Bed) && !building.HasTag(RoomConstraints.ConstraintTags.LuxuryBed))
        return false;
    }
    return true;
  }), name: ((string) ROOMS.CRITERIA.NO_COTS.NAME), description: ((string) ROOMS.CRITERIA.NO_COTS.DESCRIPTION));
  public static RoomConstraints.Constraint NO_OUTHOUSES = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID building in room.buildings)
    {
      if (building.HasTag(RoomConstraints.ConstraintTags.Toilet) && !building.HasTag(RoomConstraints.ConstraintTags.FlushToilet))
        return false;
    }
    return true;
  }), name: ((string) ROOMS.CRITERIA.NO_OUTHOUSES.NAME), description: ((string) ROOMS.CRITERIA.NO_OUTHOUSES.DESCRIPTION));
  public static RoomConstraints.Constraint LUXURY_BED_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.LuxuryBed)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.LUXURY_BED_SINGLE.NAME), description: ((string) ROOMS.CRITERIA.LUXURY_BED_SINGLE.DESCRIPTION));
  public static RoomConstraints.Constraint BED_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Bed) && !bc.HasTag(RoomConstraints.ConstraintTags.Clinic)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.BED_SINGLE.NAME), description: ((string) ROOMS.CRITERIA.BED_SINGLE.DESCRIPTION));
  public static RoomConstraints.Constraint BUILDING_DECOR_POSITIVE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc =>
  {
    DecorProvider component = bc.GetComponent<DecorProvider>();
    return (UnityEngine.Object) component != (UnityEngine.Object) null && (double) component.baseDecor > 0.0;
  }), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.NAME), description: ((string) ROOMS.CRITERIA.BUILDING_DECOR_POSITIVE.DESCRIPTION));
  public static RoomConstraints.Constraint DECORATIVE_ITEM = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(GameTags.Decoration)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.DECORATIVE_ITEM.NAME), description: ((string) ROOMS.CRITERIA.DECORATIVE_ITEM.DESCRIPTION));
  public static RoomConstraints.Constraint DECORATIVE_ITEM_20 = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(GameTags.Decoration) && bc.HasTag(RoomConstraints.ConstraintTags.Decor20)), (Func<Room, bool>) null, name: string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM_N.NAME, (object) "20"), description: string.Format((string) ROOMS.CRITERIA.DECORATIVE_ITEM_N.DESCRIPTION, (object) "20"));
  public static RoomConstraints.Constraint POWER_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.PowerStation)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.POWER_STATION.NAME), description: ((string) ROOMS.CRITERIA.POWER_STATION.DESCRIPTION));
  public static RoomConstraints.Constraint FARM_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FarmStation)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.FARM_STATION.NAME), description: ((string) ROOMS.CRITERIA.FARM_STATION.DESCRIPTION));
  public static RoomConstraints.Constraint RANCH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.RanchStation)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.RANCH_STATION.NAME), description: ((string) ROOMS.CRITERIA.RANCH_STATION.DESCRIPTION));
  public static RoomConstraints.Constraint REC_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.RecBuilding)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.REC_BUILDING.NAME), description: ((string) ROOMS.CRITERIA.REC_BUILDING.DESCRIPTION));
  public static RoomConstraints.Constraint MACHINE_SHOP = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MachineShop)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.MACHINE_SHOP.NAME), description: ((string) ROOMS.CRITERIA.MACHINE_SHOP.DESCRIPTION));
  public static RoomConstraints.Constraint FOOD_BOX = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FoodStorage)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.FOOD_BOX.NAME), description: ((string) ROOMS.CRITERIA.FOOD_BOX.DESCRIPTION));
  public static RoomConstraints.Constraint LIGHT = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    foreach (KPrefabID creature in room.cavity.creatures)
    {
      if ((UnityEngine.Object) creature != (UnityEngine.Object) null && (UnityEngine.Object) creature.GetComponent<Light2D>() != (UnityEngine.Object) null)
        return true;
    }
    foreach (KPrefabID building in room.buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null))
      {
        Light2D component1 = building.GetComponent<Light2D>();
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        {
          RequireInputs component2 = building.GetComponent<RequireInputs>();
          return component1.enabled || (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.RequirementsMet;
        }
      }
    }
    return false;
  }), name: ((string) ROOMS.CRITERIA.LIGHT.NAME), description: ((string) ROOMS.CRITERIA.LIGHT.DESCRIPTION));
  public static RoomConstraints.Constraint DESTRESSING_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.DeStressingBuilding)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.DESTRESSING_BUILDING.NAME), description: ((string) ROOMS.CRITERIA.DESTRESSING_BUILDING.DESCRIPTION));
  public static RoomConstraints.Constraint MASSAGE_TABLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MassageTable)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.MASSAGE_TABLE.NAME), description: ((string) ROOMS.CRITERIA.MASSAGE_TABLE.DESCRIPTION));
  public static RoomConstraints.Constraint MESS_STATION_SINGLE = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.MessTable)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.MESS_STATION_SINGLE.NAME), description: ((string) ROOMS.CRITERIA.MESS_STATION_SINGLE.DESCRIPTION), stomp_in_conflict: new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.REC_BUILDING
  });
  public static RoomConstraints.Constraint RESEARCH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.ResearchStation)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.RESEARCH_STATION.NAME), description: ((string) ROOMS.CRITERIA.RESEARCH_STATION.DESCRIPTION));
  public static RoomConstraints.Constraint TOILET = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Toilet)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.TOILET.NAME), description: ((string) ROOMS.CRITERIA.TOILET.DESCRIPTION));
  public static RoomConstraints.Constraint FLUSH_TOILET = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.FlushToilet)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.FLUSH_TOILET.NAME), description: ((string) ROOMS.CRITERIA.FLUSH_TOILET.DESCRIPTION));
  public static RoomConstraints.Constraint WASH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.WashStation)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.WASH_STATION.NAME), description: ((string) ROOMS.CRITERIA.WASH_STATION.DESCRIPTION));
  public static RoomConstraints.Constraint ADVANCED_WASH_STATION = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.AdvancedWashStation)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.ADVANCED_WASH_STATION.NAME), description: ((string) ROOMS.CRITERIA.ADVANCED_WASH_STATION.DESCRIPTION));
  public static RoomConstraints.Constraint CLINIC = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Clinic)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.CLINIC.NAME), description: ((string) ROOMS.CRITERIA.CLINIC.DESCRIPTION), stomp_in_conflict: new List<RoomConstraints.Constraint>()
  {
    RoomConstraints.TOILET,
    RoomConstraints.FLUSH_TOILET,
    RoomConstraints.MESS_STATION_SINGLE
  });
  public static RoomConstraints.Constraint PARK_BUILDING = new RoomConstraints.Constraint((Func<KPrefabID, bool>) (bc => bc.HasTag(RoomConstraints.ConstraintTags.Park)), (Func<Room, bool>) null, name: ((string) ROOMS.CRITERIA.PARK_BUILDING.NAME), description: ((string) ROOMS.CRITERIA.PARK_BUILDING.DESCRIPTION));
  public static RoomConstraints.Constraint ORIGINALTILES = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => 1 + room.cavity.maxY - room.cavity.minY >= 4));
  public static RoomConstraints.Constraint WILDANIMAL = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room => room.cavity.creatures.Count + room.cavity.eggs.Count > 0), name: ((string) ROOMS.CRITERIA.WILDANIMAL.NAME), description: ((string) ROOMS.CRITERIA.WILDANIMAL.DESCRIPTION));
  public static RoomConstraints.Constraint WILDANIMALS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID creature in room.cavity.creatures)
    {
      if (creature.HasTag(GameTags.Creatures.Wild))
        ++num;
    }
    return num >= 2;
  }), name: ((string) ROOMS.CRITERIA.WILDANIMALS.NAME), description: ((string) ROOMS.CRITERIA.WILDANIMALS.DESCRIPTION));
  public static RoomConstraints.Constraint WILDPLANT = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID plant in room.cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null)
      {
        BasicForagePlantPlanted component1 = plant.GetComponent<BasicForagePlantPlanted>();
        ReceptacleMonitor component2 = plant.GetComponent<ReceptacleMonitor>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.Replanted)
          ++num;
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          ++num;
      }
    }
    return num >= 2;
  }), name: ((string) ROOMS.CRITERIA.WILDPLANT.NAME), description: ((string) ROOMS.CRITERIA.WILDPLANT.DESCRIPTION));
  public static RoomConstraints.Constraint WILDPLANTS = new RoomConstraints.Constraint((Func<KPrefabID, bool>) null, (Func<Room, bool>) (room =>
  {
    int num = 0;
    foreach (KPrefabID plant in room.cavity.plants)
    {
      if ((UnityEngine.Object) plant != (UnityEngine.Object) null)
      {
        BasicForagePlantPlanted component1 = plant.GetComponent<BasicForagePlantPlanted>();
        ReceptacleMonitor component2 = plant.GetComponent<ReceptacleMonitor>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.Replanted)
          ++num;
        else if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
          ++num;
      }
    }
    return num >= 4;
  }), name: ((string) ROOMS.CRITERIA.WILDPLANTS.NAME), description: ((string) ROOMS.CRITERIA.WILDPLANTS.DESCRIPTION));

  public static string RoomCriteriaString(Room room)
  {
    string str1 = "";
    RoomType roomType = room.roomType;
    string str2;
    if (roomType != Db.Get().RoomTypes.Neutral)
    {
      str2 = str1 + "<b>" + (string) ROOMS.CRITERIA.HEADER + "</b>" + "\n    • " + roomType.primary_constraint.name;
      if (roomType.additional_constraints != null)
      {
        foreach (RoomConstraints.Constraint additionalConstraint in roomType.additional_constraints)
          str2 = !additionalConstraint.isSatisfied(room) ? str2 + "\n<color=#F44A47FF>    • " + additionalConstraint.name + "</color>" : str2 + "\n    • " + additionalConstraint.name;
      }
    }
    else
    {
      RoomType[] possibleRoomTypes = Db.Get().RoomTypes.GetPossibleRoomTypes(room);
      str2 = str1 + (possibleRoomTypes.Length > 1 ? "<b>" + (string) ROOMS.CRITERIA.POSSIBLE_TYPES_HEADER + "</b>" : "");
      foreach (RoomType suspected_type in possibleRoomTypes)
      {
        if (suspected_type != Db.Get().RoomTypes.Neutral)
        {
          if (str2 != "")
            str2 += "\n";
          str2 = str2 + "<b><color=#BCBCBC>    • " + suspected_type.Name + "</b> (" + suspected_type.primary_constraint.name + ")</color>";
          bool flag1 = false;
          if (suspected_type.additional_constraints != null)
          {
            foreach (RoomConstraints.Constraint additionalConstraint in suspected_type.additional_constraints)
            {
              if (!additionalConstraint.isSatisfied(room))
              {
                flag1 = true;
                str2 = additionalConstraint.building_criteria == null ? str2 + "\n<color=#F44A47FF>        • " + string.Format((string) ROOMS.CRITERIA.CRITERIA_FAILED.FAILED, (object) additionalConstraint.name) + "</color>" : str2 + "\n<color=#F44A47FF>        • " + string.Format((string) ROOMS.CRITERIA.CRITERIA_FAILED.MISSING_BUILDING, (object) additionalConstraint.name) + "</color>";
              }
            }
          }
          if (!flag1)
          {
            bool flag2 = false;
            foreach (RoomType resource in Db.Get().RoomTypes.resources)
            {
              if (resource != suspected_type && resource != Db.Get().RoomTypes.Neutral && Db.Get().RoomTypes.HasAmbiguousRoomType(room, suspected_type, resource))
              {
                flag2 = true;
                break;
              }
            }
            if (flag2)
              str2 = str2 + "\n<color=#F44A47FF>        • " + (string) ROOMS.CRITERIA.NO_TYPE_CONFLICTS + "</color>";
          }
        }
      }
    }
    return str2;
  }

  public static class ConstraintTags
  {
    public static Tag Bed = nameof (Bed).ToTag();
    public static Tag LuxuryBed = nameof (LuxuryBed).ToTag();
    public static Tag Toilet = nameof (Toilet).ToTag();
    public static Tag FlushToilet = nameof (FlushToilet).ToTag();
    public static Tag MessTable = nameof (MessTable).ToTag();
    public static Tag Clinic = nameof (Clinic).ToTag();
    public static Tag FoodStorage = nameof (FoodStorage).ToTag();
    public static Tag WashStation = nameof (WashStation).ToTag();
    public static Tag AdvancedWashStation = nameof (AdvancedWashStation).ToTag();
    public static Tag ResearchStation = nameof (ResearchStation).ToTag();
    public static Tag LightSource = nameof (LightSource).ToTag();
    public static Tag MassageTable = nameof (MassageTable).ToTag();
    public static Tag DeStressingBuilding = nameof (DeStressingBuilding).ToTag();
    public static Tag IndustrialMachinery = nameof (IndustrialMachinery).ToTag();
    public static Tag PowerStation = nameof (PowerStation).ToTag();
    public static Tag FarmStation = nameof (FarmStation).ToTag();
    public static Tag CreatureRelocator = nameof (CreatureRelocator).ToTag();
    public static Tag CreatureFeeder = nameof (CreatureFeeder).ToTag();
    public static Tag RanchStation = nameof (RanchStation).ToTag();
    public static Tag RecBuilding = nameof (RecBuilding).ToTag();
    public static Tag MachineShop = nameof (MachineShop).ToTag();
    public static Tag Park = nameof (Park).ToTag();
    public static Tag NatureReserve = nameof (NatureReserve).ToTag();
    public static Tag Decor20 = nameof (Decor20).ToTag();
  }

  public class Constraint
  {
    public string name;
    public string description;
    public int times_required = 1;
    public Func<Room, bool> room_criteria;
    public Func<KPrefabID, bool> building_criteria;
    public List<RoomConstraints.Constraint> stomp_in_conflict;

    public Constraint(
      Func<KPrefabID, bool> building_criteria,
      Func<Room, bool> room_criteria,
      int times_required = 1,
      string name = "",
      string description = "",
      List<RoomConstraints.Constraint> stomp_in_conflict = null)
    {
      this.room_criteria = room_criteria;
      this.building_criteria = building_criteria;
      this.times_required = times_required;
      this.name = name;
      this.description = description;
      this.stomp_in_conflict = stomp_in_conflict;
    }

    public bool isSatisfied(Room room)
    {
      int num = 0;
      if (this.room_criteria != null && this.room_criteria(room))
        ++num;
      if (this.building_criteria != null)
      {
        foreach (KPrefabID building in room.buildings)
        {
          if (!((UnityEngine.Object) building == (UnityEngine.Object) null) && this.building_criteria(building))
            ++num;
        }
        foreach (KPrefabID plant in room.plants)
        {
          if (!((UnityEngine.Object) plant == (UnityEngine.Object) null) && this.building_criteria(plant))
            ++num;
        }
      }
      return num >= this.times_required;
    }
  }
}
