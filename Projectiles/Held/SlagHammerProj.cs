using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;

		}
		public override void SetDefaults()
		{
			projectile.width = 90;
			projectile.height = 90;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.ownerHitCheck = true;
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
			int dust = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Flare);
			Main.dust[dust].velocity *= -1f;
			Main.dust[dust].noGravity = true;
			Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
			vector2_1.Normalize();
			Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
			Main.dust[dust].velocity = vector2_2;
			vector2_2.Normalize();
			Vector2 vector2_3 = vector2_2 * 34f;
			Main.dust[dust].position = projectile.Center - vector2_3;

			alphaCounter += 0.08f;
			Player player = Main.player[projectile.owner];

			if (!released)
				projectile.scale = MathHelper.Clamp(projectile.ai[0] / 10, 0, 1);

			if (player.direction == 1)
				radians += ((projectile.ai[0] + 10) / 200);
			else
				radians -= ((projectile.ai[0] + 10) / 200);

			if (radians > 6.28)
				radians -= 6.28;
			if (radians < -6.28)
				radians += 6.28;

			projectile.velocity = Vector2.Zero;

			if (projectile.ai[0] % 20 == 0)
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 19).WithPitchVariance(0.1f).WithVolume(0.5f), projectile.Center);

			if (projectile.ai[0] < chargeTime)
			{
				projectile.ai[0] += 0.7f;
				if (projectile.ai[0] >= chargeTime)
					Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
			}
			Vector2 direction = Main.MouseWorld - player.position;
			direction.Normalize();
			double throwingAngle = direction.ToRotation() + 3.14;
			projectile.position = player.Center - (Vector2.UnitX.RotatedBy(radians) * 40) - (projectile.Size / 2);
			player.itemTime = 4;
			player.itemAnimation = 4;
			player.itemRotation = MathHelper.WrapAngle((float)radians);
			if (player.whoAmI == Main.myPlayer)
				player.ChangeDir(Math.Sign(direction.X));
			if (player.direction != 1)
				throwingAngle -= 6.28;

			if ((!player.channel && Math.Abs(radians - throwingAngle) < 1) || released)
			{
				if (projectile.ai[0] < chargeTime || released)
				{
					released = true;
					projectile.scale -= 0.15f;
					if (projectile.scale < 0.15f)
						projectile.active = false;
				}
				else
				{
					projectile.active = false;
					direction *= 16;
					Projectile.NewProjectile(player.Center, direction, ModContent.ProjectileType<SlagHammerProjReturning>(), (int)(projectile.damage * 1.5f), projectile.knockBack, projectile.owner);
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = lightColor;
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, 0, width, height), color, (float)radians + 3.9f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 0);
			if (projectile.ai[0] >= chargeTime && projectile.ai[1] == 0)
			{
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, height, width, height), Color.White * 0.9f, (float)radians + 3.9f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 1);

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
					Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center - Main.screenPosition, new Rectangle(0, height * 2, width, height), color * alpha, (float)radians + 3.9f, new Vector2(0, height), projectile.scale, SpriteEffects.None, 1);
				}
			}
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.ai[1] == 0)
			{
				Player player = Main.player[projectile.owner];
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

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.ai[0] > 10)
			{
				float sineAdd = (float)Math.Sin(alphaCounter) + 2.5f;
				Main.spriteBatch.Draw(SpiritMod.Instance.GetTexture("Effects/Masks/Extra_49"), projectile.Center - Main.screenPosition, null, new Color((int)(16.5f * sineAdd), (int)(5.5f * sineAdd), (int)(0 * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
			}
		}
	}
}