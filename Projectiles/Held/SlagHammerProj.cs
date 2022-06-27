using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using SpiritMod.Projectiles.Returning;
using Terraria.Audio;

namespace SpiritMod.Projectiles.Held
{
	public class SlagHammerProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slag Breaker");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;

		}
		public override void SetDefaults()
		{
			Projectile.width = 90;
			Projectile.height = 90;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
		}

		readonly int height = 60;
		readonly int width = 60;
		double radians = 0;
		int flickerTime = 0;
		float alphaCounter = 0;
		readonly int chargeTime = 50;
		bool released = false;

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Flare);
			Main.dust[dust].velocity *= -1f;
			Main.dust[dust].noGravity = true;
			Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
			vector2_1.Normalize();
			Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
			Main.dust[dust].velocity = vector2_2;
			vector2_2.Normalize();
			Vector2 vector2_3 = vector2_2 * 34f;
			Main.dust[dust].position = Projectile.Center - vector2_3;

			alphaCounter += 0.08f;
			Player player = Main.player[Projectile.owner];

			if (!released)
				Projectile.scale = MathHelper.Clamp(Projectile.ai[0] / 10, 0, 1);

			if (player.direction == 1)
				radians += ((Projectile.ai[0] + 10) / 200);
			else
				radians -= ((Projectile.ai[0] + 10) / 200);

			if (radians > 6.28)
				radians -= 6.28;
			if (radians < -6.28)
				radians += 6.28;

			Projectile.velocity = Vector2.Zero;

			if (Projectile.ai[0] % 20 == 0)
				SoundEngine.PlaySound(SoundID.Item19 with { PitchVariance = 0.1f, Volume = 0.5f }, Projectile.Center);

			if (Projectile.ai[0] < chargeTime)
			{
				Projectile.ai[0] += 0.7f;
				if (Projectile.ai[0] >= chargeTime)
					SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
			}
			Vector2 direction = Main.MouseWorld - player.position;
			direction.Normalize();
			double throwingAngle = direction.ToRotation() + 3.14;
			Projectile.position = player.Center - (Vector2.UnitX.RotatedBy(radians) * 40) - (Projectile.Size / 2);
			player.itemTime = 4;
			player.itemAnimation = 4;
			player.itemRotation = MathHelper.WrapAngle((float)radians);
			if (player.whoAmI == Main.myPlayer)
				player.ChangeDir(Math.Sign(direction.X));
			if (player.direction != 1)
				throwingAngle -= 6.28;

			if ((!player.channel && Math.Abs(radians - throwingAngle) < 1) || released)
			{
				if (Projectile.ai[0] < chargeTime || released)
				{
					released = true;
					Projectile.scale -= 0.15f;
					if (Projectile.scale < 0.15f)
						Projectile.active = false;
				}
				else
				{
					Projectile.active = false;
					direction *= 16;
					Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, direction, ModContent.ProjectileType<SlagHammerProjReturning>(), (int)(Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Color color = lightColor;
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 0);
			if (Projectile.ai[0] >= chargeTime && Projectile.ai[1] == 0)
			{
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), Color.White * 0.9f, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);

				if (flickerTime < 16)
				{
					flickerTime++;
					color = Color.White;
					float flickerTime2 = (flickerTime / 20f);
					float alpha = 1.5f - (((flickerTime2 * flickerTime2) / 2) + (2f * flickerTime2));
					if (alpha < 0)
					{
						alpha = 0;
					}
					Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Main.player[Projectile.owner].Center - Main.screenPosition, new Rectangle(0, height * 2, width, height), color * alpha, (float)radians + 3.9f, new Vector2(0, height), Projectile.scale, SpriteEffects.None, 1);
				}
			}
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.ai[1] == 0)
			{
				Player player = Main.player[Projectile.owner];
				if (target.Center.X > player.Center.X)
					hitDirection = 1;
				else
					hitDirection = -1;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(6) == 2)
				target.AddBuff(BuffID.OnFire, 180);
		}

		public override void PostDraw(Color lightColor)
		{
			if (Projectile.ai[0] > 10)
			{
				float sineAdd = (float)Math.Sin(alphaCounter) + 2.5f;
				Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), Projectile.Center - Main.screenPosition, null, new Color((int)(16.5f * sineAdd), (int)(5.5f * sineAdd), (int)(0 * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
		}
	}
}