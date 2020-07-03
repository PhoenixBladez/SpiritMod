using Terraria;
using Terraria.ModLoader;
namespace SpiritMod.Tide
{
    public class TidePlayer : ModPlayer
    {
        public bool createdProjectiles;
		public override void PostUpdate()
		{
			if(!TideWorld.TheTide) {
				TideWorld.TidePoints = 0;
				TideWorld.EnemyKills = 0;
				createdProjectiles = false;
			} else if(TideWorld.TidePoints >= 100) {
				TideWorld.TidePoints = 0;
				TideWorld.EnemyKills = 0;
				Main.NewText("The tide has ended.", 145, 0, 255);
				TideWorld.TheTide = false;
			}
		}
    }
}
