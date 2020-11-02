// Decompiled with JetBrains decompiler
// Type: IModule3DNodeType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using NodeEditorFramework;
using UnityEngine;

public class IModule3DNodeType : IConnectionTypeDeclaration
{
  public string Identifier => "IModule3D";

  public System.Type Type => typeof (IModule3D);

  public Color Color => Color.magenta;

  public string InKnobTex => "Textures/In_Knob.png";

  public string OutKnobTex => "Textures/Out_Knob.png";
}
