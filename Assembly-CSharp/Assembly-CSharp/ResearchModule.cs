// Decompiled with JetBrains decompiler
// Type: ResearchModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ResearchModule")]
public class ResearchModule : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ResearchModule>((System.Action<ResearchModule, object>) ((component, data) => component.OnLaunch(data)));
  private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLandDelegate = new EventSystem.IntraObjectHandler<ResearchModule>((System.Action<ResearchModule, object>) ((component, data) => component.OnLand(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "grounded", KAnim.PlayMode.Loop);
    this.Subscribe<ResearchModule>(-1056989049, ResearchModule.OnLaunchDelegate);
    this.Subscribe<ResearchModule>(238242047, ResearchModule.OnLandDelegate);
  }

  public void OnLaunch(object data)
  {
  }

  public void OnLand(object data)
  {
    SpaceDestination.ResearchOpportunity researchOpportunity = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(this.GetComponent<RocketModule>().conditionManager.GetComponent<LaunchableRocket>())).TryCompleteResearchOpportunity();
    if (researchOpportunity != null)
    {
      GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "ResearchDatabank"), this.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore);
      gameObject.SetActive(true);
      gameObject.GetComponent<PrimaryElement>().Mass = (float) researchOpportunity.dataValue;
      if (!string.IsNullOrEmpty(researchOpportunity.discoveredRareItem))
      {
        GameObject prefab = Assets.GetPrefab((Tag) researchOpportunity.discoveredRareItem);
        if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
          KCrashReporter.Assert(false, "Missing prefab: " + researchOpportunity.discoveredRareItem);
        else
          GameUtil.KInstantiate(prefab, this.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore).SetActive(true);
      }
    }
    GameObject gameObject1 = GameUtil.KInstantiate(Assets.GetPrefab((Tag) "ResearchDatabank"), this.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore);
    gameObject1.SetActive(true);
    gameObject1.GetComponent<PrimaryElement>().Mass = (float) ROCKETRY.DESTINATION_RESEARCH.EVERGREEN;
  }
}
