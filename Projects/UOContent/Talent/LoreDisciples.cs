using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class LoreDisciples : BaseTalent
    {
        public LoreDisciples()
        {
            TalentDependency = typeof(LoreSeeker);
            CanBeUsed = true;
            DisplayName = "Lore disciples";
            Description = "Summon random humanoids to fight alongside you for 2m (5m cooldown).";
            MaxLevel = 4;
            ImageID = 158;
            GumpHeight = 75;
            AddEndY = 90;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var canCast = true;
                if (from.Mana < 40)
                {
                    canCast = false;
                    from.SendMessage("You need 40 mana to call upon your disciples.");
                }

                if (canCast)
                {
                    from.Mana -= 40;
                    from.RevealingAction();
                    from.PublicOverheadMessage(
                        MessageType.Spell,
                        from.SpeechHue,
                        true,
                        "Come to my aid, disciples!",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);
                    var disciples = new List<Mobile>();
                    for (var i = 0; i < Level; i++)
                    {
                        BaseCreature disciple = null;
                        var loreTeacher = ((PlayerMobile)from).GetTalent(typeof(LoreTeacher));
                        MobilePercentagePerPoint += loreTeacher?.Level > Level ? loreTeacher.Level : 0;
                        var skillIncrease = loreTeacher != null ? loreTeacher.Level * 2 : 0.0;
                        switch (Utility.RandomMinMax(1, 6))
                        {
                            case 1:
                            case 2:
                            case 3:
                                disciple = new Brigand();
                                disciple.Skills.Fencing.Base += skillIncrease;
                                disciple.Skills.Archery.Base += skillIncrease;
                                disciple.Skills.Macing.Base += skillIncrease;
                                disciple.Skills.MagicResist.Base += skillIncrease;
                                disciple.Skills.Swords.Base += skillIncrease;
                                disciple.Skills.Tactics.Base += skillIncrease;
                                disciple.Skills.Wrestling.Base += skillIncrease;
                                break;
                            case 4:
                            case 5:
                                disciple = new EvilMage();
                                break;
                            case 6:
                                disciple = new EvilHealer();
                                disciple.Skills.Forensics.Base += skillIncrease;
                                disciple.Skills.SpiritSpeak.Base += skillIncrease;
                                disciple.Skills.Swords.Base += skillIncrease;
                                break;
                        }

                        if (disciple is EvilMage)
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
                            SpellHelper.Summon(disciple, from, 0x1FE, TimeSpan.FromMinutes(2), false, false);
                            disciple.OverrideDispellable = true;
                            disciple.Say("I am here to serve thee!");
                            disciple.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                            disciple.PlaySound(0x1FE);
                            disciples.Add(disciple);
                        }
                    }

                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                    from.SendMessage("Whom do you wish them to attack?");
                    from.Target = new InternalTarget(disciples);
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly List<Mobile> m_Disciples;

            public InternalTarget(List<Mobile> disciples) : base(
                8,
                false,
                TargetFlags.None
            ) =>
                m_Disciples = disciples;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target, true))
                {
                    for (var i = 0; i < m_Disciples.Count; i++)
                    {
                        m_Disciples[i].Attack(target);
                    }
                }
            }
        }
    }
}
