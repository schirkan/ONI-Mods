// Decompiled with JetBrains decompiler
// Type: PreventFOWRevealTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PreventFOWRevealTracker")]
public class PreventFOWRevealTracker : KMonoBehaviour
{
  [Serialize]
  public List<int> preventFOWRevealCells;

  [OnSerializing]
  private void OnSerialize()
  {
    this.preventFOWRevealCells.Clear();
    for (int i = 0; i < Grid.VisMasks.Length; ++i)
    {
      if (Grid.PreventFogOfWarReveal[i])
        this.preventFOWRevealCells.Add(i);
    }
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    foreach (int preventFowRevealCell in this.preventFOWRevealCells)
      Grid.PreventFogOfWarReveal[preventFowRevealCell] = true;
  }
}
