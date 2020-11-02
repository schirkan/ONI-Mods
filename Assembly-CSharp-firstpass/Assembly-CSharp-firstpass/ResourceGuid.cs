// Decompiled with JetBrains decompiler
// Type: ResourceGuid
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;
using System;

[SerializationConfig(MemberSerialization.OptIn)]
public class ResourceGuid : IEquatable<ResourceGuid>, ISaveLoadable
{
  [Serialize]
  public string Guid;

  public ResourceGuid(string id, Resource parent = null)
  {
    if (parent != null)
      this.Guid = parent.Guid.Guid + "." + id;
    else
      this.Guid = id;
  }

  public override int GetHashCode() => this.Guid.GetHashCode();

  public override bool Equals(object obj)
  {
    ResourceGuid resourceGuid = (ResourceGuid) obj;
    return obj != null && this.Guid == resourceGuid.Guid;
  }

  public bool Equals(ResourceGuid other) => this.Guid == other.Guid;

  public static bool operator ==(ResourceGuid a, ResourceGuid b)
  {
    if ((object) a == (object) b)
      return true;
    return (object) a != null && (object) b != null && a.Guid == b.Guid;
  }

  public static bool operator !=(ResourceGuid a, ResourceGuid b)
  {
    if ((object) a == (object) b)
      return false;
    return (object) a == null || (object) b == null || a.Guid != b.Guid;
  }

  public override string ToString() => this.Guid;
}
