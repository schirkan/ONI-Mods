// Decompiled with JetBrains decompiler
// Type: AttachableBuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AttachableBuilding")]
public class AttachableBuilding : KMonoBehaviour
{
  public Tag attachableToTag;
  public System.Action<AttachableBuilding> onAttachmentNetworkChanged;
  private static readonly EventSystem.IntraObjectHandler<AttachableBuilding> AttachmentNetworkChangedDelegate = new EventSystem.IntraObjectHandler<AttachableBuilding>((System.Action<AttachableBuilding, object>) ((component, data) => component.AttachmentNetworkChanged(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.RegisterWithAttachPoint(true);
    Components.AttachableBuildings.Add(this);
    this.Subscribe<AttachableBuilding>(486707561, AttachableBuilding.AttachmentNetworkChangedDelegate);
    foreach (GameObject go in AttachableBuilding.GetAttachedNetwork(this))
      go.Trigger(486707561, (object) this);
  }

  protected override void OnSpawn() => base.OnSpawn();

  private void AttachmentNetworkChanged(object attachableBuilding)
  {
    if (this.onAttachmentNetworkChanged == null)
      return;
    this.onAttachmentNetworkChanged((AttachableBuilding) attachableBuilding);
  }

  public void RegisterWithAttachPoint(bool register)
  {
    int num = Grid.OffsetCell(Grid.PosToCell(this.gameObject), Assets.GetBuildingDef(this.GetComponent<KPrefabID>().PrefabID().Name).attachablePosition);
    bool flag = false;
    for (int idx = 0; !flag && idx < Components.BuildingAttachPoints.Count; ++idx)
    {
      for (int index = 0; index < Components.BuildingAttachPoints[idx].points.Length; ++index)
      {
        if (num == Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) Components.BuildingAttachPoints[idx]), Components.BuildingAttachPoints[idx].points[index].position))
        {
          Components.BuildingAttachPoints[idx].points[index].attachedBuilding = register ? this : (AttachableBuilding) null;
          flag = true;
          break;
        }
      }
    }
  }

  public static List<GameObject> GetAttachedNetwork(AttachableBuilding tip)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    gameObjectList.Add(tip.gameObject);
    AttachableBuilding attachableBuilding1 = tip;
    while ((UnityEngine.Object) attachableBuilding1 != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = attachableBuilding1.GetAttachedTo();
      attachableBuilding1 = (AttachableBuilding) null;
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null)
      {
        gameObjectList.Add(attachedTo.gameObject);
        attachableBuilding1 = attachedTo.GetComponent<AttachableBuilding>();
      }
    }
    BuildingAttachPoint buildingAttachPoint = tip.GetComponent<BuildingAttachPoint>();
    while ((UnityEngine.Object) buildingAttachPoint != (UnityEngine.Object) null)
    {
      bool flag = false;
      foreach (BuildingAttachPoint.HardPoint point in buildingAttachPoint.points)
      {
        if (!flag)
        {
          if ((UnityEngine.Object) point.attachedBuilding != (UnityEngine.Object) null)
          {
            foreach (AttachableBuilding attachableBuilding2 in Components.AttachableBuildings)
            {
              if ((UnityEngine.Object) attachableBuilding2 == (UnityEngine.Object) point.attachedBuilding)
              {
                gameObjectList.Add(attachableBuilding2.gameObject);
                buildingAttachPoint = attachableBuilding2.GetComponent<BuildingAttachPoint>();
                flag = true;
              }
            }
          }
        }
        else
          break;
      }
      if (!flag)
        buildingAttachPoint = (BuildingAttachPoint) null;
    }
    return gameObjectList;
  }

  public BuildingAttachPoint GetAttachedTo()
  {
    for (int idx = 0; idx < Components.BuildingAttachPoints.Count; ++idx)
    {
      for (int index = 0; index < Components.BuildingAttachPoints[idx].points.Length; ++index)
      {
        if ((UnityEngine.Object) Components.BuildingAttachPoints[idx].points[index].attachedBuilding == (UnityEngine.Object) this)
          return Components.BuildingAttachPoints[idx];
      }
    }
    return (BuildingAttachPoint) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.RegisterWithAttachPoint(false);
    Components.AttachableBuildings.Remove(this);
  }
}
