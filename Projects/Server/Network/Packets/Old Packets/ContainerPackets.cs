using System;
using System.IO;
using Server.Items;

namespace Server.Network
{
  public sealed class ContainerDisplay : Packet
  {
    public ContainerDisplay(Container c) : base(0x24, 7)
    {
      Stream.Write(c.Serial);
      Stream.Write((short)c.GumpID);
    }
  }

  public sealed class ContainerDisplayHS : Packet
  {
    public ContainerDisplayHS(Container c) : base(0x24, 9)
    {
      Stream.Write(c.Serial);
      Stream.Write((short)c.GumpID);
      Stream.Write((short)0x7D);
    }
  }

  public sealed class ContainerContentUpdate : Packet
  {
    public ContainerContentUpdate(Item item) : base(0x25, 20)
    {
      Serial parentSerial;

      if (item.Parent is Item parentItem)
      {
        parentSerial = parentItem.Serial;
      }
      else
      {
        Console.WriteLine("Warning: ContainerContentUpdate on item with !(parent is Item)");
        parentSerial = Serial.Zero;
      }

      Stream.Write(item.Serial);
      Stream.Write((ushort)item.ItemID);
      Stream.Write((byte)0); // signed, itemID offset
      Stream.Write((ushort)Math.Min(item.Amount, ushort.MaxValue));
      Stream.Write((short)item.X);
      Stream.Write((short)item.Y);
      Stream.Write(parentSerial);
      Stream.Write((ushort)(item.QuestItem ? Item.QuestItemHue : item.Hue));
    }
  }

  public sealed class ContainerContentUpdate6017 : Packet
  {
    public ContainerContentUpdate6017(Item item) : base(0x25, 21)
    {
      Serial parentSerial;

      if (item.Parent is Item parentItem)
      {
        parentSerial = parentItem.Serial;
      }
      else
      {
        Console.WriteLine("Warning: ContainerContentUpdate on item with !(parent is Item)");
        parentSerial = Serial.Zero;
      }

      Stream.Write(item.Serial);
      Stream.Write((ushort)item.ItemID);
      Stream.Write((byte)0); // signed, itemID offset
      Stream.Write((ushort)Math.Min(item.Amount, ushort.MaxValue));
      Stream.Write((short)item.X);
      Stream.Write((short)item.Y);
      Stream.Write((byte)0); // Grid Location?
      Stream.Write(parentSerial);
      Stream.Write((ushort)(item.QuestItem ? Item.QuestItemHue : item.Hue));
    }
  }

  public sealed class ContainerContent : Packet
  {
    public ContainerContent(Mobile beholder, Item beheld) : base(0x3C)
    {
      var items = beheld.Items;
      var count = items.Count;

      EnsureCapacity(5 + count * 19);

      var pos = Stream.Position;

      var written = 0;

      Stream.Write((ushort)0);

      for (var i = 0; i < count; ++i)
      {
        var child = items[i];

        if (!child.Deleted && beholder.CanSee(child))
        {
          var loc = child.Location;

          Stream.Write(child.Serial);
          Stream.Write((ushort)child.ItemID);
          Stream.Write((byte)0); // signed, itemID offset
          Stream.Write((ushort)Math.Min(child.Amount, ushort.MaxValue));
          Stream.Write((short)loc.m_X);
          Stream.Write((short)loc.m_Y);
          Stream.Write(beheld.Serial);
          Stream.Write((ushort)(child.QuestItem ? Item.QuestItemHue : child.Hue));

          ++written;
        }
      }

      Stream.Seek(pos, SeekOrigin.Begin);
      Stream.Write((ushort)written);
    }
  }

  public sealed class ContainerContent6017 : Packet
  {
    public ContainerContent6017(Mobile beholder, Item beheld) : base(0x3C)
    {
      var items = beheld.Items;
      var count = items.Count;

      EnsureCapacity(5 + count * 20);

      var pos = Stream.Position;

      var written = 0;

      Stream.Write((ushort)0);

      for (var i = 0; i < count; ++i)
      {
        var child = items[i];

        if (!child.Deleted && beholder.CanSee(child))
        {
          var loc = child.Location;

          Stream.Write(child.Serial);
          Stream.Write((ushort)child.ItemID);
          Stream.Write((byte)0); // signed, itemID offset
          Stream.Write((ushort)Math.Min(child.Amount, ushort.MaxValue));
          Stream.Write((short)loc.m_X);
          Stream.Write((short)loc.m_Y);
          Stream.Write((byte)0); // Grid Location?
          Stream.Write(beheld.Serial);
          Stream.Write((ushort)(child.QuestItem ? Item.QuestItemHue : child.Hue));

          ++written;
        }
      }

      Stream.Seek(pos, SeekOrigin.Begin);
      Stream.Write((ushort)written);
    }
  }
}
