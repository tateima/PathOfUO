using System;
using Server.Mobiles;
using Server.Pantheon;
using Server.Spells.Fifth;
using Server.Spells.Fourth;
using Server.Spells.Mysticism;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;
using Server.Targeting;

namespace Server.Talent
{
    public class NatureAffinity : BaseTalent
    {
        public NatureAffinity()
        {
            CanBeUsed = true;
            MaxLevel = 3;
            CooldownSeconds = 120;
            OnCooldown = false;
            RequiredSpell = new[]
            {
                typeof(LightningSpell), typeof(ChainLightningSpell), typeof(EnergyBoltSpell), typeof(ParalyzeSpell),
                typeof(ParalyzeFieldSpell), typeof(EagleStrikeSpell), typeof(HailStormSpell), typeof(StoneFormSpell),
                typeof(ThunderstormSpell), typeof(EssenceOfWindSpell)
            };
            DisplayName = "Nature affinity";
            Description =
                "Increases damage done by special moves, tamed creatures and nature spells. Incite wild animals to aid you in battle.";
            AdditionalDetail = $"Each level will allow stronger animals to aid you.  Tamed creatures will also gain a damage reduction of equivalent value. Natural damage sources increases by 1% per level.";
            ImageID = 139;
        }

        public override double ModifySpellScalar() => Level / 100.0;

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.AnimalLore.Base >= 25;

        public override void OnUse(Mobile from)
        {
            if (HasSkillRequirement(from))
            {
                from.SendMessage("What wild animal do you wish to incite?");
                from.Target = new InternalTarget(this);
            }
            else
            {
                from.SendMessage("You don't seem to have the right skill to use this talent.");
            }
        }

        private class InternalTarget : Target
        {
            private readonly NatureAffinity _talent;

            public InternalTarget(NatureAffinity talent) : base(
                8,
                false,
                TargetFlags.None
            ) =>
                _talent = talent;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature { AI: AIType.AI_Animal, ControlMaster: null } creature)
                {
                    if (from.CheckSkill(SkillName.AnimalLore, 0.0, 100.0))
                    {
                        if (
                            creature.IsHeroic && _talent.Level < 3
                            ||
                            creature.IsVeteran && _talent.Level < 2
                            ||
                            creature.HasMonsterSpecial
                        )
                        {
                            from.SendMessage("This animal is too strong for you.");
                        }
                        else
                        {
                            from.SendMessage("What do you wish to incite this wild animal to attack?");
                            from.Target = new InternalSecondTarget(creature, _talent);
                        }
                    }
                    else
                    {
                        from.SendMessage("You fail to connect with the creature.");
                    }

                }
                else
                {
                    from.SendMessage("That is not an animal.");
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private readonly BaseCreature _wildAnimal;
            private readonly NatureAffinity _talent;

            public InternalSecondTarget(BaseCreature creature, NatureAffinity talent) : base(
                8,
                false,
                TargetFlags.None
            ) {
                _wildAnimal = creature;
                _talent = talent;
            }


            protected override void OnTarget(Mobile from, object targeted)
            {

                if (targeted is Mobile mobile && mobile != from && _wildAnimal.CanBeHarmful(mobile) &&
                    (
                        mobile is BaseCreature baseCreature && !Deity.AlignmentCheck(_wildAnimal, Deity.GetCreatureAlignment(baseCreature.GetType()), false)
                        ||
                        mobile is PlayerMobile playerMobile && !Deity.AlignmentCheck(_wildAnimal, playerMobile.Alignment, false)
                    )
                   )
                {
                    if (from.Combatant == mobile)
                    {
                        from.SendMessage("The wild creature becomes angry towards your target!");
                        _wildAnimal.Combatant = mobile;
                        _talent.OnCooldown = true;
                        Timer.StartTimer(TimeSpan.FromSeconds(_talent.CooldownSeconds), _talent.ExpireTalentCooldown, out _talent._talentTimerToken);
                    }
                    else
                    {
                        from.SendMessage("The creature feels ambivalent towards your target.");
                    }
                }
                else
                {
                    from.SendMessage("You cannot target that.");
                }
            }
        }
    }
}
