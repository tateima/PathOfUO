using System;
using System.Collections.Generic;
using ModernUO.Serialization;
using Server.Engines.MLQuests;
using Server.Engines.MLQuests.Objectives;
using Server.Engines.MLQuests.Rewards;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Engines.PathQuests.Definitions
{
    public class MissingDaughter : MLQuest
    {
        public static QuestArea DaughtersDeathBoundary => new QuestArea(
            "Minoc peninsula",
            new Rectangle2D(new Point2D(2346, 186), new Point2D(2409, 220)),
            new[] { Map.Felucca, Map.Trammel }
        );
        public MissingDaughter()
        {
            Activated = true;
            Title = "Missing daughter"; // Missing daughter
            Description =
                "Help me bring justice adventurer!  My daughter went venturing in the peninsula north of Minoc two days ago. She has been missing ever since... Please, will you help me seek her out to the north and find out what came of her.";
            RefusalMessage = "Fair enough, be off with you then!";
            InProgressMessage = "We must not delay, my daughter might be in dire trouble!";
            CompletionNotice = CompletionNoticeShortReturn;
            OneTimeOnly = true;
            Objectives.Add(new EscortObjective(DaughtersDeathBoundary));

            Rewards.Add(new DummyReward(1116800)); // Experience reward
        }

        public override void OnRewardClaimed(MLQuestInstance instance)
        {
            BaseCreature escort = instance.Quester as BaseCreature;
            if (escort is not null)
            {
                escort.SetControlMaster(null);
                var remainsLocation = escort.Map.GetRandomNearbyLocation(escort.Location, 4, 2);
                new Torso().MoveToWorld(remainsLocation,  escort.Map);
                new LeftLeg().MoveToWorld(remainsLocation, escort.Map);
                new LeftArm().MoveToWorld(remainsLocation, escort.Map);
                new RightLeg().MoveToWorld(remainsLocation, escort.Map);
                new RightArm().MoveToWorld(remainsLocation, escort.Map);
                new Blood(0x122D).MoveToWorld(remainsLocation, escort.Map);
                new SilverBracelet().MoveToWorld(remainsLocation, escort.Map);
                escort.Say("No! What have these vile fiends done to my daughter! They shall pay for what they have done!");
            }
        }

        public override Type NextQuest => typeof(MissingHead);

        public override bool RecordCompletion => true;

        public override void OnAccept(IQuestGiver quester, PlayerMobile pm)
        {
            base.OnAccept(quester, pm);
            if (quester is BaseEscortable creature)
            {
                creature.CantWalk = false;
                creature.SetControlMaster(pm);
                creature.StartFollow();
                creature.Say("Let us be off to find my daughter at once!");
            };
        }

        public override void GetRewards(MLQuestInstance instance)
        {
            MLQuestSystem.Tell(instance.Quester, instance.Player, "Sarissa! nooo! You murderous fiends!");
            instance.Player.NonCraftExperience += 200;
            instance.ClaimRewards(); // skip gump
        }
    }
    public class MissingHead : MLQuest
    {
        public MissingHead()
        {
            Activated = true;
            Title = "Missing head";
            Description =
                "Sarissa! I cannot believe what these vile creatures have done, and where is her head?!? I can see her heirloom bracelet but her necklace is not to be found anywhere.  These wretched orcs will pay for this!";
            RefusalMessage = "How could you leave me here at a time like this! I will make you pay";
            CompletionNotice = CompletionNoticeShortReturn;
            OneTimeOnly = true;
            Objectives.Add(
                new KillObjective(10, new []{typeof(Orc), typeof(OrcishLord), typeof(OrcishMage) }, "Kill orcs around Minoc Peninsula", MissingDaughter.DaughtersDeathBoundary)
            );
            Rewards.Add(new DummyReward(1116800)); // Experience reward
            Rewards.Add(new ItemReward("Gold", typeof(Gold), 500));
        }

        public override bool RecordCompletion => true;
        public override Type NextQuest => typeof(MinocMourning);

        public override void OnRefuse(IQuestGiver quester, PlayerMobile pm)
        {
            if (quester is BaseEscortable creature)
            {
                creature.Say("Die coward!!!");
                creature.Combatant = pm;
                creature.Attack(pm);
            }
        }

        public override void OnAccept(IQuestGiver quester, PlayerMobile pm)
        {
            base.OnAccept(quester, pm);
            if (quester is BaseEscortable creature)
            {
                creature.SetControlMaster(pm);
                creature.StartFollow();
                creature.Say("Die fiends!!!");
            };
        }

        public override void GetRewards(MLQuestInstance instance)
        {
            MLQuestSystem.Tell(instance.Quester, instance.Player, "Look, I think I see her head over there! Oh the horror of it all. Please retrieve it!");
            Point3D headLocation = instance.Player.Map.GetRandomNearbyLocation(instance.Player.Location, 3, 1);
            new Head("Sarissa").MoveToWorld(headLocation, instance.Player.Map);
            instance.Player.NonCraftExperience += 350;
            instance.ClaimRewards(); // skip gump
        }
    }
    public class MinocMourning : MLQuest
    {
        public static QuestArea BonjoanEstate => new QuestArea(
            1116805,
            new Rectangle2D(new Point2D(2426, 435), new Point2D(2444, 443)),
            new[] { Map.Felucca, Map.Trammel }
        );
        public MinocMourning()
        {
            Activated = true;
            Title = "Minoc mourning"; // Minoc mourning
            Description = "Thank you, hero, for helping me... Though I have brought justice on these foul beasts, it brings me little comfort knowing my beloved Sarissa has been taken from me.  Please, I am weakened with grief, could you escort me to my estate in Minoc so that I may mourn.";
            RefusalMessage = "I... I understand, go in peace and good fortune";
            CompletionNotice = CompletionNoticeShortReturn;
            OneTimeOnly = true;
            Objectives.Add(new EscortObjective(BonjoanEstate));
            Rewards.Add(new DummyReward(1116800)); // Experience reward
        }

        public override bool RecordCompletion => true;

        public override bool CanOffer(IQuestGiver quester, PlayerMobile pm, MLQuestContext context, bool message)
        {
            bool hasHead = false;
            List<Item> heads = pm.Backpack?.FindItemsByType(typeof(Head));
            if (heads != null)
            {
                foreach (var headItem in heads)
                {
                    hasHead = headItem is Head { DefaultName: "Sarissa" };
                    if (hasHead)
                    {
                        break;
                    }
                }
            }

            if (!hasHead)
            {
                MLQuestSystem.Tell(quester, pm, "Please retrieve my daughter's head!");
            }
            return hasHead && base.CanOffer(quester, pm, context, message);
        }

        public override void OnAccept(IQuestGiver quester, PlayerMobile pm)
        {
            base.OnAccept(quester, pm);
            if (quester is BaseEscortable creature)
            {
                creature.SetControlMaster(pm);
                creature.StartFollow();
            };
        }

        public override void OnRewardClaimed(MLQuestInstance instance)
        {
            if (instance.Quester is MerryBonjoan escort)
            {
                escort.SetControlMaster(null);
                escort.CantWalk = true;
                escort.PublicOverheadMessage(MessageType.Regular, 0x0481, false, "*sobs*");
                escort.PlaySound(0x423);
                escort.IsMourning = true;
                Timer.StartTimer(TimeSpan.FromMinutes(15), () => { escort.IsMourning = false; });
            }
        }
        public override Type NextQuest => typeof(MinocRetribution);
        public override void GetRewards(MLQuestInstance instance)
        {
            MLQuestSystem.Tell(
                instance.Quester,
                instance.Player,
                "I am as always grateful to your services, for now please leave me be."
            );
            instance.Player.NonCraftExperience += 350;
            instance.ClaimRewards(); // skip gump
        }
    }
    public class MinocRetribution : MLQuest
    {
        public static QuestArea CoveOrcFort => new QuestArea(
            "Cove Orc Fort",
            new Rectangle2D(new Point2D(2152, 1344), new Point2D(2211, 1409)),
            new[] { Map.Felucca, Map.Trammel }
        );
        public MinocRetribution()
        {
            Activated = true;
            Title = "Minoc retribution";
            Description = "*sobs* The pain of her passing is weighing so heavily on my heart. My mind is clouded still with anger of orc kind. My fist burns with a retribution that could never be satisfied. The local Minoc authorities have informed me that an insignia I found on one of the brutes is similar to those found on orcs south of here near the port town of Cove. Adventurer, I desire more orc blood! Go, be my burning fist upon these pitiful creatures and bring back any information you find on their hideous corpses.";
            RefusalMessage = "Very well, I shall find other mercenaries who will heed my retribution.";
            CompletionNotice = CompletionNoticeShortReturn;
            OneTimeOnly = true;
            Objectives.Add(new KillObjective(25,
                new []{typeof(Orc), typeof(OrcishLord), typeof(OrcishMage) },
                "Kill orcs around Cove Orc Fort", CoveOrcFort,
                true, new List<Type>
                {
                    typeof(SacrificialOrcBlade)
                },
                2,
                1,
                1)
            );
            Rewards.Add(new DummyReward(1116800)); // Experience reward
            Rewards.Add(new ItemReward("Gold", typeof(Gold), 1200));
        }
        public override bool CanOffer(IQuestGiver quester, PlayerMobile pm, MLQuestContext context, bool message) => quester is MerryBonjoan { IsMourning: false } && base.CanOffer(quester, pm, context, message);

        public override bool RecordCompletion => true;

        public override void GetRewards(MLQuestInstance instance)
        {
            MLQuestSystem.Tell(
                instance.Quester,
                instance.Player,
                "Fantastic result! Tell me, did you find anything from their bloodied remains?"
            );
            instance.Player.NonCraftExperience += 1000;
        }
    }

    [SerializationGenerator(0, false)]
    public partial class SacrificialOrcBlade : Dagger
    {
        [Constructible]
        public SacrificialOrcBlade() : base() => Hue = 0x26;

        public override string DefaultName => "a sacrificial orcish blade";
    }
    [QuesterName("Merry B'onjoan the noble)")]
    [SerializationGenerator(0, false)]
    public partial class MerryBonjoan : BaseEscortable
    {
        public bool IsMourning { get; set; }

        [Constructible]
        public MerryBonjoan()
        {
            SetLevel();
            AlterLevels(7, false, 14, 17);
            InitBody();
            Female = false;
            Body = 400;
            InitOutfit();
            EquipItem(new Longsword());
            EquipItem(new RingmailArms());
            EquipItem(new RingmailChest());
            EquipItem(new ChainCoif());
            EquipItem(new ChainLegs());
            EquipItem(new Tunic());
            EquipItem(new Cloak());
            CantWalk = true;
            Name = "Merry Bonjoan";
        }

        public MerryBonjoan(Serial serial)
            : base(serial)
        {
        }

        public override bool StaticMLQuester => true;
        public override bool InitialInnocent => true;
        public override string DefaultName => "Merry Bonjoan";

        public override void Shout(PlayerMobile pm)
        {
            MLQuestSystem.Tell(
                this,
                pm,
                Utility.RandomList(
                    1116801, // You there! Please help a noble in need!
                    1074222  // Could I trouble you for some assistance?
                )
            );
        }

        public override bool OnBeforeDeath()
        {
            base.OnBeforeDeath();
            Say("My daughter, forgive me!");
            Timer.StartTimer(TimeSpan.FromHours(1), Respawn);
            return true;
        }

        private void Respawn()
        {
            MerryBonjoan me = new MerryBonjoan();
            me.MoveToWorld(new Point3D(2477, 416, 15), Map.Trammel);
        }

        public override bool DeleteCorpseOnDeath => false;

    }
}
