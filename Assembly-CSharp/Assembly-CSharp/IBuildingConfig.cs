﻿// Decompiled with JetBrains decompiler
// Type: IBuildingConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class IBuildingConfig
{
  public abstract BuildingDef CreateBuildingDef();

  public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
  }

  public abstract void DoPostConfigureComplete(GameObject go);

  public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public virtual void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public virtual void ConfigurePost(BuildingDef def)
  {
  }
}
