﻿// Decompiled with JetBrains decompiler
// Type: Database.ChoreGroups
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;

namespace Database
{
  public class ChoreGroups : ResourceSet<ChoreGroup>
  {
    public ChoreGroup Build;
    public ChoreGroup Basekeeping;
    public ChoreGroup Cook;
    public ChoreGroup Art;
    public ChoreGroup Dig;
    public ChoreGroup Research;
    public ChoreGroup Farming;
    public ChoreGroup Ranching;
    public ChoreGroup Hauling;
    public ChoreGroup Storage;
    public ChoreGroup MachineOperating;
    public ChoreGroup MedicalAid;
    public ChoreGroup Combat;
    public ChoreGroup LifeSupport;
    public ChoreGroup Toggle;

    private ChoreGroup Add(
      string id,
      string name,
      Attribute attribute,
      string sprite,
      int default_personal_priority)
    {
      ChoreGroup resource = new ChoreGroup(id, name, attribute, sprite, default_personal_priority);
      this.Add(resource);
      return resource;
    }

    public ChoreGroups(ResourceSet parent)
      : base(nameof (ChoreGroups), parent)
    {
      this.Combat = this.Add(nameof (Combat), (string) DUPLICANTS.CHOREGROUPS.COMBAT.NAME, Db.Get().Attributes.Digging, "icon_errand_combat", 5);
      this.LifeSupport = this.Add(nameof (LifeSupport), (string) DUPLICANTS.CHOREGROUPS.LIFESUPPORT.NAME, Db.Get().Attributes.LifeSupport, "icon_errand_life_support", 5);
      this.Toggle = this.Add(nameof (Toggle), (string) DUPLICANTS.CHOREGROUPS.TOGGLE.NAME, Db.Get().Attributes.Toggle, "icon_errand_toggle", 5);
      this.MedicalAid = this.Add(nameof (MedicalAid), (string) DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, Db.Get().Attributes.Caring, "icon_errand_care", 4);
      this.Basekeeping = this.Add(nameof (Basekeeping), (string) DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, Db.Get().Attributes.Strength, "icon_errand_tidy", 4);
      this.Cook = this.Add(nameof (Cook), (string) DUPLICANTS.CHOREGROUPS.COOK.NAME, Db.Get().Attributes.Cooking, "icon_errand_cook", 3);
      this.Art = this.Add(nameof (Art), (string) DUPLICANTS.CHOREGROUPS.ART.NAME, Db.Get().Attributes.Art, "icon_errand_art", 3);
      this.Research = this.Add(nameof (Research), (string) DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, Db.Get().Attributes.Learning, "icon_errand_research", 3);
      this.MachineOperating = this.Add(nameof (MachineOperating), (string) DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, Db.Get().Attributes.Machinery, "icon_errand_operate", 3);
      this.Farming = this.Add(nameof (Farming), (string) DUPLICANTS.CHOREGROUPS.FARMING.NAME, Db.Get().Attributes.Botanist, "icon_errand_farm", 3);
      this.Ranching = this.Add(nameof (Ranching), (string) DUPLICANTS.CHOREGROUPS.RANCHING.NAME, Db.Get().Attributes.Ranching, "icon_errand_ranch", 3);
      this.Build = this.Add(nameof (Build), (string) DUPLICANTS.CHOREGROUPS.BUILD.NAME, Db.Get().Attributes.Construction, "icon_errand_toggle", 2);
      this.Dig = this.Add(nameof (Dig), (string) DUPLICANTS.CHOREGROUPS.DIG.NAME, Db.Get().Attributes.Digging, "icon_errand_dig", 2);
      this.Hauling = this.Add(nameof (Hauling), (string) DUPLICANTS.CHOREGROUPS.HAULING.NAME, Db.Get().Attributes.Strength, "icon_errand_supply", 1);
      this.Storage = this.Add(nameof (Storage), (string) DUPLICANTS.CHOREGROUPS.STORAGE.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1);
      Debug.Assert(true);
    }

    public ChoreGroup FindByHash(HashedString id)
    {
      ChoreGroup choreGroup = (ChoreGroup) null;
      foreach (ChoreGroup resource in Db.Get().ChoreGroups.resources)
      {
        if (resource.IdHash == id)
        {
          choreGroup = resource;
          break;
        }
      }
      return choreGroup;
    }
  }
}
