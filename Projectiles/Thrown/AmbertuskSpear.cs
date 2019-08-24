using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Projectiles.Thrown
{
	public class AmbertuskSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ambertusk Spear");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;

			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
		}

		public override bool PreAI()
		{
			if (projectile.alpha > 0)
				projectile.alpha -= 25;
			else if (projectile.alpha < 0)
				projectile.alpha = 0;


			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 45f)
			{
				projectile.ai[1] = 45f;
				projectile.velocity.X = projectile.velocity.X * 0.98F;
				projectile.velocity.Y = projectile.velocity.Y + 0.35F;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("AmberFracture"), 240);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);

			return projHitbox.Intersects(targetHitbox);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, DustID.AmberBolt, 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("AmbertuskSpear"), 1, false, 0, false, false);

		}

	}
}
