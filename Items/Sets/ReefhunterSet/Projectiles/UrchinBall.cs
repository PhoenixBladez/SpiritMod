using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
 	public class UrchinBall : ModProjectile
	{
		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Urchin");

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.aiStyle = 0;
		}

		public override bool CanDamage() => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			if (!hasTarget)
			{
				projectile.rotation += 0.06f * System.Math.Sign(projectile.velocity.X);
				projectile.velocity.Y += 0.2f;
			}
			else
			{
				NPC npc = Main.npc[(int)projectile.ai[1]];

				if (!npc.active)
				{
					projectile.netUpdate = true;
					projectile.tileCollide = true;
					projectile.timeLeft *= 2;

					hasTarget = false;
					return;
				}

				projectile.Center = npc.Center + relativePoint;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[1] = target.whoAmI;
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			projectile.timeLeft = 240;
			projectile.velocity = Vector2.Zero;

			hasTarget = true;
			relativePoint = projectile.Center - target.Center;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; ++i)
			{
				Vector2 vel = new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedBy(i * MathHelper.TwoPi / 8f).RotatedByRandom(0.3f);
				Projectile.NewProjectile(projectile.Center + Vector2.Normalize(relativePoint) * 6, vel, ModContent.ProjectileType<UrchinSpike>(), projectile.damage, 2f, projectile.owner);
			}
		}
	}

	public class UrchinSpike : ModProjectile
	{
		private bool hasTarget = false;
		private Vector2 relativePoint = Vector2.Zero;

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.aiStyle = 0;
		}

		public override bool CanDamage() => !hasTarget;
		public override bool? CanCutTiles() => !hasTarget;

		public override void AI()
		{
			if (!hasTarget)
			{
				if (projectile.ai[0]++ > 50)
				{
					projectile.velocity.X *= 0.97f;
					projectile.velocity.Y += 0.4f;
				}

				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
				projectile.velocity.Y += 0.05f;
			}
			else
			{
				NPC npc = Main.npc[(int)projectile.ai[1]];

				projectile.alpha = 255 - (int)(projectile.timeLeft / 60f * 255);

				if (!npc.active)
					projectile.Kill();
				else
					projectile.Center = npc.Center + relativePoint;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[1] = target.whoAmI;
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			projectile.timeLeft = 60;
			projectile.velocity = Vector2.Zero;
			projectile.penetrate++;

			hasTarget = true;
			relativePoint = projectile.Center - target.Center;
		}
	}
}
