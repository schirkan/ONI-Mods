﻿// Decompiled with JetBrains decompiler
// Type: Personality
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;

public class Personality : Resource
{
  public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();
  public List<Trait> traits = new List<Trait>();
  public int headShape;
  public int mouth;
  public int neck;
  public int eyes;
  public int hair;
  public int body;
  public string nameStringKey;
  public string genderStringKey;
  public string personalityType;
  public string stresstrait;
  public string joyTrait;
  public string stickerType;
  public string congenitaltrait;
  public string unformattedDescription;

  public string description => this.GetDescription();

  public Personality(
    string name_string_key,
    string name,
    string Gender,
    string PersonalityType,
    string StressTrait,
    string JoyTrait,
    string StickerType,
    string CongenitalTrait,
    int headShape,
    int mouth,
    int neck,
    int eyes,
    int hair,
    int body,
    string description)
    : base(name, name)
  {
    this.nameStringKey = name_string_key;
    this.genderStringKey = Gender;
    this.personalityType = PersonalityType;
    this.stresstrait = StressTrait;
    this.joyTrait = JoyTrait;
    this.stickerType = StickerType;
    this.congenitaltrait = CongenitalTrait;
    this.unformattedDescription = description;
    this.headShape = headShape;
    this.mouth = mouth;
    this.neck = neck;
    this.eyes = eyes;
    this.hair = hair;
    this.body = body;
  }

  public string GetDescription()
  {
    this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
    return this.unformattedDescription;
  }

  public void SetAttribute(Attribute attribute, int value) => this.attributes.Add(new Personality.StartingAttribute(attribute, value));

  public void AddTrait(Trait trait) => this.traits.Add(trait);

  public class StartingAttribute
  {
    public Attribute attribute;
    public int value;

    public StartingAttribute(Attribute attribute, int value)
    {
      this.attribute = attribute;
      this.value = value;
    }
  }
}
