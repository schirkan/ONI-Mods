// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.USER_PROPERTY
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace FMOD.Studio
{
  public struct USER_PROPERTY
  {
    public StringWrapper name;
    public USER_PROPERTY_TYPE type;
    private Union_IntBoolFloatString value;

    public int intValue() => this.type != USER_PROPERTY_TYPE.INTEGER ? -1 : this.value.intvalue;

    public bool boolValue() => this.type == USER_PROPERTY_TYPE.BOOLEAN && this.value.boolvalue;

    public float floatValue() => this.type != USER_PROPERTY_TYPE.FLOAT ? -1f : this.value.floatvalue;

    public string stringValue() => this.type != USER_PROPERTY_TYPE.STRING ? "" : (string) this.value.stringvalue;
  }
}
