// Decompiled with JetBrains decompiler
// Type: CrewPortrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/CrewPortrait")]
[Serializable]
public class CrewPortrait : KMonoBehaviour
{
  public Image targetImage;
  public bool startTransparent;
  public bool useLabels = true;
  [SerializeField]
  public KBatchedAnimController controller;
  public float animScaleBase = 0.2f;
  public LocText duplicantName;
  public LocText duplicantJob;
  public LocText subTitle;
  public bool useDefaultExpression = true;
  private bool requiresRefresh;
  private bool areEventsRegistered;

  public IAssignableIdentity identityObject { get; private set; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.startTransparent)
      this.StartCoroutine(this.AlphaIn());
    this.requiresRefresh = true;
    ScreenResize.Instance.OnResize += new System.Action(this.RefreshScale);
  }

  private IEnumerator AlphaIn()
  {
    this.SetAlpha(0.0f);
    for (float i = 0.0f; (double) i < 1.0; i += Time.unscaledDeltaTime * 4f)
    {
      this.SetAlpha(i);
      yield return (object) 0;
    }
    this.SetAlpha(1f);
  }

  private void OnRoleChanged(object data)
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
      return;
    CrewPortrait.RefreshHat(this.identityObject, this.controller);
  }

  private void RegisterEvents()
  {
    if (this.areEventsRegistered)
      return;
    KMonoBehaviour identityObject = this.identityObject as KMonoBehaviour;
    if ((UnityEngine.Object) identityObject == (UnityEngine.Object) null)
      return;
    identityObject.Subscribe(540773776, new System.Action<object>(this.OnRoleChanged));
    this.areEventsRegistered = true;
  }

  private void UnregisterEvents()
  {
    if (!this.areEventsRegistered)
      return;
    this.areEventsRegistered = false;
    KMonoBehaviour identityObject = this.identityObject as KMonoBehaviour;
    if ((UnityEngine.Object) identityObject == (UnityEngine.Object) null)
      return;
    identityObject.Unsubscribe(540773776, new System.Action<object>(this.OnRoleChanged));
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    this.RegisterEvents();
    this.ForceRefresh();
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.UnregisterEvents();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.UnregisterEvents();
    ScreenResize.Instance.OnResize -= new System.Action(this.RefreshScale);
  }

  public void SetIdentityObject(IAssignableIdentity identity, bool jobEnabled = true)
  {
    this.UnregisterEvents();
    this.identityObject = identity;
    this.RegisterEvents();
    this.targetImage.enabled = true;
    if (this.identityObject != null)
      this.targetImage.enabled = false;
    if (this.useLabels)
    {
      switch (identity)
      {
        case MinionIdentity _:
        case MinionAssignablesProxy _:
          this.SetDuplicantJobTitleActive(jobEnabled);
          break;
      }
    }
    this.requiresRefresh = true;
  }

  public void SetSubTitle(string newTitle)
  {
    if (!((UnityEngine.Object) this.subTitle != (UnityEngine.Object) null))
      return;
    if (string.IsNullOrEmpty(newTitle))
    {
      this.subTitle.gameObject.SetActive(false);
    }
    else
    {
      this.subTitle.gameObject.SetActive(true);
      this.subTitle.SetText(newTitle);
    }
  }

  public void SetDuplicantJobTitleActive(bool state)
  {
    if (!((UnityEngine.Object) this.duplicantJob != (UnityEngine.Object) null) || this.duplicantJob.gameObject.activeInHierarchy == state)
      return;
    this.duplicantJob.gameObject.SetActive(state);
  }

  public void ForceRefresh() => this.requiresRefresh = true;

  public void Update()
  {
    if (!this.requiresRefresh || !((UnityEngine.Object) this.controller == (UnityEngine.Object) null) && !this.controller.enabled)
      return;
    this.requiresRefresh = false;
    this.Rebuild();
    this.RefreshScale();
  }

  private void RefreshScale()
  {
    float num = 1f;
    if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) GameScreenManager.Instance.ssOverlayCanvas != (UnityEngine.Object) null)
      num = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetCanvasScale();
    if (!((UnityEngine.Object) this.controller != (UnityEngine.Object) null))
      return;
    this.controller.animScale = this.animScaleBase * (1f / num);
  }

  private void Rebuild()
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
    {
      this.controller = this.GetComponentInChildren<KBatchedAnimController>();
      if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.targetImage != (UnityEngine.Object) null)
          this.targetImage.enabled = true;
        Debug.LogWarning((object) ("Controller for [" + this.name + "] null"));
        return;
      }
    }
    CrewPortrait.SetPortraitData(this.identityObject, this.controller, this.useDefaultExpression);
    if (!this.useLabels || !((UnityEngine.Object) this.duplicantName != (UnityEngine.Object) null))
      return;
    this.duplicantName.SetText(this.identityObject != null ? this.identityObject.GetProperName() : "");
    if (!(this.identityObject is MinionIdentity) || !((UnityEngine.Object) this.duplicantJob != (UnityEngine.Object) null))
      return;
    this.duplicantJob.SetText(this.identityObject != null ? (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle() : "");
    this.duplicantJob.GetComponent<ToolTip>().toolTip = (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle();
  }

  private static void RefreshHat(
    IAssignableIdentity identityObject,
    KBatchedAnimController controller)
  {
    string hat_id = "";
    MinionIdentity minionIdentity = identityObject as MinionIdentity;
    if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
      hat_id = minionIdentity.GetComponent<MinionResume>().CurrentHat;
    else if ((UnityEngine.Object) (identityObject as StoredMinionIdentity) != (UnityEngine.Object) null)
      hat_id = (identityObject as StoredMinionIdentity).currentHat;
    MinionResume.ApplyHat(hat_id, controller);
  }

  public static void SetPortraitData(
    IAssignableIdentity identityObject,
    KBatchedAnimController controller,
    bool useDefaultExpression = true)
  {
    if (identityObject == null)
    {
      controller.gameObject.SetActive(false);
    }
    else
    {
      MinionIdentity minionIdentity = identityObject as MinionIdentity;
      if ((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null)
      {
        MinionAssignablesProxy assignablesProxy = identityObject as MinionAssignablesProxy;
        if ((UnityEngine.Object) assignablesProxy != (UnityEngine.Object) null && assignablesProxy.target != null)
          minionIdentity = assignablesProxy.target as MinionIdentity;
      }
      controller.gameObject.SetActive(true);
      controller.Play((HashedString) "ui_idle");
      SymbolOverrideController component1 = controller.GetComponent<SymbolOverrideController>();
      component1.RemoveAllSymbolOverrides();
      if ((UnityEngine.Object) minionIdentity != (UnityEngine.Object) null)
      {
        Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
        foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
        {
          Accessory accessory = component2.GetAccessory(resource);
          if (accessory != null)
          {
            component1.AddSymbolOverride((HashedString) resource.targetSymbolId, accessory.symbol);
            controller.SetSymbolVisiblity(resource.targetSymbolId, true);
          }
        }
        component1.AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
        CrewPortrait.RefreshHat((IAssignableIdentity) minionIdentity, controller);
      }
      else
      {
        StoredMinionIdentity storedMinionIdentity = identityObject as StoredMinionIdentity;
        if ((UnityEngine.Object) storedMinionIdentity == (UnityEngine.Object) null)
        {
          MinionAssignablesProxy assignablesProxy = identityObject as MinionAssignablesProxy;
          if ((UnityEngine.Object) assignablesProxy != (UnityEngine.Object) null && assignablesProxy.target != null)
            storedMinionIdentity = assignablesProxy.target as StoredMinionIdentity;
        }
        if ((UnityEngine.Object) storedMinionIdentity != (UnityEngine.Object) null)
        {
          foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
          {
            Accessory accessory = storedMinionIdentity.GetAccessory(resource);
            if (accessory != null)
            {
              component1.AddSymbolOverride((HashedString) resource.targetSymbolId, accessory.symbol);
              controller.SetSymbolVisiblity(resource.targetSymbolId, true);
            }
          }
          component1.AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
          CrewPortrait.RefreshHat((IAssignableIdentity) storedMinionIdentity, controller);
        }
        else
        {
          controller.gameObject.SetActive(false);
          return;
        }
      }
      float num = 1f;
      if ((UnityEngine.Object) GameScreenManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) GameScreenManager.Instance.ssOverlayCanvas != (UnityEngine.Object) null)
        num = (float) (0.200000002980232 * (1.0 / (double) GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>().GetUserScale()));
      controller.animScale = num;
      string str = "ui";
      controller.Play((HashedString) str, KAnim.PlayMode.Loop);
      controller.SetSymbolVisiblity((KAnimHashedString) "snapTo_neck", false);
      controller.SetSymbolVisiblity((KAnimHashedString) "snapTo_goggles", false);
    }
  }

  public void SetAlpha(float value)
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null || (double) this.controller.TintColour.a == (double) value)
      return;
    this.controller.TintColour = (Color32) new Color(1f, 1f, 1f, value);
  }
}
