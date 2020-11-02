// Decompiled with JetBrains decompiler
// Type: SolidConduit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IHaveUtilityNetworkMgr
{
  [MyCmpReq]
  private KAnimGraphTileVisualizer graphTileDependency;
  private System.Action firstFrameCallback;

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  private IEnumerator RunCallback()
  {
    yield return (object) null;
    if (this.firstFrameCallback != null)
    {
      this.firstFrameCallback();
      this.firstFrameCallback = (System.Action) null;
    }
    yield return (object) null;
  }

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.solidConduitSystem;

  public UtilityNetwork GetNetwork() => this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell((KMonoBehaviour) this));

  public static SolidConduitFlow GetFlowManager() => Game.Instance.solidConduitFlow;

  public Vector3 Position => this.transform.GetPosition();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, (object) this);
  }

  protected override void OnCleanUp()
  {
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
    {
      this.GetNetworkManager().RemoveFromNetworks(cell, (object) this, false);
      SolidConduit.GetFlowManager().EmptyConduit(cell);
    }
    base.OnCleanUp();
  }
}
