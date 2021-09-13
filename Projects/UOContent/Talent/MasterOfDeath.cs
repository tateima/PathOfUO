using Server.Mobiles;
using Server.Spells;
using Server.Items;
using Server.Gumps;
using System;
using System.Linq;

namespace Server.Talent
{
    public class MasterOfDeath : BaseTalent, ITalent
    {
        public MasterOfDeath() : base()
        {
            BlockedBy = new Type[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(SummonerCommand);
            HasDeathEffect = true;
            HasKillEffect = true;
            DisplayName = "Master of death";
            Description = "Chance to summon nearby corpses as undead allies killed by player.";
            ImageID = 154;
        }
        public BaseCreature TransferMobileStats(Mobile target, BaseCreature destination)
        {
            SkillsGumpGroup[] groups = SkillsGumpGroup.Groups.Where(group => group.Name == "Magical" || group.Name == "Combat Ratings" || group.Name == "Lore & Knowledge").ToArray();
            foreach (SkillsGumpGroup group in groups)
            {
                foreach(SkillName skill in group.Skills)
                {
                    if (target.Skills[skill].Base > 0.0)
                    {
                        // take percentage of their skills
                        destination.Skills[skill].Base += AOS.Scale((int)(target.Skills[skill].Base), Level);
                    }
                }
            }
            destination.RawDex += AOS.Scale(target.RawDex, Level);
            destination.RawInt += AOS.Scale(target.RawInt, Level);
            destination.RawStr += AOS.Scale(target.RawStr, Level);
            return destination;
        }
        public override void CheckKillEffect(Mobile victim, Mobile killer)
        {
            if (Utility.Random(100) < Level)
            {
                BaseCreature undead = null;
                switch (Utility.Random(1, 16))
                {
                    case 1:
                        undead = new AncientLich();
                        break;
                    case 2:
                        undead = new Bogle();
                        break;
                    case 3:
                        undead = new LichLord();
                        break;
                    case 4:
                        undead = new Shade();
                        break;
                    case 5:
                        undead = new Spectre();
                        break;
                    case 6:
                        undead = new Wraith();
                        break;
                    case 7:
                        undead = new BoneKnight();
                        break;
                    case 8:
                        undead = new Ghoul();
                        break;
                    case 9:
                        undead = new Mummy();
                        break;
                    case 10:
                        undead = new SkeletalKnight();
                        break;
                    case 11:
                        undead = new Skeleton();
                        break;
                    case 12:
                        undead = new Zombie();
                        break;
                    case 13:
                        undead = new BoneMagi();
                        break;
                    case 14:
                        undead = new SkeletalMage();
                        break;
                    case 15:
                        undead = new RottingCorpse();
                        break;  
                    case 16:
                        undead = new Lich();
                       break;
                }
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
