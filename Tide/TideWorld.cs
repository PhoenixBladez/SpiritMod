using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Tide
{
    public class TideWorld : ModWorld
    {
        public static int TidePoints = 0;
        public static int EnemyKills = 0;
        public static bool TheTide;

        public override void Initialize()
        {
            TheTide = false;
        }

        public override void PostUpdate()
        {
            //TidePoints = EnemyKills / 2;
            if (TidePoints >= 100)
            {
                Main.NewText("The tide has ended.", 145, 0, 255);
                TidePoints = 0;
                EnemyKills = 0;
                TheTide = false;
            }
        }
    }
}
