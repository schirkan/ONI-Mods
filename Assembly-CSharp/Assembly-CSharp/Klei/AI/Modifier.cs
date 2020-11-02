// Decompiled with JetBrains decompiler
// Type: Klei.AI.Modifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI
{
  public class Modifier : Resource
  {
    public string description;
    public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();

    public Modifier(string id, string name, string description)
      : base(id, name)
      => this.description = description;

    public void Add(AttributeModifier modifier)
    {
      if (!(modifier.AttributeId != ""))
        return;
      this.SelfModifiers.Add(modifier);
    }

    public virtual void AddTo(Attributes attributes)
    {
      foreach (AttributeModifier selfModifier in this.SelfModifiers)
        attributes.Add(selfModifier);
    }

    public virtual void RemoveFrom(Attributes attributes)
    {
      foreach (AttributeModifier selfModifier in this.SelfModifiers)
        attributes.Remove(selfModifier);
    }
  }
}
