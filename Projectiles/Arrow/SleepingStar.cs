using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Mechanics.Trails;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class SleepingStar : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sleeping Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		private bool purple; //just used for client end things, shouldnt matter if synced or not

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.Size = new Vector2(10, 10);
			purple = Main.rand.NextBool();
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new StandardColorTrail(purple ? new Color(218, 94, 255) : new Color(120, 217, 255)), new RoundCap(), new SleepingStarTrailPosition(), 8f, 250f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(ModContent.BuffType<StarFlame>(), 200, true);
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
			int dusttype = purple ? 223 : 180;
			float dustscale = purple ? 0.6f : 1.2f;
			{
				for (int i = 0; i < 40; i++)
				{
					int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dusttype, 0f, -2f, 0, default, dustscale);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X = dust.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != Projectile.Center)
					{
						Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
					}
				}
			}
		}

		private bool looping = false;
		private int loopCounter = 0;
		private int loopSize = 9;
		public override void AI()
		{
			Lighting.AddLight((int)(Projectile.position.X / 16f), (int)(Projectile.position.Y / 16f), 0.396f, 0.170588235f, 0.564705882f);
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			if (!looping) //change direction slightly
			{
				Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
				Projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
			}
			if (Main.rand.Next(100) == 1 && !looping)
			{
				loopCounter = 0;
				looping = true;
				loopSize = Main.rand.Next(8, 14);
			}
			if (looping)
			{
				Vector2 currentSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
				Projectile.velocity = currentSpeed.RotatedBy(Math.PI / loopSize);
				loopCounter++;
				if (loopCounter >= loopSize * 2)
				{
					looping = false;
					loopSize = Main.rand.Next(8, 13);
				}
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
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(160, 160, 160, 100);
	}
}