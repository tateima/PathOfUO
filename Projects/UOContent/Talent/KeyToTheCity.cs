using System;
using Server.Items.Misc;
using Server.Pantheon;

namespace Server.Talent
{
    public class KeyToTheCity : BaseTalent
    {
        private CityKey _key;

        public KeyToTheCity()
        {
            DeityAlignment = Deity.Alignment.Charity;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            DisplayName = "Key to the city";
            Description = "Create a key to the city, all vendor prices are free for 20 minutes.";
            AdditionalDetail = "Each level adds 5 minutes to the duration of the item. The key must be in your backpack to work.";
            ImageID = 425;
            GumpHeight = 75;
            ManaRequired = 30;
            CooldownSeconds = 3600;
            AddEndY = 100;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && from.Mana > ManaRequired)
            {
                ApplyManaCost(from);
                Activated = true;
                OnCooldown = true;
                from.SendSound(from.Female ? 0x30A : 0x419);
                _key = new CityKey();
                from.AddToBackpack(_key);
                Timer.StartTimer(TimeSpan.FromSeconds(1200 + Level * 300), ExpireBuff);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
            else
            {
                from.SendMessage($"You need {ManaRequired.ToString()} mana to craft this special key.");
            }
        }

        private void ExpireBuff()
        {
            Activated = false;
            _key?.Delete();
        }
    }
}
