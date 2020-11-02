// Decompiled with JetBrains decompiler
// Type: Database.MinimumMorale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class MinimumMorale : VictoryColonyAchievementRequirement
  {
    public int minimumMorale;

    public override string Name() => string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, (object) this.minimumMorale);

    public override string Description() => string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, (object) this.minimumMorale);

    public MinimumMorale(int minimumMorale = 16) => this.minimumMorale = minimumMorale;

    public override bool Success()
    {
      bool flag = true;
      foreach (MinionAssignablesProxy assignablesProxy in Components.MinionAssignablesProxy)
      {
        GameObject targetGameObject = assignablesProxy.GetTargetGameObject();
        if ((Object) targetGameObject != (Object) null && !targetGameObject.HasTag(GameTags.Dead))
        {
          AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup((Component) targetGameObject.GetComponent<MinionModifiers>());
          flag = ((attributeInstance == null ? 0 : ((double) attributeInstance.GetTotalValue() >= (double) this.minimumMorale ? 1 : 0)) & (flag ? 1 : 0)) != 0;
        }
      }
      return flag;
    }

    public override void Serialize(BinaryWriter writer) => writer.Write(this.minimumMorale);

    public override void Deserialize(IReader reader) => this.minimumMorale = reader.ReadInt32();

    public override string GetProgress(bool complete) => this.Description();
  }
}
