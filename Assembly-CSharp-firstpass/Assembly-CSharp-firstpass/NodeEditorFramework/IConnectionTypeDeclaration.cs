// Decompiled with JetBrains decompiler
// Type: NodeEditorFramework.IConnectionTypeDeclaration
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using UnityEngine;

namespace NodeEditorFramework
{
  public interface IConnectionTypeDeclaration
  {
    string Identifier { get; }

    System.Type Type { get; }

    Color Color { get; }

    string InKnobTex { get; }

    string OutKnobTex { get; }
  }
}
