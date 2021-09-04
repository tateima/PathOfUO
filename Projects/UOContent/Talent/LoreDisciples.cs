using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Talent
{
    public class LoreDisciples : BaseTalent, ITalent
    {
        public LoreDisciples() : base()
        {
            TalentDependency = typeof(LoreSeeker);
            CanBeUsed = true;
            MobilePercentagePerPoint = 5;
            DisplayName = "Lore disciples";
            Description = "Summon random humanoids to fight alongside you for 2m (5m cooldown).";
            ImageID = 24035;
        }
        public override void OnUse(Mobile summoner)
        {
            if (!OnCooldown)
            {
                bool canCast = true;
                if (summoner.Mana < 40)
                {
                    canCast = false;
                    summoner.SendMessage("You need 40 mana to call upon your disciples.");
                }
                if (canCast)
                {
                    summoner.Mana -= 40;
                    summoner.RevealingAction();
                    summoner.PublicOverheadMessage(MessageType.Spell, summoner.SpeechHue, true, "Come to my aid, disciples!", false);
                    // its a talent, no need for animation timer, just a single animation is fine
                    summoner.Animate(269, 7, 1, true, false, 0);
                    List<Mobile> disciples = new List<Mobile>();
                    for (int i = 0; i < Level; i++)
                    {
                        BaseCreature disciple = null;
                        BaseTalent loreTeacher = ((PlayerMobile)summoner).GetTalent(typeof(LoreTeacher));
                        double skillIncrease = (loreTeacher != null) ? (double)(loreTeacher.Level * 3) : 0.0;
                        switch (Utility.Random(1, 4))
                        {
                            case 1:
                                disciple = new Brigand();
                                disciple.Skills.Fencing.Base += skillIncrease;
                                disciple.Skills.Macing.Base += skillIncrease;
                                disciple.Skills.MagicResist.Base += skillIncrease;
                                disciple.Skills.Swords.Base += skillIncrease;
                                disciple.Skills.Tactics.Base += skillIncrease;
                                disciple.Skills.Wrestling.Base += skillIncrease;
                                break;
                            case 2:
                                disciple = new EvilMage();
                                break;
                            case 3:
                                disciple = new EvilMageLord();
                                break;
                            case 4:
                                disciple = new EvilHealer();
                                disciple.Skills.Forensics.Base += skillIncrease;
                                disciple.Skills.SpiritSpeak.Base += skillIncrease;
                                disciple.Skills.Swords.Base += skillIncrease;
                                break;
                        }

                        if (disciple is EvilMage || disciple is EvilMageLord)
                        {
                            disciple.Skills.EvalInt.Base += skillIncrease;
                            disciple.Skills.Magery.Base += skillIncrease;
                            disciple.Skills.MagicResist.Base += skillIncrease;
                            disciple.Skills.Tactics.Base += skillIncrease;
                            disciple.Skills.Wrestling.Base += skillIncrease;
                        }

                        if (disciple != null)
                        {
                            disciple = (BaseCreature)ScaleMobileStats(disciple);
                            if (disciple.Backpack != null)
                            {
                                for (var x = disciple.Backpack.Items.Count - 1; x >= 0; x--)
                                {
                                    var item = disciple.Backpack.Items[x];
                                    item.Delete();
                                }
                            }
                            SpellHelper.Summon(disciple, summoner, 0x1FE, TimeSpan.FromMinutes(2), false, false);
                            disciple.Say("I am here to serve thee!");
                            disciple.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                            disciple.PlaySound(0x1FE);
                            disciples.Add(disciple);
                        }
                    }
                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                    summoner.SendMessage("Whom do you wish them to attack?");
                    summoner.Target = new InternalFirstTarget(summoner, disciples);
                }
            }
        }
        private class InternalFirstTarget : Target
        {
            private readonly List<Mobile> m_Disciples;
            private readonly Mobile m_Summoner;
            public InternalFirstTarget(Mobile summoner, List<Mobile> disciples) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Summoner = summoner;
                m_Disciples = disciples;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target, true))
                {
                    for (int i = 0; i < m_Disciples.Count; i++)
                    {
                        m_Disciples[i].Attack(target);
                    }
                }
            }
        }
    }
}
