using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Prim;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechStaff
{
	public class GranitechStaffProjectile : BaseHeldProj, IDrawAdditive
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granitech Staff");

		public override string Texture => "SpiritMod/Items/Sets/GranitechSet/GranitechStaff/GranitechStaffItem";

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(54, 56);
			projectile.friendly = true;
			projectile.magic = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		private Vector2 StaffTipPos = new Vector2(46, 18); //Position of the staff tip relative to the rest of the sprite
		private Vector2 StaffTipDirection
		{
			get
			{
				bool oppositeDir = Owner.direction < 0;
				Vector2 baseVec = (StaffTipPos - (projectile.Size / 2));
				if (oppositeDir)
					baseVec.X *= -1;

				return baseVec.RotatedBy(projectile.rotation); //Direction from the center of the sprite to the staff tip
			}
		}

		public ref float AiState => ref projectile.ai[0];
		public ref float AiTimer => ref projectile.ai[1];

		public const int STATE_CHARGING = 0;
		public const int STATE_LASER = 1;

		public const int LASER_TIME = 20;

		public const int BASE_MANA_COST = 80; //Done in a kinda hacky way to make the initial use not eat up 80 mana, but have the item still display 80 mana cost in total
		public const int MANA_DRAIN_TICKS = 10; //How many times it takes the player's mana, total mana consumed corresponds to stated mana cost on item

		private Vector2 BeamDirection;
		private float BeamLength;
		private bool HittingTile; //Whether or not the beam is actually hitting a tile or is just out of range, used for tile hit vfx

		public override void AbstractAI()
		{
			AiTimer++;

			if(Owner == Main.LocalPlayer)
				ScanLaser();

			projectile.rotation /= 3;
			projectile.rotation += (Owner.direction < 0) ? MathHelper.PiOver4 : -MathHelper.PiOver4;


			switch (AiState)
			{
				case STATE_CHARGING:
					ChannelKillCheck();
					int ChargeTime = Owner.HeldItem.useTime; //Corresponds to player's held item's usetime, benefits from reforges as well

					int ManaTickTime = ChargeTime / MANA_DRAIN_TICKS;
					if(AiTimer % ManaTickTime == 0)
					{
						if (!Owner.CheckMana(BASE_MANA_COST / MANA_DRAIN_TICKS, true))
							projectile.Kill();

						Owner.manaRegenDelay = (int)Owner.maxRegenDelay; //Doesnt automatically do this when using checkmana 
						return;
					}

					if(AiTimer > ChargeTime)
					{
						AiState = STATE_LASER;
						AiTimer = 0;
						Owner.GetModPlayer<MyPlayer>().Shake = 8;

						projectile.velocity = (projectile.direction > 0) ? projectile.velocity.RotatedBy(-MathHelper.Pi / 5) : projectile.velocity.RotatedBy(MathHelper.Pi / 5); //recoil effect

						/*float beamLengthLerp = MathHelper.Clamp(BeamLength / 1600f, 0, 1);
						int maxRings = (int)MathHelper.Lerp(1, 5, beamLengthLerp);
						float averageSize = MathHelper.Lerp(0.5f, 1.5f, beamLengthLerp);

						for (int j = -1; j <= 1; j++) //repeat multiple times with different offset and color, for chromatic aberration effect
						{
							Vector2 posOffset = Vector2.Normalize(BeamDirection).RotatedBy(j * MathHelper.PiOver2) * 0.5f;
							Color colorMod = (j == -1) ? new Color(255, 0, 0, 100) : ((j == 0) ? new Color(0, 255, 0, 100) : new Color(0, 0, 255, 100));

							for (int i = 0; i < maxRings; i++) //multiple rings
							{
								float progress = i / (float)maxRings;
								Color color = new Color(255, 110, 165).MultiplyRGB(colorMod);

								float modifier = (float)Math.Sin(2 * MathHelper.TwoPi * i / (float)maxRings) * 0.25f + 1;
								float scale = averageSize * modifier;

								float speed = averageSize * modifier;

								Vector2 velNormal = Vector2.Normalize(BeamDirection);
								Vector2 spawnPos = Vector2.Lerp(projectile.Center + StaffTipDirection, projectile.Center + StaffTipDirection + Vector2.Normalize(BeamDirection) * BeamLength * 0.5f, progress) + posOffset;
								Particles.ParticleHandler.SpawnParticle(new Particles.PulseCircle(spawnPos, color * 0.4f, scale * 100, 20, Particles.PulseCircle.MovementType.OutwardsSquareRooted)
								{
									Angle = BeamDirection.ToRotation(),
									ZRotation = 0.6f,
									RingColor = color,
									Velocity = velNormal * speed
								});
							}
						}*/

						projectile.netUpdate = true;
					}
					break;
				case STATE_LASER:

					if(AiTimer > LASER_TIME)
						projectile.Kill();

					break;
			}
		}

		private void ScanLaser()
		{
			Vector2 laserPos = projectile.Center + StaffTipDirection;
			Vector2 desiredDirection = Vector2.Normalize(Main.MouseWorld - laserPos);
			if (Math.Abs(MathHelper.WrapAngle(desiredDirection.ToRotation() - projectile.velocity.ToRotation())) > MathHelper.Pi / 4)
				desiredDirection = Vector2.Lerp(desiredDirection, projectile.velocity, 0.8f); //Make the beam look less jank when player mouse is between staff tip and player body

			BeamDirection = Vector2.Normalize(Vector2.Lerp(BeamDirection, desiredDirection, CursorLerpSpeed()));

			float maxDistance = 1600;
			float[] samples = new float[2];

			Collision.LaserScan(laserPos, BeamDirection, 0, maxDistance, samples);
			BeamLength = 0;
			foreach (float sample in samples)
				BeamLength += sample / samples.Length;

			HittingTile = BeamLength != maxDistance;
		}

		public override bool AutoAimCursor() => true;

		public override Vector2 HoldoutOffset() => Main.OffsetsPlayerOnhand[Owner.bodyFrame.Y / 56] * 2f - projectile.Size / 2 + new Vector2(0, -16);

		public override float CursorLerpSpeed() => (AiState == STATE_CHARGING ? 0.2f : 0.03f);

		public override bool CanDamage() => AiState == STATE_LASER;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + StaffTipDirection, projectile.Center + StaffTipDirection + BeamDirection * BeamLength, 20, ref collisionPoint);
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (AiState == STATE_CHARGING)
				DrawTelegraphBeam(spriteBatch);

			if (AiState == STATE_LASER)
				DrawBeam();


		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			Color bloomColor = Color.LightYellow * 0.33f;
			bloomColor.A = 0;
			spriteBatch.Draw(bloom, projectile.Center + StaffTipDirection - Main.screenPosition, null, bloomColor, 0, bloom.Size() / 2, 0.15f, SpriteEffects.None, 0);

			//projectile.QuickDrawGlow(spriteBatch);
		}

		private void DrawTelegraphBeam(SpriteBatch spriteBatch)
		{
			Texture2D telegraph = mod.GetTexture("Textures/GlowTrail");
			int ChargeTime = Owner.HeldItem.useTime;
			float chargeAmount = MathHelper.Clamp(AiTimer / ChargeTime, 0, 1);
			float PowF(float power) => (float)Math.Pow(chargeAmount, power);

			float ColorLerp = PowF(3);

			Vector2 scale = new Vector2(BeamLength, MathHelper.Lerp(80, 10, PowF(0.5f))) / telegraph.Size(); //Desired size for the drawn texture is 80-10 pixels tall and as wide as the beam

			for (int i = 0; i < 3; i++) //Draw multiple times, for a chromatic aberration effect
			{
				Color color;
				if (ColorLerp <= 0.5f)
					color = Color.Lerp(new Color(213, 39, 102), new Color(241, 152, 80), ColorLerp * 2);
				else
					color = Color.Lerp(new Color(241, 152, 80), new Color(243, 202, 112), (ColorLerp - 0.5f) * 2);

				Color aberrationColor = Color.White;
				switch (i) //Switch between making the telegraph's color red, green, and blue
				{
					case 0:
						aberrationColor = new Color(255, 0, 0);
						break;
					case 1:
						aberrationColor = new Color(0, 255, 0);
						break;
					case 2:
						aberrationColor = new Color(0, 0, 255);
						break;
				}

				color = color.MultiplyRGB(aberrationColor);
				Vector2 aberrationOffset = Vector2.UnitX.RotatedBy(BeamDirection.ToRotation() + (i - 1) * MathHelper.PiOver2) * 0.33f * MathHelper.Lerp(6, 1, PowF(0.5f)); //Offset each texture slightly
				color *= PowF(0.25f);

				spriteBatch.Draw(telegraph, projectile.Center + StaffTipDirection + aberrationOffset - Main.screenPosition, null, color, BeamDirection.ToRotation(), new Vector2(0, telegraph.Height / 2), scale, SpriteEffects.None, 0);
			}
		}

		private void DrawBeam()
		{
			float progress = AiTimer / LASER_TIME;
			float PowF(float power) => (float)Math.Pow(progress, power);
			Vector2 scale = new Vector2(BeamLength, MathHelper.Lerp(50, 10, PowF(0.75f)));

			float ColorLerp = PowF(2.5f);

			for (int i = 0; i < 3; i++)
			{
				Color color = Color.Lerp(new Color(255, 51, 122), new Color(140, 14, 37), ColorLerp);

				Color aberrationColor = Color.White;
				switch (i) //Switch between making the beam's color red, green, and blue
				{
					case 0:
						aberrationColor = new Color(255, 0, 0);
						break;
					case 1:
						aberrationColor = new Color(0, 255, 0);
						break;
					case 2:
						aberrationColor = new Color(0, 0, 255);
						break;
				}

				color = color.MultiplyRGB(Color.Lerp(aberrationColor, color, 0.25f));
				Vector2 aberrationOffset = Vector2.UnitX.RotatedBy(BeamDirection.ToRotation() + (i - 1) * MathHelper.PiOver2) * 0.5f * MathHelper.Lerp(5, 1, PowF(0.5f)); //Offset each texture slightly
				color *= 1 - PowF(2.5f) * 0.33f;
				color.A = 10;
				Vector2 position = projectile.Center + StaffTipDirection + (BeamDirection * BeamLength / 2) + aberrationOffset - Main.screenPosition; //Center between staff tip and beam end

				Effect beamEffect = mod.GetEffect("Effects/Laser");
				beamEffect.Parameters["uTexture"].SetValue(mod.GetTexture("Textures/Trails/Trail_1"));
				beamEffect.Parameters["Progress"].SetValue(Main.GlobalTime * 3f);
				beamEffect.Parameters["xMod"].SetValue(BeamLength / 150f);
				beamEffect.Parameters["yMod"].SetValue(2f);
				beamEffect.CurrentTechnique.Passes[0].Apply();

				SquarePrimitive beam = new SquarePrimitive
				{
					Height = scale.Y,
					Length = scale.X,
					Rotation = BeamDirection.ToRotation(),
					Position = position,
					Color = color
				};

				PrimitiveRenderer.DrawPrimitiveShape(beam, beamEffect); 
			}
		}
	}
}