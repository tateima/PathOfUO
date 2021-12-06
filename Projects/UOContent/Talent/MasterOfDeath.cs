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
            TalentDependency = typeof(SummonerCommand);
            HasDeathEffect = true;
            HasKillEffect = true;
            DisplayName = "Master of death";
            Description = "Chance to summon nearby corpses as undead allies killed by player. Requires 85 necromancy.";
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

        public static BaseCreature RandomUndead()
        {
            BaseCreature undead = Utility.RandomMinMax(1, 16) switch
            {
                1  => new AncientLich(),
                2  => new Bogle(),
                3  => new LichLord(),
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
                15 => new RottingCorpse(),
                16 => new Lich(),
                _  => null
            };

            return undead;
        }

        public override void CheckKillEffect(Mobile victim, Mobile killer)
        {
            if (Utility.Random(100) < Level * 2) // max 10%
            {
                var undead = RandomUndead();
                if (undead != null)
                {
                    // steal level % of stats from victim -- the stronger the victim the better the summon
                    undead = TransferMobileStats(victim, undead);

                    if (undead.Backpack != null)
                    {
                        for (var x = undead.Backpack.Items.Count - 1; x >= 0; x--)
                        {
                            var item = undead.Backpack.Items[x];
                            item.Delete();
                        }
                    }

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
