// Decompiled with JetBrains decompiler
// Type: DetailScreenTabHeader
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;
using UnityEngine.UI;

public class DetailScreenTabHeader : KTabMenuHeader
{
  public float SelectedHeight = 36f;
  public float UnselectedHeight = 30f;

  public override void ActivateTabArtwork(int tabIdx)
  {
    base.ActivateTabArtwork(tabIdx);
    if (tabIdx >= this.transform.childCount)
      return;
    for (int index = 0; index < this.transform.childCount; ++index)
    {
      LayoutElement component = this.transform.GetChild(index).GetComponent<LayoutElement>();
      if ((Object) component != (Object) null)
      {
        if (index == tabIdx)
        {
          component.preferredHeight = this.SelectedHeight;
          component.transform.Find("Icon").GetComponent<Image>().color = new Color(0.145098f, 0.1647059f, 0.2313726f);
        }
        else
        {
          component.preferredHeight = this.UnselectedHeight;
          component.transform.Find("Icon").GetComponent<Image>().color = new Color(0.3568628f, 0.372549f, 0.4509804f);
        }
      }
    }
  }
}
