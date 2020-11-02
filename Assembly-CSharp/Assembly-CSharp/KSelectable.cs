// Decompiled with JetBrains decompiler
// Type: KSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KSelectable")]
public class KSelectable : KMonoBehaviour
{
  private const float hoverHighlight = 0.25f;
  private const float selectHighlight = 0.2f;
  public string entityName;
  public string entityGender;
  private bool selected;
  [SerializeField]
  private bool selectable = true;
  [SerializeField]
  private bool disableSelectMarker;
  private StatusItemGroup statusItemGroup;

  public bool IsSelected => this.selected;

  public bool IsSelectable
  {
    get => this.selectable && this.isActiveAndEnabled;
    set => this.selectable = value;
  }

  public bool DisableSelectMarker => this.disableSelectMarker;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.statusItemGroup = new StatusItemGroup(this.gameObject);
    int num = (UnityEngine.Object) this.GetComponent<KPrefabID>() != (UnityEngine.Object) null ? 1 : 0;
    if (this.entityName == null || this.entityName.Length <= 0)
      this.SetName(this.name);
    if (this.entityGender != null)
      return;
    this.entityGender = "NB";
  }

  public virtual string GetName()
  {
    if (this.entityName != null && !(this.entityName == "") && this.entityName.Length > 0)
      return this.entityName;
    Debug.Log((object) "Warning Item has blank name!", (UnityEngine.Object) this.gameObject);
    return this.name;
  }

  public void SetStatusIndicatorOffset(Vector3 offset)
  {
    if (this.statusItemGroup == null)
      return;
    this.statusItemGroup.SetOffset(offset);
  }

  public void SetName(string name) => this.entityName = name;

  public void SetGender(string Gender) => this.entityGender = Gender;

  public float GetZoom()
  {
    Bounds bounds = Util.GetBounds(this.gameObject);
    return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
  }

  public Vector3 GetPortraitLocation() => Util.GetBounds(this.gameObject).center;

  private void ClearHighlight()
  {
    this.Trigger(-1201923725, (object) false);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.HighlightColour = (Color32) new Color(0.0f, 0.0f, 0.0f, 0.0f);
  }

  private void ApplyHighlight(float highlight)
  {
    this.Trigger(-1201923725, (object) true);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.HighlightColour = (Color32) new Color(highlight, highlight, highlight, highlight);
  }

  public void Select()
  {
    this.selected = true;
    this.ClearHighlight();
    this.ApplyHighlight(0.2f);
    this.Trigger(-1503271301, (object) true);
    if ((UnityEngine.Object) this.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
      this.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
    if ((UnityEngine.Object) this.transform.GetComponentInParent<LoopingSounds>() != (UnityEngine.Object) null)
      this.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
    int childCount1 = this.transform.childCount;
    for (int index1 = 0; index1 < childCount1; ++index1)
    {
      int childCount2 = this.transform.GetChild(index1).childCount;
      for (int index2 = 0; index2 < childCount2; ++index2)
      {
        if ((UnityEngine.Object) this.transform.GetChild(index1).transform.GetChild(index2).GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
          this.transform.GetChild(index1).transform.GetChild(index2).GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
      }
    }
    this.UpdateWorkerSelection(this.selected);
    this.UpdateWorkableSelection(this.selected);
  }

  public void Unselect()
  {
    if (this.selected)
    {
      this.selected = false;
      this.ClearHighlight();
      this.Trigger(-1503271301, (object) false);
    }
    if ((UnityEngine.Object) this.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
      this.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
    if ((UnityEngine.Object) this.transform.GetComponentInParent<LoopingSounds>() != (UnityEngine.Object) null)
      this.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
    foreach (Transform transform in this.transform)
    {
      if ((UnityEngine.Object) transform.GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
        transform.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
    }
    this.UpdateWorkerSelection(this.selected);
    this.UpdateWorkableSelection(this.selected);
  }

  public void Hover(bool playAudio)
  {
    this.ClearHighlight();
    if (!DebugHandler.HideUI)
      this.ApplyHighlight(0.25f);
    if (!playAudio)
      return;
    this.PlayHoverSound();
  }

  private void PlayHoverSound()
  {
    if ((UnityEngine.Object) this.GetComponent<CellSelectionObject>() != (UnityEngine.Object) null)
      return;
    UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
  }

  public void Unhover()
  {
    if (this.selected)
      return;
    this.ClearHighlight();
  }

  public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null) => on ? this.AddStatusItem(status_item, data) : this.RemoveStatusItem(status_item);

  public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null) => show ? (guid != Guid.Empty ? guid : this.AddStatusItem(status_item, data)) : (guid != Guid.Empty ? this.RemoveStatusItem(guid) : guid);

  public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null) => this.statusItemGroup == null ? Guid.Empty : this.statusItemGroup.SetStatusItem(category, status_item, data);

  public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    if (guid != Guid.Empty)
      this.statusItemGroup.RemoveStatusItem(guid);
    return this.AddStatusItem(status_item, data);
  }

  public Guid AddStatusItem(StatusItem status_item, object data = null) => this.statusItemGroup == null ? Guid.Empty : this.statusItemGroup.AddStatusItem(status_item, data);

  public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    this.statusItemGroup.RemoveStatusItem(status_item, immediate);
    return Guid.Empty;
  }

  public Guid RemoveStatusItem(Guid guid, bool immediate = false)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    this.statusItemGroup.RemoveStatusItem(guid, immediate);
    return Guid.Empty;
  }

  public bool HasStatusItem(StatusItem status_item) => this.statusItemGroup != null && this.statusItemGroup.HasStatusItem(status_item);

  public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category) => this.statusItemGroup.GetStatusItem(category);

  public StatusItemGroup GetStatusItemGroup() => this.statusItemGroup;

  public void UpdateWorkerSelection(bool selected)
  {
    Workable[] components = this.GetComponents<Workable>();
    if (components.Length == 0)
      return;
    for (int index = 0; index < components.Length; ++index)
    {
      if ((UnityEngine.Object) components[index].worker != (UnityEngine.Object) null && (UnityEngine.Object) components[index].GetComponent<LoopingSounds>() != (UnityEngine.Object) null)
        components[index].GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
    }
  }

  public void UpdateWorkableSelection(bool selected)
  {
    Worker component = this.GetComponent<Worker>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.workable != (UnityEngine.Object) null))
      return;
    Workable workable = this.GetComponent<Worker>().workable;
    if (!((UnityEngine.Object) workable.GetComponent<LoopingSounds>() != (UnityEngine.Object) null))
      return;
    workable.GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
  }

  protected override void OnLoadLevel()
  {
    this.OnCleanUp();
    base.OnLoadLevel();
  }

  protected override void OnCleanUp()
  {
    if (this.statusItemGroup != null)
    {
      this.statusItemGroup.Destroy();
      this.statusItemGroup = (StatusItemGroup) null;
    }
    if (this.selected && (UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this)
        SelectTool.Instance.Select((KSelectable) null, true);
      else
        this.Unselect();
    }
    base.OnCleanUp();
  }
}
