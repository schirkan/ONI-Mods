// Decompiled with JetBrains decompiler
// Type: Klei.FileUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.IO;
using System.Threading;

namespace Klei
{
  public static class FileUtil
  {
    private const FileUtil.Test TEST = FileUtil.Test.NoTesting;
    private const int DEFAULT_RETRY_COUNT = 0;
    private const int RETRY_MILLISECONDS = 100;
    public static FileUtil.ErrorType errorType;
    public static string errorSubject;
    public static string exceptionMessage;
    public static string exceptionStackTrace;

    public static event System.Action onErrorMessage;

    public static void ErrorDialog(
      FileUtil.ErrorType errorType,
      string errorSubject,
      string exceptionMessage,
      string exceptionStackTrace)
    {
      Debug.Log((object) string.Format("Error encountered during file access: {0} error: {1}", (object) errorType, (object) errorSubject));
      FileUtil.errorType = errorType;
      FileUtil.errorSubject = errorSubject;
      FileUtil.exceptionMessage = exceptionMessage;
      FileUtil.exceptionStackTrace = exceptionStackTrace;
      if (FileUtil.onErrorMessage == null)
        return;
      FileUtil.onErrorMessage();
    }

    public static T DoIOFunc<T>(Func<T> io_op, int retry_count = 0)
    {
      UnauthorizedAccessException unauthorizedAccessException = (UnauthorizedAccessException) null;
      IOException ioException = (IOException) null;
      Exception exception = (Exception) null;
      for (int index = 0; index <= retry_count; ++index)
      {
        try
        {
          return io_op();
        }
        catch (UnauthorizedAccessException ex)
        {
          unauthorizedAccessException = ex;
        }
        catch (IOException ex)
        {
          ioException = ex;
        }
        catch (Exception ex)
        {
          exception = ex;
        }
        Thread.Sleep(index * 100);
      }
      if (unauthorizedAccessException != null)
        throw unauthorizedAccessException;
      if (ioException != null)
        throw ioException;
      if (exception != null)
        throw exception;
      throw new Exception("Unreachable code path in FileUtil::DoIOFunc()");
    }

    public static void DoIOAction(System.Action io_op, int retry_count = 0)
    {
      UnauthorizedAccessException unauthorizedAccessException = (UnauthorizedAccessException) null;
      IOException ioException = (IOException) null;
      Exception exception = (Exception) null;
      for (int index = 0; index <= retry_count; ++index)
      {
        try
        {
          io_op();
          return;
        }
        catch (UnauthorizedAccessException ex)
        {
          unauthorizedAccessException = ex;
        }
        catch (IOException ex)
        {
          ioException = ex;
        }
        catch (Exception ex)
        {
          exception = ex;
        }
        Thread.Sleep(index * 100);
      }
      if (unauthorizedAccessException != null)
        throw unauthorizedAccessException;
      if (ioException != null)
        throw ioException;
      if (exception != null)
        throw exception;
      throw new Exception("Unreachable code path in FileUtil::DoIOAction()");
    }

    public static void DoIODialog(System.Action io_op, string io_subject, int retry_count = 0)
    {
      try
      {
        FileUtil.DoIOAction(io_op, retry_count);
      }
      catch (UnauthorizedAccessException ex)
      {
        DebugUtil.LogArgs((object) "UnauthorizedAccessException during IO on ", (object) io_subject, (object) ", squelching. Stack trace was:\n", (object) ex.Message, (object) "\n", (object) ex.StackTrace);
        FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, io_subject, ex.Message, ex.StackTrace);
      }
      catch (IOException ex)
      {
        DebugUtil.LogArgs((object) "IOException during IO on ", (object) io_subject, (object) ", squelching. Stack trace was:\n", (object) ex.Message, (object) "\n", (object) ex.StackTrace);
        FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, io_subject, ex.Message, ex.StackTrace);
      }
      catch
      {
        throw;
      }
    }

    public static T DoIODialog<T>(
      Func<T> io_op,
      string io_subject,
      T fail_result,
      int retry_count = 0)
    {
      try
      {
        return FileUtil.DoIOFunc<T>(io_op, retry_count);
      }
      catch (UnauthorizedAccessException ex)
      {
        DebugUtil.LogArgs((object) "UnauthorizedAccessException during IO on ", (object) io_subject, (object) ", squelching. Stack trace was:\n", (object) ex.Message, (object) "\n", (object) ex.StackTrace);
        FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, io_subject, ex.Message, ex.StackTrace);
      }
      catch (IOException ex)
      {
        DebugUtil.LogArgs((object) "IOException during IO on ", (object) io_subject, (object) ", squelching. Stack trace was:\n", (object) ex.Message, (object) "\n", (object) ex.StackTrace);
        FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, io_subject, ex.Message, ex.StackTrace);
      }
      catch
      {
        throw;
      }
      return fail_result;
    }

    public static FileStream Create(string filename, int retry_count = 0) => FileUtil.DoIODialog<FileStream>((Func<FileStream>) (() => File.Create(filename)), filename, (FileStream) null, retry_count);

    public static bool CreateDirectory(string path, int retry_count = 0) => FileUtil.DoIODialog<bool>((Func<bool>) (() =>
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return true;
    }), path, false, retry_count);

    public static bool DeleteDirectory(string path, int retry_count = 0) => FileUtil.DoIODialog<bool>((Func<bool>) (() =>
    {
      if (!Directory.Exists(path))
        return true;
      Directory.Delete(path, true);
      return true;
    }), path, false, retry_count);

    public static bool FileExists(string filename, int retry_count = 0) => FileUtil.DoIODialog<bool>((Func<bool>) (() => File.Exists(filename)), filename, false, retry_count);

    private enum Test
    {
      NoTesting,
      RetryOnce,
    }

    public enum ErrorType
    {
      None,
      UnauthorizedAccess,
      IOError,
    }
  }
}
