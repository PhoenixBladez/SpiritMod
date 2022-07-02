using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Prim;

namespace SpiritMod.Mechanics.BoonSystem.NemesisBoon
{
	public class NemesisBoonSword : ModProjectile, IDrawAdditive
	{

		private const int SWINGDURATION = 30;

		private const float SWINGROTATION = 0.9f;

		private const int SWINGDISTANCE = 110;

		private const int NUMBEROFSWINGS = 3;

		private bool activated => Projectile.ai[1] == 1;

		private NPC parent => Main.npc[(int)Projectile.ai[0]];
		private List<float> oldRotation = new List<float>();
		private Player player;

		private float hoverCounter;
		private bool initialized = false;
		private bool swinging = false;
		private float swingWindup = 0;
		private float slashProgress = 0;
		private float soundTimer = 0;
		private float swingTimer = 0;
		private float swingSpeed = (SWINGROTATION * 2) / SWINGDURATION;

		private Vector2 swingDirection = Vector2.Zero;
		private Vector2 swingBase = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Nemesis");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 26;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			if (!initialized)
			{
				for (int i = 0; i < Projectile.oldPos.Length; i++)
					oldRotation.Add(Projectile.rotation);
			}

			oldRotation.Add(Projectile.rotation);
			while (oldRotation.Count > Projectile.oldPos.Length)
				oldRotation.RemoveAt(0);

