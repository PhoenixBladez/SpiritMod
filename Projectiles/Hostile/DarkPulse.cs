using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class DarkPulse : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Pulse");
		}

		public override void SetDefaults()
		{
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 300;
			projectile.height = 20;
			projectile.width = 20;
			projectile.hide = true;
			aiType = ProjectileID.DeathLaser;
		}

		int timer = 1;
		public override void AI()
		{
			Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3F, 0.06F, 0.05F);
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.velocity *= 0.98f;
			int num623 = Dust.NewDust(projectile.Center, projectile.width, projectile.height,
				173, 0f, 0f, 100, default(Color), 2f);
			Main.dust[num623].noGravity = true;
			Main.dust[num623].velocity *= 0f;
			Main.dust[num623].scale *= 0.8f;
		}

		public override void Kill(int timeLeft)
		{
			int num624 = Dust.NewDust(projectile.Center, projectile.width, projectile.height,
				173, 0f, 0f, 100, default(Color), 3f);
			Main.dust[num624].velocity *= 0f;
			Main.dust[num624].scale *= 0.3f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(8) == 1)
				target.AddBuff(mod.BuffType("Shadowflame"), 200);
		}

	}
}
