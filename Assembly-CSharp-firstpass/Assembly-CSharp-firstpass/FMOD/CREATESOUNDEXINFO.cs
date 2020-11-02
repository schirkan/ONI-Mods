// Decompiled with JetBrains decompiler
// Type: FMOD.CREATESOUNDEXINFO
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace FMOD
{
  public struct CREATESOUNDEXINFO
  {
    public int cbsize;
    public uint length;
    public uint fileoffset;
    public int numchannels;
    public int defaultfrequency;
    public SOUND_FORMAT format;
    public uint decodebuffersize;
    public int initialsubsound;
    public int numsubsounds;
    public IntPtr inclusionlist;
    public int inclusionlistnum;
    public IntPtr pcmreadcallback_handle;
    public IntPtr pcmsetposcallback_handle;
    public IntPtr nonblockcallback_handle;
    public IntPtr dlsname;
    public IntPtr encryptionkey;
    public int maxpolyphony;
    public IntPtr userdata;
    public SOUND_TYPE suggestedsoundtype;
    public IntPtr fileuseropen_handle;
    public IntPtr fileuserclose_handle;
    public IntPtr fileuserread_handle;
    public IntPtr fileuserseek_handle;
    public IntPtr fileuserasyncread_handle;
    public IntPtr fileuserasynccancel_handle;
    public IntPtr fileuserdata;
    public int filebuffersize;
    public CHANNELORDER channelorder;
    public CHANNELMASK channelmask;
    public IntPtr initialsoundgroup;
    public uint initialseekposition;
    public TIMEUNIT initialseekpostype;
    public int ignoresetfilesystem;
    public uint audioqueuepolicy;
    public uint minmidigranularity;
    public int nonblockthreadid;
    public IntPtr fsbguid;

    public SOUND_PCMREADCALLBACK pcmreadcallback
    {
      set => this.pcmreadcallback_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<SOUND_PCMREADCALLBACK>((M0) value);
    }

    public SOUND_PCMSETPOSCALLBACK pcmsetposcallback
    {
      set => this.pcmsetposcallback_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<SOUND_PCMSETPOSCALLBACK>((M0) value);
    }

    public SOUND_NONBLOCKCALLBACK nonblockcallback
    {
      set => this.nonblockcallback_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<SOUND_NONBLOCKCALLBACK>((M0) value);
    }

    public FILE_OPENCALLBACK fileuseropen
    {
      set => this.fileuseropen_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_OPENCALLBACK>((M0) value);
    }

    public FILE_CLOSECALLBACK fileuserclose
    {
      set => this.fileuserclose_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_CLOSECALLBACK>((M0) value);
    }

    public FILE_READCALLBACK fileuserread
    {
      set => this.fileuserread_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_READCALLBACK>((M0) value);
    }

    public FILE_SEEKCALLBACK fileuserseek
    {
      set => this.fileuserseek_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_SEEKCALLBACK>((M0) value);
    }

    public FILE_ASYNCREADCALLBACK fileuserasyncread
    {
      set => this.fileuserasyncread_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_ASYNCREADCALLBACK>((M0) value);
    }

    public FILE_ASYNCCANCELCALLBACK fileuserasynccancel
    {
      set => this.fileuserasynccancel_handle = value == null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate<FILE_ASYNCCANCELCALLBACK>((M0) value);
    }
  }
}
