// Decompiled with JetBrains decompiler
// Type: YamlDotNet.CultureInfoAdapter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;

namespace YamlDotNet
{
  internal sealed class CultureInfoAdapter : CultureInfo
  {
    private readonly IFormatProvider _provider;

    public CultureInfoAdapter(CultureInfo baseCulture, IFormatProvider provider)
      : base(baseCulture.LCID)
      => this._provider = provider;

    public override object GetFormat(Type formatType) => this._provider.GetFormat(formatType);
  }
}
