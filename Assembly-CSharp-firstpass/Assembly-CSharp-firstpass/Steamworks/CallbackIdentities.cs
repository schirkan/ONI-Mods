// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackIdentities
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
  internal class CallbackIdentities
  {
    public static int GetCallbackIdentity(Type callbackStruct)
    {
      object[] customAttributes = callbackStruct.GetCustomAttributes(typeof (CallbackIdentityAttribute), false);
      int index = 0;
      if (index < customAttributes.Length)
        return ((CallbackIdentityAttribute) customAttributes[index]).Identity;
      throw new Exception("Callback number not found for struct " + (object) callbackStruct);
    }
  }
}
