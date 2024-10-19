using System;
using Server.Items;
using Server.Mobiles;
using Server.Pantheon;
using Server.Targeting;

namespace Server.Talent
{
    public class Bribery : BaseTalent
    {
        private BaseCreature _ally;
        public Bribery()
        {
            DisplayName = "Bribery";
            DeityAlignment = Deity.Alignment.Greed;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description =
                "Pay gold to any humanoid to be in their employment for 10 minutes.";
            AdditionalDetail = "This does not apply to invulnerable, other players and guards. Requires at least 3000 gold. Each level decreases gold requirement by 1000.";
            ImageID = 138;
            MaxLevel = 3;
            CooldownSeconds = 500;
            GumpHeight = 230;
            AddEndY = 70;
            AddEndAdditionalDetailsY = 100;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                Container bank = from.FindBankNoCreate();
                int bankGold = Banker.GetBalance(from);
                int backpackGold = 0;
                if (from.Backpack is not null)
                {
                    backpackGold =  from.Backpack.GetAmount(typeof(Gold));
                }
                int goldToUse = 3000 - Level * 1000;
                if (bankGold >= goldToUse || backpackGold >= goldToUse)
                {
                    from.Target = new InternalTarget(this, goldToUse, bankGold, backpackGold);
                }
                else
                {
                    from.SendMessage($"You need {(3000 - Level * 1000).ToString()} gold in your bank to use {DisplayName}.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        protected void ExpireBribery()
        {
            _ally.Owners.Clear();
            _ally.SetControlMaster(null);
            _ally.Summoned = false;
        }

        private class InternalTarget : Target
        {
            private readonly Bribery _bribery;
            private int _goldToUse;
            private readonly int _bankGoldAmount;
            private readonly int _backpackGoldAmount;

            public InternalTarget(Bribery bribery, int goldToUse, int bankGoldAmount, int backpackGoldAmount) : base(
                10,
                false,
                TargetFlags.None
            )
            {
                _bribery = bribery;
                _goldToUse = goldToUse;
                _backpackGoldAmount = backpackGoldAmount;
                _bankGoldAmount = bankGoldAmount;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile target)
                {
                    if (Core.AOS && !target.InLOS(from))
                    {
                        from.SendMessage("Thou cannot bribe who you cannot see.");
                    }
                    else if (target is PlayerMobile)
                    {
                        from.SendMessage("Thou cannot bribe another player.");
                    }
                    else if (target is BaseCreature creature)
                    {
                        var validTarget = Deity.CanReceiveAlignment(creature, Deity.Alignment.Greed);
                        if (validTarget)
                        {
                            creature.PlaySound(creature.GetAngerSound());
                            if (_goldToUse > 0)
                            {
                                from.PlaySound(0x2E6);
                                if (_backpackGoldAmount > _goldToUse)
                                {
                                    while (_goldToUse > 0)
                                    {
                                        foreach (var gold in from.Backpack.FindItemsByType(typeof(Gold)))
                                        {
                                            if (gold.Amount > _goldToUse)
                                            {
                                                gold.Amount -= _goldToUse;
                                                _goldToUse = 0;
                                            }
                                            else
                                            {
                                                _goldToUse -= gold.Amount;
                                                gold.Delete();
                                            }
                                        }
                                    }
                                } else if (_bankGoldAmount > _goldToUse)
                                {
                                    Banker.Withdraw(from, _goldToUse);
                                }
                                creature.PublicOverheadMessage(MessageType.Regular, 0x0481, false, "*** Becomes your temporary mercenary for a fee ***");
                            }
                            else
                            {
                                creature.PublicOverheadMessage(MessageType.Regular, 0x0481, false, "*** Your bribery skills result in a free hire ***");
                            }
                            _bribery.OnCooldown = true;
                            creature.Owners.Add(from);
                            creature.SetControlMaster(from);
                            creature.Summoned = true;
                            Timer.StartTimer(
                                TimeSpan.FromSeconds(600),
                                _bribery.ExpireBribery,
                                out _
                            );
                            _bribery._ally = creature;
                            Timer.StartTimer(TimeSpan.FromSeconds(_bribery.CooldownSeconds), _bribery.ExpireTalentCooldown, out _bribery._talentTimerToken);
                        }
                        else
                        {
                            from.SendMessage("This target is of the wrong deity alignment.");
                        }
                    }
                    else
                    {
                        from.SendMessage("Your bribery won't work on this target.");
                    }
                }
            }
        }
    }
}
