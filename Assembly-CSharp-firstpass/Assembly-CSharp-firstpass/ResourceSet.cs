// Decompiled with JetBrains decompiler
// Type: ResourceSet
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public abstract class ResourceSet : Resource
{
  public ResourceSet()
  {
  }

  public ResourceSet(string id, ResourceSet parent)
    : base(id, parent)
  {
  }

  public abstract Resource Add(Resource resource);

  public abstract int Count { get; }

  public abstract Resource GetResource(int idx);
}
