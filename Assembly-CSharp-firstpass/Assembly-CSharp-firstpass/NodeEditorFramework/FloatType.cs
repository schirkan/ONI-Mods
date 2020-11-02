// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.FloatType
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace NodeEditorFramework
{
  public class FloatType : IConnectionTypeDeclaration
  {
    public string Identifier => "Float";

    public System.Type Type => typeof (float);

    public Color Color => Color.cyan;

    public string InKnobTex => "Textures/In_Knob.png";

    public string OutKnobTex => "Textures/Out_Knob.png";
  }
}
