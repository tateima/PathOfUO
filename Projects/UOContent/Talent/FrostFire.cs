using System;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;
using Server.Spells.Third;

namespace Server.Talent
{
    public class FrostFire : BaseTalent
    {
        public FrostFire()
        {
            RequiredSpell = new[]
            {
                typeof(FireballSpell), typeof(FireFieldSpell), typeof(FlameStrikeSpell), typeof(MeteorSwarmSpell),
                typeof(MagicArrowSpell), typeof(ExplosionSpell), typeof(ImmolatingWeaponSpell)
            };
            TalentDependencies = new[] { typeof(SpellMind) };
            DisplayName = "Frost fire";
            Description =
                "Converts Level * 10% of fire spell damage to cold. Slows target. Levels increase potency. Requires either 70+ magery or spell weaving.";
            ImageID = 391;
            GumpHeight = 70;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) =>
            mobile.Skills.Magery.Value >= 70.0 || mobile.Skills.Spellweaving.Value >= 70;

        public override int ModifySpellMultiplier() => Level * 10;

        public override bool CanScaleSpellDamage(Spell spell) => false;

        public void ModifyFireSpell(ref int fire, ref int cold, Mobile target, ref int hue)
        {
            hue = 0x48F;
            var originalFire = fire;
            fire = AOS.Scale(originalFire, 100 - ModifySpellMultiplier());
            cold += AOS.Scale(originalFire, ModifySpellMultiplier());
            if (target != null)
            {
                if (target is BaseCreature creature)
                {
                    SlowCreature(creature, Utility.RandomMinMax(1,3), true);
                }
                else if (target is PlayerMobile targetPlayer)
                {
                    targetPlayer.Slow(Utility.Random(5 + Level * 3));
                }
            }
        }
    }
}
