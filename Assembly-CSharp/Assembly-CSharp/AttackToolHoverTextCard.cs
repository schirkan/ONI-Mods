// Decompiled with JetBrains decompiler
// Type: AttackToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class AttackToolHoverTextCard : HoverTextConfiguration
{
  public override void UpdateHoverElements(List<KSelectable> hover_objects)
  {
    HoverTextScreen instance = HoverTextScreen.Instance;
    HoverTextDrawer drawer = instance.BeginDrawing();
    drawer.BeginShadowBar();
    this.DrawTitle(instance, drawer);
    this.DrawInstructions(HoverTextScreen.Instance, drawer);
    drawer.EndShadowBar();
    if (hover_objects != null)
    {
      foreach (KSelectable hoverObject in hover_objects)
      {
        if ((Object) hoverObject.GetComponent<AttackableBase>() != (Object) null)
        {
          drawer.BeginShadowBar();
          drawer.DrawText(hoverObject.GetProperName().ToUpper(), this.Styles_Title.Standard);
          drawer.EndShadowBar();
          break;
        }
      }
    }
    drawer.EndDrawing();
  }
}
