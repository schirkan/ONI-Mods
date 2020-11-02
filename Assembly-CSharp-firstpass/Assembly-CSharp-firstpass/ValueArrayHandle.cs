// Decompiled with JetBrains decompiler
// Type: ValueArrayHandle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

public struct ValueArrayHandle
{
  public int handle;
  public static readonly ValueArrayHandle Invalid = new ValueArrayHandle(-1);

  public ValueArrayHandle(int handle) => this.handle = handle;

  public bool IsValid() => this.handle >= 0;
}
