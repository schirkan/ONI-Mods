// Decompiled with JetBrains decompiler
// Type: Delaunay.Triangle
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Delaunay.Utils;
using System.Collections.Generic;

namespace Delaunay
{
  public sealed class Triangle : IDisposable
  {
    private List<Site> _sites;

    public List<Site> sites => this._sites;

    public Triangle(Site a, Site b, Site c) => this._sites = new List<Site>()
    {
      a,
      b,
      c
    };

    public void Dispose()
    {
      this._sites.Clear();
      this._sites = (List<Site>) null;
    }
  }
}
