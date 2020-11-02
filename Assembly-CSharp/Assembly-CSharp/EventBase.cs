// Decompiled with JetBrains decompiler
// Type: EventBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class EventBase : Resource
{
  public int hash;

  public EventBase(string id)
    : base(id, id)
    => this.hash = Hash.SDBMLower(id);

  public virtual string GetDescription(EventInstanceBase ev) => "";
}
