﻿// Decompiled with JetBrains decompiler
// Type: Database.AccessorySlots
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Database
{
  public class AccessorySlots : ResourceSet<AccessorySlot>
  {
    public AccessorySlot Eyes;
    public AccessorySlot Hair;
    public AccessorySlot HeadShape;
    public AccessorySlot Mouth;
    public AccessorySlot Body;
    public AccessorySlot Arm;
    public AccessorySlot Hat;
    public AccessorySlot HatHair;
    public AccessorySlot HairAlways;

    public AccessorySlots(
      ResourceSet parent,
      KAnimFile default_build = null,
      KAnimFile swap_build = null,
      KAnimFile torso_swap_build = null)
      : base(nameof (AccessorySlots), parent)
    {
      if ((Object) swap_build == (Object) null)
      {
        swap_build = Assets.GetAnim((HashedString) "head_swap_kanim");
        parent = (ResourceSet) Db.Get().Accessories;
      }
      if ((Object) default_build == (Object) null)
        default_build = Assets.GetAnim((HashedString) "body_comp_default_kanim");
      if ((Object) torso_swap_build == (Object) null)
        torso_swap_build = Assets.GetAnim((HashedString) "body_swap_kanim");
      this.Eyes = new AccessorySlot(nameof (Eyes), (ResourceSet) this, swap_build);
      this.Hair = new AccessorySlot(nameof (Hair), (ResourceSet) this, swap_build);
      this.HeadShape = new AccessorySlot(nameof (HeadShape), (ResourceSet) this, swap_build);
      this.Mouth = new AccessorySlot(nameof (Mouth), (ResourceSet) this, swap_build);
      this.Hat = new AccessorySlot(nameof (Hat), (ResourceSet) this, swap_build);
      this.HatHair = new AccessorySlot("Hat_Hair", (ResourceSet) this, swap_build);
      this.HairAlways = new AccessorySlot("Hair_Always", (ResourceSet) this, swap_build, "hair");
      this.Body = new AccessorySlot(nameof (Body), (ResourceSet) this, torso_swap_build);
      this.Arm = new AccessorySlot(nameof (Arm), (ResourceSet) this, torso_swap_build);
      foreach (AccessorySlot resource in this.resources)
        resource.AddAccessories(default_build, parent);
    }
  }
}
