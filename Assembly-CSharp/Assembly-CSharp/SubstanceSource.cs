// Decompiled with JetBrains decompiler
// Type: SubstanceSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class SubstanceSource : KMonoBehaviour
{
  private bool enableRefresh;
  private static readonly float MaxPickupTime = 8f;
  [MyCmpReq]
  public Pickupable pickupable;
  [MyCmpReq]
  private PrimaryElement primaryElement;

  protected override void OnPrefabInit() => this.pickupable.SetWorkTime(SubstanceSource.MaxPickupTime);

  protected override void OnSpawn() => this.pickupable.SetWorkTime(10f);

  protected abstract CellOffset[] GetOffsetGroup();

  protected abstract IChunkManager GetChunkManager();

  public SimHashes GetElementID() => this.primaryElement.ElementID;

  public Tag GetElementTag()
  {
    Tag tag = Tag.Invalid;
    if ((Object) this.gameObject != (Object) null && (Object) this.primaryElement != (Object) null && this.primaryElement.Element != null)
      tag = this.primaryElement.Element.tag;
    return tag;
  }

  public Tag GetMaterialCategoryTag()
  {
    Tag tag = Tag.Invalid;
    if ((Object) this.gameObject != (Object) null && (Object) this.primaryElement != (Object) null && this.primaryElement.Element != null)
      tag = this.primaryElement.Element.GetMaterialCategoryTag();
    return tag;
  }
}
