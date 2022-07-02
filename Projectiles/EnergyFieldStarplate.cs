using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Projectiles
{
	public class EnergyFieldStarplate : ModProjectile
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Energizer Field");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 140;
			Projectile.height = 140;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 4;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
			Projectile.tileCollide = false; //Tells the game whether or not it can collide with a tile
		}

		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && proj.friendly && proj.IsRanged() && !proj.GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon) {
					proj.GetGlobalProjectile<SpiritGlobalProjectile>().shotFromMaliwanShockCommon = true;
					proj.damage += (int)(proj.damage * 0.15);
				}
			}
			for (int k = 0; k < 4; k++) {
				int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Electric);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 84f;
				Main.dust[dust].position = Projectile.Center - vector2_3;
			}
			return false;
		}
		public override void AI()
		{
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

				if (num416 > 1) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}

			++Projectile.localAI[1];
		}
	}
}
