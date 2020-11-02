// Decompiled with JetBrains decompiler
// Type: EntityConfigOrder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

internal class EntityConfigOrder : Attribute
{
  public int sortOrder;

  public EntityConfigOrder(int sort_order) => this.sortOrder = sort_order;
}
