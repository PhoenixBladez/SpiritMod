using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden.Projectiles
{
	public class SplitStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(10, 10);
			projectile.hostile = true;
			projectile.timeLeft = 60;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.velocity = projectile.DirectionTo(Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center) * (1 - (projectile.scale / 20f));
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(projectile.Center, Color.LightCyan.ToVector3() / 3);

			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(projectile.Center, projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f) * projectile.scale, 25));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * projectile.Opacity;

		/// <summary>Handles the splitting of this projectile.</summary>
		public int Split()
		{
			projectile.scale /= 2f;

			if (projectile.scale <= 0.5f)
			{
				projectile.Kill();
				return -1;
			}

			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.PiOver2);

			int newProj = Projectile.NewProjectile(projectile.position, -projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
			return newProj;
		}
	}
}