using System;
using System.Linq;
using System.Collections.Generic;
using Server.Items;

namespace Server.Talent
{
    public class ChemicalWarfare : BaseTalent, ITalent
    {
        public ChemicalWarfare() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeapon = new Type[] { typeof(BaseRanged) };
            RequiredWeaponSkill = SkillName.Archery;
            DisplayName = "Chemical war";
            Description = "Propels potions along with arrows or bolts from backpack.";
            ImageID = 379;
            GumpHeight = 85;
            AddEndY = 65;
            MaxLevel = 1;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return mobile.Skills.Alchemy.Value >= 70.0;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (attacker.Backpack != null && HasSkillRequirement(attacker))
            {
                BasePotion[] potions = Array.ConvertAll(attacker.Backpack.FindItemsByType(typeof(BasePotion), true), item => (BasePotion)item);
                PotionKeg[] kegs = Array.ConvertAll(attacker.Backpack.FindItemsByType(typeof(PotionKeg), true), item => (PotionKeg)item); ;

                if (potions.Length > 0)
                {
                    BasePotion[] harmfulPotions = potions.Where(w => w is BaseConflagrationPotion || w is BaseExplosionPotion || w is BasePoisonPotion || w is BaseConfusionBlastPotion).ToArray();
                    if (harmfulPotions.Length > 0)
                    {
                        BasePotion potion = harmfulPotions[Utility.Random(harmfulPotions.Length)];
                        if (potion is BaseConflagrationPotion)
                        {
                            ((BaseConflagrationPotion)potion).Explode(attacker, target.Location, target.Map);
                        }
                        else if (potion is BaseExplosionPotion)
                        {
                            ((BaseExplosionPotion)potion).Explode(attacker, true, target.Location, target.Map);
                        }
                        else if (potion is BaseConfusionBlastPotion)
                        {
                            ((BaseConfusionBlastPotion)potion).Explode(attacker, target.Location, target.Map);
                        }
                        else if (potion is BasePoisonPotion)
                        {
                            ((BasePoisonPotion)potion).DoPoison(target);
                            potion.Consume();
                        }
                    }
                }
            }
        }
    }
}
