// Decompiled with JetBrains decompiler
// Type: Database.CritterAttributes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;

namespace Database
{
  public class CritterAttributes : ResourceSet<Attribute>
  {
    public Attribute Happiness;
    public Attribute Metabolism;

    public CritterAttributes(ResourceSet parent)
      : base(nameof (CritterAttributes), parent)
    {
      this.Happiness = this.Add(new Attribute(nameof (Happiness), false, Attribute.Display.General, false));
      this.Metabolism = this.Add(new Attribute(nameof (Metabolism), false, Attribute.Display.Details, false));
      this.Metabolism.SetFormatter((IAttributeFormatter) new ToPercentAttributeFormatter(100f));
    }
  }
}
