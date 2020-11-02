// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Serialization.Utilities.ObjectAnchorCollection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.Utilities
{
  internal sealed class ObjectAnchorCollection
  {
    private readonly IDictionary<string, object> objectsByAnchor = (IDictionary<string, object>) new Dictionary<string, object>();
    private readonly IDictionary<object, string> anchorsByObject = (IDictionary<object, string>) new Dictionary<object, string>();

    public void Add(string anchor, object @object)
    {
      this.objectsByAnchor.Add(anchor, @object);
      if (@object == null)
        return;
      this.anchorsByObject.Add(@object, anchor);
    }

    public bool TryGetAnchor(object @object, out string anchor) => this.anchorsByObject.TryGetValue(@object, out anchor);

    public object this[string anchor]
    {
      get
      {
        object obj;
        if (this.objectsByAnchor.TryGetValue(anchor, out obj))
          return obj;
        throw new AnchorNotFoundException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "The anchor '{0}' does not exists", (object) anchor));
      }
    }
  }
}
