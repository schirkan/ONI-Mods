﻿// Decompiled with JetBrains decompiler
// Type: IGroupProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A8D8CAFB-CD16-4CDA-9F47-7D36796BFC75
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public interface IGroupProber
{
  void Occupy(object prober, int serial_no, IEnumerable<int> cells);

  void SetValidSerialNos(object prober, int previous_serial_no, int serial_no);

  bool ReleaseProber(object prober);
}
