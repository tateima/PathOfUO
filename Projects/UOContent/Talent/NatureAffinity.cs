using Server.Mobiles;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Mysticism;
using Server.Spells.Spellweaving;
using System;

namespace Server.Talent
{
    public class NatureAffinity : BaseTalent, ITalent
    {
        public NatureAffinity() : base()
        {
            HasDefenseEffect = true;
            RequiredSpell = new Type[] { typeof(LightningSpell), typeof(ChainLightningSpell), typeof(EnergyBoltSpell), typeof(ParalyzeSpell), typeof(ParalyzeFieldSpell), typeof(EagleStrikeSpell), typeof(HailStormSpell), typeof(StoneFormSpell), typeof(ThunderstormSpell), typeof(EssenceOfWindSpell) };
            DisplayName = "Nature affinity";
            Description = "Increases damage done by special moves and nature spells. Wild animals come to your aid in battle.";
            ImageID = 139;
        }

        public override double ModifySpellScalar()
        {
            return (Level / 100);
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile attacker, int damage)
        {
            foreach (Mobile mobile in defender.GetMobilesInRange(Level))
            {
                if (mobile is BaseCreature creature && creature.AI == AIType.AI_Animal)
                {
                    mobile.Attack(attacker);
                }
            }
        }
    }
}
