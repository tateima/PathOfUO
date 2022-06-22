using System;

namespace Server.Talent
{
    public class IronSkin : BaseTalent
    {
        private Mobile _mobile;

        public IronSkin()
        {
            TalentDependency = typeof(GiantsHeritage);
            DisplayName = "Iron skin";
            CanBeUsed = true;
            Description = "Increases physical resistance on use.";
            AdditionalDetail =
                "The resistance buff lasts between 60 and 80 seconds. Each level decreases cooldown by 5 seconds and increases the resistance by 5%.";
            AddEndAdditionalDetailsY = 100;
            CooldownSeconds = 180;
            ManaRequired = 10;
            ImageID = 119;
            GumpHeight = 70;
            AddEndY = 65;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"You require {ManaRequired.ToString()} mana to harden your skin.");
                }
                else
                {
                    ResMod = new ResistanceMod(ResistanceType.Physical, Level * 5);
                    _mobile = from;
                    OnCooldown = true;
                    if (Core.AOS)
                    {
                        _mobile.AddResistanceMod(ResMod);
                        _mobile.FixedParticles(0x373A, 10, 15, 5021, EffectLayer.Waist);
                        _mobile.PlaySound(0x63B);
                    }

                    Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
                }
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
    }
}
