﻿// Decompiled with JetBrains decompiler
// Type: Steamworks.InputMotionData_t
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct InputMotionData_t
  {
    public float rotQuatX;
    public float rotQuatY;
    public float rotQuatZ;
    public float rotQuatW;
    public float posAccelX;
    public float posAccelY;
    public float posAccelZ;
    public float rotVelX;
    public float rotVelY;
    public float rotVelZ;
  }
}