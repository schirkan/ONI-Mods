// Decompiled with JetBrains decompiler
// Type: ResourceSet`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Reflection;

[Serializable]
public class ResourceSet<T> : ResourceSet where T : Resource
{
  public List<T> resources = new List<T>();

  public T this[int idx] => this.resources[idx];

  public override int Count => this.resources.Count;

  public override Resource GetResource(int idx) => (Resource) this.resources[idx];

  public ResourceSet()
  {
  }

  public ResourceSet(string id, ResourceSet parent)
    : base(id, parent)
  {
  }

  public override void Initialize()
  {
    foreach (T resource in this.resources)
      resource.Initialize();
  }

  public bool Exists(string id)
  {
    foreach (T resource in this.resources)
    {
      if (resource.Id == id)
        return true;
    }
    return false;
  }

  public T TryGet(string id)
  {
    foreach (T resource in this.resources)
    {
      if (resource.Id == id)
        return resource;
    }
    return default (T);
  }

  public T Get(HashedString id)
  {
    foreach (T resource in this.resources)
    {
      if (new HashedString(resource.Id) == id)
        return resource;
    }
    Debug.LogError((object) ("Could not find " + typeof (T).ToString() + ": " + (object) id));
    return default (T);
  }

  public T Get(string id)
  {
    foreach (T resource in this.resources)
    {
      if (resource.Id == id)
        return resource;
    }
    Debug.LogError((object) ("Could not find " + typeof (T).ToString() + ": " + id));
    return default (T);
  }

  public override Resource Add(Resource resource)
  {
    if (!(resource is T resource1))
      Debug.LogError((object) ("Resource type mismatch: " + resource.GetType().Name + " does not match " + typeof (T).Name));
    this.Add(resource1);
    return resource;
  }

  public T Add(T resource)
  {
    if ((object) resource == null)
    {
      Debug.LogError((object) "Tried to add a null to the resource set");
      return default (T);
    }
    this.resources.Add(resource);
    return resource;
  }

  public void ResolveReferences()
  {
    foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
    {
      if (field.FieldType.IsSubclassOf(typeof (Resource)) && field.GetValue((object) this) == null)
      {
        Resource resource = (Resource) this.Get(field.Name);
        if (resource != null)
          field.SetValue((object) this, (object) resource);
      }
    }
  }
}
