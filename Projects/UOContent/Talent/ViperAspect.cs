using System;

namespace Server.Talent
{
    public class ViperAspect : BaseTalent
    {
        private Mobile _mobile;

        public ViperAspect()
        {
            BlockedBy = new[] { typeof(DragonAspect) };
            DisplayName = "Viper aspect";
            CanBeUsed = true;
            Description = "Increased poison resistance and adds a chance to poison your target on weapon or spell hit.";
            AdditionalDetail = "Each level increases effects by 2%. When used, increases resistances to poison by 5 points per level for 60-80 seconds.";
            ImageID = 195;
            CooldownSeconds = 180;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            CheckViperEffect(attacker, target);
        }

        public override int ModifySpellMultiplier() => Level * 2;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                ResMod = new ResistanceMod(ResistanceType.Poison, Level * 5);
                _mobile = from;
                OnCooldown = true;
                if (Core.AOS)
                {
                    _mobile.AddResistanceMod(ResMod);
                    _mobile.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                    _mobile.PlaySound(0x205);
                }

                Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public void ExpireBuff()
        {
            if (_mobile != null)
            {
                if (Core.AOS)
                {
                    _mobile.RemoveResistanceMod(ResMod);
                }
            }
        }

        public void CheckViperEffect(Mobile attacker, Mobile target)
        {
            if (Utility.Random(100) < Level * 2)
            {
                target.ApplyPoison(attacker, Poison.GetPoison(Level));
            }
        }

        public override void CheckSpellEffect(Mobile attacker, Mobile target)
        {
            CheckViperEffect(attacker, target);
        }
    }
}
