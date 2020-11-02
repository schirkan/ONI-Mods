// Decompiled with JetBrains decompiler
// Type: Accessory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Accessory : Resource
{
  public KAnim.Build.Symbol symbol { get; private set; }

  public HashedString batchSource { get; private set; }

  public AccessorySlot slot { get; private set; }

  public Accessory(
    string id,
    ResourceSet parent,
    AccessorySlot slot,
    HashedString batchSource,
    KAnim.Build.Symbol symbol)
    : base(id, parent)
  {
    this.slot = slot;
    this.symbol = symbol;
    this.batchSource = batchSource;
  }
}
