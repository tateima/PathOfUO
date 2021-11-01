using Server.Mobiles;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Spellweaving;
using System;

namespace Server.Talent
{
    public class FrostFire : BaseTalent, ITalent
    {
        public FrostFire() : base()
        {
            RequiredSpell = new Type[] { typeof(FireballSpell), typeof(FireFieldSpell), typeof(FlameStrikeSpell), typeof(MeteorSwarmSpell), typeof(MagicArrowSpell), typeof(ExplosionSpell), typeof(ImmolatingWeaponSpell) };
            TalentDependency = typeof(SpellMind);
            DisplayName = "Frostfire";
            Description = "Converts Level * 10% of fire spell damage to cold. Slows target. Levels increase potency.";
            ImageID = 391;
            GumpHeight = 70;
            AddEndY = 90;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return mobile.Skills.Magery.Value >= 70.0 || mobile.Skills.Spellweaving.Value >= 70;
        }

        public override bool CanScaleSpellDamage(Spell spell)
        {
            return false;
        }

        public override int ModifySpellMultiplier()
        {
            return Level * 10;
        }

        public void ModifyFireSpell(ref int fire, ref int cold, Mobile target, ref int hue) {
            hue = MonsterBuff.FrozenHue;
            int originalFire = fire;
            fire = AOS.Scale(originalFire, 100-ModifySpellMultiplier());
            cold += AOS.Scale(originalFire, ModifySpellMultiplier());
            if (target != null) {
                if (target is BaseCreature creature) {
                    creature.ActiveSpeed /= 2;
                } else if (target is PlayerMobile targetPlayer) {
                    targetPlayer.Slow(Utility.Random(5 + (Level * 3)));
                }
            }
        }
    }
}
