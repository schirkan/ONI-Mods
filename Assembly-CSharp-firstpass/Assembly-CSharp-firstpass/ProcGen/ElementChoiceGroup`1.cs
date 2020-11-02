// Decompiled with JetBrains decompiler
// Type: ProcGen.ElementChoiceGroup`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using KSerialization.Converters;
using System;
using System.Collections.Generic;

namespace ProcGen
{
  [Serializable]
  public class ElementChoiceGroup<T>
  {
    [StringEnumConverter]
    public Room.Selection selectionMethod { get; private set; }

    public List<T> choices { get; private set; }

    public ElementChoiceGroup() => this.choices = new List<T>();

    public ElementChoiceGroup(List<T> choices, Room.Selection selectionMethod)
    {
      this.choices = choices;
      this.selectionMethod = selectionMethod;
    }
  }
}
