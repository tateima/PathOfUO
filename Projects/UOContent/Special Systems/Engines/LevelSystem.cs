using System;
using Server.Mobiles;
using Server.Pantheon;
using Server.SkillHandlers;
using Server.Talent;

namespace Server.Misc;

public class LevelSystem
{
    public static bool MaxLevel(Mobile target)
    {
        if (target is PlayerMobile player)
        {
            return player.Level >= BaseCreature.MaximumCreatureLevel;
        }

        if (target is BaseCreature creature)
        {
            return creature.Level >= BaseCreature.MaximumCreatureLevel;
        }
        return true;
    }
    public static int GetRequiredExperience(int level) => level * (level - 1) * (500 + level * 5);
    public static int GetBaseExperience(int level) => GetRequiredExperience(level);

    public static int NextLevel(Mobile target)
    {
        if (target is PlayerMobile player)
        {
            return GetRequiredExperience(player.Level + 1);
        }

        if (target is BaseCreature creature)
        {
            return GetRequiredExperience(creature.Level + 1);
        }
        return GetRequiredExperience(999);
    }
    public static void CheckExperience(
        int levelExperience, int nonCraftExperience,
        int craftExperience, int rangerExperience, Mobile target
    )
    {
        bool isMaxLevel = MaxLevel(target);
        int requiredExperience = NextLevel(target);
        if (levelExperience + nonCraftExperience + craftExperience + rangerExperience >= requiredExperience && !isMaxLevel)
        {
            double totalExperienceEarned = nonCraftExperience + craftExperience + rangerExperience;
            // get remaining experience to add later
            int remainingExp = (int)(levelExperience + totalExperienceEarned - requiredExperience);
            if (remainingExp < 0)
            {
                remainingExp = 0;
            }

            if (target is PlayerMobile playerMobile)
            {
                playerMobile.Level++;
                if (playerMobile.Level >= 5 && playerMobile.Young)
                {
                    playerMobile.Young = false;
                }

                // calculate % contributions, round down not up to figure out point allocation
                double craftingContr = craftExperience / totalExperienceEarned;
                double nonCraftingContr = nonCraftExperience / totalExperienceEarned;
                double rangerContr = rangerExperience / totalExperienceEarned;
                int numberOfSkills = 0;
                if (playerMobile.Level < 70)
                {
                    numberOfSkills = playerMobile.Level > 10 ? 12 : 14;
                }

                playerMobile.NonCraftSkillPoints += (int)Math.Round(
                    nonCraftingContr * numberOfSkills,
                    MidpointRounding.AwayFromZero
                );
                playerMobile.CraftSkillPoints += (int)Math.Round(
                    craftingContr * numberOfSkills,
                    MidpointRounding.AwayFromZero
                );
                playerMobile.RangerSkillPoints += (int)Math.Round(
                    rangerContr * numberOfSkills,
                    MidpointRounding.AwayFromZero
                );
                if (playerMobile.Level < 5 || playerMobile.Level % 5 == 0 && playerMobile.Level <= 70)
                {
                    playerMobile.StatPoints += 5;
                }
                else
                {
                    playerMobile.StatPoints += 2;
                }

                if (playerMobile.Level % 5 == 0 && playerMobile.Alignment is Deity.Alignment.None &&
                    playerMobile.Level <= 70)
                {
                    playerMobile.TalentPoints += 2;

                }
                else if (playerMobile.Level <= 75)
                {
                    playerMobile.TalentPoints += 1;
                }

                // reset craft and non craft experience for next level tracking
                playerMobile.CraftExperience = (int)(remainingExp * craftingContr);
                playerMobile.NonCraftExperience = (int)(remainingExp * nonCraftingContr);
                playerMobile.RangerExperience = (int)(remainingExp * rangerContr);
                playerMobile.SendMessage("You have gained a level!");
                playerMobile.LevelExperience = GetBaseExperience(playerMobile.Level);
            }
            else if (target is BaseCreature creature)
            {
                creature.Level++;
                double scale = 1.03;
                if (creature.Tamable && creature.ControlMaster is PlayerMobile controlMaster)
                {
                    BaseTalent rangerCommand = controlMaster.GetTalent(typeof(RangerCommand));
                    if (rangerCommand != null)
                    {
                        scale += 0.003 * rangerCommand.Level;
                    }
                }
                AnimalTaming.ScaleStats(creature, scale);
                AnimalTaming.ScaleSkills(creature, scale);
                creature.LevelExperience = GetBaseExperience(creature.Level) + remainingExp;
            }

            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
            target.PlaySound(0x202);
        }

        target.InvalidateProperties();
    }
}
