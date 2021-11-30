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
using System.IO;
using System.Linq;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechSword
{
	public class GranitechSaberProjectile : BaseHeldProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Technobrand");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override string Texture => "SpiritMod/Items/Sets/GranitechSet/GranitechSword/GranitechSaberItem";

		private int swingTime; //Total time for weapon to be used
		private Vector2 initialVelocity = Vector2.Zero; //Starting velocity, used for determining swing arc direction
		public override void SetDefaults()
		{
			projectile.Size = new Vector2(88, 92);
			projectile.friendly = true;
			projectile.melee = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
			projectile.netUpdate = true;
			projectile.ownerHitCheck = true;
		}

		private ref float SwingDirection => ref projectile.ai[0];
		private ref float Timer => ref projectile.ai[1];

		private int _hitTimer = 0;
		private const int MAX_HITTIMER = 10;

		public override bool AutoAimCursor() => false; //Only aim when first used

		private const float SwingRadians = MathHelper.Pi * 0.75f; //Total radians of the sword's arc
		public override bool PreAI()
		{
			_hitTimer = Math.Max(_hitTimer - 1, 0);
			bool firstTick = projectile.timeLeft > 2; //Set to 2 on first tick of normal ai
			if (firstTick) //Initialize total swing time and initial direction
			{
				initialVelocity = projectile.velocity;
				swingTime = Owner.HeldItem.useTime;
				projectile.netUpdate = true;
			}

			//Manually change direction if needed, due to no auto aiming
			if (Math.Abs(projectile.velocity.X) > 0.15f) //Skip if absolute value is too low
			{
				int ownerDir = Owner.direction;
				Owner.ChangeDir(projectile.velocity.X > 0 ? 1 : -1);
				if (ownerDir != Owner.direction && Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Owner.whoAmI);
			}
			

			//Get new velocity for swinging motion
			float progress = Timer / swingTime;
			progress = EaseFunction.EaseCircularInOut.Ease(progress);

			projectile.velocity = initialVelocity.RotatedBy(MathHelper.Lerp(SwingRadians / 2 * SwingDirection, -SwingRadians/2 * SwingDirection, progress));
			return true;
		}

		public override void AbstractAI()
		{
			projectile.rotation += MathHelper.PiOver4 * Owner.direction;
			if(SwingDirection == Owner.direction)
			{
				projectile.rotation += MathHelper.PiOver2 * Owner.direction;
				projectile.direction = projectile.spriteDirection *= -1;
			}

			//Spawn particles
			if(!Main.dedServ && projectile.oldPos[0] != Vector2.Zero)
			{
				int numParticles = Main.rand.Next(0, 3); //0-2
				for(int i = 0; i < numParticles; i++)
				{
					Vector2 position = Owner.MountedCenter + projectile.velocity * projectile.Size.Length() * Main.rand.NextFloat(0.33f, 1f);
					Vector2 velocity = Vector2.Normalize(projectile.oldPos[0] - projectile.position) * Main.rand.NextFloat(1.25f);
					ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247), Main.rand.NextFloat(1.5f, 2f), 25));
				}
			}

			//Spawn 2 holographic sword projectiles through swing time
			if(Timer % ((swingTime / 2) + 1) == 0)
			{
				float distance = Main.rand.Next(80, 170); //Set distance from player 
				float swingRadians = Main.rand.NextFloat(MathHelper.Pi * 0.7f, MathHelper.Pi); //Set the radians of the sword's swing
				int direction = Main.rand.NextBool() ? -1 : 1; //Set swinging direction

				Vector2 position = Owner.MountedCenter + initialVelocity.RotatedBy(swingRadians / 2 * direction) * distance;
				Projectile proj = Projectile.NewProjectileDirect(position, initialVelocity.RotatedBy(swingRadians / 2 * direction), 
					ModContent.ProjectileType<GranitechSaber_Hologram>(), projectile.damage, projectile.knockBack, projectile.owner, direction);

				//Initialize variables for the projectile
				if(proj.modProjectile is GranitechSaber_Hologram holoSword)
				{
					holoSword.InitialVelocity = initialVelocity;
					holoSword.BasePosition = Owner.MountedCenter;
					holoSword.SwingTime = (int)(swingTime * (1 + (distance / 250) + (swingRadians / SwingRadians))); //Slower than normal swing, based on distance and radians
					holoSword.SwingRadians = swingRadians;
				}

				//Sync modprojectile variable initializations for multiplayer
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			}

			Owner.reuseDelay = Owner.HeldItem.reuseDelay;

			Timer++;
			if (Timer > swingTime)
				projectile.Kill();
		}

		public override Vector2 HoldoutOffset() => projectile.velocity * projectile.Size.Length() / 2;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Owner.MountedCenter, Owner.MountedCenter + projectile.velocity * projectile.Size.Length());

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffect(projectile.Center, projectile.velocity);

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffect(projectile.Center, projectile.velocity);

		private void HitEffect(Vector2 position, Vector2 direction)
		{
			if (Main.dedServ)
				return;

			if(_hitTimer == 0)
			{
				_hitTimer = MAX_HITTIMER;
				ParticleHandler.SpawnParticle(new GranitechSaber_Hit(position, Main.rand.NextFloat(0.9f, 1.1f), direction.ToRotation()));
			}

			int numParticles = Main.rand.Next(5, 8);
			for (int i = 0; i < numParticles; i++)
			{
				Vector2 velocity = direction.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(3, 20);
				ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247), Main.rand.NextFloat(2f, 2.5f), 25));
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(swingTime);
			writer.WriteVector2(initialVelocity);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			swingTime = reader.ReadInt32();
			initialVelocity = reader.ReadVector2();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			float opacity = (float)Math.Pow(Math.Sin(Timer * MathHelper.Pi / swingTime), 0.75f) * 0.75f;
			Effect effect = mod.GetEffect("Effects/GSaber");
			effect.Parameters["baseTexture"].SetValue(mod.GetTexture("Textures/GeometricTexture_2"));
			effect.Parameters["baseColor"].SetValue(new Color(25, 132, 247).ToVector4());
			effect.Parameters["overlayTexture"].SetValue(mod.GetTexture("Textures/GeometricTexture_1"));
			effect.Parameters["overlayColor"].SetValue(new Color(99, 255, 229).ToVector4());

			effect.Parameters["xMod"].SetValue(2); //scale with the total length of the strip
			effect.Parameters["yMod"].SetValue(2.5f);
			 
			float slashProgress = EaseFunction.EaseCircularInOut.Ease(Timer / swingTime);
			effect.Parameters["timer"].SetValue(Main.GlobalTime * 2);
			effect.Parameters["progress"].SetValue(slashProgress);

			Vector2 pos = Owner.MountedCenter - Main.screenPosition;

			for(int i = -1; i <= 1; i++)
			{
				Color aberrationColor = Color.White;
				switch (i)
				{
					case -1:
						aberrationColor = new Color(255, 0, 0, 0);
						break;
					case 0:
						aberrationColor = new Color(0, 255, 0, 0);
						break;
					case 1:
						aberrationColor = new Color(0, 0, 255, 0);
						break;
				}

				float offset = i * 2;
				PrimitiveSlashArc slash = new PrimitiveSlashArc
				{
					BasePosition = pos,
					StartDistance = offset + projectile.Size.Length() * 0.33f,
					EndDistance = offset + projectile.Size.Length(),
					AngleRange = new Vector2(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection),
					DirectionUnit = initialVelocity,
					Color = aberrationColor * opacity,
					SlashProgress = slashProgress
				};

				PrimitiveRenderer.DrawPrimitiveShape(slash, effect);
			}

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			return base.PreDraw(spriteBatch, lightColor);
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDrawGlow(spriteBatch);
			float opacity = (float)Math.Pow(Math.Sin(Timer * MathHelper.Pi / swingTime), 0.75f);

			Texture2D tex = ModContent.GetTexture(Texture + "_glow");
			SpriteEffects spriteEffects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			for(int i = -1; i < 1; i++)
			{
				Vector2 offset = projectile.velocity.RotatedBy(MathHelper.PiOver2) * i * 2f;
				Color aberrationColor = Color.White;
				switch (i) //Switch between making the color red, green, and blue
				{
					case -1:
						aberrationColor = new Color(255, 0, 0, 0);
						break;
					case 0:
						aberrationColor = new Color(0, 255, 0, 0);
						break;
					case 1:
						aberrationColor = new Color(0, 0, 255, 0);
						break;
				}
				spriteBatch.Draw(tex, projectile.Center + offset - Main.screenPosition, null, aberrationColor * opacity, projectile.rotation, tex.Size() / 2, projectile.scale, spriteEffects, 0);
			}
		}
	}
}