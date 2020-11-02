// Decompiled with JetBrains decompiler
// Type: GameSoundEvents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public static class GameSoundEvents
{
  public static GameSoundEvents.Event BatteryFull = new GameSoundEvents.Event("game_triggered.battery_full");
  public static GameSoundEvents.Event BatteryWarning = new GameSoundEvents.Event("game_triggered.battery_warning");
  public static GameSoundEvents.Event BatteryDischarged = new GameSoundEvents.Event("game_triggered.battery_drained");

  public class Event
  {
    public HashedString Name;

    public Event(string name) => this.Name = (HashedString) name;
  }
}
