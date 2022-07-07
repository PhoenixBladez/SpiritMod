using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Prim;
using SpiritMod.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.ScreamingTome
{
	public class ScreamingSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screaming Skull");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 42;
			Projectile.aiStyle = 0;
			Projectile.scale = 1f;
			Projectile.tileCollide = false;
			Projectile.hide = false;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White;

		private bool primsCreated = false;
		private Vector2? mousePos = null;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.frameCounter++;

			/*if (Main.rand.Next(10) == 1)
				Dust.NewDustPerfect(projectile.Center + Main.rand.NextVector2Circular(24,24), ModContent.DustType<ScreamingDust>(), Vector2.Zero);*/

			if (mousePos == null) {
				if (Projectile.frameCounter % 16 == 0) {
					if ((Main.MouseWorld.X > Projectile.Center.X && Projectile.spriteDirection == 1) || (Main.MouseWorld.X <= Projectile.Center.X && Projectile.spriteDirection == -1)) {
						Projectile.frame++;
					}
				}
				if (Projectile.frame >= 4) {
					Projectile.spriteDirection = 0 - Math.Sign(Projectile.spriteDirection);
					Projectile.frame = 0;
				}
				float x = 0.05f;
				float y = 0.05f;

				Projectile.velocity += new Vector2((float)Math.Sign(Main.player[(int)Projectile.ai[0]].Center.X - Projectile.Center.X), (float)Math.Sign(Main.player[(int)Projectile.ai[0]].Center.Y - 50 - Projectile.Center.Y)) * new Vector2(x, y);
				if (Projectile.velocity.Length() > 4) {
					Projectile.velocity *= (2f / Projectile.velocity.Length());
				}
			}
			else {
				if (Projectile.frameCounter % 8 == 0 && Projectile.frame < 5) {
					Projectile.frame++;
				}
				if (Projectile.frame > 3) {
					Projectile.spriteDirection = Math.Sign(0 - Projectile.velocity.X);
				}
			}
			if (!primsCreated) {
				AdjustMagnitude(ref Projectile.velocity);
				primsCreated = true;
				SpiritMod.primitives.CreateTrail(new SkullPrimTrail(Projectile, Color.DarkGreen, 30));
			}
			float distance = 800f;

			if (distance > 800f) 
				Projectile.active = false;
			else 
				Projectile.active = true;

			if (!player.channel && mousePos == null) {
				Projectile.tileCollide = true;
				mousePos = Main.MouseWorld;
				Projectile.timeLeft = 75;
				Vector2 direction = Main.MouseWorld - Projectile.Center;
				direction.Normalize();
				direction *= 20f;
				Projectile.extraUpdates = 0;
				Projectile.velocity = direction;
				Projectile.frame = 4;
				Projectile.frameCounter = 1;
				Projectile.ai[1] = 1;

				if (Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/skullscrem") with { PitchVariance = 0.2f, Volume = 0.33f }, Projectile.Center);
			}
			else
				Lighting.AddLight(player.position, 0f, 0.5f, 0f);

			if (Main.player[(int)Projectile.ai[0]].active && !Main.player[(int)Projectile.ai[0]].dead)
				return;

			Projectile.Kill();
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => mousePos != null;

		public override bool PreDraw(ref Color lightColor)
		{
			/* Main.spriteBatch.End();
			 Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			 Color color = new Color(255, 25, 5);
			 Color color2 = new Color(0, 150, 55);
			 Starjinx.YuyutsuShader.Parameters["progress"].SetValue(counter);
			 Starjinx.YuyutsuShader.Parameters["color1"].SetValue(color.ToVector4());
			 Starjinx.YuyutsuShader.Parameters["color2"].SetValue(color2.ToVector4());
			 Starjinx.YuyutsuShader.Parameters["noise"].SetValue(ModContent.Request<Texture2D>("Textures/noise"));
			 Starjinx.YuyutsuShader.CurrentTechnique.Passes[0].Apply();*/
			Color bloomColor = Color.DarkGreen;
			bloomColor.A = 0;
			Main.spriteBatch.Draw(TextureAssets.Extra[49].Value, Projectile.Center - Main.screenPosition, null, bloomColor, 0f, new Vector2(50,50), 0.35f, SpriteEffects.None, 0f);

			Vector2 center = new Vector2((float)(TextureAssets.Projectile[Projectile.type].Value.Width / 2), (float)(TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type] / 2));
			SpriteEffects spriteEffects3 = (Projectile.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = texture.Height / Main.projFrames[Projectile.type];
			Rectangle frameRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, frameRect, lightColor, 0f, center, 1, spriteEffects3, 0f);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, Projectile.Center - Main.screenPosition, frameRect, Color.White, 0f, center, 1, spriteEffects3, 0f);

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
			SoundEngine.PlaySound(SoundID.NPCDeath3, Projectile.Center); 

			for (int i = 0; i <= 3; i++) {
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.position + new Vector2(Main.rand.Next(Projectile.width), Main.rand.Next(Projectile.height)),
					Main.rand.NextVector2Circular(-1, 1), Mod.Find<ModGore>("bonger" + Main.rand.Next(1, 5)).Type, Projectile.scale);
				gore.timeLeft = 20;
			}
		}
	}
}