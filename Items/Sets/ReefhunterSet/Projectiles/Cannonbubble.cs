using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = 5;
			projectile.timeLeft = 360;
			projectile.aiStyle = 0;
			projectile.scale = Main.rand.NextFloat(0.85f, 1.15f);
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
		}

		private ref float JiggleStrength => ref projectile.ai[0];
		private ref float JiggleTime => ref projectile.ai[1];

		public override void AI()
		{
			projectile.velocity *= 0.975f;
			projectile.rotation = projectile.velocity.ToRotation();

			const float JiggleDecay = 0.99f; //Exponentially slows down in speed
			JiggleStrength *= JiggleDecay;
			JiggleTime += JiggleStrength;

			const float heightDeviation = 0.25f;
			const float sinePeriod = 100; //1.66 seconds
			projectile.position.Y += (float)Math.Sin(MathHelper.TwoPi * projectile.timeLeft / sinePeriod) * heightDeviation;

			if (projectile.wet)
				projectile.velocity.Y -= 0.08f;

			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.type == projectile.type && p.active && p != null && p.whoAmI != projectile.whoAmI && p.Hitbox.Intersects(projectile.Hitbox))
					BubbleCollision(p);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + (projectile.Size/2) + new Vector2(0f, projectile.gfxOffY);
				float progress = ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				float speedMult = GetSpeedRatio(3); //Trail only appears when at high speed
				Color color = projectile.GetAlpha(lightColor) * progress * speedMult * 0.5f;
				DrawBubble(spriteBatch, drawPos, color);
			}

			DrawBubble(spriteBatch, projectile.Center - Main.screenPosition, projectile.GetAlpha(lightColor));
			return false;
		}

		//Draw a bubble, with a rotated and squished outline and interior, a semi-transparent interior, and an unchanging shine
		private void DrawBubble(SpriteBatch spriteBatch, Vector2 position, Color baseColor)
		{
			Texture2D outline = ModContent.GetTexture(TexturePath + "_Outline");
			Texture2D inner = ModContent.GetTexture(TexturePath + "_Inner");
			Texture2D shine = ModContent.GetTexture(TexturePath + "_Shine");

			Vector2 squishScale = BubbleSquishScale(0.35f, 0.1f);
			Color innerColor = baseColor * 0.05f;
			innerColor.A = 0;

			spriteBatch.Draw(outline, position, null, baseColor, projectile.rotation, projectile.DrawFrame().Size() / 2, squishScale, SpriteEffects.None, 0);
			spriteBatch.Draw(inner, position, null, innerColor, projectile.rotation, projectile.DrawFrame().Size() / 2, squishScale, SpriteEffects.None, 0);
			spriteBatch.Draw(shine, position, null, baseColor, 0, projectile.DrawFrame().Size() / 2, projectile.scale, SpriteEffects.None, 0);
		}

		//Find the scale vector by which to draw the bubble's outline and interior with
		private Vector2 BubbleSquishScale(float velDelta, float jiggleDelta)
		{
			float squishAmount = GetSpeedRatio() * velDelta; //velocity based
			const float sineSpeed = 5 * MathHelper.Pi;
			squishAmount += (float)Math.Sin(sineSpeed * JiggleTime / 60) * jiggleDelta * JiggleStrength; //jiggling based
			return new Vector2(1 + squishAmount, 1 - squishAmount) * projectile.scale;
		}

		private float GetSpeedRatio(float exponent = 1) => (float)Math.Pow(MathHelper.Min(projectile.velocity.Length() / MAX_SPEED, 1), exponent);

		public override void Kill(int timeLeft)
		{
			int dustCount = Main.rand.Next(7, 12);
			Main.PlaySound(SoundID.Item54, projectile.Center);

			for (int i = 0; i < dustCount; ++i)
			{
				Vector2 speed = Main.rand.NextVec2CircularEven(0.5f, 0.5f);
				Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_Blue, speed.X * .5f, speed.Y * .5f, 0, default, Main.rand.NextFloat(0.5f, 1f));

				if(Main.rand.NextBool(3))
				{
					int d = Dust.NewDust(projectile.Center + new Vector2(Main.rand.Next(-20, 20), 0), 0, 0, ModContent.DustType<Dusts.BubbleDust>(), speed.X * .35f, Main.rand.NextFloat(-3f, -.5f), 0, default, Main.rand.NextFloat(0.75f, 1.5f));
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
			Rectangle lastProjHitboxX = new Rectangle((int)(projectile.position.X - projectile.velocity.X), (int)(projectile.position.Y), projectile.width, projectile.height);
			Rectangle lastProjHitboxY = new Rectangle((int)(projectile.position.X), (int)(projectile.position.Y - projectile.velocity.Y), projectile.width, projectile.height);

			//Check for x collision by checking if the hitboxes intersect using the last tick's x positions, repeat for y collision using last tick's y positions
			//There's likely a more accurate method without detouring projectile npc collision? Not sure at the moment, working backwards from tile collision logic
			bool collideX = !lastTargetHitboxX.Intersects(lastProjHitboxX);
			bool collideY = !lastTargetHitboxY.Intersects(lastProjHitboxY);

			//Reverse velocity based on collision direction, 
			//add target velocity if it was in the opposite direction of projectile movement (ricocheting off of target), 
			//or if projectile velocity is slow enough (pushed by target)
			bool projStopped = projectile.velocity.Length() < stoppedSpeed;
			if (collideX)
			{
				projectile.velocity.X *= -1;

				if (projStopped)
					projectile.velocity.X += target.velocity.X * highAddedVelMod;

				if (Math.Sign(target.velocity.X) == Math.Abs(projectile.velocity.X))
					projectile.velocity.X += target.velocity.X * lowAddedVelMod;
			}
			if (collideY)
			{
				projectile.velocity.Y *= -1;

				if (projStopped)
					projectile.velocity.Y += target.velocity.Y * highAddedVelMod;

				else if (Math.Sign(target.velocity.Y) == Math.Sign(projectile.velocity.Y))
					projectile.velocity.Y += target.velocity.Y * lowAddedVelMod;
			}
			
			projectile.velocity = projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.Pi / 8, MathHelper.Pi / 4) * (Main.rand.NextBool() ? -1 : 1));

			projectile.netUpdate = true;
		}

		//Causes bubbles to ricochet off each other, and pushes them outside of each other if they overlap
		private void BubbleCollision(Projectile otherBubble)
		{
			Cannonbubble otherBubbleModProj = otherBubble.modProjectile as Cannonbubble;
			while (otherBubble.Hitbox.Intersects(projectile.Hitbox))
				projectile.Center += projectile.DirectionFrom(otherBubble.Center); //Push out if stuck inside

			//2 dimensional moving circle collision formula- via https://www.vobarian.com/collisions/
			//Simplified here: assuming all masses are equal, therefore normal scalar velocity post-collision is simply equal to the normal scalar velocity of the other vector pre-collision
			Vector2 normal = projectile.DirectionFrom(otherBubble.Center);
			Vector2 tangent = normal.RotatedBy(MathHelper.PiOver2);
			float scalarTangentThis = projectile.velocity.X * tangent.X + projectile.velocity.Y * tangent.Y;
			float scalarTangentOther = otherBubble.velocity.X * tangent.X + otherBubble.velocity.Y * tangent.Y;
			float scalarNormalThis = projectile.velocity.X * normal.X + projectile.velocity.Y * normal.Y;
			float scalarNormalOther = otherBubble.velocity.X * normal.X + otherBubble.velocity.Y * normal.Y;

			projectile.velocity = scalarTangentThis * tangent + scalarNormalOther * normal;
			otherBubble.velocity = scalarTangentOther * tangent + scalarNormalThis * normal;
			float speedMultiplier = Math.Max(GetSpeedRatio(2), otherBubbleModProj.GetSpeedRatio(2)); //Less dust at low collision speed
			if (speedMultiplier <= 0.1f)
				speedMultiplier = 0f;

			OnCollideExtra(0.5f * speedMultiplier, speedMultiplier);
			(otherBubble.modProjectile as Cannonbubble).JiggleStrength = 1;
		}

		private void OnCollideExtra(float dustMult = 1f, float dustScaleMult = 1f)
		{
			JiggleStrength = 1;

			int dustCount = (int)(Main.rand.Next(6, 9) * dustMult);
			for (int i = 0; i < dustCount; i++)
			{
				int direction = Main.rand.NextBool() ? -1 : 1;
				Vector2 speed = projectile.velocity.RotatedBy(MathHelper.PiOver2 * direction).RotatedByRandom(MathHelper.Pi / 15);
				int d = Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_Blue, speed.X * .25f, speed.Y * .25f, 0, default, Main.rand.NextFloat(1f, 1.25f) * dustScaleMult);
				Main.dust[d].noGravity = true;
			}

			if (!Main.dedServ)
				Main.PlaySound(SoundID.Item54.WithPitchVariance(0.3f).WithVolume(0.5f * dustMult), projectile.Center);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Bounce(oldVelocity);
			projectile.penetrate--;
			OnCollideExtra();
			return false;
		}
	}
}
