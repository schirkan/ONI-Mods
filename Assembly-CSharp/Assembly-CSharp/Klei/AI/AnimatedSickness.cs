﻿// Decompiled with JetBrains decompiler
// Type: Klei.AI.AnimatedSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class AnimatedSickness : Sickness.SicknessComponent
  {
    private KAnimFile[] kanims;
    private Expression expression;

    public AnimatedSickness(HashedString[] kanim_filenames, Expression expression)
    {
      this.kanims = new KAnimFile[kanim_filenames.Length];
      for (int index = 0; index < kanim_filenames.Length; ++index)
        this.kanims[index] = Assets.GetAnim(kanim_filenames[index]);
      this.expression = expression;
    }

    public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
    {
      for (int index = 0; index < this.kanims.Length; ++index)
        go.GetComponent<KAnimControllerBase>().AddAnimOverrides(this.kanims[index], 10f);
      if (this.expression != null)
        go.GetComponent<FaceGraph>().AddExpression(this.expression);
      return (object) null;
    }

    public override void OnCure(GameObject go, object instace_data)
    {
      if (this.expression != null)
        go.GetComponent<FaceGraph>().RemoveExpression(this.expression);
      for (int index = 0; index < this.kanims.Length; ++index)
        go.GetComponent<KAnimControllerBase>().RemoveAnimOverrides(this.kanims[index]);
    }
  }
}
