// Decompiled with JetBrains decompiler
// Type: GasSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class GasSource : SubstanceSource
{
  protected override CellOffset[] GetOffsetGroup() => OffsetGroups.LiquidSource;

  protected override IChunkManager GetChunkManager() => (IChunkManager) GasSourceManager.Instance;

  protected override void OnCleanUp() => base.OnCleanUp();
}
