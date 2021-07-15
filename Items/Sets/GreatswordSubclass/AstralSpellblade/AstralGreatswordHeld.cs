using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria.ID;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
    public class AstralGreatswordHeld : BaseGreatsword, IDrawAdditive
    {
		public AstralGreatswordHeld() : base(1 / 60f, 20) { }

		public override string Texture => "SpiritMod/Items/Sets/GreatswordSubclass/AstralSpellblade/AstralGreatsword";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Spellblade");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 2;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		internal class StarSword 
		{
			private readonly AstralGreatswordHeld parent;
			private readonly Texture2D Texture = ModContent.GetTexture("SpiritMod/Items/Sets/GreatswordSubclass/AstralSpellblade/StarSword");
			private Vector2 Size => Texture.Size();
			private bool FullyCharged => parent.Charge == 1;
			private Vector2 Position => parent.projectile.Center + parent.projectile.DirectionFrom(parent.ProjOwner.MountedCenter) * (35 + ((float)Math.Sin(Main.GameUpdateCount / 15f) * 10));
			private float Rotation => parent.projectile.AngleFrom(parent.ProjOwner.MountedCenter) - MathHelper.PiOver2;

			private const float scale = 2f;

			public StarSword(AstralGreatswordHeld parent)
			{
				this.parent = parent;
			}

			public void Update()
			{
				if (parent.Charge == 0f)
				{
					Kill();
					return;
				}

				if (!FullyCharged)
				{

				}
			}

			public void Kill()
			{
				parent.starSword = null;
			}
			
			public void Draw(SpriteBatch spriteBatch)
			{
				spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
				SpiritMod.ShaderDict["StarSwordShader"].Parameters["colorMod"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 0.5f));
				SpiritMod.ShaderDict["StarSwordShader"].Parameters["timer"].SetValue(Main.GlobalTime / 3);
				SpiritMod.ShaderDict["StarSwordShader"].Parameters["vnoise"].SetValue(ModContent.GetTexture("SpiritMod/Textures/vnoise"));
				SpiritMod.ShaderDict["StarSwordShader"].CurrentTechnique.Passes["MainPS"].Apply();
				Rectangle drawRectangle = new Rectangle(0, 0, Texture.Width, (int)(Texture.Height * parent.Charge));
				Vector2 Origin = new Vector2(Texture.Width / 2, 0);

				Texture2D OutlineTex = ModContent.GetTexture("SpiritMod/Items/Sets/GreatswordSubclass/AstralSpellblade/StarSwordGlow");
				void DrawTex(Color color, Vector2 offset) => spriteBatch.Draw(OutlineTex, Position + offset - Main.screenPosition, drawRectangle, color * parent.Charge, Rotation, Origin, scale, SpriteEffects.None, 0);

				float Timer = (float)(Math.Sin(Main.GlobalTime * 4) / 2) + 0.5f;
				for (int i = 0; i < 6; i++)
					DrawTex(Color.White * (1 - Timer), Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 6) * 6 * Timer);
				for (int i = 0; i < 3; i++)
					DrawTex(Color.White * 0.5f, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 200) + (MathHelper.TwoPi * i / 3)) * 2.5f);

				spriteBatch.Draw(Texture, Position - Main.screenPosition, drawRectangle, Color.White * parent.Charge, Rotation, Origin, scale, SpriteEffects.None, 0);

				spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			}

			public void DrawAdditive(SpriteBatch spriteBatch)
			{
				Texture2D bloom = ModContent.GetTexture("SpiritMod/Effects/Masks/CircleGradient");
				Vector2 bloomscale = Texture.Size() / bloom.Size();
				bloomscale.Y *= parent.Charge;
				bloomscale.X *= 1.25f;
				bloomscale *= scale * 3f;
				Color color = Color.White * parent.Charge * 0.29f;
				spriteBatch.Draw(bloom, Position - Main.screenPosition, null, color, Rotation, new Vector2(bloom.Width / 2, bloom.Height / 3), bloomscale, SpriteEffects.None, 0);
			}
		}

		private StarSword starSword = null;

		public override bool CanDamage() => AiState != State_Charging;

		private float GlowOpacity = 0f;

		public const float Length = 90f;
		private const float ArcRadians = MathHelper.Pi * 0.4f;

		public override void SafeAI(Vector2 ownerPos)
		{
			if (starSword != null)
				starSword.Update();
		}

		public override void Charging(Vector2 ownerPos, bool SwingReady)
		{
			if (SwingReady && starSword == null && !Main.dedServ)
				starSword = new StarSword(this);

			if (Combo == 2)
				GlowOpacity = Math.Min(GlowOpacity + (0.25f / DrawBackTime), 0.25f);
		}

		public override Vector2 ChargeHoldout(Vector2 ownerPos, Vector2 ownerMouseDirection, ref bool AutoAimCursor)
		{
			Vector2 center = ownerPos;
			switch (Combo)
			{
				case 0:
					center += ownerMouseDirection.RotatedBy(-1 * MathHelper.Lerp(3 * ArcRadians / 4, ArcRadians, Math.Min(AiTimer / DrawBackTime, 1))) * Length;
					break;
				case 1:
					center += ownerMouseDirection.RotatedBy(MathHelper.Lerp(3 * ArcRadians / 4, ArcRadians, Math.Min(AiTimer / DrawBackTime, 1))) * Length;
					break;
				case 2:
					center += ownerMouseDirection * MathHelper.Lerp(Length, Length * 0.6f, Math.Min(AiTimer / DrawBackTime, 1));
					break;
			}

			return center;
		}

		public override void OnSwing(Vector2 ownerPos)
		{
			if (Combo == 2)
			{
				float length = (Charge == 1) ? 520 : 360;
				float width = (Charge == 1) ? 50 : 30;
				SpiritMod.primitives.CreateTrail(new ThrustVisual(ProjOwner, projectile.velocity, Color.White, width, length, 15));
				SpiritMod.primitives.CreateTrail(new ThrustVisual(ProjOwner, projectile.velocity, Color.White * 0.5f, width * 1.5f, length, 15));
				SpiritMod.primitives.CreateTrail(new ThrustVisual(ProjOwner, projectile.velocity, Color.White * 0.35f, width * 2, length, 15));
				for (int i = 0; i < 4; i++)
				{
					ParticleHandler.SpawnParticle(new ImpactLine(projectile.Center + (projectile.velocity * Length / 2),
						 projectile.velocity.RotatedBy((i % 2 == 0) ? (3 * MathHelper.PiOver4) : -(3 * MathHelper.PiOver4)).RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(8f, 10f),
						Color.White * 0.75f, Main.rand.NextFloat(0.6f, 0.8f) * new Vector2(1, 3), 14, projectile));
				}
			}
			else
			{
				SpiritMod.primitives.CreateTrail(new AstralSwordPrimTrail(projectile,
					ProjOwner,
					projectile.velocity.RotatedBy((Combo == 0 ? 1 : -1) * ArcRadians) * Length * 1.75f,
					projectile.velocity * Length * 3f,
					projectile.velocity.RotatedBy((Combo == 0 ? -1 : 1) * ArcRadians) * Length * 1.75f));
			}
		}

		public override void Swinging(Vector2 ownerPos)
		{
			float SwingProgress = (float)Math.Min(Math.Pow(AiTimer / 5f, 2.5f), Math.Pow(AiTimer / 10, 0.4f));
			switch (Combo)
			{
				case 0:
					projectile.Center = projectile.velocity.RotatedBy(-1 * (ArcRadians - (2 * ArcRadians * SwingProgress))) * Length;
					break;
				case 1:
					projectile.Center = projectile.velocity.RotatedBy(ArcRadians - (2 * ArcRadians * SwingProgress)) * Length;
					break;
				case 2:
					projectile.Center = projectile.velocity * MathHelper.Lerp(Length * 0.6f, Length, (float)Math.Pow(AiTimer / 10, 0.2f));
					break;
			}
			projectile.Center += ownerPos;

			if (!Main.dedServ && (Main.rand.NextBool(6) || (AiTimer % 2 == 0)))
			{
				Vector2 pos = ownerPos;
				Vector2 vel = Vector2.Zero;
				switch (Combo)
				{
					case 0:
						pos += projectile.velocity.RotatedBy(-1 * (ArcRadians - (2 * ArcRadians * SwingProgress))) * Length * Main.rand.NextFloat(1.75f, 2f);
						vel = -Vector2.Normalize(projectile.oldPos[1] - projectile.position).RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(1f, 2f);
						break;
					case 1:
						pos += projectile.velocity.RotatedBy(ArcRadians - (2 * ArcRadians * SwingProgress)) * Length * Main.rand.NextFloat(1.75f, 2f);
						vel = -Vector2.Normalize(projectile.oldPos[1] - projectile.position).RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(1f, 2f);
						break;
					case 2:
						pos += projectile.velocity * Length * MathHelper.Lerp(1f, 3f, SwingProgress);
						vel = projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(1f, 3f);

						if (Main.rand.NextBool())
							break;

						for (int i = 0; i < 2; i++)
						{
							ParticleHandler.SpawnParticle(new ImpactLine(ownerPos + (projectile.velocity * Length * 4),
								 projectile.velocity.RotatedBy((i % 2 == 0) ? (3.5f * MathHelper.PiOver4) : -(3.5f * MathHelper.PiOver4)).RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(12f, 15f),
								Color.White * 0.75f, Main.rand.NextFloat(0.6f, 0.8f) * new Vector2(1, 3), 18, projectile));
						}
						break;
				}
				ParticleHandler.SpawnParticle(new StarParticle(pos, vel, Color.White, SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.15f, 0.25f), 40));
			}

			if (AiTimer > 10f)
				projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			if (starSword != null)
				starSword.Kill();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D glow = ModContent.GetTexture(Texture + "_glow");
			Texture2D mask = ModContent.GetTexture(Texture + "_mask");
			SpriteEffects effects = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 origin = projectile.Size / 2;
			if (effects == SpriteEffects.None)
				origin.X = tex.Width - projectile.width / 2;

			float Timer = (float)(Math.Sin(Main.GlobalTime * 4) / 2) + 0.5f;

			
			void DrawTex(Texture2D texture, Color color, float scale = 1f, Vector2? offset = null) => spriteBatch.Draw(texture, projectile.Center + (offset ?? Vector2.Zero) - Main.screenPosition, null, color, projectile.rotation, origin, scale * projectile.scale, effects, 0);

			for (int i = 0; i < 6; i++)
				DrawTex(mask, Color.White * GlowOpacity * (1 - Timer), 1.15f, Vector2.UnitX.RotatedBy(MathHelper.TwoPi * i / 6) * 7 * Timer);
			for (int i = 0; i < 3; i++)
				DrawTex(mask, Color.White * GlowOpacity * ((1 - Timer) / 2 + 0.5f) * 0.5f, 1.15f, Vector2.UnitX.RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 200) + (MathHelper.TwoPi * i / 3)) * 3);

			DrawTex(mask, Color.White * GlowOpacity * (Timer / 2 + 0.5f), 1.15f);
			DrawTex(tex, lightColor);
			DrawTex(glow, Color.White);

			if (starSword != null)
				starSword.Draw(spriteBatch);
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (starSword != null)
				starSword.DrawAdditive(spriteBatch);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionpoint = 0;
			float CollisionLength = Length;
			float CollisionWidth = 70;
			CollisionLength *= ((Combo == 2) ? 3.25f : 2.5f);

			if (Charge >= 1)
			{
				CollisionLength *= 2;
				CollisionWidth *= 3;
			}

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), ProjOwner.Center,
					ProjOwner.Center + ProjOwner.DirectionTo(projectile.Center) * CollisionLength, CollisionWidth, ref collisionpoint) ? true : base.Colliding(projHitbox, targetHitbox);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection)
        {
			if (Combo == 2)
			{
				knockBack *= 2;
				damage = (int)(damage * 1.5f);
			}

			if(Charge == 1)
			{
				knockBack *= 2;
				damage = (int)(damage * 1.5f);
			}

             if (target.boss)
                damage = (int)(damage * 1.25f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.damage = (int)(projectile.damage * 0.85);
			if (Main.dedServ)
				return;

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starHit").WithPitchVariance(0.3f).WithVolume(0.6f), target.Center);
			Vector2 position = Main.rand.NextVector2CircularEdge(60, 60);
			Color color = Color.White;
			color.A = (byte)(color.A * 2);
			ParticleHandler.SpawnParticle(new ImpactLine(target.Center + position, -position / 4, color, Main.rand.NextFloat(0.6f, 0.8f) * new Vector2(1, 3), 10));
			for(int i = 0; i < 4; i++)
			{
				ParticleHandler.SpawnParticle(new StarParticle(
					target.Center + Main.rand.NextVector2Circular(6, 6),
					position.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.025f, 0.075f) * (Main.rand.NextBool() ? -1 : 1),
					color * 0.75f, SpiritMod.StarjinxColor(Main.GlobalTime) * 0.75f, Main.rand.NextFloat(0.3f, 0.6f), 20));
			}
		}
	}
}