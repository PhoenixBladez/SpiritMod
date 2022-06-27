using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles.Magic
{
	public class GrassAura : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Natural Aura");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 30;
			Projectile.height = 160;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 480;
		}

		public override bool PreAI()
		{
			Projectile.tileCollide = false;
			int dust = Dust.NewDust(Projectile.position + Projectile.velocity,
				Projectile.width, Projectile.height, DustID.TerraBlade);
			Main.dust[dust].scale = 1.5f;
			Main.dust[dust].noGravity = true;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.Center = new Vector2(player.Center.X + (player.direction > 0 ? 40 : -40), player.position.Y - 40);   // I dont know why I had to set it to -60 so that it would look right   (change to -40 to 40 so that it's on the floor)
			Projectile.rotation += player.direction * 0.5f;
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && !proj.friendly)
					proj.Kill();

				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] >= 10f) {
					Projectile.localAI[0] = 0f;
					int num416 = 0;
					int num417 = 0;
					float num418 = 0f;
					int num419 = Projectile.type;
					for (int num420 = 0; num420 < 1000; num420++) {
						if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
							num416++;
							if (Main.projectile[num420].ai[1] > num418) {
								num417 = num420;
								num418 = Main.projectile[num420].ai[1];
							}
						}
					}

					if (num416 > 2) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return;
					}
				}
			}
		}

	}
}
