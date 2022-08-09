using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet.Crimbine
{
	public class CrimbineAmalgam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Amalgam");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 300;
			Projectile.height = 40;
			Projectile.width = 40;
			AIType = ProjectileID.Bullet;
		}

		public override void AI()
		{
			Projectile.velocity *= .9994f;
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list)
			{
				if (Projectile != proj && proj.type == ModContent.ProjectileType<CrimbineBone>())
				{
					SoundEngine.PlaySound(SoundID.Item95, Projectile.Center);
					proj.Kill();
					Projectile.Kill();

					int n = Main.rand.Next(14, 17);
					for (int i = 0; i < n; i++)
					{
						float rotation = MathHelper.ToRadians(270 / n * i);
						Vector2 perturbedSpeed = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(rotation));
						perturbedSpeed.X *= Main.rand.NextFloat(5.5f, 7.5f);
						perturbedSpeed.Y *= Main.rand.NextFloat(8.5f, 10.5f);

						int projType = ModContent.ProjectileType<CrimbineSpine>();
						if (Main.rand.NextBool(8))
							projType = ModContent.ProjectileType<CrimbineHeart>();
						else if (Main.rand.NextBool(4))
							projType = ModContent.ProjectileType<CrimbineBlob>();

						int newProj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, projType, Projectile.damage / 5 * 6, 2, Projectile.owner);
					}
				}
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f)
			{
				Projectile.alpha += 5;
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}

			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f)
			{
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++)
				{
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f)
					{
						num416++;
						if (Main.projectile[num420].ai[1] > num418)
						{
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}
				}
				if (num416 > 1)
				{
					Main.projectile[num417].netUpdate = true;
					Main.projectile[num417].ai[1] = 36000f;
					return;
				}
			}
			//int num = 5;
			for (int k = 0; k < Main.rand.Next(6, 11); k++)
			{
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, 0.0f, 0.0f, 0, new Color(), 1f);
				Main.dust[index2].scale = Main.rand.NextFloat(.85f, 1.1f);
				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].alpha = 100;
				Main.dust[index2].noGravity = false;
				Main.dust[index2].noLight = false;
			}

		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int k = 0; k < 16; k++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Dirt, 2.5f * 1, -2.5f, 0, Color.White, 0.7f);
			return true;
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 26; k++)
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Blood, Projectile.oldVelocity.X * 0.5f, Projectile.oldVelocity.Y * 0.5f);
		}
	}
}