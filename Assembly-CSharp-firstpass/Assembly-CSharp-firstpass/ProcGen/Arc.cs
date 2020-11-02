// Decompiled with JetBrains decompiler
// Type: ProcGen.Arc
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Arc
  {
    [Serialize]
    public string type = "";
    [Serialize]
    public TagSet tags;

    public Satsuma.Arc arc { get; private set; }

    public Arc()
    {
    }

    public Arc(string type) => this.type = type;

    public Arc(Satsuma.Arc arc, string type)
    {
      this.arc = arc;
      this.type = type;
    }
  }
}
