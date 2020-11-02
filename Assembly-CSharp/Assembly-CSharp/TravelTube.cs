// Decompiled with JetBrains decompiler
// Type: TravelTube
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTube")]
public class TravelTube : KMonoBehaviour, IFirstFrameCallback, ITravelTubePiece, IHaveUtilityNetworkMgr
{
  [MyCmpReq]
  private KSelectable selectable;
  private HandleVector<int>.Handle dirtyNavCellUpdatedEntry;
  private bool isExitTube;
  private bool hasValidExitTransitions;
  private UtilityConnections connections;
  private static readonly EventSystem.IntraObjectHandler<TravelTube> OnConnectionsChangedDelegate = new EventSystem.IntraObjectHandler<TravelTube>((System.Action<TravelTube, object>) ((component, data) => component.OnConnectionsChanged(data)));
  private Guid connectedStatus;
  private System.Action firstFrameCallback;

  public IUtilityNetworkMgr GetNetworkManager() => (IUtilityNetworkMgr) Game.Instance.travelTubeSystem;

  public Vector3 Position => this.transform.GetPosition();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Grid.HasTube[Grid.PosToCell((KMonoBehaviour) this)] = true;
    Components.ITravelTubePieces.Add((ITravelTubePiece) this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.travelTubeSystem.AddToNetworks(Grid.PosToCell(this.transform.GetPosition()), (object) this, false);
    this.Subscribe<TravelTube>(-1041684577, TravelTube.OnConnectionsChangedDelegate);
  }

  protected override void OnCleanUp()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
      Game.Instance.travelTubeSystem.RemoveFromNetworks(cell, (object) this, false);
    this.Unsubscribe(-1041684577);
    Grid.HasTube[Grid.PosToCell((KMonoBehaviour) this)] = false;
    Components.ITravelTubePieces.Remove((ITravelTubePiece) this);
    GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
    base.OnCleanUp();
  }

  private void OnConnectionsChanged(object data)
  {
    this.connections = (UtilityConnections) data;
    bool flag = this.connections == UtilityConnections.Up || this.connections == UtilityConnections.Down || this.connections == UtilityConnections.Left || this.connections == UtilityConnections.Right;
    if (flag == this.isExitTube)
      return;
    this.isExitTube = flag;
    this.UpdateExitListener(this.isExitTube);
    this.UpdateExitStatus();
  }

  private void UpdateExitListener(bool enable)
  {
    if (enable && !this.dirtyNavCellUpdatedEntry.IsValid())
    {
      this.dirtyNavCellUpdatedEntry = GameScenePartitioner.Instance.Add("TravelTube.OnDirtyNavCellUpdated", (object) this, Grid.PosToCell(this.transform.GetPosition()), GameScenePartitioner.Instance.dirtyNavCellUpdateLayer, new System.Action<object>(this.OnDirtyNavCellUpdated));
      this.OnDirtyNavCellUpdated((object) null);
    }
    else
    {
      if (enable || !this.dirtyNavCellUpdatedEntry.IsValid())
        return;
      GameScenePartitioner.Instance.Free(ref this.dirtyNavCellUpdatedEntry);
    }
  }

  private void OnDirtyNavCellUpdated(object data)
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    NavGrid navGrid = Pathfinding.Instance.GetNavGrid("MinionNavGrid");
    int index = cell * navGrid.maxLinksPerCell;
    bool flag = false;
    if (this.isExitTube)
    {
      for (NavGrid.Link link = navGrid.Links[index]; link.link != PathFinder.InvalidHandle; link = navGrid.Links[index])
      {
        if (link.startNavType == NavType.Tube)
        {
          if (link.endNavType != NavType.Tube)
          {
            flag = true;
            break;
          }
          if (this.connections == UtilityConnectionsExtensions.DirectionFromToCell(link.link, cell))
          {
            flag = true;
            break;
          }
        }
        ++index;
      }
    }
    if (flag == this.hasValidExitTransitions)
      return;
    this.hasValidExitTransitions = flag;
    this.UpdateExitStatus();
  }

  private void UpdateExitStatus()
  {
    if (!this.isExitTube || this.hasValidExitTransitions)
    {
      this.connectedStatus = this.selectable.RemoveStatusItem(this.connectedStatus);
    }
    else
    {
      if (!(this.connectedStatus == Guid.Empty))
        return;
      this.connectedStatus = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NoTubeExits);
    }
  }

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
}
