using System.IO;

namespace Server.Network
{
  public sealed class DisplaySpellbook : Packet
  {
    public DisplaySpellbook(Item book) : base(0x24, 7)
    {
      Stream.Write(book.Serial);
      Stream.Write((short)-1);
    }
  }

  public sealed class DisplaySpellbookHS : Packet
  {
    public DisplaySpellbookHS(Item book) : base(0x24, 9)
    {
      Stream.Write(book.Serial);
      Stream.Write((short)-1);
      Stream.Write((short)0x7D);
    }
  }

  public sealed class NewSpellbookContent : Packet
  {
    public NewSpellbookContent(Item item, int graphic, int offset, ulong content) : base(0xBF)
    {
      EnsureCapacity(23);

      Stream.Write((short)0x1B);
      Stream.Write((short)0x01);

      Stream.Write(item.Serial);
      Stream.Write((short)graphic);
      Stream.Write((short)offset);

      for (var i = 0; i < 8; ++i)
        Stream.Write((byte)(content >> (i * 8)));
    }
  }

  public sealed class SpellbookContent : Packet
  {
    public SpellbookContent(int count, int offset, ulong content, Item item) : base(0x3C)
    {
      EnsureCapacity(5 + count * 19);

      var written = 0;

      Stream.Write((ushort)0);

      ulong mask = 1;

      for (var i = 0; i < 64; ++i, mask <<= 1)
        if ((content & mask) != 0)
        {
          Stream.Write(0x7FFFFFFF - i);
          Stream.Write((ushort)0);
          Stream.Write((byte)0);
          Stream.Write((ushort)(i + offset));
          Stream.Write((short)0);
          Stream.Write((short)0);
          Stream.Write(item.Serial);
          Stream.Write((short)0);

          ++written;
        }

      Stream.Seek(3, SeekOrigin.Begin);
      Stream.Write((ushort)written);
    }
  }

  public sealed class SpellbookContent6017 : Packet
  {
    public SpellbookContent6017(int count, int offset, ulong content, Item item) : base(0x3C)
    {
      EnsureCapacity(5 + count * 20);

      var written = 0;

      Stream.Write((ushort)0);

      ulong mask = 1;

      for (var i = 0; i < 64; ++i, mask <<= 1)
        if ((content & mask) != 0)
        {
          Stream.Write(0x7FFFFFFF - i);
          Stream.Write((ushort)0);
          Stream.Write((byte)0);
          Stream.Write((ushort)(i + offset));
          Stream.Write((short)0);
          Stream.Write((short)0);
          Stream.Write((byte)0); // Grid Location?
          Stream.Write(item.Serial);
          Stream.Write((short)0);

          ++written;
        }

      Stream.Seek(3, SeekOrigin.Begin);
      Stream.Write((ushort)written);
    }
  }
}
