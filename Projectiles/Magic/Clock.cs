using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class Clock : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clock");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = false;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 209;
			Projectile.tileCollide = false;
		}

		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w += 8) {
				int dust = Dust.NewDust(Vector2.Lerp(from, to, w * step) - new Vector2(5, 5), 10, 10, DustID.UnusedWhiteBluePurple, 0, 0);
				Main.dust[dust].scale = 1.25f;
				Main.dust[dust].velocity = Vector2.Zero;
			}
		}

		//int counter = -720;
		bool boom = false;
		private float distortStrength = 450f;

		int minuteHand = -90;
		int hourHand = -90;
		int minuteLength = 168;
		//int hourLength = 125;
		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft > 35) {
				Rectangle? sourceRectangle = null;
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SpiritMod/Effects/LargeHand", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(100, 185, 205, 0), minuteHand * (float)(Math.PI / 180), new Vector2(23, 148), 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SpiritMod/Effects/SmallHand", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition, sourceRectangle, new Color(100, 185, 205, 0), hourHand * (float)(Math.PI / 180), new Vector2(20, 88), 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreAI()
		{
			if (Projectile.timeLeft <= 35) {
				if (!boom) {
					if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive()) {
						Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(3, 15, 15).UseTargetPosition(Projectile.Center);
					}

					boom = true;
				}
				if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive()) {
					float progress = (35f - Projectile.timeLeft) / 60f; // Will range from -3 to 3, 0 being the point where the bomb explodes.
					Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(distortStrength * (1 - progress / 3f));
				}
			}
			else {
				minuteHand += 5;
				hourHand += 1;
				/*	Trail(projectile.Center, projectile.Center + new Vector2((float)Math.Sin(minuteHand * (Math.PI / 180)) * minuteLength, (float)Math.Cos(minuteHand * (Math.PI / 180)) * minuteLength));
                    Trail(projectile.Center, projectile.Center + new Vector2((float)Math.Sin(hourHand * (Math.PI / 180)) * hourLength, (float)Math.Cos(hourHand * (Math.PI / 180)) * hourLength));*/
				for (int i = 0; i < 360; i += 6) {
					Dust dust2 = Dust.NewDustPerfect(Projectile.Center + new Vector2((float)Math.Sin(i * (Math.PI / 180)) * minuteLength, (float)Math.Cos(i * (Math.PI / 180)) * minuteLength), 206);
					//dust2.alpha = fadeIn;
					dust2.velocity = Vector2.Zero;
					if (i % 30 == 0) {
						for (int j = minuteLength; j > minuteLength - 21; j -= 3) {
							Dust dust = Dust.NewDustPerfect(Projectile.Center + new Vector2((float)Math.Sin(i * (Math.PI / 180)) * j, (float)Math.Cos(i * (Math.PI / 180)) * j), 206);
							dust.velocity = Vector2.Zero;
							dust.scale = 1.5f;
							//dust.alpha = fadeIn;
						}
					}
				}
			}

			Projectile.ai[1] += 1f;
			if (Projectile.ai[1] >= 7200f) {
				Projectile.alpha += 5;
				if (Projectile.alpha > 255) {
					Projectile.alpha = 255;
					Projectile.Kill();
				}
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] >= 10f) {
				Projectile.localAI[0] = 0f;
				int num416 = 0;
				int num417 = 0;
				float num418 = 0f;
				int num419 = Projectile.type;
				for (int num420 = 0; num420 < 1000; num420++) {
					if (Main.projectile[num420].active && Main.projectile[num420].owner == Projectile.owner && Main.projectile[num420].type == num419 && Main.projectile[num420].ai[1] < 3600f) {
						num416++;
						if (Main.projectile[num420].ai[1] > num418) {
							num417 = num420;
							num418 = Main.projectile[num420].ai[1];
						}
					}

					if (num416 > 1) {
						Main.projectile[num417].netUpdate = true;
						Main.projectile[num417].ai[1] = 36000f;
						return false;
					}
				}
			}

			return false;
		}
		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive()) {
				Filters.Scene["Shockwave"].Deactivate();
			}
		}
	}
}
