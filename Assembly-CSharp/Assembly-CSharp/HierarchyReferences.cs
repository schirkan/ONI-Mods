// Decompiled with JetBrains decompiler
// Type: HierarchyReferences
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/HierarchyReferences")]
public class HierarchyReferences : KMonoBehaviour
{
  public ElementReference[] references;

  public bool HasReference(string name)
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
        return true;
    }
    return false;
  }

  public SpecifiedType GetReference<SpecifiedType>(string name) where SpecifiedType : Component
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
      {
        if (reference.behaviour is SpecifiedType)
          return (SpecifiedType) reference.behaviour;
        Debug.LogError((object) string.Format("Behavior is not specified type", (object[]) Array.Empty<object>()));
      }
    }
    Debug.LogError((object) string.Format("Could not find UI reference '{0}' or convert to specified type)", (object) name));
    return default (SpecifiedType);
  }

  public Component GetReference(string name)
  {
    foreach (ElementReference reference in this.references)
    {
      if (reference.Name == name)
        return reference.behaviour;
    }
    Debug.LogWarning((object) "Couldn't find reference to object named {0} Make sure the name matches the field in the inspector.");
    return (Component) null;
  }
}
