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
			Projectile.friendly = true;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 45;
			Projectile.height = 8;
			Projectile.alpha = 255;
			Projectile.width = 8;
		}

		public override void AI()
		{
			Player player = Main.LocalPlayer;
			int distance1 = (int)Vector2.Distance(Projectile.Center, player.Center);
			if (distance1 < 26)
			{
				player.AddBuff(BuffID.Confused, 180);
			}
		}
	}
}
