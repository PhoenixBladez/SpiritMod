using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class Cannonbubble : ModProjectile
	{
		public const float MAX_SPEED = 15f; //Initial projectile velocity, used in item shootspeed

		private const string TexturePath = "SpiritMod/Items/Sets/ReefhunterSet/Projectiles/Cannonbubble";
		public override string Texture => TexturePath + "_Outline"; //So it doesn't break when loaded through tmod

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 360;
			Projectile.aiStyle = 0;
			Projectile.scale = Main.rand.NextFloat(0.85f, 1.15f);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		private ref float JiggleStrength => ref Projectile.ai[0];
		private ref float JiggleTime => ref Projectile.ai[1];

		public override void AI()
		{
			Projectile.velocity *= 0.975f;
			Projectile.rotation = Projectile.velocity.ToRotation();

			const float JiggleDecay = 0.99f; //Exponentially slows down in speed
			JiggleStrength *= JiggleDecay;
			JiggleTime += JiggleStrength;

			const float heightDeviation = 0.25f;
			const float sinePeriod = 100; //1.66 seconds
			Projectile.position.Y += (float)Math.Sin(MathHelper.TwoPi * Projectile.timeLeft / sinePeriod) * heightDeviation;

			if (Projectile.wet)
				Projectile.velocity.Y -= 0.08f;

			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.type == Projectile.type && p.active && p != null && p.whoAmI != Projectile.whoAmI && p.Hitbox.Intersects(Projectile.Hitbox))
					BubbleCollision(p);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + (Projectile.Size/2) + new Vector2(0f, Projectile.gfxOffY);
				float progress = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				float speedMult = GetSpeedRatio(3); //Trail only appears when at high speed
				Color color = Projectile.GetAlpha(lightColor) * progress * speedMult * 0.5f;
				DrawBubble(spriteBatch, drawPos, color);
			}

			DrawBubble(spriteBatch, Projectile.Center - Main.screenPosition, Projectile.GetAlpha(lightColor));
			return false;
		}

		//Draw a bubble, with a rotated and squished outline and interior, a semi-transparent interior, and an unchanging shine
		private void DrawBubble(SpriteBatch spriteBatch, Vector2 position, Color baseColor)
		{
			Texture2D outline = ModContent.Request<Texture2D>(TexturePath + "_Outline");
			Texture2D inner = ModContent.Request<Texture2D>(TexturePath + "_Inner");
			Texture2D shine = ModContent.Request<Texture2D>(TexturePath + "_Shine");

			Vector2 squishScale = BubbleSquishScale(0.35f, 0.1f);
			Color innerColor = baseColor * 0.05f;
			innerColor.A = 0;

			spriteBatch.Draw(outline, position, null, baseColor, Projectile.rotation, Projectile.DrawFrame().Size() / 2, squishScale, SpriteEffects.None, 0);
			spriteBatch.Draw(inner, position, null, innerColor, Projectile.rotation, Projectile.DrawFrame().Size() / 2, squishScale, SpriteEffects.None, 0);
			spriteBatch.Draw(shine, position, null, baseColor, 0, Projectile.DrawFrame().Size() / 2, Projectile.scale, SpriteEffects.None, 0);
		}

		//Find the scale vector by which to draw the bubble's outline and interior with
		private Vector2 BubbleSquishScale(float velDelta, float jiggleDelta)
		{
			float squishAmount = GetSpeedRatio() * velDelta; //velocity based
			const float sineSpeed = 5 * MathHelper.Pi;
			squishAmount += (float)Math.Sin(sineSpeed * JiggleTime / 60) * jiggleDelta * JiggleStrength; //jiggling based
			return new Vector2(1 + squishAmount, 1 - squishAmount) * Projectile.scale;
		}

		private float GetSpeedRatio(float exponent = 1) => (float)Math.Pow(MathHelper.Min(Projectile.velocity.Length() / MAX_SPEED, 1), exponent);

		public override void Kill(int timeLeft)
		{
			int dustCount = Main.rand.Next(7, 12);
			SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);

			for (int i = 0; i < dustCount; ++i)
			{
				Vector2 speed = Main.rand.NextVec2CircularEven(0.5f, 0.5f);
				Dust.NewDust(Projectile.Center, 0, 0, DustID.BubbleBurst_Blue, speed.X * .5f, speed.Y * .5f, 0, default, Main.rand.NextFloat(0.5f, 1f));

				if(Main.rand.NextBool(3))
				{
					int d = Dust.NewDust(Projectile.Center + new Vector2(Main.rand.Next(-20, 20), 0), 0, 0, ModContent.DustType<Dusts.BubbleDust>(), speed.X * .35f, Main.rand.NextFloat(-3f, -.5f), 0, default, Main.rand.NextFloat(0.75f, 1.5f));
					Main.dust[d].velocity = Main.rand.NextVec2CircularEven(2.5f, 2.5f);
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			NPCCollision(target, 3, 0.5f, 2f);
			OnCollideExtra();
		}

		private void NPCCollision(Entity target, float stoppedSpeed, float lowAddedVelMod, float highAddedVelMod)
		{
			Rectangle lastTargetHitboxX = new Rectangle((int)(target.position.X - target.velocity.X), (int)(target.position.Y), target.width, target.height);
			Rectangle lastTargetHitboxY = new Rectangle((int)(target.position.X), (int)(target.position.Y - target.velocity.Y), target.width, target.height);
			Rectangle lastProjHitboxX = new Rectangle((int)(Projectile.position.X - Projectile.velocity.X), (int)(Projectile.position.Y), Projectile.width, Projectile.height);
			Rectangle lastProjHitboxY = new Rectangle((int)(Projectile.position.X), (int)(Projectile.position.Y - Projectile.velocity.Y), Projectile.width, Projectile.height);

			//Check for x collision by checking if the hitboxes intersect using the last tick's x positions, repeat for y collision using last tick's y positions
			//There's likely a more accurate method without detouring projectile npc collision? Not sure at the moment, working backwards from tile collision logic
			bool collideX = !lastTargetHitboxX.Intersects(lastProjHitboxX);
			bool collideY = !lastTargetHitboxY.Intersects(lastProjHitboxY);

			//Reverse velocity based on collision direction, 
			//add target velocity if it was in the opposite direction of projectile movement (ricocheting off of target), 
			//or if projectile velocity is slow enough (pushed by target)
			bool projStopped = Projectile.velocity.Length() < stoppedSpeed;
			if (collideX)
			{
				Projectile.velocity.X *= -1;

				if (projStopped)
					Projectile.velocity.X += target.velocity.X * highAddedVelMod;

				if (Math.Sign(target.velocity.X) == Math.Abs(Projectile.velocity.X))
					Projectile.velocity.X += target.velocity.X * lowAddedVelMod;
			}
			if (collideY)
			{
				Projectile.velocity.Y *= -1;

				if (projStopped)
					Projectile.velocity.Y += target.velocity.Y * highAddedVelMod;

				else if (Math.Sign(target.velocity.Y) == Math.Sign(Projectile.velocity.Y))
					Projectile.velocity.Y += target.velocity.Y * lowAddedVelMod;
			}
			
			Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.Pi / 8, MathHelper.Pi / 4) * (Main.rand.NextBool() ? -1 : 1));

			Projectile.netUpdate = true;
		}

		//Causes bubbles to ricochet off each other, and pushes them outside of each other if they overlap
		private void BubbleCollision(Projectile otherBubble)
		{
			Cannonbubble otherBubbleModProj = otherBubble.ModProjectile as Cannonbubble;
			while (otherBubble.Hitbox.Intersects(Projectile.Hitbox))
				Projectile.Center += Projectile.DirectionFrom(otherBubble.Center); //Push out if stuck inside

			//2 dimensional moving circle collision formula- via https://www.vobarian.com/collisions/
			//Simplified here: assuming all masses are equal, therefore normal scalar velocity post-collision is simply equal to the normal scalar velocity of the other vector pre-collision
			Vector2 normal = Projectile.DirectionFrom(otherBubble.Center);
			Vector2 tangent = normal.RotatedBy(MathHelper.PiOver2);
			float scalarTangentThis = Projectile.velocity.X * tangent.X + Projectile.velocity.Y * tangent.Y;
			float scalarTangentOther = otherBubble.velocity.X * tangent.X + otherBubble.velocity.Y * tangent.Y;
			float scalarNormalThis = Projectile.velocity.X * normal.X + Projectile.velocity.Y * normal.Y;
			float scalarNormalOther = otherBubble.velocity.X * normal.X + otherBubble.velocity.Y * normal.Y;

			Projectile.velocity = scalarTangentThis * tangent + scalarNormalOther * normal;
			otherBubble.velocity = scalarTangentOther * tangent + scalarNormalThis * normal;
			float speedMultiplier = Math.Max(GetSpeedRatio(2), otherBubbleModProj.GetSpeedRatio(2)); //Less dust at low collision speed
			if (speedMultiplier <= 0.1f)
				speedMultiplier = 0f;

			OnCollideExtra(0.5f * speedMultiplier, speedMultiplier);
			(otherBubble.ModProjectile as Cannonbubble).JiggleStrength = 1;
		}

		private void OnCollideExtra(float dustMult = 1f, float dustScaleMult = 1f)
		{
			JiggleStrength = 1;

			int dustCount = (int)(Main.rand.Next(6, 9) * dustMult);
			for (int i = 0; i < dustCount; i++)
			{
				int direction = Main.rand.NextBool() ? -1 : 1;
				Vector2 speed = Projectile.velocity.RotatedBy(MathHelper.PiOver2 * direction).RotatedByRandom(MathHelper.Pi / 15);
				int d = Dust.NewDust(Projectile.Center, 0, 0, DustID.BubbleBurst_Blue, speed.X * .25f, speed.Y * .25f, 0, default, Main.rand.NextFloat(1f, 1.25f) * dustScaleMult);
				Main.dust[d].noGravity = true;
			}

			if (!Main.dedServ)
				SoundEngine.PlaySound(SoundID.Item54.WithPitchVariance(0.3f).WithVolume(0.5f * dustMult), Projectile.Center);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Bounce(oldVelocity);
			Projectile.penetrate--;
			OnCollideExtra();
			return false;
		}
	}
}
