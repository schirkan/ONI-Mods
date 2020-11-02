// Decompiled with JetBrains decompiler
// Type: FloatListType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using NodeEditorFramework;
using ProcGen.Noise;
using UnityEngine;

public class FloatListType : IConnectionTypeDeclaration
{
  public string Identifier => "FloatList";

  public System.Type Type => typeof (FloatList);

  public Color Color => Color.blue;

  public string InKnobTex => "Textures/In_Knob.png";

  public string OutKnobTex => "Textures/Out_Knob.png";
}
