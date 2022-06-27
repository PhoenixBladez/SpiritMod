using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class MoonBubble : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
		}

		public override bool PreDraw(ref Color lightColor)
        {
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((Projectile.Center.X + 10) - Main.screenPosition.X) - (TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            int ypos = (int)((Projectile.Center.Y + 10) - Main.screenPosition.Y) - (TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), Projectile.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);
            return true;
        }
		public override Color? GetAlpha(Color lightColor) => Color.White;

		float alphaCounter;
		public override void AI()
        {
            alphaCounter += 0.04f;
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f, 0.231f, 0.255f);
			if (Projectile.timeLeft == 150) {
				Projectile.scale *= Main.rand.NextFloat(0.6f, 1.1f);
			}
			if (Projectile.ai[0] == 0) 
			{
				Projectile.velocity.X *= 0.98f;
				Projectile.velocity.Y -= 0.08f;
			}
			else
			{
				Projectile.velocity *= 0.98f;
				Projectile.timeLeft--;
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 5;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 54);
			for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonSpirit, 0f, -2f, 0, default, 2f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].scale *= .1825f;
				if (Main.dust[num].position != Projectile.Center)
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 2f;
			}
		}
	}
}
