// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.TypeData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using NodeEditorFramework.Utilities;
using System;
using UnityEngine;

namespace NodeEditorFramework
{
  public class TypeData
  {
    private IConnectionTypeDeclaration declaration;

    public string Identifier { get; private set; }

    public System.Type Type { get; private set; }

    public Color Color { get; private set; }

    public Texture2D InKnobTex { get; private set; }

    public Texture2D OutKnobTex { get; private set; }

    internal TypeData(IConnectionTypeDeclaration typeDecl)
    {
      this.Identifier = typeDecl.Identifier;
      this.declaration = typeDecl;
      this.Type = this.declaration.Type;
      this.Color = this.declaration.Color;
      this.InKnobTex = ResourceManager.GetTintedTexture(this.declaration.InKnobTex, this.Color);
      this.OutKnobTex = ResourceManager.GetTintedTexture(this.declaration.OutKnobTex, this.Color);
      if (!this.isValid())
        throw new DataMisalignedException("Type Declaration " + typeDecl.Identifier + " contains invalid data!");
    }

    public TypeData(System.Type type)
    {
      this.Identifier = type.Name;
      this.declaration = (IConnectionTypeDeclaration) null;
      this.Type = type;
      this.Color = Color.white;
      byte[] bytes = BitConverter.GetBytes(type.GetHashCode());
      this.Color = new Color(Mathf.Pow((float) bytes[0] / (float) byte.MaxValue, 0.5f), Mathf.Pow((float) bytes[1] / (float) byte.MaxValue, 0.5f), Mathf.Pow((float) bytes[2] / (float) byte.MaxValue, 0.5f));
      this.InKnobTex = ResourceManager.GetTintedTexture("Textures/In_Knob.png", this.Color);
      this.OutKnobTex = ResourceManager.GetTintedTexture("Textures/Out_Knob.png", this.Color);
    }

    public bool isValid() => this.Type != (System.Type) null && (UnityEngine.Object) this.InKnobTex != (UnityEngine.Object) null && (UnityEngine.Object) this.OutKnobTex != (UnityEngine.Object) null;
  }
}
