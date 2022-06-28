using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria.ID;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.AstralSpellblade
{
    public class AstralGreatswordHeld : BaseHeldProj
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astral Spellblade");

		public override string Texture => "SpiritMod/Items/Sets/StarjinxSet/AstralSpellblade/AstralGreatsword";

		private int maxTime; //Total time for weapon to be used
		private Vector2 initialVelocity = Vector2.Zero; //Starting velocity, used for determining swing arc direction

		private const float START_OFFSET = MathHelper.PiOver2; //The starting angle for the sword to be held at
		private const float TOTAL_RADIANS = MathHelper.Pi * 1.66f; //The total amount of radians the sword swings, from where the swing starts after drawback is done, to the ending position

		private const float DRAWBACK_TIME = 0.66f; //Portion of the swing time spent drawing back the sword
		private float SWING_TIME => 1 - DRAWBACK_TIME; //Portion of the swing time spent swinging the sword

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(64, 64);
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.netUpdate = true;
			Projectile.ownerHitCheck = true;
		}

		private float Combo => Projectile.ai[0];
		private ref float Timer => ref Projectile.ai[1];
		private float SwingDirection => Combo % 2 == 1 ? -1 : 1;
		private float UseProgress => Timer / maxTime;
		private int _hitPause = 0;
		private bool _hasHit = false;

		public override bool PreAI()
		{
			if (_hitPause == 0)
				Timer++;
			else
				_hitPause--;

			bool firstTick = Projectile.timeLeft > 2; //Set to 2 on first tick of normal ai
			if (firstTick) //Initialize total swing time and initial direction
			{
				initialVelocity = Projectile.velocity;
				maxTime = Owner.HeldItem.useTime;
				Projectile.netUpdate = true;
			}


			//Get new velocity for swinging motion
			float SwingDirection = Combo % 2 == 1 ? -1 : 1;
			if(UseProgress < DRAWBACK_TIME) //Sword position before swing
			{
				float newProgress = EaseFunction.EaseCubicInOut.Ease(UseProgress / DRAWBACK_TIME);
				newProgress = EaseFunction.EaseQuadOut.Ease(newProgress);

				float desiredAngle = TOTAL_RADIANS / 2 * SwingDirection;
				float curAngle = MathHelper.Lerp(START_OFFSET * SwingDirection, desiredAngle, newProgress);
				Projectile.velocity = initialVelocity.RotatedBy(curAngle);
			}
			else //Sword position while swinging
			{
				float newProgress = EaseFunction.EaseCircularOut.Ease((UseProgress - DRAWBACK_TIME) / SWING_TIME);

				float startAngle = TOTAL_RADIANS / 2 * SwingDirection;
				float curAngle = MathHelper.Lerp(startAngle, -startAngle, newProgress);
				Projectile.velocity = initialVelocity.RotatedBy(curAngle);
			}

			return true;
		}

		public override void AbstractAI()
		{
			Projectile.rotation += MathHelper.PiOver4 * Owner.direction;
			if (SwingDirection == Owner.direction)
			{
				Projectile.rotation += MathHelper.PiOver2 * Owner.direction;
				Projectile.direction = Projectile.spriteDirection *= -1;
			}

			float noParticleTheshold = 0.1f;
			if(Main.rand.NextBool() && !Main.dedServ && UseProgress > SWING_TIME + noParticleTheshold && UseProgress < (1 - noParticleTheshold) && _hitPause == 0)
			{
				Vector2 directionUnit = Owner.DirectionTo(Projectile.Center);
				Vector2 spawnPos = Owner.MountedCenter + directionUnit * Projectile.Size.Length();
				Vector2 vel = Owner.DirectionTo(Projectile.Center).RotatedByRandom(MathHelper.Pi / 16) * Main.rand.NextFloat(0.75f, 1.25f);
				ParticleHandler.SpawnParticle(new StarParticle(spawnPos, vel, Color.White, Main.rand.NextFloat(0.1f, 0.12f), 25));
			}

			Owner.reuseDelay = Owner.HeldItem.reuseDelay;

			if (Timer > maxTime)
				Projectile.Kill();
		}

		public override Vector2 HoldoutOffset() => Projectile.velocity * Projectile.Size.Length() / 2;
		public override bool AutoAimCursor() => false; //Only aim when first used

		//Uses a line longer than the actual size of the sprite, for a more generous hitbox
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Owner.MountedCenter, Owner.MountedCenter + Projectile.velocity * Projectile.Size.Length() * 1.5f);

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => UseProgress > DRAWBACK_TIME && _hitPause == 0; //Only do damage when swinging

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(!_hasHit)
			{
				_hitPause = 6; //6 frame pause when hitting enemies
				Owner.GetModPlayer<MyPlayer>().Shake = _hitPause;
				_hasHit = true;
				Projectile.netUpdate = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			float glowStrength = 0;
			float glowStrengthMin = 0.1f;
			if (UseProgress < DRAWBACK_TIME)
				glowStrength = EaseFunction.EaseCircularIn.Ease(UseProgress / DRAWBACK_TIME) * glowStrengthMin; //Increase strength while drawing back until reaching the minimum

			else
			{
				glowStrength = EaseFunction.EaseCircularOut.Ease((UseProgress - DRAWBACK_TIME) / SWING_TIME); //Make glow strength proportional to progress through the swing
				glowStrength = 1 - (2 * Math.Abs(glowStrength - 0.5f)); //Then, use an absolute value function to make it peak in strength midway, rather than the end
				glowStrength = EaseFunction.EaseCircularInOut.Ease(glowStrength); //Then smooth it out with an easing function
				DrawTrail(Main.spriteBatch, lightColor, EaseFunction.EaseCircularOut.Ease((UseProgress - DRAWBACK_TIME) / SWING_TIME), Math.Min(glowStrength * 5, 1));

				glowStrength = Math.Max(glowStrength, glowStrengthMin); //Finally, if too low, increase it
			}

			Texture2D projGlow = ModContent.Request<Texture2D>(Texture + "_glow");
			Texture2D projMask = ModContent.Request<Texture2D>(Texture + "_mask");
			Color additiveWhite = new Color(255, 255, 255, 0) * glowStrength;
			void DrawGlow(Texture2D tex, Vector2? offset = null, float opacityMod = 1f) => //Use a method for a bit less copy paste
				Main.spriteBatch.Draw(tex, Projectile.Center + (offset ?? Vector2.Zero) - Main.screenPosition, null, additiveWhite * opacityMod, Projectile.rotation,
				tex.Size() / 2, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			//Draw mask behind the sword
			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 6, 10, delegate (Vector2 offset, float opacityMod)
			{
				DrawGlow(projMask, offset, opacityMod * 0.8f);
			});
			DrawGlow(projMask);

			Projectile.QuickDraw(Main.spriteBatch);

			//Draw glowmask above the sword
			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 6, 6, delegate (Vector2 offset, float opacityMod)
			{
				DrawGlow(projGlow, offset, opacityMod * 0.66f);
			});
			DrawGlow(projGlow);

			return false;
		}

		//Temp, planning on moving sword trails to vertex strip system for convenience and lasting after projectile death
		private void DrawTrail(SpriteBatch spriteBatch, Color drawColor, float progress, float opacity)
		{
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Effect effect = Mod.GetEffect("Effects/AstralSpellblade");
			effect.Parameters["baseTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/noise").Value);
			effect.Parameters["baseColorDark"].SetValue(new Color(67, 37, 143).ToVector4());
			effect.Parameters["baseColorLight"].SetValue(new Color(230, 55, 166).ToVector4());
			effect.Parameters["overlayTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/voronoiLooping").Value);
			effect.Parameters["overlayColor"].SetValue(new Color(255, 245, 245).ToVector4() * 3);

			effect.Parameters["xMod"].SetValue(0.5f);
			effect.Parameters["yMod"].SetValue(0.6f);
			effect.Parameters["overlayCoordMods"].SetValue(new Vector2(1f, 0.2f));

			effect.Parameters["timer"].SetValue(-Main.GlobalTimeWrappedHourly * 1.5f);
			effect.Parameters["progress"].SetValue(progress);

			Vector2 pos = Owner.MountedCenter - Main.screenPosition;
			PrimitiveSlashArc slash = new PrimitiveSlashArc
			{
				BasePosition = pos,
				StartDistance = Projectile.Size.Length() * 0.33f,
				EndDistance = Projectile.Size.Length() * 0.95f,
				AngleRange = new Vector2(TOTAL_RADIANS / 2 * SwingDirection, -TOTAL_RADIANS / 2 * SwingDirection),
				DirectionUnit = initialVelocity,
				Color = Color.White * opacity,
				SlashProgress = progress,
				RectangleCount = 30
			};
			PrimitiveRenderer.DrawPrimitiveShape(slash, effect);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(maxTime);
			writer.WriteVector2(initialVelocity);
			writer.Write(_hitPause);
			writer.Write(_hasHit);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			maxTime = reader.ReadInt32();
			initialVelocity = reader.ReadVector2();
			_hitPause = reader.ReadInt32();
			_hasHit = reader.ReadBoolean();
		}
	}
}