using System;
using Server.Items;

namespace Server.Mobiles;

public static class Paragon
{
    public const double ChestChance = 0.10;               // Chance that a paragon will carry a paragon chest
    public const double ChocolateIngredientChance = 0.20; // Chance that a paragon will drop a chocolatiering ingredient

    public static Map[] Maps =
    {
        Map.Ilshenar
    };

    private static readonly TimeSpan FastRegenRate = TimeSpan.FromSeconds(0.5);
    private static readonly TimeSpan CPUSaverRate = TimeSpan.FromSeconds(2);

    public static Type[] Artifacts =
    {
        typeof(GoldBricks), typeof(PhillipsWoodenSteed),
        typeof(AlchemistsBauble), typeof(ArcticDeathDealer),
        typeof(BlazeOfDeath), typeof(BowOfTheJukaKing),
        typeof(BurglarsBandana), typeof(CavortingClub),
        typeof(EnchantedTitanLegBone), typeof(GwennosHarp),
        typeof(IolosLute), typeof(LunaLance),
        typeof(NightsKiss), typeof(NoxRangersHeavyCrossbow),
        typeof(OrcishVisage), typeof(PolarBearMask),
        typeof(ShieldOfInvulnerability), typeof(StaffOfPower),
        typeof(VioletCourage), typeof(HeartOfTheLion),
        typeof(WrathOfTheDryad), typeof(PixieSwatter),
        typeof(GlovesOfThePugilist)
    };

    public static int Hue = 0x501; // Paragon hue

        // Buffs
        public static double GoldBuff = 2.5;
        public static double HitsBuff = 5.0;
        public static double StrBuff = 1.05;
        public static double IntBuff = 1.20;
        public static double DexBuff = 1.20;
        public static double SkillsBuff = 1.20;
        public static double SpeedBuff = 1.20;
        public static double FameBuff = 1.40;
        public static double KarmaBuff = 1.40;
        public static int DamageBuff = 5;

    public static void Convert(BaseCreature bc)
    {
        if (bc.IsParagon)
        {
            return;
        }

        bc.Hue = Hue;

        MonsterBuff.Convert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);

        new ParagonStamRegen(bc).Start();
    }

    public static void UnConvert(BaseCreature bc)
    {
        if (!bc.IsParagon)
        {
            return;
        }

        bc.Hue = 0;

        MonsterBuff.UnConvert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
    }

    public static bool CheckConvert(BaseCreature bc) => CheckConvert(bc, bc.Location, bc.Map);

    public static bool CheckConvert(BaseCreature bc, Point3D location, Map m)
    {
        if (!Core.AOS)
        {
            return false;
        }

        if (Array.IndexOf(Maps, m) == -1)
        {
            return false;
        }

        if (bc is BaseChampion or Harrower or BaseVendor or BaseEscortable or Clone || bc.IsParagon)
        {
            return false;
        }

        var fame = bc.Fame;

        if (fame > 32000)
        {
            fame = 32000;
        }

        var chance = 1 / Math.Round(20.0 - fame / 3200.0);

        return chance > Utility.RandomDouble();
    }

    public static bool CheckArtifactChance(Mobile m, BaseCreature bc)
    {
        if (!Core.AOS)
        {
            return false;
        }

        double fame = bc.Fame;

        if (fame > 32000)
        {
            fame = 32000;
        }

        var chance =
            1 / (Math.Max(10, 100 * (0.83 - Math.Round(Math.Log(Math.Round(fame / 6000, 3) + 0.001, 10), 3))) *
                (100 - Math.Sqrt(m.Luck)) / 100.0);

        return chance > Utility.RandomDouble();
    }

    public static void GiveArtifactTo(Mobile m)
    {
        var item = Artifacts.RandomElement().CreateInstance<Item>();

        if (m.AddToBackpack(item))
        {
            m.SendMessage("As a reward for slaying the mighty paragon, an artifact has been placed in your backpack.");
        }
        else
        {
            m.SendMessage(
                "As your backpack is full, your reward for destroying the legendary paragon has been placed at your feet."
            );
        }
    }

    private class ParagonStamRegen : Timer
    {
        private readonly BaseCreature m_Owner;

        public ParagonStamRegen(Mobile m) : base(FastRegenRate, FastRegenRate) => m_Owner = m as BaseCreature;

        protected override void OnTick()
        {
            if (!m_Owner.Deleted && m_Owner.IsParagon && m_Owner.Map != Map.Internal)
            {
                m_Owner.Stam++;

                Delay = Interval = m_Owner.Stam < m_Owner.StamMax * .75 ? FastRegenRate : CPUSaverRate;
            }
            else
            {
                Stop();
            }
        }
    }
}
