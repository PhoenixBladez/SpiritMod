using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using SpiritMod.Prim;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Utilities;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechSword
{
	public class GranitechSaberProjectile : BaseHeldProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Technobrand");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override string Texture => "SpiritMod/Items/Sets/GranitechSet/GranitechSword/GranitechSaberItem";

		private int swingTime; //Total time for weapon to be used
		private Vector2 initialVelocity = Vector2.Zero; //Starting velocity, used for determining swing arc direction
		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(88, 92);
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

		private ref float SwingDirection => ref Projectile.ai[0];
		private ref float Timer => ref Projectile.ai[1];

		private int _hitTimer = 0;
		private const int MAX_HITTIMER = 10;

		public override bool AutoAimCursor() => false; //Only aim when first used

		public const float SwingRadians = MathHelper.Pi * 0.75f; //Total radians of the sword's arc
		public override bool PreAI()
		{
			Timer++;
			_hitTimer = Math.Max(_hitTimer - 1, 0);
			bool firstTick = Projectile.timeLeft > 2; //Set to 2 on first tick of normal ai
			if (firstTick) //Initialize total swing time and initial direction
			{
				initialVelocity = Projectile.velocity;
				swingTime = Owner.HeldItem.useTime;
				Projectile.netUpdate = true;
			}

			//Manually change direction if needed, due to no auto aiming
			if (Math.Abs(Projectile.velocity.X) > 0.15f) //Skip if absolute value is too low
			{
				int ownerDir = Owner.direction;
				Owner.ChangeDir(Projectile.velocity.X > 0 ? 1 : -1);
				if (ownerDir != Owner.direction && Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Owner.whoAmI);
			}
			

			//Get new velocity for swinging motion
			float progress = Timer / swingTime;
			progress = EaseFunction.EaseCircularInOut.Ease(progress);

			Projectile.velocity = initialVelocity.RotatedBy(MathHelper.Lerp(SwingRadians / 2 * SwingDirection, -SwingRadians/2 * SwingDirection, progress));
			return true;
		}

		public override void AbstractAI()
		{
			Projectile.rotation += MathHelper.PiOver4 * Owner.direction;
			if(SwingDirection == Owner.direction)
			{
				Projectile.rotation += MathHelper.PiOver2 * Owner.direction;
				Projectile.direction = Projectile.spriteDirection *= -1;
			}

			//Spawn particles
			if(!Main.dedServ && Projectile.oldPos[0] != Vector2.Zero)
			{
				int numParticles = Main.rand.Next(0, 3); //0-2
				for(int i = 0; i < numParticles; i++)
				{
					Vector2 position = Owner.MountedCenter + Projectile.velocity * Projectile.Size.Length() * Main.rand.NextFloat(0.33f, 1f);
					Vector2 velocity = Vector2.Normalize(Projectile.oldPos[0] - Projectile.position) * Main.rand.NextFloat(1.25f);
					ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247), Main.rand.NextFloat(1.5f, 2f), 25));
				}
			}

			//Spawn 2 holographic sword projectiles through swing time
			if(Timer % (swingTime / 2) == 0 && Owner == Main.LocalPlayer)
			{
				float distance = Main.rand.Next(130, 180); //Set distance from player 
				distance = MathHelper.Clamp(distance, 80, Owner.Distance(Main.MouseWorld));

				float swingRadians = Main.rand.NextFloat(MathHelper.Pi * 0.7f, MathHelper.Pi); //Set the radians of the sword's swing
				int direction = Main.rand.NextBool() ? -1 : 1; //Set swinging direction

				Vector2 position = Owner.MountedCenter + initialVelocity.RotatedBy(swingRadians / 2 * direction) * distance;
				Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), position, initialVelocity.RotatedBy(swingRadians / 2 * direction), ModContent.ProjectileType<GranitechSaber_Hologram>(), Projectile.damage, Projectile.knockBack, Projectile.owner, direction, 0);
				
				//Initialize variables for the projectile
				if(proj.ModProjectile is GranitechSaber_Hologram holoSword)
				{
					holoSword.InitialVelocity = initialVelocity;
					holoSword.BasePosition = Owner.MountedCenter;
					holoSword.SwingTime = (int)(swingTime * (1 + (distance / 250) + (swingRadians / SwingRadians))); //Slower than normal swing, based on distance and radians
					holoSword.SwingRadians = swingRadians;
					holoSword.Distance = distance;
				}

				//Sync modprojectile variable initializations for multiplayer
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			}

			Owner.reuseDelay = Owner.HeldItem.reuseDelay;

			if (Timer > swingTime)
				Projectile.Kill();
		}

		public override Vector2 HoldoutOffset() => Projectile.velocity * Projectile.Size.Length() / 2;

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Owner.MountedCenter, Owner.MountedCenter + Projectile.velocity * Projectile.Size.Length());

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffect(target.Center);

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffect(target.Center);

		public void HitEffect(Vector2 position)
		{
			if (Main.dedServ)
				return;

			Vector2 newPos = Vector2.Lerp(Projectile.Center, position, 0.5f);
			Vector2 direction = Owner.DirectionTo(newPos);
			if(_hitTimer == 0)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/EnergyImpact") with { PitchVariance = 0.1f, Volume = 0.4f }, Projectile.Center);

				_hitTimer = MAX_HITTIMER;
				ParticleHandler.SpawnParticle(new GranitechSaber_Hit(position, Main.rand.NextFloat(0.9f, 1.1f), direction.ToRotation()));
			}

			int numParticles = Main.rand.Next(6, 9);
			for (int i = 0; i < numParticles; i++)
			{
				Vector2 velocity = direction.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(3, 20);
				ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247), Main.rand.NextFloat(2.5f, 3f), 25));
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

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			float opacity = (float)Math.Pow(Math.Sin(Timer * MathHelper.Pi / swingTime), 1.5f) * 0.75f;
			Effect effect = Mod.GetEffect("Effects/GSaber");
			effect.Parameters["baseTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/GeometricTexture_2").Value);
			effect.Parameters["baseColor"].SetValue(new Color(25, 132, 247).ToVector4());
			effect.Parameters["overlayTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/GeometricTexture_1").Value);
			effect.Parameters["overlayColor"].SetValue(new Color(99, 255, 229).ToVector4());

			effect.Parameters["xMod"].SetValue(1.5f);
			effect.Parameters["yMod"].SetValue(2.5f);
			 
			float slashProgress = EaseFunction.EaseCircularInOut.Ease(Timer / swingTime);
			effect.Parameters["timer"].SetValue(-Main.GlobalTimeWrappedHourly * 6);
			effect.Parameters["progress"].SetValue(slashProgress);

			Vector2 pos = Owner.MountedCenter - Main.screenPosition;
			List<PrimitiveSlashArc> slashArcs = new List<PrimitiveSlashArc>();
			DrawAberration.DrawChromaticAberration(Vector2.UnitX, 2, delegate (Vector2 offset, Color colorMod)
			{
				PrimitiveSlashArc slash = new PrimitiveSlashArc
				{
					BasePosition = pos,
					StartDistance = offset.X + Projectile.Size.Length() * 0.33f,
					EndDistance = offset.X + Projectile.Size.Length(),
					AngleRange = new Vector2(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection),
					DirectionUnit = initialVelocity,
					Color = colorMod * opacity,
					SlashProgress = slashProgress
				};
				slashArcs.Add(slash);
			});
			PrimitiveRenderer.DrawPrimitiveShapeBatched(slashArcs.ToArray(), effect);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			return base.PreDraw(ref lightColor);
		}
		public override void PostDraw(Color lightColor)
		{
			Projectile.QuickDrawGlow(Main.spriteBatch);
			float opacity = (float)Math.Pow(Math.Sin(Timer * MathHelper.Pi / swingTime), 0.75f);

			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			DrawAberration.DrawChromaticAberration(Projectile.velocity, 2f, delegate (Vector2 offset, Color colorMod)
			{
				Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, colorMod * opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale, spriteEffects, 0);
			});
		}
	}
}