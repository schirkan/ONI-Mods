// Decompiled with JetBrains decompiler
// Type: ModsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KMod;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ModsScreen : KModalScreen
{
  [SerializeField]
  private KButton closeButtonTitle;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton toggleAllButton;
  [SerializeField]
  private KButton workshopButton;
  [SerializeField]
  private GameObject entryPrefab;
  [SerializeField]
  private Transform entryParent;
  private List<ModsScreen.DisplayedMod> displayedMods = new List<ModsScreen.DisplayedMod>();
  private List<KMod.Label> mod_footprint = new List<KMod.Label>();

  protected override void OnActivate()
  {
    base.OnActivate();
    this.closeButtonTitle.onClick += new System.Action(this.Exit);
    this.closeButton.onClick += new System.Action(this.Exit);
    this.workshopButton.onClick += (System.Action) (() => Application.OpenURL("http://steamcommunity.com/workshop/browse/?appid=457140"));
    this.UpdateToggleAllButton();
    this.toggleAllButton.onClick += new System.Action(this.OnToggleAllClicked);
    Global.Instance.modManager.Sanitize(this.gameObject);
    this.mod_footprint.Clear();
    foreach (KMod.Mod mod in Global.Instance.modManager.mods)
    {
      if (mod.enabled)
      {
        this.mod_footprint.Add(mod.label);
        if ((mod.loaded_content & (Content.Strings | Content.DLL | Content.Translation | Content.Animation)) == (mod.available_content & (Content.Strings | Content.DLL | Content.Translation | Content.Animation)))
          mod.Uncrash();
      }
    }
    this.BuildDisplay();
    Global.Instance.modManager.on_update += new Manager.OnUpdate(this.RebuildDisplay);
  }

  protected override void OnDeactivate()
  {
    Global.Instance.modManager.on_update -= new Manager.OnUpdate(this.RebuildDisplay);
    base.OnDeactivate();
  }

  private void Exit()
  {
    Global.Instance.modManager.Save();
    if (!Global.Instance.modManager.MatchFootprint(this.mod_footprint, Content.Strings | Content.DLL | Content.Translation | Content.Animation))
      Global.Instance.modManager.RestartDialog((string) UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.TITLE, (string) UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.MESSAGE, new System.Action(((KScreen) this).Deactivate), true, this.gameObject);
    else
      this.Deactivate();
    Global.Instance.modManager.events.Clear();
  }

  private void RebuildDisplay(object change_source)
  {
    if (change_source == this)
      return;
    this.BuildDisplay();
  }

  private bool ShouldDisplayMod(KMod.Mod mod) => mod.status != KMod.Mod.Status.NotInstalled && mod.status != KMod.Mod.Status.UninstallPending && !mod.HasOnlyTranslationContent();

  private void BuildDisplay()
  {
    foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
    {
      if ((UnityEngine.Object) displayedMod.rect_transform != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) displayedMod.rect_transform.gameObject);
    }
    this.displayedMods.Clear();
    ModsScreen.ModOrderingDragListener orderingDragListener = new ModsScreen.ModOrderingDragListener(this, this.displayedMods);
    for (int index = 0; index != Global.Instance.modManager.mods.Count; ++index)
    {
      KMod.Mod mod = Global.Instance.modManager.mods[index];
      if (this.ShouldDisplayMod(mod))
      {
        HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject);
        this.displayedMods.Add(new ModsScreen.DisplayedMod()
        {
          rect_transform = hierarchyReferences.gameObject.GetComponent<RectTransform>(),
          mod_index = index
        });
        hierarchyReferences.GetComponent<DragMe>().listener = (DragMe.IDragListener) orderingDragListener;
        LocText reference1 = hierarchyReferences.GetReference<LocText>("Title");
        reference1.text = mod.title;
        hierarchyReferences.GetReference<ToolTip>("Description").toolTip = mod.description;
        if (mod.crash_count != 0)
          reference1.color = Color.Lerp(Color.white, Color.red, (float) mod.crash_count / 3f);
        KButton reference2 = hierarchyReferences.GetReference<KButton>("ManageButton");
        reference2.GetComponentInChildren<LocText>().text = (string) (mod.IsLocal ? UI.FRONTEND.MODS.MANAGE_LOCAL : UI.FRONTEND.MODS.MANAGE);
        reference2.isInteractable = mod.is_managed;
        if (reference2.isInteractable)
        {
          reference2.GetComponent<ToolTip>().toolTip = (string) mod.manage_tooltip;
          reference2.onClick += mod.on_managed;
        }
        MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
        toggle.ChangeState(mod.enabled ? 1 : 0);
        toggle.onClick += (System.Action) (() => this.OnToggleClicked(toggle, mod.label));
        toggle.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => (string) (mod.enabled ? UI.FRONTEND.MODS.TOOLTIPS.ENABLED : UI.FRONTEND.MODS.TOOLTIPS.DISABLED));
        hierarchyReferences.gameObject.SetActive(true);
      }
    }
    foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
      displayedMod.rect_transform.gameObject.SetActive(true);
    int count = this.displayedMods.Count;
  }

  private void OnToggleClicked(MultiToggle toggle, KMod.Label mod)
  {
    Manager modManager = Global.Instance.modManager;
    bool enabled = !modManager.IsModEnabled(mod);
    toggle.ChangeState(enabled ? 1 : 0);
    modManager.EnableMod(mod, enabled, (object) this);
    this.UpdateToggleAllButton();
  }

  private bool AreAnyModsDisabled() => Global.Instance.modManager.mods.Any<KMod.Mod>((Func<KMod.Mod, bool>) (mod => !mod.enabled && this.ShouldDisplayMod(mod)));

  private void UpdateToggleAllButton() => this.toggleAllButton.GetComponentInChildren<LocText>().text = (string) (this.AreAnyModsDisabled() ? UI.FRONTEND.MODS.ENABLE_ALL : UI.FRONTEND.MODS.DISABLE_ALL);

  private void OnToggleAllClicked()
  {
    bool enabled = this.AreAnyModsDisabled();
    Manager modManager = Global.Instance.modManager;
    foreach (KMod.Mod mod in modManager.mods)
    {
      if (this.ShouldDisplayMod(mod))
        modManager.EnableMod(mod.label, enabled, (object) this);
    }
    this.BuildDisplay();
    this.UpdateToggleAllButton();
  }

  private struct DisplayedMod
  {
    public RectTransform rect_transform;
    public int mod_index;
  }

  private class ModOrderingDragListener : DragMe.IDragListener
  {
    private List<ModsScreen.DisplayedMod> mods;
    private ModsScreen screen;
    private int startDragIdx = -1;

    public ModOrderingDragListener(ModsScreen screen, List<ModsScreen.DisplayedMod> mods)
    {
      this.screen = screen;
      this.mods = mods;
    }

    public void OnBeginDrag(Vector2 pos) => this.startDragIdx = this.GetDragIdx(pos);

    public void OnEndDrag(Vector2 pos)
    {
      if (this.startDragIdx < 0)
        return;
      int dragIdx = this.GetDragIdx(pos);
      Global.Instance.modManager.Reinsert(this.mods[this.startDragIdx].mod_index, dragIdx < 0 || dragIdx == this.startDragIdx ? Global.Instance.modManager.mods.Count : this.mods[dragIdx].mod_index, (object) this);
      this.screen.BuildDisplay();
    }

    private int GetDragIdx(Vector2 pos)
    {
      int num = -1;
      for (int index = 0; index < this.mods.Count; ++index)
      {
        if (RectTransformUtility.RectangleContainsScreenPoint(this.mods[index].rect_transform, pos))
        {
          num = index;
          break;
        }
      }
      return num;
    }
  }
}
