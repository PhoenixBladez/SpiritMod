using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Returning
{
	public class SlagHammerProjReturning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Breaker");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 45;
			Projectile.height = 45;
			Projectile.aiStyle = 3;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 700;
			Projectile.extraUpdates = 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			if (Projectile.tileCollide)
			{
				player.GetModPlayer<MyPlayer>().Shake += 8;
				SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
			}
			if (Main.rand.Next(6) == 0)
				target.AddBuff(BuffID.OnFire, 120, true);
			{
				int n = 4;
				int deviation = Main.rand.Next(0, 300);
				for (int i = 0; i < n; i++) {
					float rotation = MathHelper.ToRadians(270 / n * i + deviation);
					Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation);
					perturbedSpeed.Normalize();
					perturbedSpeed.X *= 2.5f;
					perturbedSpeed.Y *= 2.5f;
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.Spark, Projectile.damage / 2, 2, Projectile.owner);
				}
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.tileCollide)
				damage = (int)(damage * 1.5);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[Projectile.owner];
			player.GetModPlayer<MyPlayer>().Shake += 8;
			SoundEngine.PlaySound(SoundID.Item88, Projectile.Center);
			return base.OnTileCollide(oldVelocity);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			for(int i = 0; i < ProjectileID.Sets.TrailCacheLength[Projectile.type]; i++) {
				float opacity = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - i) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
				opacity *= 0.5f;
				spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.oldPos[i] + Projectile.Size/2 - Main.screenPosition, null, Projectile.GetAlpha(lightColor) * opacity, Projectile.oldRot[i],
					TextureAssets.Projectile[Projectile.type].Value.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height /= 2;
			return true;
		}
		public override void PostDraw(Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 2.5f;
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[49].Value, Projectile.Center - Main.screenPosition, null, new Color((int)(16.5f * sineAdd), (int)(5.5f * sineAdd), (int)(0 * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
		}
		float alphaCounter = 0;
		public override void AI()
		{
			alphaCounter += 0.08f;
			int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare);
			Main.dust[d].noGravity = true;
		}

	}
}
