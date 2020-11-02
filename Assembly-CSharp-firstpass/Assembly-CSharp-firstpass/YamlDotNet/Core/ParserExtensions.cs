// Decompiled with JetBrains decompiler
// Type: YamlDotNet.Core.ParserExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using System.IO;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
  public static class ParserExtensions
  {
    public static T Expect<T>(this IParser parser) where T : ParsingEvent
    {
      T obj = parser.Allow<T>();
      if ((object) obj != null)
        return obj;
      ParsingEvent current = parser.Current;
      throw new YamlException(current.Start, current.End, string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Expected '{0}', got '{1}' (at {2}).", (object) typeof (T).Name, (object) current.GetType().Name, (object) current.Start));
    }

    public static bool Accept<T>(this IParser parser) where T : ParsingEvent
    {
      if (parser.Current == null && !parser.MoveNext())
        throw new EndOfStreamException();
      return parser.Current is T;
    }

    public static T Allow<T>(this IParser parser) where T : ParsingEvent
    {
      if (!parser.Accept<T>())
        return default (T);
      T current = (T) parser.Current;
      parser.MoveNext();
      return current;
    }

    public static T Peek<T>(this IParser parser) where T : ParsingEvent => !parser.Accept<T>() ? default (T) : (T) parser.Current;

    public static void SkipThisAndNestedEvents(this IParser parser)
    {
      int num = 0;
      do
      {
        num += parser.Peek<ParsingEvent>().NestingIncrease;
        parser.MoveNext();
      }
      while (num > 0);
    }
  }
}
