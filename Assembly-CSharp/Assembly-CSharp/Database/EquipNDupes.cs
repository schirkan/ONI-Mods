﻿// Decompiled with JetBrains decompiler
// Type: Database.EquipNDupes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class EquipNDupes : ColonyAchievementRequirement
  {
    private AssignableSlot equipmentSlot;
    private int numToEquip;

    public EquipNDupes(AssignableSlot equipmentSlot, int numToEquip)
    {
      this.equipmentSlot = equipmentSlot;
      this.numToEquip = numToEquip;
    }

    public override bool Success()
    {
      int num = 0;
      foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
      {
        Equipment equipment = minionIdentity.GetEquipment();
        if ((Object) equipment != (Object) null && equipment.IsSlotOccupied(this.equipmentSlot))
          ++num;
      }
      return num >= this.numToEquip;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.WriteKleiString(this.equipmentSlot.Id);
      writer.Write(this.numToEquip);
    }

    public override void Deserialize(IReader reader)
    {
      string id = reader.ReadKleiString();
      this.equipmentSlot = Db.Get().AssignableSlots.Get(id);
      this.numToEquip = reader.ReadInt32();
    }

    public override string GetProgress(bool complete)
    {
      int num = 0;
      foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
      {
        Equipment equipment = minionIdentity.GetEquipment();
        if ((Object) equipment != (Object) null && equipment.IsSlotOccupied(this.equipmentSlot))
          ++num;
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CLOTHE_DUPES, (object) (complete ? this.numToEquip : num), (object) this.numToEquip);
    }
  }
}
