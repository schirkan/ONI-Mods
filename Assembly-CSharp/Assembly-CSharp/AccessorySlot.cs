// Decompiled with JetBrains decompiler
// Type: AccessorySlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class AccessorySlot : Resource
{
  private KAnimFile file;

  public KAnimHashedString targetSymbolId { get; private set; }

  public List<Accessory> accessories { get; private set; }

  public AccessorySlot(
    string id,
    ResourceSet parent,
    KAnimFile swap_build,
    string build_symbol_override = null)
    : base(id, parent)
  {
    if ((UnityEngine.Object) swap_build == (UnityEngine.Object) null)
      Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", (object) id);
    this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
    this.accessories = new List<Accessory>();
    this.file = swap_build;
  }

  public void AddAccessories(KAnimFile default_build, ResourceSet parent)
  {
    KAnim.Build build = this.file.GetData().build;
    default_build.GetData().build.GetSymbol(this.targetSymbolId);
    string lower = this.Id.ToLower();
    for (int index = 0; index < build.symbols.Length; ++index)
    {
      string id = HashCache.Get().Get(build.symbols[index].hash);
      if (id.StartsWith(lower))
      {
        Accessory accessory = new Accessory(id, parent, this, this.file.batchTag, build.symbols[index]);
        this.accessories.Add(accessory);
        HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
      }
    }
  }

  public Accessory Lookup(string id) => this.Lookup(new HashedString(id));

  public Accessory Lookup(HashedString full_id) => this.accessories.Find((Predicate<Accessory>) (a => a.IdHash == full_id));
}