			if (activated)
			{
				if (!swinging)
				{
					swingWindup++;
					Projectile.timeLeft = (int)(SWINGDURATION * NUMBEROFSWINGS / 2f) + 5;
					float newRot = swingDirection.ToRotation() + SWINGROTATION + 1.57f;

					float rotDif = ((((newRot - Projectile.rotation) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
					if (swingWindup > 60)
					{
						soundTimer = 10;
						swinging = true;
					}

					if (Math.Abs(rotDif) > 0.1f)
						Projectile.rotation += rotDif / 15f;
					Projectile.rotation = MathHelper.Lerp(Projectile.rotation, newRot, 0.1f);

					float velLength = (player.Center - swingBase).Length();
					float offset = SWINGDISTANCE * Math.Min(swingWindup / 30f, 1);
					offset *= 2;
					Projectile.velocity = swingDirection * (float)Math.Pow(Math.Abs(velLength - offset), 0.3f) * Math.Sign(velLength - offset);

					if (swingWindup == 30)
						SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
				}
				else
				{
					Projectile.hostile = true;

					swingTimer += swingSpeed;
					if (swingTimer > 1 || swingTimer < 0)
					{
						soundTimer = 10;
						swingSpeed *= -1;
					}

					soundTimer--;
					if (soundTimer == 0)
						SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);

					float progress = swingTimer;
					float oldProgress = EaseFunction.EaseCircularInOut.Ease(progress - swingSpeed);
					slashProgress = progress = EaseFunction.EaseCircularInOut.Ease(progress);

					Projectile.velocity = swingDirection * 50 * (Math.Abs(oldProgress - progress));

					Projectile.rotation = swingDirection.ToRotation() + MathHelper.Lerp(SWINGROTATION, -SWINGROTATION, progress) + 1.57f;
				}

				Projectile.Center = swingBase + ((Projectile.rotation - 1.57f).ToRotationVector2() * SWINGDISTANCE * Math.Min(swingWindup / 30f, 1));

				swingBase += Projectile.velocity;
			}
			else
			{
				swingBase = Projectile.Center;
				player = Main.player[Player.FindClosest(swingBase, 0, 0)];
				swingDirection = Projectile.DirectionTo(player.Center);
				Projectile.velocity = Vector2.Zero;
				hoverCounter += 0.05f;

				Vector2 posToBe = parent.Center + new Vector2(parent.direction * (parent.width + (Projectile.width / 2)) * -1, (float)Math.Sin(hoverCounter) * 12).RotatedBy(parent.rotation);
				if (!parent.active)
					Projectile.active = false;

				Vector2 newPos = Vector2.Lerp(Projectile.Center, posToBe, 0.1f);
				Vector2 tiltDirection = Projectile.DirectionTo(newPos);
				Projectile.rotation = tiltDirection.X * ((Projectile.Center - newPos).Length() / 10f);
				Projectile.Center = newPos;

			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float collisionPoint = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + ((Projectile.rotation - 1.57f).ToRotationVector2() * 70), Projectile.width, ref collisionPoint);
		}

		public override void Kill(int timeLeft)
		{
			Vector2 vector9 = Projectile.position;
			for (int num257 = 0; num257 < 25; num257++)
			{
				int newDust = Dust.NewDust(Projectile.Center + ((Projectile.rotation - 1.57f).ToRotationVector2() * Main.rand.Next(70)) + Main.rand.NextVector2Circular(10, 10), Projectile.width, Projectile.height, DustID.Firework_Blue, 0f, 0f, 0, default, 1f);
				Main.dust[newDust].velocity *= .125f;
				Main.dust[newDust].noGravity = true;

			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

			if (swinging)
			{
				Main.spriteBatch.End(); 
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				List<PrimitiveSlashArc> slashArcs = new List<PrimitiveSlashArc>();
				Effect effect = ModContent.Request<Effect>("Effects/NemesisBoonShader", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				effect.Parameters["white"].SetValue(Color.White.ToVector4());
				effect.Parameters["opacity"].SetValue(1);
				PrimitiveSlashArc slash = new PrimitiveSlashArc
				{
					BasePosition = swingBase - Main.screenPosition,
					StartDistance = SWINGDISTANCE,
					EndDistance = SWINGDISTANCE + tex.Height,
					AngleRange = new Vector2(SWINGROTATION * Math.Sign(swingSpeed), -SWINGROTATION * Math.Sign(swingSpeed)),
					DirectionUnit = swingDirection,
					Color = Color.Cyan * Projectile.velocity.Length() * 0.2f,
					SlashProgress = Math.Sign(swingSpeed) == 1 ? slashProgress : 1 - slashProgress
				};
				slashArcs.Add(slash);
				PrimitiveRenderer.DrawPrimitiveShapeBatched(slashArcs.ToArray(), effect);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			}

			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

			Texture2D tex2 = ModContent.Request<Texture2D>(Texture + "_White", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			DrawSword(spriteBatch, tex, 1, 1);

			if (swingWindup < 60 && swingWindup > 30)
			{
				float progress = (swingWindup - 30) / 30f;
				float transparency = (float)Math.Pow(1 - progress, 2);
				float scale = 1 + progress;
				DrawSword(spriteBatch, tex2, transparency, scale);
			}
		}

		private void DrawSword(SpriteBatch spriteBatch, Texture2D tex, float transparency, float scale)
		{
			Vector2 origin = new Vector2(tex.Width / 2, tex.Height);

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * 0.8f * transparency, Projectile.rotation, origin, Projectile.scale * scale, SpriteEffects.None, 0f);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (new Vector2(Projectile.width, Projectile.height) / 2);
				Color color = Color.White * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
				float num108 = 4;
				float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				float num106 = 0f;
				Color color29 = new Color(110 - Projectile.alpha, 94 - Projectile.alpha, 25 - Projectile.alpha, 0).MultiplyRGBA(color);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = Projectile.GetAlpha(color28);
					color28 *= 1.5f - num107;
					color28 *= (float)Math.Pow((((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) / 2), 1.5f);
					Vector2 vector29 = drawPos + ((float)num103 / (float)num108 * 6.28318548f + Projectile.rotation + num106).ToRotationVector2() * (1.5f * num107 + 3f) - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) - Projectile.velocity * (float)num103;
					spriteBatch.Draw(tex, vector29, null, color28 * .6f * transparency, oldRotation[k], origin, Projectile.scale * scale, SpriteEffects.None, 0f);
				}
			}
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}
