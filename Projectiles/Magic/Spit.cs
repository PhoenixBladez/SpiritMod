using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Spit : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diseased Spit");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 10;
			projectile.height = 10;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 60;
		}
		int counter;
		public override bool PreAI()
		{
			int num = 5;
			for(int k = 0; k < 6; k++) {
				int index2 = Dust.NewDust(projectile.position, 1, 1, 184, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].position = projectile.Center - projectile.velocity / num * (float)k;
				Main.dust[index2].scale = .75f;
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].noGravity = true;
				Main.dust[index2].noLight = false;
			}
			counter++;
			if(counter >= 1440) {
				counter = -1440;
			}
			for(int i = 0; i < 20; i++) {
				float x = projectile.Center.X - projectile.velocity.X / 10f * (float)i;
				float y = projectile.Center.Y - projectile.velocity.Y / 10f * (float)i;

				int num2121 = Dust.NewDust(projectile.Center + new Vector2(0, (float)Math.Cos(counter / 4.2f) * 9.2f).RotatedBy(projectile.rotation), 6, 6, 184, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num2121].velocity *= 0f;
				Main.dust[num2121].scale *= .75f;
				Main.dust[num2121].noGravity = true;

			}

			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(target.life <= 0) {
				int num24 = 307;
				int num3 = Main.rand.Next(0, 360);
				for(int j = 0; j < 1; j++) {
					float num4 = MathHelper.ToRadians((float)(270 / 1 * j + num3));
					Vector2 vector = new Vector2(base.projectile.velocity.X, base.projectile.velocity.Y).RotatedBy((double)num4, default(Vector2));
					vector.Normalize();
					vector.X *= 4.5f;
					vector.Y *= 4.5f;
					int p = Projectile.NewProjectile(base.projectile.Center.X, base.projectile.Center.Y, vector.X, vector.Y, num24, projectile.damage / 5 * 4, 0f, 0);
					Main.projectile[p].hostile = false;
					Main.projectile[p].friendly = true;
					Main.projectile[p].magic = true;
					Main.projectile[p].melee = false;
				}
			}
		}
	}
}
