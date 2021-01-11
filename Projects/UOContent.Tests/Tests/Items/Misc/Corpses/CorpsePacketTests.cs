using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Tests;
using Server.Tests.Network;
using Xunit;

namespace UOContent.Tests
{
    public class CorpsePacketTests : IClassFixture<ServerFixture>
    {
        [Fact]
        public void TestCorpseEquipPacket()
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var weapon = new VikingSword();
            m.EquipItem(weapon);

            var c = new Corpse(m, m.Items);

            var expected = new CorpseEquip(m, c).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendCorpseEquip(m, c);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(ProtocolChanges.None)]
        [InlineData(ProtocolChanges.ContainerGridLines)]
        public void TestCorpseContainerPacket(ProtocolChanges changes)
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var weapon = new VikingSword();
            m.EquipItem(weapon);

            var c = new Corpse(m, m.Items);

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.ProtocolChanges = changes;

            var expected = (ns.ContainerGridLines ? (Packet)new CorpseContent6017(m, c) : new CorpseContent(m, c)).Compile();

            ns.SendCorpseContent(m, c);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }
    }
}
