// Decompiled with JetBrains decompiler
// Type: Geometry.RectangleUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

namespace Geometry
{
  public class RectangleUtil
  {
    public static void Subtract(
      KRect r1,
      KRect r2,
      List<KRect> result,
      HorizontalEvent[] events,
      Strip[] strips,
      List<Strip> activeStrips,
      List<VerticalEvent> verticalEvents)
    {
      strips[0] = new Strip(r1.min.y, r1.max.y, false);
      strips[1] = new Strip(r2.min.y, r2.max.y, true);
      events[0] = new HorizontalEvent(r1.min.x, strips[0], true);
      events[1] = new HorizontalEvent(r1.max.x, strips[0], false);
      events[2] = new HorizontalEvent(r2.min.x, strips[1], true);
      events[3] = new HorizontalEvent(r2.max.x, strips[1], false);
      Array.Sort<HorizontalEvent>(events, (Comparison<HorizontalEvent>) ((a, b) => a.x.CompareTo(b.x)));
      activeStrips.Clear();
      for (int index = 0; index < events.Length; ++index)
      {
        if (index > 0 && activeStrips.Count > 0)
          RectangleUtil.GenerateActiveRectangles(events[index - 1].x, events[index].x, result, activeStrips, verticalEvents);
        if (events[index].isStart)
          activeStrips.Add(events[index].strip);
        else
          activeStrips.Remove(events[index].strip);
      }
    }

    public static void GenerateActiveRectangles(
      float x0,
      float x1,
      List<KRect> result,
      List<Strip> activeStrips,
      List<VerticalEvent> verticalEvents)
    {
      verticalEvents.Clear();
      for (int index = 0; index < activeStrips.Count; ++index)
      {
        verticalEvents.Add(new VerticalEvent(activeStrips[index].yMin, true, activeStrips[index].subtract));
        verticalEvents.Add(new VerticalEvent(activeStrips[index].yMax, false, activeStrips[index].subtract));
      }
      verticalEvents.Sort((Comparison<VerticalEvent>) ((a, b) => a.y.CompareTo(b.y)));
      int num1 = 0;
      float y0 = float.NegativeInfinity;
      for (int index = 0; index < verticalEvents.Count; ++index)
      {
        int num2 = num1;
        num1 += (verticalEvents[index].isStart ? 1 : -1) * (verticalEvents[index].subtract ? -1 : 1);
        if (num1 == 1 && num2 == 0)
          y0 = verticalEvents[index].y;
        else if (num1 == 0 && num2 > 0 && ((double) y0 != (double) verticalEvents[index].y && (double) x0 != (double) x1))
          result.Add(new KRect(x0, y0, x1, verticalEvents[index].y));
      }
    }
  }
}
