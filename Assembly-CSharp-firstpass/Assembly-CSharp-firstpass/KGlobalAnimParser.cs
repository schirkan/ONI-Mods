// Decompiled with JetBrains decompiler
// Type: KGlobalAnimParser
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1DCC1186-7B52-4F7C-B248-8468843E982E
// Assembly location: H:\Steam\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp-firstpass.dll

using Klei;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KGlobalAnimParser
{
  public static KAnimHashedString MISSING_SYMBOL = new KAnimHashedString(nameof (MISSING_SYMBOL));
  public static string ANIM_COMMAND_FILE = "batchgroup.yaml";
  public const float ANIM_SCALE = 0.005f;
  private Dictionary<HashedString, AnimCommandFile> commandFiles = new Dictionary<HashedString, AnimCommandFile>();
  private Dictionary<int, KAnimFileData> files = new Dictionary<int, KAnimFileData>();

  private static KGlobalAnimParser instance => Singleton<KGlobalAnimParser>.Instance;

  public static void CreateInstance() => Singleton<KGlobalAnimParser>.CreateInstance();

  public static KGlobalAnimParser Get() => KGlobalAnimParser.instance;

  public static void DestroyInstance()
  {
    if (KGlobalAnimParser.instance != null)
    {
      KGlobalAnimParser.instance.commandFiles.Clear();
      KGlobalAnimParser.instance.commandFiles = (Dictionary<HashedString, AnimCommandFile>) null;
      KGlobalAnimParser.instance.files.Clear();
      KGlobalAnimParser.instance.files = (Dictionary<int, KAnimFileData>) null;
    }
    Singleton<KGlobalAnimParser>.DestroyInstance();
  }

  public KAnimFileData GetFile(KAnimFile anim_file)
  {
    KAnimFileData kanimFileData = (KAnimFileData) null;
    int instanceId = anim_file.GetInstanceID();
    if (!this.files.TryGetValue(instanceId, out kanimFileData))
    {
      kanimFileData = new KAnimFileData(anim_file.name);
      this.files[instanceId] = kanimFileData;
    }
    return kanimFileData;
  }

  public KAnimFileData Load(KAnimFile anim_file)
  {
    KAnimFileData kanimFileData = (KAnimFileData) null;
    if (!this.files.TryGetValue(anim_file.GetInstanceID(), out kanimFileData))
      kanimFileData = this.GetFile(anim_file);
    return kanimFileData;
  }

  public static AnimCommandFile GetParseCommands(string path)
  {
    string path1 = path;
    string fullName = Directory.GetParent(path1).FullName;
    HashedString key = new HashedString(fullName);
    if (KGlobalAnimParser.Get().commandFiles.ContainsKey(key))
      return KGlobalAnimParser.instance.commandFiles[key];
    string str = Path.Combine(fullName, KGlobalAnimParser.ANIM_COMMAND_FILE);
    if (!File.Exists(str))
      return (AnimCommandFile) null;
    AnimCommandFile animCommandFile = YamlIO.LoadFile<AnimCommandFile>(str);
    animCommandFile.directory = "Assets/anim/" + Directory.GetParent(path1).Name;
    KGlobalAnimParser.instance.commandFiles[key] = animCommandFile;
    return animCommandFile;
  }

  public static void ParseAnimData(
    KBatchGroupData data,
    HashedString fileNameHash,
    FastReader reader,
    KAnimFileData animFile)
  {
    KGlobalAnimParser.CheckHeader("ANIM", reader);
    KGlobalAnimParser.Assert(reader.ReadUInt32() == 5U, "Invalid anim.bytes version");
    reader.ReadInt32();
    reader.ReadInt32();
    int num1 = reader.ReadInt32();
    animFile.maxVisSymbolFrames = 0;
    animFile.animCount = 0;
    animFile.frameCount = 0;
    animFile.elementCount = 0;
    animFile.firstAnimIndex = data.anims.Count;
    animFile.animBatchTag = data.groupID;
    data.animIndex.Add((KAnimHashedString) fileNameHash, data.anims.Count);
    animFile.firstElementIndex = data.frameElements.Count;
    for (int index1 = 0; index1 < num1; ++index1)
    {
      KAnim.Anim anim = new KAnim.Anim(animFile, data.anims.Count);
      anim.name = reader.ReadKleiString();
      string text = animFile.name + "." + anim.name;
      anim.id = (HashedString) text;
      HashCache.Get().Add(anim.name);
      HashCache.Get().Add(text);
      anim.hash = (HashedString) anim.name;
      anim.rootSymbol.HashValue = reader.ReadInt32();
      anim.frameRate = reader.ReadSingle();
      anim.firstFrameIdx = data.animFrames.Count;
      anim.numFrames = reader.ReadInt32();
      anim.totalTime = (float) anim.numFrames / anim.frameRate;
      anim.scaledBoundingRadius = 0.0f;
      for (int index2 = 0; index2 < anim.numFrames; ++index2)
      {
        KAnim.Anim.Frame frame = new KAnim.Anim.Frame();
        float num2 = reader.ReadSingle();
        float num3 = reader.ReadSingle();
        float num4 = reader.ReadSingle();
        float num5 = reader.ReadSingle();
        frame.bbox = new AABB3(new Vector3(num2 - num4 * 0.5f, (float) -((double) num3 + (double) num5 * 0.5), 0.0f) * 0.005f, new Vector3(num2 + num4 * 0.5f, (float) -((double) num3 - (double) num5 * 0.5), 0.0f) * 0.005f);
        float val1 = Math.Max(Math.Abs(frame.bbox.max.x), Math.Abs(frame.bbox.min.x));
        float val2 = Math.Max(Math.Abs(frame.bbox.max.y), Math.Abs(frame.bbox.min.y));
        float num6 = Math.Max(val1, val2);
        anim.unScaledSize.x = Math.Max(anim.unScaledSize.x, val1 / 0.005f);
        anim.unScaledSize.y = Math.Max(anim.unScaledSize.y, val2 / 0.005f);
        anim.scaledBoundingRadius = Math.Max(anim.scaledBoundingRadius, Mathf.Sqrt((float) ((double) num6 * (double) num6 + (double) num6 * (double) num6)));
        frame.idx = data.animFrames.Count;
        frame.firstElementIdx = data.frameElements.Count;
        frame.numElements = reader.ReadInt32();
        int num7 = 0;
        for (int index3 = 0; index3 < frame.numElements; ++index3)
        {
          KAnim.Anim.FrameElement frameElement = new KAnim.Anim.FrameElement();
          frameElement.fileHash = (KAnimHashedString) fileNameHash;
          frameElement.symbol = new KAnimHashedString(reader.ReadInt32());
          frameElement.frame = reader.ReadInt32();
          frameElement.folder = new KAnimHashedString(reader.ReadInt32());
          frameElement.flags = reader.ReadInt32();
          float a = reader.ReadSingle();
          float b = reader.ReadSingle();
          float g = reader.ReadSingle();
          float r = reader.ReadSingle();
          frameElement.multColour = new Color(r, g, b, a);
          float num8 = reader.ReadSingle();
          float num9 = reader.ReadSingle();
          float num10 = reader.ReadSingle();
          float num11 = reader.ReadSingle();
          float num12 = reader.ReadSingle();
          float num13 = reader.ReadSingle();
          double num14 = (double) reader.ReadSingle();
          frameElement.transform.m00 = num8;
          frameElement.transform.m01 = num10;
          frameElement.transform.m02 = num12;
          frameElement.transform.m10 = num9;
          frameElement.transform.m11 = num11;
          frameElement.transform.m12 = num13;
          int symbolIndex = data.GetSymbolIndex(frameElement.symbol);
          if (symbolIndex == -1)
          {
            ++num7;
            frameElement.symbol = KGlobalAnimParser.MISSING_SYMBOL;
          }
          else
          {
            frameElement.symbolIdx = symbolIndex;
            data.frameElements.Add(frameElement);
            ++animFile.elementCount;
          }
        }
        frame.numElements -= num7;
        data.animFrames.Add(frame);
        ++animFile.frameCount;
      }
      data.AddAnim(anim);
      ++animFile.animCount;
    }
    Debug.Assert(num1 == animFile.animCount);
    data.animCount[(KAnimHashedString) fileNameHash] = animFile.animCount;
    animFile.maxVisSymbolFrames = Math.Max(animFile.maxVisSymbolFrames, reader.ReadInt32());
    data.UpdateMaxVisibleSymbols(animFile.maxVisSymbolFrames);
    KGlobalAnimParser.ParseHashTable(reader);
  }

  private static void ParseHashTable(FastReader reader)
  {
    int num = reader.ReadInt32();
    for (int index = 0; index < num; ++index)
    {
      int hash = reader.ReadInt32();
      string text = reader.ReadKleiString();
      HashCache.Get().Add(hash, text);
    }
  }

  public static int ParseBuildData(
    KBatchGroupData data,
    KAnimHashedString fileNameHash,
    FastReader reader,
    List<Texture2D> textures)
  {
    KGlobalAnimParser.CheckHeader("BILD", reader);
    int num1 = reader.ReadInt32();
    switch (num1)
    {
      case 9:
      case 10:
        KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(data.groupID);
        if (group == null)
          Debug.LogErrorFormat("[{1}] Failed to get group [{0}]", (object) data.groupID, (object) fileNameHash.DebuggerDisplay);
        int length1 = reader.ReadInt32();
        int length2 = reader.ReadInt32();
        KAnim.Build build = data.AddNewBuildFile(fileNameHash);
        build.textureCount = textures.Count;
        if (textures.Count > 0)
          data.AddTextures(textures);
        build.symbols = new KAnim.Build.Symbol[length1];
        build.frames = new KAnim.Build.SymbolFrame[length2];
        build.name = reader.ReadKleiString();
        build.batchTag = group.swapTarget.IsValid ? group.target : data.groupID;
        build.fileHash = fileNameHash;
        int index1 = 0;
        for (int index2 = 0; index2 < build.symbols.Length; ++index2)
        {
          KAnimHashedString kanimHashedString = new KAnimHashedString(reader.ReadInt32());
          KAnim.Build.Symbol symbol = new KAnim.Build.Symbol();
          symbol.build = build;
          symbol.hash = kanimHashedString;
          if (num1 > 9)
            symbol.path = new KAnimHashedString(reader.ReadInt32());
          symbol.colourChannel = new KAnimHashedString(reader.ReadInt32());
          symbol.flags = reader.ReadInt32();
          symbol.firstFrameIdx = data.symbolFrameInstances.Count;
          symbol.numFrames = reader.ReadInt32();
          symbol.symbolIndexInSourceBuild = index2;
          int val2 = 0;
          for (int index3 = 0; index3 < symbol.numFrames; ++index3)
          {
            KAnim.Build.SymbolFrame symbolFrame = new KAnim.Build.SymbolFrame();
            KAnim.Build.SymbolFrameInstance symbolFrameInstance = new KAnim.Build.SymbolFrameInstance();
            symbolFrameInstance.symbolFrame = symbolFrame;
            symbolFrame.fileNameHash = fileNameHash;
            symbolFrame.sourceFrameNum = reader.ReadInt32();
            symbolFrame.duration = reader.ReadInt32();
            symbolFrameInstance.buildImageIdx = data.textureStartIndex[fileNameHash] + reader.ReadInt32();
            if (symbolFrameInstance.buildImageIdx >= textures.Count + data.textureStartIndex[fileNameHash])
              Debug.LogErrorFormat("{0} Symbol: [{1}] tex count: [{2}] buildImageIdx: [{3}] group total [{4}]", (object) fileNameHash.ToString(), (object) symbol.hash, (object) textures.Count, (object) symbolFrameInstance.buildImageIdx, (object) data.textureStartIndex[fileNameHash]);
            symbolFrameInstance.symbolIdx = data.GetSymbolCount();
            val2 = Math.Max(symbolFrame.sourceFrameNum + symbolFrame.duration, val2);
            float num2 = reader.ReadSingle();
            float num3 = reader.ReadSingle();
            float num4 = reader.ReadSingle();
            float num5 = reader.ReadSingle();
            symbolFrame.bboxMin = new Vector2(num2 - num4 * 0.5f, num3 - num5 * 0.5f);
            symbolFrame.bboxMax = new Vector2(num2 + num4 * 0.5f, num3 + num5 * 0.5f);
            float x1 = reader.ReadSingle();
            float num6 = reader.ReadSingle();
            float x2 = reader.ReadSingle();
            float num7 = reader.ReadSingle();
            symbolFrame.uvMin = new Vector2(x1, 1f - num6);
            symbolFrame.uvMax = new Vector2(x2, 1f - num7);
            build.frames[index1] = symbolFrame;
            data.symbolFrameInstances.Add(symbolFrameInstance);
            ++index1;
          }
          symbol.numLookupFrames = val2;
          data.AddBuildSymbol(symbol);
          build.symbols[index2] = symbol;
        }
        KGlobalAnimParser.ParseHashTable(reader);
        return build.index;
      default:
        Debug.LogError((object) (fileNameHash.ToString() + " has invalid build.bytes version [" + (object) num1 + "]"));
        return -1;
    }
  }

  public static void PostParse(KBatchGroupData data)
  {
    for (int index1 = 0; index1 < data.GetSymbolCount(); ++index1)
    {
      KAnim.Build.Symbol symbol = data.GetSymbol(index1);
      if (symbol == null)
      {
        Debug.LogWarning((object) ("Symbol null for [" + (object) data.groupID + "] idx: [" + (object) index1 + "]"));
      }
      else
      {
        if (symbol.numLookupFrames <= 0)
        {
          int a = symbol.numFrames;
          for (int firstFrameIdx = symbol.firstFrameIdx; firstFrameIdx < symbol.firstFrameIdx + symbol.numFrames; ++firstFrameIdx)
          {
            KAnim.Build.SymbolFrameInstance symbolFrameInstance = data.GetSymbolFrameInstance(firstFrameIdx);
            a = Mathf.Max(a, symbolFrameInstance.symbolFrame.sourceFrameNum + symbolFrameInstance.symbolFrame.duration);
          }
          symbol.numLookupFrames = a;
        }
        symbol.frameLookup = new int[symbol.numLookupFrames];
        if (symbol.numLookupFrames <= 0)
        {
          Debug.LogWarning((object) ("No lookup frames for  [" + (object) data.groupID + "] build: [" + symbol.build.name + "] idx: [" + (object) index1 + "] id: [" + (object) symbol.hash + "]"));
        }
        else
        {
          for (int index2 = 0; index2 < symbol.numLookupFrames; ++index2)
            symbol.frameLookup[index2] = -1;
          for (int firstFrameIdx = symbol.firstFrameIdx; firstFrameIdx < symbol.firstFrameIdx + symbol.numFrames; ++firstFrameIdx)
          {
            KAnim.Build.SymbolFrameInstance symbolFrameInstance = data.GetSymbolFrameInstance(firstFrameIdx);
            if (symbolFrameInstance.symbolFrame == null)
            {
              Debug.LogWarning((object) ("No symbol frame  [" + (object) data.groupID + "] symFrameIdx: [" + (object) firstFrameIdx + "] id: [" + (object) symbol.hash + "]"));
            }
            else
            {
              for (int sourceFrameNum = symbolFrameInstance.symbolFrame.sourceFrameNum; sourceFrameNum < symbolFrameInstance.symbolFrame.sourceFrameNum + symbolFrameInstance.symbolFrame.duration; ++sourceFrameNum)
              {
                if (sourceFrameNum >= symbol.frameLookup.Length)
                  Debug.LogWarning((object) ("Too many lookup frames [" + (object) sourceFrameNum + ">=" + (object) symbol.frameLookup.Length + "] for  [" + (object) data.groupID + "] idx: [" + (object) index1 + "] id: [" + (object) symbol.hash + "]"));
                else
                  symbol.frameLookup[sourceFrameNum] = firstFrameIdx;
              }
            }
          }
          string str1 = HashCache.Get().Get(symbol.path);
          if (!string.IsNullOrEmpty(str1))
          {
            int length = str1.IndexOf("/");
            if (length != -1)
            {
              string str2 = str1.Substring(0, length);
              symbol.folder = new KAnimHashedString(str2);
              HashCache.Get().Add(symbol.folder.HashValue, str2);
            }
          }
        }
      }
    }
  }

  private static void Assert(bool condition, string message)
  {
    if (!condition)
      throw new Exception(message);
  }

  private static void CheckHeader(string header, FastReader reader)
  {
    char[] chArray = reader.ReadChars(header.Length);
    for (int index = 0; index < header.Length; ++index)
    {
      if ((int) chArray[index] != (int) header[index])
        throw new Exception("Expected " + header);
    }
  }
}
