using Server.Mobiles;
using Server.Spells.Fifth;
using Server.Spells.Fourth;
using Server.Spells.Mysticism;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Spellweaving;

namespace Server.Talent
{
    public class NatureAffinity : BaseTalent
    {
        public NatureAffinity()
        {
            HasDefenseEffect = true;
            RequiredSpell = new[]
            {
                typeof(LightningSpell), typeof(ChainLightningSpell), typeof(EnergyBoltSpell), typeof(ParalyzeSpell),
                typeof(ParalyzeFieldSpell), typeof(EagleStrikeSpell), typeof(HailStormSpell), typeof(StoneFormSpell),
                typeof(ThunderstormSpell), typeof(EssenceOfWindSpell)
            };
            DisplayName = "Nature affinity";
            Description =
                "Increases damage done by special moves and nature spells. Wild animals come to your aid in battle.";
            AdditionalDetail = $"The damage is increased by 1% per level. {PassiveDetail}";
            ImageID = 139;
        }

        public override double ModifySpellScalar() => Level / 100.0;

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            foreach (var mobile in defender.GetMobilesInRange(Level))
            {
                if (mobile is BaseCreature { AI: AIType.AI_Animal })
                {
                    mobile.Attack(target);
                }
            }
        }
    }
}
