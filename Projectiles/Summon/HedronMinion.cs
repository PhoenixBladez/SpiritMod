using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HedronMinion : ModProjectile
	{
		float localaione = 0;
		float localaizero = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hedron");
			Main.projFrames[base.projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 46;
			projectile.timeLeft = 10000;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.penetrate = 12;
			projectile.ignoreWater = true;
			projectile.minion = true;
			projectile.sentry = true;
			projectile.minionSlots = 0;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			ProjectileExtras.Explode(projectile.whoAmI, 120, 120,
				delegate
			{
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("SpiritBoom"), (int)(projectile.damage), 0, Main.myPlayer);
				for (int i = 0; i < 15; i++)
				{
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
				}
			});
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 4)
			{
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 4)
					projectile.frame = 0;
			}

			if (localaizero == 0f)
			{
				localaizero = projectile.Center.Y;
				projectile.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (projectile.Center.Y >= localaizero)
			{
				localaione = -1f;
				projectile.netUpdate = true;
			}
			if (projectile.Center.Y <= localaizero - 25f)
			{
				localaione = 1f;
				projectile.netUpdate = true;
			}
			projectile.velocity.Y = MathHelper.Clamp(projectile.velocity.Y + 0.05f * localaione, -2f, 2f);
		}

	}
}