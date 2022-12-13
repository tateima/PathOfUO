using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class DeathKnightForm : BaseTalent
    {
        public List<ResistanceMod> ResistanceMods { get; set; }
        public List<DefaultSkillMod> DefaultSkillMods { get; set; }
        public List<DefaultSkillMod> OriginalSkillMods { get; set; }

        public List<StatMod> StatMods { get; set; }

        public DeathKnightForm()
        {
            TalentDependencies = new[] { typeof(MasterOfDeath) };
            StatModNames = new[] { "DeathKnightForm" };
            DisplayName = "Death Knight";
            Description = "Assume death knight form.";
            AdditionalDetail = "Allows the user to transform in a powerful death knight for a temporary period of time. Each level reduces cooldown by 15 seconds and increases power by 10%. Requires 80+ necromancy and 75+ tactics";
            ImageID = 429;
            GumpHeight = 70;
            AddEndY = 80;
            CooldownSeconds = 150;
            ManaRequired = 60;
            MaxLevel = 2;
            CanBeUsed = true;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Necromancy.Base >= 80 && mobile.Skills.Tactics.Base > 75;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && from.Mana >= ManaRequired && HasSkillRequirement(from))
            {
                ApplyManaCost(from);
                OnCooldown = true;
                User = (PlayerMobile)from;
                OriginalSkillMods = new List<DefaultSkillMod>();
                int necroModifier = from.Skills.Necromancy.Base > 80.0 ? (int)(from.Skills.Necromancy.Base - 80.0) / 10 : 0;
                int modifier = necroModifier + Level * 10;
                int skillNum = 0;
                foreach (var skill in from.Skills)
                {
                    DefaultSkillMod skillMod = new DefaultSkillMod(
                        skill.SkillName,
                        $"DeathKnightOriginal{skillNum}",
                        false,
                        0
                    );
                    from.AddSkillMod(skillMod);
                    OriginalSkillMods.Add(skillMod);
                }
                from.BodyMod = 147;
                ResistanceMods = new List<ResistanceMod>
                {
                    new (ResistanceType.Physical, "DeathKnightArmor", 75 + modifier),
                    new (ResistanceType.Fire, "DeathKnightFire", 30 + modifier),
                    new (ResistanceType.Cold, "DeathKnightCold", 30 + modifier),
                    new (ResistanceType.Poison, "DeathKnightPoison", 30 + modifier)
                };
                DefaultSkillMods = new List<DefaultSkillMod>
                {
                    new (SkillName.Swords, "DeathKnightSwords", false, 85 + modifier),
                    new (SkillName.Wrestling, "DeathKnightWrestling", false, 85 + modifier),
                    new (SkillName.MagicResist, "DeathKnightMagicResist", false, 85 + modifier),
                    new (SkillName.Healing, "DeathKnightHealing", false, 85 + modifier),
                    new (SkillName.Anatomy, "DeathKnightAnatomy", false, 85 + modifier),
                    new (SkillName.Magery, "DeathKnightMagery", false, 85 + modifier),
                    new (SkillName.EvalInt, "DeathKnightEvalInt", false, 85 + modifier),
                    new (SkillName.Meditation, "DeathKnightMeditation", false, 85 + modifier)
                };
                foreach (var resistanceMod in ResistanceMods)
                {
                    from.AddResistanceMod(resistanceMod);
                }
                foreach (var defaultSkillMod in DefaultSkillMods)
                {
                    from.AddSkillMod(defaultSkillMod);
                }

                int strModifier = 126 + modifier - from.RawStr;
                int dexModifier = 85 + modifier - from.RawDex;
                int intModifier = 95 + modifier - from.RawInt;
                from.AddStatMod(new StatMod(StatType.Str, "DeathKnightStr", strModifier, TimeSpan.FromSeconds(30)));
                from.AddStatMod(new StatMod(StatType.Dex, "DeathKnightDex", dexModifier, TimeSpan.FromSeconds(30)));
                from.AddStatMod(new StatMod(StatType.Int, "DeathKnightInt", intModifier, TimeSpan.FromSeconds(30)));

                Effects.PlaySound(from.Location, from.Map, 0x1FB);
                Effects.SendLocationParticles(
                    EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration),
                    0x37CC,
                    1,
                    40,
                    97,
                    3,
                    9917,
                    0
                );

                Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTransform);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 15), ExpireTalentCooldown, out _talentTimerToken);
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public void ExpireTransform()
        {
            User.BodyMod = 0;
            foreach (var skillMod in DefaultSkillMods)
            {
                User.RemoveSkillMod(skillMod);
            }
            foreach (var originalSkillMod in OriginalSkillMods)
            {
                User.RemoveSkillMod(originalSkillMod);
            }
            foreach (var resistanceMod in ResistanceMods)
            {
                User.RemoveResistanceMod(resistanceMod);
            }
        }
    }
}
