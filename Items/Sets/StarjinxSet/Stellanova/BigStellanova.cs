using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Prim;
using Terraria.ID;
using System.IO;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class BigStellanova : ModProjectile, IDrawAdditive
	{
		//Constants
		private const int FADEIN_TIME = 40;
		private const int MAX_FLASHTIME = 10;
		private const float CHARGE_STEP = 1 / 7f;
		private const int MAX_TIMELEFT = 300;
		private const int MAX_TIMETOLAUNCH = 100; //How long after the projectile is charged before it automatically launches early
		private const int ANTICIPATION_TIME = 15;
		private const float LAUNCH_SPEED = 35f;

		private const int STATE_CHARGING = 0;
		private const int STATE_ANTICIPATION = 1;
		private const int STATE_LAUNCH = 2;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellanova");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = true;
			Projectile.timeLeft = MAX_TIMELEFT;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.scale = 0.1f;
		}

		private ref float Timer => ref Projectile.ai[0];
		private ref float Charge => ref Projectile.ai[1];
		private float AiState = STATE_CHARGING;
		private Vector2 LaunchTrajectory = Vector2.Zero;
		private int _flashTime;
		private bool Charging => Charge < 1 && (Timer < MAX_TIMETOLAUNCH || Charge == 0) && AiState == STATE_CHARGING;

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => AiState == STATE_LAUNCH;

		public override void AI()
		{
			Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.velocity.ToRotation(), 0.2f);
			Timer++;
			_flashTime = Math.Max(_flashTime - 1, 0);
			Charge = Math.Min(Charge, 1);

			int TimeActive = MAX_TIMELEFT - Projectile.timeLeft;

			if (Charge > 0 && AiState != STATE_LAUNCH) //Don't time out if any charge is recieved, until launched
				Projectile.timeLeft = Math.Max(Projectile.timeLeft, FADEIN_TIME);

			float progress = 1;

			if (Projectile.timeLeft >= FADEIN_TIME) //fade in
			{
				if (AiState != STATE_LAUNCH) //if launching, stay at 1 progress
					progress = Math.Min(TimeActive / (float)FADEIN_TIME, 1);
			}

			else //fade out
				progress = Math.Min(Projectile.timeLeft / (float)FADEIN_TIME, 1);

			progress = EaseFunction.EaseQuadOut.Ease(progress);
			Projectile.scale = 1 * progress;
			Projectile.alpha = (int)(255 * (1 - progress));

			if(Charging)
			{
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.08f);
				for (int i = 0; i < Main.maxProjectiles; ++i) //Gravitate StellanovaStarfire projectiles to this proj
				{
					Projectile p = Main.projectile[i];
					float pullRange = 400;
					float absorbRange = 20;
					if (p.active && p.DistanceSQ(Projectile.Center) < pullRange * pullRange && p.type == ModContent.ProjectileType<StellanovaStarfire>())
					{
						float pullStrength = 0.07f;
						p.velocity = Vector2.Lerp(p.velocity, p.DirectionTo(Projectile.Center) * 25, pullStrength);

						if (p.DistanceSQ(Projectile.Center) < absorbRange * absorbRange)
						{
							Projectile.damage += p.damage / 2;
							p.Kill();
							_flashTime = MAX_FLASHTIME;
							Charge += CHARGE_STEP;
							Timer = 0;
							Projectile.netUpdate = true;
						}
					}
				}
			}

			else
			{
				switch(AiState)
				{
					case STATE_CHARGING:
						Projectile.netUpdate = true;
						Timer = 0;
						AiState = STATE_ANTICIPATION;
						LaunchTrajectory = Projectile.DirectionFrom(Main.player[Projectile.owner].Center);
						break;

					case STATE_ANTICIPATION:
						Timer++;
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, -LaunchTrajectory * LAUNCH_SPEED * 0.5f, 0.04f);
						if(Timer > ANTICIPATION_TIME)
						{
							Projectile.netUpdate = true;
							AiState = STATE_LAUNCH;
							Projectile.timeLeft = MAX_TIMELEFT / 2;
						}
						break;

					case STATE_LAUNCH:
						float speedMod = (Charge / 2) + 0.5f;
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, LaunchTrajectory * LAUNCH_SPEED * speedMod, 0.1f);
						break;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(AiState != STATE_LAUNCH)
			{
				Projectile.Bounce(oldVelocity, 1);
				return false;
			}
			return base.OnTileCollide(oldVelocity);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AiState);
			writer.Write(_flashTime);
			writer.WriteVector2(LaunchTrajectory);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AiState = reader.ReadSingle();
			_flashTime = reader.ReadInt32();
			LaunchTrajectory = reader.ReadVector2();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Color orange = Color.Lerp(StellanovaStarfire.Orange, Color.White, 0.4f) * Projectile.Opacity;
			Color purple = StellanovaStarfire.Purple * Projectile.Opacity;
			float chargeProgress = EaseFunction.EaseCubicIn.Ease(Charge);
			if (_flashTime > 0)
			{
				float strength = (_flashTime / (float)MAX_FLASHTIME) * 0.5f;

				orange = Color.Lerp(orange, Color.White, strength);
				purple = Color.Lerp(purple, Color.White, strength);
			}
			Color yellow = Color.Lerp(StellanovaStarfire.Yellow, Color.White * Projectile.Opacity, 0.5f) * 0.7f; //used for godrays, only fades in when opacity is high enough
			yellow.A = 0;

			float fluctuate = (float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly * 2.5f)) * 0.1f; //fluctuate between 90% and 110% of scale
			float flashPulse = MathHelper.Lerp(0.25f, 0f, _flashTime / (float)MAX_FLASHTIME); //25% increase in scale that quickly fades away when absorbing projectiles
			float chargeScale = (EaseFunction.EaseCubicOut.Ease(chargeProgress) * 0.66f) + 1; //Increase scale depending on how charged the projectile is
			float modifiedScale = Projectile.scale * (1 + fluctuate + flashPulse) * chargeScale;

			PreDrawOrbExtras(Main.spriteBatch, modifiedScale, yellow);
			DrawOrb(Main.spriteBatch, modifiedScale, orange, purple);

			return false;
		}

		private void PreDrawOrbExtras(SpriteBatch sB, float scale, Color color)
		{

			Texture2D Lu1 = Mod.Assets.Request<Texture2D>("Textures/T_Lu_Noise_31").Value;
			Texture2D Lu2 = Mod.Assets.Request<Texture2D>("Textures/T_Lu_Noise_32").Value;

			void DrawMask(Texture2D tex, float opacity, float maskScale, float rotationDirection) =>
				sB.Draw(tex, Projectile.Center - Main.screenPosition, null, color * opacity, Main.GlobalTimeWrappedHourly * rotationDirection, tex.Size() / 2, scale * maskScale, SpriteEffects.None, 0);

			float rotationSpeed = 2f + Charge;
			DrawMask(Lu2, 0.15f, 0.06f, -rotationSpeed);
			DrawMask(Lu1, 0.15f, 0.06f, rotationSpeed);
			DrawMask(Lu2, 0.3f, 0.05f, -rotationSpeed);
			DrawMask(Lu1, 0.3f, 0.05f, rotationSpeed);

			DrawGodray.DrawGodrays(sB, Projectile.Center - Main.screenPosition, color, 20 * scale * Projectile.Opacity, 8 * scale * Projectile.Opacity, 16);


			//not ideal, basically only stable because no sprites are drawn after this in the duration of the current spritebatch, as the current spritebatch is ended
			//convert to spritebatch drawing later if a suitable additive mask for a blur line is found?
			//alternatively just literally use the same ray texture godrays use they look basically the same when obscured 
			float blurLength = 130 * scale * Projectile.Opacity;
			float blurWidth = 9 * scale * Projectile.Opacity;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 12) % 1) * 0.1f) + 1f;
			Effect blurEffect = ModContent.Request<Effect>("Effects/BlurLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			var blurLine = new SquarePrimitive()
			{
				Position = Projectile.Center - Main.screenPosition,
				Height = blurWidth * flickerStrength,
				Length = blurLength * flickerStrength,
				Rotation = 0,
				Color = color * flickerStrength
			};

			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
		}

		private void DrawOrb(SpriteBatch spriteBatch, float scale, Color lightColor, Color darkColor)
		{
			Effect effect = ModContent.Request<Effect>("Effects/StellanovaOrb", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			effect.Parameters["uTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/MilkyNoise").Value);
			effect.Parameters["distortTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/noiseNormal").Value);
			effect.Parameters["timer"].SetValue(Main.GlobalTimeWrappedHourly * 0.33f);
			effect.Parameters["intensity"].SetValue(1.33f + (Charge / 4));
			effect.Parameters["lightColor"].SetValue(new Color(lightColor.R, lightColor.G, lightColor.B, (int)(170 * Projectile.Opacity)).ToVector4());
			effect.Parameters["darkColor"].SetValue(new Color(darkColor.R, darkColor.G, darkColor.B, (int)(195 * Projectile.Opacity)).ToVector4());
			effect.Parameters["coordMod"].SetValue(0.33f);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, RasterizerState.CullNone, effect, Main.GameViewMatrix.ZoomMatrix);

			float orbScale = 0.08f; //scaled down drastically due to large texture size
			float stretchFactor = 0.5f;
			float speed = EaseFunction.EaseCircularIn.Ease(Projectile.velocity.Length() / LAUNCH_SPEED);
			Vector2 stretch = new Vector2(1 + stretchFactor * speed, 1 - stretchFactor * speed);

			Texture2D projTex = TextureAssets.Projectile[Projectile.type].Value;
			spriteBatch.Draw(projTex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation,
				projTex.Size() / 2, scale * orbScale * stretch, SpriteEffects.None, 0);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}

		//In hindsight there's not much of a reason for this to not just be a vertex strip, looks basically the same
		public void AdditiveCall(SpriteBatch sB)
		{
			int trailLength = ProjectileID.Sets.TrailCacheLength[Projectile.type];
			float speed = (Projectile.velocity.Length() / LAUNCH_SPEED);
			float opacity = speed * 0.5f;

			for(int j = 1; j <= 2; j++)
			{
				for (float i = 0; i < trailLength - 1; i += 0.25f / j)
				{
					float progress = i / (float)trailLength;
					Color color;
					//3 color lerp
					if (progress < 0.33f)
						color = Color.Lerp(StellanovaStarfire.Yellow, StellanovaStarfire.Orange, progress * 3);
					else
						color = Color.Lerp(StellanovaStarfire.Orange, StellanovaStarfire.Purple, (progress - 0.33f) * 1.5f);

					Texture2D mask = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
					float scale = 0.5f * Projectile.scale * (1 - progress) / (float)Math.Pow(j, 0.75f);
					scale *= 1 + (float)Math.Sin((progress - Main.GlobalTimeWrappedHourly) * MathHelper.TwoPi * (float)Math.Pow(j, 0.5f)) / 15;
					Vector2 drawPos = Vector2.Lerp(Projectile.oldPos[(int)i], Projectile.oldPos[(int)i + 1], i % 1) + Projectile.Size / 2 - Main.screenPosition;

					sB.Draw(mask, drawPos, null, color * opacity * EaseFunction.EaseQuadOut.Ease(1 - progress), 0f, mask.Size() / 2, scale, SpriteEffects.None, 0);
				}
			}
		}
	}
}