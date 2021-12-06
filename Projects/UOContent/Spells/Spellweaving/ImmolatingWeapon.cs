using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Talent;

namespace Server.Spells.Spellweaving
{
    public class ImmolatingWeaponSpell : ArcanistSpell
    {
        private static readonly SpellInfo _info = new(
            "Immolating Weapon",
            "Thalshara",
            -1
        );

        private static readonly Dictionary<BaseWeapon, ImmolatingWeaponTimer> _table = new();

        public ImmolatingWeaponSpell(Mobile caster, Item scroll = null)
            : base(caster, scroll, _info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(1.0);

        public override double RequiredSkill => 10.0;
        public override int RequiredMana => 32;

        public override bool CheckCast()
        {
            if (!(Caster.Weapon is BaseWeapon weapon) || weapon is Fists || weapon is BaseRanged)
            {
                Caster.SendLocalizedMessage(1060179); // You must be wielding a weapon to use this ability!
                return false;
            }

            return base.CheckCast();
        }

        public override void OnCast()
        {
            if (!(Caster.Weapon is BaseWeapon weapon) || weapon is Fists || weapon is BaseRanged)
            {
                Caster.SendLocalizedMessage(1060179); // You must be wielding a weapon to use this ability!
            }
            else if (CheckSequence())
            {
                Caster.PlaySound(0x5CA);
                Caster.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);

                if (!IsImmolating(weapon)) // On OSI, the effect is not re-applied
                {
                    var skill = Caster.Skills.Spellweaving.Value;

                    var duration = 10 + (int)(skill / 24) + FocusLevel;
                    var damage = 5 + (int)(skill / 24) + FocusLevel;
                    FireAffinityPower(ref damage);
                    if (CheckFireAffinity())
                    {
                        duration += FireAffinity.Level * 2;
                    } 

                    var t = new ImmolatingWeaponTimer(TimeSpan.FromSeconds(duration), damage, Caster, weapon);
                    _table[weapon] = t;
                    t.Start();

                    weapon.InvalidateProperties();
                }
            }

            FinishSequence();
        }

        public static bool IsImmolating(BaseWeapon weapon) => _table.ContainsKey(weapon);

        public static int GetImmolatingDamage(BaseWeapon weapon) =>
            _table.TryGetValue(weapon, out var entry) ? entry._damage : 0;

        public static void DoEffect(BaseWeapon weapon, Mobile target)
        {
            if (_table.Remove(weapon, out var timer))
            {
                timer.Stop();

                Timer.StartTimer(TimeSpan.FromSeconds(0.25), () => FinishEffect(target, timer));
            }
        }

        private static void FinishEffect(Mobile target, ImmolatingWeaponTimer timer)
        {
            int fire = 100;
            int cold = 0;
            int hue = 0;
            int damage = timer._damage;
            if (timer._caster is PlayerMobile playerCaster) {
                BaseTalent frostFire = playerCaster.GetTalent(typeof(FrostFire));
                if (frostFire != null && fire > 0) {
                    ((FrostFire)frostFire).ModifyFireSpell(ref fire, ref cold, target, hue: ref hue);
                }
            }
            AOS.Damage(target, timer._caster, timer._damage, 0, fire, cold, 0, 0);
        }

        public static void StopImmolating(BaseWeapon weapon)
        {
            if (_table.Remove(weapon, out var timer))
            {
                timer._caster?.PlaySound(0x27);
                timer.Stop();

                weapon.InvalidateProperties();
            }
        }

        private class ImmolatingWeaponTimer : Timer
        {
            public readonly Mobile _caster;
            public readonly int _damage;
            public readonly BaseWeapon _weapon;

            public ImmolatingWeaponTimer(TimeSpan duration, int damage, Mobile caster, BaseWeapon weapon) : base(duration)
            {
                _damage = damage;
                _caster = caster;
                _weapon = weapon;
            }

            protected override void OnTick()
            {
                StopImmolating(_weapon);
            }
        }
    }
}
