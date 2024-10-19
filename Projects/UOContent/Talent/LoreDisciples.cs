using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class LoreDisciples : BaseTalent
    {
        public LoreDisciples()
        {
            TalentDependencies = new[] { typeof(LoreSeeker) };
            CanBeUsed = true;
            DisplayName = "Lore disciples";
            Description = "Summon random humanoids to fight alongside you for 2 minutes.";
            AdditionalDetail = "These disciples will be either a Brigand, a Mage or a Healer. The raw stats of these disciples grows by 3% per level. Additionally, their skills will be improved by 2% for every point in the Lore Teacher talent.";
            CooldownSeconds = 300;
            ManaRequired = 40;
            MaxLevel = 5;
            ImageID = 158;
            GumpHeight = 75;
            AddEndY = 90;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                var canCast = true;
                if (from.Mana < ManaRequired)
                {
                    canCast = false;
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to call upon your disciples.");
                }

                if (canCast)
                {
                    ApplyManaCost(from);
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
                    var loreTeacher = ((PlayerMobile)from).GetTalent(typeof(LoreTeacher));
                    var loreMaster = ((PlayerMobile)from).GetTalent(typeof(LoreMaster));
                    int level = loreTeacher?.Level > Level ? loreTeacher.Level : 0;
                    int modifier = LoreSeeker.GetLoreModifier(from, level);
                    for (var i = 0; i < Level; i++)
                    {
                        BaseCreature disciple = null;
                        MobilePercentagePerPoint += modifier;
                        var skillIncrease = modifier * 2;
                        // lore master logic
                        if (loreMaster is not null && Utility.Random(100) < loreMaster.Level)
                        {
                            disciple = Utility.Random(3) switch
                            {
                                1 => new WarriorGuard(2),
                                2 => new ArcherGuard(2),
                                3 => new MageGuard(2),
                                _ => new NobleLord(2)
                            };
                        }
                        else
                        {
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
                        }

                        if (disciple != null)
                        {
                            disciple = (BaseCreature)ScaleMobileStats(disciple);
                            disciple.SetLevel();
                            EmptyCreatureBackpack(disciple);
                            SpellHelper.Summon(disciple, from, 0x1FE, TimeSpan.FromMinutes(2), false, false);
                            disciple.OverrideDispellable = true;
                            disciple.Say("I am here to serve thee!");
                            disciple.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                            disciple.PlaySound(0x1FE);
                            disciples.Add(disciple);
                        }
                    }

                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                    from.SendMessage("Whom do you wish them to attack?");
                    from.Target = new InternalTarget(disciples);
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        private class InternalTarget : Target
        {
            private readonly List<Mobile> _disciples;

            public InternalTarget(List<Mobile> disciples) : base(
                8,
                false,
                TargetFlags.None
            ) =>
                _disciples = disciples;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target && from.CanBeHarmful(target, true))
                {
                    for (var i = 0; i < _disciples.Count; i++)
                    {
                        _disciples[i].Attack(target);
                    }
                }
            }
        }
    }
}
