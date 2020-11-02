// Decompiled with JetBrains decompiler
// Type: TemperatureCookable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/TemperatureCookable")]
public class TemperatureCookable : KMonoBehaviour, ISim1000ms
{
  [MyCmpReq]
  private PrimaryElement element;
  public float cookTemperature = 273150f;
  public string cookedID;

  public void Sim1000ms(float dt)
  {
    if ((double) this.element.Temperature <= (double) this.cookTemperature || this.cookedID == null)
      return;
    this.Cook();
  }

  private void Cook()
  {
    Vector3 position = this.transform.GetPosition();
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) this.cookedID), position);
    gameObject.SetActive(true);
    KSelectable component1 = this.gameObject.GetComponent<KSelectable>();
    if ((Object) SelectTool.Instance != (Object) null && (Object) SelectTool.Instance.selected != (Object) null && (Object) SelectTool.Instance.selected == (Object) component1)
      SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
    PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
    component2.Temperature = this.element.Temperature;
    component2.Mass = this.element.Mass;
    this.gameObject.DeleteObject();
  }
}
