// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace NodeEditorFramework
{
  public struct NodeData
  {
    public string adress;
    public Type[] typeOfNodeCanvas;

    public NodeData(string name, Type[] types)
    {
      this.adress = name;
      this.typeOfNodeCanvas = types;
    }
  }
}
