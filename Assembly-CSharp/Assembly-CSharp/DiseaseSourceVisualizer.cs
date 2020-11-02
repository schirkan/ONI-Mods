// Decompiled with JetBrains decompiler
// Type: DiseaseSourceVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/DiseaseSourceVisualizer")]
public class DiseaseSourceVisualizer : KMonoBehaviour
{
  [SerializeField]
  private Vector3 offset;
  private GameObject visualizer;
  private bool visible;
  public string alwaysShowDisease;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateVisibility();
    Components.DiseaseSourceVisualizers.Add(this);
  }

  protected override void OnCleanUp()
  {
    OverlayScreen.Instance.OnOverlayChanged -= new System.Action<HashedString>(this.OnViewModeChanged);
    base.OnCleanUp();
    Components.DiseaseSourceVisualizers.Remove(this);
    if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.visualizer);
    this.visualizer = (GameObject) null;
  }

  private void CreateVisualizer()
  {
    if ((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null || (UnityEngine.Object) GameScreenManager.Instance.worldSpaceCanvas == (UnityEngine.Object) null)
      return;
    this.visualizer = Util.KInstantiate(Assets.UIPrefabs.ResourceVisualizer, GameScreenManager.Instance.worldSpaceCanvas);
  }

  public void UpdateVisibility()
  {
    this.CreateVisualizer();
    if (string.IsNullOrEmpty(this.alwaysShowDisease))
    {
      this.visible = false;
    }
    else
    {
      Klei.AI.Disease disease = Db.Get().Diseases.Get(this.alwaysShowDisease);
      if (disease != null)
        this.SetVisibleDisease(disease);
    }
    if (!((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null))
      return;
    this.Show(OverlayScreen.Instance.GetMode());
  }

  private void SetVisibleDisease(Klei.AI.Disease disease)
  {
    Sprite overlaySprite = Assets.instance.DiseaseVisualization.overlaySprite;
    Color32 colorByName = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
    Image component = this.visualizer.transform.GetChild(0).GetComponent<Image>();
    component.sprite = overlaySprite;
    component.color = (Color) colorByName;
    this.visible = true;
  }

  private void Update()
  {
    if ((UnityEngine.Object) this.visualizer == (UnityEngine.Object) null)
      return;
    this.visualizer.transform.SetPosition(this.transform.GetPosition() + this.offset);
  }

  private void OnViewModeChanged(HashedString mode) => this.Show(mode);

  public void Show(HashedString mode)
  {
    this.enabled = this.visible && mode == OverlayModes.Disease.ID;
    if (!((UnityEngine.Object) this.visualizer != (UnityEngine.Object) null))
      return;
    this.visualizer.SetActive(this.enabled);
  }
}
