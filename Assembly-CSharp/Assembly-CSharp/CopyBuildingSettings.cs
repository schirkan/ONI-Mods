// Decompiled with JetBrains decompiler
// Type: CopyBuildingSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/CopyBuildingSettings")]
public class CopyBuildingSettings : KMonoBehaviour
{
  [MyCmpReq]
  private KPrefabID id;
  public Tag copyGroupTag;
  private static readonly EventSystem.IntraObjectHandler<CopyBuildingSettings> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CopyBuildingSettings>((System.Action<CopyBuildingSettings, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<CopyBuildingSettings>(493375141, CopyBuildingSettings.OnRefreshUserMenuDelegate);
  }

  private void OnRefreshUserMenu(object data) => Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_mirror", (string) UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME, new System.Action(this.ActivateCopyTool), Action.BuildingUtility1, tooltipText: ((string) UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP)));

  private void ActivateCopyTool()
  {
    CopySettingsTool.Instance.SetSourceObject(this.gameObject);
    PlayerController.Instance.ActivateTool((InterfaceTool) CopySettingsTool.Instance);
  }

  public static bool ApplyCopy(int targetCell, GameObject sourceGameObject)
  {
    ObjectLayer objectLayer = ObjectLayer.Building;
    Building component1 = (Building) sourceGameObject.GetComponent<BuildingComplete>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      objectLayer = component1.Def.ObjectLayer;
    GameObject gameObject = Grid.Objects[targetCell, (int) objectLayer];
    if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null || (UnityEngine.Object) gameObject == (UnityEngine.Object) sourceGameObject)
      return false;
    KPrefabID component2 = sourceGameObject.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component2 == (UnityEngine.Object) null)
      return false;
    KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component3 == (UnityEngine.Object) null)
      return false;
    CopyBuildingSettings component4 = sourceGameObject.GetComponent<CopyBuildingSettings>();
    if ((UnityEngine.Object) component4 == (UnityEngine.Object) null)
      return false;
    CopyBuildingSettings component5 = gameObject.GetComponent<CopyBuildingSettings>();
    if ((UnityEngine.Object) component5 == (UnityEngine.Object) null)
      return false;
    if (component4.copyGroupTag != Tag.Invalid)
    {
      if (component4.copyGroupTag != component5.copyGroupTag)
        return false;
    }
    else if (component3.PrefabID() != component2.PrefabID())
      return false;
    component3.Trigger(-905833192, (object) sourceGameObject);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, (string) UI.COPIED_SETTINGS, gameObject.transform, new Vector3(0.0f, 0.5f, 0.0f));
    return true;
  }
}
