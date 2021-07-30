
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class RuneHeart : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rune Heart");

		public override void SetDefaults()
		{
			projectile.Size = Vector2.One * 18;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 360;
			projectile.scale = Main.rand.NextFloat(1.3f, 1.5f);
		}

		public override bool CanDamage() => false;

		private ref float AiState => ref projectile.ai[0];
		private ref float AiTimer => ref projectile.ai[1];

		private const float STATE_SLOWDOWN = 0;
		private const float STATE_LOCKON = 1;
		private const float STATE_FASTHOME = 2;

		private const int FADETIME = 30;
		private const int LOCKONTIME = 30;

		private Player Owner => Main.player[projectile.owner];

		public override void AI()
		{
			if(Owner.dead || !Owner.active)
			{
				projectile.Kill();
				return;
			}

			projectile.alpha = Math.Max(projectile.alpha - (255 / (FADETIME + LOCKONTIME)), 0);
			switch (AiState)
			{
				case STATE_SLOWDOWN:
					projectile.velocity *= 0.97f;
					if(AiTimer >= FADETIME)
					{
						AiState = STATE_LOCKON;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;
				case STATE_LOCKON:
					projectile.velocity = projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Owner.Center) * projectile.velocity.Length(), 0.08f));
					if (AiTimer >= LOCKONTIME)
					{
						AiState = STATE_FASTHOME;
						AiTimer = 0;
						projectile.netUpdate = true;
					}
					break;
				case STATE_FASTHOME:
					float homeStrength = MathHelper.Lerp(0.03f, 0.2f, Math.Min(AiTimer / 120, 1));
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Owner.Center) * 14, homeStrength);
					if (projectile.Hitbox.Intersects(Owner.Hitbox))
					{
						projectile.Kill();

						if (!Main.dedServ)
						{
							Particles.ParticleHandler.SpawnParticle(new Particles.PulseCircle(Owner.Center, Color.Lerp(new Color(252, 3, 102), Color.White, 0.25f), 70, 20));
							Main.PlaySound(SoundID.Item29.WithPitchVariance(0.2f).WithVolume(0.6f), Owner.Center);
						}

						int healAmount = Math.Min(projectile.damage, Owner.statLifeMax2 - Owner.statLife);
						if (healAmount == 0)
							return;

						Owner.statLife += healAmount;
						Owner.HealEffect(healAmount);
					}
					break;
			}

			AiTimer++;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.Lerp(new Color(252, 3, 102), Color.White, 0.25f) * 0.1f), new RoundCap(), new ArrowGlowPosition(), 80, 120);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(252, 3, 102) * 0.2f), new RoundCap(), new ArrowGlowPosition(), 40, 80);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D projTex = Main.projectileTexture[projectile.type];
			Color color = Color.Lerp(new Color(252, 3, 102), Color.White, 1 - projectile.Opacity);
			void DrawTex(Texture2D tex, Vector2 position, Color drawcolor, float Scale = 1f) =>
				spriteBatch.Draw(tex, position, null, drawcolor * projectile.Opacity, 0, projTex.Size() / 2, projectile.scale * Scale, SpriteEffects.None, 0);

			float Timer = (float)(Math.Sin(Main.GameUpdateCount / 15f) / 2) + 0.5f;
			for (int i = 0; i < 5; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy((i / 5f) * MathHelper.TwoPi) * Timer * 8;
				DrawTex(ModContent.GetTexture(Texture + "_outline"), projectile.Center + offset - Main.screenPosition, color * (1 - Timer));
			}
			DrawTex(projTex, projectile.Center - Main.screenPosition, Color.Lerp(color, Color.White, 0.25f));

			DrawTex(ModContent.GetTexture(Texture + "_mask"), projectile.Center - Main.screenPosition, color * (1 - projectile.Opacity) * 2);
			return false;
		}
	}
}
