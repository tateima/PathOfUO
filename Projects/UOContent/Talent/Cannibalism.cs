using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class Cannibalism : BaseTalent
    {
        public Cannibalism()
        {
            MaxLevel = 2;
            TalentDependencies = new[] { typeof(BondingMaster) };
            DisplayName = "Cannibalise pet";
            CanBeUsed = true;
            Description = "Sacrifice another tamed animal, transferring some stats and kills to a target creature.";
            AdditionalDetail = "You may only sacrifice a maximum of 3 tamed creatures to your chosen target. The transfer increases by 1% per level, per stack of cannibalism.";
            ImageID = 376;
            AddEndAdditionalDetailsY = 70;
        }

        public override void OnUse(Mobile from)
        {
            from.SendMessage("Which pet do you wish to improve?");
            from.Target = new InternalFirstTarget(Level);
        }

        private class InternalFirstTarget : Target
        {
            private readonly int _level;

            public InternalFirstTarget(int level) : base(
                3,
                false,
                TargetFlags.None
            ) =>
                _level = level;

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature { ControlMaster: { } } creature && creature.ControlMaster == @from && creature.Controlled && creature.CannibalPoints < 3)
                {
                    from.SendMessage("Wish pet do you wish to sacrifice?");
                    from.Target = new InternalSecondTarget(creature, _level);
                }
                else
                {
                    from.SendMessage("You cannot cannibalise with that target.");
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private BaseCreature _cannibalCreature;
            private readonly int _level;

            public InternalSecondTarget(Mobile cannibalCreature, int level) : base(
                3,
                false,
                TargetFlags.None
            )
            {
                _level = level;
                _cannibalCreature = (BaseCreature)cannibalCreature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is BaseCreature { ControlMaster: { } } creature && creature.ControlMaster == @from && creature.Controlled && _cannibalCreature.CanCannibalise(creature) && creature.CannibalPoints == 0)
                {
                    _cannibalCreature = TransferMobileStats(creature, _cannibalCreature);
                    _cannibalCreature.CannibalPoints += 1;
                    creature.Kill();
                }
                else
                {
                    from.SendMessage("You cannot cannibalise that target with your last.");
                }
            }

            public BaseCreature TransferMobileStats(Mobile target, BaseCreature destination)
            {
                int modifier = destination.CannibalPoints + 1;
                int dexModifier = AOS.Scale(target.RawDex, _level * modifier);
                if (dexModifier < 1)
                {
                    dexModifier = 1;
                }
                destination.RawDex += dexModifier;
                int intModifier = AOS.Scale(target.RawInt, _level * modifier);
                if (intModifier < 1)
                {
                    intModifier = 1;
                }
                destination.RawInt += intModifier;
                int strModifier = AOS.Scale(target.RawStr, _level * modifier);
                if (strModifier < 1)
                {
                    strModifier = 1;
                }
                destination.RawStr += strModifier;
                int hitsModifier = AOS.Scale(target.HitsMax, _level * modifier);
                if (hitsModifier < 1)
                {
                    hitsModifier = 1;
                }
                destination.SetHits(destination.HitsMax + hitsModifier);
                int manaModifier = AOS.Scale(target.ManaMax, _level * modifier);
                if (manaModifier < 1)
                {
                    manaModifier = 1;
                }
                destination.SetMana(destination.ManaMax + manaModifier);
                int stamModifier = AOS.Scale(target.ManaMax, _level * modifier);
                if (stamModifier < 1)
                {
                    stamModifier = 1;
                }
                destination.SetStam(destination.StamMax + stamModifier);
                List<Skill> skills = new List<Skill>();
                GetTopSkills(target, ref skills, 3);
                foreach (var skill in skills)
                {
                    if (destination.Skills[skill.SkillName].Base < 120)
                    {
                        double skillModifier = target.Skills[skill.SkillName].Base/100.0 * (_level * (modifier * 1.0));
                        if (skillModifier < 1.0)
                        {
                            skillModifier = 1.0;
                        }

                        destination.Skills[skill.SkillName].Base += skillModifier;
                        if (destination.Skills[skill.SkillName].Base > 120.0)
                        {
                            destination.Skills[skill.SkillName].Base = 120.0;
                        }
                    }
                }

                return destination;
            }
        }
    }
}
