using System;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Talent
{
    public class Automaton : BaseTalent
    {
        private AutomatonConstruct _construct;
        public Automaton()
        {
            UpgradeCost = true;
            BlockedBy = new[] { typeof(SmoothTalker) };
            TalentDependencies = new[] { typeof(Inventive) };
            DisplayName = "Automaton";
            MobilePercentagePerPoint = 10;
            CooldownSeconds = 480;
            CanBeUsed = true;
            Description =
                "Construct an automaton to assist you.";
            AdditionalDetail = "As your level increases, the upgrade costs also increase substantially. A manual exists that specifies the exact requirements. Requires 95 tinkering skill points. The more engineering skills you have (Carpentry, Tailoring, Blacksmithing and Tinkering) the more powerful the automaton will become.";
            ImageID = 352;
            GumpHeight = 230;
            AddEndY = 135;
            AddEndAdditionalDetailsY = AddEndY;
        }

        public override bool HasUpgradeRequirement(Mobile mobile)
        {
            var ironCost = 0;
            var copperCost = 0;
            var goldCost = 0;
            var agapiteCost = 0;
            var veriteCost = 0;
            var bronzeCost = 0;
            var valoriteCost = 0;
            var shadowIronCost = 0;
            var dullCopperCost = 0;
            var hideCost = 0;
            var barbedHideCost = 0;
            var hornedHideCost = 0;
            var spinedHideCost = 0;
            var clockParts = 0;

            switch (Level)
            {
                case 1:
                    clockParts = 500;
                    ironCost = 2000;
                    dullCopperCost = 4000;
                    bronzeCost = 2000;
                    shadowIronCost = 2000;
                    goldCost = 2000;
                    spinedHideCost = 2000;
                    hornedHideCost = 2000;
                    break;
                case 2:
                    ironCost = 4000;
                    clockParts = 800;
                    goldCost = 4000;
                    bronzeCost = 4000;
                    shadowIronCost = 4000;
                    hornedHideCost = 3000;
                    barbedHideCost = 2000;
                    break;
                case 3:
                    clockParts = 1000;
                    goldCost = 7000;
                    agapiteCost = 5000;
                    veriteCost = 4000;
                    hornedHideCost = 5000;
                    barbedHideCost = 3000;
                    break;
                case 4:
                    clockParts = 3000;
                    goldCost = 9000;
                    agapiteCost = 6000;
                    veriteCost = 5000;
                    valoriteCost = 4000;
                    hornedHideCost = 6000;
                    barbedHideCost = 5000;
                    break;
            }

            if (Level == 0)
            {
                return true;
            }

            return HasResourceQuantity(mobile, typeof(ClockParts), clockParts)
                   && HasResourceQuantity(mobile, typeof(IronIngot), ironCost)
                   && HasResourceQuantity(mobile, typeof(DullCopperIngot), dullCopperCost)
                   && HasResourceQuantity(mobile, typeof(CopperIngot), copperCost)
                   && HasResourceQuantity(mobile, typeof(ShadowIronIngot), shadowIronCost)
                   && HasResourceQuantity(mobile, typeof(BronzeIngot), bronzeCost)
                   && HasResourceQuantity(mobile, typeof(GoldIngot), goldCost)
                   && HasResourceQuantity(mobile, typeof(AgapiteIngot), agapiteCost)
                   && HasResourceQuantity(mobile, typeof(VeriteIngot), veriteCost)
                   && HasResourceQuantity(mobile, typeof(ValoriteIngot), valoriteCost)
                   && HasResourceQuantity(mobile, typeof(Hides), hideCost)
                   && HasResourceQuantity(mobile, typeof(HornedHides), hornedHideCost)
                   && HasResourceQuantity(mobile, typeof(BarbedHides), barbedHideCost)
                   && HasResourceQuantity(mobile, typeof(SpinedHides), spinedHideCost);

        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Tinkering.Base >= 95;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                from.RevealingAction();
                var inventive = ((PlayerMobile)from).GetTalent(typeof(Inventive));
                // its a talent, no need for animation timer, just a single animation is fine
                from.Animate(269, 7, 1, true, false, 0);
                _construct = new AutomatonConstruct();
                switch (Utility.RandomMinMax(1, 8))
                {
                    case 1:
                        break;
                    case 2:
                        _construct.Body = 113;
                        break;
                    case 3:
                        _construct.Body = 111;
                        break;
                    case 4:
                        _construct.Body = 166;
                        break;
                    case 5:
                        _construct.Body = 110;
                        break;
                    case 6:
                        _construct.Body = 107;
                        break;
                    case 7:
                        _construct.Body = 109;
                        break;
                    case 8:
                        _construct.Body = 108;
                        break;
                }
                int modifier = 2;
                var score = EngineeringScore(from);
                MobilePercentagePerPoint += (int)(score / 50);
                if (inventive != null)
                {
                    modifier += inventive.Level;
                    MobilePercentagePerPoint += inventive.Level;
                }
                _construct = (AutomatonConstruct)ScaleMobile(_construct);
                EmptyCreatureBackpack(_construct);
                SpellHelper.Summon(_construct, from, 0x042, TimeSpan.FromMinutes(modifier), false, false);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                OnCooldown = true;
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public override void UpdateMobile(Mobile mobile)
        {
            var manual = mobile.Backpack?.FindItemByType<AutomatonManual>() ?? mobile.BankBox?.FindItemByType<AutomatonManual>();
            if (manual is null)
            {
                mobile.Backpack?.AddItem(new AutomatonManual());
                mobile.SendMessage("An automaton manual has been placed in your backpack.");
            }
        }

        public static double EngineeringScore(Mobile mobile)
        {
            var score = mobile.Skills.Tinkering.Value + mobile.Skills.Blacksmith.Value + mobile.Skills.Carpentry.Value + mobile.Skills.Tailoring.Value;
            return score;
        }
    }
}
