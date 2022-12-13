using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;

namespace Server.Talent
{
    public class ChemicalWarfare : BaseTalent
    {
        public ChemicalWarfare()
        {
            TalentDependencies = new[] { typeof(ArcherFocus) };
            RequiredWeapon = new[] { typeof(BaseRanged) };
            RequiredWeaponSkill = SkillName.Archery;
            DisplayName = "Chemical war";
            Description = "Propels potions along with arrows or bolts from backpack. Requires 70+ Alchemy.";
            AdditionalDetail = "The potions are consumed upon successful attack.";
            ManaRequired = 10;
            ImageID = 379;
            GumpHeight = 85;
            AddEndY = 95;
            MaxLevel = 1;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Alchemy.Value >= 70.0;

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (attacker.Backpack != null && HasSkillRequirement(attacker) && attacker.Mana >= ManaRequired)
            {
                var potions = Array.ConvertAll(
                    attacker.Backpack.FindItemsByType(typeof(BasePotion)),
                    item => (BasePotion)item
                );
                if (potions.Length > 0)
                {
                    var harmfulPotions = potions.Where(
                            w => w is BaseConflagrationPotion or BaseExplosionPotion or BasePoisonPotion or BaseConfusionBlastPotion
                        )
                        .ToArray();
                    if (harmfulPotions.Length > 0)
                    {
                        ApplyManaCost(attacker);
                        var potion = harmfulPotions[Utility.Random(harmfulPotions.Length)];
                        if (potion is BaseConflagrationPotion conflagrationPotion)
                        {
                            conflagrationPotion.Users = new List<Mobile> { attacker };
                            conflagrationPotion.Explode(attacker, target.Location, target.Map);
                        }
                        else if (potion is BaseExplosionPotion explosionPotion)
                        {
                            explosionPotion.Users = new HashSet<Mobile> { attacker };
                            explosionPotion.Explode(attacker, true, target.Location, target.Map);
                        }
                        else if (potion is BaseConfusionBlastPotion blastPotion)
                        {
                            blastPotion.Users = new List<Mobile> { attacker };
                            blastPotion.Explode(attacker, target.Location, target.Map);
                        }
                        else if (potion is BasePoisonPotion poisonPotion)
                        {
                            poisonPotion.DoPoison(target);
                            poisonPotion.Consume();
                        }
                    }
                }
            }
        }
    }
}
