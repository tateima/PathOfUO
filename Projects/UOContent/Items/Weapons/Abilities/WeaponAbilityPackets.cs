using System.Buffers;
using System.Runtime.CompilerServices;
using Server.Network;

namespace Server.Items
{
    public static class WeaponAbilityPackets
    {
        public static unsafe void Configure()
        {
            IncomingPackets.RegisterEncoded(0x19, true, &SetAbility);
        }

        public static void SetAbility(NetState state, IEntity e, EncodedReader reader)
        {
            var m = state.Mobile;
            var index = reader.ReadInt32();

            if (index >= 1 && index < WeaponAbility.Abilities.Length)
            {
                if (m.Mounted && index == 6) //dismount
                {
                    WeaponAbility.ClearCurrentAbility(m);
                    m.SendLocalizedMessage(1061283); // You cannot perform that attack while mounted!
                    return;
                }

                WeaponAbility.SetCurrentAbility(m, WeaponAbility.Abilities[index]);
            }
            else
            {
                WeaponAbility.ClearCurrentAbility(m);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SendClearWeaponAbility(this NetState ns) =>
            ns?.Send(stackalloc byte[] { 0xBF, 0x00, 0x5, 0x00, 0x21 });

        public static void SendToggleSpecialAbility(this NetState ns, int abilityId, bool active)
        {
            if (ns.CannotSendPackets())
            {
                return;
            }

            var writer = new SpanWriter(stackalloc byte[8]);
            writer.Write((byte)0xBF); // Packet ID
            writer.Write((ushort)8);
            writer.Write((short)0x25);
            writer.Write((short)abilityId);
            writer.Write(active);

            ns.Send(writer.Span);
        }
    }
}
