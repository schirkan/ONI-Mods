// Decompiled with JetBrains decompiler
// Type: IAssignableIdentity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public interface IAssignableIdentity
{
  string GetProperName();

  List<Ownables> GetOwners();

  Ownables GetSoleOwner();

  bool IsNull();
}
