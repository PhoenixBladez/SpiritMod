using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.ScreamingTome
{
	public class ScreamingSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screaming Skull");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 42;
			projectile.aiStyle = 0;
			projectile.scale = 1f;
			projectile.tileCollide = false;
			projectile.hide = false;
			projectile.extraUpdates = 1;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.magic = true;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		private bool primsCreated = false;
		private Vector2? mousePos = null;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.frameCounter++;

			if (mousePos == null) {
				if (projectile.frameCounter % 16 == 0) {
					if ((Main.MouseWorld.X > projectile.Center.X && projectile.spriteDirection == 1) || (Main.MouseWorld.X <= projectile.Center.X && projectile.spriteDirection == -1)) {
						projectile.frame++;
					}
				}
				if (projectile.frame >= 4) {
					projectile.spriteDirection = 0 - Math.Sign(projectile.spriteDirection);
					projectile.frame = 0;
				}
				float x = 0.05f;
				float y = 0.05f;

				projectile.velocity += new Vector2((float)Math.Sign(Main.player[(int)projectile.ai[0]].Center.X - projectile.Center.X), (float)Math.Sign(Main.player[(int)projectile.ai[0]].Center.Y - 50 - projectile.Center.Y)) * new Vector2(x, y);
				if (projectile.velocity.Length() > 4) {
					projectile.velocity *= (2f / projectile.velocity.Length());
				}
			}
			else {
				if (projectile.frameCounter % 8 == 0 && projectile.frame < 5) {
					projectile.frame++;
				}
				if (projectile.frame > 3) {
					projectile.spriteDirection = Math.Sign(0 - projectile.velocity.X);
				}
			}
			if (!primsCreated) {
				AdjustMagnitude(ref projectile.velocity);
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new SkullPrimTrail(projectile, Color.DarkGreen, 30));
			}
			float distance = 800f;

			if (distance > 800f) 
				projectile.active = false;
			else 
				projectile.active = true;

			if (!player.channel && mousePos == null) {
				projectile.tileCollide = true;
				mousePos = Main.MouseWorld;
				projectile.timeLeft = 75;
				Vector2 direction = Main.MouseWorld - projectile.Center;
				direction.Normalize();
				direction *= 20f;
				projectile.extraUpdates = 0;
				projectile.velocity = direction;
				projectile.frame = 4;
				projectile.frameCounter = 1;
				projectile.ai[1] = 1;

				if (Main.netMode != NetmodeID.Server)
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/skullscrem").WithPitchVariance(0.2f).WithVolume(0.33f), projectile.Center);
			}
			else
				Lighting.AddLight(player.position, 0f, 0.5f, 0f);

			if (Main.player[(int)projectile.ai[0]].active && !Main.player[(int)projectile.ai[0]].dead)
				return;

			projectile.Kill();
		}

		public override bool CanDamage() => mousePos != null;

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			/* Main.spriteBatch.End();
			 Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			 Color color = new Color(255, 25, 5);
			 Color color2 = new Color(0, 150, 55);
			 Starjinx.YuyutsuShader.Parameters["progress"].SetValue(counter);
			 Starjinx.YuyutsuShader.Parameters["color1"].SetValue(color.ToVector4());
			 Starjinx.YuyutsuShader.Parameters["color2"].SetValue(color2.ToVector4());
			 Starjinx.YuyutsuShader.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			 Starjinx.YuyutsuShader.CurrentTechnique.Passes[0].Apply();*/
			Color bloomColor = Color.DarkGreen;
			bloomColor.A = 0;
			Main.spriteBatch.Draw(ModContent.GetTexture("SpiritMod/Effects/Masks/Extra_49"), projectile.Center - Main.screenPosition, null, bloomColor, 0f, new Vector2(50,50), 0.35f, SpriteEffects.None, 0f);

			Vector2 center = new Vector2((float)(Main.projectileTexture[projectile.type].Width / 2), (float)(Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type] / 2));
			SpriteEffects spriteEffects3 = (projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frameRect = new Rectangle(0, projectile.frame * frameHeight, texture.Width, frameHeight);
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, frameRect, drawColor, 0f, center, 1, spriteEffects3, 0f);
			Main.spriteBatch.Draw(ModContent.GetTexture(Texture + "_glow"), projectile.Center - Main.screenPosition, frameRect, Color.White, 0f, center, 1, spriteEffects3, 0f);

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

		private void AdjustMagnitude(ref Vector2 vector)
		{
			if (vector.Length() > 6f) {
				vector *= 6f / vector.Length();
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.position.X, (int)projectile.position.Y, 3, 1f, 0f); 

			for (int i = 0; i <= 3; i++) {
				Gore gore = Gore.NewGoreDirect(projectile.position + new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height)),
					Main.rand.NextVector2Circular(-1, 1),
					mod.GetGoreSlot("Gores/Skelet/bonger" + Main.rand.Next(1, 5)),
					projectile.scale);
				gore.timeLeft = 20;
			}
		}
	}
}