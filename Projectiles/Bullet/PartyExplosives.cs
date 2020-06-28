using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	public class PartyExplosives : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Party Explosives");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 90;
			projectile.alpha = 255;
			projectile.height = 8;
			projectile.width = 8;
			aiType = ProjectileID.Bullet;

		}

		int timer = 1;
		public override void AI()
		{
			timer++;
			if(timer >= Main.rand.Next(60, 90)) {
				projectile.Kill();
			}
			Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
			projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			projectile.velocity.X *= .96f;
			projectile.velocity.Y *= .96f;
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			projectile.ai[1] += 1f;
			if(projectile.ai[1] >= 7200f) {
				projectile.alpha += 5;
				if(projectile.alpha > 255) {
					projectile.alpha = 255;
					projectile.Kill();
				}
			}

			projectile.localAI[0] += 1f;
			if(projectile.localAI[0] >= 10f) {
				projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = projectile.type;
				for(int num420 = 0; num420 < 1000; num420++) {
					if(Main.projectile[num420].active && Main.projectile[num420].owner == projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if(Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if(num416 > 22) {
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate {
					for(int i = 0; i < 10; i++) {
						int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 2f);
						Main.dust[num].noGravity = true;
						Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(77, Main.LocalPlayer);
						Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
						Main.dust[num].scale *= .25f;
						if(Main.dust[num].position != projectile.Center)
							Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
					}
				});
		}
	}
}
