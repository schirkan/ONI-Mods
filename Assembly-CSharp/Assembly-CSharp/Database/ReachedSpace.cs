// Decompiled with JetBrains decompiler
// Type: Database.ReachedSpace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.IO;

namespace Database
{
  public class ReachedSpace : VictoryColonyAchievementRequirement
  {
    private SpaceDestinationType destinationType;

    public ReachedSpace(SpaceDestinationType destinationType = null) => this.destinationType = destinationType;

    public override string Name() => this.destinationType != null ? string.Format((string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION, (object) this.destinationType.Name) : (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION;

    public override string Description() => this.destinationType != null ? string.Format((string) COLONY_ACHIEVEMENTS.DISTANT_PLANET_REACHED.REQUIREMENTS.REACHED_SPACE_DESTINATION_DESCRIPTION, (object) this.destinationType.Name) : (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.REACH_SPACE_ANY_DESTINATION_DESCRIPTION;

    public override bool Success()
    {
      foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
      {
        if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
        {
          SpaceDestination destination = SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.savedSpacecraftDestinations[spacecraft.id]);
          if (this.destinationType == null || destination.GetDestinationType() == this.destinationType)
          {
            if (this.destinationType == Db.Get().SpaceDestinationTypes.Wormhole)
              Game.Instance.unlocks.Unlock("temporaltear");
            return true;
          }
        }
      }
      return SpacecraftManager.instance.hasVisitedWormHole;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.destinationType == null ? (byte) 1 : (byte) 0);
      if (this.destinationType == null)
        return;
      writer.WriteKleiString(this.destinationType.Id);
    }

    public override void Deserialize(IReader reader)
    {
      if (reader.ReadByte() > (byte) 0)
        return;
      string id = reader.ReadKleiString();
      this.destinationType = Db.Get().SpaceDestinationTypes.Get(id);
    }

    public override string GetProgress(bool completed) => this.destinationType == null ? (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET : (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.LAUNCHED_ROCKET_TO_WORMHOLE;
  }
}
