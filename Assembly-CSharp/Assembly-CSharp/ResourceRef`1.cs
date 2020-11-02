// Decompiled with JetBrains decompiler
// Type: ResourceRef`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceRef<ResourceType> : ISaveLoadable where ResourceType : Resource
{
  [Serialize]
  private ResourceGuid guid;
  private ResourceType resource;

  public ResourceRef(ResourceType resource) => this.Set(resource);

  public ResourceRef()
  {
  }

  public ResourceType Get() => this.resource;

  public void Set(ResourceType resource) => this.resource = resource;

  [System.Runtime.Serialization.OnSerializing]
  private void OnSerializing()
  {
    if ((object) this.resource == null)
      this.guid = (ResourceGuid) null;
    else
      this.guid = this.resource.Guid;
  }

  [System.Runtime.Serialization.OnDeserialized]
  private void OnDeserialized()
  {
    if (!(this.guid != (ResourceGuid) null))
      return;
    this.resource = Db.Get().GetResource<ResourceType>(this.guid);
    this.guid = (ResourceGuid) null;
  }
}
