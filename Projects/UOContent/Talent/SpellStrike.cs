using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;
using System;

namespace Server.Talent
{
    public class SpellStrike : BaseTalent, ITalent
    {
        public TimerExecutionToken _ActivatedTimerToken;

        private int m_RemainingSpells;
        public int RemainingSpells
        {
            get
            {
                return m_RemainingSpells;
            }
            set
            {
                m_RemainingSpells = value;
            }
        }
        public SpellStrike() : base()
        {
            TalentDependency = typeof(PlanarShift);
            DisplayName = "Spell Strike";
            CanBeUsed = true;
            Description = "Casts 1-10 random magery spell on target on hit. Spell circle increases with level. 2 minute cooldown. Requires 20-100 magery.";
            ImageID = 373;
            AddEndY = 115;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            switch(Level)
            {
                case 0:
                    return mobile.Skills.Magery.Value >= 20.0;
                    break;
                case 1:
                    return mobile.Skills.Magery.Value >= 40.0;
                    break;
                case 2:
                    return mobile.Skills.Magery.Value >= 60.0;
                    break;
                case 3:
                    return mobile.Skills.Magery.Value >= 80.0;
                    break;
                case 4:
                    return mobile.Skills.Magery.Value >= 100.0;
                    break;
                case 5:
                    return true;
                    break;
            }
            return false;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (RemainingSpells > 0 && HasSkillRequirement(attacker) && Activated)
            {
                LightningWand wand = new LightningWand();
                wand.Parent = attacker;
                Spell spell = null;
                switch (Level)
                {
                    case 1:
                        switch (Utility.Random(1, 5))
                        {
                            case 1:
                                spell = new ClumsySpell(attacker, wand);
                                break;
                            case 2:
                                spell = new FeeblemindSpell(attacker, wand);
                                break;
                            case 3:
                                spell = new MagicArrowSpell(attacker, wand);
                                break;
                            case 4:
                                spell = new WeakenSpell(attacker, wand);
                                break;
                            case 5:
                                spell = new HarmSpell(attacker, wand);
                                break;
                        }
                        break;
                    case 2:
                        switch (Utility.Random(1, 4))
                        {
                            case 1:
                                spell = new FireballSpell(attacker, wand);
                                break;
                            case 2:
                                spell = new PoisonSpell(attacker, wand);
                                break;
                            case 3:
                                spell = new LightningSpell(attacker, wand);
                                break;
                            case 4:
                                spell = new FireFieldSpell(attacker, wand);
                                break;
                        }
                        break;
                    case 3:
                        switch (Utility.Random(1, 5))
                        {
                            case 1:
                                spell = new LightningSpell(attacker, wand);
                                break;
                            case 2:
                                spell = new FireFieldSpell(attacker, wand);
                                break;
                            case 3:
                                spell = new ParalyzeSpell(attacker, wand);
                                break;
                            case 4:
                                spell = new PoisonFieldSpell(attacker, wand);
                                break;
                            case 5:
                                spell = new MindBlastSpell(attacker, wand);
                                break;
                        }
                        break;
                    case 4:
                        switch (Utility.Random(1, 4))
                        {
                            case 1:
                                spell = new EnergyBoltSpell(attacker, wand);
                                break;
                            case 2:
                                spell = new ExplosionSpell(attacker, wand);
                                break;
                            case 3:
                                spell = new ParalyzeFieldSpell(attacker, wand);
                                break;
                            case 4:
                                spell = new PoisonFieldSpell(attacker, wand);
                                break;
                        }
                        break;
                    case 5:
                        switch (Utility.Random(1, 5))
                        {
                            case 1:
                                spell = new ChainLightningSpell(attacker, wand);
                                break;
                            case 2:
                                spell = new FlameStrikeSpell(attacker, wand);
                                break;
                            case 3:
                                spell = new MeteorSwarmSpell(attacker, wand);
                                break;
                            case 4:
                                spell = new EnergyBoltSpell(attacker, wand);
                                break;
                            case 5:
                                spell = new ExplosionSpell(attacker, wand);
                                break;
                        }
                        break;
                }
                if (spell != null)
                {
                    spell.Cast();
                    RemainingSpells--;
                    wand.Delete();
                }
            } else
            {
                Activated = false;
                OnCooldown = true;
                attacker.SendMessage("Your weapon loses its magery abilities");
                Timer.StartTimer(TimeSpan.FromMinutes(2), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override void OnUse(Mobile mobile)
        {
            if (mobile.Mana < 40)
            {
                mobile.SendMessage("You require 40 mana to imbue your weapon with magery force.");
            }
            else if (!Activated && !OnCooldown)
            {
                Activated = true;
                mobile.Mana -= 40;
                RemainingSpells = Level + Utility.Random(1, Level);
                mobile.PlaySound(0x1E9);
                mobile.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
            }
        }
    }
}

