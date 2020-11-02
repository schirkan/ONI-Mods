// Decompiled with JetBrains decompiler
// Type: ArtifactTier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class ArtifactTier
{
  public EffectorValues decorValues;
  public StringKey name_key;

  public ArtifactTier(StringKey str_key, EffectorValues values)
  {
    this.decorValues = values;
    this.name_key = str_key;
  }
}
