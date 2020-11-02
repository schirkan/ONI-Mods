// Decompiled with JetBrains decompiler
// Type: DescriptorPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/DescriptorPanel")]
public class DescriptorPanel : KMonoBehaviour
{
  [SerializeField]
  private GameObject customLabelPrefab;
  private List<GameObject> labels = new List<GameObject>();

  public bool HasDescriptors() => this.labels.Count > 0;

  public void SetDescriptors(IList<Descriptor> descriptors)
  {
    int index;
    for (index = 0; index < descriptors.Count; ++index)
    {
      GameObject gameObject;
      if (index >= this.labels.Count)
      {
        gameObject = Util.KInstantiate((Object) this.customLabelPrefab != (Object) null ? this.customLabelPrefab : ScreenPrefabs.Instance.DescriptionLabel, this.gameObject);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.labels.Add(gameObject);
      }
      else
        gameObject = this.labels[index];
      gameObject.GetComponent<LocText>().text = descriptors[index].IndentedText();
      gameObject.GetComponent<ToolTip>().toolTip = descriptors[index].tooltipText;
      gameObject.SetActive(true);
    }
    for (; index < this.labels.Count; ++index)
      this.labels[index].SetActive(false);
  }
}
