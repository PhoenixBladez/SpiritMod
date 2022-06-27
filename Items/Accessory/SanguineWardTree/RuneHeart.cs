using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class RuneHeart : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Rune Heart");

		public override void SetDefaults()
		{
			Projectile.Size = Vector2.One * 18;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 360;
			Projectile.scale = Main.rand.NextFloat(1f, 1.2f);
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		private ref float AiState => ref Projectile.ai[0];
		private ref float AiTimer => ref Projectile.ai[1];

		private const float STATE_SLOWDOWN = 0;
		private const float STATE_LOCKON = 1;
		private const float STATE_FASTHOME = 2;

		private const int FADETIME = 30;
		private const int LOCKONTIME = 30;

		private Player Owner => Main.player[Projectile.owner];

		public override void AI()
		{
			if(Owner.dead || !Owner.active)
			{
				Projectile.Kill();
				return;
			}

			Projectile.alpha = Math.Max(Projectile.alpha - (255 / (FADETIME + LOCKONTIME)), 0);
			switch (AiState)
			{
				case STATE_SLOWDOWN:
					Projectile.velocity *= 0.97f;
					if(AiTimer >= FADETIME)
					{
						AiState = STATE_LOCKON;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;
				case STATE_LOCKON:
					Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Owner.Center) * Projectile.velocity.Length(), 0.08f));
					if (AiTimer >= LOCKONTIME)
					{
						AiState = STATE_FASTHOME;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}
					break;
				case STATE_FASTHOME:
					float homeStrength = MathHelper.Lerp(0.03f, 0.2f, Math.Min(AiTimer / 120, 1));
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Owner.Center) * 14, homeStrength);
					if (Projectile.Hitbox.Intersects(Owner.Hitbox))
					{
						Projectile.Kill();

						if (!Main.dedServ)
						{
							Particles.ParticleHandler.SpawnParticle(new Particles.PulseCircle(Owner, Color.Lerp(new Color(252, 3, 102), Color.White, 0.25f), 70, 20));
							SoundEngine.PlaySound(SoundID.Item29.WithPitchVariance(0.2f).WithVolume(0.6f), Owner.Center);
						}

						int healAmount = Math.Min(Projectile.damage, Owner.statLifeMax2 - Owner.statLife);
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
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, Color.Lerp(new Color(252, 3, 102), Color.White, 0.25f) * 0.1f), new RoundCap(), new ArrowGlowPosition(), 64, 96);
			tM.CreateTrail(Projectile, new OpacityUpdatingTrail(Projectile, new Color(252, 3, 102) * 0.2f), new RoundCap(), new ArrowGlowPosition(), 32, 64);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D projTex = TextureAssets.Projectile[Projectile.type].Value;
			Color color = Color.Lerp(new Color(252, 3, 102), Color.White, 1 - Projectile.Opacity);
			void DrawTex(Texture2D tex, Vector2 position, Color drawcolor, float Scale = 1f) =>
				spriteBatch.Draw(tex, position, null, drawcolor * Projectile.Opacity, 0, projTex.Size() / 2, Projectile.scale * Scale, SpriteEffects.None, 0);

			float Timer = (float)(Math.Sin(Main.GameUpdateCount / 15f) / 2) + 0.5f;
			for (int i = 0; i < 5; i++)
			{
				Vector2 offset = Vector2.UnitX.RotatedBy((i / 5f) * MathHelper.TwoPi) * Timer * 8;
				DrawTex(ModContent.Request<Texture2D>(Texture + "_outline"), Projectile.Center + offset - Main.screenPosition, color * (1 - Timer));
			}
			DrawTex(projTex, Projectile.Center - Main.screenPosition, Color.Lerp(color, Color.White, 0.25f));

			DrawTex(ModContent.Request<Texture2D>(Texture + "_mask"), Projectile.Center - Main.screenPosition, color * (1 - Projectile.Opacity) * 2);
			return false;
		}
	}
}
