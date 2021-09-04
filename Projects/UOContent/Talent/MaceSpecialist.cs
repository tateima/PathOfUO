using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class MaceSpecialist : BaseTalent, ITalent
    {
        public MaceSpecialist() : base()
        {
            BlockedBy = new Type[] { typeof(TwoHandedMaceSpecialist) };
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new Type[] { typeof(Mace), typeof(Maul), typeof(Club), typeof(DiamondMace), typeof(MagicWand), typeof(HammerPick), typeof(Scepter), typeof(WarMace) };
            RequiredWeaponSkill = SkillName.Macing;
            DisplayName = "Mace specialist";
            Description = "Increases damage to one handed macefighting weapons.";
            ImageID = 30226;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }


    }
}
