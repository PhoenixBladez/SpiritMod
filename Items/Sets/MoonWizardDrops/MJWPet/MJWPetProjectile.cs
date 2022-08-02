using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MoonWizardDrops.MJWPet
{
	internal class MJWPetProjectile : ModProjectile
	{
		private Player Owner => Main.player[Projectile.owner];
		private ref float State => ref Projectile.ai[0];
		private ref float Timer => ref Projectile.ai[1];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly Lightbulb");
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 26;
			Projectile.height = 46;
			Projectile.tileCollide = false;

			AIType = 0;
		}

		public override void AI()
		{
			var modPlayer = Main.player[Projectile.owner].GetModPlayer<GlobalClasses.Players.PetPlayer>();

			if (Main.player[Projectile.owner].dead)
				modPlayer.mjwPet = false;

			if (modPlayer.mjwPet)
				Projectile.timeLeft = 2;

			Lighting.AddLight(Projectile.Center, new Vector3(0.025f, 0.05f, 2f) * 0.9f);
			Projectile.rotation = Projectile.velocity.X * 0.06f;

			if (State == 0)
				Follow();
			else if (State == 1)
				Teleport();

			if (Projectile.velocity.X > 0)
				Projectile.spriteDirection = -1;
			else if (Projectile.velocity.X < 0)
				Projectile.spriteDirection = 1;

			if (Projectile.frameCounter++ > 8)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;

				if (Projectile.frame >= Main.projFrames[Type])
					Projectile.frame = 0;
			}
		}

		private void Follow()
		{
			const float MaxSpeed = 7;

			Vector2 targetPosition = Owner.Center;

			if (Owner.controlUp)
				targetPosition.Y -= 200;

			if (Owner.controlDown)
				targetPosition.Y += 200;

			if (Owner.controlRight)
				targetPosition.X += 200;

			if (Owner.controlLeft)
				targetPosition.X -= 200;

			if (Projectile.DistanceSQ(targetPosition) > 140 * 140)
			{
				Projectile.velocity += Projectile.DirectionTo(targetPosition) * 0.1f;

				if (Projectile.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MaxSpeed;
			}
			else
				Projectile.velocity *= 0.98f;

			if (Projectile.DistanceSQ(Owner.Center) > 900 * 900)
				State = 1;
		}

		private void Teleport()
		{
			const float TeleportCutoff = 30f;

			Timer++;

			if (Timer < TeleportCutoff)
			{
				Projectile.alpha = (int)(255 * (Timer / TeleportCutoff));
				Projectile.velocity *= 0.9f;
			}
			else if (Timer >= TeleportCutoff)
			{
				Projectile.Center = Owner.Center + (Owner.velocity * 4) - new Vector2(0, 140);
				Projectile.alpha = 0;
				Projectile.velocity = Owner.velocity * 1.2f;

				for (int i = 0; i < 20; i++)
					Dust.NewDustPerfect(Projectile.Center, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-8, -4)));

				int count = Main.rand.Next(4, 9);
				for (int i = 0; i < count; i++)
				{
					int type = ModContent.ProjectileType<NPCs.Boss.MoonWizard.Projectiles.MoonBubble>();
					int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(Main.rand.NextFloat(2, 4f), 0).RotatedByRandom(MathHelper.TwoPi), type, NPCUtils.ToActualDamage(0), 0);
					Main.projectile[p].timeLeft = 60;
					Main.projectile[p].scale = Main.rand.NextFloat(0.4f, 0.8f);
				}

				Timer = 0;
				State = 0;
			}
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
			int frameHeight = tex.Height / Main.projFrames[Type];
			SpriteEffects effect = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 origin = Projectile.Size / 2f;
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);

			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, frame, Color.White * Projectile.Opacity, Projectile.rotation, origin, 1f, effect, 0); //Base glowmask

			float sine = (float)Math.Sin(Main.GameUpdateCount * 0.05f) * 0.2f;
			Vector2 pulseOffset = new Vector2(0, 14);
			float glowScale = 1.1f + sine;

			if (glowScale > 1f)
				Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition - pulseOffset, frame, Color.White * 0.33f * Projectile.Opacity, Projectile.rotation, origin - pulseOffset, glowScale, effect, 0);

			DrawPulses();
		}

		private void DrawPulses()
		{
			const float PulseScale = 0.25f;

			Texture2D tex = TextureAssets.GlowMask[239].Value;
			float timer = (float)((Main.GlobalTimeWrappedHourly * 0.5f) % 1.0);

			float scaleOne = timer;
			if (scaleOne > 0.5) scaleOne = 1f - timer;
			else if (scaleOne < 0.0) scaleOne = 0.0f;

			float timerOffset = (float)((timer + 0.5) % 1.0);

			float scaleTwo = timerOffset;
			if (scaleTwo > 0.5) scaleTwo = 1f - timerOffset;
			else if (scaleTwo < 0.0) scaleTwo = 0.0f;

			Rectangle source = tex.Frame(1, 1, 0, 0);
			Color drawCol = new Color(75, 231, 255) * 1.6f;
			Vector2 pos = Projectile.Center - Main.screenPosition + new Vector2(0, -14).RotatedBy(Projectile.rotation);
			Vector2 origin = tex.Size() / 2f;

			float scaleThree = 1f + timer * 0.75f;
			float scaleFour = 1f + timerOffset * 0.75f;

			Main.spriteBatch.Draw(tex, pos, source, drawCol * scaleOne * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale * PulseScale * scaleThree, SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(tex, pos, source, drawCol * scaleTwo * Projectile.Opacity, Projectile.rotation, origin, Projectile.scale * PulseScale * scaleFour, SpriteEffects.FlipHorizontally, 0.0f);
		}
	}
}
