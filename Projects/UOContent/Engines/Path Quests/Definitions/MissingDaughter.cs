using System;
using ModernUO.Serialization;
using Server.Engines.MLQuests;
using Server.Engines.MLQuests.Objectives;
using Server.Engines.MLQuests.Rewards;
using Server.Ethics;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.PathQuests.Definitions
{
    public class MissingDaughter : MLQuest
    {
        public static QuestArea DaughtersDeathBoundary => new QuestArea(
            1116805,
            new Rectangle2D(new Point2D(2346, 186), new Point2D(2409, 220)),
            new[] { Map.Felucca, Map.Trammel }
        );
        public MissingDaughter()
        {
            Activated = true;
            Title = 1116801; // Missing daughter
            Description =
                1116802; // Help me bring justice adventurer!  My daughter went venturing in the peninsula north of Minoc two days ago. She has been missing ever since... Please, will you help me seek her out to the north and find out what came of her.
            RefusalMessage = 1116803; // Fair enough, be off with you then!
            InProgressMessage = 1116804; // We must not delay, my daughter might be in dire trouble!
            CompletionNotice = CompletionNoticeShortReturn;
            OneTimeOnly = true;
            Objectives.Add(
                new EscortObjective(DaughtersDeathBoundary));

            Rewards.Add(new DummyReward(1116800)); // Experience reward
        }

        public override void OnRewardClaimed(MLQuestInstance instance)
        {
            ((BaseCreature)instance.Quester).SetControlMaster(null);
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
            MLQuestSystem.Tell(instance.Quester, instance.Player, 1116806);
            instance.Player.NonCraftExperience += 200;
            instance.ClaimRewards(); // skip gump
        }
    }
    public class MissingHead : MLQuest
    {
        public MissingHead()
        {
            Activated = true;
            Title = 1116808; // Missing head
            Description =
                1116809; // Sarissa! I cannot believe what these vile creatures have done, and where is her head?!? I can see her heirloom bracelet but her necklace is not to be found anywhere.  May be these wretched orcs have taken both from her defiled corpse! Help me slay these abominations!
            RefusalMessage = 1116810; // How could you leave me here at a time like this! I will make you pay
            CompletionNotice = CompletionNoticeShortReturn;
            OneTimeOnly = true;
            Objectives.Add(
                new KillObjective(10, new []{typeof(Orc), typeof(OrcishLord), typeof(OrcishMage) }, "Kill orcs around Minoc Peninsula", MissingDaughter.DaughtersDeathBoundary)
            );
            Rewards.Add(new DummyReward(1116800)); // Experience reward
            Rewards.Add(new ItemReward("Gold", typeof(Gold), 500));
        }

        public override bool RecordCompletion => true;

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
            MLQuestSystem.Tell(instance.Quester, instance.Player, 1116811); // Look, I think I see her head over there! Oh the horror of it all.
            Point3D headLocation = instance.Player.Map.GetRandomNearbyLocation(instance.Player.Location, 3, 1);
            new Head("Sarissa").MoveToWorld(headLocation, instance.Player.Map);
            instance.Player.NonCraftExperience += 350;
            instance.ClaimRewards(); // skip gump
        }
    }
    [QuesterName("Merry B'onjoan the noble)")]
    public sealed class MerryBonjoan : BaseEscortable
    {
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
                    1116807, // You there! Please help a noble in need!
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

        public override void EndEscortSequence(PlayerMobile pm)
        {
            var remainsLocation = Map.GetRandomNearbyLocation(Location, 4, 2);
            new Torso().MoveToWorld(remainsLocation, Map);
            new LeftLeg().MoveToWorld(remainsLocation, Map);
            new LeftArm().MoveToWorld(remainsLocation, Map);
            new RightLeg().MoveToWorld(remainsLocation, Map);
            new RightArm().MoveToWorld(remainsLocation, Map);
            new Blood(0x122D).MoveToWorld(remainsLocation, Map);
            new SilverBracelet().MoveToWorld(remainsLocation, Map);
            Say("No! What have these vile fiends done to my daughter! They shall pay for what they have done!");
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
}
