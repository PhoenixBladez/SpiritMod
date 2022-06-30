using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Projectiles.BaseProj;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
	public class GranitechSaber_Hologram : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Technobrand");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}


		public int SwingTime; //Total time for weapon to be used
		public Vector2 InitialVelocity = Vector2.Zero; //Starting velocity, used for determining swing arc direction
		public Vector2 BasePosition = Vector2.Zero;
		public float SwingRadians;
		public float Distance;

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

		public override void AI()
		{

			++Timer;
			if (Timer == 3)
            {
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/EnergySwordSlash").WithPitchVariance(0.4f).WithVolume(0.8f), Projectile.Center);
			}
			Projectile.timeLeft = 2;

			_hitTimer = Math.Max(_hitTimer - 1, 0);
			float progress = Timer / SwingTime;
			progress = EaseFunction.EaseCircularInOut.Ease(progress);
			Projectile.velocity = InitialVelocity.RotatedBy(MathHelper.Lerp(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection, progress));

			Projectile.Center = BasePosition + Projectile.velocity * Distance;

			Projectile.alpha = (int)MathHelper.Lerp(0, 200, 1 - (float)Math.Sin((Timer / SwingTime) * MathHelper.Pi));

			Projectile.direction = Projectile.spriteDirection = (Projectile.Center.X < BasePosition.X) ? -1 : 1;

			Projectile.rotation = Projectile.velocity.ToRotation() - (Projectile.spriteDirection < 0 ? MathHelper.Pi : 0);
			Projectile.rotation += MathHelper.PiOver4 * Projectile.direction;
			if (SwingDirection == Projectile.direction)
			{
				Projectile.rotation += MathHelper.PiOver2 * Projectile.direction;
				Projectile.direction = Projectile.spriteDirection *= -1;
			}

			/*if (!Main.dedServ && projectile.oldPos[0] != Vector2.Zero)
			{
				int numParticles = Main.rand.Next(0, 2); //0-1
				for (int i = 0; i < numParticles; i++)
				{
					Vector2 position = BasePosition + projectile.velocity * (distance + Main.rand.NextFloat(-projectile.Size.Length() / 2, projectile.Size.Length()/2));
					Vector2 velocity = Vector2.Normalize(projectile.oldPos[0] - projectile.position) * Main.rand.NextFloat(1.25f);
					ParticleHandler.SpawnParticle(new GranitechParticle(position, velocity, (Main.rand.NextBool() ? new Color(99, 255, 229) : new Color(25, 132, 247)) * projectile.Opacity, Main.rand.NextFloat(), 20));
				}
			}*/

			if (Timer > SwingTime)
				Projectile.Kill();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 halfLine = Projectile.velocity * Projectile.Size.Length() / 2;
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => HitEffect(target.Center);

		public override void OnHitPvp(Player target, int damage, bool crit) => HitEffect(target.Center);

		public void HitEffect(Vector2 position)
		{
			SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/GranitechImpact").WithPitchVariance(0.4f).WithVolume(0.4f), Projectile.Center);

			Projectile.damage = (int)(Projectile.damage * 0.75f);
			if (Main.dedServ)
				return;

			Vector2 newPos = Vector2.Lerp(Projectile.Center, position, 0.5f);
			Vector2 direction = Vector2.Normalize(newPos - BasePosition);
			if (_hitTimer == 0)
			{
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/EnergyImpact").WithPitchVariance(0.1f).WithVolume(0.4f), Projectile.Center);

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
			writer.Write(SwingTime);
			writer.WriteVector2(InitialVelocity);
			writer.WriteVector2(BasePosition);
			writer.Write(SwingRadians);
			writer.Write(Distance);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SwingTime = reader.ReadInt32();
			InitialVelocity = reader.ReadVector2();
			BasePosition = reader.ReadVector2();
			SwingRadians = reader.Read();
			Distance = reader.ReadSingle();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft > 2) //bandaid fix for flickering
				return false;

			float opacity = Projectile.Opacity;
			float xMod = (1 + (Distance / 250) + (SwingRadians / GranitechSaberProjectile.SwingRadians));
			Effect effect = Mod.GetEffect("Effects/GSaber");
			effect.Parameters["baseTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/GeometricTexture_2").Value);
			effect.Parameters["baseColor"].SetValue(new Color(25, 132, 247).ToVector4());
			effect.Parameters["overlayTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/GeometricTexture_1").Value);
			effect.Parameters["overlayColor"].SetValue(new Color(99, 255, 229).ToVector4());

			effect.Parameters["xMod"].SetValue(1.5f * xMod); //scale with the total length of the strip
			effect.Parameters["yMod"].SetValue(2.5f);

			float slashProgress = EaseFunction.EaseCircularInOut.Ease(Timer / SwingTime);
			effect.Parameters["timer"].SetValue(-Main.GlobalTimeWrappedHourly * 6);
			effect.Parameters["progress"].SetValue(slashProgress);

			Vector2 pos = BasePosition - Main.screenPosition;

			List<PrimitiveSlashArc> slashArcs = new List<PrimitiveSlashArc>();
			DrawAberration.DrawChromaticAberration(Vector2.UnitX, 4, delegate (Vector2 offset, Color colorMod)
			{
				PrimitiveSlashArc slash = new PrimitiveSlashArc
				{
					BasePosition = pos,
					StartDistance = offset.X + Distance - Projectile.Size.Length() / 2 * SwingDirection,
					EndDistance = offset.X + Distance + Projectile.Size.Length() / 2 * SwingDirection,
					AngleRange = new Vector2(SwingRadians / 2 * SwingDirection, -SwingRadians / 2 * SwingDirection),
					DirectionUnit = InitialVelocity,
					Color = colorMod * opacity * 0.5f,
					SlashProgress = slashProgress
				};
				slashArcs.Add(slash);
			});
			PrimitiveRenderer.DrawPrimitiveShapeBatched(slashArcs.ToArray(), effect);

			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			DrawAberration.DrawChromaticAberration(Projectile.velocity, 3, delegate (Vector2 offset, Color colorMod)
			{
				Main.spriteBatch.Draw(tex, Projectile.Center + offset - Main.screenPosition, null, colorMod * opacity, Projectile.rotation, tex.Size() / 2, Projectile.scale, spriteEffects, 0);
			});

			return false;
		}
	}
}