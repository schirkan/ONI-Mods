// Decompiled with JetBrains decompiler
// Type: LogicBitSelectorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LogicBitSelectorSideScreen : SideScreenContent, IRenderEveryTick
{
  private ILogicRibbonBitSelector target;
  public GameObject rowPrefab;
  public KImage inputDisplayIcon;
  public KImage outputDisplayIcon;
  public GameObject readerDescriptionContainer;
  public GameObject writerDescriptionContainer;
  [NonSerialized]
  public Dictionary<int, MultiToggle> toggles_by_int = new Dictionary<int, MultiToggle>();
  private Color activeColor;
  private Color inactiveColor;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.activeColor = (Color) GlobalAssets.Instance.colorSet.logicOnText;
    this.inactiveColor = (Color) GlobalAssets.Instance.colorSet.logicOffText;
  }

  public void SelectToggle(int bit)
  {
    this.target.SetBitSelection(bit);
    this.target.UpdateVisuals();
    this.RefreshToggles();
  }

  private void RefreshToggles()
  {
    for (int index = 0; index < this.target.GetBitDepth(); ++index)
    {
      int n = index;
      if (!this.toggles_by_int.ContainsKey(index))
      {
        GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowPrefab.transform.parent.gameObject, true);
        gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("bitName").SetText(string.Format((string) UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.BIT, (object) (index + 1)));
        gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = this.target.IsBitActive(index) ? this.activeColor : this.inactiveColor;
        gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText((string) (this.target.IsBitActive(index) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE));
        MultiToggle component = gameObject.GetComponent<MultiToggle>();
        this.toggles_by_int.Add(index, component);
      }
      this.toggles_by_int[index].onClick = (System.Action) (() => this.SelectToggle(n));
    }
    foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
    {
      if (this.target.GetBitSelection() == keyValuePair.Key)
        keyValuePair.Value.ChangeState(0);
      else
        keyValuePair.Value.ChangeState(1);
    }
  }

  public override bool IsValidForTarget(GameObject target) => target.GetComponent<ILogicRibbonBitSelector>() != null;

  public override void SetTarget(GameObject new_target)
  {
    if ((UnityEngine.Object) new_target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Invalid gameObject received");
    }
    else
    {
      this.target = new_target.GetComponent<ILogicRibbonBitSelector>();
      if (this.target == null)
      {
        Debug.LogError((object) "The gameObject received is not an ILogicRibbonBitSelector");
      }
      else
      {
        this.titleKey = this.target.SideScreenTitle;
        this.readerDescriptionContainer.SetActive(this.target.SideScreenDisplayReaderDescription());
        this.writerDescriptionContainer.SetActive(this.target.SideScreenDisplayWriterDescription());
        this.RefreshToggles();
        this.UpdateInputOutputDisplay();
        foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
          this.UpdateStateVisuals(keyValuePair.Key);
      }
    }
  }

  public void RenderEveryTick(float dt)
  {
    if (this.target.Equals((object) null))
      return;
    foreach (KeyValuePair<int, MultiToggle> keyValuePair in this.toggles_by_int)
      this.UpdateStateVisuals(keyValuePair.Key);
    this.UpdateInputOutputDisplay();
  }

  private void UpdateInputOutputDisplay()
  {
    if (this.target.SideScreenDisplayReaderDescription())
      this.outputDisplayIcon.color = this.target.GetOutputValue() > 0 ? this.activeColor : this.inactiveColor;
    if (!this.target.SideScreenDisplayWriterDescription())
      return;
    this.inputDisplayIcon.color = this.target.GetInputValue() > 0 ? this.activeColor : this.inactiveColor;
  }

  private void UpdateStateVisuals(int bit)
  {
    MultiToggle multiToggle = this.toggles_by_int[bit];
    multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<KImage>("stateIcon").color = this.target.IsBitActive(bit) ? this.activeColor : this.inactiveColor;
    multiToggle.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("stateText").SetText((string) (this.target.IsBitActive(bit) ? UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_ACTIVE : UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.STATE_INACTIVE));
  }
}
