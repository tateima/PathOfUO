using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class Reckoning : BaseTalent
    {
        public int Stacks { get; set; }
        public int StoredDamage { get; set; }
        public Reckoning()
        {
            TalentDependency = typeof(HolyAvenger);
            RequiredWeapon = new[]
            {
                typeof(BaseWeapon)
            };
            CanBeUsed = true;
            DisplayName = "Reckoning";
            Description = "Stacks all damage taken to be returned on next successful hit . Requires 85+ chivalry.";
            AdditionalDetail = "Each level increases the maximum stacks by 3.";
            CooldownSeconds = 60;
            ManaRequired = 30;
            MaxLevel = 3;
            HasDamageAbsorptionEffect = true;
            CanAbsorbSpells = true;
            ImageID = 427;
            AddEndY = 115;
        }

        public override bool HasSkillRequirement(Mobile mobile) =>
            mobile.Skills[SkillName.Chivalry].Base >= 85; //&& mobile.Karma > 15000;

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated && Stacks >= 2)
            {
                damage += StoredDamage;
                Stacks = 0;
                Activated = false;
                StoredDamage = 0;
                attacker.FixedParticles(0x375A, 10, 15, 5017, EffectLayer.Waist);
                attacker.PlaySound(0x1EE);
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Activated && Stacks < Level * 3)
            {
                StoredDamage += damage;
                Stacks++;
            }
            return damage;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"Reckoning requires {ManaRequired.ToString()} mana to use.");
                }
                else
                {
                    from.PublicOverheadMessage(MessageType.Spell, from.SpeechHue, true, "In Aoth Nol", false);
                    Activated = true;
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
    }
}
