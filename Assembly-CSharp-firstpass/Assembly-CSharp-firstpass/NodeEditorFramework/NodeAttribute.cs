// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.NodeAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace NodeEditorFramework
{
  public class NodeAttribute : Attribute
  {
    public Type[] typeOfNodeCanvas;

    public bool hide { get; private set; }

    public string contextText { get; private set; }

    public NodeAttribute(bool HideNode, string ReplacedContextText, Type[] nodeCanvasTypes = null)
    {
      this.hide = HideNode;
      this.contextText = ReplacedContextText;
      Type[] typeArray = nodeCanvasTypes;
      if (typeArray == null)
        typeArray = new Type[1]{ typeof (NodeCanvas) };
      this.typeOfNodeCanvas = typeArray;
    }
  }
}
