﻿using Microsoft.Xna.Framework;
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
			_splitTimer = System.Math.Max(_splitTimer--, 0);

			projectile.Size = new Vector2(10, 10) * projectile.scale;

			Vector2 nearestCenter = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center;
			projectile.velocity += projectile.DirectionTo(nearestCenter) * 0.15f;

			float speed = (((1 - (projectile.scale / 20f)) * 8f) + 0.5f) * (1 + (_splitTimer / 20f));
			if (projectile.velocity.Length() > speed)
				projectile.velocity = Vector2.Normalize(projectile.velocity) * speed;

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
			_splitTimer = 20;

			projectile.scale /= 2f;
			if (projectile.scale <= 0.5f)
			{
				projectile.Kill();
				return -1;
			}

			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.PiOver2) * 2f;

			int newProj = Projectile.NewProjectile(projectile.position, -projectile.velocity, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
			Main.projectile[newProj].timeLeft = projectile.timeLeft;
			Main.projectile[newProj].scale = projectile.scale;
			return newProj;
		}
	}
}