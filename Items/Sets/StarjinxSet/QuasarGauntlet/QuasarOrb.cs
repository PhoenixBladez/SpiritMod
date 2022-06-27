using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.QuasarGauntlet
{
	public class QuasarOrb : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Orb");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.Size = Vector2.One * 24;
			Projectile.tileCollide = true;
			Projectile.hide = true;
			//projectile.scale = 0.8f;
			Projectile.extraUpdates = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.scale = 0f;
			Projectile.alpha = 255;
		}

		private const float MAXPOWER = 1.6f; //The maximum size and damage multiplier
		private const float STARTPOWER = 1f; //The starting size and damage multiplier
		private const int MAXPOWERHITS = 5; //The amount of times the projectile needs to hit an enemy to reach max power
		private float power = STARTPOWER;

		public ref float AiState => ref Projectile.ai[0];
		public ref float Timer => ref Projectile.ai[1];

		public const int STATE_SLOWDOWN = 0;
		public const int STATE_ANTICIPATION = 1;
		public const int STATE_RETURN = 2;
		private const int SLOWDOWN_TIME = 300;
		private const int ANTICIPATION_TIME = 20;
		private bool dying;
		private float scaleMod;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Projectile.Distance(targetHitbox.Center.ToVector2()) < (50 * (Projectile.scale + 0.4f)); //circular collision
		public override void AI()
		{
			Projectile.rotation += 0.05f * (Math.Sign(Projectile.velocity.X) > 0 ? 1 : -1);
			Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3());
			Timer++;
			Player player = Main.player[Projectile.owner];
			switch (AiState)
			{
				case STATE_SLOWDOWN: //Slow down until it hits a stop, returning automatically after a given amount of time

					Projectile.alpha = Math.Max(Projectile.alpha - 25, 0);
					scaleMod = MathHelper.Lerp(scaleMod, 0.66f, 0.1f);

					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.03f);
					if (Timer > SLOWDOWN_TIME)
					{
						AiState = STATE_ANTICIPATION;
						Timer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_ANTICIPATION: //Briefly move in opposite direction before dashing back to player

					Projectile.tileCollide = false;
					Projectile.alpha = Math.Max(Projectile.alpha - 25, 0);
					scaleMod = MathHelper.Lerp(scaleMod, 0.66f, 0.1f);
					Projectile.velocity = Projectile.DirectionFrom(player.MountedCenter) * 4;
					if (Timer > ANTICIPATION_TIME)
					{
						AiState = STATE_RETURN;
						Timer = 0;
						Projectile.netUpdate = true;
					}
					break;

				case STATE_RETURN: //Quickly return to player
					Projectile.tileCollide = false;

					Vector2 desiredPosition = player.MountedCenter;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Lerp(Projectile.Center, desiredPosition, dying ? 0.13f : 0.07f) - Projectile.Center, dying ? 0.15f : 0.18f);

					float maxSpeed = 18;
					float fadeDistance = 100;
					int fadeTime = 20;

					//cap max velocity
					if (Projectile.velocity.Length() > maxSpeed)
						Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;

					//fade out when close to player
					if (Projectile.Distance(player.MountedCenter) < fadeDistance || dying)
					{
						if (!dying)
						{
							dying = true;
							Projectile.netUpdate = true;
						}

						Projectile.alpha += 255 / fadeTime;
						scaleMod = MathHelper.Lerp(scaleMod, 0f, 0.1f);
						if (Projectile.alpha >= 255)
							Projectile.Kill();
					}
					else
						scaleMod = MathHelper.Lerp(scaleMod, 0.66f, 0.1f);

					break;
			}

			//pulse in scale
			if (Timer % 80 == 0)
				scaleMod += 0.4f;

			power = MathHelper.Clamp(power, STARTPOWER, MAXPOWER);

			Projectile.scale = MathHelper.Lerp(Projectile.scale, power * scaleMod, 0.05f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Bounce(oldVelocity, 1f);

			SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => IncreaseDamage(ref damage);

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit) => IncreaseDamage(ref damage);

        private void IncreaseDamage(ref int damage)
        {
            damage = (int)(damage * power);

            if (power < MAXPOWER)
			{
				Projectile.netUpdate = true;
				power += (MAXPOWER - STARTPOWER) / MAXPOWERHITS;
			}
        }

		public void AdditiveCall(SpriteBatch sB)
		{
			//Colors used by the black hole
			Color White = new Color(255, 251, 199) * 0.7f;
			Color Yellow = new Color(255, 170, 0) * 0.6f;
			Color Red = new Color(199, 0, 73) * 0.5f;
			Color Magenta = Color.Magenta * 0.4f;

			float rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
			Vector2 scale = new Vector2(1f - Projectile.velocity.Length() / 50, 1f + Projectile.velocity.Length() / 50) * Projectile.scale;

			Vector2 drawCenter = Projectile.Center - Main.screenPosition;
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			sB.Draw(bloom, drawCenter, null, Color.Magenta * Projectile.Opacity, rotation, bloom.Size() / 2, scale * 0.68f, SpriteEffects.None, 0);

			sB.End(); sB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, RasterizerState.CullNone, default, Main.GameViewMatrix.ZoomMatrix);
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

			//Set parameters for radial noise shader
			Effect effect = SpiritMod.Instance.GetEffect("Effects/PortalShader");
			effect.Parameters["PortalNoise"].SetValue(Mod.Assets.Request<Texture2D>("Utilities/Noise/SpiralNoise").Value);
			effect.Parameters["Timer"].SetValue(MathHelper.WrapAngle(Main.GlobalTimeWrappedHourly / 3));
			effect.Parameters["DistortionStrength"].SetValue(0);
			effect.Parameters["Rotation"].SetValue(Projectile.rotation);
			effect.CurrentTechnique.Passes[0].Apply();

			//Draw layers of the black hole, in various colors and sizes
			sB.Draw(tex, drawCenter, null, Magenta * Projectile.Opacity, rotation, tex.Size() / 2, scale * 1.1f, SpriteEffects.None, 0);
			sB.Draw(tex, drawCenter, null, Red * Projectile.Opacity, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);
			sB.Draw(tex, drawCenter, null, Yellow * Projectile.Opacity, rotation, tex.Size() / 2, scale * 0.75f, SpriteEffects.None, 0);
			sB.Draw(tex, drawCenter, null, White * Projectile.Opacity, rotation, tex.Size() / 2, scale * 0.62f, SpriteEffects.None, 0);

			sB.End(); sB.Begin(SpriteSortMode.Deferred, BlendState.Additive, default, default, RasterizerState.CullNone, default, Main.GameViewMatrix.ZoomMatrix);

			float blurLength = 200 * Projectile.scale;
			float blurWidth = 12 * Projectile.scale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 10) % 1) * 0.1f) + 1f;
			Effect blurEffect = Mod.GetEffect("Effects/BlurLine");

			//Blur line below the circles
			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = Projectile.Center - Main.screenPosition,
				Height = blurWidth * flickerStrength,
				Length = blurLength * flickerStrength,
				Rotation = 0,
				Color = White * flickerStrength * Projectile.Opacity
			};
			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);

			//Draw circles in the center of the black hole
			CirclePrimitive[] circles = new CirclePrimitive[]
			{
				new CirclePrimitive() //white circle
				{
					Color = Color.White * Projectile.Opacity,
					Radius = 20,
					Position = Projectile.Center - Main.screenPosition,
					ScaleModifier = scale,
					MaxRadians = MathHelper.TwoPi,
					Rotation = Projectile.velocity.ToRotation()
				},
				new CirclePrimitive() //smaller black circle
				{
					Color = Color.Black * Projectile.Opacity,
					Radius = 16,
					Position = Projectile.Center - Main.screenPosition,
					ScaleModifier = scale,
					MaxRadians = MathHelper.TwoPi,
					Rotation = Projectile.velocity.ToRotation()
				}
			};

			Effect circleAA = Mod.GetEffect("Effects/CirclePrimitiveAA");
			PrimitiveRenderer.DrawPrimitiveShapeBatched(circles, circleAA);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(power);
			writer.Write(dying);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			power = reader.ReadSingle();
			dying = reader.ReadBoolean();
		}
	}
}