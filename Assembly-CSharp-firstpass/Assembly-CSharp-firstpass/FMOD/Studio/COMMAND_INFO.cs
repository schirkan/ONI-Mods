// Decompiled with JetBrains decompiler
// Type: FMOD.Studio.COMMAND_INFO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

namespace FMOD.Studio
{
  public struct COMMAND_INFO
  {
    private StringWrapper commandname;
    public int parentcommandindex;
    public int framenumber;
    public float frametime;
    public INSTANCETYPE instancetype;
    public INSTANCETYPE outputtype;
    public uint instancehandle;
    public uint outputhandle;
  }
}
