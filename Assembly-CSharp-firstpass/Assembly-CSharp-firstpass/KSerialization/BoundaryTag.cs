// Decompiled with JetBrains decompiler
// Type: KSerialization.BoundaryTag
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace KSerialization
{
  public enum BoundaryTag : uint
  {
    DirectoryStart = 3235774464, // 0xC0DE0000
    DirectoryEnd = 3235774465, // 0xC0DE0001
    TemplateStart = 3235774467, // 0xC0DE0003
    TemplateEnd = 3235774468, // 0xC0DE0004
    FieldStart = 3235774469, // 0xC0DE0005
    FieldEnd = 3235774470, // 0xC0DE0006
    PropertyStart = 3235774471, // 0xC0DE0007
    PropertyEnd = 3235774472, // 0xC0DE0008
  }
}
