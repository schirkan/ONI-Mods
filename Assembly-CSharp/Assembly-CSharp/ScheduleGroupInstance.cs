// Decompiled with JetBrains decompiler
// Type: ScheduleGroupInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
  [Serialize]
  private string scheduleGroupID;
  [Serialize]
  public int segments;

  public ScheduleGroup scheduleGroup
  {
    get => Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
    set => this.scheduleGroupID = value.Id;
  }

  public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
  {
    this.scheduleGroup = scheduleGroup;
    this.segments = scheduleGroup.defaultSegments;
  }
}
