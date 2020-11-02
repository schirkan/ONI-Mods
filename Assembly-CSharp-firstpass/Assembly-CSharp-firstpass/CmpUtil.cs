// Decompiled with JetBrains decompiler
// Type: CmpUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

public class CmpUtil
{
  private static Dictionary<Type, CmpFns> sCmpFns = new Dictionary<Type, CmpFns>();

  public static CmpFns GetCmpFns(string type_name) => CmpUtil.GetCmpFns(Type.GetType(type_name));

  public static CmpFns GetCmpFns(Type type)
  {
    CmpFns cmpFns = (CmpFns) null;
    if (!CmpUtil.sCmpFns.TryGetValue(type, out cmpFns))
    {
      cmpFns = new CmpFns(type);
      CmpUtil.sCmpFns[type] = cmpFns;
    }
    return cmpFns;
  }
}
