﻿// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Database
{
  public class ColonyAchievement : Resource
  {
    public string description;
    public bool isVictoryCondition;
    public string messageTitle;
    public string messageBody;
    public string shortVideoName;
    public string loopVideoName;
    public string steamAchievementId;
    public string icon;
    public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();
    public System.Action<KMonoBehaviour> victorySequence;

    public string victoryNISSnapshot { get; private set; }

    public ColonyAchievement(
      string Id,
      string steamAchievementId,
      string Name,
      string description,
      bool isVictoryCondition,
      List<ColonyAchievementRequirement> requirementChecklist,
      string messageTitle = "",
      string messageBody = "",
      string videoDataName = "",
      string victoryLoopVideo = "",
      System.Action<KMonoBehaviour> VictorySequence = null,
      string victorySnapshot = "",
      string icon = "")
      : base(Id, Name)
    {
      this.Id = Id;
      this.steamAchievementId = steamAchievementId;
      this.Name = Name;
      this.description = description;
      this.isVictoryCondition = isVictoryCondition;
      this.requirementChecklist = requirementChecklist;
      this.messageTitle = messageTitle;
      this.messageBody = messageBody;
      this.shortVideoName = videoDataName;
      this.loopVideoName = victoryLoopVideo;
      this.victorySequence = VictorySequence;
      this.victoryNISSnapshot = string.IsNullOrEmpty(victorySnapshot) ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot;
      this.icon = icon;
    }
  }
}
