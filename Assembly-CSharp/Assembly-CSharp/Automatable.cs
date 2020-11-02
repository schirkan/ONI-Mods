// Decompiled with JetBrains decompiler
// Type: Automatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Automatable")]
public class Automatable : KMonoBehaviour
{
  [Serialize]
  private bool automationOnly = true;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  private static readonly EventSystem.IntraObjectHandler<Automatable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Automatable>((System.Action<Automatable, object>) ((component, data) => component.OnCopySettings(data)));

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Automatable>(-905833192, Automatable.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    Automatable component = ((GameObject) data).GetComponent<Automatable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.automationOnly = component.automationOnly;
  }

  public bool GetAutomationOnly() => this.automationOnly;

  public void SetAutomationOnly(bool only) => this.automationOnly = only;

  public bool AllowedByAutomation(bool is_transfer_arm) => !this.GetAutomationOnly() | is_transfer_arm;
}
