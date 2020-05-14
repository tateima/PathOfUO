/***************************************************************************
 *                                Packets.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Server.Accounting;
using Server.Compression;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Menus;
using Server.Menus.ItemLists;
using Server.Menus.Questions;
using Server.Prompts;
using Server.Targeting;

namespace Server.Network
{
  public enum PMMessage : byte
  {
    CharNoExist = 1,
    CharExists = 2,
    CharInWorld = 5,
    LoginSyncError = 6,
    IdleWarning = 7
  }

  public enum LRReason : byte
  {
    CannotLift = 0,
    OutOfRange = 1,
    OutOfSight = 2,
    TryToSteal = 3,
    AreHolding = 4,
    Inspecific = 5
  }

  [Flags]
  public enum CMEFlags
  {
    None = 0x00,
    Disabled = 0x01,
    Arrow = 0x02,
    Highlighted = 0x04,
    Colored = 0x20
  }

  public sealed class WorldItem : Packet
  {
    public WorldItem(Item item) : base(0x1A)
    {
      EnsureCapacity(20);

      // 14 base length
      // +2 - Amount
      // +2 - Hue
      // +1 - Flags

      var serial = item.Serial.Value;
      var itemID = item.ItemID & 0x3FFF;
      var amount = item.Amount;
      var loc = item.Location;
      var x = loc.m_X;
      var y = loc.m_Y;
      var hue = item.Hue;
      var flags = item.GetPacketFlags();
      var direction = (int)item.Direction;

      if (amount != 0)
        serial |= 0x80000000;
      else
        serial &= 0x7FFFFFFF;

      Stream.Write(serial);

      if (item is BaseMulti)
        Stream.Write((short)(itemID | 0x4000));
      else
        Stream.Write((short)itemID);

      if (amount != 0) Stream.Write((short)amount);

      x &= 0x7FFF;

      if (direction != 0) x |= 0x8000;

      Stream.Write((short)x);

      y &= 0x3FFF;

      if (hue != 0) y |= 0x8000;

      if (flags != 0) y |= 0x4000;

      Stream.Write((short)y);

      if (direction != 0)
        Stream.Write((byte)direction);

      Stream.Write((sbyte)loc.m_Z);

      if (hue != 0)
        Stream.Write((ushort)hue);

      if (flags != 0)
        Stream.Write((byte)flags);
    }
  }

  public sealed class WorldItemSA : Packet
  {
    public WorldItemSA(Item item) : base(0xF3, 24)
    {
      Stream.Write((short)0x1);

      var itemID = item.ItemID;

      if (item is BaseMulti)
      {
        Stream.Write((byte)0x02);

        Stream.Write(item.Serial);

        itemID &= 0x3FFF;

        Stream.Write((short)itemID);

        Stream.Write((byte)0);
        /*} else if ( ) {
          m_Stream.Write( (byte) 0x01 );

          m_Stream.Write( (int) item.Serial );

          m_Stream.Write( (short) itemID );

          m_Stream.Write( (byte) item.Direction );*/
      }
      else
      {
        Stream.Write((byte)0x00);

        Stream.Write(item.Serial);

        itemID &= 0x7FFF;

        Stream.Write((short)itemID);

        Stream.Write((byte)0);
      }

      var amount = item.Amount;
      Stream.Write((short)amount);
      Stream.Write((short)amount);

      var loc = item.Location;
      var x = loc.m_X & 0x7FFF;
      var y = loc.m_Y & 0x3FFF;
      Stream.Write((short)x);
      Stream.Write((short)y);
      Stream.Write((sbyte)loc.m_Z);

      Stream.Write((byte)item.Light);
      Stream.Write((short)item.Hue);
      Stream.Write((byte)item.GetPacketFlags());
    }
  }

  public sealed class WorldItemHS : Packet
  {
    public WorldItemHS(Item item) : base(0xF3, 26)
    {
      Stream.Write((short)0x1);

      var itemID = item.ItemID;

      if (item is BaseMulti)
      {
        Stream.Write((byte)0x02);

        Stream.Write(item.Serial);

        itemID &= 0x3FFF;

        Stream.Write((ushort)itemID);

        Stream.Write((byte)0);
        /*} else if ( ) {
          m_Stream.Write( (byte) 0x01 );

          m_Stream.Write( (int) item.Serial );

          m_Stream.Write( (ushort) itemID );

          m_Stream.Write( (byte) item.Direction );*/
      }
      else
      {
        Stream.Write((byte)0x00);

        Stream.Write(item.Serial);

        itemID &= 0xFFFF;

        Stream.Write((ushort)itemID);

        Stream.Write((byte)0);
      }

      var amount = item.Amount;
      Stream.Write((short)amount);
      Stream.Write((short)amount);

      var loc = item.Location;
      var x = loc.m_X & 0x7FFF;
      var y = loc.m_Y & 0x3FFF;
      Stream.Write((short)x);
      Stream.Write((short)y);
      Stream.Write((sbyte)loc.m_Z);

      Stream.Write((byte)item.Light);
      Stream.Write((short)item.Hue);
      Stream.Write((byte)item.GetPacketFlags());

      Stream.Write((short)0x00); // ??
    }
  }

  public sealed class RemoveEntity : Packet
  {
    public RemoveEntity(IEntity entity) : base(0x1D, 5)
    {
      Stream.Write(entity.Serial);
    }
  }

  public sealed class MultiTargetReqHS : Packet
  {
    public MultiTargetReqHS(MultiTarget t) : base(0x99, 30)
    {
      Stream.Write(t.AllowGround);
      Stream.Write(t.TargetID);
      Stream.Write((byte)t.Flags);

      Stream.Fill();

      Stream.Seek(18, SeekOrigin.Begin);
      Stream.Write((short)t.MultiID);
      Stream.Write((short)t.Offset.X);
      Stream.Write((short)t.Offset.Y);
      Stream.Write((short)t.Offset.Z);

      // DWORD Hue
    }
  }

  public sealed class MultiTargetReq : Packet
  {
    public MultiTargetReq(MultiTarget t) : base(0x99, 26)
    {
      Stream.Write(t.AllowGround);
      Stream.Write(t.TargetID);
      Stream.Write((byte)t.Flags);

      Stream.Fill();

      Stream.Seek(18, SeekOrigin.Begin);
      Stream.Write((short)t.MultiID);
      Stream.Write((short)t.Offset.X);
      Stream.Write((short)t.Offset.Y);
      Stream.Write((short)t.Offset.Z);
    }
  }

  public sealed class CancelTarget : Packet
  {
    public static readonly Packet Instance = SetStatic(new CancelTarget());

    public CancelTarget() : base(0x6C, 19)
    {
      Stream.Write((byte)0);
      Stream.Write(0);
      Stream.Write((byte)3);
      Stream.Fill();
    }
  }

  public sealed class TargetReq : Packet
  {
    public TargetReq(Target t) : base(0x6C, 19)
    {
      Stream.Write(t.AllowGround);
      Stream.Write(t.TargetID);
      Stream.Write((byte)t.Flags);
      Stream.Fill();
    }
  }

  public sealed class DragEffect : Packet
  {
    public DragEffect(IEntity src, IEntity trg, int itemID, int hue, int amount) : base(0x23, 26)
    {
      Stream.Write((short)itemID);
      Stream.Write((byte)0);
      Stream.Write((short)hue);
      Stream.Write((short)amount);
      Stream.Write(src.Serial);
      Stream.Write((short)src.X);
      Stream.Write((short)src.Y);
      Stream.Write((sbyte)src.Z);
      Stream.Write(trg.Serial);
      Stream.Write((short)trg.X);
      Stream.Write((short)trg.Y);
      Stream.Write((sbyte)trg.Z);
    }
  }

  public interface IGumpWriter
  {
    int TextEntries { get; set; }
    int Switches { get; set; }

    void AppendLayout(bool val);
    void AppendLayout(int val);
    void AppendLayout(uint val);
    void AppendLayoutNS(int val);
    void AppendLayout(string text);
    void AppendLayout(byte[] buffer);
    void WriteStrings(List<string> strings);
    void Flush();
  }

  public sealed class DisplayGumpPacked : Packet, IGumpWriter
  {
    private static readonly byte[] m_True = Gump.StringToBuffer(" 1");
    private static readonly byte[] m_False = Gump.StringToBuffer(" 0");

    private static readonly byte[] m_BeginTextSeparator = Gump.StringToBuffer(" @");
    private static readonly byte[] m_EndTextSeparator = Gump.StringToBuffer("@");

    private static readonly byte[] m_Buffer = new byte[48];

    private readonly Gump m_Gump;

    private readonly PacketWriter m_Layout;

    private int m_StringCount;
    private readonly PacketWriter m_Strings;

    static DisplayGumpPacked() => m_Buffer[0] = (byte)' ';

    public DisplayGumpPacked(Gump gump)
      : base(0xDD)
    {
      m_Gump = gump;

      m_Layout = PacketWriter.CreateInstance(8192);
      m_Strings = PacketWriter.CreateInstance(8192);
    }

    public int TextEntries { get; set; }

    public int Switches { get; set; }

    public void AppendLayout(bool val)
    {
      AppendLayout(val ? m_True : m_False);
    }

    public void AppendLayout(int val)
    {
      var toString = val.ToString();
      var bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, m_Buffer, 1) + 1;

      m_Layout.Write(m_Buffer, 0, bytes);
    }

    public void AppendLayout(uint val)
    {
      var toString = val.ToString();
      var bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, m_Buffer, 1) + 1;

      m_Layout.Write(m_Buffer, 0, bytes);
    }

    public void AppendLayoutNS(int val)
    {
      var toString = val.ToString();
      var bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, m_Buffer, 1);

      m_Layout.Write(m_Buffer, 1, bytes);
    }

    public void AppendLayout(string text)
    {
      AppendLayout(m_BeginTextSeparator);

      m_Layout.WriteAsciiFixed(text, text.Length);

      AppendLayout(m_EndTextSeparator);
    }

    public void AppendLayout(byte[] buffer)
    {
      m_Layout.Write(buffer, 0, buffer.Length);
    }

    public void WriteStrings(List<string> strings)
    {
      m_StringCount = strings.Count;

      for (var i = 0; i < strings.Count; ++i)
      {
        var v = strings[i] ?? "";

        m_Strings.Write((ushort)v.Length);
        m_Strings.WriteBigUniFixed(v, v.Length);
      }
    }

    public void Flush()
    {
      EnsureCapacity(28 + (int)m_Layout.Length + (int)m_Strings.Length);

      Stream.Write(m_Gump.Serial);
      Stream.Write(m_Gump.TypeID);
      Stream.Write(m_Gump.X);
      Stream.Write(m_Gump.Y);

      // Note: layout MUST be null terminated (don't listen to krrios)
      m_Layout.Write((byte)0);
      WritePacked(m_Layout);

      Stream.Write(m_StringCount);

      WritePacked(m_Strings);

      PacketWriter.ReleaseInstance(m_Layout);
      PacketWriter.ReleaseInstance(m_Strings);
    }

    private void WritePacked(PacketWriter src)
    {
      var buffer = src.UnderlyingStream.GetBuffer();
      var length = (int)src.Length;

      if (length == 0)
      {
        Stream.Write(0);
        return;
      }

      var wantLength = 1 + buffer.Length * 1024 / 1000;

      wantLength += 4095;
      wantLength &= ~4095;

      var packBuffer = ArrayPool<byte>.Shared.Rent(wantLength);

      var packLength = wantLength;

      ZLib.Pack(packBuffer, ref packLength, buffer, length, ZLibQuality.Default);

      Stream.Write(4 + packLength);
      Stream.Write(length);
      Stream.Write(packBuffer, 0, packLength);

      ArrayPool<byte>.Shared.Return(packBuffer);
    }
  }

  public sealed class DisplayGumpFast : Packet, IGumpWriter
  {
    private static readonly byte[] m_True = Gump.StringToBuffer(" 1");
    private static readonly byte[] m_False = Gump.StringToBuffer(" 0");

    private static readonly byte[] m_BeginTextSeparator = Gump.StringToBuffer(" @");
    private static readonly byte[] m_EndTextSeparator = Gump.StringToBuffer("@");

    private readonly byte[] m_Buffer = new byte[48];
    private int m_LayoutLength;

    public DisplayGumpFast(Gump g) : base(0xB0)
    {
      m_Buffer[0] = (byte)' ';

      EnsureCapacity(4096);

      Stream.Write(g.Serial);
      Stream.Write(g.TypeID);
      Stream.Write(g.X);
      Stream.Write(g.Y);
      Stream.Write((ushort)0xFFFF);
    }

    public int TextEntries { get; set; }

    public int Switches { get; set; }

    public void AppendLayout(bool val)
    {
      AppendLayout(val ? m_True : m_False);
    }

    public void AppendLayout(int val)
    {
      var toString = val.ToString();
      var bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, m_Buffer, 1) + 1;

      Stream.Write(m_Buffer, 0, bytes);
      m_LayoutLength += bytes;
    }

    public void AppendLayout(uint val)
    {
      var toString = val.ToString();
      var bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, m_Buffer, 1) + 1;

      Stream.Write(m_Buffer, 0, bytes);
      m_LayoutLength += bytes;
    }

    public void AppendLayoutNS(int val)
    {
      var toString = val.ToString();
      var bytes = Encoding.ASCII.GetBytes(toString, 0, toString.Length, m_Buffer, 1);

      Stream.Write(m_Buffer, 1, bytes);
      m_LayoutLength += bytes;
    }

    public void AppendLayout(string text)
    {
      AppendLayout(m_BeginTextSeparator);

      var length = text.Length;
      Stream.WriteAsciiFixed(text, length);
      m_LayoutLength += length;

      AppendLayout(m_EndTextSeparator);
    }

    public void AppendLayout(byte[] buffer)
    {
      var length = buffer.Length;
      Stream.Write(buffer, 0, length);
      m_LayoutLength += length;
    }

    public void WriteStrings(List<string> text)
    {
      Stream.Seek(19, SeekOrigin.Begin);
      Stream.Write((ushort)m_LayoutLength);
      Stream.Seek(0, SeekOrigin.End);

      Stream.Write((ushort)text.Count);

      for (var i = 0; i < text.Count; ++i)
      {
        var v = text[i] ?? "";

        int length = (ushort)v.Length;

        Stream.Write((ushort)length);
        Stream.WriteBigUniFixed(v, length);
      }
    }

    public void Flush()
    {
    }
  }

  public sealed class DisplayGump : Packet
  {
    public DisplayGump(Gump g, string layout, string[] text) : base(0xB0)
    {
      layout ??= "";

      EnsureCapacity(256);

      Stream.Write(g.Serial);
      Stream.Write(g.TypeID);
      Stream.Write(g.X);
      Stream.Write(g.Y);
      Stream.Write((ushort)(layout.Length + 1));
      Stream.WriteAsciiNull(layout);

      Stream.Write((ushort)text.Length);

      for (var i = 0; i < text.Length; ++i)
      {
        var v = text[i] ?? "";

        var length = (ushort)v.Length;

        Stream.Write(length);
        Stream.WriteBigUniFixed(v, length);
      }
    }
  }

  public sealed class DisplayPaperdoll : Packet
  {
    public DisplayPaperdoll(Mobile m, string text, bool canLift) : base(0x88, 66)
    {
      byte flags = 0x00;

      if (m.Warmode)
        flags |= 0x01;

      if (canLift)
        flags |= 0x02;

      Stream.Write(m.Serial);
      Stream.WriteAsciiFixed(text, 60);
      Stream.Write(flags);
    }
  }

  public sealed class PopupMessage : Packet
  {
    public PopupMessage(PMMessage msg) : base(0x53, 2)
    {
      Stream.Write((byte)msg);
    }
  }

  public sealed class PlaySound : Packet
  {
    public PlaySound(int soundID, IPoint3D target) : base(0x54, 12)
    {
      Stream.Write((byte)1); // flags
      Stream.Write((short)soundID);
      Stream.Write((short)0); // volume
      Stream.Write((short)target.X);
      Stream.Write((short)target.Y);
      Stream.Write((short)target.Z);
    }
  }

  public sealed class PlayMusic : Packet
  {
    public static readonly Packet InvalidInstance = SetStatic(new PlayMusic(MusicName.Invalid));

    private static readonly Packet[] m_Instances = new Packet[60];

    public PlayMusic(MusicName name) : base(0x6D, 3)
    {
      Stream.Write((short)name);
    }

    public static Packet GetInstance(MusicName name)
    {
      if (name == MusicName.Invalid)
        return InvalidInstance;

      var v = (int)name;
      Packet p;

      if (v >= 0 && v < m_Instances.Length)
      {
        p = m_Instances[v];

        if (p == null)
          m_Instances[v] = p = SetStatic(new PlayMusic(name));
      }
      else
      {
        p = new PlayMusic(name);
      }

      return p;
    }
  }

  public sealed class ScrollMessage : Packet
  {
    public ScrollMessage(int type, int tip, string text) : base(0xA6)
    {
      text ??= "";

      EnsureCapacity(10 + text.Length);

      Stream.Write((byte)type);
      Stream.Write(tip);
      Stream.Write((ushort)text.Length);
      Stream.WriteAsciiFixed(text, text.Length);
    }
  }

  public sealed class CurrentTime : Packet
  {
    public CurrentTime() : base(0x5B, 4)
    {
      var now = DateTime.UtcNow;

      Stream.Write((byte)now.Hour);
      Stream.Write((byte)now.Minute);
      Stream.Write((byte)now.Second);
    }
  }

  public sealed class MapChange : Packet
  {
    public MapChange(Mobile m) : base(0xBF)
    {
      EnsureCapacity(6);

      Stream.Write((short)0x08);
      Stream.Write((byte)(m.Map?.MapID ?? 0));
    }
  }

  public sealed class SeasonChange : Packet
  {
    private static readonly SeasonChange[][] m_Cache = new SeasonChange[][]
    {
      new SeasonChange[2],
      new SeasonChange[2],
      new SeasonChange[2],
      new SeasonChange[2],
      new SeasonChange[2]
    };

    public SeasonChange(int season, bool playSound = true) : base(0xBC, 3)
    {
      Stream.Write((byte)season);
      Stream.Write(playSound);
    }

    public static SeasonChange Instantiate(int season) => Instantiate(season, true);

    public static SeasonChange Instantiate(int season, bool playSound)
    {
      if (season >= 0 && season < m_Cache.Length)
      {
        var idx = playSound ? 1 : 0;

        var p = m_Cache[season][idx];

        if (p == null)
        {
          m_Cache[season][idx] = p = new SeasonChange(season, playSound);
          p.SetStatic();
        }

        return p;
      }

      return new SeasonChange(season, playSound);
    }
  }

  public sealed class SupportedFeatures : Packet
  {
    public SupportedFeatures(NetState ns) : base(0xB9, ns.ExtendedSupportedFeatures ? 5 : 3)
    {
      var flags = ExpansionInfo.CoreExpansion.SupportedFeatures;

      flags |= Value;

      if (ns.Account.Limit >= 6)
      {
        flags |= FeatureFlags.LiveAccount;
        flags &= ~FeatureFlags.UOTD;

        if (ns.Account.Limit > 6)
          flags |= FeatureFlags.SeventhCharacterSlot;
        else
          flags |= FeatureFlags.SixthCharacterSlot;
      }

      if (ns.ExtendedSupportedFeatures)
        Stream.Write((uint)flags);
      else
        Stream.Write((ushort)flags);
    }

    public static FeatureFlags Value { get; set; }

    public static SupportedFeatures Instantiate(NetState ns) => new SupportedFeatures(ns);
  }

  public static class AttributeNormalizer
  {
    public static int Maximum { get; set; } = 25;

    public static bool Enabled { get; set; } = true;

    public static void Write(PacketWriter stream, int cur, int max)
    {
      if (Enabled && max != 0)
      {
        stream.Write((short)Maximum);
        stream.Write((short)(cur * Maximum / max));
      }
      else
      {
        stream.Write((short)max);
        stream.Write((short)cur);
      }
    }

    public static void WriteReverse(PacketWriter stream, int cur, int max)
    {
      if (Enabled && max != 0)
      {
        stream.Write((short)(cur * Maximum / max));
        stream.Write((short)Maximum);
      }
      else
      {
        stream.Write((short)cur);
        stream.Write((short)max);
      }
    }
  }

  public sealed class MobileHits : Packet
  {
    public MobileHits(Mobile m) : base(0xA1, 9)
    {
      Stream.Write(m.Serial);
      Stream.Write((short)m.HitsMax);
      Stream.Write((short)m.Hits);
    }
  }

  public sealed class MobileHitsN : Packet
  {
    public MobileHitsN(Mobile m) : base(0xA1, 9)
    {
      Stream.Write(m.Serial);
      AttributeNormalizer.Write(Stream, m.Hits, m.HitsMax);
    }
  }

  public sealed class MobileMana : Packet
  {
    public MobileMana(Mobile m) : base(0xA2, 9)
    {
      Stream.Write(m.Serial);
      Stream.Write((short)m.ManaMax);
      Stream.Write((short)m.Mana);
    }
  }

  public sealed class MobileManaN : Packet
  {
    public MobileManaN(Mobile m) : base(0xA2, 9)
    {
      Stream.Write(m.Serial);
      AttributeNormalizer.Write(Stream, m.Mana, m.ManaMax);
    }
  }

  public sealed class MobileStam : Packet
  {
    public MobileStam(Mobile m) : base(0xA3, 9)
    {
      Stream.Write(m.Serial);
      Stream.Write((short)m.StamMax);
      Stream.Write((short)m.Stam);
    }
  }

  public sealed class MobileStamN : Packet
  {
    public MobileStamN(Mobile m) : base(0xA3, 9)
    {
      Stream.Write(m.Serial);
      AttributeNormalizer.Write(Stream, m.Stam, m.StamMax);
    }
  }

  public sealed class MobileAttributes : Packet
  {
    public MobileAttributes(Mobile m) : base(0x2D, 17)
    {
      Stream.Write(m.Serial);

      Stream.Write((short)m.HitsMax);
      Stream.Write((short)m.Hits);

      Stream.Write((short)m.ManaMax);
      Stream.Write((short)m.Mana);

      Stream.Write((short)m.StamMax);
      Stream.Write((short)m.Stam);
    }
  }

  public sealed class MobileAttributesN : Packet
  {
    public MobileAttributesN(Mobile m) : base(0x2D, 17)
    {
      Stream.Write(m.Serial);

      AttributeNormalizer.Write(Stream, m.Hits, m.HitsMax);
      AttributeNormalizer.Write(Stream, m.Mana, m.ManaMax);
      AttributeNormalizer.Write(Stream, m.Stam, m.StamMax);
    }
  }

  public sealed class PathfindMessage : Packet
  {
    public PathfindMessage(IPoint3D p) : base(0x38, 7)
    {
      Stream.Write((short)p.X);
      Stream.Write((short)p.Y);
      Stream.Write((short)p.Z);
    }
  }
}
