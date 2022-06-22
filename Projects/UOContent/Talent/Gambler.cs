using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class Gambler : BaseTalent
    {
        public Gambler()
        {
            CanBeUsed = true;
            TalentDependency = typeof(SmoothTalker);
            DisplayName = "Gambler";
            Description = "Gamble gold with target npc, high chance of gold loss.";
            AdditionalDetail = $"The gold received increases by 50 per level. Wins can also result in some rarer items. {PassiveDetail}";
            CooldownSeconds = 300;
            ImageID = 362;
            GumpHeight = 85;
            AddEndY = 75;
            MaxLevel = 10;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            var manual = mobile.Backpack?.FindItemByType<GamblersGuide>() ?? mobile.BankBox?.FindItemByType<GamblersGuide>();
            if (manual is null)
            {
                mobile.Backpack?.AddItem(new GamblersGuide());
                mobile.SendMessage("A gamblers guide has been placed in your backpack.");
            }
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                from.SendMessage("Whom do you wish to gamble with?");
                from.Target = new InternalTarget(from, this);
            }
        }

        public override double ModifySpellScalar() => Level / 100 * 2; // 2% per point

        private class InternalTarget : Target
        {
            private readonly Mobile _gambler;
            private readonly Gambler _gamblerTalent;

            public InternalTarget(Mobile gambler, Gambler gamblerTalent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _gambler = gambler;
                _gamblerTalent = gamblerTalent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseVendor or BaseEscortable or Gypsy or Actor or Artist or Sculptor or Samurai or Ninja)
                {
                    var now = DateTime.Now;
                    BaseCreature opponent = (BaseCreature)targeted;
                    if (opponent.NextGambleTime > now)
                    {
                        opponent.SayTo(from, "I have no more money to gamble with");
                    }
                    else
                    {
                        double goldAmount = Utility.Random(_gamblerTalent.Level * 50);
                        var chanceOfLosing = 30.0;
                        var stakes = Utility.RandomDouble();
                        if (targeted is Gypsy or GypsyLord)
                        {
                            chanceOfLosing += targeted is GypsyLord ? Utility.RandomMinMax(40, 60) : Utility.RandomMinMax(20, 30);
                            stakes += Utility.RandomDouble();
                        }

                        goldAmount *= stakes;
                        if (_gamblerTalent.CanAffordLoss((PlayerMobile)_gambler, (int)goldAmount))
                        {
                            bool loss = Utility.Random(100) < (int)chanceOfLosing;
                            _gamblerTalent.ProcessGoldGain(
                                (PlayerMobile)_gambler,
                                (int)goldAmount,
                                loss,
                                true
                            );
                            if (!loss)
                            {
                                opponent.GambleLosses++;
                                if (opponent.GambleLosses >= 5)
                                {
                                    opponent.NextGambleTime = now.AddHours(2);
                                }
                                if (opponent is Gypsy && Utility.Random(100) < 10)
                                {
                                    var jewelry = Loot.RandomJewelry();
                                    _gambler.Backpack?.AddItem(jewelry);
                                    opponent.SayTo(from, "Here's the bonus stakes");
                                } else if (opponent is GypsyLord && Utility.Random(2000) < 1)
                                {
                                    if (Utility.RandomBool())
                                    {
                                        var pack = LootPack.Rich;
                                        pack.ForceGenerate(_gambler, _gambler.Backpack, pack.RandomEntry(), 1);
                                    }
                                    else
                                    {
                                        Item stakeItem = Utility.Random(2) switch
                                        {
                                            1 => new RuneWord(),
                                            2 => Loot.RandomShard(),
                                            _ => new RuneScroll()
                                        };
                                        _gambler.Backpack.AddItem(stakeItem);
                                    }
                                    opponent.SayTo(from, "You are a lucky adventurer today!");
                                }
                            }
                            else if (opponent.GambleLosses > 0)
                            {
                                var jewel = _gambler.Backpack?.FindItemByType<BaseJewel>();
                                if (jewel != null && Utility.Random(100) < 10)
                                {
                                    opponent.Backpack?.AddItem(jewel);
                                    opponent.SayTo(from, "I'll take that jewel thank ye!");
                                }
                                opponent.GambleLosses--;
                            }
                            string winSpeech = Utility.Random(6) switch
                            {
                                0 => $"Haha! Good game, where's my {(int)goldAmount}!",
                                1 => $"Booyah! The kids will be eating tonight! Hand over the {(int)goldAmount} gold!",
                                2 => $"I will be a lord yet! Hand over the {(int)goldAmount} gold!",
                                3 => $"I'm heading to the inn with my {(int)goldAmount} gold!",
                                4 => $"Are you sure you want to play me again? Thank you for the {(int)goldAmount} gold!",
                                5 => $"I am dominating today! Give me the {(int)goldAmount} gold!",
                                _ => $"I can go and buy some bread with this {(int)goldAmount} gold!"
                            };
                            string lossSpeech = Utility.Random(6) switch
                            {
                                0 => $"Bah! You beat me! Here is your {(int)goldAmount} gold!",
                                1 => $"Alas! My kids will go hungry tonight, take the {(int)goldAmount} gold!",
                                2 => $"My lover is going to kill me! Here is your {(int)goldAmount} gold!",
                                3 => $"I really should find a real profession, take the {(int)goldAmount} gold!",
                                4 => $"Wow, you are good! Here is the {(int)goldAmount} gold!",
                                5 => $"Stop beating me! Take the rest of my {(int)goldAmount} gold!",
                                _ => $"Time for me to go and sleep on the streets and eat mouldy cheese. Here is the {(int)goldAmount} gold!"
                            };
                            opponent.SayTo(
                                from,
                                loss
                                    ? winSpeech
                                    : lossSpeech
                            );
                            from.SendSound(0x32);
                            _gamblerTalent.OnCooldown = true;
                            Timer.StartTimer(TimeSpan.FromSeconds(_gamblerTalent.CooldownSeconds), _gamblerTalent.ExpireTalentCooldown, out _gamblerTalent._talentTimerToken);
                        }
                        else
                        {
                            opponent.Say("Thou cannot afford to bet with me!");
                        }
                    }
                }
                else
                {
                    _gambler.SendMessage("You cannot gamble with that target.");
                }
            }
        }
    }
}
