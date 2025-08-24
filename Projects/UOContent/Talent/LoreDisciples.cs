using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class LoreDisciples : BaseTalent
    {
        private List<BaseCreature> _disciples;
        private PlayerMobile _loreSeeker;
        private DateTime _startSummonDate;
        private int _remainingSeconds;
        public LoreDisciples()
        {
            TalentDependencies = new[] { typeof(LoreSeeker) };
            CanBeUsed = true;
            DisplayName = "Lore disciples";
            Description = "Summon random humanoids to fight alongside you for 2 minutes.";
            AdditionalDetail = "These disciples will be either a Brigand, a Mage or a Healer. The raw stats of these disciples grows by 3% per level. Additionally, their skills will be improved by 2% for every point in the Lore Teacher talent.";
            CooldownSeconds = 600;
            _remainingSeconds = 600;
            HasGroupKillEffect = true;
            HasKillEffect = true;
            ManaRequired = 20;
            MaxLevel = 3;
            ImageID = 158;
            GumpHeight = 75;
            AddEndY = 115;
            _disciples = new List<BaseCreature>();
        }

        public List<Mobile> Disciples { get; set; }

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
                    _loreSeeker = (PlayerMobile)from;
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
                    var loreTeacher = ((PlayerMobile)from).GetTalent(typeof(LoreTeacher));
                    var loreMaster = ((PlayerMobile)from).GetTalent(typeof(LoreMaster));
                    int level = Level;
                    if (loreTeacher != null)
                    {
                        level += loreTeacher.Level;
                    }
                    int modifier = LoreSeeker.GetLoreModifier(from, level);
                    MobilePercentagePerPoint = modifier;
                    for (var i = 0; i < Level; i++)
                    {
                        BaseCreature disciple = null;
                        var skillIncrease = modifier * 2;
                        // lore master logic
                        if (loreMaster is not null)
                        {
                            CooldownSeconds -= loreMaster.Level * 13;
                            if (Utility.Random(100) < loreMaster.Level)
                            {
                                disciple = Utility.Random(3) switch
                                {
                                    1 => new WarriorGuard(2),
                                    2 => new ArcherGuard(2),
                                    3 => new MageGuard(2),
                                    _ => new NobleLord(2)
                                };
                                disciple.Skills.Fencing.Base += skillIncrease;
                                disciple.Skills.Archery.Base += skillIncrease;
                                disciple.Skills.Macing.Base += skillIncrease;
                                disciple.Skills.MagicResist.Base += skillIncrease;
                                disciple.Skills.Swords.Base += skillIncrease;
                                disciple.Skills.Tactics.Base += skillIncrease;
                                disciple.Skills.Wrestling.Base += skillIncrease;
                                disciple.Skills.Forensics.Base += skillIncrease;
                                disciple.Skills.SpiritSpeak.Base += skillIncrease;
                                disciple.Skills.EvalInt.Base += skillIncrease;
                                disciple.Skills.Magery.Base += skillIncrease;
                            }
                        }
                        else
                        {
                            switch (Utility.RandomMinMax(1, 6))
                            {
                                case 1:
                                case 2:
                                case 3:
                                    disciple = new Brigand();
                                    break;
                                case 4:
                                case 5:
                                    disciple = new EvilMage();
                                    break;
                                case 6:
                                    disciple = new EvilHealer();
                                    break;
                            }

                            if (disciple is EvilMage)
                            {
                                disciple.Title = "the mage";
                                disciple.SpeechHue = Utility.RandomDyedHue();
                                disciple.Hue = Race.Human.RandomSkinHue();

                                if (disciple.Female == Utility.RandomBool())
                                {
                                    disciple.Body = 0x191;
                                    disciple.Name = NameList.RandomName("female");
                                }
                                else
                                {
                                    disciple.Body = 0x190;
                                    disciple.Name = NameList.RandomName("male");
                                }
                            }
                        }

                        if (disciple != null)
                        {
                            disciple.Level = Utility.RandomMinMax(_loreSeeker.Level - 2, _loreSeeker.Level + 2);
                            if (loreMaster != null)
                            {
                                disciple = (BaseCreature)ScaleMobileStats(disciple);
                            }
                            EmptyCreatureBackpack(disciple);
                            SpellHelper.Summon(disciple, from, 0x1FE, TimeSpan.FromMinutes(4), false, false);
                            _startSummonDate = Core.Now;
                            disciple.OverrideDispellable = true;
                            disciple.Say("I am here to serve thee!");
                            disciple.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                            disciple.PlaySound(0x1FE);
                            _disciples.Add(disciple);
                        }
                    }

                    Timer.StartTimer(TimeSpan.FromSeconds(10), HealTick);
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    from.SendMessage("Whom do you wish them to attack?");
                    from.Target = new InternalTarget(_disciples);
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public virtual void HealTick()
        {
            foreach (var disciple in _disciples)
            {
                if (disciple is EvilHealer)
                {
                    if (disciple.Mana > 20 && disciple.Alive && !disciple.Deleted)
                    {
                        Mobile target = null;
                        if (_loreSeeker != null && _loreSeeker.Hits < _loreSeeker.HitsMax/2)
                        {
                            target = _loreSeeker;
                        } else if (disciple.Hits < disciple.HitsMax / 2)
                        {
                            target = disciple;
                        }
                        if (target is { Alive: true } && disciple.CanSee(target))
                        {
                            disciple.Mana -= 20;
                            target.Heal(Utility.RandomMinMax(10, 25));
                            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                            target.PlaySound(0x202);
                        }
                    }
                    Timer.StartTimer(TimeSpan.FromSeconds(10), HealTick);
                }
            }
        }

        public override void CheckKillEffect(Mobile victim, Mobile killer)
        {
            foreach (var disciple in _disciples)
            {
                if (disciple.Alive && !disciple.Deleted)
                {
                    var expression = Utility.Random(3) switch
                    {
                        1 => "Glory to our master.",
                        2 => "We shall endure all perils in this land.",
                        3 => "All hail the true ruler of this kingdom.",
                        _ => "I feel so powerful as a disciple."
                    };
                    disciple.Say(expression);
                }
            }
        }

        public override void CheckGroupKillEffect(Mobile victim, Mobile killer)
        {
            if (OnCooldown)
            {
                _remainingSeconds = CooldownSeconds - (int)(Core.Now - _startSummonDate).TotalSeconds;
                if (_disciples.Count > 0)
                {
                    _remainingSeconds -= Level + SummonerCommandLevel(killer);
                } else
                {
                    _remainingSeconds--;
                }
                if (_remainingSeconds <= 0)
                {
                    ExpireTalentCooldown();
                    if (_talentTimerToken.Running)
                    {
                        _talentTimerToken.Cancel();
                    }
                    _remainingSeconds = CooldownSeconds;
                }
            }
        }


        private class InternalTarget : Target
        {
            private readonly List<BaseCreature> _disciples;

            public InternalTarget(List<BaseCreature> disciples) : base(
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
