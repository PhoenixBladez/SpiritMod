using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Hostile
{
	public class Comet : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 72;
			projectile.penetrate = 1;
			projectile.timeLeft = 6000;
			projectile.height = 72;
            projectile.tileCollide = true;
		}
        public void AdditiveCall(SpriteBatch spriteBatch)
        {
			for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = new Color(56, 199, 255) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = projectile.scale * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length); 
                Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Hostile/Comet");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale * .85f, default, default);
               
            }
        }
        public override void AI()
        {
			if (projectile.soundDelay <= 0) {
				projectile.soundDelay = 10;
				projectile.soundDelay *= 9;
				if (Main.rand.NextBool(3))
					Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 9);
			}
			Lighting.AddLight(projectile.position, 0.069f, .227f, .255f);
            projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            if (projectile.timeLeft > 5960)
            {
                projectile.velocity.Y -= .1f;
            }
            else
            {
                projectile.velocity.Y += 0.1F;                
            }
        }
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.NPCHit, projectile.Center, 3);
            if (Main.rand.NextBool(4))
            {
                Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, ItemID.FallenStar, 1, false, 0, false, false);
            }
            for (int i = 0; i < 20; i++) {
				int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Electric, 0f, -2f, 0, default, .9f);
				Main.dust[num].noGravity = true;
				Dust expr_62_cp_0 = Main.dust[num];
				expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				Dust expr_92_cp_0 = Main.dust[num];
				expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-30, 31) / 20) - 1.5f);
				if (Main.dust[num].position != projectile.Center) {
		    		Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 6f;
			    }
            }
            for (int num625 = 0; num625 < 3; num625++) {
				float scaleFactor10 = 0.33f;
				if (num625 == 1) {
					scaleFactor10 = 0.66f;
				}
				if (num625 == 2) {
					scaleFactor10 = 1f;
				}

				int num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13AB6_cp_0 = Main.gore[num626];
				expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
				Gore expr_13AD6_cp_0 = Main.gore[num626];
				expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
				num626 = Gore.NewGore(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
				Main.gore[num626].velocity *= scaleFactor10;
				Gore expr_13B79_cp_0 = Main.gore[num626];
				expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
				Gore expr_13B99_cp_0 = Main.gore[num626];
				expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
            }
        }
	}
}