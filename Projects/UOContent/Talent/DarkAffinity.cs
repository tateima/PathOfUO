using System;
using Server.Spells.Mysticism;
using Server.Spells.Necromancy;
using Server.Spells.Spellweaving;

namespace Server.Talent
{
    public class DarkAffinity : BaseTalent
    {
        private Mobile _mobile;
        private ResistanceMod _resistanceMod;

        public DarkAffinity()
        {
            CanBeUsed = true;
            BlockedBy = new[] { typeof(LightAffinity) };
            RequiredSpell = new[] { typeof(NecromancerSpell), typeof(SpellPlagueSpell), typeof(WordOfDeathSpell), typeof(NetherCycloneSpell) };
            DisplayName = "Dark affinity";
            Description = "Enhances damage and strength of dark arts and spells. Requires 50+ Necromancy.";
            AdditionalDetail = "Each level increases the effect by 2%. When used, increases resistances to cold by 5 points per level for 60-80 seconds. Spell weavers are not blocked on this talent.";
            CooldownSeconds = 180;
            ImageID = 129;
            GumpHeight = 95;
            AddEndY = 80;
            AddEndAdditionalDetailsY = 100;
        }


        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Necromancy].Base >= 50;

        public override double ModifySpellScalar() => Level / 100.0 * 2;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                _resistanceMod = new ResistanceMod(ResistanceType.Cold, Level * 5);
                _mobile = from;
                OnCooldown = true;
                _mobile.AddResistanceMod(_resistanceMod);
                if (Core.AOS)
                {
                    from.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                    from.PlaySound(0x0FC);
                }
                else
                {
                    from.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                    from.PlaySound(0x1F1);
                }

                Timer.StartTimer(TimeSpan.FromSeconds(60 + Utility.Random(20)), ExpireBuff, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 5), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public void ExpireBuff()
        {
            _mobile?.RemoveResistanceMod(_resistanceMod);
        }

        public override bool IgnoreTalentBlock(Mobile mobile) => mobile.Skills.Spellweaving.Value > 0.0;
    }
}
