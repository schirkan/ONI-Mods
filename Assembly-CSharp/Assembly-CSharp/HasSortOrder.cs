// Decompiled with JetBrains decompiler
// Type: HasSortOrder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/HasSortOrder")]
public class HasSortOrder : KMonoBehaviour, IHasSortOrder
{
  public int sortOrder { get; set; }
}
