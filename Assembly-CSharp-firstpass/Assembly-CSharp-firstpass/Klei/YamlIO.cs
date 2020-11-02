// Decompiled with JetBrains decompiler
// Type: Klei.YamlIO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace Klei
{
  public static class YamlIO
  {
    private const bool verbose_errors = false;

    public static void Save<T>(
      T some_object,
      string filename,
      List<Tuple<string, Type>> tagMappings = null)
    {
      using (StreamWriter streamWriter = new StreamWriter(filename))
      {
        SerializerBuilder serializerBuilder = new SerializerBuilder();
        if (tagMappings != null)
        {
          foreach (Tuple<string, Type> tagMapping in tagMappings)
            serializerBuilder = serializerBuilder.WithTagMapping(tagMapping.first, tagMapping.second);
        }
        serializerBuilder.Build().Serialize((TextWriter) streamWriter, (object) some_object);
      }
    }

    public static void SaveOrWarnUser<T>(
      T some_object,
      string filename,
      List<Tuple<string, Type>> tagMappings = null)
    {
      FileUtil.DoIODialog((System.Action) (() => YamlIO.Save<T>(some_object, filename, tagMappings)), filename);
    }

    public static T LoadFile<T>(
      FileHandle filehandle,
      YamlIO.ErrorHandler handle_error = null,
      List<Tuple<string, Type>> tagMappings = null)
    {
      return YamlIO.Parse<T>(FileSystem.ConvertToText(filehandle.source.ReadBytes(filehandle.full_path)), filehandle, handle_error, tagMappings);
    }

    public static T LoadFile<T>(
      string filename,
      YamlIO.ErrorHandler handle_error = null,
      List<Tuple<string, Type>> tagMappings = null)
    {
      return YamlIO.LoadFile<T>(FileSystem.FindFileHandle(filename), handle_error, tagMappings);
    }

    public static void LogError(YamlIO.Error error, bool force_log_as_warning)
    {
      YamlIO.ErrorLogger errorLogger = force_log_as_warning || error.severity == YamlIO.Error.Severity.Recoverable ? new YamlIO.ErrorLogger(Debug.LogWarningFormat) : new YamlIO.ErrorLogger(Debug.LogErrorFormat);
      if (error.inner_exception == null)
        errorLogger("{0} parse error in {1}\n{2}", new object[3]
        {
          (object) error.severity,
          (object) error.file.full_path,
          (object) error.message
        });
      else
        errorLogger("{0} parse error in {1}\n{2}\n{3}", new object[4]
        {
          (object) error.severity,
          (object) error.file.full_path,
          (object) error.message,
          (object) error.inner_exception.Message
        });
    }

    public static T Parse<T>(
      string readText,
      FileHandle debugFileHandle,
      YamlIO.ErrorHandler handle_error = null,
      List<Tuple<string, Type>> tagMappings = null)
    {
      try
      {
        if (handle_error == null)
          handle_error = new YamlIO.ErrorHandler(YamlIO.LogError);
        readText = readText.Replace("\t", "    ");
        System.Action<string> unmatchedLogFn = (System.Action<string>) (error => handle_error(new YamlIO.Error()
        {
          file = debugFileHandle,
          text = readText,
          message = error,
          severity = YamlIO.Error.Severity.Recoverable
        }, false));
        DeserializerBuilder deserializerBuilder = new DeserializerBuilder();
        deserializerBuilder.IgnoreUnmatchedProperties(unmatchedLogFn);
        if (tagMappings != null)
        {
          foreach (Tuple<string, Type> tagMapping in tagMappings)
            deserializerBuilder = deserializerBuilder.WithTagMapping(tagMapping.first, tagMapping.second);
        }
        return deserializerBuilder.Build().Deserialize<T>((TextReader) new StringReader(readText));
      }
      catch (Exception ex)
      {
        handle_error(new YamlIO.Error()
        {
          file = debugFileHandle,
          text = readText,
          message = ex.Message,
          inner_exception = ex.InnerException,
          severity = YamlIO.Error.Severity.Fatal
        }, false);
      }
      return default (T);
    }

    public struct Error
    {
      public FileHandle file;
      public string message;
      public Exception inner_exception;
      public string text;
      public YamlIO.Error.Severity severity;

      public enum Severity
      {
        Fatal,
        Recoverable,
      }
    }

    public delegate void ErrorHandler(YamlIO.Error error, bool force_log_as_warning);

    private delegate void ErrorLogger(string format, params object[] args);
  }
}
