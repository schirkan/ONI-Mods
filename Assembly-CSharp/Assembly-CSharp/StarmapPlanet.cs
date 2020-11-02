// Decompiled with JetBrains decompiler
// Type: StarmapPlanet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanet")]
public class StarmapPlanet : KMonoBehaviour
{
  public List<StarmapPlanetVisualizer> visualizers;

  public void SetSprite(Sprite sprite, Color color)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
    {
      visualizer.image.sprite = sprite;
      visualizer.image.color = color;
    }
  }

  public void SetFillAmount(float amount)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.image.fillAmount = amount;
  }

  public void SetUnknownBGActive(bool active, Color color)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
    {
      visualizer.unknownBG.gameObject.SetActive(active);
      visualizer.unknownBG.color = color;
    }
  }

  public void SetSelectionActive(bool active)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.selection.gameObject.SetActive(active);
  }

  public void SetAnalysisActive(bool active)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.analysisSelection.SetActive(active);
  }

  public void SetLabel(string text)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
    {
      visualizer.label.text = text;
      this.ShowLabel(false);
    }
  }

  public void ShowLabel(bool show)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.label.gameObject.SetActive(show);
  }

  public void SetOnClick(System.Action del)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.button.onClick = del;
  }

  public void SetOnEnter(System.Action del)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.button.onEnter = del;
  }

  public void SetOnExit(System.Action del)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.button.onExit = del;
  }

  public void AnimateSelector(float time)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
      visualizer.selection.anchoredPosition = new Vector2(0.0f, (float) (25.0 + (double) Mathf.Sin(time * 4f) * 5.0));
  }

  public void ShowAsCurrentRocketDestination(bool show)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
    {
      RectTransform rectTransform = visualizer.rocketIconContainer.rectTransform();
      if (rectTransform.childCount > 0)
        rectTransform.GetChild(rectTransform.childCount - 1).GetComponent<HierarchyReferences>().GetReference<Image>("fg").color = show ? new Color(0.1176471f, 0.8627451f, 0.3137255f) : Color.white;
    }
  }

  public void SetRocketIcons(int numRockets, GameObject iconPrefab)
  {
    foreach (StarmapPlanetVisualizer visualizer in this.visualizers)
    {
      RectTransform rectTransform1 = visualizer.rocketIconContainer.rectTransform();
      for (int childCount = rectTransform1.childCount; childCount < numRockets; ++childCount)
        Util.KInstantiateUI(iconPrefab, visualizer.rocketIconContainer, true);
      for (int childCount = rectTransform1.childCount; childCount > numRockets; --childCount)
        UnityEngine.Object.Destroy((UnityEngine.Object) rectTransform1.GetChild(childCount - 1).gameObject);
      int num = 0;
      foreach (RectTransform rectTransform2 in (Transform) rectTransform1)
      {
        rectTransform2.anchoredPosition = new Vector2((float) num * -10f, 0.0f);
        ++num;
      }
    }
  }
}
