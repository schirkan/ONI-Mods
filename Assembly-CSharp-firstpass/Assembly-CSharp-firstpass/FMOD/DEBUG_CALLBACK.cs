// Decompiled with JetBrains decompiler
// Type: FMOD.DEBUG_CALLBACK
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace FMOD
{
  public delegate RESULT DEBUG_CALLBACK(
    DEBUG_FLAGS flags,
    StringWrapper file,
    int line,
    StringWrapper func,
    StringWrapper message);
}
