using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.SteamRaider
{
	public class StarLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Laser");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 2;
			projectile.height = 2;
			projectile.aiStyle = -1;
			projectile.friendly = false;
			projectile.penetrate = 8;
			projectile.alpha = 255;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.extraUpdates = 5;
		}

		public override bool PreAI()
		{
			Dust dust = Dust.NewDustPerfect(projectile.Center, 226);
			dust.velocity = Vector2.Zero;
			dust.noLight = true;
			dust.noGravity = true;
			return true;
		}

	}
}