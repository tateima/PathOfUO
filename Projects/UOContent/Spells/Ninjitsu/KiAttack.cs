using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Spells.Ninjitsu
{
    public class KiAttack : NinjaMove
    {
        private static readonly Dictionary<Mobile, KiAttackTimer> m_Table = new();

        public override int BaseMana => 25;
        public override double RequiredSkill => 80.0;

        public override TextDefinition AbilityMessage =>
            new(1063099); // Your Ki Attack must be complete within 2 seconds for the damage bonus!

        public override void OnUse(Mobile from)
        {
            if (!Validate(from))
            {
                return;
            }

            var t = new KiAttackTimer(from);
            m_Table[from] = t;
            t.Start();
        }

        public override bool Validate(Mobile from)
        {
            if (from.Hidden && from.AllowedStealthSteps > 0)
            {
                from.SendLocalizedMessage(1063127); // You cannot use this ability while in stealth mode.
                return false;
            }

            if (Core.ML && from.Weapon is BaseRanged)
            {
                from.SendLocalizedMessage(1075858); // You can only use this with melee attacks.
                return false;
            }

            return base.Validate(from);
        }

        public override double GetDamageScalar(Mobile attacker, Mobile defender)
        {
            if (attacker.Hidden)
            {
                return 1.0;
            }

            /*
             * Pub40 changed pvp damage max to 55%
             */

            return 1.0 + GetBonus(attacker) / (Core.ML && attacker.Player && defender.Player ? 40 : 10);
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!Validate(attacker) || !CheckMana(attacker, true))
            {
                return;
            }

            if (GetBonus(attacker) == 0.0)
            {
                attacker.SendLocalizedMessage(1063101); // You were too close to your target to cause any additional damage.
            }
            else
            {
                attacker.FixedParticles(0x37BE, 1, 5, 0x26BD, 0x0, 0x1, EffectLayer.Waist);
                attacker.PlaySound(0x510);

                attacker.SendLocalizedMessage(
                    1063100
                ); // Your quick flight to your target causes extra damage as you strike!
                defender.FixedParticles(0x37BE, 1, 5, 0x26BD, 0, 0x1, EffectLayer.Waist);

                CheckGain(attacker);
            }

            ClearCurrentMove(attacker);
        }

        public override void OnClearMove(Mobile from)
        {
            if (m_Table.Remove(from, out var t))
            {
                t.Stop();
            }
        }

        public static double GetBonus(Mobile from)
        {
            if (!m_Table.TryGetValue(from, out var t))
            {
                return 0;
            }

            var xDelta = t.m_Location.X - from.X;
            var yDelta = t.m_Location.Y - from.Y;

            return Math.Min(Math.Sqrt(xDelta * xDelta + yDelta * yDelta), 20.0);
        }

        private class KiAttackTimer : Timer
        {
            public readonly Mobile m_Mobile;
            public Point3D m_Location;

            public KiAttackTimer(Mobile m) : base(TimeSpan.FromSeconds(2.0))
            {
                m_Mobile = m;
                m_Location = m.Location;
            }

            protected override void OnTick()
            {
                ClearCurrentMove(m_Mobile);
                m_Mobile.SendLocalizedMessage(1063102); // You failed to complete your Ki Attack in time.

                m_Table.Remove(m_Mobile);
            }
        }
    }
}
