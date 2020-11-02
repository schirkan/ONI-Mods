// Decompiled with JetBrains decompiler
// Type: Resource
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Diagnostics;

[DebuggerDisplay("{IdHash}")]
public class Resource
{
  public string Name;
  public string Id;
  public HashedString IdHash;
  public bool Disabled;

  public ResourceGuid Guid { get; private set; }

  public Resource()
  {
  }

  public Resource(string id, ResourceSet parent = null, string name = null)
  {
    Debug.Assert(id != null);
    this.Id = id;
    this.IdHash = new HashedString(this.Id);
    this.Guid = new ResourceGuid(id, (Resource) parent);
    parent?.Add(this);
    if (name != null)
      this.Name = name;
    else
      this.Name = id;
  }

  public Resource(string id, string name)
  {
    Debug.Assert(id != null);
    this.Guid = new ResourceGuid(id);
    this.Id = id;
    this.IdHash = new HashedString(this.Id);
    this.Name = name;
  }

  public virtual void Initialize()
  {
  }
}
