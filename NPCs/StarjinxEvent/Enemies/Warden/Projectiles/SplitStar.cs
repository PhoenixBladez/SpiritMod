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
		private int _splitTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(10, 10);
			Projectile.hostile = true;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			_splitTimer = System.Math.Max(_splitTimer--, 0);

			Projectile.Size = new Vector2(10, 10) * Projectile.scale;

			Vector2 nearestCenter = Main.player[Player.FindClosest(Projectile.position, Projectile.width, Projectile.height)].Center;
			Projectile.velocity += Projectile.DirectionTo(nearestCenter) * 0.15f;

			float speed = (((1 - (Projectile.scale / 20f)) * 8f) + 0.5f) * (1 + (_splitTimer / 20f));
			if (Projectile.velocity.Length() > speed)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * speed;

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

			Lighting.AddLight(Projectile.Center, Color.LightCyan.ToVector3() / 3);

			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f) * Projectile.scale, 25));
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

		/// <summary>Handles the splitting of this projectile.</summary>
		public int Split()
		{
			_splitTimer = 20;

			Projectile.scale /= 2f;
			if (Projectile.scale <= 0.5f)
			{
				Projectile.Kill();
				return -1;
			}

			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.PiOver2) * 2f;

			int newProj = Projectile.NewProjectile(Projectile.position, -Projectile.velocity, Projectile.type, Projectile.damage, Projectile.knockBack, Projectile.owner);
			Main.projectile[newProj].timeLeft = Projectile.timeLeft;
			Main.projectile[newProj].scale = Projectile.scale;
			return newProj;
		}
	}
}