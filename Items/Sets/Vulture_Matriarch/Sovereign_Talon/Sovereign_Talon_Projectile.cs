using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
	public class Sovereign_Talon_Projectile : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Sovereign Talon");

		public override void SetDefaults()
		{
			Projectile.width = 44;
			Projectile.height = 42;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(Projectile, new SoverignTalonTrail(Projectile, new Color(255, 236, 115, 200)), new NoCap(), new DefaultTrailPosition(), 100f, 200f, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_1").Value, 0.05f, 1f, 1f));
			tM.CreateTrail(Projectile, new SoverignTalonTrail(Projectile, Color.White * 0.2f), new NoCap(), new DefaultTrailPosition(), 24f, 80f, new DefaultShader());
			tM.CreateTrail(Projectile, new SoverignTalonTrail(Projectile, Color.White * 0.2f), new NoCap(), new DefaultTrailPosition(), 24f, 80f, new DefaultShader());
			tM.CreateTrail(Projectile, new SoverignTalonTrail(Projectile, Color.Gold * 0.4f), new NoCap(), new DefaultTrailPosition(), 40f, 250f, new DefaultShader());
		}

		private ref float Timer => ref Projectile.localAI[0];
		private ref float RotationOffset => ref Projectile.localAI[1];
		private ref float Charge => ref Projectile.ai[0];

		public const int TimePerSwing = 25;
		public const int maxcharge = 5;

		public override void AI()
		{
			//lock the projectile's position to the player's center, and make the player "hold" the projectile
			Player projOwner = Main.player[Projectile.owner];
			Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
			Projectile.direction = projOwner.direction;
			projOwner.heldProj = Projectile.whoAmI;
			projOwner.itemTime = 2;
			projOwner.itemAnimation = 2;
			Lighting.AddLight(Projectile.Center, new Color(255, 236, 115).ToVector3() * (float)Math.Pow(Charge / maxcharge, 2));

			//reset the swing and increase charge, and update the direction
			if (++Timer % TimePerSwing == 0 && projOwner == Main.LocalPlayer)
			{
				if (!projOwner.channel || Charge >= maxcharge)
				{
					Projectile.Kill();
					return;
				}

				Projectile.velocity = projOwner.DirectionTo(Main.MouseWorld);
				Projectile.spriteDirection = (Main.rand.NextBool()) ? -1 : 1;
				Charge++;
				RotationOffset = -Projectile.spriteDirection * ((Charge < maxcharge) ? Main.rand.NextFloat(0.35f, 0.45f) * Charge : MathHelper.Pi);
				Projectile.netUpdate = true;

				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/SwordSlash1").WithPitchVariance(0.6f).WithVolume(0.8f), ownerMountedCenter);
				SoundEngine.PlaySound(projOwner.HeldItem.UseSound, ownerMountedCenter);
			}

			//move the projectile from the player's center to where it would be for a swing
			float Distance = 20 + (Math.Abs((float)Math.Sin((Timer / TimePerSwing) * MathHelper.Pi)) * 150 * ((Charge == maxcharge) ? 0.75f : 1));
			Projectile.Center = ownerMountedCenter;
			Projectile.position += Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.Lerp(RotationOffset, -RotationOffset, (Timer / TimePerSwing) % 1)) * Distance;

			//fire a wave halfway through the final swing
			if (Charge == maxcharge && (Timer % TimePerSwing) == TimePerSwing / 2)
			{
				projOwner.GetModPlayer<MyPlayer>().Shake += 5;
				var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 6, ModContent.ProjectileType<Talon_Projectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				proj.netUpdate = true;
				if (!Main.dedServ)
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.3f), Projectile.Center);
			}

			//set the owner's direction and item rotation, and the projectile's rotation
			projOwner.ChangeDir(Projectile.velocity.X > 0 ? 1 : -1);

			Projectile.rotation = Projectile.AngleFrom(ownerMountedCenter) + MathHelper.ToRadians(135f);
			if (Projectile.spriteDirection == -1)
				Projectile.rotation -= MathHelper.ToRadians(90f);

			projOwner.itemRotation = MathHelper.WrapAngle(projOwner.AngleTo(Projectile.Center) - ((projOwner.direction < 0) ? MathHelper.Pi : 0));
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Timer);
			writer.Write(RotationOffset);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Timer = reader.ReadSingle();
			RotationOffset = reader.ReadSingle();
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[Projectile.owner].Center, Projectile.Center) ? true : base.Colliding(projHitbox, targetHitbox);

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_glow");
			SpriteEffects effects = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 origin = Projectile.Size / 2;
			if (effects == SpriteEffects.FlipHorizontally)
				origin.X = tex.Width - Projectile.width / 2;

			void DrawGlow(Vector2 pos, float opacity, float scale = 1f, float? rot = null) => Main.spriteBatch.Draw(glowTex, pos - Main.screenPosition, null, Color.White * opacity * (float)Math.Pow(Charge / maxcharge, 2), rot ?? Projectile.rotation, origin, Projectile.scale * scale, effects, 0);

			float timer = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) / 2 + 0.5f;

			for (int i = 0; i < 6; i++)
			{
				Vector2 drawPos = Projectile.Center + Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * timer * 6;
				DrawGlow(drawPos, 1 - timer);
			}
			DrawGlow(Projectile.Center, timer / 2 + 0.5f, 1.1f);

			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale, effects, 0);
			return false;
		}
	}

	internal class SoverignTalonTrail : ITrailColor
	{
		private Color _colour;
		private Projectile _proj;

		public SoverignTalonTrail(Projectile projectile, Color colour)
		{
			_colour = colour;
			_proj = projectile;
		}

		public Color GetColourAt(float distanceFromStart, float trailLength, List<Vector2> points)
		{
			float progress = distanceFromStart / trailLength;
			return _colour * (1f - progress) * (float)Math.Pow(_proj.ai[0] / Sovereign_Talon_Projectile.maxcharge, 2);
		}
	}
}