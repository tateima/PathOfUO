using System;
using System.Linq;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Talent
{
    public class MasterOfDeath : BaseTalent
    {
        public MasterOfDeath()
        {
            BlockedBy = new[] { typeof(GreaterFireElemental) };
            TalentDependencies = new[] { typeof(SummonerCommand) };
            HasDeathEffect = true;
            HasKillEffect = true;
            DisplayName = "Master of death";
            Description = "Chance to summon nearby corpses as undead allies killed by player. Requires 85 necromancy.";
            AdditionalDetail = "The chance increases by 5% per level and will strengthen the risen by 1%. These risen will summon for 2 minutes.";
            ImageID = 154;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Necromancy].Base >= 85;

        public BaseCreature TransferMobileStats(Mobile target, BaseCreature destination)
        {
            var groups = SkillsGumpGroup.Groups.Where(
                    group => group.Name is "Magical" or "Combat Ratings" or "Lore & Knowledge"
                )
                .ToArray();
            foreach (var group in groups)
            {
                foreach (var skill in group.Skills)
                {
                    if (target.Skills[skill].Base > 0.0)
                    {
                        // take percentage of their skills
                        destination.Skills[skill].Base += AOS.Scale((int)target.Skills[skill].Base, Level);
                    }
                }
            }

            destination.RawDex += AOS.Scale(target.RawDex, Level);
            destination.RawInt += AOS.Scale(target.RawInt, Level);
            destination.RawStr += AOS.Scale(target.RawStr, Level);
            return destination;
        }

        public static BaseCreature RandomUndead(bool meleeOnly = false)
        {
            BaseCreature undead;
            if (meleeOnly)
            {
                undead = Utility.RandomMinMax(1, 8) switch
                {
                    1  => new Spectre(),
                    2  => new BoneKnight(),
                    3  => new Ghoul(),
                    4  => new Mummy(),
                    5  => new SkeletalKnight(),
                    6  => new Skeleton(),
                    7  => new Zombie(),
                    8  => new RottingCorpse(),
                    _  => null
                };
            }
            else
            {
                undead = Utility.RandomMinMax(1, 16) switch
                {
                    2  => new Bogle(),
                    4  => new Shade(),
                    5  => new Spectre(),
                    6  => new Wraith(),
                    7  => new BoneKnight(),
                    8  => new Ghoul(),
                    9  => new Mummy(),
                    10 => new SkeletalKnight(),
                    11 => new Skeleton(),
                    12 => new Zombie(),
                    13 => new BoneMagi(),
                    14 => new SkeletalMage(),
                    _  => null
                };
            }
            return undead;
        }

        public override void CheckKillEffect(Mobile victim, Mobile killer)
        {
            if (Utility.Random(100) < Level * 5 && HasSkillRequirement(killer))
            {
                var undead = RandomUndead();
                if (undead != null)
                {
                    // steal level % of stats from victim -- the stronger the victim the better the summon
                    undead = TransferMobileStats(victim, undead);

                    EmptyCreatureBackpack(undead);
                    undead.SetLevel();
                    SpellHelper.Summon(undead, killer, 0x1FE, TimeSpan.FromMinutes(2), false, false);
                    undead.Say("Master...");
                    Effects.PlaySound(killer.Location, killer.Map, 0x1FB);
                    Effects.SendLocationParticles(
                        EffectItem.Create(undead.Location, undead.Map, EffectItem.DefaultDuration),
                        0x37CC,
                        1,
                        40,
                        97,
                        3,
                        9917,
                        0
                    );
                }
            }
        }
    }
}
