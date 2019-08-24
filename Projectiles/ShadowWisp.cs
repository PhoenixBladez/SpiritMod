using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShadowWisp : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Wisp");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 120;
			projectile.height = 20;
			projectile.width = 20;
			projectile.hide = true;
			aiType = ProjectileID.Bullet;
		}

		int timer = 1;
		public override void AI()
		{
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3F, 0.06F, 0.05F);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.velocity *= 0.98f;
			int num623 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 100, default(Color), 2f);
			Main.dust[num623].noGravity = true;
			Main.dust[num623].velocity *= 0f;
			Main.dust[num623].scale *= 0.8f;
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 100, default(Color), 3f);
			Main.dust[num624].velocity *= 0f;
			Main.dust[num624].scale *= 0.3f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 1)
				target.AddBuff(BuffID.ShadowFlame, 200);
		}
	}
}
