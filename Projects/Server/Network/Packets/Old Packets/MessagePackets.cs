using System;

namespace Server.Network
{
  public sealed class MessageLocalized : Packet
  {
    private static readonly MessageLocalized[] m_Cache_IntLoc = new MessageLocalized[15000];
    private static readonly MessageLocalized[] m_Cache_CliLoc = new MessageLocalized[100000];
    private static readonly MessageLocalized[] m_Cache_CliLocCmp = new MessageLocalized[5000];

    public MessageLocalized(Serial serial, int graphic, MessageType type, int hue, int font, int number, string name,
      string args) : base(0xC1)
    {
      name ??= "";
      args ??= "";

      if (hue == 0)
        hue = 0x3B2;

      EnsureCapacity(50 + args.Length * 2);

      Stream.Write(serial);
      Stream.Write((short)graphic);
      Stream.Write((byte)type);
      Stream.Write((short)hue);
      Stream.Write((short)font);
      Stream.Write(number);
      Stream.WriteAsciiFixed(name, 30);
      Stream.WriteLittleUniNull(args);
    }

    public static MessageLocalized InstantiateGeneric(int number)
    {
      MessageLocalized[] cache = null;
      var index = 0;

      if (number >= 3000000)
      {
        cache = m_Cache_IntLoc;
        index = number - 3000000;
      }
      else if (number >= 1000000)
      {
        cache = m_Cache_CliLoc;
        index = number - 1000000;
      }
      else if (number >= 500000)
      {
        cache = m_Cache_CliLocCmp;
        index = number - 500000;
      }

      MessageLocalized p;

      if (cache != null && index >= 0 && index < cache.Length)
      {
        p = cache[index];

        if (p == null)
        {
          cache[index] = p = new MessageLocalized(Serial.MinusOne, -1, MessageType.Regular, 0x3B2, 3, number,
            "System", "");
          p.SetStatic();
        }
      }
      else
      {
        p = new MessageLocalized(Serial.MinusOne, -1, MessageType.Regular, 0x3B2, 3, number, "System", "");
      }

      return p;
    }
  }

  [Flags]
  public enum AffixType
  {
    Append = 0x00,
    Prepend = 0x01,
    System = 0x02
  }

  public sealed class MessageLocalizedAffix : Packet
  {
    public MessageLocalizedAffix(Serial serial, int graphic, MessageType messageType, int hue, int font, int number,
      string name, AffixType affixType, string affix, string args) : base(0xCC)
    {
      name ??= "";
      affix ??= "";
      args ??= "";

      if (hue == 0)
        hue = 0x3B2;

      EnsureCapacity(52 + affix.Length + args.Length * 2);

      Stream.Write(serial);
      Stream.Write((short)graphic);
      Stream.Write((byte)messageType);
      Stream.Write((short)hue);
      Stream.Write((short)font);
      Stream.Write(number);
      Stream.Write((byte)affixType);
      Stream.WriteAsciiFixed(name, 30);
      Stream.WriteAsciiNull(affix);
      Stream.WriteBigUniNull(args);
    }
  }

  public sealed class AsciiMessage : Packet
  {
    public AsciiMessage(Serial serial, int graphic, MessageType type, int hue, int font, string name, string text) : base(0x1C)
    {
      name ??= "";
      text ??= "";

      if (hue == 0)
        hue = 0x3B2;

      EnsureCapacity(45 + text.Length);

      Stream.Write(serial);
      Stream.Write((short)graphic);
      Stream.Write((byte)type);
      Stream.Write((short)hue);
      Stream.Write((short)font);
      Stream.WriteAsciiFixed(name, 30);
      Stream.WriteAsciiNull(text);
    }
  }

  public sealed class UnicodeMessage : Packet
  {
    public UnicodeMessage(Serial serial, int graphic, MessageType type, int hue, int font, string lang, string name,
      string text) : base(0xAE)
    {
      if (string.IsNullOrEmpty(lang)) lang = "ENU";
      name ??= "";
      text ??= "";

      if (hue == 0)
        hue = 0x3B2;

      EnsureCapacity(50 + text.Length * 2);

      Stream.Write(serial);
      Stream.Write((short)graphic);
      Stream.Write((byte)type);
      Stream.Write((short)hue);
      Stream.Write((short)font);
      Stream.WriteAsciiFixed(lang, 4);
      Stream.WriteAsciiFixed(name, 30);
      Stream.WriteBigUniNull(text);
    }
  }
}
