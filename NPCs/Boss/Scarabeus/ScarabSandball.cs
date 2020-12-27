using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Scarabeus
{
	public class ScarabSandball : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Ball");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 360;
			projectile.scale = 0.75f;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Sandstorm, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true;
				Main.dust[d].scale = 2f;
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.Center.X, (int)projectile.Center.Y);
		}

		public override void AI()
		{
			projectile.rotation += 0.1f;
			if (Main.rand.Next(2) == 0) {
				int d = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, DustID.Sandstorm, projectile.oldVelocity.X * 0.2f, projectile.oldVelocity.Y * 0.2f);
				Main.dust[d].noGravity = true; 
				Main.dust[d].scale = 1.5f;
			}

			if(projectile.velocity.Y < 15f)
				projectile.velocity.Y += 0.2f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture2D = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, texture2D.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
				return false;
		}
	}
}