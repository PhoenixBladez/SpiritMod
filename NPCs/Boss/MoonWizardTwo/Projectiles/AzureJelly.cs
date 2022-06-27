using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles
{
	public class AzureJelly : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Jelly");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = -1;
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
		}
		
		public override bool PreDraw(ref Color lightColor)
        {
			Color color = Color.Pink;
			color.A = 0;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            int xpos = (int)((Projectile.Center.X + 10) - Main.screenPosition.X) - (TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            int ypos = (int)((Projectile.Center.Y + 10) - Main.screenPosition.Y) - (TextureAssets.Projectile[Projectile.type].Value.Width / 2);
            Texture2D ripple = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
            Main.spriteBatch.Draw(ripple, new Vector2(xpos, ypos), new Microsoft.Xna.Framework.Rectangle?(), color, Projectile.rotation, ripple.Size() / 2f, .5f, spriteEffects, 0);
            return true;
        }

		public override Color? GetAlpha(Color lightColor) => Color.White;

		public override void AI()
        {
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0.075f, 0.231f, 0.255f);
			Player player = Main.player[(int)Projectile.ai[0]];

			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Vector2 direction = player.Center - Projectile.Center;
			float rotDifference = ((((direction.ToRotation() - Projectile.velocity.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;

			Projectile.velocity = Projectile.velocity.RotatedBy(Math.Sign(rotDifference) * 0.01f);
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
