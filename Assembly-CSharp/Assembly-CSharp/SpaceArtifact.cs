// Decompiled with JetBrains decompiler
// Type: SpaceArtifact
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
  public const string ID = "SpaceArtifact";
  [SerializeField]
  private string ui_anim;
  [SerializeField]
  private ArtifactTier artifactTier;

  public void SetArtifactTier(ArtifactTier tier) => this.artifactTier = tier;

  public ArtifactTier GetArtifactTier() => this.artifactTier;

  public void SetUIAnim(string anim) => this.ui_anim = anim;

  public string GetUIAnim() => this.ui_anim;

  public List<Descriptor> GetEffectDescriptions() => new List<Descriptor>()
  {
    new Descriptor(string.Format("This is an artifact from space", (object[]) Array.Empty<object>()), string.Format("This is the tooltip string", (object[]) Array.Empty<object>()), Descriptor.DescriptorType.Information)
  };

  public List<Descriptor> GetDescriptors(GameObject go) => this.GetEffectDescriptions();
}
