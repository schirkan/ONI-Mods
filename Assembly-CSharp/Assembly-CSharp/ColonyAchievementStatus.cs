// Decompiled with JetBrains decompiler
// Type: ColonyAchievementStatus
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using KSerialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

public class ColonyAchievementStatus
{
  public bool success;
  public bool failed;
  private List<ColonyAchievementRequirement> requirements = new List<ColonyAchievementRequirement>();

  public List<ColonyAchievementRequirement> Requirements => this.requirements;

  public void UpdateAchievement()
  {
    if (this.requirements == null || this.requirements.Count <= 0)
      return;
    this.success = true;
    foreach (ColonyAchievementRequirement requirement in this.requirements)
    {
      requirement.Update();
      this.success &= requirement.Success();
      this.failed |= requirement.Fail();
    }
  }

  public void Deserialize(IReader reader)
  {
    this.success = reader.ReadByte() > (byte) 0;
    this.failed = reader.ReadByte() > (byte) 0;
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      System.Type type = System.Type.GetType(reader.ReadKleiString());
      if (type != (System.Type) null)
      {
        ColonyAchievementRequirement uninitializedObject = (ColonyAchievementRequirement) FormatterServices.GetUninitializedObject(type);
        uninitializedObject.Deserialize(reader);
        this.requirements.Add(uninitializedObject);
      }
    }
  }

  public void SetRequirements(
    List<ColonyAchievementRequirement> requirementChecklist)
  {
    this.requirements = requirementChecklist;
  }

  public void Serialize(BinaryWriter writer)
  {
    writer.Write(this.success ? (byte) 1 : (byte) 0);
    writer.Write(this.failed ? (byte) 1 : (byte) 0);
    writer.Write(this.requirements != null ? this.requirements.Count : 0);
    if (this.requirements == null)
      return;
    foreach (ColonyAchievementRequirement requirement in this.requirements)
    {
      writer.WriteKleiString(requirement.GetType().ToString());
      requirement.Serialize(writer);
    }
  }
}
