using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Sword.Artifact
{
	public class Thanos2Proj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thanos Afterimage");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 22;
			projectile.aiStyle = 113;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 600;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			projectile.light = 0;
			aiType = ProjectileID.Shuriken;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();
			else
			{
				aiType = ProjectileID.Shuriken;
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.velocity *= 0.75f;
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.25f, projectile.height * 0.25f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(15) == 0)
				target.AddBuff(mod.BuffType("Crystallize"), 240, true);
		}

		public override void AI()
		{
			projectile.tileCollide = true;
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, mod.DustType("Crystal"), 0f, 0f);
			Main.dust[dust2].scale = 1.0f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].velocity *= 0f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y, 0);
			int newDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Crystal"), 0f, 0f, 0, default(Color), 1f);
			Main.dust[newDust].scale = 2f;
			int newDust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Crystal"), 0f, 0f, 0, default(Color), 1f);
			Main.dust[newDust1].scale = 2f;
			int newDust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Crystal"), 0f, 0f, 0, default(Color), 1f);
			Main.dust[newDust2].scale = 2f;
			int newDust3 = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Crystal"), 0f, 0f, 0, default(Color), 1f);
			Main.dust[newDust3].scale = 2f;
		}

	}
}
