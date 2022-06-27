using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Gores : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Gores");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 300;
			Projectile.height = 300;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 120;
			Projectile.alpha = 255;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 0 : 0), player.position.Y + 30);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(BuffID.Ichor, 380);
	}
}
