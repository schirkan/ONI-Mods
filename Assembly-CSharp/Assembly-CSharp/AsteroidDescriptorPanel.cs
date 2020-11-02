// Decompiled with JetBrains decompiler
// Type: AsteroidDescriptorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject customLabelPrefab;
  private List<GameObject> labels = new List<GameObject>();

  public bool HasDescriptors() => this.labels.Count > 0;

  public void SetDescriptors(IList<AsteroidDescriptor> descriptors)
  {
    int index1;
    for (index1 = 0; index1 < descriptors.Count; ++index1)
    {
      GameObject gameObject;
      if (index1 >= this.labels.Count)
      {
        gameObject = Util.KInstantiate((Object) this.customLabelPrefab != (Object) null ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, this.gameObject);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.labels.Add(gameObject);
      }
      else
        gameObject = this.labels[index1];
      HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
      component1.GetReference<LocText>("Label").text = descriptors[index1].text;
      component1.GetReference<ToolTip>("ToolTip").toolTip = descriptors[index1].tooltip;
      if (descriptors[index1].bands != null)
      {
        Transform reference1 = component1.GetReference<Transform>("BandContainer");
        Transform reference2 = component1.GetReference<Transform>("BarBitPrefab");
        int index2;
        for (index2 = 0; index2 < descriptors[index1].bands.Count; ++index2)
        {
          Transform transform = index2 < reference1.childCount ? reference1.GetChild(index2) : Util.KInstantiateUI<Transform>(reference2.gameObject, reference1.gameObject);
          Image component2 = transform.GetComponent<Image>();
          LayoutElement component3 = transform.GetComponent<LayoutElement>();
          component2.color = descriptors[index1].bands[index2].second;
          double third = (double) descriptors[index1].bands[index2].third;
          component3.flexibleWidth = (float) third;
          transform.GetComponent<ToolTip>().toolTip = descriptors[index1].bands[index2].first;
          transform.gameObject.SetActive(true);
        }
        for (; index2 < reference1.childCount; ++index2)
          reference1.GetChild(index2).gameObject.SetActive(false);
      }
      gameObject.SetActive(true);
    }
    for (; index1 < this.labels.Count; ++index1)
      this.labels[index1].SetActive(false);
  }
}
