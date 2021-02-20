using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class SlagHammerProjReturning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Breaker");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 45;
			projectile.height = 45;
			projectile.aiStyle = 3;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 700;
			projectile.extraUpdates = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			if (projectile.tileCollide)
			{
				player.GetModPlayer<MyPlayer>().Shake += 8;
				Main.PlaySound(SoundID.Item88, projectile.Center);
			}
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.OnFire, 120, true);
			{
				int n = 4;
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 2.5f;
					perturbedSpeed.Y *= 2.5f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.Spark, projectile.damage / 2, 2, projectile.owner);
				}
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.tileCollide)
				damage = (int)(damage * 1.5);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[projectile.owner];
			player.GetModPlayer<MyPlayer>().Shake += 8;
			Main.PlaySound(SoundID.Item88, projectile.Center);
			return base.OnTileCollide(oldVelocity);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[projectile.type]; i++) {
				float opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				opacity *= 0.5f;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.oldPos[i] + projectile.Size/2 - Main.screenPosition, null, projectile.GetAlpha(lightColor) * opacity, projectile.oldRot[i],
					Main.projectileTexture[projectile.type].Size() / 2, projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = height /= 2;
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2.5f;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), projectile.Center - Main.screenPosition, null, new Color((int)(16.5f * sineAdd), (int)(5.5f * sineAdd), (int)(0 * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
		}
		float alphaCounter = 0;
		public override void AI()
		{
			alphaCounter += 0.08f;
			int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, 127);
			Main.dust[d].noGravity = true;
		}

	}
}
