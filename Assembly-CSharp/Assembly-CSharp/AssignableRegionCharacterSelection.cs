// Decompiled with JetBrains decompiler
// Type: AssignableRegionCharacterSelection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AssignableRegionCharacterSelection")]
public class AssignableRegionCharacterSelection : KMonoBehaviour
{
  [SerializeField]
  private KButton buttonPrefab;
  [SerializeField]
  private GameObject buttonParent;
  private UIPool<KButton> buttonPool;
  private Dictionary<KButton, MinionIdentity> buttonIdentityMap = new Dictionary<KButton, MinionIdentity>();
  private List<CrewPortrait> portraitList = new List<CrewPortrait>();

  public event System.Action<MinionIdentity> OnDuplicantSelected;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.buttonPool = new UIPool<KButton>(this.buttonPrefab);
    this.gameObject.SetActive(false);
  }

  public void Open()
  {
    this.gameObject.SetActive(true);
    this.buttonPool.ClearAll();
    foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
    {
      KButton btn = this.buttonPool.GetFreeElement(this.buttonParent, true);
      CrewPortrait componentInChildren = btn.GetComponentInChildren<CrewPortrait>();
      componentInChildren.SetIdentityObject((IAssignableIdentity) minionIdentity);
      this.portraitList.Add(componentInChildren);
      btn.ClearOnClick();
      btn.onClick += (System.Action) (() => this.SelectDuplicant(btn));
      this.buttonIdentityMap.Add(btn, minionIdentity);
    }
  }

  public void Close()
  {
    this.buttonPool.DestroyAllActive();
    this.buttonIdentityMap.Clear();
    this.portraitList.Clear();
    this.gameObject.SetActive(false);
  }

  private void SelectDuplicant(KButton btn)
  {
    if (this.OnDuplicantSelected != null)
      this.OnDuplicantSelected(this.buttonIdentityMap[btn]);
    this.Close();
  }
}
