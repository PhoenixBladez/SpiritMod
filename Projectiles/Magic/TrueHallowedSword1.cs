using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class TrueHallowedSword1 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword Apparition");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 20;
			projectile.height = 64;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 180;
		}

		public override bool PreAI()
		{
			projectile.tileCollide = false;

			if (Main.rand.Next(4) == 0)
			{
				int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 58, 0f, 0f);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 57, 0f, 0f); //to make some with gravity to fly all over the place :P
			}

			projectile.velocity.Y += projectile.ai[0];
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;

			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 2;
			}
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			Dust.NewDust(projectile.position + projectile.velocity * 0, projectile.width, projectile.height, 58, projectile.oldVelocity.X * 0, projectile.oldVelocity.Y * 0);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 58, projectile.oldVelocity.X * 0.3f, projectile.oldVelocity.Y * 0.3f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("HallowedLight1"), 120, true);
		}

	}
}