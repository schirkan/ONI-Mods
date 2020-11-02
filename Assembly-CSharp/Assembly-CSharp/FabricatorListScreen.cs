// Decompiled with JetBrains decompiler
// Type: FabricatorListScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class FabricatorListScreen : KToggleMenu
{
  private void Refresh()
  {
    List<KToggleMenu.ToggleInfo> toggleInfoList = new List<KToggleMenu.ToggleInfo>();
    foreach (Fabricator fabricator in Components.Fabricators.Items)
    {
      KSelectable component = fabricator.GetComponent<KSelectable>();
      toggleInfoList.Add(new KToggleMenu.ToggleInfo(component.GetName(), (object) fabricator));
    }
    this.Setup((IList<KToggleMenu.ToggleInfo>) toggleInfoList);
  }

  protected override void OnSpawn() => this.onSelect += new KToggleMenu.OnSelect(this.OnClickFabricator);

  protected override void OnActivate()
  {
    base.OnActivate();
    this.Refresh();
  }

  private void OnClickFabricator(KToggleMenu.ToggleInfo toggle_info)
  {
    Fabricator userData = (Fabricator) toggle_info.userData;
    SelectTool.Instance.Select(userData.GetComponent<KSelectable>());
  }
}
