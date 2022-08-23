using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.AnimationTester
{
	internal class AnimationProjectile : ModProjectile
	{
		internal Animation anim;

		public override void SetDefaults()
		{
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(20);
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
		}

		public void SetAnimation(Animation a)
		{
			anim = a;

			Projectile.timeLeft = anim.timeLeft;
		}

		public override void AI()
		{
			Projectile.frameCounter++;
			if (Projectile.frameCounter > anim.FPS)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;

				if (Projectile.frame >= anim.maxFrames)
					Projectile.frame = 0;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = anim.Texture;
			Rectangle singleFrame = tex.Frame(1, anim.maxFrames, 0, 0, 0, 0);

			Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, singleFrame.Height * Projectile.frame, singleFrame.Width, singleFrame.Height), lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
			return false;
		}
	}
}
