// Decompiled with JetBrains decompiler
// Type: SubstanceChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SubstanceChunk")]
public class SubstanceChunk : KMonoBehaviour, ISaveLoadable
{
  private static readonly KAnimHashedString symbolToTint = new KAnimHashedString("substance_tinter");

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Color colour = (Color) this.GetComponent<PrimaryElement>().Element.substance.colour;
    colour.a = 1f;
    this.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint, colour);
  }

  private void OnRefreshUserMenu(object data) => Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELEASEELEMENT.NAME, new System.Action(this.OnRelease), tooltipText: ((string) UI.USERMENUACTIONS.RELEASEELEMENT.TOOLTIP)));

  private void OnRelease()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass > 0.0)
      SimMessages.AddRemoveSubstance(cell, component.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount);
    this.gameObject.DeleteObject();
  }
}
