using System;
using Server.Items;

namespace Server.Talent
{
    public class GiantsHeritage : BaseTalent
    {
        private Mobile _mobile;

        public GiantsHeritage()
        {
            RequiredWeapon = new[] { typeof(BaseWeapon) };
            StatModNames = new[] { "GiantsHeritage" };
            TalentDependency = typeof(DivineStrength);
            DisplayName = "Giant's Heritage";
            CooldownSeconds = 180;
            CanBeUsed = true;
            Description = "Increases strength per level while active. The more stamina you have the more damage you do.";
            AdditionalDetail =
                "Each level decreases the cooldown by 5 seconds and increases strength by 2 points per level. This buff lasts between 61 and 80 seconds.";
            ImageID = 144;
            AddEndAdditionalDetailsY = 100;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated)
            {
                var extraDamage = (int)(attacker.Stam * SpecialDamageScalar);
                damage += extraDamage;
                attacker.Stam -= 10 + Utility.Random(10);
                if (attacker.Stam < 10)
                {
                    _mobile = attacker;
                    ExpireBuff();
                }
            }
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Stam < 1)
                {
                    from.SendMessage("You cannot use giant's heritage at this time.");
                }
                else
                {
                    _mobile = from;
                    Activated = true;
                    OnCooldown = true;
                    ResetMobileMods(_mobile);
                    _mobile.AddStatMod(new StatMod(StatType.Str, StatModNames[0], Level * 2, TimeSpan.Zero));
                    _mobile.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                    _mobile.PlaySound(0x1AB);
                    Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }

        public void ExpireBuff()
        {
            _mobile?.RemoveStatMod("GiantsHeritage");

            Activated = false;
        }
    }
}
