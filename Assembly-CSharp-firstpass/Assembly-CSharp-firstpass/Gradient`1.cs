// Decompiled with JetBrains decompiler
// Type: Gradient`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

[Serializable]
public class Gradient<T>
{
  public Gradient(T content, float bandSize)
  {
    this.bandSize = bandSize;
    this.content = content;
  }

  public T content { get; protected set; }

  public float bandSize { get; protected set; }

  public float maxValue { get; set; }
}
