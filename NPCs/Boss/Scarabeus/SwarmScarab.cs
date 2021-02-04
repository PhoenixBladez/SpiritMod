using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class SwarmScarab : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab");
			Main.projFrames[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 360;
		}

		public override void Kill(int timeLeft)
		{
			Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
			for (int i = 0; i < 10; i++) {
				int randomDustType = Main.rand.Next(3);
				if (randomDustType == 0)
					randomDustType = 5;
				else if (randomDustType == 1)
					randomDustType = 36;
				else
					randomDustType = 32;
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, randomDustType, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 1.2f;
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.Center.X, (int)projectile.Center.Y);
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.spriteDirection = -projectile.direction;
			projectile.localAI[1]++;
			if(projectile.velocity.Length() < 16 && projectile.localAI[1] >= 40) {
				projectile.velocity *= 1.03f;
			}
			projectile.frameCounter++;
			if(projectile.frameCounter > 6) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.frame = 0;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			SpriteEffects spriteeffects = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			float rotation = (projectile.spriteDirection > 0) ? projectile.rotation - MathHelper.Pi : projectile.rotation;
			Rectangle frame = new Rectangle(0, projectile.frame * texture2D.Height / Main.projFrames[projectile.type], texture2D.Width, texture2D.Height / Main.projFrames[projectile.type]);
			spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, frame, lightColor, rotation, frame.Size() / 2f, projectile.scale, spriteeffects, 0f);
				return false;
		}
	}
}