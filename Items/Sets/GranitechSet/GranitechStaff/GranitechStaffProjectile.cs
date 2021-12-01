using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.Prim;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechStaff
{
	public class GranitechStaffProjectile : BaseHeldProj, IDrawAdditive
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Granitech Staff");

		public override string Texture => "SpiritMod/Items/Sets/GranitechSet/GranitechStaff/GranitechStaffItem";

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(76, 80);
			projectile.friendly = true;
			projectile.magic = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = (LASER_TIME/3) + 1;
		}

		private Vector2 StaffTipPos = new Vector2(55, 29); //Position of the staff tip relative to the rest of the sprite
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

		public const int LASER_TIME = 25;

		public const int MANA_DRAIN_TICKS = 5; //How many times it takes the player's mana

		private Vector2 BeamDirection;
		private float BeamLength;
		private bool HittingTile; //Whether or not the beam is actually hitting a tile or is just out of range, used for tile hit vfx

		private readonly Color lightCyan = new Color(99, 255, 229);
		private readonly Color midBlue = new Color(25, 132, 247);
		private readonly Color darkBlue = new Color(20, 8, 189);

		public override void AbstractAI()
		{
			AiTimer++;
			if (Owner == Main.LocalPlayer)
				ScanLaser();

			projectile.rotation /= 2.5f;
			projectile.rotation += (Owner.direction < 0) ? MathHelper.PiOver4 : -MathHelper.PiOver4;

			switch (AiState)
			{
				case STATE_CHARGING:
					ChannelKillCheck();
					int ChargeTime = Owner.HeldItem.useTime; //Corresponds to player's held item's usetime, thus benefits from reforges as well

					int ManaTickTime = ChargeTime / MANA_DRAIN_TICKS;
					if(AiTimer % ManaTickTime == 0)
					{
						if (!Owner.CheckMana(Owner.HeldItem.mana, true))
							projectile.Kill();

						Owner.manaRegenDelay = (int)Owner.maxRegenDelay; //Doesnt automatically do this when using checkmana 
						return;
					}

					if(AiTimer > ChargeTime)
					{
						AiState = STATE_LASER;
						AiTimer = 0;
						OnLaserFire();

						projectile.netUpdate = true;
					}
					break;
				case STATE_LASER:

					if (!Main.dedServ && HittingTile) //Make particles if hitting a tile
					{
						Vector2 endPoint = projectile.Center + StaffTipDirection + BeamDirection * BeamLength;
						float beamprogress = AiTimer / (float)LASER_TIME;

						ParallelParticles(endPoint, -BeamDirection * Main.rand.NextFloat(5, 10), 20, Main.rand.NextFloat(1.5f, 2f) * beamprogress);

						if(Main.rand.NextBool())
							ParallelParticles(endPoint, BeamDirection * Main.rand.NextFloat(5), 20, Main.rand.NextFloat(1f, 1.5f) * beamprogress);
					}

					if(AiTimer > LASER_TIME)
						projectile.Kill();

					break;
			}
		}

		private void ParallelParticles(Vector2 position, Vector2 velocity, float width, float scale, int numParticles = 1)
		{
			for (int i = 0; i < numParticles; i++) 
			{
				float widthDeviation = Main.rand.NextFloat(-1, 1); //Determine how far from the center of the beam the particle will spawn
				Vector2 newVel = velocity * (1 - Math.Abs(widthDeviation)); //Slow down particle based on how far it is from beam

				Vector2 newPos = position + BeamDirection.RotatedBy(MathHelper.PiOver2) * widthDeviation * width; //Offset particle

				ParticleHandler.SpawnParticle(new GranitechParticle(newPos, newVel, Main.rand.NextBool() ? lightCyan : midBlue, scale, 30, Main.rand.Next(4)));
			}
		}

		/// <summary>
		/// Mostly visual effects, called upon the laser being first fired
		/// </summary>
		private void OnLaserFire()
		{
			Owner.GetModPlayer<MyPlayer>().Shake = 15;

			projectile.velocity = (projectile.direction > 0) ? projectile.velocity.RotatedBy(-MathHelper.Pi / 5) : projectile.velocity.RotatedBy(MathHelper.Pi / 5); //recoil effect

			if (Main.netMode == NetmodeID.Server)
				return;

			Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/GranitechLaserBlast").WithPitchVariance(0.1f).WithVolume(0.8f), projectile.Center);

			float beamLengthLerp = MathHelper.Clamp(BeamLength / 800f, 0, 1);
			int maxRings = 3;
			float minRingDist = 20 * beamLengthLerp;
			float maxRingDist = 150 * beamLengthLerp;
			float sizeModifier = MathHelper.Lerp(0.5f, 1f, beamLengthLerp);

			float minRingSize = 1f;
			float maxRingSize = 2.5f;
			float RingSpeed = 1.25f;

			DrawAberration.DrawChromaticAberration(BeamDirection, 2f, delegate (Vector2 offset, Color colorMod)
			{
				for (int i = 0; i < maxRings; i++) //multiple rings
				{
					float progress = i / (float)(maxRings - 1);
					Color color = Color.Lerp(lightCyan, darkBlue, progress).MultiplyRGB(colorMod);

					float scale = MathHelper.Lerp(minRingSize, maxRingSize, progress) * sizeModifier;
					float speed = sizeModifier * RingSpeed;

					Vector2 velNormal = Vector2.Normalize(BeamDirection);
					Vector2 spawnPos = Vector2.Lerp(projectile.Center + StaffTipDirection + Vector2.Normalize(BeamDirection) * minRingDist,
						projectile.Center + StaffTipDirection + Vector2.Normalize(BeamDirection) * maxRingDist, progress) + offset;

					ParticleHandler.SpawnParticle(new PulseCircle(projectile, color * 0.4f, scale * 100, (int)(LASER_TIME * 0.66f),
						PulseCircle.MovementType.OutwardsSquareRooted, spawnPos)
					{
						Angle = BeamDirection.ToRotation(),
						ZRotation = 0.6f,
						RingColor = color,
						Velocity = velNormal * speed
					});
				}
			});

			for (int j = -1; j <= 1; j++) //repeat multiple times with different offset and color, for chromatic aberration effect
			{
				Vector2 posOffset = Vector2.Normalize(BeamDirection).RotatedBy(j * MathHelper.PiOver2) * 2f;
				Color colorMod = (j == -1) ? new Color(255, 0, 0, 100) : ((j == 0) ? new Color(0, 255, 0, 100) : new Color(0, 0, 255, 100));

			}

			ParallelParticles(projectile.Center + StaffTipDirection, BeamDirection * Main.rand.NextFloat(30, 40), 35, Main.rand.NextFloat(1.5f, 3f), Main.rand.Next(14, 18));
		}

		/// <summary>
		/// Scans from the tip of the staff towards the mouse, finding the length and direction of the beam, and checking if it hits a tile
		/// </summary>
		private void ScanLaser()
		{
			Vector2 laserPos = projectile.Center + StaffTipDirection;
			Vector2 desiredDirection = Vector2.Normalize(Main.MouseWorld - laserPos);
			if (Math.Abs(MathHelper.WrapAngle(desiredDirection.ToRotation() - projectile.velocity.ToRotation())) > MathHelper.Pi / 4)
				desiredDirection = Vector2.Lerp(desiredDirection, projectile.velocity, 0.8f); //Make the beam look less jank when player mouse is between staff tip and player body

			BeamDirection = Vector2.Normalize(Vector2.Lerp(BeamDirection, desiredDirection, CursorLerpSpeed()));

			float maxDistance = 1600;
			float[] samples = new float[4];

			Collision.LaserScan(laserPos, BeamDirection, 1, maxDistance, samples);
			BeamLength = 0;
			foreach (float sample in samples)
				BeamLength += sample / samples.Length;

			HittingTile = BeamLength != maxDistance;
			projectile.netUpdate = true;
		}

		public override bool AutoAimCursor() => true;

		public override Vector2 HoldoutOffset()
		{
			Vector2 sizeOffset = projectile.Size/2;
			Vector2 handPos = Main.OffsetsPlayerOnhand[Owner.bodyFrame.Y / 56] * 2f;
			if (Owner.direction > 0)
				handPos.X += Owner.width;

			return handPos - sizeOffset - new Vector2(18 * Owner.direction, 14).RotatedBy(projectile.rotation + MathHelper.PiOver2 * Owner.direction) + new Vector2(2, 2);
		}

		public override float CursorLerpSpeed() => (AiState == STATE_CHARGING ? 0.2f : 0.015f);

		public override bool CanDamage() => AiState == STATE_LASER;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center + StaffTipDirection, projectile.Center + StaffTipDirection + BeamDirection * BeamLength, 30, ref collisionPoint);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;

			Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/EnergyImpact").WithPitchVariance(0.1f).WithVolume(0.6f), target.Center);
			float scale = Main.rand.NextFloat(0.8f, 1f);
			DrawAberration.DrawChromaticAberration(BeamDirection, 2f, delegate (Vector2 offset, Color colorMod) 
			{
				Color color = new Color(99, 255, 229).MultiplyRGB(colorMod);

				ParticleHandler.SpawnParticle(new PulseCircle(target.Center + offset, color * 0.4f, scale * 100, (int)(LASER_TIME * 0.66f),
					PulseCircle.MovementType.OutwardsSquareRooted)
				{
					Angle = BeamDirection.ToRotation(),
					ZRotation = 0.6f,
					RingColor = color,
					Velocity = BeamDirection
				});
			});

			ParallelParticles(target.Center, BeamDirection * Main.rand.NextFloat(10, 15), 25, Main.rand.NextFloat(1.5f, 2f), Main.rand.Next(7, 10));
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
			Color bloomColor = Color.LightBlue * 0.33f;
			bloomColor.A = 0;
			spriteBatch.Draw(bloom, projectile.Center + StaffTipDirection - Main.screenPosition, null, bloomColor, 0, bloom.Size() / 2, 0.15f, SpriteEffects.None, 0);

			projectile.QuickDrawGlow(spriteBatch);
		}

		private void DrawTelegraphBeam(SpriteBatch spriteBatch)
		{
			Texture2D telegraph = mod.GetTexture("Textures/GlowTrail");
			int ChargeTime = Owner.HeldItem.useTime;
			float chargeAmount = MathHelper.Clamp(AiTimer / ChargeTime, 0, 1);
			float PowF(float power) => (float)Math.Pow(chargeAmount, power);

			float ColorLerp = PowF(2);

			Vector2 scale = new Vector2(BeamLength, MathHelper.Lerp(120, 10, PowF(0.5f))) / telegraph.Size(); //Desired size for the drawn texture is 80-10 pixels tall and as wide as the beam

			for (int i = 0; i < 3; i++) //Draw multiple times, for a chromatic aberration effect
			{
				Color color;
				if (ColorLerp <= 0.5f)
					color = Color.Lerp(darkBlue, midBlue, ColorLerp * 2);
				else
					color = Color.Lerp(midBlue, lightCyan, (ColorLerp - 0.5f) * 2);

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
				color *= 0.75f * PowF(0.05f);

				spriteBatch.Draw(telegraph, projectile.Center + StaffTipDirection + aberrationOffset - Main.screenPosition, null, color, BeamDirection.ToRotation(), new Vector2(0, telegraph.Height / 2), scale, SpriteEffects.None, 0);
			}
		}

		private void DrawBeam()
		{
			float progress = AiTimer / LASER_TIME;
			float PowF(float power) => (float)Math.Pow(progress, power);
			Vector2 scale = new Vector2(BeamLength, MathHelper.Lerp(60, 10, PowF(0.75f)));

			float ColorLerp = PowF(2.5f);

			Color color = Color.Lerp(midBlue, darkBlue, ColorLerp);
			color *= 1 - PowF(2.5f) * 0.33f;
			color.A = 10;
			Vector2 position = projectile.Center + StaffTipDirection + (BeamDirection * BeamLength / 2) - Main.screenPosition; //Center between staff tip and beam end

			//Draw the beam and apply shader parameters
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

			//Draw a visual effect for hitting tiles
			if (HittingTile)
			{
				float scaleMod = scale.Y / 60;
				Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
				Vector2 endPos = projectile.Center + StaffTipDirection + BeamDirection * BeamLength;
				Main.spriteBatch.Draw(bloom, endPos - Main.screenPosition, null, darkBlue, 0, bloom.Size() / 2, 0.5f * scaleMod, SpriteEffects.None, 0);

				float blurLength = 400 * scaleMod;
				float blurWidth = 12 * scaleMod;
				float flickerStrength = (((float)Math.Sin(Main.GlobalTime * 20) % 1) * 0.3f) + 1f;
				Effect blurEffect = mod.GetEffect("Effects/BlurLine");

				for(int i = -1; i <= 1; i++)
				{
					float rotation = BeamDirection.ToRotation() + MathHelper.PiOver2 + (MathHelper.Pi / 6) * i;
					float size = 1 - Math.Abs(i * 0.5f);
					SquarePrimitive blurLine = new SquarePrimitive()
					{
						Position = endPos - Main.screenPosition,
						Height = blurWidth * flickerStrength * size,
						Length = blurLength * flickerStrength * size,
						Rotation = rotation,
						Color = Color.Lerp(lightCyan, Color.White, 0.25f) * (1 - ColorLerp)
					};
					PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
				}
			}
		}
	}
}