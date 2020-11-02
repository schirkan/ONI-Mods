// Decompiled with JetBrains decompiler
// Type: TravelTubeUtilityNetworkLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class TravelTubeUtilityNetworkLink : UtilityNetworkLink, IHaveUtilityNetworkMgr
{
  protected override void OnSpawn() => base.OnSpawn();

  protected override void OnConnect(int cell1, int cell2) => Game.Instance.travelTubeSystem.AddLink(cell1, cell2);

  protected override void OnDisconnect(int cell1, int cell2) => Game.Instance.travelTubeSystem.RemoveLink(cell1, cell2);

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.travelTubeSystem;
}
