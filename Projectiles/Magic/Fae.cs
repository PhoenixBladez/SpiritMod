using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Fae : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fae Bolt");
		}

		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 26;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 1000;
			projectile.alpha = 255;
			projectile.light = 0f;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			aiType = ProjectileID.Bullet;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return true;
		}

		public override bool PreAI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 0.5f, 0.5f, 0.9f);

			for (int i = 0; i < 10; i++)
			{
				float x = projectile.Center.X - projectile.velocity.X * .1f * i;
				float y = projectile.Center.Y - projectile.velocity.Y * .1f * i;
				int num = Dust.NewDust(new Vector2(x, y), 1, 1, 242);
				Main.dust[num].alpha = projectile.alpha;
				Main.dust[num].position.X = x;
				Main.dust[num].position.Y = y;
				Main.dust[num].velocity *= 0f;
				Main.dust[num].noGravity = true;
			}

			projectile.velocity.Y += 0.4F;
			projectile.velocity.X *= 1.005F;
			projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -10, 10);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);

			for (int i = 0; i < 40; i++)
			{
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height,
					242, 0f, -2f, 0, default(Color), 2f);
				Main.dust[num].noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 0)
				target.AddBuff(mod.BuffType("HolyLight"), 120, true);
		}

	}
}