
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class RuneHeart : ModProjectile, IDrawAdditive, ITrailProjectile
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
			projectile.scale = Main.rand.NextFloat(0.7f, 0.9f);
			projectile.hide = true;
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
					projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Owner.Center), 0.08f);
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
							Main.PlaySound(SoundID.Item29.WithPitchVariance(0.2f).WithVolume(0.6f), Owner.Center);

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
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, Color.Lerp(new Color(252, 3, 102), Color.White, 0.5f) * 0.33f), new RoundCap(), new ArrowGlowPosition(), 40, 120);
			tM.CreateTrail(projectile, new OpacityUpdatingTrail(projectile, new Color(252, 3, 102) * 0.45f), new RoundCap(), new ArrowGlowPosition(), 20, 80);
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D projTex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(projTex, projectile.Center - Main.screenPosition, null, new Color(252, 3, 102) * projectile.Opacity, 0, projTex.Size() / 2, projectile.scale * 1.2f, SpriteEffects.None, 0);
			spriteBatch.Draw(projTex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, 0, projTex.Size() / 2, projectile.scale, SpriteEffects.None, 0);
		}
	}
}
