using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon.Glitterfly
{
	public class GlitterDust : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glitter Dust");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 45;
			projectile.height = 8;
			projectile.alpha = 255;
			projectile.width = 8;
		}

		public override void AI()
		{
			Player player = Main.LocalPlayer;
			int distance1 = (int)Vector2.Distance(projectile.Center, player.Center);
			if (distance1 < 26)
			{
				player.AddBuff(BuffID.Confused, 180);
			}
		}
	}
}
