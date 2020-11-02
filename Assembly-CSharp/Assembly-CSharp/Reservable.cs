// Decompiled with JetBrains decompiler
// Type: Reservable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Reservable")]
public class Reservable : KMonoBehaviour
{
  private GameObject reservedBy;

  public GameObject ReservedBy => this.reservedBy;

  public bool isReserved => !((Object) this.reservedBy == (Object) null);

  public bool Reserve(GameObject reserver)
  {
    if (!((Object) this.reservedBy == (Object) null))
      return false;
    this.reservedBy = reserver;
    return true;
  }

  public void ClearReservation(GameObject reserver)
  {
    if (!((Object) this.reservedBy == (Object) reserver))
      return;
    this.reservedBy = (GameObject) null;
  }
}
