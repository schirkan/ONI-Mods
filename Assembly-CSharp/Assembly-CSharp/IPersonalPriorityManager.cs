// Decompiled with JetBrains decompiler
// Type: IPersonalPriorityManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public interface IPersonalPriorityManager
{
  int GetAssociatedSkillLevel(ChoreGroup group);

  int GetPersonalPriority(ChoreGroup group);

  void SetPersonalPriority(ChoreGroup group, int value);

  bool IsChoreGroupDisabled(ChoreGroup group);

  void ResetPersonalPriorities();
}
