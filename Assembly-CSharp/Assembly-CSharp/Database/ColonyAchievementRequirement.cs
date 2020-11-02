// Decompiled with JetBrains decompiler
// Type: Database.ColonyAchievementRequirement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.IO;

namespace Database
{
  public abstract class ColonyAchievementRequirement
  {
    public virtual void Update()
    {
    }

    public abstract bool Success();

    public virtual bool Fail() => false;

    public abstract void Serialize(BinaryWriter writer);

    public abstract void Deserialize(IReader reader);

    public virtual string GetProgress(bool complete) => "";
  }
}
