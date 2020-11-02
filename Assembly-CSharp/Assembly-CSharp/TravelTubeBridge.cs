// Decompiled with JetBrains decompiler
// Type: TravelTubeBridge
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTubeBridge")]
public class TravelTubeBridge : KMonoBehaviour, ITravelTubePiece
{
  private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>((System.Action<TravelTubeBridge, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>((System.Action<TravelTubeBridge, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));

  public Vector3 Position => this.transform.GetPosition();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Grid.HasTube[Grid.PosToCell((KMonoBehaviour) this)] = true;
    Components.ITravelTubePieces.Add((ITravelTubePiece) this);
    this.Subscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
    this.Subscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
    this.Unsubscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
    Grid.HasTube[Grid.PosToCell((KMonoBehaviour) this)] = false;
    Components.ITravelTubePieces.Remove((ITravelTubePiece) this);
    base.OnCleanUp();
  }

  private void OnBuildingBroken(object data)
  {
  }

  private void OnBuildingFullyRepaired(object data)
  {
  }
}
